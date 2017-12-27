#if !NETSTANDARD1_2 || __DESKTOP__
using System;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using SafeApp.Utilities;

#if __IOS__
using ObjCRuntime;
#endif

namespace SafeApp.AppBindings {
  public partial class AppBindings : IAppBindings {
#if __IOS__
    internal const string DllName = "__Internal";
#else
    internal const string DllName = "safe_app";
#endif

    public bool IsMockBuild() {
      var ret = IsMockBuildNative();
      return ret;
    }

    [DllImport(DllName, EntryPoint = "is_mock_build")]
    internal static extern bool IsMockBuildNative();

    [DllImport(DllName, EntryPoint = "app_unregistered")]
    internal static extern void AppUnregisteredNative(
      [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1)] byte[] bootstrapConfig,
      IntPtr bootstrapConfigLen,
      IntPtr userData,
      NoneCb oDisconnectNotifierCb,
      FfiResultAppCb oCb);

    [DllImport(DllName, EntryPoint = "app_registered")]
    internal static extern void AppRegisteredNative(
      [MarshalAs(UnmanagedType.LPStr)] string appId,
      ref AuthGrantedNative authGranted,
      IntPtr userData,
      NoneCb oDisconnectNotifierCb,
      FfiResultAppCb oCb);

    public Task AppReconnectAsync(IntPtr app) {
      var (ret, userData) = BindingUtils.PrepareTask();
      AppReconnectNative(app, userData, OnFfiResultCb);
      return ret;
    }

    [DllImport(DllName, EntryPoint = "app_reconnect")]
    internal static extern void AppReconnectNative(IntPtr app, IntPtr userData, FfiResultCb oCb);

    public Task<AccountInfo> AppAccountInfoAsync(IntPtr app) {
      var (ret, userData) = BindingUtils.PrepareTask<AccountInfo>();
      AppAccountInfoNative(app, userData, OnFfiResultAccountInfoCb);
      return ret;
    }

    [DllImport(DllName, EntryPoint = "app_account_info")]
    internal static extern void AppAccountInfoNative(IntPtr app, IntPtr userData, FfiResultAccountInfoCb oCb);

    public Task<string> AppExeFileStemAsync() {
      var (ret, userData) = BindingUtils.PrepareTask<string>();
      AppExeFileStemNative(userData, OnFfiResultStringCb);
      return ret;
    }

    [DllImport(DllName, EntryPoint = "app_exe_file_stem")]
    internal static extern void AppExeFileStemNative(IntPtr userData, FfiResultStringCb oCb);

    public Task AppSetAdditionalSearchPathAsync(string newPath) {
      var (ret, userData) = BindingUtils.PrepareTask();
      AppSetAdditionalSearchPathNative(newPath, userData, OnFfiResultCb);
      return ret;
    }

    [DllImport(DllName, EntryPoint = "app_set_additional_search_path")]
    internal static extern void AppSetAdditionalSearchPathNative(
      [MarshalAs(UnmanagedType.LPStr)] string newPath,
      IntPtr userData,
      FfiResultCb oCb);

    public void AppFree(IntPtr app) {
      AppFreeNative(app);
    }

    [DllImport(DllName, EntryPoint = "app_free")]
    internal static extern void AppFreeNative(IntPtr app);

    public Task AppResetObjectCacheAsync(IntPtr app) {
      var (ret, userData) = BindingUtils.PrepareTask();
      AppResetObjectCacheNative(app, userData, OnFfiResultCb);
      return ret;
    }

    [DllImport(DllName, EntryPoint = "app_reset_object_cache")]
    internal static extern void AppResetObjectCacheNative(IntPtr app, IntPtr userData, FfiResultCb oCb);

    public Task<string> AppContainerNameAsync(string appId) {
      var (ret, userData) = BindingUtils.PrepareTask<string>();
      AppContainerNameNative(appId, userData, OnFfiResultStringCb);
      return ret;
    }

    [DllImport(DllName, EntryPoint = "app_container_name")]
    internal static extern void AppContainerNameNative(
      [MarshalAs(UnmanagedType.LPStr)] string appId,
      IntPtr userData,
      FfiResultStringCb oCb);

    public Task AccessContainerRefreshAccessInfoAsync(IntPtr app) {
      var (ret, userData) = BindingUtils.PrepareTask();
      AccessContainerRefreshAccessInfoNative(app, userData, OnFfiResultCb);
      return ret;
    }

    [DllImport(DllName, EntryPoint = "access_container_refresh_access_info")]
    internal static extern void AccessContainerRefreshAccessInfoNative(IntPtr app, IntPtr userData, FfiResultCb oCb);

    public Task<ContainerPermissions[]> AccessContainerFetchAsync(IntPtr app) {
      var (ret, userData) = BindingUtils.PrepareTask<ContainerPermissions[]>();
      AccessContainerFetchNative(app, userData, OnFfiResultContainerPermissionsListCb);
      return ret;
    }

    [DllImport(DllName, EntryPoint = "access_container_fetch")]
    internal static extern void AccessContainerFetchNative(IntPtr app, IntPtr userData, FfiResultContainerPermissionsListCb oCb);

    public Task<MDataInfo> AccessContainerGetContainerMDataInfoAsync(IntPtr app, string name) {
      var (ret, userData) = BindingUtils.PrepareTask<MDataInfo>();
      AccessContainerGetContainerMDataInfoNative(app, name, userData, OnFfiResultMDataInfoCb);
      return ret;
    }

    [DllImport(DllName, EntryPoint = "access_container_get_container_mdata_info")]
    internal static extern void AccessContainerGetContainerMDataInfoNative(
      IntPtr app,
      [MarshalAs(UnmanagedType.LPStr)] string name,
      IntPtr userData,
      FfiResultMDataInfoCb oCb);

    public Task<ulong> CipherOptNewPlaintextAsync(IntPtr app) {
      var (ret, userData) = BindingUtils.PrepareTask<ulong>();
      CipherOptNewPlaintextNative(app, userData, OnFfiResultULongCb);
      return ret;
    }

    [DllImport(DllName, EntryPoint = "cipher_opt_new_plaintext")]
    internal static extern void CipherOptNewPlaintextNative(IntPtr app, IntPtr userData, FfiResultULongCb oCb);

    public Task<ulong> CipherOptNewSymmetricAsync(IntPtr app) {
      var (ret, userData) = BindingUtils.PrepareTask<ulong>();
      CipherOptNewSymmetricNative(app, userData, OnFfiResultULongCb);
      return ret;
    }

    [DllImport(DllName, EntryPoint = "cipher_opt_new_symmetric")]
    internal static extern void CipherOptNewSymmetricNative(IntPtr app, IntPtr userData, FfiResultULongCb oCb);

    public Task<ulong> CipherOptNewAsymmetricAsync(IntPtr app, ulong peerEncryptKeyH) {
      var (ret, userData) = BindingUtils.PrepareTask<ulong>();
      CipherOptNewAsymmetricNative(app, peerEncryptKeyH, userData, OnFfiResultULongCb);
      return ret;
    }

    [DllImport(DllName, EntryPoint = "cipher_opt_new_asymmetric")]
    internal static extern void CipherOptNewAsymmetricNative(IntPtr app, ulong peerEncryptKeyH, IntPtr userData, FfiResultULongCb oCb);

    public Task CipherOptFreeAsync(IntPtr app, ulong handle) {
      var (ret, userData) = BindingUtils.PrepareTask();
      CipherOptFreeNative(app, handle, userData, OnFfiResultCb);
      return ret;
    }

    [DllImport(DllName, EntryPoint = "cipher_opt_free")]
    internal static extern void CipherOptFreeNative(IntPtr app, ulong handle, IntPtr userData, FfiResultCb oCb);

    public Task<ulong> AppPubSignKeyAsync(IntPtr app) {
      var (ret, userData) = BindingUtils.PrepareTask<ulong>();
      AppPubSignKeyNative(app, userData, OnFfiResultULongCb);
      return ret;
    }

    [DllImport(DllName, EntryPoint = "app_pub_sign_key")]
    internal static extern void AppPubSignKeyNative(IntPtr app, IntPtr userData, FfiResultULongCb oCb);

    public Task<(ulong, ulong)> SignGenerateKeyPairAsync(IntPtr app) {
      var (ret, userData) = BindingUtils.PrepareTask<(ulong, ulong)>();
      SignGenerateKeyPairNative(app, userData, OnFfiResultULongULongCb);
      return ret;
    }

