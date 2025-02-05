using System.Text;
using ark.abstractions.test;
using ark.extensions;

using Shouldly;

namespace framework;

public class AppsHelperTest
{
    [Fact]
    public void IsNullOrEmpty_Null_Valid()
    {
        var isNullOrEmpty = AppsHelper.IsNullOrEmpty(null);

        isNullOrEmpty.ShouldBeTrue();
    }

    [Fact]
    public void IsNullOrEmpty_Empty_Valid()
    {
        var isNullOrEmpty = AppsHelper.IsNullOrEmpty("");

        isNullOrEmpty.ShouldBeTrue();
    }

    [Fact]
    public void IsNullOrEmpty_NotEmpty_Valid()
    {
        var isNullOrEmpty = AppsHelper.IsNullOrEmpty("not empty");

        isNullOrEmpty.ShouldBeFalse();
    }

    [Fact]
    public void IsNullOrEmpty_WhiteSpace_Valid()
    {
        var isNullOrEmpty = AppsHelper.IsNullOrEmpty(" ");

        isNullOrEmpty.ShouldBeFalse();
    }

    [Fact]
    public void IsNullOrEmpty_IEnumerable_Empty()
    {
        var isNullOrEmpty = AppsHelper.IsNullOrEmpty(new List<string>());

        isNullOrEmpty.ShouldBeTrue();
    }

    [Fact]
    public void IsNullOrEmpty_IEnumerable_NotEmpty()
    {
        var isNullOrEmpty = AppsHelper.IsNullOrEmpty(new List<string> { "not empty" });

        isNullOrEmpty.ShouldBeFalse();
    }

    [Fact]
    public void IsNullOrEmpty_IEnumerable_Null()
    {
        var isNullOrEmpty = AppsHelper.IsNullOrEmpty((IEnumerable<string>)null!);

        isNullOrEmpty.ShouldBeTrue();
    }

    [Fact]
    public void IsNullOrEmpty_IDictionary_Empty()
    {
        var isNullOrEmpty = AppsHelper.IsNullOrEmpty(new Dictionary<string, string>());

        isNullOrEmpty.ShouldBeTrue();
    }

    [Fact]
    public void IsNullOrEmpty_IDictionary_NotEmpty()
    {
        var isNullOrEmpty = AppsHelper.IsNullOrEmpty(new Dictionary<string, string> { { "key", "value" } });

        isNullOrEmpty.ShouldBeFalse();
    }

    [Fact]
    public void IsNullOrEmpty_IDictionary_Null()
    {
        var isNullOrEmpty = AppsHelper.IsNullOrEmpty((IDictionary<string, string>)null!);

        isNullOrEmpty.ShouldBeTrue();
    }


    [Fact]
    public void ThrowArgumentExceptionIfNullOrEmpty_Valid()
    {
        Action act = () => AppsHelper.ThrowArgumentExceptionIfNullOrEmpty(null as string, "paramName");

        act.ShouldThrow<ArgumentException>();
    }

    [Fact]
    public void ThrowArgumentExceptionIfNullOrEmpty_Array_Valid()
    {
        Action act = () => AppsHelper.ThrowArgumentExceptionIfNullOrEmpty(null as Array, "paramName");

        act.ShouldThrow<ArgumentException>();
    }

    [Fact]
    public void ThrowArgumentExceptionIfNullOrEmpty_Valid_StringEmpty()
    {
        Action act = () => AppsHelper.ThrowArgumentExceptionIfNullOrEmpty("", "paramName");

        act.ShouldThrow<ArgumentException>();
    }

    [Fact]
    public void ThrowArgumentExceptionIfNullOrEmpty_Array_Valid_StringEmpty()
    {
        Action act = () => AppsHelper.ThrowArgumentExceptionIfNullOrEmpty(Array.Empty<string>(), "paramName");

        act.ShouldThrow<ArgumentException>();
    }

