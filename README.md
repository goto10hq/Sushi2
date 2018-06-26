![Sushi2](https://raw.githubusercontent.com/goto10hq/Sushi2/master/sushi-icon.png)

# Sushi2
> Utility library for cool kids

[![Software License](https://img.shields.io/badge/license-MIT-brightgreen.svg?style=flat-square)](LICENSE.md)
[![Latest Version on NuGet](https://img.shields.io/nuget/v/Sushi2.svg?style=flat-square)](https://www.nuget.org/packages/Sushi2/)
[![NuGet](https://img.shields.io/nuget/dt/Sushi2.svg?style=flat-square)](https://www.nuget.org/packages/Sushi2/)
[![Visual Studio Team services](https://img.shields.io/vso/build/frohikey/c3964e53-4bf3-417a-a96e-661031ef862f/117.svg?style=flat-square)](https://github.com/goto10hq/Sushi2)

## Cultures

Small shortcuts for predefined culture info. And of course good ol' invariant one.

```Sushi2.Cultures.Czech```

```Sushi2.Cultures.English```

```Sushi2.Cultures.Invariant```

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

### GetEnumFieldDescription(Enum field)
### GetEnumFieldDescription(Enum field, string zeroValueName)

```csharp
Console.WriteLine(EnumTools.GetEnumFieldDescription(Robot.One));
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

All the calls return ``Robot.One``.

## Extensions

### string ToDbString(this string text)

Returns trimmed string and never null.

```csharp
"   foo ".ToDbString()
```

```
foo
```

## History

Based on various tools, helpers, ... I've been building since .NET beta packed in library called **Sushi**. 
I've removed some legacy code, refactored existing one and voila here we go with a fresh new smile for .NET Standard.
