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
// FileName: TemplateTagBuilderTest.cs
//
// Author:   jmr.pineda
// eMail:    jmr.pineda@pinedatec.eu
// Profile:  http://pinedatec.eu/profile
//
//           Copyrights (c) PinedaTec.eu 2025, all rights reserved.
//           CC BY-NC-ND - https://creativecommons.org/licenses/by-nc-nd/4.0
//
//  Created at: 2025-02-07T10:24:27.182Z
//
// --------------------------------------------------------------------------------------

#endregion

using System.Globalization;

using Microsoft.Extensions.Configuration;

using CoreX.abstractions.test;
using CoreX.providers;

using Moq;

using Shouldly;

namespace providers;

public class TemplateTagsBuilderTest : IClassFixture<TestFixture>
{
    private readonly TestFixture _fixture;

    public TemplateTagsBuilderTest(TestFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    public void GetStandardTagsTest()
    {
        // Arrange
        ITemplateTagsBuilder templateTagsBuilder = new TemplateTagsBuilder();
        var dict = new Dictionary<string, object?>();

        // Act
        var result = templateTagsBuilder.GetStandardTags(dict);

        // Assert
        result.ShouldNotBeNull();
        result.Count.ShouldBe(14);
        result.ContainsKey("Year").ShouldBeTrue();
        result["Year"].ShouldBe(DateTime.UtcNow.Year);

        result.ContainsKey("Month").ShouldBeTrue();
        result["Month"].ShouldBe(CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(DateTime.UtcNow.Month));

        result.ContainsKey("Day").ShouldBeTrue();
        result["Day"].ShouldBe(DateTime.UtcNow.Day);

        result.ContainsKey("DayOfWeek").ShouldBeTrue();
        result["DayOfWeek"].ShouldBe(DateTime.UtcNow.DayOfWeek);

        result.ContainsKey("Hour").ShouldBeTrue();
        result["Hour"].ShouldBe(DateTime.UtcNow.Hour);

        result.ContainsKey("Minute").ShouldBeTrue();
        result["Minute"].ShouldBe(DateTime.UtcNow.Minute);

        result.ContainsKey("Second").ShouldBeTrue();
        result["Second"].ShouldBe(DateTime.UtcNow.Second);

        result.ContainsKey("DayOfYear").ShouldBeTrue();
        result["DayOfYear"].ShouldBe(DateTime.UtcNow.DayOfYear);

        result.ContainsKey("Now").ShouldBeTrue();
        // result["Now"].ShouldBe(DateTime.UtcNow);

        result.ContainsKey("MachineName").ShouldBeTrue();
        result["MachineName"].ShouldBe(Environment.MachineName);

        result.ContainsKey("HostName").ShouldBeTrue();
        result["HostName"].ShouldBe(Environment.MachineName);

        result.ContainsKey("CurrentUser").ShouldBeTrue();
        result["CurrentUser"].ShouldBe(Environment.UserName);

        result.ContainsKey("UserDomainName").ShouldBeTrue();
        result["UserDomainName"].ShouldBe(Environment.UserDomainName);
    }

    [Fact]
    public void GetStandardTagsTest_DuplicateKey()
    {
        // Arrange
        var templateTagsBuilder = new TemplateTagsBuilder();
        var dict = new Dictionary<string, object?>();

        dict.Add("Year", 2021);

        // Act
        var result = templateTagsBuilder.GetStandardTags(dict);

        // Assert
        result.ShouldNotBeNull();
        result.Count.ShouldBe(14);

        result.ContainsKey("Year").ShouldBeTrue();
        result["Year"].ShouldBe(2021);

        result.ContainsKey("Month").ShouldBeTrue();
        result.ContainsKey("Day").ShouldBeTrue();
        result.ContainsKey("DayOfWeek").ShouldBeTrue();
        result.ContainsKey("Hour").ShouldBeTrue();
        result.ContainsKey("Minute").ShouldBeTrue();
        result.ContainsKey("Minute").ShouldBeTrue();
        result.ContainsKey("Second").ShouldBeTrue();
        result.ContainsKey("DayOfYear").ShouldBeTrue();
        result.ContainsKey("Now").ShouldBeTrue();
        result.ContainsKey("MachineName").ShouldBeTrue();
        result.ContainsKey("HostName").ShouldBeTrue();
        result.ContainsKey("CurrentUser").ShouldBeTrue();
        result.ContainsKey("UserDomainName").ShouldBeTrue();
    }
    [Fact]
    public void GetStandardTagsTest_EmptyDict()
    {
        // Arrange
        var templateTagsBuilder = new TemplateTagsBuilder();
        var dict = new Dictionary<string, object?>();

        // Act
        var result = templateTagsBuilder.GetStandardTags(dict);

        // Assert
        result.ShouldNotBeNull();
        result.Count.ShouldBe(14);

        result.ContainsKey("Year").ShouldBeTrue();
        result.ContainsKey("Month").ShouldBeTrue();
        result.ContainsKey("Day").ShouldBeTrue();
        result.ContainsKey("DayOfWeek").ShouldBeTrue();
        result.ContainsKey("Hour").ShouldBeTrue();
        result.ContainsKey("Minute").ShouldBeTrue();
        result.ContainsKey("Second").ShouldBeTrue();
        result.ContainsKey("DayOfYear").ShouldBeTrue();
        result.ContainsKey("Now").ShouldBeTrue();
        result.ContainsKey("MachineName").ShouldBeTrue();
        result.ContainsKey("HostName").ShouldBeTrue();
        result.ContainsKey("CurrentUser").ShouldBeTrue();
        result.ContainsKey("UserDomainName").ShouldBeTrue();
    }

    [Fact]
    public void GetStandardTagsTest_NullDict()
    {
        // Arrange
        var templateTagsBuilder = new TemplateTagsBuilder();

        // Act
        var result = templateTagsBuilder.GetStandardTags(null!);

        // Assert
        result.ShouldNotBeNull();
        result.Count.ShouldBe(14);

        result.ContainsKey("Year").ShouldBeTrue();
        result.ContainsKey("Month").ShouldBeTrue();
        result.ContainsKey("Day").ShouldBeTrue();
        result.ContainsKey("DayOfWeek").ShouldBeTrue();
        result.ContainsKey("Hour").ShouldBeTrue();
        result.ContainsKey("Minute").ShouldBeTrue();
        result.ContainsKey("Second").ShouldBeTrue();
        result.ContainsKey("DayOfYear").ShouldBeTrue();
        result.ContainsKey("Now").ShouldBeTrue();
        result.ContainsKey("MachineName").ShouldBeTrue();
        result.ContainsKey("HostName").ShouldBeTrue();
        result.ContainsKey("CurrentUser").ShouldBeTrue();
        result.ContainsKey("UserDomainName").ShouldBeTrue();
    }

    [Fact]
    public void Parse_Text_Ok()
    {
        // Arrange
        var templateTagsBuilder = new TemplateTagsBuilder();
        var expression = "${Year}";
        var tagValues = templateTagsBuilder.GetStandardTags();

        // Act
        var result = templateTagsBuilder.Parse(expression, tagValues, exceptionNotResolved: true);

        // Assert
        result.ShouldBe(DateTime.UtcNow.Year.ToString());
    }

    [Fact]
    public void Parse_Int_Ok()
    {
        // Arrange
        var templateTagsBuilder = new TemplateTagsBuilder();
        var expression = "${number}";
        var tagValues = templateTagsBuilder.GetStandardTags();
        tagValues.Add("number", 21);

        // Act
        var result = templateTagsBuilder.Parse(expression, tagValues, exceptionNotResolved: true);

        // Assert
        result.ShouldBe("21");
    }

    [Fact]
    public void Parse_Int_Ok_WithFormat()
    {
        // Arrange
        var templateTagsBuilder = new TemplateTagsBuilder();
        var expression = "${number:[format:000]}";
        var tagValues = templateTagsBuilder.GetStandardTags();
        tagValues.Add("number", 21);

        // Act
        var result = templateTagsBuilder.Parse(expression, tagValues, exceptionNotResolved: true);

        // Assert
        result.ShouldBe("021");
    }

    [Fact]
    public void Parse_DateTime_Ok()
    {
        // Arrange
        var templateTagsBuilder = new TemplateTagsBuilder();
        var expression = "${datetime}";
        var tagValues = templateTagsBuilder.GetStandardTags();
        tagValues.Add("datetime", DateTime.UtcNow);

        // Act
        var result = templateTagsBuilder.Parse(expression, tagValues, exceptionNotResolved: true);

        // Assert
        result.ShouldBe(DateTime.UtcNow.ToString());
    }

    [Fact]
    public void Parse_DateTime_Ok_WithFormat()
    {
        // Arrange
        var templateTagsBuilder = new TemplateTagsBuilder();
        var expression = "${datetime:[format:yyyy*MM*dd]}";
        var tagValues = templateTagsBuilder.GetStandardTags();
        tagValues.Add("datetime", DateTime.UtcNow);

        // Act
        var result = templateTagsBuilder.Parse(expression, tagValues, exceptionNotResolved: true);

        // Assert
        result.ShouldBe(DateTime.UtcNow.ToString("yyyy*MM*dd"));
    }

    [Fact]
    public void Parse_Boolean_Ok()
    {
        // Arrange
        var templateTagsBuilder = new TemplateTagsBuilder();
        var expression = "${enabled}";
        var tagValues = templateTagsBuilder.GetStandardTags();
        tagValues.Add("enabled", true);

        // Act
        var result = templateTagsBuilder.Parse(expression, tagValues, exceptionNotResolved: true);

        // Assert
        result.ShouldBe(true.ToString().ToLower());
    }

    [Fact]
    public void Parse_NotResolved_WhenExceptionIsTrue()
    {
        // Arrange
        var templateTagsBuilder = new TemplateTagsBuilder();
        var expression = "${Year}";

        // Act
        Action todo = () => templateTagsBuilder.Parse(expression, exceptionNotResolved: true);

        // Assert
        todo.ShouldThrow<InvalidOperationException>();
    }

    [Fact]
    public void Parse_NotResolved_WhenExceptionIsFalse()
    {
        // Arrange
        var templateTagsBuilder = new TemplateTagsBuilder();
        var expression = "${Year}";

        // Act
        Action todo = () => templateTagsBuilder.Parse(expression, exceptionNotResolved: false);

        // Assert
        todo.ShouldNotThrow();
    }

    [Fact]
    public void Parse_WithCryptoProvider()
    {
        // Arrange
        var templateTagsBuilder = new TemplateTagsBuilder();
        var expression = "${Year}";
        var tagValues = templateTagsBuilder.GetStandardTags();
        Func<string, string, string, string> cryptoProvider = (key, value, expression) => value;

        // Act
        var result = templateTagsBuilder.Parse(expression, tagValues, cryptoProvider: cryptoProvider);

        // Assert
        result.ShouldBe(DateTime.UtcNow.Year.ToString());
    }

    [Fact]
    public void Parse_WithSecretKey_WithCryptoProvider()
    {
        // Arrange
        var templateTagsBuilder = new TemplateTagsBuilder();
        var expression = "${secret:variable}";
        var tagValues = templateTagsBuilder.GetStandardTags();
        Func<string, string, string, string> cryptoProvider = (key, gkey, value) => value.ToUpper();
        tagValues.Add("variable", "value");

        // Act
        var result = templateTagsBuilder.Parse(expression, tagValues, cryptoProvider: cryptoProvider);

        // Assert
        result.ShouldNotBe("secret:value".ToUpper());
    }

    [Fact]
    public void Parse_WithConfigSection()
    {
        // Arrange
        var templateTagsBuilder = new TemplateTagsBuilder();
        var expression = "${sample}";
        var tagValues = templateTagsBuilder.GetStandardTags();
        var configSection = new Mock<IConfigurationSection>();
        configSection.Setup(x => x["sample"]).Returns("2021");

        // Act
        var result = templateTagsBuilder.Parse(expression, tagValues, configSection.Object);

        // Assert
        result.ShouldBe("2021");
    }

    [Fact]
    public void Parse_WithConfigSection_WithoutKey()
    {
        // Arrange
        var templateTagsBuilder = new TemplateTagsBuilder();
        var expression = "${sample}";
        var tagValues = templateTagsBuilder.GetStandardTags();
        var configSection = new Mock<IConfigurationSection>();

        // Act
        Action action = () => templateTagsBuilder.Parse(expression, tagValues, configSection.Object, true);

        // Assert
        action.ShouldThrow<InvalidOperationException>();
    }

    [Fact]
    public void Parse_WithConfigSection_DuplicatedKey()
    {
        // Arrange
        var templateTagsBuilder = new TemplateTagsBuilder();
        var expression = "${Year}";
        var tagValues = templateTagsBuilder.GetStandardTags();
        var configSection = new Mock<IConfigurationSection>();
        configSection.Setup(x => x["Year"]).Returns("2021");

        // Act
        var result = templateTagsBuilder.Parse(expression, tagValues, configSection.Object);

        // Assert
        result.ShouldBe(DateTime.UtcNow.Year.ToString());
    }

    [Fact]
    public void Parse_DottedKeyNames()
    {
        // Arrange
        var templateTagsBuilder = new TemplateTagsBuilder();
        var expression = "${some.values}";
        var tagValues = templateTagsBuilder.GetStandardTags();
        tagValues.Add("some.values", "testing");

        // Act.
        var result = templateTagsBuilder.Parse(expression, tagValues);

        // Assert
        result.ShouldNotBeNullOrEmpty();
        result.ShouldBe("testing");
    }

    [Fact]
    public void Parse_RandomNumbers()
    {
        // Arrange
        var templateTagsBuilder = new TemplateTagsBuilder();
        var expression = "${RandomNumber:[10]}";
        var tagValues = templateTagsBuilder.GetStandardTags();

        // Act.
        for (int ndx = 0; ndx != 10; ndx++)
        {
            var result = templateTagsBuilder.Parse(expression, tagValues);

            // Assert
            result.ShouldNotBeNullOrEmpty();

            int iResult = int.Parse(result);
            iResult.ShouldBeLessThan(10);
            iResult.ShouldBeGreaterThanOrEqualTo(0);
        }
    }

    [Fact]
    public void Parse_RandomNumbers_Min()
    {
        // Arrange
        var templateTagsBuilder = new TemplateTagsBuilder();
        var expression = "${RandomNumber:[10,100]}";
        var tagValues = templateTagsBuilder.GetStandardTags();

        // Act.
        for (int ndx = 0; ndx != 10; ndx++)
        {
            var result = templateTagsBuilder.Parse(expression, tagValues);

            // Assert
            result.ShouldNotBeNullOrEmpty();

            int iResult = int.Parse(result);
            iResult.ShouldBeLessThan(100);
            iResult.ShouldBeGreaterThanOrEqualTo(10);
        }
    }

    [Fact]
    public void Parse_RandomNumbers_MinReversed()
    {
        // Arrange
        var templateTagsBuilder = new TemplateTagsBuilder();
        var expression = "${RandomNumber:[100,10]}";
        var tagValues = templateTagsBuilder.GetStandardTags();

        // Act.
        for (int ndx = 0; ndx != 10; ndx++)
        {
            var result = templateTagsBuilder.Parse(expression, tagValues);

            // Assert
            result.ShouldNotBeNullOrEmpty();

            int iResult = int.Parse(result);
            iResult.ShouldBeLessThan(100);
            iResult.ShouldBeGreaterThanOrEqualTo(10);
        }
    }

    [Fact]
    public void Parse_RandomString()
    {
        // Arrange
        var templateTagsBuilder = new TemplateTagsBuilder();
        var expression = "${RandomString:[10]}";
        var tagValues = templateTagsBuilder.GetStandardTags();

        // Act.
        var result = templateTagsBuilder.Parse(expression, tagValues);

        // Assert
        result.ShouldNotBeNullOrEmpty();
        result.Length.ShouldBe(10);
    }

    [Fact]
    public void Parse_DefaultValues_Int()
    {
        // Arrange
        var templateTagsBuilder = new TemplateTagsBuilder();
        var expression = "${sample=[default:(int)10]}";
        var tagValues = templateTagsBuilder.GetStandardTags();

        // Act.
        var result = templateTagsBuilder.Parse(expression, tagValues);

        // Assert
        result.ShouldNotBeNullOrEmpty();
        result.ShouldBe("10");
    }

    [Fact]
    public void Parse_DefaultValues_String()
    {
        // Arrange
        var templateTagsBuilder = new TemplateTagsBuilder();
        var expression = "${sample=[default:(string)data_text]}";
        var tagValues = templateTagsBuilder.GetStandardTags();

        // Act.
        var result = templateTagsBuilder.Parse(expression, tagValues);

        // Assert
        result.ShouldNotBeNullOrEmpty();
        result.ShouldBe("data_text");
    }

    [Fact]
    public void Parse_DefaultValues_DateTime()
    {
        // Arrange
        var templateTagsBuilder = new TemplateTagsBuilder();
        var expression = "${sample=[default:(datetime)2024/11/01 12:00:00]}";
        var tagValues = templateTagsBuilder.GetStandardTags();

        // Act.
        var result = templateTagsBuilder.Parse(expression, tagValues);

        // Assert
        result.ShouldNotBeNullOrEmpty();
        result.ShouldBe(DateTime.Parse("2024/11/01 12:00:00").ToString());
    }

    [Fact]
    public void Parse_DefaultValues_DateTime_WithFormat_yyyy()
    {
        // Arrange
        var templateTagsBuilder = new TemplateTagsBuilder();
        var expression = "${sample=[default:(datetime)2024/11/01 12:00:00][format:yyyy]}";
        var tagValues = templateTagsBuilder.GetStandardTags();

        // Act.
        var result = templateTagsBuilder.Parse(expression, tagValues);

        // Assert
        result.ShouldNotBeNullOrEmpty();
        result.ShouldBe("2024");
    }

    [Fact]
    public void Parse_DefaultValues_DateTime_WithFormat_MM()
    {
        // Arrange
        var templateTagsBuilder = new TemplateTagsBuilder();
        var expression = "${sample=[default:(datetime)2024/11/01 12:00:00][format:MM]}";
        var tagValues = templateTagsBuilder.GetStandardTags();

        // Act.
        var result = templateTagsBuilder.Parse(expression, tagValues);

        // Assert
        result.ShouldNotBeNullOrEmpty();
        result.ShouldBe("11");
    }

    [Theory]
    [InlineData("date")]
    [InlineData("dateonly")]
    public void Parse_DefaultValues_DateOnly(string keyFormat)
    {
        // Arrange
        var templateTagsBuilder = new TemplateTagsBuilder();
        var expression = "${sample=[default:(" + keyFormat + ")2024/11/01]}";
        var tagValues = templateTagsBuilder.GetStandardTags();

        // Act.
        var result = templateTagsBuilder.Parse(expression, tagValues);

        // Assert
        result.ShouldNotBeNullOrEmpty();
        result.ShouldBe(DateOnly.Parse("2024/11/01").ToShortDateString());
    }

    [Fact]
    public void Parse_DefaultValues_Bool_WithTrue()
    {
        // Arrange
        var templateTagsBuilder = new TemplateTagsBuilder();
        var expression = "${sample=[default:(bool)true]}";
        var tagValues = templateTagsBuilder.GetStandardTags();

        // Act.
        var result = templateTagsBuilder.Parse(expression, tagValues);

        // Assert
        result.ShouldNotBeNullOrEmpty();
        result.ShouldBe("true");
    }

    [Fact]
    public void Parse_DefaultValues_Bool_WithFalse()
    {
        // Arrange
        var templateTagsBuilder = new TemplateTagsBuilder();
        var expression = "${sample=[default:(bool)false]}";
        var tagValues = templateTagsBuilder.GetStandardTags();

        // Act.
        var result = templateTagsBuilder.Parse(expression, tagValues);

        // Assert
        result.ShouldNotBeNullOrEmpty();
        result.ShouldBe("false");
    }

    [Theory]
    [InlineData("time")]
    [InlineData("timeonly")]
    public void Parse_DefaultValues_TimeOnly(string keyFormat)
    {
        // Arrange
        var templateTagsBuilder = new TemplateTagsBuilder();
        var expression = "${sample=[default:(" + keyFormat + ")12:00:00]}";
        var tagValues = templateTagsBuilder.GetStandardTags();

        // Act.
        var result = templateTagsBuilder.Parse(expression, tagValues);

        // Assert
        result.ShouldNotBeNullOrEmpty();
        result.ShouldBe("12:00:00");
    }

    [Fact]
    public void Parse_DefaultValues_Decimal()
    {
        // Arrange
        var templateTagsBuilder = new TemplateTagsBuilder();
        var expression = "${sample=[default:(decimal)10.5]}";
        var tagValues = templateTagsBuilder.GetStandardTags();

        // Act.
        var result = templateTagsBuilder.Parse(expression, tagValues);

        // Assert
        result.ShouldNotBeNullOrEmpty();
        result.ShouldBe(10.5.ToString());
    }

    [Fact]
    public void Parse_DefaultValues_Double()
    {
        // Arrange
        var templateTagsBuilder = new TemplateTagsBuilder();
        var expression = "${sample=[default:(double)10.5]}";
        var tagValues = templateTagsBuilder.GetStandardTags();

        // Act.
        var result = templateTagsBuilder.Parse(expression, tagValues);

        // Assert
        result.ShouldNotBeNullOrEmpty();
        result.ShouldBe(10.5.ToString());
    }

    [Fact]
    public void Parse_DefaultValues_Int_Separator()
    {
        // Arrange
        var templateTagsBuilder = new TemplateTagsBuilder();
        var expression = "${sample:[default:(int)10]}";
        var tagValues = templateTagsBuilder.GetStandardTags();

        // Act. 
        var result = templateTagsBuilder.Parse(expression, tagValues);

        // Assert
        result.ShouldNotBeNullOrEmpty();
        result.ShouldBe("10");
    }

    [Fact]
    public void Parse_DefaultValues_Int_WithTag()
    {
        // Arrange
        var templateTagsBuilder = new TemplateTagsBuilder();
        var expression = "${sample=[default:(int)10]}";
        var tagValues = templateTagsBuilder.GetStandardTags();
        tagValues.Add("sample", 5);

        // Act. 
        var result = templateTagsBuilder.Parse(expression, tagValues);

        // Assert
        result.ShouldNotBeNullOrEmpty();
        result.ShouldBe("5");
    }

    [Fact]
    public void Parse_DefaultValues_Int_WithFormat()
    {
        // Arrange
        var templateTagsBuilder = new TemplateTagsBuilder();
        var expression = "${sample=[default:(int)10][format:000]}";
        var tagValues = templateTagsBuilder.GetStandardTags();

        // Act. 
        var result = templateTagsBuilder.Parse(expression, tagValues);

        // Assert
        result.ShouldNotBeNullOrEmpty();
        result.ShouldBe("010");
    }

    [Fact]
    public void Parse_DefaultValues_String_NoCast()
    {
        // Arrange
        var templateTagsBuilder = new TemplateTagsBuilder();
        var expression = "${sample=[default:asd asdasdj. / asshsd]}";
        var tagValues = templateTagsBuilder.GetStandardTags();

        // Act. 
        var result = templateTagsBuilder.Parse(expression, tagValues);

        // Assert
        result.ShouldNotBeNullOrEmpty();
        result.ShouldBe("asd asdasdj. / asshsd");
    }

    [Fact]
    public void Parse_Escaped()
    {
        // Arrange
        var templateTagsBuilder = new TemplateTagsBuilder();
        var expression = "${sample} $${{sample}}";
        var tagValues = templateTagsBuilder.GetStandardTags();

        tagValues.Add("sample", "xxxxxxxxxxxxxxx");

        // Act.
        var result = templateTagsBuilder.Parse(expression, tagValues);

        // Assert
        result.ShouldNotBeNullOrEmpty();
        result.ShouldBe("xxxxxxxxxxxxxxx ${sample}");
    }
}
