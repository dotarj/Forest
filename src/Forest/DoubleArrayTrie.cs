// Copyright (c) Arjen Post. See License.txt in the project root for license information.

using System;
using System.Diagnostics;

namespace Forest
{
    public class DoubleArrayTrie
    {
        private readonly int terminatorValue;

        private int[] @base = new int[64];
        private int[] check = new int[64];
        private int[] tail = new int[64];

        public DoubleArrayTrie()
        {
            terminatorValue = GetCharacterValue('#');
        }

        public void Add(string key)
        {
            throw new NotImplementedException();
        }

        public bool ContainsKey(string key)
        {
            if (key == null)
            {
                throw new ArgumentNullException("key");
            }

            Debug.WriteLine($"DoubleArrayTrie.ContainsKey({key}): Executing.");

            var baseIndex = 1;

            for (var keyIndex = 0; keyIndex < key.Length; keyIndex++)
            {
                // Get the numerical value of the current character.
                var characterValue = GetCharacterValue(key[keyIndex]);

                int baseValue;
                int checkValue;

                // Try to get the base value using the base index and the check value using the base value and the
                // character value. These methods return false if the base or check array length is smaller than the
                // requested index.
                if (!TryGetBaseValue(baseIndex, out baseValue) || !TryGetCheckValue(baseValue + characterValue, out checkValue))
                {
                    Debug.WriteLine($"DoubleArrayTrie.ContainsKey({key})': Character '{key[keyIndex]}' not matched.");

                    break;
                }

                // A positive value indicates a possible character match.
                if (checkValue > 0)
                {
                    // The value in check does not match the current base index, which indicates that the character
                    // did not match.
                    if (checkValue != baseIndex)
                    {
                        Debug.WriteLine($"DoubleArrayTrie.ContainsKey({key})': Character '{key[keyIndex]}' not matched.");

                        break;
                    }

                    Debug.WriteLine($"DoubleArrayTrie.ContainsKey({key})': Character '{key[keyIndex]}' matched in base.");

                    // The value in check matches the current base index, which indicates that the character matched.
                    // The base value + the character value is the new base index. Proceed to the next character.
                    baseIndex = baseValue + characterValue;
                }
                // A negative value indicates that the rest of the key is located in the tail.
                else if (checkValue < 0)
                {
                    Debug.WriteLine($"DoubleArrayTrie.ContainsKey({key})': Character '{key[keyIndex]}' matched in base. Proceeding matching in tail.");

                    // Use the negation of the check value as the offset for futher matching in the tail.
                    var tailOffset = -checkValue;
                    // Use the index of the next character as the offset for futher matching in the tail.
                    var keyOffset = keyIndex + 1;

                    return CheckTailValue(key, keyOffset, tailOffset);
                }
                else
                {
                    // TODO: Should this be an exception?
                    return false;
                }
            }

            // TODO: Add debug logging.

            return false;
        }

        private bool CheckTailValue(string key, int keyOffset, int tailOffset)
        {
            Debug.WriteLine($"DoubleArrayTrie.CheckTailValue({key}, {keyOffset}, {tailOffset}): Executing.");

            for (int keyIndex = keyOffset, tailIndex = tailOffset; keyIndex < key.Length; keyIndex++, tailIndex++)
            {
                var characterValue = GetCharacterValue(key[keyIndex]);
                int tailValue;

                // Try to get the tail value using the tail index. This method returns false if the tail array length
                // is smaller than the requested index.
                if (!TryGetTailValue(tailIndex, out tailValue))
                {
                    Debug.WriteLine($"DoubleArrayTrie.CheckTailValue({key}, {keyOffset}, {tailOffset}): Character '{key[keyIndex]}' not matched.");

                    break;
                }

                // The value in tail matches the terminator value, which indicates that a key with the same prefix was
                // inserted but not the current key.
                if (tailValue == terminatorValue)
                {
                    Debug.WriteLine($"DoubleArrayTrie.CheckTailValue({key}, {keyOffset}, {tailOffset}): Character '{key[keyIndex]}' not matched.");

                    break;
                }

                // The value in tail does not match the character value.
                if (characterValue != tailValue)
                {
                    Debug.WriteLine($"DoubleArrayTrie.CheckTailValue({key}, {keyOffset}, {tailOffset}): Character '{key[keyIndex]}' not matched.");

                    break;
                }

                // The value in tail matches the character value, which indicates that the character matched. The last
                // character of the key has been reached. If the next character in tail matches the terminator value
                // the key matched.
                if (keyIndex == key.Length - 1)
                {
                    // Try to get the tail value using the tail index. This method returns false if the tail array
                    // length is smaller than the requested index.
                    if (!TryGetTailValue(tailIndex, out tailValue))
                    {
                        break;
                    }

                    // The value in tail matches the terminator value, which indicates that matching is successful.
                    if (tailValue == terminatorValue)
                    {
                        return true;
                    }
                }

                // The value in tail matches the character value, which indicates that the character matched. Proceed
                // to the next character.
            }

            return false;
        }

        private int GetCharacterValue(char character)
        {
            // Temporary method to retrieve character value.
            if (character == '#')
            {
                return -1;
            }

            return (int)character - 65;
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

        private bool TryGetTailValue(int index, out int value)
        {
            if (index < tail.Length)
            {
                value = tail[index];

                return true;
            }

            value = 0;

            return false;
        }
    }
}