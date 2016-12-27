// Copyright (c) Arjen Post. See License.txt in the project root for license information.

namespace Forest
{
    public interface ICharacterValueMapper
    {
        int MinCharacterValue { get; }

        int MaxCharacterValue { get; }

        int GetCharacterValue(char character);

        char GetCharacter(int characterValue);
    }
}