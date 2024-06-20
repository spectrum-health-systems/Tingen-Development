// ================================================================ 24.6.0 =====
// Tingen-development: The development version of Tingen
// https://github.com/APrettyCoolProgram/Tingen_development
// Copyright (c) A Pretty Cool Program. All rights reserved.
// Licensed under the Apache 2.0 license.
// ================================================================ 240620 =====

// u240620.1102

/* -----------------------------------------------------------------
 * Important information about Tingen.cs (and Tingen_development.cs)
 * -----------------------------------------------------------------
 *
 * Tingen.cs and Tingen_development.cs are the entry points for the Tingen web service. Tingen.cs is the stable release intended for
 * production environments, while Tingen_development.cs is the development version.
 *
 * You are currently viewing Tingen_development.cs.
 *
 * These classes are pretty bare-bones because the heavy lifting is done in Outpost31, which is shared between the production and
 * development version of Tingen.
 *
 * Tingen.cs/Tingen_development.cs should not be modified, so don't worry if the "// uYYMMDD.HHMM" comment up above is old.
 *
 * Any changes to the Tingen web service should be made in Outpost31, generally in TingenApp.Start() and TingenApp.Stop().
 */

/* ----------------------------------------------
 * IMPORTANT INFORMATION ABOUT TINGEN_DEVELOPMENT
 * ----------------------------------------------
 *
 * This is the development version of Tingen, and should not be used in production environments.
 *
 * For stable releases of Tingen: https://github.com/APrettyCoolProgram/Tingen
 *
 * For production environments: https://github.com/spectrum-health-systems/Tingen-CommunityRelease
 *
 * For more information about Tingen: https://github.com/spectrum-health-systems/Tingen-Documentation
 *
 * For more information about web services and Avatar: https://github.com/myAvatar-Development-Community
 */

using System.Reflection;
using System.Web.Services;
using Outpost31.Core;
using Outpost31.Core.Logger;
using Outpost31.Core.Session;
using ScriptLinkStandard.Objects;

namespace Tingen_development
{
    /// <summary>The entry class for Tingen.</summary>
    /// <remarks>
    ///  <para>
    ///   - This class is designed to be static, and <i>should not be modified</i>.<br/>
    ///   - The heavy lifting is done in the <see href="https://github.com/spectrum-health-systems/Outpost31">Outpost31</see> project.
    ///  </para>
    /// </remarks>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    public class Tingen_development : WebService
    {
        /// <summary>Assembly name for log files.</summary>
        /// <remarks>
        ///   <para>
        ///    - Define the assembly name here so it can be used to write log files throughout the class.
        ///   </para>
        /// </remarks>
        public static string AssemblyName { get; set; } = Assembly.GetExecutingAssembly().GetName().Name;

        /// <summary>The current version of Tingen.</summary>
        /// <remarks>
        ///   <para>
        ///    - This is used in a few places in this class, so let's define it here.
        ///   </para>
        /// </remarks>
        public static string TingenVersion { get; set; } = Assembly.GetExecutingAssembly().GetName().Version.ToString();

        /// <summary>Returns the current version of Tingen.</summary>
        /// <remarks>
        ///  <para>
        ///   - Required by Avatar.<br/>
        ///   - <i>Should not be modified</i>.
        ///  </para>
        /// </remarks>
        /// <returns>The current version of Tingen.</returns>
        [WebMethod]
        public string GetVersion() => $"VERSION {TingenVersion}";

        /// <summary>Starts the Tingen web service</summary>
        /// <param name="sentOptionObject">The OptionObject sent from Avatar.</param>
        /// <param name="sentScriptParameter">The Script Parameter sent from Avatar.</param>
        /// <remarks>
        ///  <para>
        ///   - Required by Avatar.<br/>
        ///   - <i>Should not be modified</i><br/>
        ///   - The heavy lifting is done in <see href="https://github.com/spectrum-health-systems/Outpost31">Outpost31</see>
        ///  </para>
        /// </remarks>
        /// <returns>The finalized OptionObject to myAvatar.</returns>
        [WebMethod]
        public OptionObject2015 RunScript(OptionObject2015 sentOptionObject, string sentScriptParameter)
        {
            /* Trace logs can't go here - the infrastrucure isn't setup yet. */

            TingenSession tnSession = TingenSession.Build(sentOptionObject, sentScriptParameter, TingenVersion);

            LogEvent.Trace(1, AssemblyName, tnSession.TraceInfo);

            TingenApp.Start(tnSession);

            TingenApp.Stop(tnSession);

            /*[1]*/
            return tnSession.AvData.ReturnOptionObject;
        }
    }
}

/*

=================
DEVELOPMENT NOTES
=================

[1] It's important that the return object is formatted correctly before this point. Currently OptionObjects are formatted closer to
    the work being done, but we need to make sure that is happenening. This might actually be done in TingenApp.Stop(). Regardless, we
    should have a failsafe to make sure the return object is formatted correctly before it gets returned to Avatar.

*/