    [DllImport(DllName, EntryPoint = "sign_generate_key_pair")]
    internal static extern void SignGenerateKeyPairNative(IntPtr app, IntPtr userData, FfiResultULongULongCb oCb);

    public Task<ulong> SignPubKeyNewAsync(IntPtr app, IntPtr data) {
      var (ret, userData) = BindingUtils.PrepareTask<ulong>();
      SignPubKeyNewNative(app, data, userData, OnFfiResultULongCb);
      return ret;
    }

    [DllImport(DllName, EntryPoint = "sign_pub_key_new")]
    internal static extern void SignPubKeyNewNative(IntPtr app, IntPtr data, IntPtr userData, FfiResultULongCb oCb);

    public Task<IntPtr> SignPubKeyGetAsync(IntPtr app, ulong handle) {
      var (ret, userData) = BindingUtils.PrepareTask<IntPtr>();
      SignPubKeyGetNative(app, handle, userData, OnFfiResultByteArraySignPublicKeyLenCb);
      return ret;
    }

    [DllImport(DllName, EntryPoint = "sign_pub_key_get")]
    internal static extern void SignPubKeyGetNative(IntPtr app, ulong handle, IntPtr userData, FfiResultByteArraySignPublicKeyLenCb oCb);

    public Task SignPubKeyFreeAsync(IntPtr app, ulong handle) {
      var (ret, userData) = BindingUtils.PrepareTask();
      SignPubKeyFreeNative(app, handle, userData, OnFfiResultCb);
      return ret;
    }

    [DllImport(DllName, EntryPoint = "sign_pub_key_free")]
    internal static extern void SignPubKeyFreeNative(IntPtr app, ulong handle, IntPtr userData, FfiResultCb oCb);

    public Task<ulong> SignSecKeyNewAsync(IntPtr app, IntPtr data) {
      var (ret, userData) = BindingUtils.PrepareTask<ulong>();
      SignSecKeyNewNative(app, data, userData, OnFfiResultULongCb);
      return ret;
    }

    [DllImport(DllName, EntryPoint = "sign_sec_key_new")]
    internal static extern void SignSecKeyNewNative(IntPtr app, IntPtr data, IntPtr userData, FfiResultULongCb oCb);

    public Task<IntPtr> SignSecKeyGetAsync(IntPtr app, ulong handle) {
      var (ret, userData) = BindingUtils.PrepareTask<IntPtr>();
      SignSecKeyGetNative(app, handle, userData, OnFfiResultByteArraySignSecretKeyLenCb);
      return ret;
    }

    [DllImport(DllName, EntryPoint = "sign_sec_key_get")]
    internal static extern void SignSecKeyGetNative(IntPtr app, ulong handle, IntPtr userData, FfiResultByteArraySignSecretKeyLenCb oCb);

    public Task SignSecKeyFreeAsync(IntPtr app, ulong handle) {
      var (ret, userData) = BindingUtils.PrepareTask();
      SignSecKeyFreeNative(app, handle, userData, OnFfiResultCb);
      return ret;
    }

    [DllImport(DllName, EntryPoint = "sign_sec_key_free")]
    internal static extern void SignSecKeyFreeNative(IntPtr app, ulong handle, IntPtr userData, FfiResultCb oCb);

    public Task<ulong> AppPubEncKeyAsync(IntPtr app) {
      var (ret, userData) = BindingUtils.PrepareTask<ulong>();
      AppPubEncKeyNative(app, userData, OnFfiResultULongCb);
      return ret;
    }

    [DllImport(DllName, EntryPoint = "app_pub_enc_key")]
    internal static extern void AppPubEncKeyNative(IntPtr app, IntPtr userData, FfiResultULongCb oCb);

    public Task<(ulong, ulong)> EncGenerateKeyPairAsync(IntPtr app) {
      var (ret, userData) = BindingUtils.PrepareTask<(ulong, ulong)>();
      EncGenerateKeyPairNative(app, userData, OnFfiResultULongULongCb);
      return ret;
    }

    [DllImport(DllName, EntryPoint = "enc_generate_key_pair")]
    internal static extern void EncGenerateKeyPairNative(IntPtr app, IntPtr userData, FfiResultULongULongCb oCb);

    public Task<ulong> EncPubKeyNewAsync(IntPtr app, IntPtr data) {
      var (ret, userData) = BindingUtils.PrepareTask<ulong>();
      EncPubKeyNewNative(app, data, userData, OnFfiResultULongCb);
      return ret;
    }

    [DllImport(DllName, EntryPoint = "enc_pub_key_new")]
    internal static extern void EncPubKeyNewNative(IntPtr app, IntPtr data, IntPtr userData, FfiResultULongCb oCb);

    public Task<IntPtr> EncPubKeyGetAsync(IntPtr app, ulong handle) {
      var (ret, userData) = BindingUtils.PrepareTask<IntPtr>();
      EncPubKeyGetNative(app, handle, userData, OnFfiResultByteArrayAsymPublicKeyLenCb);
      return ret;
    }

    [DllImport(DllName, EntryPoint = "enc_pub_key_get")]
    internal static extern void EncPubKeyGetNative(IntPtr app, ulong handle, IntPtr userData, FfiResultByteArrayAsymPublicKeyLenCb oCb);

    public Task EncPubKeyFreeAsync(IntPtr app, ulong handle) {
      var (ret, userData) = BindingUtils.PrepareTask();
      EncPubKeyFreeNative(app, handle, userData, OnFfiResultCb);
      return ret;
    }

    [DllImport(DllName, EntryPoint = "enc_pub_key_free")]
    internal static extern void EncPubKeyFreeNative(IntPtr app, ulong handle, IntPtr userData, FfiResultCb oCb);

    public Task<ulong> EncSecretKeyNewAsync(IntPtr app, IntPtr data) {
      var (ret, userData) = BindingUtils.PrepareTask<ulong>();
      EncSecretKeyNewNative(app, data, userData, OnFfiResultULongCb);
      return ret;
    }

    [DllImport(DllName, EntryPoint = "enc_secret_key_new")]
    internal static extern void EncSecretKeyNewNative(IntPtr app, IntPtr data, IntPtr userData, FfiResultULongCb oCb);

    public Task<IntPtr> EncSecretKeyGetAsync(IntPtr app, ulong handle) {
      var (ret, userData) = BindingUtils.PrepareTask<IntPtr>();
      EncSecretKeyGetNative(app, handle, userData, OnFfiResultByteArrayAsymSecretKeyLenCb);
      return ret;
    }

    [DllImport(DllName, EntryPoint = "enc_secret_key_get")]
    internal static extern void EncSecretKeyGetNative(IntPtr app, ulong handle, IntPtr userData, FfiResultByteArrayAsymSecretKeyLenCb oCb);

    public Task EncSecretKeyFreeAsync(IntPtr app, ulong handle) {
      var (ret, userData) = BindingUtils.PrepareTask();
      EncSecretKeyFreeNative(app, handle, userData, OnFfiResultCb);
      return ret;
    }

    [DllImport(DllName, EntryPoint = "enc_secret_key_free")]
    internal static extern void EncSecretKeyFreeNative(IntPtr app, ulong handle, IntPtr userData, FfiResultCb oCb);

    public Task<byte[]> SignAsync(IntPtr app, IntPtr data, IntPtr len, ulong signSkH) {
      var (ret, userData) = BindingUtils.PrepareTask<byte[]>();
      SignNative(app, data, len, signSkH, userData, OnFfiResultByteListCb);
      return ret;
    }

    [DllImport(DllName, EntryPoint = "sign")]
    internal static extern void SignNative(IntPtr app, IntPtr data, IntPtr len, ulong signSkH, IntPtr userData, FfiResultByteListCb oCb);

    public Task<byte[]> VerifyAsync(IntPtr app, IntPtr signedData, IntPtr len, ulong signPkH) {
      var (ret, userData) = BindingUtils.PrepareTask<byte[]>();
      VerifyNative(app, signedData, len, signPkH, userData, OnFfiResultByteListCb);
      return ret;
    }

    [DllImport(DllName, EntryPoint = "verify")]
    internal static extern void VerifyNative(
      IntPtr app,
      IntPtr signedData,
      IntPtr len,
      ulong signPkH,
      IntPtr userData,
      FfiResultByteListCb oCb);

    public Task<byte[]> EncryptAsync(IntPtr app, IntPtr data, IntPtr len, ulong pkH, ulong skH) {
      var (ret, userData) = BindingUtils.PrepareTask<byte[]>();
      EncryptNative(app, data, len, pkH, skH, userData, OnFfiResultByteListCb);
      return ret;
    }