    [Fact]
    public void ThrowArgumentExceptionIfNullOrEmpty_Invalid()
    {
        Action act = () => AppsHelper.ThrowArgumentExceptionIfNullOrEmpty("not empty", "paramName");

        act.ShouldNotThrow();
    }

    [Fact]
    public void ThrowExceptionIfNullOrEmpty_Valid()
    {
        Action act = () => AppsHelper.ThrowExceptionIfNullOrEmpty(null as string, "paramName");

        act.ShouldThrow<Exception>();
    }

    [Fact]
    public void ThrowExceptionIfNullOrEmpty_Valid_StringEmpty()
    {
        Action act = () => AppsHelper.ThrowExceptionIfNullOrEmpty("", "paramName");

        act.ShouldThrow<Exception>();
    }

    [Fact]
    public void ThrowExceptionIfNullOrEmpty_Invalid()
    {
        Action act = () => AppsHelper.ThrowExceptionIfNullOrEmpty("not empty", "paramName");

        act.ShouldNotThrow();
    }

    [Fact]
    public void ThrowExceptionIfNullOrEmpty_Array_Valid()
    {
        Action act = () => AppsHelper.ThrowExceptionIfNullOrEmpty(null as string[], "paramName");

        act.ShouldThrow<Exception>();
    }

    [Fact]
    public void ThrowExceptionIfNullOrEmpty_Array_Valid_Empty()
    {
        Action act = () => AppsHelper.ThrowExceptionIfNullOrEmpty(new string[0], "paramName");

        act.ShouldThrow<Exception>();
    }

    [Fact]
    public void ThrowExceptionIfNullOrEmpty_Array_Invalid()
    {
        Action act = () => AppsHelper.ThrowExceptionIfNullOrEmpty(new string[] { "not empty" }, "paramName");

        act.ShouldNotThrow();
    }

    [Fact]
    public void ThrowExceptionIfNullOrEmpty_TException_Valid()
    {
        Action act = () => AppsHelper.ThrowExceptionIfNullOrEmpty<ArgumentException>(null as string, "paramName");

        act.ShouldThrow<ArgumentException>();
    }

    [Fact]
    public void ThrowExceptionIfNullOrEmpty_TException_Invalid()
    {
        Action act = () => AppsHelper.ThrowExceptionIfNullOrEmpty<ArgumentException>("not empty", "paramName");

        act.ShouldNotThrow();
    }

    [Fact]
    public void ThrowExceptionIfNotNullOrEmpty_TException_Valid()
    {
        Action act = () => AppsHelper.ThrowExceptionIfNotNullOrEmpty<ArgumentException>("not empty", "paramName");

        act.ShouldThrow<ArgumentException>();
    }

    [Fact]
    public void ThrowExceptionIfNotNullOrEmpty_TException_Invalid()
    {
        Action act = () => AppsHelper.ThrowExceptionIfNotNullOrEmpty<ArgumentException>(null as string, "paramName");

        act.ShouldNotThrow();
    }

    [Fact]
    public void ThrowExceptionIfNullOrEmpty_Array_TException_Invalid()
    {
        Action act = () => AppsHelper.ThrowExceptionIfNullOrEmpty<ArgumentException>(new string[] { "not empty" }, "paramName");

        act.ShouldNotThrow();
    }

    [Fact]
    public void ThrowExceptionIfNullOrEmpty_Array_TException_Valid()
    {
        Action act = () => AppsHelper.ThrowExceptionIfNullOrEmpty<ArgumentException>(null as string[], "paramName");

        act.ShouldThrow<ArgumentException>();
    }

    [Fact]
    public void ThrowArgumentExceptionIfNull_Valid()
    {
        Action act = () => AppsHelper.ThrowArgumentExceptionIfNull(null as object, "paramName");

        act.ShouldThrow<ArgumentException>();
    }

    [Fact]
    public void ThrowArgumentExceptionIfNull_Invalid()
    {
        Action act = () => AppsHelper.ThrowArgumentExceptionIfNull("not null", "paramName");

        act.ShouldNotThrow();
    }

