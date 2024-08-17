// ================================================================ 24.8.0 =====
// Tingen: Tingen web service.
// Repository: https://github.com/APrettyCoolProgram/Tingen-Development
// Documentation: https://github.com/spectrum-health-systems/Tingen-Documentation
// Copyright (c) A Pretty Cool Program. All rights reserved.
// Licensed under the Apache 2.0 license.
// ================================================================ 240817 =====

// -----------------------------------------------------------------------------
//                                 About Tingen
// -----------------------------------------------------------------------------
// Tingen is a custom web service for Avatar.
//
// Tingen doesn't actually do much, it mainly accepts a request from Avatar and
// forwards it to Outpost31 to do the heavy lifting.
//
// In theory, this source code shouldn't need to be modified, so if the update
// timestamps are really old, that's fine.

// u240817.1003_code
// u240817.1003_documentation

using System.Reflection;
using System.Web.Services;
using Outpost31.Core;
using Outpost31.Core.Logger;
using Outpost31.Core.Session;
using ScriptLinkStandard.Objects;

namespace Tingen_development
{
    /// <summary>Entry point for Tingen.</summary>
    /// <include file='XmlDoc/Tingen_doc.xml' path='Doc/Sec[@name="Tingen"]/Tingen/*'/>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    public class Tingen_development : WebService
    {
        /// <summary>Assembly name for logging purposes.</summary>
        /// <remarks> The assembly name is defined here so it can be used to write log files throughout the class.</remarks>
        public static string AssemblyName { get; set; } = Assembly.GetExecutingAssembly().GetName().Name;

        /// <summary>Tingen current version.</summary>
        /// <remarks>The version number is referenced in both <c>GetVersion()</c> and <c>RunScript()</c></remarks>.
        public static string TingenVersion { get; set; } = Assembly.GetExecutingAssembly().GetName().Version.ToString();

        /// <summary>Get the current version of Tingen.</summary>
        /// <remarks>This method is required by Avatar and should not be modified.</remarks>
        /// <returns>The current version of Tingen.</returns>
        [WebMethod]
        public string GetVersion() => $"VERSION {TingenVersion}";

        /// <summary>Starts the Tingen web service.</summary>
        /// <param name="sentOptionObject">The OptionObject sent from Avatar.</param>
        /// <param name="sentScriptParameter">The ScriptParameter sent from Avatar.</param>
        /// <remarks>This method is required by Avatar and should not be modified.</remarks>
        /// <returns>The finalized OptionObject to myAvatar.</returns>
        [WebMethod]
        public OptionObject2015 RunScript(OptionObject2015 sentOptionObject, string sentScriptParameter)
        {
            /* Trace logs can't go here - the infrastructure isn't setup yet.
             */

            TingenSession tnSession = TingenSession.Build(sentOptionObject, sentScriptParameter, TingenVersion);

            /* The only difference between the development and stable versions of Tingen is that the development version
             * uses the "UAT" system code, while the stable version uses the "LIVE" system code.
             */
            tnSession.AvData.SystemCode = "UAT";

            LogEvent.Trace(1, AssemblyName, tnSession.TraceInfo);

            TingenApp.StartApp(tnSession);

            TingenApp.StopApp(tnSession);

            return tnSession.AvData.ReturnOptionObject;
        }
    }
}