    [DllImport(DllName, EntryPoint = "encrypt")]
    internal static extern void EncryptNative(
      IntPtr app,
      IntPtr data,
      IntPtr len,
      ulong pkH,
      ulong skH,
      IntPtr userData,
      FfiResultByteListCb oCb);

    public Task<byte[]> DecryptAsync(IntPtr app, IntPtr data, IntPtr len, ulong pkH, ulong skH) {
      var (ret, userData) = BindingUtils.PrepareTask<byte[]>();
      DecryptNative(app, data, len, pkH, skH, userData, OnFfiResultByteListCb);
      return ret;
    }

    [DllImport(DllName, EntryPoint = "decrypt")]
    internal static extern void DecryptNative(
      IntPtr app,
      IntPtr data,
      IntPtr len,
      ulong pkH,
      ulong skH,
      IntPtr userData,
      FfiResultByteListCb oCb);

    public Task<byte[]> EncryptSealedBoxAsync(IntPtr app, IntPtr data, IntPtr len, ulong pkH) {
      var (ret, userData) = BindingUtils.PrepareTask<byte[]>();
      EncryptSealedBoxNative(app, data, len, pkH, userData, OnFfiResultByteListCb);
      return ret;
    }

    [DllImport(DllName, EntryPoint = "encrypt_sealed_box")]
    internal static extern void EncryptSealedBoxNative(
      IntPtr app,
      IntPtr data,
      IntPtr len,
      ulong pkH,
      IntPtr userData,
      FfiResultByteListCb oCb);

    public Task<byte[]> DecryptSealedBoxAsync(IntPtr app, IntPtr data, IntPtr len, ulong pkH, ulong skH) {
      var (ret, userData) = BindingUtils.PrepareTask<byte[]>();
      DecryptSealedBoxNative(app, data, len, pkH, skH, userData, OnFfiResultByteListCb);
      return ret;
    }

    [DllImport(DllName, EntryPoint = "decrypt_sealed_box")]
    internal static extern void DecryptSealedBoxNative(
      IntPtr app,
      IntPtr data,
      IntPtr len,
      ulong pkH,
      ulong skH,
      IntPtr userData,
      FfiResultByteListCb oCb);

    public Task<byte[]> Sha3HashAsync(IntPtr data, IntPtr len) {
      var (ret, userData) = BindingUtils.PrepareTask<byte[]>();
      Sha3HashNative(data, len, userData, OnFfiResultByteListCb);
      return ret;
    }

    [DllImport(DllName, EntryPoint = "sha3_hash")]
    internal static extern void Sha3HashNative(IntPtr data, IntPtr len, IntPtr userData, FfiResultByteListCb oCb);

    public Task<IntPtr> GenerateNonceAsync() {
      var (ret, userData) = BindingUtils.PrepareTask<IntPtr>();
      GenerateNonceNative(userData, OnFfiResultByteArrayAsymNonceLenCb);
      return ret;
    }

    [DllImport(DllName, EntryPoint = "generate_nonce")]
    internal static extern void GenerateNonceNative(IntPtr userData, FfiResultByteArrayAsymNonceLenCb oCb);

    public Task<ulong> IDataNewSelfEncryptorAsync(IntPtr app) {
      var (ret, userData) = BindingUtils.PrepareTask<ulong>();
      IDataNewSelfEncryptorNative(app, userData, OnFfiResultULongCb);
      return ret;
    }

    [DllImport(DllName, EntryPoint = "idata_new_self_encryptor")]
    internal static extern void IDataNewSelfEncryptorNative(IntPtr app, IntPtr userData, FfiResultULongCb oCb);

    public Task IDataWriteToSelfEncryptorAsync(IntPtr app, ulong seH, byte[] data) {
      var (ret, userData) = BindingUtils.PrepareTask();
      IDataWriteToSelfEncryptorNative(app, seH, data, (IntPtr)data.Length, userData, OnFfiResultCb);
      return ret;
    }

