# ark.abstractions

Abstractions, extensions

## Nugets

- ark.extensions
- ark.providers

## ark.extensions

This project contains several useful extensions for working with collections, strings, and other common .NET data types.

### Components

- Extensions to provide code-snippets, like throws an exception if the instance is null, or some boolean is false.

## ark.providers

This project provides implementations of providers for different services and functionalities.

### TemplateTagsBuilder

The **TemplateTagsBuilder** is a utility designed to facilitate the creation and management of template tags within a project. It provides a structured way to define, organize, and manipulate tags that can be used in various templating scenarios, such as generating dynamic content, customizing templates, and managing placeholders.

####  Purpose

**Tag Definition**: Allows developers to define custom tags that can be used within templates.
**Tag Organization**: Helps in organizing tags in a systematic manner, making it easier to manage and maintain them.
**Dynamic Content**: Enables the generation of dynamic content by replacing tags with actual values or content during runtime.
**Template Customization**: Provides flexibility in customizing templates by using predefined tags that can be replaced or modified as needed.
**Placeholder Management**: Simplifies the management of placeholders within templates, ensuring that all tags are properly handled and replaced.

## NLogExecutionTimeAttribute

The `NLogExecutionTimeAttribute` is used to log the execution time of methods using NLog. To use this attribute, you need to include `[module: NLogExecutionTimeAttribute]` in one file of the project that will use it.
