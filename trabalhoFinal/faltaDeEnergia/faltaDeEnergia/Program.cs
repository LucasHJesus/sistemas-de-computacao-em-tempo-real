using System.Collections.Concurrent;
using System.Diagnostics;
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
        static int numThread = 4;
        static int portReceptor = 1235;
        static int portSender = 6942;
        static int portTarget = 3333;
        static int maxMUNumber = 300000;
        static double limite = 0.0;

        static IPEndPoint targetEndpoint = null;
        static UdpClient UdpClientReceptor = null;
        static UdpClient UdpClientSender = null;
        static IPAddress remoteAddress = null;
        static IPEndPoint IPReceptor = null;
        static IPEndPoint remoteEndPoint = null;

        static Thread recievePackets = null;
        static Thread sendPackets = null;
        static Thread distributingPackets = null;
        static Thread processData = null;
        static Thread[] partialCluster = null;

        static ConcurrentQueue<string> bufferReciever = null;
        static ConcurrentQueue<MedidasLeitura> bufferParsed = null;
        static ConcurrentQueue<Double[][]> centroidBuffer = null;
        static ConcurrentQueue<MedidasLeitura>[] bufferProcessing = null;
        static ConcurrentQueue<int> indexThreadQueue = null;
        static ConcurrentQueue<string> bufferSender = null;


 
        private static string packetFormater(MedidasLeitura[] medidas, double latitude, double longitude )
        {
            Random rnd = new Random();
            int idPKT = rnd.Next();
            string mensagem = "{\"URI\":\"100/11\" ,\"idPKT\":" + idPKT.ToString() + ",\"cords\":[{" +
                "\"Lat\": " + latitude.ToString(CultureInfo.InvariantCulture) + ",\"Long\": " + longitude.ToString(CultureInfo.InvariantCulture) + "}]";

            string idMUs = "";

            foreach(MedidasLeitura medida in medidas)
            {
                idMUs += medida.idMU.ToString() + ",";
            }



            idMUs = idMUs.Remove(idMUs.Length - 1, 1);

            mensagem += ",\"idMUs\":[" + idMUs + "]}";

            return mensagem;
            
        }
        private static void recieveData()
        {
            Console.WriteLine("Recieving Started");
            while (true)
            {
                byte[] receivedBytes = UdpClientReceptor.Receive(ref IPReceptor);
                string receivedText = Encoding.ASCII.GetString(receivedBytes);
                bufferReciever.Enqueue(receivedText);

            }
        }

        private static void parsingData()
        {
            
            string packet;
            Console.WriteLine("Parsing Started");
            while(true)
            {
                if(bufferReciever.TryDequeue(out packet))
                {
                    try
                    {
                        MedidasLeitura? medidas = JsonSerializer.Deserialize<MedidasLeitura>(packet);

                        
                        if(medidas != null && medidas.medidas[0].corrente == limite && medidas.medidas[1].corrente == limite && medidas.medidas[2].corrente == limite) 
                        {

                            bufferParsed.Enqueue(medidas);
                            
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

        private static void processingData()
        {
            Console.WriteLine("Processing Started");
            partialCluster = new Thread[numThread];
            double[][] centroids; double longitudeCentroide; double latitudeCentroide;
            int p; int numberOfClusters = 1; int indexBufferProcessing;
            bool dequeued;
            MedidasLeitura medidas;

            

            centroidBuffer = new ConcurrentQueue<double[][]>();

            while (true)
            {
                MedidasLeitura[] arrayMedidas = new MedidasLeitura[maxMUNumber];

                longitudeCentroide = 0.0;
                latitudeCentroide = 0.0;
                dequeued = false;
                p = 0;
                indexBufferProcessing = 0;

                while (p < 2)
                {
                    if (bufferParsed.TryDequeue(out medidas))
                    {
                        dequeued = true;
                        p = 0;
                        bufferProcessing[indexBufferProcessing%numThread].Enqueue(medidas);
                        arrayMedidas[indexBufferProcessing] = medidas;
                        indexBufferProcessing++;
                        
                    }
                    else
                    {
                        p++;
                        Thread.Sleep(100);
                    }
                }

                if (!dequeued || arrayMedidas.Length<5) continue;
                
                Array.Resize(ref arrayMedidas, indexBufferProcessing);


                for(int i = 0; i < numThread; i++)
                {
                    indexThreadQueue.Enqueue(i);
                }

                for (int indexThread = 0; indexThread < numThread; indexThread++)
                {
                    partialCluster[indexThread] = new Thread(() => partialClustering(numberOfClusters));
                    partialCluster[indexThread].Start();
                }

                for (int m = 0; m < numThread; m++) partialCluster[m].Join(); 

                while(centroidBuffer.TryDequeue(out centroids))
                {
                    latitudeCentroide += centroids[0][0];
                    longitudeCentroide += centroids[0][1];
                }


                latitudeCentroide /= (Double)numThread;
                longitudeCentroide /= (Double)numThread;

                string mensagem = packetFormater(arrayMedidas,latitudeCentroide,longitudeCentroide);

                bufferSender.Enqueue(mensagem) ;

            }
           
            
        }

        private static void partialClustering(int numberCLusters) 
        {
            indexThreadQueue.TryDequeue(out int indexThread);
            Console.WriteLine("IndexThread: " + indexThread);
            double[][] data = new double[bufferProcessing[indexThread].Count][];

            MedidasLeitura parcialData; int indexData = 0;

            while (bufferProcessing[indexThread].TryDequeue(out parcialData))
            {
                data[indexData] = new double[] { parcialData.latitude, parcialData.longitude };
                indexData++;
            }

            if (data == null) return;
            KMeans km = new KMeans(numberCLusters, data, "plusplus", 100, 0);
            km.Cluster(10);

             centroidBuffer.Enqueue(km.means);

            Console.WriteLine("Thread " + indexThread + " finish computing");
        }

        static void sendData()
        {
            Console.WriteLine("Send data started");
           
            while (true)
            {
                while(bufferSender.TryDequeue(out string message))
                {
                    if (UdpClientReceptor == null) continue;
                    byte[] data = Encoding.ASCII.GetBytes(message);

                    try
                    {
                        UdpClientReceptor.Send(data, data.Length, targetEndpoint);
                        Console.WriteLine("message Sent: "+ message);
                    }
                    catch (Exception ex) { }
                }
                
                
            }
        }

        static void Main(string[] args)
        { 

            bufferReciever = new ConcurrentQueue<string>();
            bufferSender = new ConcurrentQueue<string>();
            indexThreadQueue = new ConcurrentQueue<int>();
            bufferParsed = new ConcurrentQueue<MedidasLeitura>();
            bufferProcessing = new ConcurrentQueue<MedidasLeitura>[numThread];
            for(int i =0;i < numThread; i++)
            {
                bufferProcessing[i] = new ConcurrentQueue<MedidasLeitura>();
            }


            UdpClientReceptor = new UdpClient(portReceptor);
            IPReceptor = null;
            
            UdpClientSender = new UdpClient(portSender);
            targetEndpoint = new IPEndPoint(IPAddress.Broadcast, portTarget); //Parse("10.15.31.130")


            recievePackets = new Thread(recieveData);
            recievePackets.Start();

            distributingPackets = new Thread(parsingData);
            distributingPackets.Start();

            processData = new Thread(processingData);
            processData.Start();


            sendPackets = new Thread(sendData);
            sendPackets.Start();

        }
    }
}
