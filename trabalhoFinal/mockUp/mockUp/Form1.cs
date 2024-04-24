using System.Globalization;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System;
using System.Diagnostics;

namespace mockUp
{
    public partial class Form1 : Form
    {
        Thread threadEnvioPacotesModulo2 = null;
        UdpClient usocketConexaoUDPModulo2 = null;
        IPEndPoint ipConexaoEnvioUDPModulo2 = null;

        static Stopwatch watch = null;

        int modoDeGeracao;
        double latitude;
        double longitude;
        string formatoPacoteModulo2;
        byte[] bytesAEnviarModulo2;

        double maxLat;
        double minLat;
        double maxLong;
        double minLong;

        bool flagStop;

        public Form1()
        {
            InitializeComponent();
            maxLat = -18.85;
            minLat = -18.98;
            maxLong = -48.2;
            minLong = -48.37;
            flagStop = false;
        }

        private void rbNormal_CheckedChanged(object sender, EventArgs e)
        {
            modoDeGeracao = 1;
        }

        private void rbFaltaDeEnergia_CheckedChanged(object sender, EventArgs e)
        {
            modoDeGeracao = 2;
        }

        private void rbFurtoDeEnergia_CheckedChanged(object sender, EventArgs e)
        {
            modoDeGeracao = 3;
        }

        private void btnParar_Click(object sender, EventArgs e)
        {
            if (usocketConexaoUDPModulo2 != null)
                usocketConexaoUDPModulo2.Close();
            flagStop = true;
        }

        private void btnGerar_Click(object sender, EventArgs e)
        {
            watch = new Stopwatch();
            usocketConexaoUDPModulo2 = new UdpClient(1234);
            //IPAddress.Broadcast
            //IPAddress.Parse("255.255.255.255")
            ipConexaoEnvioUDPModulo2 = new IPEndPoint(IPAddress.Parse("255.255.255.255"), 1235);

            switch (modoDeGeracao)
            {
                case 2:
                    threadEnvioPacotesModulo2 = new Thread(EnvioPacotesFalta);
                    break;
                case 3:
                    threadEnvioPacotesModulo2 = new Thread(EnvioPacoteFurto);
                    break;
                default:
                    threadEnvioPacotesModulo2 = new Thread(EnvioPacoteNormal);
                    break;
            }
            threadEnvioPacotesModulo2.Start();

        }

        private void EnvioPacoteNormal()
        {
            Random rnd = new Random();
            double valorASomar = 0;

            watch.Start();

            for (int i = 0; i < nudTotalDePacotes.Value; i++)
            {
                if (flagStop)
                    break;

                latitude = rnd.NextDouble() * (maxLat - minLat) + minLat;
                longitude = rnd.NextDouble() * (maxLong- minLong) + minLong;
                /*
                 URI
                idMU
                coord
                idAtivo
                numPct
                timeStamping
                freqEnvioMS
                medidas = [{fase
                            tensão
                            corrente
                            angTensão
                            potApaVA
                            potReatVAr
                            potRealW
                            fatorP
                            freq},
                           {fase
                            tensão
                            corrente
                            angTensão
                            potApaVA
                            potReatVAr
                            potRealW
                            fatorP
                            freq},
                           {fase
                            tensão
                            corrente
                            angTensão
                            potApaVA
                            potReatVAr
                            potRealW
                            fatorP
                            freq}]
                 */
               
               formatoPacoteModulo2 = "{\"URI\":\"99/1\"," +
                                        "\"idMU\":" + i.ToString() +
                                        ",\"latitude\":" + latitude.ToString(CultureInfo.InvariantCulture) + ",\"longitude\":" + longitude.ToString(CultureInfo.InvariantCulture) +
                                        ",\"idAtivo\":\"\"" +
                                        ",\"numPct\":" + i.ToString() +
                                        ",\"timeStamping\":\"\""+
                                        ",\"freqEnvioMS\":" + "1000" +
                                        ",\"medidas\":" + "[{\"fase\":\"A\"" +
                                                           ",\"tensao\":" + "220.0" +
                                                           ",\"corrente\":" + (rnd.NextDouble() * 100).ToString(CultureInfo.InvariantCulture) +
                                                           ",\"angTensao\": 0.0,\"potAtivaVA\":0.0,\"potReativaVAr\":0.0,\"potRealW\":0.0,\"fatorP\":0.0,\"freq\":60}," +
                                                           "{\"fase\":\"B\"" +
                                                           ",\"tensao\":" + "220.0" +
                                                           ",\"corrente\":" + (rnd.NextDouble() * 100).ToString(CultureInfo.InvariantCulture) +
                                                           ",\"angTensao\": \"\",\"potAtivaVA\":0.0,\"potReativaVAr\":0.0,\"potRealW\":0.0,\"fatorP\":0.0,\"freq\":60}," +
                                                           "{\"fase\":\"C\"" +
                                                           ",\"tensao\":" + "220.0" +
                                                           ",\"corrente\":" + (rnd.NextDouble() * 100).ToString(CultureInfo.InvariantCulture) +
                                                           ",\"angTensao\": 0.0,\"potAtivaVA\":0.0,\"potReativaVAr\":0.0,\"potRealW\":0.0,\"fatorP\":0.0,\"freq\":60}]}";
                

                bytesAEnviarModulo2 = Encoding.ASCII.GetBytes(formatoPacoteModulo2);
                if (usocketConexaoUDPModulo2 != null)
                {
                    try 
                    {
                        usocketConexaoUDPModulo2.Send(bytesAEnviarModulo2, bytesAEnviarModulo2.Length, ipConexaoEnvioUDPModulo2);
                    } catch (Exception ex) { }
                }
            }
            if (usocketConexaoUDPModulo2 != null)
                usocketConexaoUDPModulo2.Close();
            flagStop = false;

            watch.Stop();

            TimeSpan tempo = watch.Elapsed;

            
            //MessageBox.Show("time elapsed: " + tempo.TotalMilliseconds + " ms");
        }

        private void EnvioPacotesFalta()
        {
            watch.Start();
            Random rnd = new Random();
            double valorASomar = 0;
            double latMid = rnd.NextDouble() * (maxLat - minLat) + minLat;
            double longMid = rnd.NextDouble() * (maxLong - minLong) + minLong;
            double corrente;

            for (int i = 0; i < nudTotalDePacotes.Value; i++)
            {
                if (flagStop)
                    break;

                if (i < nudPacotesFalta.Value) //os primeiros pacotes, vou mandar eles pertecendo a uma mesma regiao geografica inserindo pouca variacao nas coordenadas geograficas
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
            flagStop = false;

            watch.Stop();

            TimeSpan tempo = watch.Elapsed;

            //MessageBox.Show("time elapsed: " + tempo.TotalMilliseconds + " ms");
        }

        private void EnvioPacoteFurto()
        {

            formatoPacoteModulo2 = "{\"Lat\":" + latitude.ToString(CultureInfo.InvariantCulture) +
                                      ",\"Long\":" + longitude.ToString(CultureInfo.CreateSpecificCulture("en-GB")) +
                                      ",\"codErro\":95}";
            //envia(formatoPacoteModulo2);

            //Timer(1m);

            while (true)
            {
                formatoPacoteModulo2 = "{\"Lat\":" + latitude.ToString(CultureInfo.InvariantCulture) +
                                      ",\"Long\":" + longitude.ToString(CultureInfo.CreateSpecificCulture("en-GB")) +
                                      "}";
                //envia(formatoPacoteModulo2);
            }
        }

    }
}
