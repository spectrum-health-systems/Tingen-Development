// ================================================================ 24.6.0 =====
// Tingen_development: The development version of Tingen
// https://github.com/APrettyCoolProgram/Tingen_development
// Copyright (c) A Pretty Cool Program. All rights reserved.
// Licensed under the Apache 2.0 license.
// ================================================================ 240606 =====

// u240606.0824

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
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using System.Web.Services;
using Outpost31.Core.Logger;
using Outpost31.Core.Session;
using ScriptLinkStandard.Objects;

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
        /// <summary>Returns the current version of Tingen.</summary>
        /// <remarks>
        ///  <para>
        ///   - Required by Avatar.<br/>
        ///   - The version number is the current development version in <b>YY.MM</b> format (ex: "<b>24.5</b>").
        ///  </para>
        /// </remarks>
        /// <returns>The current version of Tingen.</returns>
        [WebMethod]
        public string GetVersion() => "VERSION 24.6";

        /// <summary>The starting method for Tingen.</summary>
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
            /* Trace logs cannot be used here. For debugging purposes, use a Primeval log. */
            LogEvent.Primeval(Assembly.GetExecutingAssembly().GetName().Name);
            Dictionary<string, string> hardCode = SetHardCodes();
            LogEvent.Primeval(Assembly.GetExecutingAssembly().GetName().Name);
            TingenSession tnSession = TingenSession.Build(sentOptionObject, sentScriptParameter, hardCode["avSystemCode"], hardCode["tnDataRoot"], hardCode["tnConfigFileName"]);
            LogEvent.Primeval(Assembly.GetExecutingAssembly().GetName().Name);
            Outpost31.Core.Framework.Maintenance.VerifyFrameworkStructure(tnSession);
            LogEvent.Primeval(Assembly.GetExecutingAssembly().GetName().Name);
            ////TingenSession.Initialize(tnSession); // need?

            /* Logging is done a little different in this method, since the Tingen Session is not yet initialized. We'll get the
             * AssemblyName here instead of at the top of the method.
             */
            string assemblyName = Assembly.GetExecutingAssembly().GetName().Name; // TODO Move to top?

            LogEvent.Trace(1, assemblyName, tnSession.TraceInfo);

            switch (tnSession.TnConfig.TingenMode)
            {
                case "disabled":
                    LogEvent.Trace(2, assemblyName, tnSession.TraceInfo);

                    /* If Tingen is disabled, we should update the service status files so the necessary users are notified. When
                     * Tingen is re-enabled, the service status files will need to be manually updated using the Admin Module.
                     */
                    Outpost31.Module.Admin.Service.Status.UpdateAll(tnSession);

                    EndTingen(tnSession, assemblyName);

                    break;

                case "development":
                    LogEvent.Trace(2, assemblyName, tnSession.TraceInfo);

                    Outpost31.Core.Framework.Maintenance.DevelopmentModeCleanup(tnSession.TnPath.Tingen.Primeval, tnSession.TnPath.SystemCode.Sessions);

                    StartTingen(tnSession, assemblyName);

                    EndTingen(tnSession, assemblyName);

                    break;

                default:
                    LogEvent.Trace(2, assemblyName, tnSession.TraceInfo);

                    StartTingen(tnSession, assemblyName);

                    EndTingen(tnSession, assemblyName);

                    break;
            }

            return tnSession.AvData.ReturnOptionObject.ToReturnOptionObject();
        }

        private static Dictionary <string, string> SetHardCodes()
        {
            return new Dictionary<string, string>
            {
                { "avSystemCode",     "UAT" },
                { "tnDataRoot",       @"C:\TingenData\" },
                { "tnConfigFileName", "Tingen.config" }
            };
        }

        private static void StartTingen(TingenSession tnSession, string assemblyName)
        {
            LogEvent.Trace(1, assemblyName, tnSession.TraceInfo);

            Outpost31.Core.Roundhouse.Parse(tnSession);
        }

        private static void EndTingen(TingenSession tnSession, string assemblyName)
        {
            LogEvent.Trace(2, assemblyName, tnSession.TraceInfo);

            if (tnSession.TnConfig.TingenMode == "disabled")
            {
                tnSession.AvData.ReturnOptionObject = tnSession.AvData.SentOptionObject.Clone(); // TODO move to core functionality
            }
            else
            {
                tnSession.AvData.ReturnOptionObject = tnSession.AvData.WorkOptionObject.Clone(); // TODO move to core functionality
            }

            ////var path = $@"{tnSession.TnPath.SystemCode.CurrentSession}\Session.md";

            ////File.WriteAllText(path, Catalog.SessionDetails(tnSession));
        }
    }
}

/*

Development notes
-----------------
- Is there a efficient way to automatically update the service status files when Tingen is re-enabled?

*/