    [DllImport(DllName, EntryPoint = "idata_write_to_self_encryptor")]
    internal static extern void IDataWriteToSelfEncryptorNative(
      IntPtr app,
      ulong seH,
      [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 3)] byte[] data,
      IntPtr dataLen,
      IntPtr userData,
      FfiResultCb oCb);

    public Task<IntPtr> IDataCloseSelfEncryptorAsync(IntPtr app, ulong seH, ulong cipherOptH) {
      var (ret, userData) = BindingUtils.PrepareTask<IntPtr>();
      IDataCloseSelfEncryptorNative(app, seH, cipherOptH, userData, OnFfiResultByteArrayXorNameLenCb);
      return ret;
    }

    [DllImport(DllName, EntryPoint = "idata_close_self_encryptor")]
    internal static extern void IDataCloseSelfEncryptorNative(
      IntPtr app,
      ulong seH,
      ulong cipherOptH,
      IntPtr userData,
      FfiResultByteArrayXorNameLenCb oCb);

    public Task<ulong> IDataFetchSelfEncryptorAsync(IntPtr app, IntPtr name) {
      var (ret, userData) = BindingUtils.PrepareTask<ulong>();
      IDataFetchSelfEncryptorNative(app, name, userData, OnFfiResultULongCb);
      return ret;
    }

    [DllImport(DllName, EntryPoint = "idata_fetch_self_encryptor")]
    internal static extern void IDataFetchSelfEncryptorNative(IntPtr app, IntPtr name, IntPtr userData, FfiResultULongCb oCb);

    public Task<ulong> IDataSerialisedSizeAsync(IntPtr app, IntPtr name) {
      var (ret, userData) = BindingUtils.PrepareTask<ulong>();
      IDataSerialisedSizeNative(app, name, userData, OnFfiResultULongCb);
      return ret;
    }

    [DllImport(DllName, EntryPoint = "idata_serialised_size")]
    internal static extern void IDataSerialisedSizeNative(IntPtr app, IntPtr name, IntPtr userData, FfiResultULongCb oCb);

    public Task<ulong> IDataSizeAsync(IntPtr app, ulong seH) {
      var (ret, userData) = BindingUtils.PrepareTask<ulong>();
      IDataSizeNative(app, seH, userData, OnFfiResultULongCb);
      return ret;
    }

    [DllImport(DllName, EntryPoint = "idata_size")]
    internal static extern void IDataSizeNative(IntPtr app, ulong seH, IntPtr userData, FfiResultULongCb oCb);

    public Task<byte[]> IDataReadFromSelfEncryptorAsync(IntPtr app, ulong seH, ulong fromPos, ulong len) {
      var (ret, userData) = BindingUtils.PrepareTask<byte[]>();
      IDataReadFromSelfEncryptorNative(app, seH, fromPos, len, userData, OnFfiResultByteListCb);
      return ret;
    }

    [DllImport(DllName, EntryPoint = "idata_read_from_self_encryptor")]
    internal static extern void IDataReadFromSelfEncryptorNative(
      IntPtr app,
      ulong seH,
      ulong fromPos,
      ulong len,
      IntPtr userData,
      FfiResultByteListCb oCb);

    public Task IDataSelfEncryptorWriterFreeAsync(IntPtr app, ulong handle) {
      var (ret, userData) = BindingUtils.PrepareTask();
      IDataSelfEncryptorWriterFreeNative(app, handle, userData, OnFfiResultCb);
      return ret;
    }

    [DllImport(DllName, EntryPoint = "idata_self_encryptor_writer_free")]
    internal static extern void IDataSelfEncryptorWriterFreeNative(IntPtr app, ulong handle, IntPtr userData, FfiResultCb oCb);

    public Task IDataSelfEncryptorReaderFreeAsync(IntPtr app, ulong handle) {
      var (ret, userData) = BindingUtils.PrepareTask();
      IDataSelfEncryptorReaderFreeNative(app, handle, userData, OnFfiResultCb);
      return ret;
    }

    [DllImport(DllName, EntryPoint = "idata_self_encryptor_reader_free")]
    internal static extern void IDataSelfEncryptorReaderFreeNative(IntPtr app, ulong handle, IntPtr userData, FfiResultCb oCb);

    public Task<(uint, string)> EncodeAuthReqAsync(ref AuthReq req) {
      var reqNative = req.ToNative();
      var (ret, userData) = BindingUtils.PrepareTask<(uint, string)>();
      EncodeAuthReqNative(ref reqNative, userData, OnFfiResultUIntStringCb);
      reqNative.Free();
      return ret;
    }

    [DllImport(DllName, EntryPoint = "encode_auth_req")]
    internal static extern void EncodeAuthReqNative(ref AuthReqNative req, IntPtr userData, FfiResultUIntStringCb oCb);

    public Task<(uint, string)> EncodeContainersReqAsync(ref ContainersReq req) {
      var reqNative = req.ToNative();
      var (ret, userData) = BindingUtils.PrepareTask<(uint, string)>();
      EncodeContainersReqNative(ref reqNative, userData, OnFfiResultUIntStringCb);
      reqNative.Free();
      return ret;
    }

    [DllImport(DllName, EntryPoint = "encode_containers_req")]
    internal static extern void EncodeContainersReqNative(ref ContainersReqNative req, IntPtr userData, FfiResultUIntStringCb oCb);

    public Task<(uint, string)> EncodeUnregisteredReqAsync(byte[] extraData) {
      var (ret, userData) = BindingUtils.PrepareTask<(uint, string)>();
      EncodeUnregisteredReqNative(extraData, (IntPtr)extraData.Length, userData, OnFfiResultUIntStringCb);
      return ret;
    }

    [DllImport(DllName, EntryPoint = "encode_unregistered_req")]
    internal static extern void EncodeUnregisteredReqNative(
      [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1)] byte[] extraData,
      IntPtr extraDataLen,
      IntPtr userData,
      FfiResultUIntStringCb oCb);

    public Task<(uint, string)> EncodeShareMDataReqAsync(ref ShareMDataReq req) {
      var reqNative = req.ToNative();
      var (ret, userData) = BindingUtils.PrepareTask<(uint, string)>();
      EncodeShareMDataReqNative(ref reqNative, userData, OnFfiResultUIntStringCb);
      reqNative.Free();
      return ret;
    }

    [DllImport(DllName, EntryPoint = "encode_share_mdata_req")]
    internal static extern void EncodeShareMDataReqNative(ref ShareMDataReqNative req, IntPtr userData, FfiResultUIntStringCb oCb);

    [DllImport(DllName, EntryPoint = "decode_ipc_msg")]
    internal static extern void DecodeIpcMsgNative(
      [MarshalAs(UnmanagedType.LPStr)] string msg,
      IntPtr userData,
      UIntAuthGrantedCb oAuth,
      UIntByteListCb oUnregistered,
      UIntCb oContainers,
      UIntCb oShareMData,
      NoneCb oRevoked,
      FfiResultUIntCb oErr);

    public Task AppInitLoggingAsync(string outputFileNameOverride) {
      var (ret, userData) = BindingUtils.PrepareTask();
      AppInitLoggingNative(outputFileNameOverride, userData, OnFfiResultCb);
      return ret;
    }

    [DllImport(DllName, EntryPoint = "app_init_logging")]
    internal static extern void AppInitLoggingNative(
      [MarshalAs(UnmanagedType.LPStr)] string outputFileNameOverride,
      IntPtr userData,
      FfiResultCb oCb);

    public Task<string> AppOutputLogPathAsync(string outputFileName) {
      var (ret, userData) = BindingUtils.PrepareTask<string>();
      AppOutputLogPathNative(outputFileName, userData, OnFfiResultStringCb);
      return ret;
    }

    [DllImport(DllName, EntryPoint = "app_output_log_path")]
    internal static extern void AppOutputLogPathNative(
      [MarshalAs(UnmanagedType.LPStr)] string outputFileName,
      IntPtr userData,
      FfiResultStringCb oCb);

    public Task<MDataInfo> MDataInfoNewPrivateAsync(IntPtr name, ulong typeTag, IntPtr secretKey, IntPtr nonce) {
      var (ret, userData) = BindingUtils.PrepareTask<MDataInfo>();
      MDataInfoNewPrivateNative(name, typeTag, secretKey, nonce, userData, OnFfiResultMDataInfoCb);
      return ret;
    }

    [DllImport(DllName, EntryPoint = "mdata_info_new_private")]
    internal static extern void MDataInfoNewPrivateNative(
      IntPtr name,
      ulong typeTag,
      IntPtr secretKey,
      IntPtr nonce,
      IntPtr userData,
      FfiResultMDataInfoCb oCb);

    public Task<MDataInfo> MDataInfoRandomPublicAsync(ulong typeTag) {
      var (ret, userData) = BindingUtils.PrepareTask<MDataInfo>();
      MDataInfoRandomPublicNative(typeTag, userData, OnFfiResultMDataInfoCb);
      return ret;
    }

    [DllImport(DllName, EntryPoint = "mdata_info_random_public")]
    internal static extern void MDataInfoRandomPublicNative(ulong typeTag, IntPtr userData, FfiResultMDataInfoCb oCb);

    public Task<MDataInfo> MDataInfoRandomPrivateAsync(ulong typeTag) {
      var (ret, userData) = BindingUtils.PrepareTask<MDataInfo>();
      MDataInfoRandomPrivateNative(typeTag, userData, OnFfiResultMDataInfoCb);
      return ret;
    }

    [DllImport(DllName, EntryPoint = "mdata_info_random_private")]
    internal static extern void MDataInfoRandomPrivateNative(ulong typeTag, IntPtr userData, FfiResultMDataInfoCb oCb);

    public Task<byte[]> MDataInfoEncryptEntryKeyAsync(ref MDataInfo info, byte[] input) {
      var (ret, userData) = BindingUtils.PrepareTask<byte[]>();
      MDataInfoEncryptEntryKeyNative(ref info, input, (IntPtr)input.Length, userData, OnFfiResultByteListCb);
      return ret;
    }

    [DllImport(DllName, EntryPoint = "mdata_info_encrypt_entry_key")]
    internal static extern void MDataInfoEncryptEntryKeyNative(
      ref MDataInfo info,
      [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 2)] byte[] input,
      IntPtr inputLen,
      IntPtr userData,
      FfiResultByteListCb oCb);

    public Task<byte[]> MDataInfoEncryptEntryValueAsync(ref MDataInfo info, byte[] input) {
      var (ret, userData) = BindingUtils.PrepareTask<byte[]>();
      MDataInfoEncryptEntryValueNative(ref info, input, (IntPtr)input.Length, userData, OnFfiResultByteListCb);
      return ret;
    }

    [DllImport(DllName, EntryPoint = "mdata_info_encrypt_entry_value")]
    internal static extern void MDataInfoEncryptEntryValueNative(
      ref MDataInfo info,
      [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 2)] byte[] input,
      IntPtr inputLen,
      IntPtr userData,
      FfiResultByteListCb oCb);

    public Task<byte[]> MDataInfoDecryptAsync(ref MDataInfo info, byte[] input) {
      var (ret, userData) = BindingUtils.PrepareTask<byte[]>();
      MDataInfoDecryptNative(ref info, input, (IntPtr)input.Length, userData, OnFfiResultByteListCb);
      return ret;
    }

    [DllImport(DllName, EntryPoint = "mdata_info_decrypt")]
    internal static extern void MDataInfoDecryptNative(
      ref MDataInfo info,
      [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 2)] byte[] input,
      IntPtr inputLen,
      IntPtr userData,
      FfiResultByteListCb oCb);

    public Task<byte[]> MDataInfoSerialiseAsync(ref MDataInfo info) {
      var (ret, userData) = BindingUtils.PrepareTask<byte[]>();
      MDataInfoSerialiseNative(ref info, userData, OnFfiResultByteListCb);
      return ret;
    }

    [DllImport(DllName, EntryPoint = "mdata_info_serialise")]
    internal static extern void MDataInfoSerialiseNative(ref MDataInfo info, IntPtr userData, FfiResultByteListCb oCb);

    public Task<MDataInfo> MDataInfoDeserialiseAsync(IntPtr ptr, IntPtr len) {
      var (ret, userData) = BindingUtils.PrepareTask<MDataInfo>();
      MDataInfoDeserialiseNative(ptr, len, userData, OnFfiResultMDataInfoCb);
      return ret;
    }

    [DllImport(DllName, EntryPoint = "mdata_info_deserialise")]
    internal static extern void MDataInfoDeserialiseNative(IntPtr ptr, IntPtr len, IntPtr userData, FfiResultMDataInfoCb oCb);

    public Task MDataPutAsync(IntPtr app, ref MDataInfo info, ulong permissionsH, ulong entriesH) {
      var (ret, userData) = BindingUtils.PrepareTask();
      MDataPutNative(app, ref info, permissionsH, entriesH, userData, OnFfiResultCb);
      return ret;
    }

    [DllImport(DllName, EntryPoint = "mdata_put")]
    internal static extern void MDataPutNative(
      IntPtr app,
      ref MDataInfo info,
      ulong permissionsH,
      ulong entriesH,
      IntPtr userData,
      FfiResultCb oCb);

    public Task<ulong> MDataGetVersionAsync(IntPtr app, ref MDataInfo info) {
      var (ret, userData) = BindingUtils.PrepareTask<ulong>();
      MDataGetVersionNative(app, ref info, userData, OnFfiResultULongCb);
      return ret;
    }

    [DllImport(DllName, EntryPoint = "mdata_get_version")]
    internal static extern void MDataGetVersionNative(IntPtr app, ref MDataInfo info, IntPtr userData, FfiResultULongCb oCb);

    public Task<ulong> MDataSerialisedSizeAsync(IntPtr app, ref MDataInfo info) {
      var (ret, userData) = BindingUtils.PrepareTask<ulong>();
      MDataSerialisedSizeNative(app, ref info, userData, OnFfiResultULongCb);
      return ret;
    }

    [DllImport(DllName, EntryPoint = "mdata_serialised_size")]
    internal static extern void MDataSerialisedSizeNative(IntPtr app, ref MDataInfo info, IntPtr userData, FfiResultULongCb oCb);

    public Task<(byte[], ulong)> MDataGetValueAsync(IntPtr app, ref MDataInfo info, byte[] key) {
      var (ret, userData) = BindingUtils.PrepareTask<(byte[], ulong)>();
      MDataGetValueNative(app, ref info, key, (IntPtr)key.Length, userData, OnFfiResultByteListULongCb);
      return ret;
    }

    [DllImport(DllName, EntryPoint = "mdata_get_value")]
    internal static extern void MDataGetValueNative(
      IntPtr app,
      ref MDataInfo info,
      [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 3)] byte[] key,
      IntPtr keyLen,
      IntPtr userData,
      FfiResultByteListULongCb oCb);

    public Task<ulong> MDataListEntriesAsync(IntPtr app, ref MDataInfo info) {
      var (ret, userData) = BindingUtils.PrepareTask<ulong>();
      MDataListEntriesNative(app, ref info, userData, OnFfiResultULongCb);
      return ret;
    }

    [DllImport(DllName, EntryPoint = "mdata_list_entries")]
    internal static extern void MDataListEntriesNative(IntPtr app, ref MDataInfo info, IntPtr userData, FfiResultULongCb oCb);

    public Task<(MDataKey, IntPtr)> MDataListKeysAsync(IntPtr app, ref MDataInfo info) {
      var (ret, userData) = BindingUtils.PrepareTask<(MDataKey, IntPtr)>();
      MDataListKeysNative(app, ref info, userData, OnFfiResultMDataKeyULongCb);
      return ret;
    }

    [DllImport(DllName, EntryPoint = "mdata_list_keys")]
    internal static extern void MDataListKeysNative(IntPtr app, ref MDataInfo info, IntPtr userData, FfiResultMDataKeyULongCb oCb);

    public Task<(MDataValue, IntPtr)> MDataListValuesAsync(IntPtr app, ref MDataInfo info) {
      var (ret, userData) = BindingUtils.PrepareTask<(MDataValue, IntPtr)>();
      MDataListValuesNative(app, ref info, userData, OnFfiResultMDataValueULongCb);
      return ret;
    }

    [DllImport(DllName, EntryPoint = "mdata_list_values")]
    internal static extern void MDataListValuesNative(IntPtr app, ref MDataInfo info, IntPtr userData, FfiResultMDataValueULongCb oCb);

    public Task MDataMutateEntriesAsync(IntPtr app, ref MDataInfo info, ulong actionsH) {
      var (ret, userData) = BindingUtils.PrepareTask();
      MDataMutateEntriesNative(app, ref info, actionsH, userData, OnFfiResultCb);
      return ret;
    }

    [DllImport(DllName, EntryPoint = "mdata_mutate_entries")]
    internal static extern void MDataMutateEntriesNative(IntPtr app, ref MDataInfo info, ulong actionsH, IntPtr userData, FfiResultCb oCb);

    public Task<ulong> MDataListPermissionsAsync(IntPtr app, ref MDataInfo info) {
      var (ret, userData) = BindingUtils.PrepareTask<ulong>();
      MDataListPermissionsNative(app, ref info, userData, OnFfiResultULongCb);
      return ret;
    }

    [DllImport(DllName, EntryPoint = "mdata_list_permissions")]
    internal static extern void MDataListPermissionsNative(IntPtr app, ref MDataInfo info, IntPtr userData, FfiResultULongCb oCb);

    public Task<PermissionSet> MDataListUserPermissionsAsync(IntPtr app, ref MDataInfo info, ulong userH) {
      var (ret, userData) = BindingUtils.PrepareTask<PermissionSet>();
      MDataListUserPermissionsNative(app, ref info, userH, userData, OnFfiResultPermissionSetCb);
      return ret;
    }

    [DllImport(DllName, EntryPoint = "mdata_list_user_permissions")]
    internal static extern void MDataListUserPermissionsNative(
      IntPtr app,
      ref MDataInfo info,
      ulong userH,
      IntPtr userData,
      FfiResultPermissionSetCb oCb);

    public Task MDataSetUserPermissionsAsync(IntPtr app, ref MDataInfo info, ulong userH, ref PermissionSet permissionSet, ulong version) {
      var (ret, userData) = BindingUtils.PrepareTask();
      MDataSetUserPermissionsNative(app, ref info, userH, ref permissionSet, version, userData, OnFfiResultCb);
      return ret;
    }

    [DllImport(DllName, EntryPoint = "mdata_set_user_permissions")]
    internal static extern void MDataSetUserPermissionsNative(
      IntPtr app,
      ref MDataInfo info,
      ulong userH,
      ref PermissionSet permissionSet,
      ulong version,
      IntPtr userData,
      FfiResultCb oCb);

    public Task MDataDelUserPermissionsAsync(IntPtr app, ref MDataInfo info, ulong userH, ulong version) {
      var (ret, userData) = BindingUtils.PrepareTask();
      MDataDelUserPermissionsNative(app, ref info, userH, version, userData, OnFfiResultCb);
      return ret;
    }

    [DllImport(DllName, EntryPoint = "mdata_del_user_permissions")]
    internal static extern void MDataDelUserPermissionsNative(
      IntPtr app,
      ref MDataInfo info,
      ulong userH,
      ulong version,
      IntPtr userData,
      FfiResultCb oCb);

    public Task<ulong> MDataEntriesNewAsync(IntPtr app) {
      var (ret, userData) = BindingUtils.PrepareTask<ulong>();
      MDataEntriesNewNative(app, userData, OnFfiResultULongCb);
      return ret;
    }

    [DllImport(DllName, EntryPoint = "mdata_entries_new")]
    internal static extern void MDataEntriesNewNative(IntPtr app, IntPtr userData, FfiResultULongCb oCb);

    public Task MDataEntriesInsertAsync(IntPtr app, ulong entriesH, byte[] key, byte[] value) {
      var (ret, userData) = BindingUtils.PrepareTask();
      MDataEntriesInsertNative(app, entriesH, key, (IntPtr)key.Length, value, (IntPtr)value.Length, userData, OnFfiResultCb);
      return ret;
    }

    [DllImport(DllName, EntryPoint = "mdata_entries_insert")]
    internal static extern void MDataEntriesInsertNative(
      IntPtr app,
      ulong entriesH,
      [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 3)] byte[] key,
      IntPtr keyLen,
      [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 4)] byte[] value,
      IntPtr valueLen,
      IntPtr userData,
      FfiResultCb oCb);

    public Task<IntPtr> MDataEntriesLenAsync(IntPtr app, ulong entriesH) {
      var (ret, userData) = BindingUtils.PrepareTask<IntPtr>();
      MDataEntriesLenNative(app, entriesH, userData, OnFfiResultULongCb);
      return ret;
    }

    [DllImport(DllName, EntryPoint = "mdata_entries_len")]
    internal static extern void MDataEntriesLenNative(IntPtr app, ulong entriesH, IntPtr userData, FfiResultULongCb oCb);

    public Task<(byte[], ulong)> MDataEntriesGetAsync(IntPtr app, ulong entriesH, byte[] key) {
      var (ret, userData) = BindingUtils.PrepareTask<(byte[], ulong)>();
      MDataEntriesGetNative(app, entriesH, key, (IntPtr)key.Length, userData, OnFfiResultByteListULongCb);
      return ret;
    }

    [DllImport(DllName, EntryPoint = "mdata_entries_get")]
    internal static extern void MDataEntriesGetNative(
      IntPtr app,
      ulong entriesH,
      [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 3)] byte[] key,
      IntPtr keyLen,
      IntPtr userData,
      FfiResultByteListULongCb oCb);

    [DllImport(DllName, EntryPoint = "mdata_entries_for_each")]
    internal static extern void MDataEntriesForEachNative(
      IntPtr app,
      ulong entriesH,
      IntPtr userData,
      ByteListByteListULongCb oEachCb,
      FfiResultCb oDoneCb);

    public Task MDataEntriesFreeAsync(IntPtr app, ulong entriesH) {
      var (ret, userData) = BindingUtils.PrepareTask();
      MDataEntriesFreeNative(app, entriesH, userData, OnFfiResultCb);
      return ret;
    }

    [DllImport(DllName, EntryPoint = "mdata_entries_free")]
    internal static extern void MDataEntriesFreeNative(IntPtr app, ulong entriesH, IntPtr userData, FfiResultCb oCb);

    public Task<ulong> MDataEntryActionsNewAsync(IntPtr app) {
      var (ret, userData) = BindingUtils.PrepareTask<ulong>();
      MDataEntryActionsNewNative(app, userData, OnFfiResultULongCb);
      return ret;
    }

    [DllImport(DllName, EntryPoint = "mdata_entry_actions_new")]
    internal static extern void MDataEntryActionsNewNative(IntPtr app, IntPtr userData, FfiResultULongCb oCb);

    public Task MDataEntryActionsInsertAsync(IntPtr app, ulong actionsH, byte[] key, byte[] value) {
      var (ret, userData) = BindingUtils.PrepareTask();
      MDataEntryActionsInsertNative(app, actionsH, key, (IntPtr)key.Length, value, (IntPtr)value.Length, userData, OnFfiResultCb);
      return ret;
    }

    [DllImport(DllName, EntryPoint = "mdata_entry_actions_insert")]
    internal static extern void MDataEntryActionsInsertNative(
      IntPtr app,
      ulong actionsH,
      [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 3)] byte[] key,
      IntPtr keyLen,
      [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 4)] byte[] value,
      IntPtr valueLen,
      IntPtr userData,
      FfiResultCb oCb);

    public Task MDataEntryActionsUpdateAsync(IntPtr app, ulong actionsH, byte[] key, byte[] value, ulong entryVersion) {
      var (ret, userData) = BindingUtils.PrepareTask();
      MDataEntryActionsUpdateNative(
        app,
        actionsH,
        key,
        (IntPtr)key.Length,
        value,
        (IntPtr)value.Length,
        entryVersion,
        userData,
        OnFfiResultCb);
      return ret;
    }

    [DllImport(DllName, EntryPoint = "mdata_entry_actions_update")]
    internal static extern void MDataEntryActionsUpdateNative(
      IntPtr app,
      ulong actionsH,
      [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 3)] byte[] key,
      IntPtr keyLen,
      [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 4)] byte[] value,
      IntPtr valueLen,
      ulong entryVersion,
      IntPtr userData,
      FfiResultCb oCb);

    public Task MDataEntryActionsDeleteAsync(IntPtr app, ulong actionsH, byte[] key, ulong entryVersion) {
      var (ret, userData) = BindingUtils.PrepareTask();
      MDataEntryActionsDeleteNative(app, actionsH, key, (IntPtr)key.Length, entryVersion, userData, OnFfiResultCb);
      return ret;
    }

    [DllImport(DllName, EntryPoint = "mdata_entry_actions_delete")]
    internal static extern void MDataEntryActionsDeleteNative(
      IntPtr app,
      ulong actionsH,
      [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 3)] byte[] key,
      IntPtr keyLen,
      ulong entryVersion,
      IntPtr userData,
      FfiResultCb oCb);

    public Task MDataEntryActionsFreeAsync(IntPtr app, ulong actionsH) {
      var (ret, userData) = BindingUtils.PrepareTask();
      MDataEntryActionsFreeNative(app, actionsH, userData, OnFfiResultCb);
      return ret;
    }

    [DllImport(DllName, EntryPoint = "mdata_entry_actions_free")]
    internal static extern void MDataEntryActionsFreeNative(IntPtr app, ulong actionsH, IntPtr userData, FfiResultCb oCb);

    public Task<byte[]> MDataEncodeMetadataAsync(ref MetadataResponse metadata) {
      var (ret, userData) = BindingUtils.PrepareTask<byte[]>();
      MDataEncodeMetadataNative(ref metadata, userData, OnFfiResultByteListCb);
      return ret;
    }

    [DllImport(DllName, EntryPoint = "mdata_encode_metadata")]
    internal static extern void MDataEncodeMetadataNative(ref MetadataResponse metadata, IntPtr userData, FfiResultByteListCb oCb);

    public Task<ulong> MDataPermissionsNewAsync(IntPtr app) {
      var (ret, userData) = BindingUtils.PrepareTask<ulong>();
      MDataPermissionsNewNative(app, userData, OnFfiResultULongCb);
      return ret;
    }

    [DllImport(DllName, EntryPoint = "mdata_permissions_new")]
    internal static extern void MDataPermissionsNewNative(IntPtr app, IntPtr userData, FfiResultULongCb oCb);

    public Task<IntPtr> MDataPermissionsLenAsync(IntPtr app, ulong permissionsH) {
      var (ret, userData) = BindingUtils.PrepareTask<IntPtr>();
      MDataPermissionsLenNative(app, permissionsH, userData, OnFfiResultULongCb);
      return ret;
    }

    [DllImport(DllName, EntryPoint = "mdata_permissions_len")]
    internal static extern void MDataPermissionsLenNative(IntPtr app, ulong permissionsH, IntPtr userData, FfiResultULongCb oCb);

    public Task<PermissionSet> MDataPermissionsGetAsync(IntPtr app, ulong permissionsH, ulong userH) {
      var (ret, userData) = BindingUtils.PrepareTask<PermissionSet>();
      MDataPermissionsGetNative(app, permissionsH, userH, userData, OnFfiResultPermissionSetCb);
      return ret;
    }

    [DllImport(DllName, EntryPoint = "mdata_permissions_get")]
    internal static extern void MDataPermissionsGetNative(
      IntPtr app,
      ulong permissionsH,
      ulong userH,
      IntPtr userData,
      FfiResultPermissionSetCb oCb);

    public Task<(UserPermissionSet, IntPtr)> MDataListPermissionSetsAsync(IntPtr app, ulong permissionsH) {
      var (ret, userData) = BindingUtils.PrepareTask<(UserPermissionSet, IntPtr)>();
      MDataListPermissionSetsNative(app, permissionsH, userData, OnFfiResultUserPermissionSetULongCb);
      return ret;
    }

    [DllImport(DllName, EntryPoint = "mdata_list_permission_sets")]
    internal static extern void MDataListPermissionSetsNative(
      IntPtr app,
      ulong permissionsH,
      IntPtr userData,
      FfiResultUserPermissionSetULongCb oCb);

    public Task MDataPermissionsInsertAsync(IntPtr app, ulong permissionsH, ulong userH, ref PermissionSet permissionSet) {
      var (ret, userData) = BindingUtils.PrepareTask();
      MDataPermissionsInsertNative(app, permissionsH, userH, ref permissionSet, userData, OnFfiResultCb);
      return ret;
    }

    [DllImport(DllName, EntryPoint = "mdata_permissions_insert")]
    internal static extern void MDataPermissionsInsertNative(
      IntPtr app,
      ulong permissionsH,
      ulong userH,
      ref PermissionSet permissionSet,
      IntPtr userData,
      FfiResultCb oCb);

    public Task MDataPermissionsFreeAsync(IntPtr app, ulong permissionsH) {
      var (ret, userData) = BindingUtils.PrepareTask();
      MDataPermissionsFreeNative(app, permissionsH, userData, OnFfiResultCb);
      return ret;
    }

    [DllImport(DllName, EntryPoint = "mdata_permissions_free")]
    internal static extern void MDataPermissionsFreeNative(IntPtr app, ulong permissionsH, IntPtr userData, FfiResultCb oCb);

    public Task<(File, ulong)> DirFetchFileAsync(IntPtr app, ref MDataInfo parentInfo, string fileName) {
      var (ret, userData) = BindingUtils.PrepareTask<(File, ulong)>();
      DirFetchFileNative(app, ref parentInfo, fileName, userData, OnFfiResultFileULongCb);
      return ret;
    }

    [DllImport(DllName, EntryPoint = "dir_fetch_file")]
    internal static extern void DirFetchFileNative(
      IntPtr app,
      ref MDataInfo parentInfo,
      [MarshalAs(UnmanagedType.LPStr)] string fileName,
      IntPtr userData,
      FfiResultFileULongCb oCb);

    public Task DirInsertFileAsync(IntPtr app, ref MDataInfo parentInfo, string fileName, ref File file) {
      var fileNative = file.ToNative();
      var (ret, userData) = BindingUtils.PrepareTask();
      DirInsertFileNative(app, ref parentInfo, fileName, ref fileNative, userData, OnFfiResultCb);
      fileNative.Free();
      return ret;
    }

    [DllImport(DllName, EntryPoint = "dir_insert_file")]
    internal static extern void DirInsertFileNative(
      IntPtr app,
      ref MDataInfo parentInfo,
      [MarshalAs(UnmanagedType.LPStr)] string fileName,
      ref FileNative file,
      IntPtr userData,
      FfiResultCb oCb);

    public Task DirUpdateFileAsync(IntPtr app, ref MDataInfo parentInfo, string fileName, ref File file, ulong version) {
      var fileNative = file.ToNative();
      var (ret, userData) = BindingUtils.PrepareTask();
      DirUpdateFileNative(app, ref parentInfo, fileName, ref fileNative, version, userData, OnFfiResultCb);
      fileNative.Free();
      return ret;
    }

    [DllImport(DllName, EntryPoint = "dir_update_file")]
    internal static extern void DirUpdateFileNative(
      IntPtr app,
      ref MDataInfo parentInfo,
      [MarshalAs(UnmanagedType.LPStr)] string fileName,
      ref FileNative file,
      ulong version,
      IntPtr userData,
      FfiResultCb oCb);

    public Task DirDeleteFileAsync(IntPtr app, ref MDataInfo parentInfo, string fileName, ulong version) {
      var (ret, userData) = BindingUtils.PrepareTask();
      DirDeleteFileNative(app, ref parentInfo, fileName, version, userData, OnFfiResultCb);
      return ret;
    }

    [DllImport(DllName, EntryPoint = "dir_delete_file")]
    internal static extern void DirDeleteFileNative(
      IntPtr app,
      ref MDataInfo parentInfo,
      [MarshalAs(UnmanagedType.LPStr)] string fileName,
      ulong version,
      IntPtr userData,
      FfiResultCb oCb);

    public Task<ulong> FileOpenAsync(IntPtr app, ref MDataInfo parentInfo, ref File file, ulong openMode) {
      var fileNative = file.ToNative();
      var (ret, userData) = BindingUtils.PrepareTask<ulong>();
      FileOpenNative(app, ref parentInfo, ref fileNative, openMode, userData, OnFfiResultULongCb);
      fileNative.Free();
      return ret;
    }

    [DllImport(DllName, EntryPoint = "file_open")]
    internal static extern void FileOpenNative(
      IntPtr app,
      ref MDataInfo parentInfo,
      ref FileNative file,
      ulong openMode,
      IntPtr userData,
      FfiResultULongCb oCb);

    public Task<ulong> FileSizeAsync(IntPtr app, ulong fileH) {
      var (ret, userData) = BindingUtils.PrepareTask<ulong>();
      FileSizeNative(app, fileH, userData, OnFfiResultULongCb);
      return ret;
    }

    [DllImport(DllName, EntryPoint = "file_size")]
    internal static extern void FileSizeNative(IntPtr app, ulong fileH, IntPtr userData, FfiResultULongCb oCb);

    public Task<byte[]> FileReadAsync(IntPtr app, ulong fileH, ulong position, ulong len) {
      var (ret, userData) = BindingUtils.PrepareTask<byte[]>();
      FileReadNative(app, fileH, position, len, userData, OnFfiResultByteListCb);
      return ret;
    }

    [DllImport(DllName, EntryPoint = "file_read")]
    internal static extern void FileReadNative(
      IntPtr app,
      ulong fileH,
      ulong position,
      ulong len,
      IntPtr userData,
      FfiResultByteListCb oCb);

    public Task FileWriteAsync(IntPtr app, ulong fileH, byte[] data) {
      var (ret, userData) = BindingUtils.PrepareTask();
      FileWriteNative(app, fileH, data, (IntPtr)data.Length, userData, OnFfiResultCb);
      return ret;
    }

    [DllImport(DllName, EntryPoint = "file_write")]
    internal static extern void FileWriteNative(
      IntPtr app,
      ulong fileH,
      [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 3)] byte[] data,
      IntPtr dataLen,
      IntPtr userData,
      FfiResultCb oCb);

    public Task<File> FileCloseAsync(IntPtr app, ulong fileH) {
      var (ret, userData) = BindingUtils.PrepareTask<File>();
      FileCloseNative(app, fileH, userData, OnFfiResultFileCb);
      return ret;
    }

    [DllImport(DllName, EntryPoint = "file_close")]
    internal static extern void FileCloseNative(IntPtr app, ulong fileH, IntPtr userData, FfiResultFileCb oCb);

    #region Callbacks

    internal delegate void ByteListByteListULongCb(
      IntPtr userData,
      IntPtr keyPtr,
      IntPtr keyLen,
      IntPtr valuePtr,
      IntPtr valueLen,
      ulong entryVersion);

    internal delegate void FfiResultAccountInfoCb(IntPtr userData, ref FfiResult result, ref AccountInfo accountInfo);

