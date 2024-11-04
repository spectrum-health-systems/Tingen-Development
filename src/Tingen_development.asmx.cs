// =============================================================== 24.11.0 =====
// Tingen: The Tingen web service entry point.
// Repository: https://github.com/APrettyCoolProgram/Tingen-Development
// Documentation: https://github.com/spectrum-health-systems/Tingen-Documentation
// Copyright (c) A Pretty Cool Program. All rights reserved.
// Licensed under the Apache 2.0 license.
// ================================================================ 241104 =====

// u241023.1223_code
// u241031_documentation

using System.Reflection;
using System.Web.Services;
using Outpost31.Core;
using Outpost31.Core.Logger;
using Outpost31.Core.Session;
using ScriptLinkStandard.Objects;

namespace Tingen_development
{
    /// <summary>The entry point for the Tingen web service.</summary>
    /// <include file='XmlDoc/Tingen_doc.xml' path='Tingen/Type[@name="Class"]/Tingen/*'/>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    public class Tingen_development : WebService
    {
        /// <summary>The executing Assembly name.</summary>
        /// <remarks>A required component for writing log files, defined here so it can be used throughout the class.</remarks>
        public static string ExeAsm { get; set; } = Assembly.GetExecutingAssembly().GetName().Name;

        /// <summary>The Tingen current version number.</summary>
        /// <include file='XmlDoc/Tingen_doc.xml' path='Tingen/Type[@name="Property"]/TingenVersionNumber/*'/>
        public static string TingenVersionNumber { get; set; } = Assembly.GetExecutingAssembly().GetName().Version.ToString();

        /// <summary>Get the current version of Tingen.</summary>
        /// <returns>The current version number of Tingen.</returns>
        /// <include file='XmlDoc/Tingen_doc.xml' path='Tingen/Type[@name="Method"]/GetVersion/*'/>
        [WebMethod]
        public string GetVersion() => $"VERSION {TingenVersionNumber}";

        /// <summary>Starts the Tingen web service.</summary>
        /// <param name="sentOptionObject">The SentOptionObject sent from Avatar.</param>
        /// <param name="sentScriptParameter">The SentScriptParameter sent from Avatar.</param>
        /// <returns>The finalized OptionObject to myAvatar.</returns>
        /// <include file='XmlDoc/Tingen_doc.xml' path='Tingen/Type[@name="Method"]/RunScript/*'/>
        [WebMethod]
        public OptionObject2015 RunScript(OptionObject2015 sentOptionObject, string sentScriptParameter)
        {
            /* Trace Logs can't go here because the logging infrastructure hasn't been initialized yet, so if you
             * need to create a log file here, use a Primeval Log.
             */

            TingenSession tnSession = TingenSession.Build(sentOptionObject, sentScriptParameter, TingenVersionNumber);

            tnSession.AvData.SystemCode = "UAT";

            LogEvent.Trace(1, ExeAsm, tnSession.TraceInfo);

            TingenApp.StartApp(tnSession);

            TingenApp.StopApp(tnSession);

            return tnSession.AvData.ReturnOptionObject;
        }
    }
}

/*
=================
DEVELOPMENT NOTES
=================

*/