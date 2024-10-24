using ConMaster.Deepslate.Network;
using ConMaster.Deepslate.Protocol;
using ConMaster.Deepslate.Protocol.Enums;
using ConMaster.Deepslate.Protocol.Packets;
using System.IdentityModel.Tokens.Jwt;

namespace ConMaster
{
    public class Program
    {
        public const string BIOME_DEFINITIONS = @"C:\Users\samde\Documents\GitHub\serenity\packages\data\data\biome_definition_list.nbt";
        public const string FILE_NAME = @"C:\Users\samde\Documents\GitHub\serenity\packages\data\data\canonical_block_states.nbt";
        public static readonly JwtSecurityTokenHandler JwtParser = new() { MaximumTokenSizeInBytes = int.MaxValue };
        public static void Main(string[] args)
        {
            Console.WriteLine(Convert.ToHexString("Just test tho"u8));
            Console.WriteLine(BitConverter.ToString("Just test tho"u8.ToArray()));




            DeepslateProtocol proto = new(729);
            Server server = new(new Raknet.Server(), proto);
            proto.OnWarn += Proto_OnWarn;
            proto.OnError += Proto_OnError;
            proto.PlayerLogin.OnRecieved += async (proto, client, packet) =>
            {
                client.SendPacket([
                    PlayStatusPacket.FromStatus(PlayStatus.FailedVanillaEditorMismatch),
                ]);
            };
            server.Start();
            Thread.Sleep(100000);

            /*
            //Real and validate token data





            Span<byte> data = File.ReadAllBytes("C:\\Users\\samde\\Documents\\vs-coding-snippets\\identityData.txt");
            ParseTokens(loginData);
            

            long time = Stopwatch.GetTimestamp();
            for (int i = 0; i < 1_000; i++) JsonSerializer.Deserialize<ExpandoObject>(data);
            //Console.WriteLine((Stopwatch.GetTimestamp() - time) / 10_000d + "ms");


            time = Stopwatch.GetTimestamp();
            for (int i = 0; i < 1_000; i++)
            {
                ReadOnlySpan<byte> lastToken = default;
                Utf8JsonReader reader = new(data);
                while (reader.Read())
                {
                    Console.WriteLine(reader.TokenType);
                    if (reader.TokenType == JsonTokenType.String)
                    {
                        //lastToken = reader.ValueSpan;
                    }
                }
                Encoding.UTF8.GetString(lastToken);
            }
            Console.WriteLine("New: " + (Stopwatch.GetTimestamp() - time) / 10_000d + "ms");

            time = Stopwatch.GetTimestamp();
            for (int i = 0; i < 1_000; i++)
            {
                ClientIdentityDataJsonChain test = JsonSerializer.Deserialize<ClientIdentityDataJsonChain>(data)!;
                string lmao = test.Chain?[test.Chain.Length - 1]??string.Empty;
            }
            Console.WriteLine("Old: " + (Stopwatch.GetTimestamp() - time) / 10_000d + "ms");


            
            /*
            Span<byte> clientData = File.ReadAllBytes("C:\\Users\\samde\\Documents\\vs-coding-snippets\\clientData.txt");

            string base64SubjectPublicKeyInfo = "MHYwEAYHKoZIzj0CAQYFK4EEACIDYgAEIqt3KPH//1jbXq8Tjpj85NSrMs4LV2N+B93poUnuHejQWhAJS9dmmQV9vO2VL7+tp3NzS8N64JVmPdJOux5yOAwqX7q+/ZVw+zIz/KXtWJmcbeQDchZS5bDgx14LIChs";

            ECDsa yes2 = ECDsa.Create();
            yes2.ImportSubjectPublicKeyInfo(Convert.FromBase64String(base64SubjectPublicKeyInfo), out _);

            int dotIndex = clientData.LastIndexOf((byte)'.');
            Console.WriteLine($"Dot Index: {dotIndex}");

            Span<byte> data = clientData.Slice(dotIndex + 1);
            Span<byte> buffer = stackalloc byte[data.Length];
            data.CopyTo(buffer);


            buffer.Replace((byte)'/', (byte)'-');
            buffer.Replace((byte)'+', (byte)'-');

            bool decodeSuccess = Base64.DecodeFromUtf8InPlace(buffer, out int writen) == System.Buffers.OperationStatus.Done;
            Console.WriteLine($"Decode Success: {decodeSuccess}, Bytes Written: {writen}");

            bool verifySuccess = yes2.VerifyData(
                clientData.Slice(0, dotIndex),
                buffer.Slice(0, writen),
                HashAlgorithmName.SHA384
            );
            Console.WriteLine($"Signature Verification Result: {verifySuccess}");

            /*

            ParseTokens(loginData);

            byte[] array = File.ReadAllBytes("C:\\Users\\samde\\Documents\\vs-coding-snippets\\clientData.txt");

            Span<byte> buffer = array;
            Span<byte> buffer1 = new byte[buffer.Length];
            buffer.CopyTo(buffer1);
            ReadOnlySpan<char> buffer2 = loginData.ClientData;

            Console.WriteLine("{0} - {1}", buffer.Length, buffer2.Length);

            long time = Stopwatch.GetTimestamp();
            for (int i = 0; i < 1_000; i++) buffer.IndexOf((byte)'.');
            //Console.WriteLine((Stopwatch.GetTimestamp() - time) / 10_000d + "ms");

            time = Stopwatch.GetTimestamp();
            for (int i = 0; i < 1_000; i++)
            {
                buffer.CopyTo(buffer1);
                Span<byte> data = GetJwtPayloadInPlace(buffer1);
                ClientDataTest test = JsonSerializer.Deserialize<ClientDataTest>(data);
            }
            Console.WriteLine("New: " + (Stopwatch.GetTimestamp() - time) / 10_000d + "ms");

            time = Stopwatch.GetTimestamp();
            for (int i = 0; i < 1_000; i++)
            {
                buffer.CopyTo(buffer1);
                var payload = JwtParser.ReadJwtToken(loginData.ClientData).Payload;
                ClientLoginPayload.FromJwtPayload(payload);
                //buffer2.Split(ranges, '.');
                //ReadOnlySpan<char> data = buffer2.Slice(ranges[1].Start.Value, ranges[1].End.Value - ranges[1].Start.Value);
            }
            Console.WriteLine("Old: " + (Stopwatch.GetTimestamp() - time) / 10_000d + "ms");


            /*
            byte[] bytes = new byte[Marshal.SizeOf<SomeData>()];

            SomeData data = new()
            {
                Age = 18,
                Data = 654,
                FloatDataLike = 54.46540,
                MoreAge = 6540,
                SoperData = 466,
            };

            MemoryMarshal.Write(bytes, data);


            File.WriteAllBytes("Test.bin", bytes);


            var bob = MemoryMarshal.Read<SomeData>(File.ReadAllBytes("Test.bin"));
            Console.WriteLine(bob);


            /*
            Proxy p = new(IPAddress.Loopback, IPAddress.Any, 19132, 3000);
            await p.Start();
            /*
            EndPoint endpoit = null!;
            UDPSocket listener = new UDPSocket();
            listener.Server("127.0.0.1", 3000);

            UDPSocket bds = new UDPSocket();
            bds.Client("127.0.0.1", 19132);

            bds.OnReceive += (c, e) =>
            {
                Console.WriteLine("From BDS: " + ((IPEndPoint)e).Port);
                listener.SendTo(c, e);
            };
            listener.OnReceive += (s, e) =>
            {
                Console.WriteLine("Listened From: " + ((IPEndPoint)e).Port);
                endpoit = e;
                bds.Send(s);
            };

            byte[] buffer = new byte[4096];
            UnconnectedPing ping = new()
            {
                Guid = (uint)Random.Shared.NextInt64(),
                Time = DateTime.Now.Ticks,
            };

            bds.Send(buffer.AsMemory(0, ping.Serialize(buffer).Length));
            Thread.Sleep(50_000);
            */
            //UdpProxy.MainProxyTest(args);

            /*
            Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            
            EndPoint endPoint = new IPEndPoint(IPAddress.Any, 0);

            byte[] buffer = new byte[4096];

            UnconnectedPing ping = new()
            {
                Guid = (uint)Random.Shared.NextInt64(),
                Time = DateTime.Now.Ticks,
            };

            int size = ping.Serialize(buffer).Length;

            Console.WriteLine(BitConverter.ToString(ping.Serialize(buffer).ToArray()));

            Console.WriteLine(size);

            socket.Bind(new IPEndPoint(IPAddress.Any, 3000));

            // Send ping to server
            socket.SendTo(buffer, size, SocketFlags.None, new IPEndPoint(IPAddress.Loopback, 19132));

            while (true)
            {
                try
                {
                    int data = socket.ReceiveFrom(buffer, ref endPoint);
                    Console.WriteLine(data);
                }
                catch (Exception)
                {
                    Console.WriteLine("Error");
                }
            }
            //Expect response

            /*
            Proxy p = new(IPAddress.Parse("127.0.0.1"), IPAddress.Parse("127.0.0.1"), 19132, 3000);

            await p.Start();
            /*
            var pong = new UnconnectedPong()
            {
                Time = 0,
                Guid = 0,
                MOTD = "MCPE;Dedicated Server;390;1.14.60;3;10;13253860892328930865;Bedrock level;Survival;1;19132;19133;"
            };
            Span<byte> data = pong.Serialize(stackalloc byte[255]);
            Console.WriteLine(BitConverter.ToString(data.ToArray()));
            /*
            IServerProvider raknet = new Raknet.Server();
            raknet.OnError += (sender, args) =>
            {
                Console.WriteLine(args.GetException() + ": " + args.GetException()?.StackTrace);
            };
            raknet.OnClientConnected += (sender, args) =>
            {
                Console.WriteLine("RAKNET: new client: " + args.Address.Address );
            };
            raknet.OnClientDisconnected += (sender, args) =>
            {
                Console.WriteLine("RAKNET: disconnected client: " + args.Address.Address);
            };
            Server server = new(raknet, new(1205));
            server.Protocol.OnWarn += (sender, message) =>
            {
                Console.WriteLine(message);
            };
            server.Start();
            string command = Console.ReadLine() ?? "stop";
            while (!command.Equals("stop", StringComparison.OrdinalIgnoreCase))
            {
                Console.WriteLine("unknow command: " + command);
                command = Console.ReadLine() ?? "stop";
            }
            server.Stop();
            Console.WriteLine("Server is shutdown...");
            /*
#if DEBUG
            Log.Default.IsDebugEnabled = true;
#endif
            BedrockRunner runner = new();
            runner.Run().Wait();
            */




            /*
            BlockType type = new(
                "minecraft:spruce_log", 
                [
                    new StringBlockState((Utf8String)"pillar_axis"u8, ["y"u8,"x"u8,"z"u8])    
                ]
            );
            BlockPermutation permutation = new(type, [1]);
            Console.WriteLine("Hash: " + permutation.Hash);
            return;
            */


            /*
            RentedBuffer rentedBuffer = RentedBuffer.Alloc(256);
            int offset = 0;
            ConstantMemoryBufferWriter w = new(rentedBuffer.Span, ref offset);
            ConstantNBTWriter writer = new(w, NBTMode.Bedrock);
            writer.WriteCompoudEntry(TagType.Compoud, ""u8);
            Dictionary<string, StateValue> states = new()
            {
                { "redstone_signal", new IntStateValue(1) }
            };

            BPermutation p = new("minecraft:heavy_weighted_pressure_plate", states);
            Stopwatch sw = Stopwatch.StartNew();
            writer.Write(p);
            ReadOnlySpan<byte> buf = w.GetWritenBytes();
            Console.WriteLine(sw.ElapsedMilliseconds);
            Console.WriteLine(GetHashFor(buf));
            rentedBuffer.Dispose();*/
            /*
            long loop = 100_000_000;
            int o = 0;
            uint length = 31233;
            long ms = Stopwatch.GetTimestamp();

            for (long i = 0; i < loop; i++)
            {
                o = (int)Math.Ceiling(Math.Log2(length));
            }

            Console.WriteLine(Stopwatch.GetTimestamp() - ms);
            Console.WriteLine("O: " + o);
            ms = Stopwatch.GetTimestamp();

            for (long i = 0; i < loop; i++)
            {
                if (length == 0) continue;
                o = 1;
                uint l = length - 1;
                while ((l >>= 1) != 0) o++;
            }
            Console.WriteLine(Stopwatch.GetTimestamp() - ms);
            Console.WriteLine("O: " + o);

            ms = Stopwatch.GetTimestamp();

            for (long i = 0; i < loop; i++)
            {
                o = 32 - BitOperations.LeadingZeroCount(length - 1u);
            }
            Console.WriteLine(Stopwatch.GetTimestamp() - ms);
            Console.WriteLine("O: " + o);

            return;


            //nint s = Interlocked.Increment(ref ms);

            //ref byte test = ref MemoryMarshal.GetReference(test1);



            //ref byte t1 = ref MemoryMarshal.GetReference(test1);
            //ref byte t2 = ref MemoryMarshal.GetReference("Test"u8);
            /*for (long i = 0; i < loop; i++)
            {
                string str = Encoding.UTF8.GetString("test"u8);
                byte[] buff = ArrayPool<byte>.Shared.Rent(str.Length + 100);
                int a = Encoding.UTF8.GetBytes(str, buff);
                buff.AsSpan().CopyTo(copyTo);
                bool t = str == "Test";
                ArrayPool<byte>.Shared.Return(buff);
            }
            Console.WriteLine(Stopwatch.GetTimestamp() - ms);
            ms = Stopwatch.GetTimestamp();
            for (long i = 0; i < loop; i++)
            {
                Utf8String str = (Utf8String)"test"u8;
                str.Span.CopyTo(copyTo);
                bool t = str == "Test"u8;
            }
            Console.WriteLine(Stopwatch.GetTimestamp() - ms);
            ms = Stopwatch.GetTimestamp();

            Utf8String ts = Utf8String.From(""u8);

            return;*/
            /*
            byte[] biomes = File.ReadAllBytes(BIOME_DEFINITIONS);
            BinaryStream stream = new(new MemoryStream(biomes));
            while (!stream.IsEndOfStream)
            {
                Console.WriteLine(NBTDefiniton.NetworkNBT.ReadRootTag(stream, out _));
            }*/
            //int num = 0x0002_0000;
            //float s = num;
            /*
            long loop = 1_000_000;
            ReadOnlySpan<byte> test1 = "Test"u8;
            //Span<byte> test = *(Span<byte>*)&test1;
            Span<byte> a = 0;
            string test2 = "Test";
            test2.Equals("Test");
            long ms = Stopwatch.GetTimestamp();

            //nint s = Interlocked.Increment(ref ms);

            //ref byte test = ref MemoryMarshal.GetReference(test1);



            //ref byte t1 = ref MemoryMarshal.GetReference(test1);
            //ref byte t2 = ref MemoryMarshal.GetReference("Test"u8);
            for (long i = 0; i < loop; i++)
            {
                bool t = (test2.Equals("Test"));  //(SequenceEqual(ref t1, ref t2, 4));
            }
            Console.WriteLine(Stopwatch.GetTimestamp() - ms);
            ms = Stopwatch.GetTimestamp();
            for (long i = 0; i < loop; i++)
            {
                bool t = test1.SequenceEqual("Test"u8);
            }
            Console.WriteLine(Stopwatch.GetTimestamp() - ms);
            ms = Stopwatch.GetTimestamp();
            */

            //Console.WriteLine("\x1b[0mTest: 000");
            //Console.WriteLine("\u001b[38;2;255;180;60mTest: 80");
            /*

            Console.WriteLine((GC.GetGCMemoryInfo().HeapSizeBytes >> 20) + " Mb");

            byte[] data = File.ReadAllBytes(FILE_NAME);
            int offset = 0;
            ConstantMemoryBufferReader reader = new(data, ref offset);
            ConstantNBTReader nbtReader = new(reader, NBTMode.Network);
            BinaryStream stream = new(new MemoryStream(data));

            long ms = Stopwatch.GetTimestamp();
            while (!reader.IsEndOfStream)
            {
                TagType type = nbtReader.ReadType();
                nbtReader.Mode.SkipString(reader);
                nbtReader.SkipTag(type);
                //nbtReader.ReadCompoudEntry(out TagType type, out ReadOnlySpan<byte> key);
                //Console.WriteLine(type);
                //nbtReader.SkipTag(type);
            }
            Console.WriteLine("Time: " + (Stopwatch.GetTimestamp() - ms) / TimeSpan.TicksPerMillisecond);

            offset = 0;
            
            ms = Stopwatch.GetTimestamp();
            while (!reader.IsEndOfStream)
            {
                NBTDefiniton.NetworkNBT.ReadRootTag(reader, out string name);
                //TagType type = nbtReader.ReadType();
                //nbtReader.Mode.SkipString(reader);
                //nbtReader.SkipTag(type);
                //nbtReader.ReadCompoudEntry(out TagType type, out ReadOnlySpan<byte> key);
                //Console.WriteLine(type);
                //nbtReader.SkipTag(type);
            }
            Console.WriteLine("Time: " + (Stopwatch.GetTimestamp() - ms) / TimeSpan.TicksPerMillisecond);
            ms = Stopwatch.GetTimestamp();
            while (!stream.IsEndOfStream)
            {
                NBTDefiniton.NetworkNBT.ReadRootTag(stream, out string name);
                //TagType type = nbtReader.ReadType();
                //nbtReader.Mode.SkipString(reader);
                //nbtReader.SkipTag(type);
                //nbtReader.ReadCompoudEntry(out TagType type, out ReadOnlySpan<byte> key);
                //Console.WriteLine(type);
                //nbtReader.SkipTag(type);
            }
            Console.WriteLine("Time: " + (Stopwatch.GetTimestamp() - ms) / TimeSpan.TicksPerMillisecond);

            Console.WriteLine((GC.GetGCMemoryInfo().HeapSizeBytes >> 20) + " Mb");
            /*
            int permutations = 0;
            while (!reader.IsEndOfStream)
            {
                NBTDefiniton.VariableNBT.ReadRootTag(reader, out string rootName);
                permutations++;
            }
            Console.WriteLine(permutations);
            /*
            Console.WriteLine(reader.ReadType());
            Console.WriteLine(reader.ReadRawString().AsString());
            foreach (var item in reader.ReadCompoudAsEnumerable()) {
                Console.WriteLine(item.Type);
                Console.WriteLine(item.Key.AsString());
                break;
            }



            /*
            Server server = new();
            server.OnClientConnected += connection => Console.WriteLine("Connected: " + connection.Address);
            server.OnClientDisconnected += connection => Console.WriteLine("Disconnected: " + connection.Address);
            server.OnGamePacket += (connection, packetData) => Console.WriteLine("Game Packet: " + BitConverter.ToString(packetData.ToArray()));
            _ = server.Start();
            server.SetMotd(new ServerMotdInfo
            {
                CurrentPlayerCount = 1,
                MaxPlayerCount = 10,
                Name = "My Server",
                GameVersion = "2.20.0",
                IsEducationEdition = false,
                LevelName = "Level Name 2",
                ProtocolVersion = 390
            });
            Thread.Sleep(50_000);
            server.Stop().Wait();
            /*
            //Test t = new Test();
            CustomRandom r1 = new CustomRandom(1);
            Random r2 = new Random(0);
            long LOOP = 10_000_000_000;
            //int PARALEL_COUNT = 100;
            //long PARALEL_LOOP = LOOP / PARALEL_COUNT;
            for (long i = 0; i < LOOP; i++)
            {
                i+=10;
            }
            Console.WriteLine("START");
            long ms = Stopwatch.GetTimestamp();
            //int random = Random.Shared.Next(5);
            for (long i = 0; i < LOOP; i++)
            {
                if(r1.NextDouble() < 0.000000001) Console.WriteLine("ZERO");
            }
            Console.WriteLine("Time: " + (Stopwatch.GetTimestamp() - ms) / TimeSpan.TicksPerMillisecond);
            ms = Stopwatch.GetTimestamp();
            for (long i = 0; i < LOOP; i++)
            {
                if(r2.NextDouble() < 0.000000001) Console.WriteLine("ZERO");
            }
            Console.WriteLine("Time: " + (Stopwatch.GetTimestamp() - ms) / TimeSpan.TicksPerMillisecond);
            */
        }

        private static void Proto_OnError(object sender, ErrorEventArgs e)
        {
            Console.WriteLine(e.GetException());
        }

        private static void Proto_OnWarn(object arg1, string arg2)
        {
            Console.WriteLine(arg2);
        }
    }
}