#if __IOS__
        [MonoPInvokeCallback(typeof(FfiResultAccountInfoCb))]
#endif
    private static void OnFfiResultAccountInfoCb(IntPtr userData, ref FfiResult result, ref AccountInfo accountInfo) {
      BindingUtils.CompleteTask(userData, ref result, accountInfo);
    }

    internal delegate void FfiResultAppCb(IntPtr userData, ref FfiResult result, IntPtr app);

    internal delegate void FfiResultByteArrayAsymNonceLenCb(IntPtr userData, ref FfiResult result, IntPtr nonce);

#if __IOS__
        [MonoPInvokeCallback(typeof(FfiResultByteArrayAsymNonceLenCb))]
#endif
    private static void OnFfiResultByteArrayAsymNonceLenCb(IntPtr userData, ref FfiResult result, IntPtr nonce) {
      BindingUtils.CompleteTask(userData, ref result, nonce);
    }

    internal delegate void FfiResultByteArrayAsymPublicKeyLenCb(IntPtr userData, ref FfiResult result, IntPtr pubEncKey);

#if __IOS__
        [MonoPInvokeCallback(typeof(FfiResultByteArrayAsymPublicKeyLenCb))]
#endif
    private static void OnFfiResultByteArrayAsymPublicKeyLenCb(IntPtr userData, ref FfiResult result, IntPtr pubEncKey) {
      BindingUtils.CompleteTask(userData, ref result, pubEncKey);
    }

    internal delegate void FfiResultByteArrayAsymSecretKeyLenCb(IntPtr userData, ref FfiResult result, IntPtr secEncKey);

