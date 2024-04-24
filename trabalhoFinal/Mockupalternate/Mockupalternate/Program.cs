using System.Diagnostics;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Text;

namespace Mockupalternate
{
    internal class Program
    {
        static Thread sender = null;
        static Thread reciever = null;
        static Stopwatch sw = null;

        static double maxLat = -18.85;
        static double minLat = -18.98;
        static double maxLong = -48.2;
        static double minLong = -48.37;

        
        static void send()
        {
            Random rnd = new Random();
            double valorASomar = 0;
            double latMid = rnd.NextDouble() * (maxLat - minLat) + minLat;
            double longMid = rnd.NextDouble() * (maxLong - minLong) + minLong;
            double corrente;
            double latitude;
            double longitude;

            int numeroPacotes;
            Console.WriteLine("Numero total de pacotes");
            numeroPacotes =Convert.ToInt32(Console.ReadLine());
            int numeroPacotesFalta;
            Console.WriteLine("Numero total de pacotes com falta");
            numeroPacotesFalta = Convert.ToInt32(Console.ReadLine());

            for (int i = 0; i < numeroPacotes; i++)
            {
                if (flagStop)
                    break;

                if (i < numeroPacotesFalta) //os primeiros pacotes, vou mandar eles pertecendo a uma mesma regiao geografica inserindo pouca variacao nas coordenadas geograficas
                {
                    valorASomar = (double)(rnd.Next(-10, 10) / 5000.0); //pequena variacao de posicao
                    latitude = latMid + valorASomar;
                    longitude = longMid + valorASomar;
                    corrente = 0.0;

                }
                else
                {
                    latitude = rnd.NextDouble() * (maxLat - minLat) + minLat;
                    longitude = rnd.NextDouble() * (maxLong - minLong) + minLong;
                    corrente = rnd.NextDouble() * 100;
                }

                formatoPacoteModulo2 = "{\"URI\":\"99/1\"," +
                                        "\"idMU\":" + i.ToString(CultureInfo.InvariantCulture) +
                                        ",\"latitude\":" + latitude.ToString(CultureInfo.InvariantCulture) + ",\"longitude\":" + longitude.ToString(CultureInfo.InvariantCulture) +
                                        ",\"idAtivo\":\"\"" +
                                        ",\"numPct\":" + i.ToString(CultureInfo.InvariantCulture) +
                                        ",\"timeStamping\":\"\"" +
                                        ",\"freqEnvioMS\":" + "1000" +
                                        ",\"medidas\":" + "[{\"fase\":\"A\"" +
                                                           ",\"tensao\":" + "220.0" +
                                                           ",\"corrente\":" + (corrente).ToString(CultureInfo.InvariantCulture) +
                                                           ",\"angTensao\": \"\",\"potAtivaVA\":0.0,\"potReativaVAr\":0.0,\"potRealW\":0.0,\"fatorP\":0.0,\"freq\":60}," +
                                                           "{\"fase\":\"B\"" +
                                                           ",\"tensao\":" + "220.0" +
                                                           ",\"corrente\":" + (corrente).ToString(CultureInfo.InvariantCulture) +
                                                           ",\"angTensao\": 0.0,\"potAtivaVA\":0.0,\"potReativaVAr\":0.0,\"potRealW\":0.0,\"fatorP\":0.0,\"freq\":60}," +
                                                           "{\"fase\":\"C\"" +
                                                           ",\"tensao\":" + "220.0" +
                                                           ",\"corrente\":" + (corrente).ToString(CultureInfo.InvariantCulture) +
                                                           ",\"angTensao\": 0.0,\"potAtivaVA\":0.0,\"potReativaVAr\":0.0,\"potRealW\":0.0,\"fatorP\":0.0,\"freq\":60}]}";



                bytesAEnviarModulo2 = Encoding.ASCII.GetBytes(formatoPacoteModulo2);
                if (usocketConexaoUDPModulo2 != null)
                {
                    try
                    {
                        usocketConexaoUDPModulo2.Send(bytesAEnviarModulo2, bytesAEnviarModulo2.Length, ipConexaoEnvioUDPModulo2);
                    }
                    catch (Exception ex) { }
                }
            }
            if (usocketConexaoUDPModulo2 != null)
                usocketConexaoUDPModulo2.Close();
           

           

            
        }

        static void recieve()
        {

        }

        static void Main(string[] args)
        {
            sender = new Thread(send);
            sw.Start();
            sender.Start();

            reciever = new Thread(recieve);
            reciever.Start();
            reciever.Join();



        }
    }
}
