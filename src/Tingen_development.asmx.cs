// ================================================================ 24.6.0 =====
// Tingen_development: The development version of Tingen
// https://github.com/APrettyCoolProgram/Tingen_development
// Copyright (c) A Pretty Cool Program. All rights reserved.
// Licensed under the Apache 2.0 license.
// ================================================================ 240607 =====

// u240607.0821

/* PLEASE READ
 * -----------
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

using System.Collections.Generic;
using System.Reflection;
using System.Web.Services;
using Outpost31.Core.Logger;
using Outpost31.Core.Session;
using ScriptLinkStandard.Objects;
using Outpost31.Core;

namespace Tingen_development
{
    /// <summary>The entry class for Tingen.</summary>
    /// <remarks>
    ///  <para>
    ///   - This class is designed to be static, and should not be modified.
    ///  </para>
    /// </remarks>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    public class Tingen_development : WebService
    {
        /// <summary>Assembly information.</summary>
        /// <remarks>
        ///   <para>
        ///    - Name for log files<br/>
        ///    - Version for rest.
        ///   </para>
        /// </remarks>
        public static string AssemblyName { get; set; }  = Assembly.GetExecutingAssembly().GetName().Name;
        public static string TingenVersion { get; set; } = Assembly.GetExecutingAssembly().GetName().Version.ToString();

        /// <summary>Returns the current version of Tingen.</summary>
        /// <remarks>
        ///  <para>
        ///   - Required by Avatar.<br/>
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
        ///   - Tingen uses a few hardcoded variables, which are set in <see cref="TingenSession.BuildStaticVars()"/>"<br/>
        ///  </para>
        /// </remarks>
        /// <returns>The finalized OptionObject to myAvatar.</returns>
        [WebMethod]
        public OptionObject2015 RunScript(OptionObject2015 sentOptionObject, string sentScriptParameter)
        {
            TingenSession tnSession = TingenSession.Build(sentOptionObject, sentScriptParameter, TingenVersion);

            LogEvent.Trace(1, AssemblyName, tnSession.TraceInfo);

            switch (tnSession.TnConfig.TingenMode)
            {
                case "disabled":
                    LogEvent.Trace(2, AssemblyName, tnSession.TraceInfo);

                    Outpost31.Core.Framework.Refresh.RefreshOnDisable(tnSession);

                    Stop.WebApp(tnSession);

                    //EndTingen(tnSession);

                    break;

                case "development":
                    LogEvent.Trace(2, AssemblyName, tnSession.TraceInfo);

                    Outpost31.Core.Framework.Refresh.RefreshOnDevelopment(tnSession);

                    Start.WebApp(tnSession);

                    Stop.WebApp(tnSession);

                    //StartTingen(tnSession);

                    //EndTingen(tnSession);

                    break;

                default: // "enabled"
                    LogEvent.Trace(2, AssemblyName, tnSession.TraceInfo);

                    Start.WebApp(tnSession);

                    Stop.WebApp(tnSession);

                    //StartTingen(tnSession);

                    //EndTingen(tnSession);

                    break;
            }

            return tnSession.AvData.ReturnOptionObject.ToReturnOptionObject();
        }

        //private static void StartTingen(TingenSession tnSession)
        //{
        //    LogEvent.Trace(1, AssemblyName, tnSession.TraceInfo);

        //    Outpost31.Core.Roundhouse.Parse(tnSession);
        //}

        //private static void EndTingen(TingenSession tnSession)
        //{
        //    LogEvent.Trace(2, AssemblyName, tnSession.TraceInfo);

        //    if (tnSession.TnConfig.TingenMode == "disabled")
        //    {
        //        tnSession.AvData.ReturnOptionObject = tnSession.AvData.SentOptionObject.Clone(); // TODO move to core functionality
        //    }
        //    else
        //    {
        //        tnSession.AvData.ReturnOptionObject = tnSession.AvData.WorkOptionObject.Clone(); // TODO move to core functionality
        //    }

        //    ////var path = $@"{tnSession.TnPath.SystemCode.CurrentSession}\Session.md";

        //    ////File.WriteAllText(path, Catalog.SessionDetails(tnSession));
        //}
    }
}

/*

Development notes
-----------------
- Is there a efficient way to automatically update the service status files when Tingen is re-enabled?

*/