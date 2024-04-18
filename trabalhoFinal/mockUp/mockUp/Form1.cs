using System.Globalization;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System;

namespace mockUp
{
    public partial class Form1 : Form
    { 
        Thread threadEnvioPacotesModulo2 = null;
        UdpClient usocketConexaoUDPModulo2 = null;
        //IPEndPoint ipConexaoRecebimentoUDPModulo2 = null;
        IPEndPoint ipConexaoEnvioUDPModulo2 = null;

        int modoDeGeracao;
        double latitude;
        double longitude;
        string formatoPacoteModulo2;
        byte[] bytesAEnviarModulo2;
        public Form1()
        {
            InitializeComponent();
            Console.WriteLine("suiii");
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

        private void btnGerar_Click(object sender, EventArgs e)
        {
            Console.WriteLine("aqui");
            usocketConexaoUDPModulo2 = new UdpClient(1234);
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



            for (int i = 0; i < nudTotalDePacotes.Value; i++)
            {
                if (i < nudPacoteslFurto.Value) //os primeiros pacotes, vou mandar eles pertecendo a uma mesma regiao geografica inserindo pouca variacao nas coordenadas geograficas
                    valorASomar = (double)(rnd.Next(-10, 10) / 5000.0); //pequena variacao de posicao
                else
                    valorASomar = (double)(rnd.Next(-10, 10) / 50.0); //grande variacao de posicao
                latitude = latitude + valorASomar;
                longitude = longitude + valorASomar;

                //latitude = rnd.NextDouble() * (maxDouble - minDouble) + minDouble;
                //longitude = rnd.NextDouble() * (maxDouble - minDouble) + minDouble;


                formatoPacoteModulo2 = "{\"Lat\":" + latitude.ToString(CultureInfo.InvariantCulture) +
                                       ",\"Long\":" + longitude.ToString(CultureInfo.CreateSpecificCulture("en-GB")) +
                                       ",\"codErro\":95}";
                //"N", CultureInfo.CreateSpecificCulture("en-US")

                bytesAEnviarModulo2 = Encoding.ASCII.GetBytes(formatoPacoteModulo2);
                if (usocketConexaoUDPModulo2 != null)
                {
                    usocketConexaoUDPModulo2.Send(bytesAEnviarModulo2, bytesAEnviarModulo2.Length, ipConexaoEnvioUDPModulo2);
                    Console.WriteLine(i);
                }
            }
        }

        private void EnvioPacotesFalta()
        {
            
            
           
        }
    
        private void EnvioPacoteFurto()
        {

            formatoPacoteModulo2 = "{\"Lat\":" + latitude.ToString(CultureInfo.InvariantCulture) +
                                      ",\"Long\":" + longitude.ToString(CultureInfo.CreateSpecificCulture("en-GB")) +
                                      ",\"codErro\":95}";
            //envia(formatoPacoteModulo2);

            //Timer(1m);

            while(true)
            {
                formatoPacoteModulo2 = "{\"Lat\":" + latitude.ToString(CultureInfo.InvariantCulture) +
                                      ",\"Long\":" + longitude.ToString(CultureInfo.CreateSpecificCulture("en-GB")) +
                                      "}";
                //envia(formatoPacoteModulo2);
            }
        }
    }
}
