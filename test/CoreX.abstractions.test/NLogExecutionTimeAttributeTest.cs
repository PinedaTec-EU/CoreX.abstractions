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
// FileName: NLogExecutionTimeAttributeTest.cs
//
// Author:   jmr.pineda
// eMail:    jmr.pineda@pinedatec.eu
// Profile:  http://pinedatec.eu/profile
//
//           Copyrights (c) PinedaTec.eu 2025, all rights reserved.
//           CC BY-NC-ND - https://creativecommons.org/licenses/by-nc-nd/4.0
//
//  Created at: 2025-02-09T08:48:27.160Z
//
// --------------------------------------------------------------------------------------

#endregion

using System.Reflection;

using CoreX.abstractions.test;
using CoreX.aspects;

using Moq;

using NLog;

[module: NLogExecutionTimeAttribute]

namespace abstractions;

public class NLogExecutionTimeAttributeTest : IClassFixture<TestFixture>
{
    private readonly TestFixture _fixture;
    private readonly ILogger _logger = LogManager.GetCurrentClassLogger();

    public NLogExecutionTimeAttributeTest(TestFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    public void Init_ShouldLogStartMessage()
    {
        // Arrange
        var logger = new Mock<ILogger>();
        var instance = new Mock<object>();
        var method = new Mock<MethodBase>();
        var args = new object[] { };
        var sut = new NLogExecutionTimeAttribute(logger.Object);

        logger.Setup(l => l.Log(typeof(NLogExecutionTimeAttribute), It.IsAny<LogEventInfo>()))
            .Callback<Type, LogEventInfo>((t, e) =>
            {
                _logger.Log(t, e);
            });

        method.Setup(m => m.DeclaringType).Returns(typeof(NLogExecutionTimeAttribute));
        method.Setup(m => m.Name).Returns(nameof(Init_ShouldLogStartMessage));

        // Act
        sut.Init(instance.Object, method.Object, args);

        logger.Verify(l => l.Log(typeof(NLogExecutionTimeAttribute), It.IsAny<LogEventInfo>()), Times.Once);
    }

    [Fact]
    public void OnExit_ShouldLogStartMessage()
    {
        // Arrange
        var logger = new Mock<ILogger>();
        var instance = new Mock<object>();
        var method = new Mock<MethodBase>();
        var args = new object[] { };
        var sut = new NLogExecutionTimeAttribute(logger.Object);

        logger.Setup(l => l.Log(typeof(NLogExecutionTimeAttribute), It.IsAny<LogEventInfo>()))
            .Callback<Type, LogEventInfo>((t, e) =>
            {
                _logger.Log(t, e);
            });

        method.Setup(m => m.DeclaringType).Returns(typeof(NLogExecutionTimeAttribute));
        method.Setup(m => m.Name).Returns(nameof(OnExit_ShouldLogStartMessage));

        // Act
        sut.Init(instance.Object, method.Object, args);
        sut.OnExit();

        logger.Verify(l => l.Log(typeof(NLogExecutionTimeAttribute), It.IsAny<LogEventInfo>()), Times.Exactly(2));
    }

    [Fact]
    public void OnSampleMethod_ShouldLogExecutionTime()
    {
        // Arrange
        var sampleDataClass = new SampleDataClass();

        sampleDataClass.SampleMethod();
    }
}

public class SampleDataClass
{
    [NLogExecutionTimeAttribute]
    public void SampleMethod()
    {
        // Do nothing
        Task.Delay(1000).Wait();
    }
}

[AttributeUsage(AttributeTargets.Method)]
public class SampleDecorator : NLogExecutionTimeAttribute
{
}