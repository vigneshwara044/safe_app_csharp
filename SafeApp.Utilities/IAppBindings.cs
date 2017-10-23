using System;

namespace SafeApp.Utilities {
  public interface IAppBindings {
    void AccessContainerGetContainerMDataInfo(IntPtr appPtr, string name, AccessContainerGetContainerMDataInfoCb callback);
    void AppExeFileStem(AppExeFileStemCb callback);
    void AppInitLogging(string fileName, InitLoggingCb callback);
    void AppOutputLogPath(string fileName, AppOutputLogPathCallback callback);
    void AppPubSignKey(IntPtr appPtr, AppPubSignKeyCb callback);

    void AppRegistered(string appId, IntPtr authGrantedFfiPtr, NetObsCb netObsCb, AppRegisteredCb appRegCb);

    void AppSetAdditionalSearchPath(string path, AppSetAdditionalSearchPathCb callback);
    void CipherOptFree(IntPtr appPtr, ulong cipherOptHandle, CipherOptFreeCb callback);
    void CipherOptNewPlaintext(IntPtr appPtr, CipherOptNewPlaintextCb callback);

    void DecodeIpcMessage(
      string encodedReq,
      DecodeAuthCb authCb,
      DecodeUnregCb unregCb,
      DecodeContCb contCb,
      DecodeShareMDataCb shareMDataCb,
      DecodeRevokedCb revokedCb,
      DecodeErrorCb errorCb);

    void DecryptSealedBox(IntPtr appPtr, IntPtr data, IntPtr len, ulong pkHandle, ulong skHandle, DecryptSealedBoxCb callback);
    void EncGenerateKeyPair(IntPtr appPtr, EncGenerateKeyPairCb callback);
    void EncodeAuthReq(IntPtr authReq, EncodeAuthReqCb callback);
    void EncPubKeyFree(IntPtr appPtr, ulong encryptPubKeyHandle, EncPubKeyFreeCb callback);
    void EncPubKeyGet(IntPtr appPtr, ulong encryptPubKeyHandle, EncPubKeyGetCb callback);
    void EncPubKeyNew(IntPtr appPtr, IntPtr asymPublicKey, EncPubKeyNewCb callback);
    void EncryptSealedBox(IntPtr appPtr, IntPtr data, IntPtr len, ulong pkHandle, EncryptSealedBoxCb callback);
    void EncSecretKeyFree(IntPtr appPtr, ulong encryptSecKeyHandle, EncSecretKeyFreeCb callback);
    void EncSecretKeyGet(IntPtr appPtr, ulong encryptSecKeyHandle, EncSecretKeyGetCb callback);
    void EncSecretKeyNew(IntPtr appPtr, IntPtr asymSecretKey, EncSecretKeyNewCb callback);
    void FreeApp(IntPtr appPtr);

    void MDataEntriesForEach(
      IntPtr appPtr,
      ulong entriesHandle,
      MDataEntriesForEachCb forEachCallback,
      MDataEntriesForEachResCb resultCallback);

    void MDataEntriesFree(IntPtr appPtr, ulong entriesHandle, MDataEntriesFreeCb callback);

    void MDataEntriesInsert(
      IntPtr appPtr,
      ulong entriesHandle,
      IntPtr keyPtr,
      IntPtr keyLen,
      IntPtr valuePtr,
      IntPtr valueLen,
      MDataEntriesInsertCb callback);

    void MDataEntriesLen(IntPtr appPtr, ulong entriesHandle, MDataEntriesLenCb callback);
    void MDataEntriesNew(IntPtr appPtr, MDataEntriesNewCb callback);
    void MDataEntryActionsFree(IntPtr appPtr, ulong actionsHandle, MDataEntryActionsFreeCb callback);

    void MDataEntryActionsInsert(
      IntPtr appPtr,
      ulong actionsHandle,
      IntPtr keyPtr,
      IntPtr keyLen,
      IntPtr valuePtr,
      IntPtr valueLen,
      MDataEntryActionsInsertCb callback);

    void MDataEntryActionsNew(IntPtr appPtr, MDataEntryActionsNewCb callback);
    void MDataGetValue(IntPtr appPtr, ulong infoHandle, IntPtr keyPtr, IntPtr keyLen, MDataGetValueCb callback);
    void MDataInfoDecrypt(IntPtr appPtr, ulong mDataInfoH, IntPtr cipherText, IntPtr cipherLen, MDataInfoDecryptCb callback);
    void MDataInfoDeserialise(IntPtr appPtr, IntPtr ptr, IntPtr len, MDataInfoDeserialiseCb callback);

