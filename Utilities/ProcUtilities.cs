using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Rair.Utilities.Core;
using Rair.Utilities.Core.Extensions;

namespace Rair.Utilities.Windows.Utilities
{
    public static class ProcUtilities
    {
        [DllImport("Shlwapi.dll", SetLastError = true, CharSet = CharSet.Auto)]
        private static extern uint AssocQueryString(AssocF flags, AssocStr str, string pszAssoc, string pszExtra, [Out] StringBuilder pszOut, [In][Out] ref uint pcchOut);[Flags]

        public enum AssocF
        {
            InitNoRemapClsid = 0x1,
            InitByExeName = 0x2,
            OpenByExeName = 0x2,
            InitDefaultToStar = 0x4,
            InitDefaultToFolder = 0x8,
            NoUserSettings = 0x10,
            NoTruncate = 0x20,
            Verify = 0x40,
            RemapRunDll = 0x80,
            NoFixUps = 0x100,
            IgnoreBaseClass = 0x200
        }

        public enum AssocStr
        {
            Command = 1,
            Executable,
            FriendlyDocName,
            FriendlyAppName,
            NoOpen,
            ShellNewValue,
            DDECommand,
            DDEIfExec,
            DDEApplication,
            DDETopic
        }

        public static Result StartProcess(string file)
        {
            try
            {
                var fi = new FileInfo(file);
                if (FileExtensionHasAssociattion(fi))
                {
                    Process.Start(file);
                    return Result.Success();
                }

                return Result.Failure("No associated program");
            }
            catch (Exception ex)
            {
                return Result.Failure(ex);
            }
        }

        private static bool FileExtensionHasAssociattion(FileInfo file)
        {
            var assocProc = FileExtentionInfo(AssocStr.Executable, file.Extension);
            return !assocProc.IsEmpty();
        }

        private static string FileExtentionInfo(AssocStr assocStr, string doctype)
        {
            uint pcchOut = 0;
            AssocQueryString(AssocF.Verify, assocStr, doctype, null, null, ref pcchOut);

            var pszOut = new StringBuilder((int)pcchOut);
            AssocQueryString(AssocF.Verify, assocStr, doctype, null, pszOut, ref pcchOut);
            return pszOut.ToString();
        }
    }
}
