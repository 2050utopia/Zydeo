﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZD.AU
{
    /// <summary>
    /// All kinds of magic constants.
    /// </summary>
    internal class Magic
    {
        /// <summary>
        /// Name of update service's log file in the TEMP folder.
        /// </summary>
        public static readonly string SvcLogFileName = "Zydeo.v{0}.{1}.update.log";

        /// <summary>
        /// Name of the update UI's log file, in the user's Zydeo appdata folder.
        /// </summary>
        public static readonly string GuiLogFileName = "Update.log";

        /// <summary>
        /// Short name of AU helper service.
        /// </summary>
        public static readonly string ServiceShortName = "ZydeoUpdateService";

        /// <summary>
        /// Display name of AU helper service.
        /// </summary>
        public static readonly string ServiceDisplayName = "Zydeo Update Helper";

        /// <summary>
        /// Prefix prepended to random-named EXE copies in TEMP folder.
        /// </summary>
        public static readonly string TempCopyPrefix = "ZydAU-";

        /// <summary>
        /// Zydeo folder within the user's appdata folder.
        /// </summary>
        /// <remarks>Keep in sync with <see cref="ZD.Gui.Magic.ZydeoUserFolder"/>.</remarks>
        public static readonly string ZydeoUserFolder = "Zydeo";

        /// <summary>
        /// Name of file storing info about latest update in user's appdata folder.
        /// </summary>
        public static readonly string ZydeoUpdateInfoFile = "Update.xml";

        /// <summary>
        /// Zydeo's registry key under HKLM\Software.
        /// </summary>
        public static readonly string ZydeoSoftwareRegKey = "Zydeo";

        /// <summary>
        /// Name of DWORD value, in Zydeo's registry key, for salt.
        /// </summary>
        public static readonly string ZydeoSaltRegVal = "Salt";

        /// <summary>
        /// URL of Zydeo site to upen (if update fails and user clicks link)
        /// </summary>
        public static readonly string ZydeoSiteUrl = "zydeo.net";

        /// <summary>
        /// URL that returns info about available updates.
        /// </summary>
        public static readonly string UpdateCheckUrl = "http://zydeo.net/autoupdate";

        /// <summary>
        /// POSTDATA to send to update URL.
        /// </summary>
        public static readonly string UpdatePostPattern = "product={0}&salt={1:X8}&vmaj={2}&vmin={3}&winver={4}";

        /// <summary>
        /// Product for which we request update from URL. Same URL may be serving other products later.
        /// </summary>
        public static readonly string UpdateProduct = "Zydeo";

        /// <summary>
        /// Timeout, in seconds, after which download fails.
        /// </summary>
        public static readonly double DownloadTimeoutSec = 30;

        /// <summary>
        /// <para>How long the service waits for a connection from update client, in msec.</para>
        /// <para>If client doesn't show up in time, process just quits.</para>
        /// <para>Same number is used for client-side timeout (how long updater UI waits for connection).</para>
        /// </summary>
        public static readonly int ServicePipeTimeoutMsec = 20000;

        /// <summary>
        /// Returned through named pipe by update service upon failure.
        /// </summary>
        public static readonly byte SrvCodeFail = 0xff;

        /// <summary>
        /// Returned through named pipe by update service when it launches installer.
        /// </summary>
        public static readonly byte SrvCodeInstallStarted = 0x01;

        /// <summary>
        /// Returned through named pipe by update service upon success (installer finished).
        /// </summary>
        public static readonly byte SrvCodeSuccess = 0x00;
    }
}