    void MDataInfoEncryptEntryKey(IntPtr appPtr, ulong infoH, IntPtr inputPtr, IntPtr inputLen, MDataInfoEncryptEntryKeyCb callback);

    void MDataInfoEncryptEntryValue(IntPtr appPtr, ulong infoH, IntPtr inputPtr, IntPtr inputLen, MDataInfoEncryptEntryValueCb callback);

    void MDataInfoFree(IntPtr appPtr, ulong infoHandle, MDataInfoFreeCb callback);
    void MDataInfoNewPublic(IntPtr appPtr, IntPtr xorNameArr, ulong typeTag, MDataInfoNewPublicCb callback);
    void MDataInfoRandomPrivate(IntPtr appPtr, ulong typeTag, MDataInfoRandomPrivateCb callback);
    void MDataInfoRandomPublic(IntPtr appPtr, ulong typeTag, MDataInfoRandomPublicCb callback);
    void MDataInfoSerialise(IntPtr appPtr, ulong infoHandle, MDataInfoSerialiseCb callback);
    void MDataKeysForEach(IntPtr appPtr, ulong keysHandle, MDataKeysForEachCb forEachCb, MDataKeysForEachResCb resCb);

    void MDataKeysFree(IntPtr appPtr, ulong keysHandle, MDataKeysFreeCb callback);

    void MDataKeysLen(IntPtr appPtr, ulong keysHandle, MDataKeysLenCb callback);
    void MDataListEntries(IntPtr appPtr, ulong infoHandle, MDataListEntriesCb callback);
    void MDataListKeys(IntPtr appPtr, ulong infoHandle, MDataListKeysCb callback);
    void MDataMutateEntries(IntPtr appPtr, ulong infoHandle, ulong actionsHandle, MDataMutateEntriesCb callback);
    void MDataPermissionSetAllow(IntPtr appPtr, ulong setHandle, MDataAction action, MDataPermissionSetAllowCb callback);
    void MDataPermissionSetFree(IntPtr appPtr, ulong setHandle, MDataPermissionSetFreeCb callback);
    void MDataPermissionSetNew(IntPtr appPtr, MDataPermissionSetNewCb callback);
    void MDataPermissionsFree(IntPtr appPtr, ulong permissionsHandle, MDataPermissionsFreeCb callback);

    void MDataPermissionsInsert(
      IntPtr appPtr,
      ulong permissionsHandle,
      ulong userHandle,
      ulong permissionSetHandle,
      MDataPermissionsInsertCb callback);

    void MDataPermissionsNew(IntPtr appPtr, MDataPermissionsNewCb callback);
    void MDataPut(IntPtr appPtr, ulong infoHandle, ulong permissionsHandle, ulong entriesHandle, MDataPutCb callback);
    void Sha3Hash(IntPtr data, IntPtr len, Sha3HashCb callback);
    void SignKeyFree(IntPtr appPtr, ulong signKeyHandle, SignKeyFreeCb callback);

    // ReSharper disable InconsistentNaming
    void IDataCloseSelfEncryptor(IntPtr appPtr, ulong seH, ulong cipherOptH, IDataCloseSelfEncryptorCb callback);

    void IDataFetchSelfEncryptor(IntPtr appPtr, IntPtr xorNameArr, IDataFetchSelfEncryptorCb callback);
    void IDataNewSelfEncryptor(IntPtr appPtr, IDataNewSelfEncryptorCb callback);

    void IDataReadFromSelfEncryptor(IntPtr appPtr, ulong seHandle, ulong fromPos, ulong len, IDataReadFromSelfEncryptorCb callback);

    void IDataSelfEncryptorReaderFree(IntPtr appPtr, ulong sEReaderHandle, IDataSelfEncryptorReaderFreeCb callback);
    void IDataSelfEncryptorWriterFree(IntPtr appPtr, ulong sEWriterHandle, IDataSelfEncryptorWriterFreeCb callback);
    void IDataSize(IntPtr appPtr, ulong seHandle, IDataSizeCb callback);

    void IDataWriteToSelfEncryptor(IntPtr appPtr, ulong seHandle, IntPtr data, IntPtr size, IDataWriteToSelfEncryptorCb callback);
    // ReSharper restore InconsistentNaming
  }
}
