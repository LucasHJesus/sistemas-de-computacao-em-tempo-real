using System.Collections.Concurrent;
using System.Globalization;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;

namespace faltaDeEnergia
{
    public class MedidasEletricas
    {
        public string fase { get; set; }
        public double tensão { get; set; }
        public double corrente { get; set; }
        public double? angTensão { get; set; }
        public double? potApaVA { get; set; }
        public double? potRearVAr { get; set; }
        public double? potRealW { get; set; }
        public double? fatorP { get; set; }
        public double? freq { get; set; }


    }
    public class MedidasLeitura
    {
        public string URI { get; set; }
        public int? idMU { get; set; }
        public double latitude { get; set; }
        public double longitude { get; set; }
        public string idAtivo { get; set; }
        public int? numPct { get; set; }
        public string? timeStamping { get; set; }
        public int? freEnvioMS { get; set; }
        public MedidasEletricas[] medidas { get; set; }
    }
    internal class Program
    {
        static int numThread = 5;
        static int portReceptor = 1235;
        static int portSender = 6942;

        static IPEndPoint targetEndpoint = null;
        static UdpClient UdpClientReceptor = null;
        static UdpClient UdpClientSender = null;
        static IPAddress remoteAddress = null;
        static IPEndPoint IPReceptor = null;
        static IPEndPoint remoteEndPoint = null;

        static Thread recievePackets = null;
        static Thread sendPackets = null;
        static Thread distributingPackets = null;

        static ConcurrentQueue<string> bufferReciever = null;
        static ConcurrentQueue<MedidasLeitura>[] buffersProcessing = null;

        
        private static void recieveData()
        {
            
            while (true)
            {
                byte[] receivedBytes = UdpClientReceptor.Receive(ref IPReceptor);
                string receivedText = Encoding.ASCII.GetString(receivedBytes);
                bufferReciever.Enqueue(receivedText);

            }
        }

        private static void distributingData()
        {
            int i = 0;
            string packet;
            Console.WriteLine("entrou 1");
            while(true)
            {
                if(bufferReciever.TryDequeue(out packet))
                {
                    try
                    {
                        //Console.WriteLine(packet);
                        MedidasLeitura? medidas = JsonSerializer.Deserialize<MedidasLeitura>(packet);
                        if(medidas != null && medidas.medidas[0].corrente<1.0 && medidas.medidas[1].corrente < 1.0 && medidas.medidas[2].corrente < 1.0) 
                        {
                            buffersProcessing[i % numThread].Enqueue(medidas);
                            i++;
                        }
                        

                    }
                    catch (Exception ex) 
                    {
                        Console.WriteLine(packet);
                        Console.WriteLine(ex.ToString());
                       
                    }
                   
                }
            }
            
        }

        static void sendData()
        {
            string stringToSend = "AE CARAAAAAAAI";
            string message = "Hello from C#!";
            byte[] data = Encoding.ASCII.GetBytes(message);
            while (true)
            {
                if (UdpClientReceptor != null)
                {
                    try
                    {
                        UdpClientReceptor.Send(data, data.Length, targetEndpoint);
                    }
                    catch (Exception ex) { }
                }
            }
        }

        static void Main(string[] args)
        {
            bufferReciever = new ConcurrentQueue<string>();
            buffersProcessing = new ConcurrentQueue<MedidasLeitura>[numThread];
            
            UdpClientReceptor = new UdpClient(portReceptor);
            IPReceptor = null;
            
            UdpClientSender = new UdpClient(portSender);
            targetEndpoint = new IPEndPoint(IPAddress.Broadcast, portSender);

            Console.WriteLine("waiting for data...");
            recievePackets = new Thread(recieveData);
            recievePackets.Start();

            distributingPackets = new Thread(distributingData);
            distributingPackets.Start();

            sendPackets = new Thread(sendData);
            sendPackets.Start();

        }
    }
}
