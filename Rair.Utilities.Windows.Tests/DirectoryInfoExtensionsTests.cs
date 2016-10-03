using System;
using System.IO;
using Rair.Utilities.Windows.Extensions;
using Xunit;

namespace Rair.Utilities.Windows.Tests
{
    public class DirectoryInfoExtensionsTests
    {
        [Fact()]
        public void GetSubFolderTest()
        {
            var dir = new DirectoryInfo(Path.GetTempPath());
            var sub = dir.GetSubFolder("Tester");
            Assert.NotNull(sub);
        }

        [Fact()]
        public void GetFileTest()
        {
            var dir = new DirectoryInfo(Path.GetTempPath());
            var file = dir.GetFile("TesterFile");
            Assert.NotNull(file);
        }

        [Fact()]
        public void RemoveTest()
        {
            var dir = new DirectoryInfo(Path.GetTempPath());
            dir.Create();
            var res = dir.Remove();
            Assert.Equal(res.IsFailure, true);
        }
    }
}