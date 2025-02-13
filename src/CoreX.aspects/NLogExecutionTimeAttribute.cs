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
// FileName: NLogExecutionTimeAttribute.cs
//
// Author:   jmr.pineda
// eMail:    jmr.pineda@pinedatec.eu
// Profile:  http://pinedatec.eu/profile
//
//           Copyrights (c) PinedaTec.eu 2025, all rights reserved.
//           CC BY-NC-ND - https://creativecommons.org/licenses/by-nc-nd/4.0
//
//  Created at: 2025-02-09T08:48:27.159Z
//
// --------------------------------------------------------------------------------------

#endregion

using System.Diagnostics;
using System.Reflection;

using MethodDecorator.Fody.Interfaces;

using NLog;

namespace CoreX.aspects;

/// <summary>
/// Aspect that logs the execution time of a method.
/// </summary>
[AttributeUsage(AttributeTargets.Method | AttributeTargets.Constructor | AttributeTargets.Module, AllowMultiple = false, Inherited = true)]
public class NLogExecutionTimeAttribute : Attribute, IMethodDecorator
{
    private static readonly ILogger _sLogger = LogManager.GetCurrentClassLogger();

    private readonly ILogger _logger = _sLogger;

    private Stopwatch? _stopwatch;

    private string _letId = Ulid.NewUlid().ToString();
    private string _methodDeclaringType = string.Empty;
    private string _methodName = string.Empty;

    private LogLevel _level = LogLevel.Trace;
    private bool _logOnInit = true;
    private bool _logOnExit = true;
    private bool _logOnException = true;
    private bool _logOnEntry = false;

    /// <summary>
    /// Initializes a new instance of the <see cref="NLogExecutionTimeAttribute"/> class.
    /// </summary>
    /// <param name="logger">Custom logger to use</param>
    /// <param name="level">Log level to use when required</param>
    /// <param name="logOnInit">When unset, disables the log for OnInit method</param>
    /// <param name="logOnEntry">When unset, disables the log for OnEntry method</param>
    /// <param name="logOnExit">When unset, disables the log for the OnExit method</param>
    /// <param name="logOnException">When unset, disables the log for theOnException method</param>
    public NLogExecutionTimeAttribute(ILogger logger, LogLevel level, bool logOnInit = true, bool logOnEntry = false, bool logOnExit = true, bool logOnException = true)
     : this(level, logOnInit, logOnEntry, logOnExit, logOnException)
    {
        _logger = logger;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="NLogExecutionTimeAttribute"/> class. 
    /// </summary>
    /// <param name="logger">Custom logger to use</param>
    /// <param name="logOnInit">When unset, disables the log for OnInit method</param>
    /// <param name="logOnEntry">When unset, disables the log for OnEntry method</param>
    /// <param name="logOnExit">When unset, disables the log for the OnExit method</param>
    /// <param name="logOnException">When unset, disables the log for theOnException method</param>
    public NLogExecutionTimeAttribute(ILogger logger, bool logOnInit = true, bool logOnEntry = false, bool logOnExit = true, bool logOnException = true)
        : this(logOnInit, logOnEntry, logOnExit, logOnException)
    {
        _logger = logger;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="NLogExecutionTimeAttribute"/> class. 
    /// </summary>
    /// <param name="logOnInit">When unset, disables the log for OnInit method</param>
    /// <param name="logOnEntry">When unset, disables the log for OnEntry method</param>
    /// <param name="logOnExit">When unset, disables the log for the OnExit method</param>
    /// <param name="logOnException">When unset, disables the log for theOnException method</param>
    public NLogExecutionTimeAttribute(bool logOnInit = true, bool logOnEntry = false, bool logOnExit = true, bool logOnException = true)
        : this(LogLevel.Trace, logOnInit, logOnEntry, logOnExit, logOnException)
    { }

    /// <summary>
    /// Initializes a new instance of the <see cref="NLogExecutionTimeAttribute"/> class.
    /// </summary>
    /// <param name="level">Log level to use when required</param>
    /// <param name="logOnInit">When unset, disables the log for OnInit method</param>
    /// <param name="logOnEntry">When unset, disables the log for OnEntry method</param>
    /// <param name="logOnExit">When unset, disables the log for the OnExit method</param>
    /// <param name="logOnException">When unset, disables the log for theOnException method</param> 
    public NLogExecutionTimeAttribute(LogLevel level, bool logOnInit = true, bool logOnEntry = false, bool logOnExit = true, bool logOnException = true)
    {
        _level = level;
        _logOnInit = logOnInit;
        _logOnEntry = logOnEntry;
        _logOnExit = logOnExit;
        _logOnException = logOnException;
    }

    public void Init(object instance, MethodBase method, object[] args)
    {
        _stopwatch = Stopwatch.StartNew();
        _methodDeclaringType = method.DeclaringType!.Name;
        _methodName = method.Name;

        if (!_logOnInit)
        {
            return;
        }

        LogEventInfo logEvent;
        var message = $"[{_letId}] Operation [{_methodDeclaringType}.{_methodName}] executing";
        logEvent = new LogEventInfo(_level, _logger.Name, message);

        _logger.Log(typeof(NLogExecutionTimeAttribute), logEvent);
    }

    public void OnEntry()
    {
        if (!_logOnEntry)
        {
            return;
        }

        LogEventInfo logEvent;
        var message = $"[{_letId}] Operation [{_methodDeclaringType}.{_methodName}] started";
        logEvent = new LogEventInfo(_level, _logger.Name, message);

        _logger.Log(typeof(NLogExecutionTimeAttribute), logEvent);
    }

    public void OnExit()
    {
        _stopwatch!.Stop();

        if (!_logOnExit)
        {
            return;
        }

        var message = $"[{_letId}] Operation [{_methodDeclaringType}.{_methodName}] completed in {_stopwatch.ElapsedMilliseconds} ms";
        LogEventInfo logEvent = new LogEventInfo(_level, _logger.Name, message);

        _logger.Log(typeof(NLogExecutionTimeAttribute), logEvent);
    }

    // #pragma warning disable CA1857 // A constant is expected for the parameter

    public void OnException(Exception exception)
    {
        _stopwatch!.Stop();

        if (!_logOnException)
        {
            return;
        }

        var message = $"[{_letId}] Operation [{_methodDeclaringType}.{_methodName}] completed in {_stopwatch.ElapsedMilliseconds} ms:\n{exception.Message}";
        LogEventInfo logEvent = new LogEventInfo(LogLevel.Error, _logger.Name, message);

        _logger.Log(typeof(NLogExecutionTimeAttribute), logEvent);
        //         var componentException = args.Exception as ComponentException;

        //         if (_reportOnErrorEnabled || (componentException != null && !componentException.IsReported))
        //         {
        //             var genComponent = args.Instance as IGenericComponent;

        //             if (genComponent is not null)
        //             {
        //                 genComponent.Logger.ReportError(args.Exception, caller: args.Method.Name);
        //                 return;
        //             }

        //             _logger.ReportError(args.Exception, caller: args.Method.Name);
        //         }
    }

    // #pragma warning restore CA1857 // A constant is expected for the parameter
}
