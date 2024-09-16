using Apps.Common;

namespace GeneratePassword;

internal sealed record Config(
    bool IncludeLowerCase = true,
    bool IncludeUpperCase = true,
    bool IncludeNumbers = true,
    bool IncludeSymbols = true,
    bool ExcludeAmbiguous = false,
    bool ExcludeBrackets = false)
{
    public int RequiredCharsAmount = 
        IncludeLowerCase.ToInt() + IncludeUpperCase.ToInt() + IncludeNumbers.ToInt() + IncludeSymbols.ToInt();
};