#if __IOS__
        [MonoPInvokeCallback(typeof(FfiResultByteArrayAsymSecretKeyLenCb))]
#endif
    private static void OnFfiResultByteArrayAsymSecretKeyLenCb(IntPtr userData, ref FfiResult result, IntPtr secEncKey) {
      BindingUtils.CompleteTask(userData, ref result, secEncKey);
    }

    internal delegate void FfiResultByteArraySignPublicKeyLenCb(IntPtr userData, ref FfiResult result, IntPtr pubSignKey);

#if __IOS__
        [MonoPInvokeCallback(typeof(FfiResultByteArraySignPublicKeyLenCb))]
#endif
    private static void OnFfiResultByteArraySignPublicKeyLenCb(IntPtr userData, ref FfiResult result, IntPtr pubSignKey) {
      BindingUtils.CompleteTask(userData, ref result, pubSignKey);
    }

    internal delegate void FfiResultByteArraySignSecretKeyLenCb(IntPtr userData, ref FfiResult result, IntPtr pubSignKey);

#if __IOS__
        [MonoPInvokeCallback(typeof(FfiResultByteArraySignSecretKeyLenCb))]
#endif
    private static void OnFfiResultByteArraySignSecretKeyLenCb(IntPtr userData, ref FfiResult result, IntPtr pubSignKey) {
      BindingUtils.CompleteTask(userData, ref result, pubSignKey);
    }

    internal delegate void FfiResultByteArrayXorNameLenCb(IntPtr userData, ref FfiResult result, IntPtr name);

