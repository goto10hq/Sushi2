![Sushi2](https://raw.githubusercontent.com/goto10hq/Sushi2/master/sushi-icon.png)

# Sushi2

> Utility library for cool kids

[![Software License](https://img.shields.io/badge/license-MIT-brightgreen.svg)](LICENSE.md)
[![Latest Version on NuGet](https://img.shields.io/nuget/v/Sushi2.svg)](https://www.nuget.org/packages/Sushi2/)
[![NuGet](https://img.shields.io/nuget/dt/Sushi2.svg)](https://www.nuget.org/packages/Sushi2/)
[![Visual Studio Team services](https://img.shields.io/vso/build/frohikey/c3964e53-4bf3-417a-a96e-661031ef862f/117.svg)](https://github.com/goto10hq/Sushi2)
[![.NETStandard 2.0](https://img.shields.io/badge/.NETStandard-2.0-blue.svg)](https://github.com/dotnet/standard/blob/master/docs/versions/netstandard2.0.md)

_Slowly and steady I'm updating doc here..._

## Cultures

Small shortcuts for predefined culture info. And of course good ol' invariant/current ones.

```Sushi2.Cultures.Czech```

```Sushi2.Cultures.English```

```Sushi2.Cultures.Invariant```

```Sushi2.Cultures.Current```

## Enum Tools

Enum helper methods.

```csharp
public enum Robot
{
    [Description("First")]
    One = 1,
    [Description("Second")]
    Two = 2,
    Three = 3
}
```

### string GetEnumFieldDescription(Enum field)
### string GetEnumFieldDescription(Enum field, string zeroValueName)

```csharp
EnumTools.GetEnumFieldDescription(Robot.One);
```

```
First
```

### Parse(object value)
### Parse(object value, T defaultValue)

```csharp
EnumTools.Parse<Robot>("1");
EnumTools.Parse<Robot>("One");
EnumTools.Parse<Robot>(Robot.One);
EnumTools.Parse<Robot>(1);
EnumTools.Parse<Robot>("foo", Robot.One); // this one fails, def value is returned
```

```
Robot.One
Robot.One
Robot.One
Robot.One
Robot.One
```

## Extensions

### string ToDbString(this string text)

Returns trimmed string and never null.

```csharp
"   foo ".ToDbString()
```

```
foo
```

### string ToNormalizedString(this string text)

Returns trimmed, lower-cased string without diacritics chars. Oh yeah and never null value.

```csharp
" Žluťoučký kůň ".ToNormalizedString()
```

```
zlutoucky kun
```

### string ToCzechSortedString(this string text)

Returns normalized string usable for sorting operations for Czech language. Usable in cases when you cannot use a culture specific algorithm (DocumentDB etc.).

```csharp
"Žluťoučký kůň".ToSortedString()
```

```
zbluau*ouad*kz* kuco*
```

### string ToCzechNonBreakingSpacesString(this string text, string nbsp = "\&nbsp;")

Returns string with space replaced with a non-breaking space string.

```csharp
"Dr. Ferda mravenec má telefonní číslo 600 111 333.".ToCzechNonBreakingSpacesString("~");
```

```
Dr.~Ferda mravenec má telefonní číslo 600~111~333.
```

## History

Based on various tools, helpers, ... I've been building since .NET beta packed in library called **Sushi**. 
I've removed some legacy code, refactored existing one and voila here we go with a fresh new smile for .NET Standard.
