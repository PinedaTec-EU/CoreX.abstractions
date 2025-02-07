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
// FileName: AppsHelper.cs
//
// Author:   jmr.pineda
// eMail:    jmr.pineda@pinedatec.eu
// Profile:  http://pinedatec.eu/profile
//
//           Copyrights (c) PinedaTec.eu 2025, all rights reserved.
//           CC BY-NC-ND - https://creativecommons.org/licenses/by-nc-nd/4.0
//
//  Created at: 2025-02-07T00:41:48.094Z
//
// --------------------------------------------------------------------------------------

#endregion

using System.Collections;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using System.Net;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;

using NLog;

namespace ark.extensions;

[DebuggerStepThrough]
public static class AppsHelper
{
    private static readonly ILogger _logger = LogManager.GetCurrentClassLogger();

    public static bool IsNullOrEmpty([NotNullWhen(false)] this string? value) =>
        string.IsNullOrEmpty(value);

    public static bool IsNullOrEmpty<T>([NotNullWhen(false)] this IEnumerable<T>? value) =>
        value is null || !value.Any();

    public static bool IsNullOrEmpty<T>([NotNullWhen(false)] this ICollection<T>? value) =>
        value is null || value.Count == 0;

    public static bool IsNullOrEmpty<T, W>([NotNullWhen(false)] this IDictionary<T, W>? value) =>
        value is null || value.Count == 0;

    /// <summary>
    /// Throws an <see cref="ArgumentNullException"/> if the value is null or empty
    /// </summary>
    /// <param name="value"></param>
    /// <param name="message"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    public static string ThrowArgumentExceptionIfNullOrEmpty([NotNull] this string? value, string message)
    {
        if (value is null || value.IsNullOrEmpty())
        {
            var exp = new ArgumentNullException(message);
            _logger.Error(exp, message);

            throw exp;
        }

        return value;
    }

    /// <summary>
    /// Throws an <see cref="ArgumentNullException"/> if the value is null or empty
    /// </summary>
    /// <param name="value"></param>
    /// <param name="message"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    public static Array ThrowArgumentExceptionIfNullOrEmpty([NotNull] this Array? value, string message)
    {
        if (value is null || value.Length == 0)
        {
            var exp = new ArgumentNullException(message);
            _logger.Error(exp, message);

            throw exp;
        }

        return value;
    }

    /// <summary>
    /// Throws an <see cref="NullReferenceException"/> if the value is null or empty
    /// </summary>
    /// <param name="value"></param>
    /// <param name="message"></param>
    /// <returns></returns>
    /// <exception cref="NullReferenceException"></exception>
    public static string ThrowExceptionIfNullOrEmpty([NotNull] this string? value, string message)
    {
        if (value is null || value.IsNullOrEmpty())
        {
            var exp = new NullReferenceException(message);
            _logger.Error(exp, message);

            throw exp;
        }

        return value;
    }

    /// <summary>
    /// Throws an <see cref="NullReferenceException"/> if the value is null or empty
    /// </summary>
    /// <param name="value"></param>
    /// <param name="message"></param>
    /// <returns></returns>
    /// <exception cref="NullReferenceException"></exception>
    public static Array ThrowExceptionIfNullOrEmpty([NotNull] this Array? value, string message)
    {
        if (value is null || value.Length == 0)
        {
            var exp = new NullReferenceException(message);
            _logger.Error(exp, message);

            throw exp;
        }

        return value;
    }

    /// <summary>
    /// Throws an exception of type <typeparamref name="TException"/> if the value is null or empty
    /// </summary>
    /// <typeparam name="TException"></typeparam>
    /// <param name="value"></param>
    /// <param name="message"></param>
    /// <returns></returns>
    public static string ThrowExceptionIfNullOrEmpty<TException>([NotNull] this string? value, string message)
        where TException : Exception
    {
        if (value is null || value.IsNullOrEmpty())
        {
            var exp = (TException)Activator.CreateInstance(typeof(TException), message)!;

            _logger.Error(exp, message);
            throw exp;
        }

        return value;
    }

