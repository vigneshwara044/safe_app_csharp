using System;
using System.Collections.Generic;
using System.Text;
using SafeApp.Utilities;

namespace SafeApp.MockAuthBindings
{
  public abstract class IpcReq { }

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

  public class UnregisteredIpcReq : IpcReq
  {
    public uint ReqId;
    public List<byte> SerialisedCfg;

    public UnregisteredIpcReq(uint reqId, IntPtr serialisedCfgPtr, ulong serialisedCfgLen)
    {
      ReqId = reqId;
      SerialisedCfg = BindingUtils.CopyToByteList(serialisedCfgPtr, (int)serialisedCfgLen);
    }
  }

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

  public class ShareMDataIpcReq : IpcReq
  {
    public uint ReqId;
    public ShareMDataReq ShareMDataReq;
    public List<string> MetadataList;

    public ShareMDataIpcReq(uint reqId, ShareMDataReq shareMDataReq, List<string> metadataList)
    {
      ReqId = reqId;
      ShareMDataReq = shareMDataReq;
      MetadataList = metadataList;
    }
  }

  public class IpcReqRejected : IpcReq {
    public readonly string Msg;

    public IpcReqRejected(string msg)
    {
      Msg = msg;
    }
  }

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

  public class RevokedIpcReq : IpcReq { }

}
