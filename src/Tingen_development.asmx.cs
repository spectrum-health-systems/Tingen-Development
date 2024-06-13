// ================================================================ 24.6.0 =====
// Tingen_development: The development version of Tingen
// https://github.com/APrettyCoolProgram/Tingen_development
// Copyright (c) A Pretty Cool Program. All rights reserved.
// Licensed under the Apache 2.0 license.
// ================================================================ 240613 =====

// u240612.0935

/* ----------------------------------
 * IMPORTANT INFORMATION ABOUT TINGEN
 * ----------------------------------
 * The Tingen class is the entry point for the Tingen web service.
 *
 * You'll notice it's pretty bare-bones, and doesn't do much. That's because the heavy lifting is done in the Outpost31
 * project. All Tingen does is provide the interface that Avatar expects, and then hands the work off to Outpost31.
 *
 * This class really shouldn't be modified (so don't worry about the build number being really old, it's fine)
 *
 * When a new version of Tingen is released, the version and build number in the file header should be updated.
 */

/* ----------------------------------------------
 * IMPORTANT INFORMATION ABOUT TINGEN_DEVELOPMENT
 * ----------------------------------------------
 *
 * This is the development version of Tingen, and should not be used in production environments.
 *
 * For stable releases of Tingen: https://github.com/APrettyCoolProgram/Tingen
 *
 * For production environments: https://github.com/spectrum-health-systems/Tingen-Community-Release
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
    ///   - The heavy lifting is done in the <see cref="Outpost31"/> project.
    ///  </para>
    /// </remarks>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    public class Tingen_development : WebService
    {
        /// <summary>Assembly information.</summary>
        /// <remarks>
        ///  <para>
        ///   There are two assembly properties that we'll set here to use elsewhere in the class:<br/>
        ///   <list type="table">
        ///    <item>
        ///     <term>AssemblyName</term>
        ///     <description>The assemblyname used when creating trace logs.</description>
        ///    </item>
        ///    <item>
        ///     <term>TingenVersion</term>
        ///     <description>The current version of Tingen, used in <see cref="GetVersion()"/> and in the Tingen Session object.</description>
        ///    </item>
        ///   </list>
        ///  </para>
        /// </remarks>
        public static string AssemblyName { get; set; } = Assembly.GetExecutingAssembly().GetName().Name;
        public static string TingenVersion { get; set; } = Assembly.GetExecutingAssembly().GetName().Version.ToString();

        /// <summary>Returns the current version of Tingen.</summary>
        /// <remarks>
        ///  <para>
        ///   - Required by Avatar.<br/>
        ///   - Should <i>not be modified</i>.<br/>
        ///  </para>
        /// </remarks>
        /// <returns>The current version of Tingen.</returns>
        [WebMethod]
        public string GetVersion() => $"VERSION {TingenVersion}";

        /// <summary>Determines what work needs to be done, and returns data to Avatar.</summary>
        /// <param name="sentOptionObject">The OptionObject sent from Avatar.</param>
        /// <param name="sentScriptParameter">The Script Parameter sent from Avatar.</param>
        /// <remarks>
        ///  <para>
        ///   - Required by Avatar.<br/>
        ///   - Should <i>not be modified</i>, since the heavy lifting is done elsewhere.<br/>
        ///  </para>
        /// </remarks>
        /// <returns>The finalized OptionObject to myAvatar.</returns>
        [WebMethod]
        public OptionObject2015 RunScript(OptionObject2015 sentOptionObject, string sentScriptParameter)
        {
            TingenSession tnSession = TingenSession.Build(sentOptionObject, sentScriptParameter, TingenVersion);

            LogEvent.Trace(1, AssemblyName, tnSession.TraceInfo);

            TingenApp.Start(tnSession);

            TingenApp.Stop(tnSession);

            return tnSession.AvData.ReturnOptionObject.ToReturnOptionObject();
        }
    }
}

/*

-----------------
Development notes
-----------------

*/