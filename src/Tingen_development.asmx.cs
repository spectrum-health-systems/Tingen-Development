// ================================================================ 24.7.0 =====
// Tingen-Development: Tingen development version.
// Repository: https://github.com/APrettyCoolProgram/Tingen-Development
// Documentation: https://github.com/spectrum-health-systems/Tingen-Documentation
// Copyright (c) A Pretty Cool Program. All rights reserved.
// Licensed under the Apache 2.0 license.
// ================================================================ 240702 =====

// u240627.0909_code
// u240702.1225_documentation

using System.Reflection;
using System.Web.Services;
using Outpost31.Core;
using Outpost31.Core.Logger;
using Outpost31.Core.Session;
using ScriptLinkStandard.Objects;

namespace Tingen_development
{
    /// <summary>Entry point for Tingen.</summary>
    /// <include file='XMLDoc/Tingen.xml' path='doc/sec[@name="tingen"]/Tingen/*'/>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    public class Tingen_development : WebService
    {
        /// <summary>Assembly name for logging purposes.</summary>
        /// <include file='XMLDoc/Common.xml' path='doc/sec[@name="common"]/AssemblyName/*'/>
        public static string AssemblyName { get; set; } = Assembly.GetExecutingAssembly().GetName().Name;

        /// <summary>Tingen current version.</summary>
        /// <include file='XMLDoc/Tingen.xml' path='doc/sec[@name="tingen"]/TingenVersion/*'/>
        public static string TingenVersion { get; set; } = Assembly.GetExecutingAssembly().GetName().Version.ToString();

        /// <summary>Get the current version of Tingen.</summary>
        /// <returns>The current version of Tingen.</returns>
        /// <include file='XMLDoc/Tingen.xml' path='doc/sec[@name="tingen"]/GetVersion/*'/>
        [WebMethod]
        public string GetVersion() => $"VERSION {TingenVersion}";

        /// <summary>Start the Tingen web service</summary>
        /// <param name="sentOptionObject">The <paramref name="OptionObject"/> sent from Avatar.</param>
        /// <param name="sentScriptParameter">The <paramref name="Script Parameter"/> sent from Avatar.</param>
        /// <returns>The finalized OptionObject to myAvatar.</returns>
        /// <include file='XMLDoc/Tingen.xml' path='doc/sec[@name="tingen"]/RunScript/*'/>
        [WebMethod]
        public OptionObject2015 RunScript(OptionObject2015 sentOptionObject, string sentScriptParameter)
        {
            /* Trace logs can't go here - the infrastrucure isn't setup yet. */

            TingenSession tnSession = TingenSession.Build(sentOptionObject, sentScriptParameter, TingenVersion);

            /* TODO: Put these in examples in the XML documentation
             * The only difference between Tingen_development.cs and Tingen.cs is the AvatarSystemCode that is set here.
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