// ================================================================ 24.5.0 =====
// Tingen_development: The development version of Tingen
// https://github.com/APrettyCoolProgram/Tingen_development
// Copyright (c) A Pretty Cool Program. All rights reserved.
// Licensed under the Apache 2.0 license.
// ================================================================ 240525 =====

// u240525.0928

/* PLEASE NOTE
 * -----------
 * This is the development version of Tingen, and should not be used in
 * production environments.
 *
 * For stable releases of Abatab:
 *  https://github.com/APrettyCoolProgram/Tingen
 *
 * For production environments:
 *  https://github.com/spectrum-health-systems/Tingen-Community-Release
 *
 * For more information about Abatab:
 *  https://github.com/spectrum-health-systems/Tingen-Documentation-Project
 *
 * For more information about web services and Avatar:
 *  https://github.com/myAvatar-Development-Community
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using ScriptLinkStandard.Objects;

namespace Tingen_development
{
    /// <summary>
    /// Summary description for Tingen_development
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class Tingen_development : System.Web.Services.WebService
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
             * This is a terrible, dangerous way to test Abatab, and should be removed at some point. But if you really,
             * really want to do something dumb and test Abatab in a way that is not recommended (and might not even
             * work), uncomment the line below, debug the solution, click the GetVersion link, and then the "Invoke"
             * button. Good luck!
             */
            RunScript(null, "admin-service-status-update");

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
        ///                 <description>C:\Abatab\UAT\Configs\abatab.config</description>
        ///             </item>
        ///             <item>
        ///                 <term>Production</term>
        ///                 <description>C:\Abatab\LIVE\Configs\abatab.config</description>
        ///             </item>
        ///         </list>
        ///     </para>
        ///     <para>
        ///         Abatab has the following modes:
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
            // For development use only: Abatab.Core.Debuggler.Primeval.Log($"[START]");

            //const string configFilePath = @"C:\Abatab\UAT\Configs\abatab.config";

            //AbatabSession abSession = AbatabSession.Load(configFilePath, sentOptionObject, sentScriptParameter);

            //if (abSession.AbatabMode == "enabled")
            //{
            //    Abatab.Core.Common.ParseScriptModule.Run(abSession);
            //}
            //else
            //{
            //    Abatab.Core.Debuggler.Primeval.Log("[SERVICE DISABLED]");
            //    // [TODO] Just make the sent OptionObject the return OptionObject.
            //}

            /* For development use only: Abatab.Core.Debuggler.Primeval.Log($"[SEND]"); */

            return sentOptionObject.ToReturnOptionObject(); // [TODO] Eventually this should be abSession.OptionObject.ToReturnOptionObject();
        }

    }
}
