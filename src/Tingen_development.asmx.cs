// ================================================================ 24.5.0 =====
// Tingen_development: The development version of Tingen
// https://github.com/APrettyCoolProgram/Tingen_development
// Copyright (c) A Pretty Cool Program. All rights reserved.
// Licensed under the Apache 2.0 license.
// ================================================================ 240525 =====

// u240525.1402

/* PLEASE NOTE
 * -----------
 * This is the development version of Tingen, and should not be used in
 * production environments.
 *
 * For stable releases of Tingen:
 *  https://github.com/APrettyCoolProgram/Tingen
 *
 * For production environments:
 *  https://github.com/spectrum-health-systems/Tingen-Community-Release
 *
 * For more information about Tingen:
 *  https://github.com/spectrum-health-systems/Tingen-Documentation-Project
 *
 * For more information about web services and Avatar:
 *  https://github.com/myAvatar-Development-Community
 */

using System.Web.Services;
using Outpost31.Core.Session;
using ScriptLinkStandard.Objects;

namespace Tingen_development
{
    /// <summary>The entry class for Tingen.</summary>
    /// <remarks>
    ///     This class is designed to be static, and should not be modified.
    /// </remarks>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    public class Tingen_development : WebService
    {
        /// <summary>Returns the current version of Tingen.</summary>
        /// <remarks>
        ///     <para> This method is required by Avatar. </para>
        ///     <para> The version number the current development version in <b>YY.MM</b> format. </para>
        /// </remarks>
        /// <returns>The current version of Tingen.</returns>
        [WebMethod]
        public string GetVersion()
        {
            /* DON'T DO THIS!
             * This is a terrible, dangerous way to test Tingen, and should be removed at some point. But if you really,
             * really want to do something dumb and test Tingen in a way that is not recommended (and might not even
             * work), uncomment the line below, debug the solution, click the GetVersion link, and then the "Invoke"
             * button. Good luck!
             */
            //RunScript(null, "admin-service-status-update");

            return "VERSION 24.5";
        }

        /// <summary>The starting method for Tingen.</summary>
        /// <param name="sentOptionObject">The OptionObject sent from myAvatar.</param>
        /// <param name="sentScriptParameter">The Script Parameter sent from myAvatar.</param>
        /// <remarks>
        ///     <para>This method is required by Avatar, and should be modified since the work is done elsewhere.</para>
        ///     <para>
        ///         The only difference between the development and production versions of this class is the <paramref name="configFilePath"/> value
        ///         <list type="table">
        ///             <item>
        ///                 <term>Development</term>
        ///                 <description>C:\Tingen\UAT\Configs\Tingen.config</description>
        ///             </item>
        ///             <item>
        ///                 <term>Production</term>
        ///                 <description>C:\Tingen\LIVE\Configs\Tingen.config</description>
        ///             </item>
        ///         </list>
        ///     </para>
        ///     <para>
        ///         Tingen has the following modes:
        ///         <list type="table">
        ///             <item>
        ///                 <term>Enabled</term>
        ///                 <description>Work is done, and a modified sentOptionObject is returned to Avatar</description>
        ///             </item>
        ///             <item>
        ///                 <term>Disabled</term>
        ///                 <description><i>No work</i> is done, and the <i>unmodified</i> sentOptionObject is returned to Avatar</description>
        ///             </item>
        ///         </list>
        ///     </para>
        ///     - This class has Debuggler statements that should remain in place for development purposes, and
        ///       commented out in the production version.
        /// </remarks>
        /// <returns>The finalized OptionObject to myAvatar.</returns>
        [WebMethod]
        public OptionObject2015 RunScript(OptionObject2015 sentOptionObject, string sentScriptParameter)
        {
            Outpost31.Core.Debuggler.Primeval.Log($"[START]"); /* <- For development use only */

            const string configFilePath = @"C:\TingenData\UAT\Configs\Tingen.config";

            TingenSession tnSession = TingenSession.Load(configFilePath, sentOptionObject, sentScriptParameter);

            Outpost31.Core.Debuggler.Primeval.Log($"[tnSession.AvatarSystemCode] {tnSession.AvatarSystemCode.ToLower()}");

            if (tnSession.TingenMode == "enabled")
            {
                Outpost31.Core.Common.ParseScriptModule.Run(tnSession);
            }
            else
            {
                //Outpost31.Core.Debuggler.Primeval.Log("[SERVICE DISABLED]"); /* <- For development use only */
                // [TODO] Just make the sent OptionObject the return OptionObject.
            }

            Outpost31.Core.Debuggler.Primeval.Log($"[END]"); /* <- For development use only */

            return sentOptionObject.ToReturnOptionObject(); // [TODO] Eventually this should be tnSession.OptionObject.ToReturnOptionObject();
        }

    }
}