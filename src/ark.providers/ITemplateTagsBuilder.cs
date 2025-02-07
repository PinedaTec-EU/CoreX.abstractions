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
// FileName: ITemplateTagsBuilder.cs
//
// Author:   jmr.pineda
// eMail:    jmr.pineda@pinedatec.eu
// Profile:  http://pinedatec.eu/profile
//
//           Copyrights (c) PinedaTec.eu 2025, all rights reserved.
//           CC BY-NC-ND - https://creativecommons.org/licenses/by-nc-nd/4.0
//
//  Created at: 2025-02-07T08:40:51.683Z
//
// --------------------------------------------------------------------------------------

#endregion

using Microsoft.Extensions.Configuration;

namespace ark.providers;

public interface ITemplateTagsBuilder
{
    /// <summary>
    /// Get the standard tags
    /// </summary>
    /// <returns></returns>
    Dictionary<string, object?> GetStandardTags();

    /// <summary>
    /// Get the standard tags, appending the provided dictionary 
    /// </summary>
    /// <param name="dict"></param>
    /// <returns></returns>
    Dictionary<string, object?> GetStandardTags(Dictionary<string, object?> dict);

    /// <summary>
    /// Parse the expression, replacing the tags with the values 
    /// </summary>
    /// <param name="expression"></param>
    /// <param name="tagValues"></param>
    /// <param name="section"></param>
    /// <param name="exceptionNotResolved"></param>
    /// <param name="cryptoProvider"></param>
    /// <returns></returns>
    string Parse(string expression, Dictionary<string, object?>? tagValues = null, IConfigurationSection? section = null,
        bool exceptionNotResolved = false, Func<string, string, string, string>? cryptoProvider = null);
}
