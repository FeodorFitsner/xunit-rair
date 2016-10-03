using System;
using System.IO;
using System.Linq;
using Rair.Utilities.Core;

namespace Rair.Utilities.Windows.Extensions
{
    public static class DirectoryInfoExtensions
    {
        public static DirectoryInfo GetSubFolder(this DirectoryInfo dir, string folder, params string[] folders)
        {
            if (dir == null) return null;

            var res = Path.Combine(dir.FullName, folder);
            res = folders.Aggregate(res, Path.Combine);

            return new DirectoryInfo(res);
        }

        public static FileInfo GetFile(this DirectoryInfo dir, string fileName)
        {
            return new FileInfo(Path.Combine(dir.FullName, fileName));
        }

        public static Result Remove(this DirectoryInfo dir, bool recursive = true)
        {
            try
            {
                if (!dir.Exists) return Result.Nothing($"{dir.FullName} doesn't exist.");
                dir.Delete(recursive);
                return dir.Exists
                    ? Result.Failure($"{dir.FullName} still exists")
                    : Result.Success($"{dir.FullName} deleted");
            }
            catch (Exception ex)
            {
                return Result.Failure(ex);
            }
        }
    }
}
