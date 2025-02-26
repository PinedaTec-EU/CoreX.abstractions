# TemplateTagsBuilder

The `TemplateTagsBuilder` class is a utility designed to facilitate the creation and management of template tags within a project. It provides a structured way to define, organize, and manipulate tags that can be used in various templating scenarios, such as generating dynamic content, customizing templates, and managing placeholders.

## Summary

The `TemplateTagsBuilder` class offers the following functionalities:

- **Tag Definition**: Allows developers to define custom tags that can be used within templates.
- **Dynamic Content**: Enables the generation of dynamic content by replacing tags with actual values or content during runtime.
- **Template Customization**: Provides flexibility in customizing templates by using predefined tags that can be replaced or modified as needed.
- **Placeholder Management**: Simplifies the management of placeholders within templates, ensuring that all tags are properly handled and replaced.

### Standard tags provided

- Version: AppsHelper.GetAppVersion()
- Year: DateTime.UtcNow.Year
- Month: CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(DateTime.UtcNow.Month)
- Day: DateTime.UtcNow.Day
- DayOfWeek: DateTime.UtcNow.DayOfWeek
- Hour: DateTime.UtcNow.Hour
- Minute: DateTime.UtcNow.Minute
- Second: DateTime.UtcNow.Second
- DayOfYear: DateTime.UtcNow.DayOfYear
- Now: DateTime.UtcNow
- HostName: Environment.MachineName
- MachineName: Environment.MachineName
- CurrentUser: Environment.UserName
- UserDomainName: Environment.UserDomainName

## Examples

### Example: Parsing a Simple Expression

```csharp
var templateTagsBuilder = new TemplateTagsBuilder();
var expression = "${Year}";
var tagValues = templateTagsBuilder.GetStandardTags();

var result = templateTagsBuilder.Parse(expression, tagValues);

Console.WriteLine(result); // Outputs the current year
```

### Example: Using default values

```csharp
var templateTagsBuilder = new TemplateTagsBuilder();
var expression = "${sample=[default:(int)10]}";
var tagValues = templateTagsBuilder.GetStandardTags();

var result = templateTagsBuilder.Parse(expression, tagValues);

Console.WriteLine(result); // Outputs "10"
```

### Example: Formatting DateTime

```csharp
var templateTagsBuilder = new TemplateTagsBuilder();
var expression = "${datetime:[format:yyyy-MM-dd]}";
var tagValues = templateTagsBuilder.GetStandardTags();
tagValues.Add("datetime", DateTime.UtcNow);

var result = templateTagsBuilder.Parse(expression, tagValues);

Console.WriteLine(result); // Outputs the current date in "yyyy-MM-dd" format
```

### Example: Formatting values

```csharp
var templateTagsBuilder = new TemplateTagsBuilder();
var expression = "${datetime:[format:yyyy-MM-dd]}";
var tagValues = templateTagsBuilder.GetStandardTags();
tagValues.Add("datetime", DateTime.UtcNow);

var result = templateTagsBuilder.Parse(expression, tagValues);

Console.WriteLine(result); // Outputs the current date in "yyyy-MM-dd" format
```

### Example: Using a Crypto Provider

```csharp
var templateTagsBuilder = new TemplateTagsBuilder();
var expression = "${secret:variable}";
var tagValues = templateTagsBuilder.GetStandardTags();
Func<string, string, string, string> cryptoProvider = (key, gkey, value) => value.ToUpper();
tagValues.Add("variable", "value");

var result = templateTagsBuilder.Parse(expression, tagValues, cryptoProvider: cryptoProvider);

Console.WriteLine(result); // Outputs "SECRET:VALUE"
```

### Example: Escaped variables

```csharp
var templateTagsBuilder = new TemplateTagsBuilder();
var expression = "${variable} $${{variable}}";
var tagValues = templateTagsBuilder.GetStandardTags();

tagValues.Add("variable", "value");

var result = templateTagsBuilder.Parse(expression, tagValues);

Console.WriteLine(result); // Outputs "value ${variable}"
```

### Example: Using configuration section

```csharp
var templateTagsBuilder = new TemplateTagsBuilder();
var expression = "${company}";
var tagValues = templateTagsBuilder.GetStandardTags();

var config = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("sample.config.json", optional: false, reloadOnChange: true)
    .Build();

// { "template": { "company": "pinedatec.eu"}}
var configSection = config.GetSection("template");

var result = templateTagsBuilder.Parse(expression, tagValues, configSection);

Console.WriteLine(result); // Outputs: "pinedatec.eu"
```

## Recommendations

Use Meaningful Tag Names: When defining custom tags, use meaningful names that clearly indicate their purpose. This will make your templates easier to understand and maintain.
Organize Tags Systematically: Group related tags together and document their usage. This will help in managing and maintaining the tags effectively.
Handle Exceptions Gracefully: Ensure that your code handles exceptions gracefully, especially when tags cannot be resolved. This will prevent runtime errors and improve the robustness of your application.
Leverage Default Values and Formatting: Use default values and formatting options to provide flexibility in your templates. This will allow you to customize the output based on different scenarios.

## Comments

The TemplateTagsBuilder class is a powerful utility for managing template tags in .NET applications. It provides a comprehensive set of features for defining, organizing, and manipulating tags, making it an essential tool for developers working with dynamic content and templates.
