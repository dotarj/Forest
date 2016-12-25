// Copyright (c) Arjen Post. See License.txt in the project root for license information.

using System;
using System.Diagnostics;
using System.Linq;

namespace Forest
{
    public class DoubleArrayTrie
    {
        private const char terminator = '#';
        private const char garbage = '?';

        private readonly ICharacterValueMapper characterValueMapper;

        private int[] @base = new int[16];
        private int[] check = new int[16];
        private char[] tail = new char[16];

        private int tailPosition = 1;

        public DoubleArrayTrie(ICharacterValueMapper characterValueMapper)
        {
            if (characterValueMapper == null)
            {
                throw new ArgumentNullException(nameof(characterValueMapper));
            }

            this.characterValueMapper = characterValueMapper;

            @base[1] = 1;
        }

        /// <summary>
        /// Adds the specified key to the <see cref="DoubleArrayTrie"/>.
        /// </summary>
        /// <param name="key">The key to add to the <see cref="DoubleArrayTrie"/>.</param>
        /// <returns>true if the key is added to the <see cref="DoubleArrayTrie"/>; false if the key is already present.</returns>
        /// <exception cref="ArgumentNullException">key is null.</exception>
        public bool Add(string key)
        {
            if (key == null)
            {
                throw new ArgumentNullException("key");
            }

            Debug.WriteLine($"DoubleArrayTrie.Add(\"{key}\"): Executing.");

            var baseIndex = 1;

            for (var keyIndex = 0; keyIndex < key.Length; keyIndex++)
            {
                // Get the base value using the base index.
                var baseValue = GetBaseValue(baseIndex);

                // A negative base value indicates that further matching is to be performed in the tail.
                if (baseValue < 0)
                {
                    Debug.WriteLine($"DoubleArrayTrie.Add(\"{key}\"): Proceed matching in tail.");

                    // Use the negation of the base value as the offset for further matching in the tail.
                    var tailOffset = -baseValue;

                    // Use the index of the next character as the offset for further matching in the tail.
                    var keyOffset = keyIndex;

                    // Determine whether the key is already present. If so, return false as it is already added.
                    if (CheckTailValues(key, keyOffset, tailOffset))
                    {
                        Debug.WriteLine($"DoubleArrayTrie.Add(\"{key}\"): The key '{key}' is already present.");

                        break;
                    }

                    Debug.WriteLine($"DoubleArrayTrie.Add(\"{key}\"): Collision on base[{baseIndex}] for character '{key[keyIndex]}'.");

                    // A conflict has been detected, the base value points to a suffix in tail which does not equal the
                    // suffix of the current key. To resolve this conflict the starting characters common to the suffix
                    // in tail and of the key must be inserted in base, the common characters of the suffix in tail
                    // should be removed, and the the remaining key should be added to tail.
                    ResolveConflict(key, keyOffset, baseIndex);

                    Debug.WriteLine(GetCurrentState());

                    return true;
                }

                // Get the numerical value of the current character.
                var characterValue = characterValueMapper.GetCharacterValue(key[keyIndex]);

                // Get the check value using the base value and the character value.
                var checkValue = GetCheckValue(baseValue + characterValue);

                // A value equal to 0 indicates that the rest of the key is to be inserted in the tail.
                if (checkValue == 0)
                {
                    Debug.WriteLine($"DoubleArrayTrie.Add(\"{key}\"): base[{baseValue + characterValue}] for character '{key[keyIndex]}' available.");

                    // Set the base value using the base value and the character value. Use the negation of the tail
                    // position as the new base value. This negative value indicates that the rest of the key is
                    // inserted in the tail.
                    SetBaseValue(baseValue + characterValue, -tailPosition);

                    // Set the check value using the base value and the character value. Use the base index as the new
                    // value. This value indicates the node it comes from.
                    SetCheckValue(baseValue + characterValue, baseIndex);

                    // Store the rest of the key in the tail using the current tail position as the offset. 
                    SetTailValues(key, keyIndex + 1);

                    Debug.WriteLine(GetCurrentState());

                    return true;
                }

                Debug.WriteLine($"DoubleArrayTrie.Add(\"{key}\"): Character '{key[keyIndex]}' matched.");

                baseIndex = baseValue + characterValue;
            }

            return false;
        }