    /// <summary>
    /// Throws an exception of type <typeparamref name="TException"/> if the value is null or empty
    /// </summary>
    /// <typeparam name="TException"></typeparam>
    /// <param name="value"></param>
    /// <param name="message"></param>
    /// <returns></returns>
    public static Array ThrowExceptionIfNullOrEmpty<TException>([NotNull] this Array? value, string message)
        where TException : Exception
    {
        if (value is null || value.Length == 0)
        {
            var exp = (TException)Activator.CreateInstance(typeof(TException), message)!;

            _logger.Error(exp, message);
            throw exp;
        }

        return value;
    }

    /// <summary>
    /// Throws an <see cref="ArgumentNullException"/> if the value is not null or empty
    /// </summary>
    /// <typeparam name="TException"></typeparam>
    /// <param name="value"></param>
    /// <param name="message"></param>
    /// <returns></returns>
    public static string? ThrowExceptionIfNotNullOrEmpty<TException>([NotNullWhen(false)] this string? value, string message)
        where TException : Exception
    {
        if (!(value is null || value.IsNullOrEmpty()))
        {
            var exp = (TException)Activator.CreateInstance(typeof(TException), message)!;

            _logger.Error(exp, message);
            throw exp;
        }

        return value;
    }

    /// <summary>
    /// Throws an <see cref="ArgumentNullException"/> if the value is null
    /// </summary>
    /// <typeparam name="TObject"></typeparam>
    /// <param name="value"></param>
    /// <param name="message"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    public static TObject ThrowArgumentExceptionIfNull<TObject>([NotNull][DoesNotReturnIf(false)] this TObject? value, string message)
    {
        if (value is null)
        {
            var exp = new ArgumentNullException(message);

            _logger.Error(exp, message);
            throw exp;
        }

        return value!;
    }

    /// <summary>
    /// Throws an <see cref="NullReferenceException"/> if the value is null
    /// </summary>
    /// <param name="value"></param>
    /// <param name="message"></param>
    /// <returns></returns>
    /// <exception cref="NullReferenceException"></exception>
    public static object ThrowExceptionIfNull([NotNull] this object? value, string message)
    {
        if (value is null)
        {
            var exp = new NullReferenceException(message);

            _logger.Error(exp, message);
            throw exp;
        }

        return value!;
    }

    /// <summary>
    /// Throws an exception of type <typeparamref name="TException"/> if the value is null
    /// </summary>
    /// <typeparam name="TException"></typeparam>
    /// <param name="value"></param>
    /// <param name="message"></param>
    /// <returns></returns>
    public static object ThrowExceptionIfNull<TException>([NotNull] this object? value, string message)
        where TException : Exception
    {
        if (value is null)
        {
            var exp = (TException)Activator.CreateInstance(typeof(TException), message)!;

            _logger.Error(exp, message);
            throw exp;
        }

        return value;
    }

    /// <summary>
    /// Throws an exception of type <typeparamref name="TException"/> if the value is not null
    /// </summary>
    /// <typeparam name="TException"></typeparam>
    /// <param name="value"></param>
    /// <param name="message"></param>
    /// <returns></returns>
    public static object? ThrowExceptionIfNotNull<TException>([NotNullWhen(false)] this object? value, string message)
        where TException : Exception
    {
        if (value is not null)
        {
            var exp = (TException)Activator.CreateInstance(typeof(TException), message)!;

            _logger.Error(exp, message);
            throw exp;
        }

        return value;
    }

    /// <summary>
    /// Throws an <see cref="ArgumentException"/> if the value is false
    /// </summary>
    /// <param name="value"></param>
    /// <param name="varName"></param>
    /// <param name="message"></param>
    public static void ThrowArgumentExceptionIfFalse([DoesNotReturnIf(false)] this bool value, string varName, string message) =>
        (!value).ThrowArgumentExceptionIfTrue(varName, message);

    /// <summary>
    /// Throws an <see cref="ArgumentException"/> if the value is false
    /// </summary>
    /// <param name="value"></param>
    /// <param name="varName"></param>
    /// <param name="message"></param>
    public static void ThrowArgumentExceptionIfFalse([DoesNotReturnIf(false)] this Func<bool> value, string varName, string message) =>
        (!value()).ThrowArgumentExceptionIfTrue(varName, message);

