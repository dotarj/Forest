// Copyright (c) Arjen Post. See License.txt in the project root for license information.

namespace Forest
{
    public class TemporaryCharacterValueMapper : ICharacterValueMapper
    {
        public int MaxCharacterValue
        {
            get
            {
                return 26;
            }
        }

        public int MinCharacterValue
        {
            get
            {
                return 1;
            }
        }

        public char GetCharacter(int characterValue)
        {
            if (characterValue == -1)
            {
                return '#';
            }

            return (char)(characterValue + 95);
        }

        public int GetCharacterValue(char character)
        {
            // Temporary method to retrieve character value.
            if (character == '#')
            {
                return -1;
            }

            return (int)character - 95;
        }
    }
}