#if __IOS__
        [MonoPInvokeCallback(typeof(FfiResultByteArrayXorNameLenCb))]
#endif
    private static void OnFfiResultByteArrayXorNameLenCb(IntPtr userData, ref FfiResult result, IntPtr name) {
      BindingUtils.CompleteTask(userData, ref result, name);
    }

    internal delegate void FfiResultByteListCb(IntPtr userData, ref FfiResult result, IntPtr signedDataPtr, IntPtr signedDataLen);

#if __IOS__
        [MonoPInvokeCallback(typeof(FfiResultByteListCb))]
#endif
    private static void OnFfiResultByteListCb(IntPtr userData, ref FfiResult result, IntPtr signedDataPtr, IntPtr signedDataLen) {
      BindingUtils.CompleteTask(userData, ref result, BindingUtils.CopyToByteArray(signedDataPtr, signedDataLen));
    }

    internal delegate void FfiResultByteListULongCb(
      IntPtr userData,
      ref FfiResult result,
      IntPtr contentPtr,
      IntPtr contentLen,
      ulong version);

#if __IOS__
        [MonoPInvokeCallback(typeof(FfiResultByteListULongCb))]
#endif
    private static void OnFfiResultByteListULongCb(
      IntPtr userData,
      ref FfiResult result,
      IntPtr contentPtr,
      IntPtr contentLen,
      ulong version) {
      BindingUtils.CompleteTask(userData, ref result, (BindingUtils.CopyToByteArray(contentPtr, contentLen), version));
    }

    internal delegate void FfiResultCb(IntPtr userData, ref FfiResult result);

