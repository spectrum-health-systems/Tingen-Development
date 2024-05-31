// ================================================================ 24.5.0 =====
// Tingen_development: The development version of Tingen
// https://github.com/APrettyCoolProgram/Tingen_development
// Copyright (c) A Pretty Cool Program. All rights reserved.
// Licensed under the Apache 2.0 license.
// ================================================================ 240531 =====

// u240531.1350

/* PLEASE NOTE
 * -----------
 * This is the development version of Tingen, and should not be used in production environments.
 *
 * For stable releases of Tingen: https://github.com/APrettyCoolProgram/Tingen
 *
 * For production environments: https://github.com/spectrum-health-systems/Tingen-Community-Release
 *
 * For more information about Tingen: https://github.com/spectrum-health-systems/Tingen-Documentation-Project
 *
 * For more information about web services and Avatar: https://github.com/myAvatar-Development-Community
 */

using System.Web.Services;
using Outpost31.Core.Configuration;
using Outpost31.Core.Session;
using ScriptLinkStandard.Objects;

namespace Tingen_development
{
    /// <summary>The entry class for Tingen.</summary>
    /// <remarks>
    ///  <para>
    ///   This class is designed to be static, and should not be modified.
    ///  </para>
    /// </remarks>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    public class Tingen_development : WebService
    {
        /// <summary>Returns the current version of Tingen.</summary>
        /// <remarks>
        ///  <para>
        ///   This method is required by Avatar.<br/><br/>
        ///   The version number the current development version in <b>YY.MM</b> format.
        ///  </para>
        /// </remarks>
        /// <returns>The current version of Tingen.</returns>
        [WebMethod]
        public string GetVersion() => "VERSION 24.5";

        /// <summary>The starting method for Tingen.</summary>
        /// <param name="sentOptionObject">The OptionObject sent from myAvatar.</param>
        /// <param name="sentScriptParameter">The Script Parameter sent from myAvatar.</param>
        /// <remarks>
        ///  <para>
        ///   This method:
        ///   <list type="bullet">
        ///    <item>Is required by Avatar</item>
        ///    <item>Should not be modified, since the actual work is done elsewhere.</item>
        ///    <item>Contains Primeval.Log() statements for debugging that should be commented out in the production version.</item>
        ///   </list>
        ///  </para>
        ///  <para>
        ///   The only difference between the development and production versions of this class is the value of <c>configFilePath</c>:
        ///   <list type="table">
        ///    <item>
        ///     <term>Development</term>
        ///     <description>C:\Tingen\UAT\Configs\Tingen.config</description>
        ///    </item>
        ///    <item>
        ///     <term>Production</term>
        ///     <description>C:\Tingen\LIVE\Configs\Tingen.config</description>
        ///    </item>
        ///   </list>
        ///  </para>
        ///  <para>
        ///   Tingen has the following modes:
        ///   <list type="table">
        ///    <item>
        ///     <term>Enabled</term>
        ///     <description>Work is done, and a modified sentOptionObject is returned to Avatar</description>
        ///    </item>
        ///    <item>
        ///     <term>Disabled</term>
        ///     <description><i>No work</i> is done, and the <i>unmodified</i> sentOptionObject is returned to Avatar</description>
        ///    </item>
        ///   </list>
        ///  </para>
        /// </remarks>
        /// <returns>The finalized OptionObject to myAvatar.</returns>
        [WebMethod]
        public OptionObject2015 RunScript(OptionObject2015 sentOptionObject, string sentScriptParameter)
        {
            Outpost31.Core.Debuggler.PrimevalLog.Create($"[START]"); /* <- For development use only */

            string configFilePath   = TingenConfiguration.GetPath("UAT");
            TingenSession tnSession = TingenSession.Load(configFilePath, sentOptionObject, sentScriptParameter);

            if (tnSession.TingenMode == "enabled")
            {
                Outpost31.Core.Roundhouse.Parse(tnSession);
            }
            else
            {
                /* If Tingen is disabled, update all of the service status files.
                 */
                Outpost31.Module.Admin.Service.AllUpdate(tnSession);
                Outpost31.Core.Avatar.TheOptionObject.ReturnClonedSent(tnSession);
            }

            Outpost31.Core.Debuggler.PrimevalLog.Create($"[END]"); /* <- For development use only */

            return tnSession.AvData.ReturnOptionObject;
        }
    }
}