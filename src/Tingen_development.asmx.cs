// ================================================================ 24.6.0 =====
// Tingen_development: The development version of Tingen
// https://github.com/APrettyCoolProgram/Tingen_development
// Copyright (c) A Pretty Cool Program. All rights reserved.
// Licensed under the Apache 2.0 license.
// ================================================================ 240606 =====

// u240605.1530

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

using System.IO;
using System.Reflection;
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
            /* Debugging requires writing a Primeval log, since trace logs aren't available until the Tingen Session is initialized.
             * This will probably be used relatively often during developement, so it's worth keeping around, but it should be
             * commneted out in production.
             */
            LogEvent.Primeval(Assembly.GetExecutingAssembly().GetName().Name);

            /* The only difference between the development and production versions of Tingen is the hardcoded Avatar System Code. For
             * development, the Avatar System Code is "UAT". For production, the Avatar System Code is "LIVE"
             */
            const string avatarSystemCode = "UAT";
            string configFilePath         = $@"C:\TingenData\{avatarSystemCode}\Config\Tingen.config";

            TingenSession tnSession = TingenSession.Build(sentOptionObject, sentScriptParameter, avatarSystemCode, configFilePath);

            TingenSession.Initialize(tnSession);

            /* Logging is done a little different in this method, since the Tingen Session is not yet initialized. We'll get the
             * AssemblyName here instead of at the top of the method.
             */
            string assemblyName = Assembly.GetExecutingAssembly().GetName().Name;

            LogEvent.Trace(1, assemblyName, tnSession.TraceInfo);

            switch (tnSession.Config.TingenMode)
            {
                case "disabled":
                    LogEvent.Trace(2, assemblyName, tnSession.TraceInfo);

                    /* If Tingen is disabled, we should update the service status files so the necessary users are notified. When
                     * Tingen is re-enabled, the service status files will need to be manually updated using the Admin Module.
                     */
                    Outpost31.Module.Admin.Service.AllUpdate(tnSession);

                    /* Normally we would let Outpost31.Core.OptionObject handle this, but since we really want to limit what is done
                     * while Tingen is disabled, we'll just clone the SentObject to the ReturnObject and return it.
                     */
                    tnSession.AvatarData.ReturnObject = tnSession.AvatarData.SentObject.Clone();

                    break;

                default:
                    LogEvent.Trace(2, assemblyName, tnSession.TraceInfo);

                    Outpost31.Core.Roundhouse.Parse(tnSession);

                    tnSession.AvatarData.ReturnObject = tnSession.AvatarData.WorkObject.Clone();

                    var path = $@"{tnSession.Framework.SystemCodePath.Session}\Session.md";

                    File.WriteAllText(path, Catalog.SessionDetails(tnSession));

                    LogEvent.Trace(2, assemblyName, tnSession.TraceInfo);

                    break;
            }

            LogEvent.Primeval(Assembly.GetExecutingAssembly().GetName().Name);

            return tnSession.AvatarData.ReturnObject;
        }
    }
}

/*

Development notes
-----------------

- Is there a efficient way to automatically update the service status files when Tingen is re-enabled?

*/