    [Fact]
    public void ThrowExceptionIfNull_Valid()
    {
        Action act = () => AppsHelper.ThrowExceptionIfNull(null as string, "paramName");

        act.ShouldThrow<Exception>();
    }

    [Fact]
    public void ThrowExceptionIfNull_Invalid()
    {
        Action act = () => AppsHelper.ThrowExceptionIfNull("not null", "paramName");

        act.ShouldNotThrow();
    }

    [Fact]
    public void ThrowExceptionIfNull_TException_Valid()
    {
        Action act = () => AppsHelper.ThrowExceptionIfNull<TestException>(null, "paramName");

        act.ShouldThrow<TestException>();
    }

    [Fact]
    public void ThrowExceptionIfNull_TException_Invalid()
    {
        Action act = () => AppsHelper.ThrowExceptionIfNull<ArgumentException>("not null", "paramName");

        act.ShouldNotThrow();
    }

    [Fact]
    public void ThrowExceptionIfNotNull_TException_Valid()
    {
        Action act = () => AppsHelper.ThrowExceptionIfNotNull<ArgumentException>("not null", "paramName");

        act.ShouldThrow<ArgumentException>();
    }

    [Fact]
    public void ThrowExceptionIfNotNull_TException_Invalid()
    {
        Action act = () => AppsHelper.ThrowExceptionIfNotNull<ArgumentException>(null, "paramName");

        act.ShouldNotThrow();
    }

    [Fact]
    public void ThrowArgumentExceptionIfFalse_Valid()
    {
        Action act = () => AppsHelper.ThrowArgumentExceptionIfFalse(false, "paramName", "message");

        act.ShouldThrow<ArgumentException>();
    }

    [Fact]
    public void ThrowArgumentExceptionIfFalse_Invalid()
    {
        Action act = () => AppsHelper.ThrowArgumentExceptionIfFalse(true, "paramName", "message");

        act.ShouldNotThrow();
    }

    [Fact]
    public void ThrowArgumentExceptionIfTrue_Valid()
    {
        Action act = () => AppsHelper.ThrowArgumentExceptionIfTrue(true, "paramName", "message");

        act.ShouldThrow<ArgumentException>();
    }

    [Fact]
    public void ThrowArgumentExceptionIfTrue_Invalid()
    {
        Action act = () => AppsHelper.ThrowArgumentExceptionIfTrue(false, "paramName", "message");

        act.ShouldNotThrow();
    }

    [Fact]
    public void ThrowExceptionIfFalse_TException_Valid()
    {
        Action act = () => AppsHelper.ThrowExceptionIfFalse<ArgumentException>(false, "paramName");

        act.ShouldThrow<ArgumentException>();
    }

    [Fact]
    public void ThrowExceptionIfFalse_TException_Invalid()
    {
        Action act = () => AppsHelper.ThrowExceptionIfFalse<ArgumentException>(true, "paramName");

        act.ShouldNotThrow();
    }

    [Fact]
    public void ThrowExceptionIfTrue_TException_Valid()
    {
        Action act = () => AppsHelper.ThrowExceptionIfTrue<ArgumentException>(true, "paramName");

        act.ShouldThrow<ArgumentException>();
    }

    [Fact]
    public void ThrowExceptionIfTrue_TException_Invalid()
    {
        Action act = () => AppsHelper.ThrowExceptionIfTrue<ArgumentException>(false, "paramName");

        act.ShouldNotThrow();
    }

    [Fact]
    public void GetText_Valid()
    {
        StringBuilder sb = new();
        sb.AppendLine("key=value");
        var text = AppsHelper.GetText(sb);

        text.ShouldNotBeEmpty();
        text.ShouldBe("key=value");
    }

    [Fact]
    public void GetText_Null()
    {
        var text = AppsHelper.GetText(null!);

        text.ShouldBeNull();
    }

