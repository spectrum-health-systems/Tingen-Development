// ================================================================ 24.7.0 =====
// Tingen-development: The development version of Tingen
// https://github.com/APrettyCoolProgram/Tingen_development
// Copyright (c) A Pretty Cool Program. All rights reserved.
// Licensed under the Apache 2.0 license.
// ================================================================ 240702 =====

// u240627.0909_code
// u240702.1225_documentation

/* -----------
 * PLEASE READ
 * -----------
 *
 * Tingen_development.cs are the entry points for the development version of the Tingen web service.
 *
 * This class is pretty bare-bones because the heavy lifting is done in Outpost31, which is shared between the production and
 * development versions of Tingen.
 *
 * Tingen_development.cs should not be modified (so don't worry if the "// uYYMMDD.HHMM" comment up above is old). Any changes to the
 * Tingen web service should be made in Outpost31, generally in Outpost31.Core.TingenApp.Start() and Outpost31.Core.TingenApp.Stop().
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
    /// <summary>Entry point for Tingen.</summary>
    /// <remarks>
    ///  <para>
    ///   This class:
    ///   <list type="bullet">
    ///    <item>Should not be modified</item>
    ///    <item>Doesn't do much (the heavy lifting is done in <a href="https://github.com/spectrum-health-systems/Tingen-Documentation/blob/main/Glossary.md#Outpost31">Outpost31</a>)</item>
    ///   </list>
    ///  </para>
    ///  <para>
    ///   Please see the <a href="https://github.com/spectrum-health-systems/Tingen-Documentation">Tingen documentation</a> for more information.
    ///  </para>
    /// </remarks>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    public class Tingen_development : WebService
    {
        /// <summary>Assembly name.</summary>
        /// <remarks>
        ///   <para>
        ///    The assembly name is defined here so it can be used to write log files throughout the class.
        ///   </para>
        /// </remarks>
        public static string AssemblyName { get; set; } = Assembly.GetExecutingAssembly().GetName().Name;

        /// <summary>Tingen current version.</summary>
        /// <remarks>
        ///   <para>
        ///    The version number is defined here, since both <see cref="GetVersion()"/> and <see cref="RunScript()"/> use this.
        ///   </para>
        /// </remarks>
        public static string TingenVersion { get; set; } = Assembly.GetExecutingAssembly().GetName().Version.ToString();

        /// <summary>Get the current version of Tingen.</summary>
        /// <remarks>
        ///  <para>
        ///   This method:
        ///   <list type="bullet">
        ///    <item>Is required by Avatar</item>
        ///    <item>Should not be modified</item>
        ///   </list>
        ///  </para>
        /// </remarks>
        /// <returns>The current version of Tingen.</returns>
        [WebMethod]
        public string GetVersion() => $"VERSION {TingenVersion}";

        /// <summary>Start the Tingen web service</summary>
        /// <param name="sentOptionObject">The <paramref name="OptionObject"/> sent from Avatar.</param>
        /// <param name="sentScriptParameter">The <paramref name="Script Parameter"/> sent from Avatar.</param>
        /// <remarks>
        ///     <para>
        ///         This method:
        ///         <list type="bullet">
        ///             <item>Is required by Avatar</item>
        ///             <item>Should not be modified</item>
        ///         </list>
        ///     </para>
        ///     <para>
        ///         The only difference between Tingen_development.cs and Tingen.cs is the <b>AvatarSystemCode</b>:
        ///         <list type="table">
        ///             <listheader>
        ///                 <term>Setting</term>
        ///                 <description>Description</description>
        ///             </listheader>
        ///             <item>
        ///                 <term>UAT</term>
        ///                 <description>The <b>AvatarSystemCode</b> for the UAT environment</description>
        ///             </item>
        ///             <item>
        ///                 <term>LIVE</term>
        ///                 <description>The <b>AvatarSystemCode</b> for the LIVE environment</description>
        ///             </item>
        ///         </list>
        ///     </para>
        /// </remarks>
        /// <returns>The finalized <b>OptionObject</b> to myAvatar.</returns>
        [WebMethod]
        public OptionObject2015 RunScript(OptionObject2015 sentOptionObject, string sentScriptParameter)
        {
            /* Trace logs can't go here - the infrastrucure isn't setup yet. */

            TingenSession tnSession = TingenSession.Build(sentOptionObject, sentScriptParameter, TingenVersion);

            /* The only difference between Tingen_development.cs and Tingen.cs is the AvatarSystemCode that is set here.
             *  - Tingen_development.cs = "UAT".
             *  - Tingen.cs = "LIVE".
             */
            tnSession.AvData.AvatarSystemCode = "UAT";

            LogEvent.Trace(1, AssemblyName, tnSession.TraceInfo);

            TingenApp.Start(tnSession);

            TingenApp.Stop(tnSession);

            return tnSession.AvData.ReturnOptionObject;
        }
    }
}