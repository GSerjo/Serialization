using System;
using ProtoBuf;

namespace Terminal.Protobuf
{
    [ProtoContract(ImplicitFields = ImplicitFields.AllPublic)]
    public sealed class Request1
    {
        public Guid Id { get; set; }
        public byte[] RawData { get; set; }
    }
}