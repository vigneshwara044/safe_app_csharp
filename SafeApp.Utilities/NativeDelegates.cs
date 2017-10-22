using System;

namespace SafeApp.Utilities {
  public delegate void InitLoggingCb(IntPtr self, FfiResult result);

  public delegate void AppExeFileStemCb(IntPtr self, FfiResult result, string exeFileStem);

  public delegate void AppSetAdditionalSearchPathCb(IntPtr self, FfiResult result);

  public delegate void AppOutputLogPathCallback(IntPtr self, FfiResult result, string path);

  public delegate void MDataInfoDecryptCb(IntPtr self, FfiResult result, IntPtr plainText, IntPtr len);

  public delegate void NetworkObserverCb(IntPtr self, int errorCode, int eventType);

  public delegate void EncodeAuthReqCb(IntPtr self, FfiResult result, uint requestId, string encodedReq);

  public delegate void AccessContainerGetContainerMDataInfoCb(IntPtr self, FfiResult result, ulong mDataInfoHandle);

  public delegate void AppPubSignKeyCb(IntPtr self, FfiResult result, ulong signKeyHandle);

  public delegate void CipherOptFreeCb(IntPtr self, FfiResult result);

  public delegate void CipherOptNewPlaintextCb(IntPtr self, FfiResult result, ulong cipherOptHandle);

  public delegate void DecryptSealedBoxCb(IntPtr self, FfiResult result, IntPtr data, IntPtr dataLen);

  public delegate void EncGenerateKeyPairCb(IntPtr self, FfiResult result, ulong encPubKeyHandle, ulong encSecKeyHandle);

  public delegate void EncPubKeyFreeCb(IntPtr self, FfiResult result);

  public delegate void EncPubKeyGetCb(IntPtr self, FfiResult result, IntPtr asymPublicKey);

  public delegate void EncPubKeyNewCb(IntPtr self, FfiResult result, ulong encryptPubKeyHandle);

  public delegate void EncryptSealedBoxCb(IntPtr self, FfiResult result, IntPtr data, IntPtr dataLen);

  public delegate void EncSecretKeyFreeCb(IntPtr self, FfiResult result);

  public delegate void EncSecretKeyGetCb(IntPtr self, FfiResult result, IntPtr asymSecretKey);

  public delegate void EncSecretKeyNewCb(IntPtr self, FfiResult result, ulong encryptSecKeyHandle);

  // ReSharper disable InconsistentNaming - IData
  public delegate void IDataCloseSelfEncryptorCb(IntPtr self, FfiResult result, IntPtr xorNameArr);

  public delegate void IDataFetchSelfEncryptorCb(IntPtr self, FfiResult result, ulong sEReaderHandle);

  public delegate void IDataNewSelfEncryptorCb(IntPtr self, FfiResult result, ulong sEWriterHandle);

  public delegate void IDataReadFromSelfEncryptorCb(IntPtr self, FfiResult result, IntPtr data, IntPtr dataLen);

  public delegate void IDataSelfEncryptorReaderFreeCb(IntPtr self, FfiResult result);

  public delegate void IDataSelfEncryptorWriterFreeCb(IntPtr self, FfiResult result);

  public delegate void IDataSizeCb(IntPtr self, FfiResult result, ulong len);

  public delegate void IDataWriteToSelfEncryptorCb(IntPtr self, FfiResult result);
  // ReSharper restore InconsistentNaming

  public delegate void MDataEntriesForEachCb(
    IntPtr self,
    IntPtr entryKey,
    IntPtr entryKeyLen,
    IntPtr entryVal,
    IntPtr entryValLen,
    ulong entryVersion);

  public delegate void MDataEntriesForEachResCb(IntPtr self, FfiResult result);

  public delegate void MDataEntriesFreeCb(IntPtr self, FfiResult result);

  public delegate void MDataEntriesInsertCb(IntPtr self, FfiResult result);

