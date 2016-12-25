// Copyright (c) Arjen Post. See License.txt in the project root for license information.

namespace Forest
{
    public class TemporaryICharacterValueMapper : ICharacterValueMapper
    {
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