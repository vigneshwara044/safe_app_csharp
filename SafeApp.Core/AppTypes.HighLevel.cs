using System.Runtime.InteropServices;
using JetBrains.Annotations;

namespace SafeApp.Core
{
    [PublicAPI]
    public struct XorUrlEncoder
    {
        public ulong EncodingVersion;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = (int)AppConstants.XorNameLen)]
        public byte[] Xorname;
        public ulong TypeTag;
        public ulong DataType;
        public ushort ContentType;
        [MarshalAs(UnmanagedType.LPStr)]
        public string Path;
        [MarshalAs(UnmanagedType.LPStr)]
        public string SubNames;
        public ulong ContentVersion;
    }
}
