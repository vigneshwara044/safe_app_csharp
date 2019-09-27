using System;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using SafeApp.Core;

#if __IOS__
using ObjCRuntime;
#endif

namespace SafeApp.AppBindings
{
    internal partial class AppBindings : IAppBindings
    {
        #region Connect
        public void Connect(
            string appId,
            string authCredentials,
            Action<FfiResult, IntPtr, GCHandle> oCb)
        {
            var userData = BindingUtils.ToHandlePtr(oCb);
            ConnectNative(appId, authCredentials, userData, DelegateOnFfiResultSafeCb);
        }

        [DllImport(DllName, EntryPoint = "connect")]
        private static extern void ConnectNative(
            [MarshalAs(UnmanagedType.LPStr)] string appId,
            [MarshalAs(UnmanagedType.LPStr)] string authCredentials,
            IntPtr userData,
            FfiResultSafeCb oCb);

        private delegate void FfiResultSafeCb(IntPtr userData, IntPtr result, IntPtr app);

#if __IOS__
        [MonoPInvokeCallback(typeof(FfiResultSafeCb))]
#endif
        private static void OnFfiResultSafeCb(IntPtr userData, IntPtr result, IntPtr app)
        {
            var action = BindingUtils.FromHandlePtr<Action<FfiResult, IntPtr, GCHandle>>(userData, false);
            action(Marshal.PtrToStructure<FfiResult>(result), app, GCHandle.FromIntPtr(userData));
        }

        private static readonly FfiResultSafeCb DelegateOnFfiResultSafeCb = OnFfiResultSafeCb;

        #endregion

        #region XorUrl
        public Task<string> XorurlEncodeAsync(
            ref byte[] name,
            ulong typeTag,
            ulong dataType,
            ushort contentType,
            string path,
            string subNames,
            ulong contentVersion,
            string baseEncoding)
        {
            var (ret, userData) = BindingUtils.PrepareTask<string>();
            XorurlEncodeNative(
                ref name,
                typeTag,
                dataType,
                contentType,
                path,
                subNames,
                contentVersion,
                baseEncoding,
                userData,
                DelegateOnFfiResultStringCb);
            return ret;
        }

        [DllImport(DllName, EntryPoint = "xorurl_encode")]
        private static extern void XorurlEncodeNative(
            ref byte[] name,
            ulong typeTag,
            ulong dataType,
            ushort contentType,
            [MarshalAs(UnmanagedType.LPStr)] string path,
            [MarshalAs(UnmanagedType.LPStr)] string subNames,
            ulong contentVersion,
            [MarshalAs(UnmanagedType.LPStr)] string baseEncoding,
            IntPtr userData,
            FfiResultStringCb oCb);

        public Task<XorUrlEncoder> XorurlEncoderAsync(
            ref byte[] name,
            ulong typeTag,
            ulong dataType,
            ushort contentType,
            string path,
            string subNames,
            ulong contentVersion)
        {
            var (ret, userData) = BindingUtils.PrepareTask<XorUrlEncoder>();
            XorurlEncoderNative(
                ref name,
                typeTag,
                dataType,
                contentType,
                path,
                subNames,
                contentVersion,
                userData,
                DelegateOnFfiResultXorUrlEncoderCb);
            return ret;
        }

        [DllImport(DllName, EntryPoint = "xorurl_encoder")]
        private static extern void XorurlEncoderNative(
            ref byte[] name,
            ulong typeTag,
            ulong dataType,
            ushort contentType,
            [MarshalAs(UnmanagedType.LPStr)] string path,
            [MarshalAs(UnmanagedType.LPStr)] string subNames,
            ulong contentVersion,
            IntPtr userData,
            FfiResultXorUrlEncoderCb oCb);

        public Task<XorUrlEncoder> XorurlEncoderFromUrlAsync(string xorUrl)
        {
            var (ret, userData) = BindingUtils.PrepareTask<XorUrlEncoder>();
            XorurlEncoderFromUrlNative(xorUrl, userData, DelegateOnFfiResultXorUrlEncoderCb);
            return ret;
        }

        [DllImport(DllName, EntryPoint = "xorurl_encoder_from_url")]
        private static extern void XorurlEncoderFromUrlNative(
            [MarshalAs(UnmanagedType.LPStr)] string xorUrl,
            IntPtr userData,
            FfiResultXorUrlEncoderCb oCb);

        private delegate void FfiResultXorUrlEncoderCb(IntPtr userData, IntPtr result, IntPtr xorurlEncoder);

#if __IOS__
        [MonoPInvokeCallback(typeof(FfiResultXorUrlEncoderCb))]
#endif
        private static void OnFfiResultXorUrlEncoderCb(IntPtr userData, IntPtr result, IntPtr xorurlEncoder)
        {
            BindingUtils.CompleteTask(
                userData,
                Marshal.PtrToStructure<FfiResult>(result),
                () => Marshal.PtrToStructure<XorUrlEncoder>(xorurlEncoder));
        }

        private static readonly FfiResultXorUrlEncoderCb DelegateOnFfiResultXorUrlEncoderCb = OnFfiResultXorUrlEncoderCb;

        #endregion
    }
}