  public delegate void MDataEntriesLenCb(IntPtr self, ulong len);

  public delegate void MDataEntriesNewCb(IntPtr self, FfiResult result, ulong mDataEntriesHandle);

  public delegate void MDataEntryActionsFreeCb(IntPtr self, FfiResult result);

  public delegate void MDataEntryActionsInsertCb(IntPtr self, FfiResult result);

  public delegate void MDataEntryActionsNewCb(IntPtr self, FfiResult result, ulong mDataEntryActionsHandle);

  public delegate void MDataGetValueCb(IntPtr self, FfiResult result, IntPtr data, IntPtr dataLen, ulong entryVersion);

  public delegate void MDataInfoDeserialiseCb(IntPtr self, FfiResult result, ulong mDataInfoHandle);

  public delegate void MDataInfoFreeCb(IntPtr self, FfiResult result);

  public delegate void MDataInfoNewPublicCb(IntPtr self, FfiResult result, ulong mDataInfoHandle);

  public delegate void MDataInfoRandomPrivateCb(IntPtr self, FfiResult result, ulong mDataInfoHandle);

  public delegate void MDataInfoRandomPublicCb(IntPtr self, FfiResult result, ulong mDataInfoHandle);

  public delegate void MDataInfoSerialiseCb(IntPtr self, FfiResult result, IntPtr data, IntPtr dataLen);

  public delegate void MDataListEntriesCb(IntPtr self, FfiResult result, ulong mDataEntriesHandle);

  public delegate void MDataMutateEntriesCb(IntPtr self, FfiResult result);

  public delegate void MDataPermissionSetNewCb(IntPtr self, FfiResult result, ulong mDataPermissionSetHandle);

  public delegate void MDataPermissionsFreeCb(IntPtr self, FfiResult result);

  public delegate void MDataPermissionsInsertCb(IntPtr self, FfiResult result);

  public delegate void MDataPermissionsNewCb(IntPtr self, FfiResult result, ulong mDataPermissionsHandle);

  public delegate void MDataPermissionSetAllowCb(IntPtr self, FfiResult result);

  public delegate void MDataPermissionSetFreeCb(IntPtr self, FfiResult result);

  public delegate void MDataPutCb(IntPtr self, FfiResult result);

  public delegate void Sha3HashCb(IntPtr self, FfiResult result, IntPtr digest, IntPtr len);

  public delegate void MDataListKeysCb(IntPtr self, FfiResult result, ulong keysHandle);

  public delegate void MDataKeysLenCb(IntPtr self, FfiResult result, IntPtr keysLen);

  public delegate void MDataKeysForEachCb(IntPtr self, IntPtr bytePtr, IntPtr byteLen);

  public delegate void MDataKeysForEachResCb(IntPtr self, FfiResult result);

  public delegate void MDataKeysFreeCb(IntPtr self, FfiResult result);

  public delegate void DecodeAuthCb(IntPtr self, uint reqId, IntPtr authGrantedFfiPtr);

  public delegate void DecodeUnregCb(IntPtr self, uint reqId, IntPtr bsConfig, IntPtr bsSize);

  public delegate void DecodeContCb(IntPtr self, uint reqId);

  public delegate void DecodeShareMDataCb(IntPtr self, uint reqId);

  public delegate void DecodeRevokedCb(IntPtr self);

  public delegate void DecodeErrorCb(IntPtr self, FfiResult result);

  public delegate void NetObsCb(IntPtr self, FfiResult result, int eventType);

  public delegate void AppRegisteredCb(IntPtr self, FfiResult result, IntPtr appPtr);

  public delegate void MDataInfoEncryptEntryKeyCb(IntPtr self, FfiResult result, IntPtr dataPtr, IntPtr dataLen);

  public delegate void MDataInfoEncryptEntryValueCb(IntPtr self, FfiResult result, IntPtr dataPtr, IntPtr dataLen);

  public delegate void SignKeyFreeCb(IntPtr self, FfiResult result);
}
