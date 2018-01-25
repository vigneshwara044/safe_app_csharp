using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using SafeApp.Utilities;

namespace SafeApp.MockAuthBindings
{
  public abstract class IpcReq { }
  [PublicAPI]
  public class AuthIpcReq : IpcReq
  {
    public AuthReq AuthReq;
    public uint ReqId;

    public AuthIpcReq(uint reqId, AuthReq authReq)
    {
      ReqId = reqId;
      AuthReq = authReq;
    }
  }
  [PublicAPI]
  public class UnregisteredIpcReq : IpcReq
  {
    public uint ReqId;
    public List<byte> ExtraData;

    public UnregisteredIpcReq(uint reqId, IntPtr extraDataPtr, ulong extraDataLength)
    {
      ReqId = reqId;
      ExtraData = BindingUtils.CopyToByteList(extraDataPtr, (int)extraDataLength);
    }
  }
  [PublicAPI]
  public class ContainersIpcReq : IpcReq
  {
    public uint ReqId;
    public ContainersReq ContainersReq;

    public ContainersIpcReq(uint reqId, ContainersReq containersReq)
    {
      ReqId = reqId;
      ContainersReq = containersReq;
    }
  }
  [PublicAPI]
  public class ShareMDataIpcReq : IpcReq
  {
    public uint ReqId;
    public ShareMDataReq ShareMDataReq;
    public MetadataResponse MetadataResponse;

    public ShareMDataIpcReq(uint reqId, ShareMDataReq shareMDataReq, MetadataResponse metadataResponse)
    {
      ReqId = reqId;
      ShareMDataReq = shareMDataReq;
      MetadataResponse = metadataResponse;
    }
  }
  [PublicAPI]
  public class IpcReqRejected : IpcReq {
    public readonly string Msg;

    public IpcReqRejected(string msg)
    {
      Msg = msg;
    }
  }
  [PublicAPI]
  public class IpcReqError : IpcReq
  {
    public readonly int Code;
    public readonly string Description;
    public readonly string Msg;

    public IpcReqError(int code, string description, string msg)
    {
      Code = code;
      Description = description;
      Msg = msg;
    }
  }
}
