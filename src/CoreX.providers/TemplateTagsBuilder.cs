#region Header

// --------------------------------------------------------------------------------------
// Powered by:
// 
//     __________.__                  .___    ___________                             
//     \______   \__| ____   ____   __| _/____\__    ___/___   ____       ____  __ __ 
//      |     ___/  |/    \_/ __ \ / __ |\__  \ |    |_/ __ \_/ ___\    _/ __ \|  |  \
//      |    |   |  |   |  \  ___// /_/ | / __ \|    |\  ___/\  \___    \  ___/|  |  /
//      |____|   |__|___|  /\___  >____ |(____  /____| \___  >\___  > /\ \___  >____/ 
//                   \/     \/     \/     \/           \/     \/  \/     \/
// 
// 
// FileName: TemplateTagsBuilder.cs
//
// Author:   jmr.pineda
// eMail:    jmr.pineda@pinedatec.eu
// Profile:  http://pinedatec.eu/profile
//
//           Copyrights (c) PinedaTec.eu 2025, all rights reserved.
//           CC BY-NC-ND - https://creativecommons.org/licenses/by-nc-nd/4.0
//
//  Created at: 2025-02-07T08:40:51.684Z
//
// --------------------------------------------------------------------------------------

#endregion

namespace CoreX.providers;

using System.Globalization;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

using Microsoft.Extensions.Configuration;

using CoreX.aspects;
using CoreX.extensions;

using NLog;

public class TemplateTagsBuilder : ITemplateTagsBuilder
{
    private const string msg_NotResolved = "Cannot find a key '{0}', so the expression is not resolved!";

    // Samples
    // ${same:value}
    // ${same.value}
    // ${same.value:[format: asdasdasd]}
    // ${same.value:[default:(int)10]}
    // ${same.value:[default:(bool)true]}
    // ${same.value:[default:(decimal)10,4]}
    // ${same.value:[format: asasdas][default:10]}
    // $${{same.value}} // Escaped

    private readonly string _regularPattern = @"\$\{(?'var'\w+(?:[:|\.]*\w+)*)(?:(?'extended'[=:]?(\[.*?\]))?)\}";
    private readonly string _escapedPattern = @"\$\$\{\{(?'var'\w+(?:[:|\.]*\w+)*)(?:(?'extended'[=:]?(\[.*?\]))?)\}\}";
    private readonly string _randomNumberPattern = @"\$\{randomnumber:\[(?'min'\d+)(?:\s*,\s*(?'max'\d+))?\]\}";
    private readonly string _randomStringPattern = @"\$\{randomstring:\[(?'length'\d+)\]\}";
    private readonly string _extendedPattern = @"(?'format'\[format:(?'formvalue'.*?)\])|(?'default'\[default:(?'defvalue'.*?)\])";

    private readonly ILogger _logger = LogManager.GetCurrentClassLogger();

    public bool ParseBooleanToLower { get; set; } = true;

    public string SearchPattern { get => _regularPattern; }

    public TemplateTagsBuilder(string? regularExpression = null)
    {
        _regularPattern = regularExpression ?? _regularPattern;
    }

    public virtual Dictionary<string, object?> GetStandardTags(Dictionary<string, object?> dict)
    {
        var stag = this.GetStandardTags();

        if (dict is null)
        {
            return stag;
        }

        stag = dict.Aggregate(stag, (current, next) =>
        {
            current[next.Key] = next.Value;
            return current;
        });

        return stag;
    }

