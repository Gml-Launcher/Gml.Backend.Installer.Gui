using System.Globalization;

namespace Gml.Backend.Installer.Gui;

public class Language
{
    public Language(CultureInfo cultureInfo)
    {
        Name = cultureInfo.NativeName;
        Code = cultureInfo.Name;
    }

    public Language(string name, string code)
    {
        Name = name;
        Code = code;
    }

    public string Name { get; }
    public string Code { get; }
}