    /// <summary>
    /// Throws an <see cref="ArgumentException"/> if the value is true
    /// </summary>
    /// <param name="value"></param>
    /// <param name="varName"></param>
    /// <param name="message"></param>
    public static void ThrowArgumentExceptionIfTrue([DoesNotReturnIf(true)] this Func<bool> value, string varName, string message) =>
        value().ThrowArgumentExceptionIfTrue(varName, message);

    /// <summary>
    /// Throws an <see cref="ArgumentException"/> if the value is true
    /// </summary>
    /// <param name="value"></param>
    /// <param name="varName"></param>
    /// <param name="message"></param>
    /// <exception cref="ArgumentException"></exception>
    public static void ThrowArgumentExceptionIfTrue([DoesNotReturnIf(true)] this bool value, string varName, string message)
    {
        if (value)
        {
            var exp = new ArgumentException($"[{varName}]: {message}");

            _logger.Error(exp, message);
            throw exp;
        }
    }

    /// <summary>
    /// Throws an exception of type <typeparamref name="TException"/> if the value is false
    /// </summary>
    /// <typeparam name="TException"></typeparam>
    /// <param name="value"></param>
    /// <param name="message"></param>
    public static void ThrowExceptionIfFalse<TException>([DoesNotReturnIf(false)] this bool value, string message) where TException : Exception =>
        (!value).ThrowExceptionIfTrue<TException>(message);

    /// <summary>
    /// Throws an exception of type <typeparamref name="TException"/> if the value is true
    /// </summary>
    /// <typeparam name="TException"></typeparam>
    /// <param name="value"></param>
    /// <param name="message"></param>
    public static void ThrowExceptionIfTrue<TException>([DoesNotReturnIf(true)] this bool value, string message) where TException : Exception
    {
        if (value)
        {
            var exp = (TException)Activator.CreateInstance(typeof(TException), message)!;

            _logger.Error(exp, message);
            throw exp;
        }
    }

    public static void ThrowExceptionIfTrue([DoesNotReturnIf(true)] this bool value, string message, Exception exception)
    {
        if (value)
        {
            _logger.Error(exception, message);
            throw exception;
        }
    }

    /// <summary>
    /// Formats a string with the given arguments
    /// </summary>
    /// <param name="str"></param>
    /// <param name="args"></param>
    /// <returns></returns>
    [Obsolete("Use string interpolation instead")]
    public static string StringFormat(this string str, params object?[] args) =>
        string.Format(str, args);

    /// <summary>
    /// Gets the text from a <see cref="StringBuilder"/> removing the last CRLF
    /// </summary>
    /// <param name="sb"></param>
    /// <param name="removeLastCRLF"></param>
    /// <returns></returns>
    public static string? GetText(this StringBuilder sb, bool removeLastCRLF = true)
    {
        if (removeLastCRLF)
        {
            return sb?.ToString().TrimEnd('\r', '\n');
        }

        return sb?.ToString();
    }

    /// <summary>
    /// Converts a string to a byte array
    /// </summary>
    /// <param name="str"></param>
    /// <returns></returns>
    public static byte[] ToByteArray(this string str) =>
        Encoding.UTF8.GetBytes(str);

    /// <summary>
    /// Converts a string to a base64 string
    /// </summary>
    /// <param name="str"></param>
    /// <returns></returns>
    public static string ToBase64String(this string str) =>
        Convert.ToBase64String(str.ToByteArray());

    /// <summary>
    /// Converts a byte array to a base64 string
    /// </summary>
    /// <param name="bytes"></param>
    /// <returns></returns>
    public static string GetString(this byte[] bytes) =>
        Convert.ToBase64String(bytes);

    /// <summary>
    /// Merges two dictionaries
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="U"></typeparam>
    /// <param name="dict1"></param>
    /// <param name="dict2"></param>
    /// <returns></returns>
    public static IDictionary<T, U> Merge<T, U>(this IDictionary<T, U> dict1, IDictionary<T, U>? dict2) where T : notnull
    {
        if (dict2 is null)
        {
            return dict1;
        }

        if (dict1 is null)
        {
            return dict2;
        }

        var retDict = new Dictionary<T, U>(dict1);
        foreach (var kvp in dict2)
        {
            if (!retDict.ContainsKey(kvp.Key))
            {
                retDict.Add(kvp.Key, kvp.Value);
            }
        }

        return retDict;
    }