    public virtual Dictionary<string, object?> GetStandardTags()
    {
        Dictionary<string, object?> dict = new(StringComparer.InvariantCultureIgnoreCase);
        DateTime currentTime = DateTime.UtcNow;

        dict.Add("Version", AppsHelper.GetAppVersion().ToString());
        dict.Add("Year", currentTime.Year);
        dict.Add("Month", CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(currentTime.Month));
        dict.Add("Day", currentTime.Day);
        dict.Add("DayOfWeek", currentTime.DayOfWeek);
        dict.Add("Hour", currentTime.Hour);
        dict.Add("Minute", currentTime.Minute);
        dict.Add("Second", currentTime.Second);
        dict.Add("DayOfYear", currentTime.DayOfYear);
        dict.Add("Now", currentTime);
        dict.Add("HostName", Environment.MachineName);
        dict.Add("MachineName", Environment.MachineName);

        dict.Add("CurrentUser", Environment.UserName);
        dict.Add("UserDomainName", Environment.UserDomainName);

        return dict;
    }

    [NLogExecutionTimeAttribute]
    public string Parse(string expression, Dictionary<string, object?>? tagValues = null, IConfigurationSection? section = null,
        bool exceptionNotResolved = false, Func<string, string, string, string>? cryptoProvider = null) =>
        this.Parse(expression, 0, tagValues, section, exceptionNotResolved, cryptoProvider);

    private string Parse(string expression, int iteration, Dictionary<string, object?>? tagValues = null, IConfigurationSection? section = null,
           bool exceptionNotResolved = false, Func<string, string, string, string>? cryptoProvider = null)
    {
        _logger.Trace($"Parsing expression");

        expression.ThrowArgumentExceptionIfNullOrEmpty(nameof(expression));
        _regularPattern.ThrowArgumentExceptionIfNullOrEmpty(nameof(_regularPattern));

        tagValues ??= new Dictionary<string, object?>(StringComparer.InvariantCultureIgnoreCase);

        var matches = Regex.Matches(expression, _regularPattern, RegexOptions.IgnoreCase |
            RegexOptions.Multiline);
        string resolvedExpression = expression;

        if (matches.Count == 0)
        {
            if (iteration > 0)
            {
                expression = this.ParseEscaped(expression);
                _logger.Trace("Finished parsing expression");
            }
            else
            {
                _logger.Warn("No matches found in expression, and the expression is not resolved!");
            }

            return expression;
        }

        StringBuilder sb = new StringBuilder(resolvedExpression);
        _logger.Debug("Found {0} matches in expression", matches.Count);
        foreach (Match m in matches)
        {
            this.ParseMatch(tagValues, section, cryptoProvider, sb, m);
        }

        resolvedExpression = sb.ToString();

        if (resolvedExpression == expression)
        {
            if (exceptionNotResolved)
            {
                StringBuilder sba = new();
                var list = matches.Select(m => m.Value);
                var listText = string.Join(", ", list);
                sba.Append(listText);
                //matches.ToList().ForEach(mat => sba.Append(mat.Value));
                string text = sba.ToString();

                throw new InvalidOperationException(string.Format(msg_NotResolved, text));
            }

            _logger.Warn("Cannot find a key, so the expression is not completly resolved!");

            return resolvedExpression;
        }

        return this.Parse(resolvedExpression, iteration + 1, tagValues, section, exceptionNotResolved, cryptoProvider);
    }

    private string ParseEscaped(string expression)
    {
        _logger.Trace("Parsing escaped expression");

        expression.ThrowArgumentExceptionIfNullOrEmpty(nameof(expression));

        var matches = Regex.Matches(expression, _escapedPattern, RegexOptions.IgnoreCase |
            RegexOptions.Multiline);
        string resolvedExpression = expression;

        if (matches.Count == 0)
        {
            _logger.Warn("No matches found in expression, and the expression is not resolved!");
            return expression;
        }

        StringBuilder sb = new StringBuilder(resolvedExpression);
        _logger.Debug("Found {0} matches in expression", matches.Count);
        foreach (Match m in matches)
        {
            var key = m.Groups["var"].Value;
            var gkey = m.Value;
            var value = "${" + key + "}";
            this.ReplaceValue(gkey, key, value, sb, false);
        }

        return sb.ToString();
    }

