using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using SafeApp.Utilities;

#pragma warning disable SA1401 // Fields should be private
namespace SafeApp.MockAuthBindings
{
    /// <summary>
    /// IPC message
    /// </summary>
    public abstract class IpcReq
    {
    }

    /// <summary>
    /// Authentication IPC message
    /// </summary>
    [PublicAPI]
    public class AuthIpcReq : IpcReq
    {
        /// <summary>
        /// Authentication request.
        /// </summary>
        public AuthReq AuthReq;

        /// <summary>
        /// Request id.
        /// </summary>
        public uint ReqId;

        /// <summary>
        /// Initialize AuthIPC request.
        /// </summary>
        /// <param name="reqId">Request id.</param>
        /// <param name="authReq">Authentication request.</param>
        public AuthIpcReq(uint reqId, AuthReq authReq)
        {
            ReqId = reqId;
            AuthReq = authReq;
        }
    }

    /// <summary>
    /// Unregistered IPC request.
    /// </summary>
    [PublicAPI]
    public class UnregisteredIpcReq : IpcReq
    {
        public List<byte> ExtraData;

        /// <summary>
        /// Request id.
        /// </summary>
        public uint ReqId;

        public UnregisteredIpcReq(uint reqId, IntPtr extraDataPtr, ulong extraDataLength)
        {
            ReqId = reqId;
            ExtraData = BindingUtils.CopyToByteList(extraDataPtr, (int)extraDataLength);
        }
    }

    /// <summary>
    /// Containers IPC request.
    /// </summary>
    [PublicAPI]
    public class ContainersIpcReq : IpcReq
    {
        /// <summary>
        /// Container request.
        /// </summary>
        public ContainersReq ContainersReq;

        /// <summary>
        /// Request id.
        /// </summary>
        public uint ReqId;

        public ContainersIpcReq(uint reqId, ContainersReq containersReq)
        {
            ReqId = reqId;
            ContainersReq = containersReq;
        }
    }

    /// <summary>
    /// Share mutable data IPC request.
    /// </summary>
    [PublicAPI]
    public class ShareMDataIpcReq : IpcReq
    {
        /// <summary>
        /// Metadata response.
        /// </summary>
        public MetadataResponse MetadataResponse;

        /// <summary>
        /// Request id.
        /// </summary>
        public uint ReqId;

        /// <summary>
        /// Share mutable data request.
        /// </summary>
        public ShareMDataReq ShareMDataReq;

        public ShareMDataIpcReq(uint reqId, ShareMDataReq shareMDataReq, MetadataResponse metadataResponse)
        {
            ReqId = reqId;
            ShareMDataReq = shareMDataReq;
            MetadataResponse = metadataResponse;
        }
    }

    /// <summary>
    /// IPC request rejected.
    /// </summary>
    [PublicAPI]
    public class IpcReqRejected : IpcReq
    {
        /// <summary>
        /// Message string.
        /// </summary>
        public readonly string Msg;

        public IpcReqRejected(string msg)
        {
            Msg = msg;
        }
    }

    /// <summary>
    /// IPC Error request.
    /// </summary>
    [PublicAPI]
    public class IpcReqError : IpcReq
    {
        /// <summary>
        /// Error code.
        /// It's a negative value.
        /// </summary>
        public readonly int Code;

        /// <summary>
        /// Error description.
        /// </summary>
        public readonly string Description;

        /// <summary>
        /// Error message.
        /// </summary>
        public readonly string Msg;

        public IpcReqError(int code, string description, string msg)
        {
            Code = code;
            Description = description;
            Msg = msg;
        }
    }
}
#pragma warning restore SA1401 // Fields should be private
