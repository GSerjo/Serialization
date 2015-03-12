using System;

namespace Terminal.Protobuf
{
    public sealed class Request2
    {
        public Guid Id { get; set; }
        public byte[] RawData { get; set; }
    }
}