    private void ParseMatch(Dictionary<string, object?> tagValues, IConfigurationSection? section,
        Func<string, string, string, string>? cryptoProvider, StringBuilder sb, Match match)
    {
        string key = match.Groups["var"].Value.ToLower();
        string gkey = match.Value;
        bool secret = TemplateTagsBuilder.CheckKeyWord(ref key);

        string format = this.GetExtendedFormat(match);

        if (TemplateTagsBuilder.IsReservedKeyword(key))
        {
            this.ParseReservedWords(cryptoProvider, sb, match, key, gkey, secret, format);
            return;
        }

        if (tagValues.ContainsKey(key))
        {
            var value = TemplateTagsBuilder.FormatObject(tagValues[key], format);
            if (value is not null)
            {
                this.ReplaceValue(gkey, key, value, sb, secret, cryptoProvider);
                return;
            }
        }

        if (section is not null)
        {
            var value = section[key];
            if (value is not null)
            {
                this.ReplaceValue(gkey, key, value, sb, secret, cryptoProvider);
                return;
            }
        }

        if (match.Groups["extended"].Success)
        {
            this.GetExtendedDefaultValues(cryptoProvider, sb, match, key, gkey, secret, format);
            return;
        }
    }

    private static bool CheckKeyWord(ref string key)
    {
        bool secret = key.StartsWith("secret:");
        bool shasec = key.StartsWith("shasec:");

        if (secret)
        {
            key = key.Replace("secret:", string.Empty);
        }
        else
        {
            if (shasec)
            {
                key = key.Replace("shasec:", string.Empty);
            }
        }

        return secret;
    }

    private void ParseReservedWords(Func<string, string, string, string>? cryptoProvider, StringBuilder sb, Match match, string key, string gkey, bool secret, string format)
    {
        if (key.StartsWith("randomstring"))
        {
            var value = this.GenerateRandomString(match);
            this.ReplaceValue(gkey, key, value, sb, secret, cryptoProvider);
            return;
        }
        if (key.StartsWith("randomnumber"))
        {
            var value = this.GenerateRandomNumber(match, format);
            this.ReplaceValue(gkey, key, value, sb, secret, cryptoProvider);
            return;
        }

        throw new NotImplementedException();
    }

    private void GetExtendedDefaultValues(Func<string, string, string, string>? cryptoProvider, StringBuilder sb, Match match, string key, string gkey, bool secret, string format)
    {
        var extended = match.Groups["extended"].Value;

        var matches = Regex.Matches(extended, _extendedPattern, RegexOptions.IgnoreCase |
            RegexOptions.Multiline);

        if (matches.Count == 0)
        {
            throw new InvalidOperationException(
                string.Format(msg_NotResolved, extended));
        }

        var value = matches[0].Groups["defvalue"].Value;
        var objValue = TemplateTagsBuilder.ConvertToType(value);

        value = TemplateTagsBuilder.FormatObject(objValue, format)!;
        this.ReplaceValue(gkey, key, value, sb, secret, cryptoProvider);
        return;
    }

    private string GetExtendedFormat(Match match)
    {
        var format = string.Empty;
        if (match.Groups["extended"].Success)
        {
            var matches = Regex.Matches(match.Value, _extendedPattern, RegexOptions.IgnoreCase |
                RegexOptions.Multiline);

            var em = matches.FirstOrDefault(em => em.Groups["formvalue"].Success);
            if (em is not null)
            {
                format = em.Groups["formvalue"].Value;
            }
        }

        return format;
    }

    private static object ConvertToType(string input)
    {
        if (string.IsNullOrEmpty(input))
        {
            throw new ArgumentNullException(nameof(input));
        }

        var typeValuePair = input.Trim('(', ')').Split(')');

        if (typeValuePair.Length != 2)
        {
            typeValuePair = ["string", input];
        }

        string type = typeValuePair[0].Trim();
        string value = typeValuePair[1].Trim();