    /// <summary>
    /// Delays the execution of the action while the condition is true
    /// </summary>
    /// <param name="millisecondsDelay"></param>
    /// <param name="condition"></param>
    /// <param name="peekingPeriod"></param>
    /// <param name="cancellationToken"></param>
    /// <param name="caller"></param>
    /// <returns></returns>
    public static async Task<bool> DelayWhileAsync(int millisecondsDelay, Expression<Func<bool>> condition, int peekingPeriod = 250, CancellationToken cancellationToken = default,
      [ConstantExpected][CallerMemberName] string caller = "") =>
        await DelayWhileAsync(TimeSpan.FromMilliseconds(millisecondsDelay), condition, peekingPeriod, cancellationToken, caller);

    /// <summary>
    /// Delays the execution of the action while the condition is true
    /// </summary>
    /// <param name="timeout"></param>
    /// <param name="action"></param>
    /// <param name="peekingPeriod"></param>
    /// <param name="cancellationToken"></param>
    /// <param name="caller"></param>
    /// <returns></returns>
    public static async Task<bool> DelayWhileAsync(TimeSpan timeout, Expression<Func<bool>> action, int peekingPeriod = 250, CancellationToken cancellationToken = default, [ConstantExpected][CallerMemberName] string caller = "")
    {
        var logger = LogManager.GetCurrentClassLogger();
        var sw = Stopwatch.StartNew();

        if (Debugger.IsAttached)
        {
            timeout = TimeSpan.FromSeconds(30);
        }

        if (cancellationToken.IsCancellationRequested)
        {
            logger.Warn("[{0}]: Cancellation requested", caller);
            return false;
        }

        var uid = Ulid.NewUlid();

        logger.Info("[{1}|{0}]: Delayed while condition, timeout: [{2}ms]", caller, uid, timeout.TotalMilliseconds);
        var strAction = action.ToString();
        logger.Debug("[{0}] Condition: [{1}]", uid, strAction);

        using var timer = new PeriodicTimer(TimeSpan.FromMilliseconds(peekingPeriod));
        bool result = false;

        var compiledAction = action.Compile();

        DateTime endTimeUtc = DateTime.UtcNow.Add(timeout);
        while (DateTime.UtcNow < endTimeUtc &&
            await timer.WaitForNextTickAsync(cancellationToken))
        {
            result = !compiledAction();

            if (result)
            {
                logger.Debug("[{1}|{0}] While condition is true in {2}ms", caller, uid, sw.ElapsedMilliseconds);
                return result;
            }
        }

        logger.Debug("[{1}|{0}] Timeout {2}ms: While condition is false", caller, uid, timeout.TotalMilliseconds);
        return result;
    }

    private static Lock _lock = new();

    /// <summary>
    /// Gets the value from the dictionary or adds it if it does not exist
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    /// <param name="dict"></param>
    /// <param name="key"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    public static TValue GetOrAdd<TKey, TValue>(this IDictionary<TKey, TValue> dict, TKey key, TValue value)
    {
        using (var scope = _lock.EnterScope())
        {
            if (!dict.ContainsKey(key))
            {
                dict.Add(key, value);
            }

            return dict[key];
        }
    }

    /// <summary>
    /// Fires and forgets a task
    /// </summary>
    /// <param name="task"></param>
    public static void FireAndForget(this Task task)
    {
        if (!task.IsCompleted || task.IsFaulted)
        {
            _ = task.ConfigureAwait(false);
        }
    }

    /// <summary>
    /// Gets the name of the caller
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="caller"></param>
    /// <returns></returns>
    [DebuggerStepThrough]
    public static string GetCallerName(this object obj, [ConstantExpected][CallerMemberName] string caller = "") =>
         caller;

    /// <summary>
    /// Gets the name of the caller
    /// </summary>
    /// <param name="caller"></param>
    /// <returns></returns>
    [DebuggerStepThrough]
    public static string GetCallerName([CallerMemberName] string caller = "") =>
         caller;