        private void ResolveConflict(string key, int keyOffset, int baseIndex)
        {
            // Store the base value, containing the tail offset for the collided key, for later use.
            var currentTailOffset = -GetBaseValue(baseIndex);

            // Get the number of characters common to the key and the collided key in tail.
            var commonCharactersLength = GetCommonCharactersLength(key, keyOffset, currentTailOffset);

            Debug.WriteLine($"DoubleArrayTrie.ResolveConflict(\"{key}\", {keyOffset}, {baseIndex}): Common characters length: {commonCharactersLength}.");

            AddCommonCharacters(key, keyOffset, commonCharactersLength, ref baseIndex);

            // Get the next character after the common characters of the key and the collided key in tail.
            var nextTailCharacter = tail[currentTailOffset + commonCharactersLength];
            var nextKeyCharacter = key[keyOffset + commonCharactersLength];

            // Get an available base value for the next character after the common characters of the key and the
            // collided key in tail.
            var availableBaseValue = GetAvailableBaseValue(new[] { nextTailCharacter, nextKeyCharacter });

            // The base value previously pointing to the tail is replaced by the available base value.
            SetBaseValue(baseIndex, availableBaseValue);

            var t = GetBaseValue(baseIndex) + characterValueMapper.GetCharacterValue(nextTailCharacter);

            // Set the base value using the previously stored tail offset.
            SetBaseValue(t, -currentTailOffset);
            SetCheckValue(t, baseIndex);

            MoveTailValues(currentTailOffset, currentTailOffset + commonCharactersLength + 1);

            var tt = GetBaseValue(baseIndex) + characterValueMapper.GetCharacterValue(nextKeyCharacter);

            SetBaseValue(tt, -tailPosition);
            SetCheckValue(tt, baseIndex);

            SetTailValues(key, keyOffset + commonCharactersLength + 1);
        }

        private void AddCommonCharacters(string key, int keyOffset, int commonCharactersLength, ref int baseIndex)
        {
            // All characters in tail common to the current key must be added to base.
            for (var commonCharacterIndex = 0; commonCharacterIndex < commonCharactersLength; commonCharacterIndex++)
            {
                var availableBaseValue = GetAvailableBaseValue(new[] { key[keyOffset + commonCharacterIndex] });
                var characterValue = characterValueMapper.GetCharacterValue(key[keyOffset + commonCharacterIndex]);

                Debug.WriteLine($"DoubleArrayTrie.AddCommonCharacters(\"{key}\", {keyOffset}, {commonCharactersLength}, ref {baseIndex}): Updating base[{baseIndex}] using common character '{key[keyOffset + commonCharacterIndex]}'.");

                SetBaseValue(baseIndex, availableBaseValue);
                SetCheckValue(availableBaseValue + characterValue, baseIndex);

                baseIndex = availableBaseValue + characterValue;
            }
        }

        private void MoveTailValues(int oldOffset, int newOffset)
        {
            var terminatorReached = false;

            for (int oldIndex = oldOffset, newIndex = newOffset; ; oldIndex++, newIndex++)
            {
                if (terminatorReached)
                {
                    if (tail[oldIndex] == terminator)
                    {
                        SetTailValue(oldIndex, garbage);

                        break;
                    }

                    SetTailValue(oldIndex, garbage);

                    continue;
                }

                SetTailValue(oldIndex, tail[newIndex]);

                if (tail[newIndex] == terminator)
                {
                    terminatorReached = true;
                }
            }
        }

        private int GetCommonCharactersLength(string key, int keyOffset, int tailOffset)
        {
            var length = 0;

            for (int keyIndex = keyOffset, tailIndex = tailOffset; keyIndex < key.Length; keyIndex++, tailIndex++)
            {
                var keyValue = key[keyIndex];
                var tailValue = tail[tailIndex];

                if (keyValue != tailValue)
                {
                    break;
                }

                length++;
            }

            return length;
        }

        private int GetAvailableBaseValue(char[] characters)
        {
            for (var baseValue = 1; ; baseValue++)
            {
                for (var characterIndex = 0; characterIndex < characters.Length; characterIndex++)
                {
                    var characterValue = characterValueMapper.GetCharacterValue(characters[characterIndex]);
                    var checkIndex = baseValue + characterValue;

                    if (check[checkIndex] != 0)
                    {
                        break;
                    }

                    if (characterIndex == characters.Length - 1)
                    {
                        return baseValue;
                    }
                }
            }
        }

        private void SetTailValues(string key, int keyOffset)
        {
            Debug.WriteLine($"DoubleArrayTrie.SetTailValues(\"{key}\", {keyOffset}): Writing remaining key '{key.Substring(keyOffset)}' to tail starting from offset {tailPosition}.");

            var tailIndex = tailPosition;

            for (int keyIndex = keyOffset; keyIndex < key.Length; keyIndex++, tailIndex++)
            {
                var character = key[keyIndex];

                SetTailValue(tailIndex, character);
            }

            // Insert the terminator after the last character of the key. The terminator indicates the end of the key.
            SetTailValue(tailIndex, terminator);

            // Update the tail position using the length of the remaining key + 1 for the terminator.
            tailPosition = tailPosition + key.Length - keyOffset + 1;

            Debug.WriteLine($"DoubleArrayTrie.SetTailValues(\"{key}\", {keyOffset}): Current tail position is now {tailPosition}.");
        }