#if __IOS__
        [MonoPInvokeCallback(typeof(FfiResultCb))]
#endif
    private static void OnFfiResultCb(IntPtr userData, ref FfiResult result) {
      BindingUtils.CompleteTask(userData, ref result);
    }

    internal delegate void FfiResultContainerPermissionsListCb(
      IntPtr userData,
      ref FfiResult result,
      IntPtr containerPermsPtr,
      IntPtr containerPermsLen);

#if __IOS__
        [MonoPInvokeCallback(typeof(FfiResultContainerPermissionsListCb))]
#endif
    private static void OnFfiResultContainerPermissionsListCb(
      IntPtr userData,
      ref FfiResult result,
      IntPtr containerPermsPtr,
      IntPtr containerPermsLen) {
      BindingUtils.CompleteTask(
        userData,
        ref result,
        BindingUtils.CopyToObjectArray<ContainerPermissions>(containerPermsPtr, containerPermsLen));
    }

    internal delegate void FfiResultFileCb(IntPtr userData, ref FfiResult result, ref FileNative file);

#if __IOS__
        [MonoPInvokeCallback(typeof(FfiResultFileCb))]
#endif
    private static void OnFfiResultFileCb(IntPtr userData, ref FfiResult result, ref FileNative file) {
      BindingUtils.CompleteTask(userData, ref result, new File(file));
    }

    internal delegate void FfiResultFileULongCb(IntPtr userData, ref FfiResult result, ref FileNative file, ulong version);

#if __IOS__
        [MonoPInvokeCallback(typeof(FfiResultFileULongCb))]
#endif
    private static void OnFfiResultFileULongCb(IntPtr userData, ref FfiResult result, ref FileNative file, ulong version) {
      BindingUtils.CompleteTask(userData, ref result, (new File(file), version));
    }

    internal delegate void FfiResultMDataInfoCb(IntPtr userData, ref FfiResult result, ref MDataInfo mdataInfo);

#if __IOS__
        [MonoPInvokeCallback(typeof(FfiResultMDataInfoCb))]
#endif
    private static void OnFfiResultMDataInfoCb(IntPtr userData, ref FfiResult result, ref MDataInfo mdataInfo) {
      BindingUtils.CompleteTask(userData, ref result, mdataInfo);
    }

    internal delegate void FfiResultMDataKeyULongCb(IntPtr userData, ref FfiResult result, ref MDataKeyNative keys, IntPtr len);

#if __IOS__
        [MonoPInvokeCallback(typeof(FfiResultMDataKeyULongCb))]
#endif
    private static void OnFfiResultMDataKeyULongCb(IntPtr userData, ref FfiResult result, ref MDataKeyNative keys, IntPtr len) {
      BindingUtils.CompleteTask(userData, ref result, (new MDataKey(keys), len));
    }

    internal delegate void FfiResultMDataValueULongCb(IntPtr userData, ref FfiResult result, ref MDataValueNative values, IntPtr len);

#if __IOS__
        [MonoPInvokeCallback(typeof(FfiResultMDataValueULongCb))]
#endif
    private static void OnFfiResultMDataValueULongCb(IntPtr userData, ref FfiResult result, ref MDataValueNative values, IntPtr len) {
      BindingUtils.CompleteTask(userData, ref result, (new MDataValue(values), len));
    }

    internal delegate void FfiResultPermissionSetCb(IntPtr userData, ref FfiResult result, ref PermissionSet permSet);

#if __IOS__
        [MonoPInvokeCallback(typeof(FfiResultPermissionSetCb))]
#endif
    private static void OnFfiResultPermissionSetCb(IntPtr userData, ref FfiResult result, ref PermissionSet permSet) {
      BindingUtils.CompleteTask(userData, ref result, permSet);
    }

    internal delegate void FfiResultStringCb(IntPtr userData, ref FfiResult result, string filename);

#if __IOS__
        [MonoPInvokeCallback(typeof(FfiResultStringCb))]
#endif
    private static void OnFfiResultStringCb(IntPtr userData, ref FfiResult result, string filename) {
      BindingUtils.CompleteTask(userData, ref result, filename);
    }

    internal delegate void FfiResultUIntCb(IntPtr userData, ref FfiResult result, uint reqId);

    internal delegate void FfiResultUIntStringCb(IntPtr userData, ref FfiResult result, uint reqId, string encodedPtr);

#if __IOS__
        [MonoPInvokeCallback(typeof(FfiResultUIntStringCb))]
#endif
    private static void OnFfiResultUIntStringCb(IntPtr userData, ref FfiResult result, uint reqId, string encodedPtr) {
      BindingUtils.CompleteTask(userData, ref result, (reqId, encodedPtr));
    }

    internal delegate void FfiResultULongCb(IntPtr userData, ref FfiResult result, ulong handle);

#if __IOS__
        [MonoPInvokeCallback(typeof(FfiResultULongCb))]
#endif
    private static void OnFfiResultULongCb(IntPtr userData, ref FfiResult result, ulong handle) {
      BindingUtils.CompleteTask(userData, ref result, handle);
    }

    internal delegate void FfiResultULongULongCb(IntPtr userData, ref FfiResult result, ulong pkH, ulong skH);

#if __IOS__
        [MonoPInvokeCallback(typeof(FfiResultULongULongCb))]
#endif
    private static void OnFfiResultULongULongCb(IntPtr userData, ref FfiResult result, ulong pkH, ulong skH) {
      BindingUtils.CompleteTask(userData, ref result, (pkH, skH));
    }

    internal delegate void FfiResultUserPermissionSetULongCb(
      IntPtr userData,
      ref FfiResult result,
      ref UserPermissionSet userPermSets,
      IntPtr len);

#if __IOS__
        [MonoPInvokeCallback(typeof(FfiResultUserPermissionSetULongCb))]
#endif
    private static void OnFfiResultUserPermissionSetULongCb(
      IntPtr userData,
      ref FfiResult result,
      ref UserPermissionSet userPermSets,
      IntPtr len) {
      BindingUtils.CompleteTask(userData, ref result, (userPermSets, len));
    }

    internal delegate void NoneCb(IntPtr userData);

    internal delegate void UIntAuthGrantedCb(IntPtr userData, uint reqId, ref AuthGrantedNative authGranted);

    internal delegate void UIntByteListCb(IntPtr userData, uint reqId, IntPtr serialisedCfgPtr, IntPtr serialisedCfgLen);

    internal delegate void UIntCb(IntPtr userData, uint reqId);

    #endregion
  }
}
#endif
