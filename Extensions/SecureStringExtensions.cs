using System.Runtime.InteropServices;
using System.Security;

namespace Rair.Utilities.Windows.Extensions
{
    public static class SecureStringExtensions
    {
        public static string ToPlain(this SecureString value)
        {
            var bstr = Marshal.SecureStringToBSTR(value);

            try
            {
                return Marshal.PtrToStringBSTR(bstr);
            }
            finally
            {
                Marshal.FreeBSTR(bstr);
            }
        }
    }
}
