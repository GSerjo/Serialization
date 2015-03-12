using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using Newtonsoft.Json;
using ProtoBuf;
using Terminal.Protobuf;

namespace Terminal
{
    internal class Program
    {
        private const int Iterations = 1000000;

        private static T DeserializeBinary<T>(byte[] value)
        {
            using (var stream = new MemoryStream(value))
            {
                var formatter = new BinaryFormatter();
                return (T)formatter.Deserialize(stream);
            }
        }

        private static T DeserializeJson<T>(byte[] value)
        {
            return JsonConvert.DeserializeObject<T>(Encoding.Default.GetString(value));
        }

        private static T DeserializeProtobuf<T>(byte[] value)
        {
            return Serializer.Deserialize<T>(new MemoryStream(value));
        }

        private static void Init()
        {
            MeasureProtobuf();
            MeasureJson();
            MeasureBinary();
        }

        private static void Main()
        {
//            Init();

            MeasureProtobuf(Iterations);
            MeasureJson(Iterations);
            MeasureBinary(Iterations);

            Console.ReadKey();
        }

        private static void MeasureBinary(int iterations = 1)
        {
            var request = new Request3
            {
                Id = Guid.NewGuid(),
                RawData = File.ReadAllBytes(@"DataStore\Cert.cer")
            };

            Stopwatch stopwatch = Stopwatch.StartNew();

            for (int i = 0; i < iterations; i++)
            {
                byte[] serialized = SerializeBinary(request);
                var target = DeserializeBinary<Request3>(serialized);
            }

            stopwatch.Stop();

            Console.WriteLine("Binary: {0}ms", stopwatch.Elapsed.TotalMilliseconds);
        }

        private static void MeasureJson(int iterations = 1)
        {
            var request = new Request2
            {
                Id = Guid.NewGuid(),
                RawData = File.ReadAllBytes(@"DataStore\Cert.cer")
            };

            Stopwatch stopwatch = Stopwatch.StartNew();

            for (int i = 0; i < iterations; i++)
            {
                byte[] serialized = SerializeJson(request);
                var target = DeserializeJson<Request2>(serialized);
            }

            stopwatch.Stop();

            Console.WriteLine("Json.Net: {0}ms", stopwatch.Elapsed.TotalMilliseconds);
        }

        private static void MeasureProtobuf(int iterations = 1)
        {
            var request = new Request1
            {
                Id = Guid.NewGuid(),
                RawData = File.ReadAllBytes(@"DataStore\Cert.cer")
            };

            Stopwatch stopwatch = Stopwatch.StartNew();

            for (int i = 0; i < iterations; i++)
            {
                byte[] serialized = SerializeProtobuf(request);
                var target = DeserializeProtobuf<Request1>(serialized);
            }

            stopwatch.Stop();

            Console.WriteLine("Protobuf: {0}ms", stopwatch.Elapsed.TotalMilliseconds);
        }

        private static byte[] SerializeBinary(object value)
        {
            using (var stream = new MemoryStream())
            {
                var formatter = new BinaryFormatter();
                formatter.Serialize(stream, value);
                return stream.ToArray();
            }
        }

        private static byte[] SerializeJson(object value)
        {
            return Encoding.Default.GetBytes(JsonConvert.SerializeObject(value));
        }

        private static byte[] SerializeProtobuf<T>(T value)
        {
            using (var stream = new MemoryStream())
            {
                Serializer.Serialize(stream, value);
                return stream.ToArray();
            }
        }
    }
}
