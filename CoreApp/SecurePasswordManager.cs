using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;

internal class SecurePasswordManager
{
    private const string LowerCase = "abcdefghijklmnopqrstuvwxyz";
    private const string UpperCase = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
    private const string Numbers = "0123456789";
    private const string SpecialCharacters = "!@#$%^&*()_-+=[{]};:<>|./?";

    // Password length and complexity
    public int MinPasswordLength { get; private set; }     
    public int MinLowerCase { get; private set; }
    public int MinUpperCase { get; private set; }
    public int MinNumbers { get; private set; }
    public int MinSpecialCharacters { get; private set; }

    public SecurePasswordManager(int minPasswordLength = 8, int minLowerCase = 1, int minUpperCase = 1, int minNumbers = 1, int minSpecialCharacters = 1)
    {
        MinPasswordLength = minPasswordLength;
        MinLowerCase = minLowerCase;
        MinUpperCase = minUpperCase;
        MinNumbers = minNumbers;
        MinSpecialCharacters = minSpecialCharacters;
    }

    public string GeneratePassword()
    {
        var passwordBuilder = new StringBuilder();
        var random = new Random();

        // Add random characters of each type to meet minimum criteria
        passwordBuilder.Append(GetRandomCharacters(LowerCase, MinLowerCase, random));
        passwordBuilder.Append(GetRandomCharacters(UpperCase, MinUpperCase, random));
        passwordBuilder.Append(GetRandomCharacters(Numbers, MinNumbers, random));
        passwordBuilder.Append(GetRandomCharacters(SpecialCharacters, MinSpecialCharacters, random));

        // Fill the remaining length of the password with a random selection of all character types
        int remainingLength = MinPasswordLength - passwordBuilder.Length;
        string allCharacters = LowerCase + UpperCase + Numbers + SpecialCharacters;
        passwordBuilder.Append(GetRandomCharacters(allCharacters, remainingLength, random));

        // Shuffle the constructed password to avoid predictable patterns
        var passwordArray = passwordBuilder.ToString().ToCharArray();
        passwordArray = passwordArray.OrderBy(x => random.Next()).ToArray();

        return new string(passwordArray);
    }

    private string GetRandomCharacters(string possibleCharacters, int count, Random random)
    {
        return new string(Enumerable.Repeat(possibleCharacters, count)
                                     .Select(s => s[random.Next(s.Length)]).ToArray());
    }

    public bool ValidatePassword(string password)
    {
        if (password.Length < MinPasswordLength)
            return false;

        // Validate the number of each type of character
        int lowerCaseCount = password.Count(char.IsLower);
        int upperCaseCount = password.Count(char.IsUpper);
        int numberCount = password.Count(char.IsDigit);
        int specialCharCount = password.Count(c => SpecialCharacters.Contains(c));

        return lowerCaseCount >= MinLowerCase &&
               upperCaseCount >= MinUpperCase &&
               numberCount >= MinNumbers &&
               specialCharCount >= MinSpecialCharacters;
    }
}
