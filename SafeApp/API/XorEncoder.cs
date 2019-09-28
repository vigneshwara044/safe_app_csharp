using System.Threading.Tasks;
using SafeApp.AppBindings;
using SafeApp.Core;

namespace SafeApp.API
{
    public static class XorEncoder
    {
        static readonly IAppBindings AppBindings = AppResolver.Current;

        public static Task<string> EncodeAsync(
            byte[] xorName,
            ulong typeTag,
            ulong dataType,
            ushort contentType,
            string path,
            string subNames,
            ulong contentVersion,
            string baseEncoding)
            => AppBindings.XorurlEncodeAsync(xorName, typeTag, dataType, contentType, path, subNames, contentVersion, baseEncoding);

        public static Task<XorUrlEncoder> EncodeAsync(
            byte[] xorName,
            ulong typeTag,
            ulong dataType,
            ushort contentType,
            string path,
            string subNames,
            ulong contentVersion)
            => AppBindings.XorurlEncoderAsync(xorName, typeTag, dataType, contentType, path, subNames, contentVersion);

        public static Task<XorUrlEncoder> XorUrlEncoderFromUrl(string xorUrl)
            => AppBindings.XorurlEncoderFromUrlAsync(xorUrl);
    }
}
