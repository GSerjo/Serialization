using System;

namespace Terminal.Protobuf
{
    [Serializable]
    public sealed class Request3
    {
        public Guid Id { get; set; }
        public byte[] RawData { get; set; }
    }
}