    [Fact]
    public void GetText_RemoveEndCRLF()
    {
        StringBuilder sb = new();
        sb.AppendLine("key=value");
        sb.AppendLine("key2=value2");
        var text = AppsHelper.GetText(sb, true);

        text.ShouldNotBeEmpty();
        text.ShouldBe("key=value\nkey2=value2");
    }

    [Fact]
    public void GetText_NotRemoveEndCRLF()
    {
        StringBuilder sb = new();
        sb.AppendLine("key=value");
        sb.AppendLine("key2=value2");
        var text = AppsHelper.GetText(sb, false);

        text.ShouldNotBeEmpty();
        text.ShouldBe("key=value\nkey2=value2\n");
    }

    [Fact]
    public void GetText_RemoveCRLF_Null()
    {
        var text = AppsHelper.GetText(null!, true);

        text.ShouldBeNull();
    }

    [Fact]
    public void Merge_Dictionary_Valid()
    {
        var dict1 = new Dictionary<string, string> { { "key", "value" } };
        var dict2 = new Dictionary<string, string> { { "key2", "value2" } };

        var merged = AppsHelper.Merge(dict1, dict2);
        merged.ShouldNotBeNull();
        merged.Count.ShouldBe(2);
    }

    [Fact]
    public void Merge_Dictionary1_Null()
    {
        var dict1 = new Dictionary<string, string> { { "key", "value" } };

        var merged = AppsHelper.Merge(dict1, null);
        merged.ShouldNotBeNull();
        merged.Count.ShouldBe(1);
    }

    [Fact]
    public void Merge_Dictionary2_Null()
    {
        var dict2 = new Dictionary<string, string> { { "key", "value" } };

        var merged = AppsHelper.Merge(null!, dict2);
        merged.ShouldNotBeNull();
        merged.Count.ShouldBe(1);
    }

    [Fact]
    public void Merge_Dictionary_Null()
    {
        var merged = AppsHelper.Merge<string, string>(null!, null);
        merged.ShouldBeNull();
    }

    [Fact(Skip = "POC")]
    public void CreateCancellationTokenSource_Case01()
    {
        CancellationTokenSource externalCTS = new CancellationTokenSource();
        CancellationTokenSource internalCTS = new CancellationTokenSource();
        var linkedCTS = CancellationTokenSource.CreateLinkedTokenSource(externalCTS.Token, internalCTS.Token);

        linkedCTS.Cancel();

        linkedCTS.IsCancellationRequested.ShouldBeTrue();
        externalCTS.IsCancellationRequested.ShouldBeFalse();
        internalCTS.IsCancellationRequested.ShouldBeFalse();
    }

    [Fact(Skip = "POC")]
    public void CreateCancellationTokenSource_Case02()
    {
        CancellationTokenSource externalCTS = new CancellationTokenSource();
        CancellationTokenSource internalCTS = new CancellationTokenSource();
        var linkedCTS = CancellationTokenSource.CreateLinkedTokenSource(externalCTS.Token, internalCTS.Token);

        externalCTS.Cancel();

        linkedCTS.IsCancellationRequested.ShouldBeTrue();
        externalCTS.IsCancellationRequested.ShouldBeTrue();
        internalCTS.IsCancellationRequested.ShouldBeFalse();
    }

    [Fact(Skip = "POC")]
    public void CreateCancellationTokenSource_Case03()
    {
        CancellationTokenSource externalCTS = new CancellationTokenSource();
        CancellationTokenSource internalCTS = new CancellationTokenSource();
        var linkedCTS = CancellationTokenSource.CreateLinkedTokenSource(externalCTS.Token, internalCTS.Token);

        internalCTS.Cancel();

        linkedCTS.IsCancellationRequested.ShouldBeTrue();
        externalCTS.IsCancellationRequested.ShouldBeFalse();
        internalCTS.IsCancellationRequested.ShouldBeTrue();
    }
}