        /// <summary>
        /// Determines whether the <see cref="DoubleArrayTrie"/> contains an element with the specified key.
        /// </summary>
        /// <param name="key">The key to locate in the <see cref="DoubleArrayTrie"/>.</param>
        /// <returns>true if the <see cref="DoubleArrayTrie"/> contains the key; otherwise, false.</returns>
        /// <exception cref="ArgumentNullException">key is null.</exception>
        public bool ContainsKey(string key)
        {
            if (key == null)
            {
                throw new ArgumentNullException("key");
            }

            Debug.WriteLine($"DoubleArrayTrie.ContainsKey(\"{key}\"): Executing.");

            var baseIndex = 1;

            for (var keyIndex = 0; keyIndex < key.Length; keyIndex++)
            {
                // Get the numerical value of the current character.
                var characterValue = characterValueMapper.GetCharacterValue(key[keyIndex]);

                int baseValue;
                int checkValue;

                // Try to get the base value using the base index. This method returns false if the check array length
                // is smaller than the requested index.
                if (!TryGetBaseValue(baseIndex, out baseValue))
                {
                    Debug.WriteLine($"DoubleArrayTrie.ContainsKey(\"{key}\"): Character '{key[keyIndex]}' not matched.");

                    break;
                }

                // A positive value indicates a possible character match.
                if (baseValue > 0)
                {
                    // Try to get the check value using the base value and the character value. This method returns
                    // false if the check array length is smaller than the requested index.
                    if (!TryGetCheckValue(baseValue + characterValue, out checkValue))
                    {
                        Debug.WriteLine($"DoubleArrayTrie.ContainsKey(\"{key}\"): Character '{key[keyIndex]}' not matched.");

                        break;
                    }

                    // The value in check does not match the current base index, which indicates that the character
                    // did not match.
                    if (checkValue != baseIndex)
                    {
                        Debug.WriteLine($"DoubleArrayTrie.ContainsKey(\"{key}\"): Character '{key[keyIndex]}' not matched.");

                        break;
                    }

                    Debug.WriteLine($"DoubleArrayTrie.ContainsKey(\"{key}\"): Character '{key[keyIndex]}' matched.");

                    // The value in check matches the current base index, which indicates that the character matched.
                    // The base value + the character value is the new base index. Proceed to the next character.
                    baseIndex = baseValue + characterValue;
                }
                // A negative value indicates that the rest of the key is located in the tail.
                else if (baseValue < 0)
                {
                    Debug.WriteLine($"DoubleArrayTrie.ContainsKey(\"{key}\"): Proceed matching in tail.");

                    // Use the negation of the base value as the offset for further matching in the tail.
                    var tailOffset = -baseValue;
                    // Use the index of the next character as the offset for further matching in the tail.
                    var keyOffset = keyIndex;

                    return CheckTailValues(key, keyOffset, tailOffset);
                }
                else
                {
                    // TODO: Should this be an exception?
                    break;
                }
            }

            Debug.WriteLine($"DoubleArrayTrie.ContainsKey(\"{key}\"):  Key '{key}' not matched.");

            return false;
        }