        return type.ToLower() switch
        {
            "int" => int.Parse(value, CultureInfo.InvariantCulture),
            "double" => double.Parse(value, CultureInfo.InvariantCulture),
            "decimal" => decimal.Parse(value, CultureInfo.InvariantCulture),
            "datetime" => DateTime.Parse(value, CultureInfo.InvariantCulture),
            "dateonly" => DateOnly.Parse(value, CultureInfo.InvariantCulture),
            "date" => DateOnly.Parse(value, CultureInfo.InvariantCulture),
            "timeonly" => DateTime.Parse(value, CultureInfo.InvariantCulture).TimeOfDay,
            "time" => DateTime.Parse(value, CultureInfo.InvariantCulture).TimeOfDay,
            "bool" => bool.Parse(value),
            "string" => value,
            _ => throw new InvalidOperationException($"Unsupported type [{type}]")
        };
    }

    private static string? FormatObject(object? value, string format)
    {
        if (value is null)
        {
            return default;
        }

        if (format.IsNullOrEmpty())
        {
            return value.ToString();
        }

        switch (value)
        {
            case DateTime dateTime:
                return dateTime.ToString(format, CultureInfo.InvariantCulture);
            case int intValue:
                return intValue.ToString(format, CultureInfo.InvariantCulture);
            case double doubleValue:
                return doubleValue.ToString(format, CultureInfo.InvariantCulture);
            case decimal decimalValue:
                return decimalValue.ToString(format, CultureInfo.InvariantCulture);
            case IFormattable formattable:
                return formattable.ToString(format, CultureInfo.InvariantCulture);
            default:
                throw new ArgumentException("Unsupported type for formatting", nameof(value));
        }
    }

    private static bool IsReservedKeyword(string key) =>
        new[] { "randomstring", "randomnumber", "secret", "shasec" }.Contains(key);

    private void ReplaceValue(string gkey, string key, string value, StringBuilder strb, bool secret,
        Func<string, string, string, string>? cryptoProvider = null)
    {
        _logger.Trace("Replacing value for key: {0}", key);
        cryptoProvider ??= (k, gk, v) => v;
        if (bool.TryParse(value, out bool boolValue) && this.ParseBooleanToLower)
        {
            value = value.ToLower();
        }

        if (secret)
        {
            value = $"secret:{cryptoProvider(key, gkey, value)}";
        }

        if (key.Equals("secrets", StringComparison.InvariantCultureIgnoreCase))
        {
            value = $"shasec:{cryptoProvider(key, gkey, value)}";
        }

        strb.Replace(gkey, value);
    }

    private string GenerateRandomString(Match match)
    {
        _logger.Trace("Generating random string");

        var matches = Regex.Matches(match.Value, _randomStringPattern, RegexOptions.IgnoreCase |
            RegexOptions.Multiline);

        if (matches.Count == 0)
        {
            throw new InvalidOperationException(string.Format(msg_NotResolved, match.Value));
        }

        var length = int.Parse(matches[0].Groups["length"].Value);
        return AppsHelper.GenerateRandomString(length);
    }

    private string GenerateRandomNumber(Match match, string format)
    {
        _logger.Trace("Generating random number");

        var matches = Regex.Matches(match.Value, _randomNumberPattern, RegexOptions.IgnoreCase |
            RegexOptions.Multiline);

        if (matches.Count == 0)
        {
            throw new InvalidOperationException(
                string.Format(msg_NotResolved, match.Value));
        }

        var minLength = int.Parse(matches[0].Groups["min"].Value);
        var maxLength = 0;

        if (matches[0].Groups["max"].Success)
        {
            maxLength = int.Parse(matches[0].Groups["max"].Value);
        }

        if (minLength > maxLength)
        {
            var temp = minLength;
            minLength = maxLength;
            maxLength = temp;
        }

        return RandomNumberGenerator.GetInt32(minLength, maxLength).ToString(format);
    }
}