    /// <summary>
    /// Gets the acronym of the caller name (Upper case letters)
    /// </summary>
    /// <param name="randomNumber"></param>
    /// <param name="caller"></param>
    /// <returns></returns>
    [DebuggerStepThrough]
    public static string GetAcronimCallerName(bool randomNumber = false, [ConstantExpected][CallerMemberName] string caller = "")
    {
        var acronim = string.Empty;

        _ = caller.ToCharArray().Select<char, char?>(c =>
        {
            if (char.IsUpper(c))
            {
                acronim += c;
            }

            return null; // Return a value to satisfy the Select method
        }).ToList();

        if (randomNumber)
        {
            acronim += "." + RandomNumberGenerator.GetInt32(1000, 9999);
        }

        return acronim.ToLower();
    }

    public static bool Contains(this string str, string[] values, StringComparison comparisonType = StringComparison.InvariantCultureIgnoreCase) =>
        values.Any(v => str.Contains(v, comparisonType));

    /// <summary>
    /// Gets the environment variable (Case insensitive by default).
    /// </summary>
    /// <param name="variableName"></param>
    /// <param name="stringComparison"></param>
    /// <returns></returns>
    public static string? GetEnvironmentVariable(string variableName, StringComparison stringComparison = StringComparison.InvariantCultureIgnoreCase)
    {
        IDictionary environmentVariables = Environment.GetEnvironmentVariables();

        foreach (DictionaryEntry entry in environmentVariables)
        {
            if (string.Equals(entry.Key.ToString(), variableName, stringComparison))
            {
                return entry.Value!.ToString();
            }
        }

        return null;
    }

    /// <summary>
    /// Converts a boolean value to 'Yes' or 'No'
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static string ToYesNo(this bool value) =>
        value ? "Yes" : "No";

    /// <summary>
    /// Converts a boolean value to 'enabled' or 'disabled'
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static string ToEnabledDisabled(this bool value) =>
        value ? "enabled" : "disabled";

    public static string EscapeJsonString(this string str) =>
        str.Replace("\"", "'");

    /// <summary>
    /// Waits for a port to be open on a host.
    /// </summary>
    /// <param name="host"></param>
    /// <param name="port"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [OverloadResolutionPriority(1)]
    public static Task<bool> WaitForPortAsync(string host, int port, CancellationToken cancellationToken = default) =>
        WaitForPortAsync(host, port, 5000, 500, cancellationToken);

    /// <summary>
    /// Waits for a port to be open on a host.
    /// </summary>
    /// <param name="host"></param>
    /// <param name="port"></param>
    /// <param name="timeout"></param>
    /// <param name="retryInterval"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [OverloadResolutionPriority(2)]
    public static Task<bool> WaitForPortAsync(string host, int port, int timeout = 5000, int retryInterval = 500, CancellationToken cancellationToken = default)
    {
        var logger = LogManager.GetCurrentClassLogger();

        logger.Info("Waiting for port [{0}] on host [{1}]", port, host);

        DateTime endTime = DateTime.Now.AddMilliseconds(timeout);

        while (DateTime.Now < endTime)
        {
            try
            {
                cancellationToken.ThrowIfCancellationRequested();

                using TcpClient client = new();

                var result = client.BeginConnect(host, port, null, null);
                bool success = result.AsyncWaitHandle.WaitOne(retryInterval);

                if (success && client.Connected)
                {
                    logger.Debug("Port [{0}] on host [{1}] is open", port, host);

                    client.EndConnect(result);
                    return Task.FromResult(true);
                }
            }
            catch (SocketException)
            {
                // Ignorar la excepción y esperar antes de reintentar } 
                Thread.Sleep(retryInterval);
            }
        }

        logger.Debug("Timeout waiting for port [{0}] on host [{1}]", port, host);
        return Task.FromResult(false);
    }

    /// <summary>
    /// Gets a random free port.
    /// </summary>
    /// <returns></returns>
    public static int GetRandomFreePort()
    {
        var listener = new TcpListener(IPAddress.Loopback, 0);
        listener.Start();
        var port = ((IPEndPoint)listener.LocalEndpoint).Port;
        listener.Stop();

        return port;
    }
}