        private bool CheckTailValues(string key, int keyOffset, int tailOffset)
        {
            for (int keyIndex = keyOffset, tailIndex = tailOffset; keyIndex < key.Length; keyIndex++, tailIndex++)
            {
                var character = key[keyIndex];
                char tailValue;

                // Try to get the tail value using the tail index. This method returns false if the tail array length
                // is smaller than the requested index.
                if (!TryGetTailValue(tailIndex, out tailValue))
                {
                    Debug.WriteLine($"DoubleArrayTrie.CheckTailValues(\"{key}\", {keyOffset}, {tailOffset}): Character '{key[keyIndex]}' not matched.");

                    break;
                }

                // The value in tail matches the terminator value, which indicates that a key with the same prefix was
                // inserted but not the current key.
                if (tailValue == terminator)
                {
                    Debug.WriteLine($"DoubleArrayTrie.CheckTailValues(\"{key}\", {keyOffset}, {tailOffset}): Character '{key[keyIndex]}' not matched.");

                    break;
                }

                // The value in tail does not match the character value.
                if (tailValue != character)
                {
                    Debug.WriteLine($"DoubleArrayTrie.CheckTailValues(\"{key}\", {keyOffset}, {tailOffset}): Character '{key[keyIndex]}' not matched.");

                    break;
                }

                Debug.WriteLine($"DoubleArrayTrie.CheckTailValues(\"{key}\", {keyOffset}, {tailOffset}): Character '{key[keyIndex]}' matched.");

                // The value in tail matches the character value, which indicates that the character matched. The last
                // character of the key has been reached. If the next character in tail matches the terminator value
                // the key matched.
                if (keyIndex == key.Length - 1)
                {
                    // Try to get the tail value using the next tail index. This method returns false if the tail array
                    // length is smaller than the requested index.
                    if (!TryGetTailValue(tailIndex + 1, out tailValue))
                    {
                        break;
                    }

                    // The value in tail matches the terminator value, which indicates that matching is successful.
                    if (tailValue == terminator)
                    {
                        Debug.WriteLine($"DoubleArrayTrie.CheckTailValues(\"{key}\", {keyOffset}, {tailOffset}): Key '{key}' matched.");

                        return true;
                    }
                }

                // The value in tail matches the character value, which indicates that the character matched. Proceed
                // to the next character.
            }

            Debug.WriteLine($"DoubleArrayTrie.CheckTailValues(\"{key}\", {keyOffset}, {tailOffset}): Key '{key}' not matched.");

            return false;
        }

        private bool TryGetBaseValue(int index, out int value)
        {
            if (index < @base.Length)
            {
                value = @base[index];

                return true;
            }

            value = 0;

            return false;
        }

        private int GetBaseValue(int index)
        {
            if (index >= @base.Length)
            {
                return 0;
            }

            return @base[index];
        }

        private void SetBaseValue(int index, int value)
        {
            ResizeBaseIfNecessary(index);

            Debug.WriteLine($"DoubleArrayTrie.SetBaseValue({index}, {value}): Updating base[{index}] to {value}.");

            @base[index] = value;
        }

        private void ResizeBaseIfNecessary(int index)
        {
            while (index >= @base.Length)
            {
                Debug.WriteLine($"DoubleArrayTrie.ResizeBaseIfNecessary({index}): Resizing base to {@base.Length * 2}.");

                Array.Resize(ref @base, @base.Length * 2);
            }
        }

        private bool TryGetCheckValue(int index, out int value)
        {
            if (index < check.Length)
            {
                value = check[index];

                return true;
            }

            value = 0;

            return false;
        }

        private int GetCheckValue(int index)
        {
            if (index >= check.Length)
            {
                return 0;
            }

            return check[index];
        }

        private void SetCheckValue(int index, int value)
        {
            ResizeCheckIfNecessary(index);

            Debug.WriteLine($"DoubleArrayTrie.SetCheckValue({index}, {value}): Updating check[{index}] to {value}.");

            check[index] = value;
        }

        private void ResizeCheckIfNecessary(int index)
        {
            while (index >= check.Length)
            {
                Debug.WriteLine($"DoubleArrayTrie.ResizeCheckIfNecessary({index}): Resizing check to {check.Length * 2}.");

                Array.Resize(ref check, check.Length * 2);
            }
        }

        private bool TryGetTailValue(int index, out char value)
        {
            if (index < tail.Length)
            {
                value = tail[index];

                return true;
            }

            value = terminator;

            return false;
        }

        private void SetTailValue(int index, char value)
        {
            ResizeTailIfNecessary(index);

            Debug.WriteLine($"DoubleArrayTrie.SetTailValue({index}, {value}): Updating tail[{index}] to '{value}'.");

            tail[index] = value;
        }

        private void ResizeTailIfNecessary(int index)
        {
            while (index >= tail.Length)
            {
                Debug.WriteLine($"DoubleArrayTrie.ResizeTailIfNecessary({index}): Resizing tail to {check.Length * 2}.");

                Array.Resize(ref tail, tail.Length * 2);
            }
        }

        private string GetCurrentState()
        {
            var baseValues = string.Join(",", @base.Select(value => value.ToString().PadLeft(4)));
            var checkValues = string.Join(",", check.Select(value => value.ToString().PadLeft(4)));
            var tailValues = string.Join(",", tail.Select(value => value.ToString().Replace('\0', ' ').PadLeft(4)));

            return $"DoubleArrayTrie.GetCurrentState(): base:  {baseValues}\nDoubleArrayTrie.GetCurrentState(): check: {checkValues}\nDoubleArrayTrie.GetCurrentState(): tail:  {tailValues}";
        }
    }
}