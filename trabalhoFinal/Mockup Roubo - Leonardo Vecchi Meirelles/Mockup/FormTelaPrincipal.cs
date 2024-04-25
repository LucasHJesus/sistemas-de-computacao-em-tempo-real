using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using Newtonsoft.Json;
using ScottPlot.Drawing;
using ScottPlot;
using ScottPlot.Drawing.Colormaps;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Linq.Expressions;
using System.Diagnostics;

namespace Pratica2
{

    public partial class FormTelaPrincipal : Form
    {
        static Stopwatch Stopwatch = null;
        //MODULO 2:
        Thread threadEnvioPacotesConstantes = null;
        Thread threadEnvioPacotesPadronizacao = null;
        UdpClient usocketConexaoUDPModulo2 = null;
        IPEndPoint ipConexaoEnvioUDPModulo2 = null;
        string menssagemRecebidaModulo2;
        string formatoPacoteModulo2;
        byte[] bytesRecebidosModulo2;
        byte[] bytesAEnviarModulo2;
        private List<int> dadosPlotarGraficoModulo2 = new List<int>();
        bool geraRouboEnergiaButton = false;
        double maxNeighboringDistance = 0.1;
        double maxEnergyIncrease = 50.0;
        double minEnergyIncrease = 40.0;
        private bool isRunning = true;


        public FormTelaPrincipal()
        {
            InitializeComponent();
            Control.CheckForIllegalCrossThreadCalls = false; //possibilita que componentes sejam chamados por threads diferentes                                    //PASSO 1: encontra o IP e atribui a classe principal de dados e controle
        }

        private void iniciaConexao()
        {
            
            ipConexaoEnvioUDPModulo2 = new IPEndPoint(IPAddress.Parse("255.255.255.255"), 10728);
        }

        private void buttonIniciarMod2_Click(object sender, EventArgs e)
        {
            //parte 0: inicializacoes de interface
            buttonIniciarMod2.Enabled = false;
            buttonPararMod2.Enabled = true;
            dadosPlotarGraficoModulo2.Clear();
            geraRouboEnergiaButton = false;
            progressBarEnvioModulo2.Maximum = (int)numericUpDownQtdePacotes.Value;
            progressBarEnvioModulo2.Value = 0;


            //Parte 1: inicia conexao UDP
            usocketConexaoUDPModulo2 = new UdpClient(1234);
            iniciaConexao();
            threadEnvioPacotesPadronizacao = new Thread(EnvioPacotesUDP_Padronizacao);
            threadEnvioPacotesPadronizacao.Start();
            threadEnvioPacotesPadronizacao.Join();
            threadEnvioPacotesConstantes = new Thread(EnvioPacotesUDP_Constantes);
            threadEnvioPacotesConstantes.Start();
        }
        private void buttonPararMod2_Click(object sender, EventArgs e)
        {
            //parte 0: inicializacoes de interface
            //buttonIniciarMod2.Enabled = true;
            //buttonPararMod2.Enabled = false;
            //pararRecebimentoDadosModulo2 = true;
            geraRouboEnergiaButton = true;

            //Parte 1: inicia conexao UDP
            //Thread.Sleep(100); // vamos dar um tempo antes de fechar
            //usocketConexaoUDPModulo2.Close();
            //Close();
        }

        private double CalculateDistance(double lat1, double lon1, double lat2, double lon2)
        {
            // Convert latitude and longitude from degrees to radians
            double lat1Rad = lat1 * Math.PI / 180.0;
            double lon1Rad = lon1 * Math.PI / 180.0;
            double lat2Rad = lat2 * Math.PI / 180.0;
            double lon2Rad = lon2 * Math.PI / 180.0;

            // Earth radius in kilometers
            double earthRadius = 6371.0;

            // Calculate the differences
            double deltaLat = lat2Rad - lat1Rad;
            double deltaLon = lon2Rad - lon1Rad;

            // Haversine formula to compute distance
            double a = Math.Sin(deltaLat / 2) * Math.Sin(deltaLat / 2) +
                       Math.Cos(lat1Rad) * Math.Cos(lat2Rad) *
                       Math.Sin(deltaLon / 2) * Math.Sin(deltaLon / 2);
            double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            double distance = earthRadius * c;

            return distance;
        }


        //working packet 1
        //private void EnvioPacotesUDP_Padronizacao()
        //{
        //    Random rnd = new Random();
        //    double minLat = -18.98;
        //    double maxLat = -18.85;
        //    double minLong = -48.37;
        //    double maxLong = -48.2;

        //    for (int i = 0; i < numericUpDownQtdePacotes.Value; i++)
        //    {
        //        double latitude = rnd.NextDouble() * (maxLat - minLat) + minLat;
        //        double longitude = rnd.NextDouble() * (maxLong - minLong) + minLong;

        //        formatoPacoteModulo2 = "{\"Lat\":" + latitude.ToString(CultureInfo.InvariantCulture) +
        //                              ",\"Long\":" + longitude.ToString(CultureInfo.CreateSpecificCulture("en-GB")) +
        //                              ",\"ID\":" + i + "}";

        //        //"N", CultureInfo.CreateSpecificCulture("en-US")
        //        progressBarEnvioModulo2.Value = i;
        //        bytesAEnviarModulo2 = Encoding.ASCII.GetBytes(formatoPacoteModulo2);
        //        if (usocketConexaoUDPModulo2 != null)
        //            usocketConexaoUDPModulo2.Send(bytesAEnviarModulo2, bytesAEnviarModulo2.Length, ipConexaoEnvioUDPModulo2);
        //    }
        //}

        double[,] houseCoordinates;

        private void EnvioPacotesUDP_Padronizacao()
        {
            Random rnd = new Random();
            double minLat = -18.98;
            double maxLat = -18.85;
            double minLong = -48.37;
            double maxLong = -48.2;

            // Initialize the array
            houseCoordinates = new double[(int)numericUpDownQtdePacotes.Value, 2];

            for (int i = 0; i < numericUpDownQtdePacotes.Value; i++)
            {
                double latitude = rnd.NextDouble() * (maxLat - minLat) + minLat;
                double longitude = rnd.NextDouble() * (maxLong - minLong) + minLong;

                houseCoordinates[i, 0] = latitude;
                houseCoordinates[i, 1] = longitude;

                formatoPacoteModulo2 = "{\"Lat\":" + latitude.ToString(CultureInfo.InvariantCulture) +
                                      ",\"Long\":" + longitude.ToString(CultureInfo.CreateSpecificCulture("en-GB")) +
                                      ",\"ID\":" + i + "}";

                //"N", CultureInfo.CreateSpecificCulture("en-US")
                progressBarEnvioModulo2.Value = i;
                bytesAEnviarModulo2 = Encoding.ASCII.GetBytes(formatoPacoteModulo2);
                if (usocketConexaoUDPModulo2 != null)
                    usocketConexaoUDPModulo2.Send(bytesAEnviarModulo2, bytesAEnviarModulo2.Length, ipConexaoEnvioUDPModulo2);
            }
            /*formatoPacoteModulo2 = "{" + "\"Ready\":" + 0 + "}";
            bytesAEnviarModulo2 = Encoding.ASCII.GetBytes(formatoPacoteModulo2);
            if (usocketConexaoUDPModulo2 != null)
                usocketConexaoUDPModulo2.Send(bytesAEnviarModulo2, bytesAEnviarModulo2.Length, ipConexaoEnvioUDPModulo2);*/
        }

        private void EnvioPacotesUDP_Constantes()
        {
            Random rnd = new Random();
            double maxEnergyConsumption = 50.0;
            double minEnergyConsumption = 40.0;
            double anomalyEnergyMin = 20.0;
            double anomalyEnergyMax = 30.0;
            double lowConsumptionMin = 0.0;
            double lowConsumptionMax = 20.0;
            double lowConsumptionProbability = 0.05;
            int numeroTotalCasas = (int)numericUpDownQtdePacotes.Value; // Convert the value to an integer
            int j = 0;
            // Define a flag array to mark houses with anomalies
            bool[] housesWithAnomalies = new bool[numeroTotalCasas];
            // Define a flag array to mark houses with low consumption
            bool[] housesWithLowConsumption = new bool[numeroTotalCasas];
            // Array to store the previous anomaly state of each house
            bool[] previousAnomalyState = new bool[numeroTotalCasas];

            Stopwatch = new Stopwatch();
            // Define a boolean vector to track houses affected by neighboring anomalies
            bool[] affectedByAnomalies = new bool[numeroTotalCasas];
            // Define a list to store neighboring houses for each house
            List<int>[] neighborLists = new List<int>[numeroTotalCasas];

            // Pre-calculate neighbor lists for each house
            for (int i = 0; i < numeroTotalCasas; i++)
            {
                progressBarEnvioModulo2.Value = i;
                neighborLists[i] = new List<int>();
                for (int k = 0; k < numeroTotalCasas; k++)
                {
                    if (k != i && CalculateDistance(houseCoordinates[i, 0], houseCoordinates[i, 1], houseCoordinates[k, 0], houseCoordinates[k, 1]) <= maxNeighboringDistance)
                    {
                        neighborLists[i].Add(k);
                    }
                }
            }

            while (true)
            {
                Stopwatch.Start();
                for (int i = 0; i < numericUpDownQtdePacotes.Value; i++)
                {
                    double consumoEnergia;

                    // Check if the house has an anomaly
                    if (housesWithAnomalies[i])
                    {
                        // If it's a new anomaly, set consumption to zero
                        if (!previousAnomalyState[i])
                        {
                            consumoEnergia = 0;
                        }
                        else
                        {
                            // Set consumption to a random value within the anomaly energy range
                            consumoEnergia = rnd.NextDouble() * (anomalyEnergyMax - anomalyEnergyMin) + anomalyEnergyMin;
                        }
                        // Set the affected flag for all neighbors
                        foreach (int neighbor in neighborLists[i])
                        {
                            affectedByAnomalies[neighbor] = true;
                        }
                    }
                    else if (housesWithLowConsumption[i])
                    {
                        // Set consumption to a random value within the low consumption range
                        consumoEnergia = rnd.NextDouble() * (lowConsumptionMax - lowConsumptionMin) + lowConsumptionMin;
                        housesWithLowConsumption[i] = false;
                    }
                    else
                    {
                        // Generate normal energy consumption
                        consumoEnergia = rnd.NextDouble() * (maxEnergyConsumption - minEnergyConsumption) + minEnergyConsumption;

                        // Ensure that energy consumption does not exceed the maximum limit for houses without anomalies
                        consumoEnergia = Math.Min(consumoEnergia, maxEnergyConsumption);
                    }
                    if (affectedByAnomalies[i])
                    {
                        // Increase consumption of house i due to neighboring anomaly
                        double increaseAmount = rnd.NextDouble() * (maxEnergyIncrease - minEnergyIncrease) + minEnergyIncrease;
                        consumoEnergia += increaseAmount;
                    }

                    //Check for neighboring houses and increase their energy consumption if necessary
                    /*for (int k = 0; k < numeroTotalCasas; k++)
                            {
                                // Skip the current house and houses without anomalies
                                if (k != i && housesWithAnomalies[k])
                                {
                                    // Calculate the distance between houses using their coordinates
                                    double distance = CalculateDistance(houseCoordinates[i, 0], houseCoordinates[i, 1], houseCoordinates[k, 0], houseCoordinates[k, 1]);

                                    // Increase energy consumption of neighboring houses if they are within a certain distance
                                    if (distance <= maxNeighboringDistance)
                                    {
                                        double increaseAmount = rnd.NextDouble() * (maxEnergyIncrease - minEnergyIncrease) + minEnergyIncrease;
                                        consumoEnergia += increaseAmount;

                                        Console.WriteLine("Energy consumption increased for house " + i + " due to anomaly in house " + k);
                                    }
                                }
                            }*/

                    // Construct the JSON string
                    formatoPacoteModulo2 = "{\"ID\":" + i + ",\"ConsumoEnergia\":" + consumoEnergia.ToString(CultureInfo.InvariantCulture) + "}";

                    progressBarEnvioModulo2.Value = i;
                    bytesAEnviarModulo2 = Encoding.ASCII.GetBytes(formatoPacoteModulo2);
                    if (usocketConexaoUDPModulo2 != null)
                        try
                        {
                            usocketConexaoUDPModulo2.Send(bytesAEnviarModulo2, bytesAEnviarModulo2.Length, ipConexaoEnvioUDPModulo2);
                        }
                        catch (Exception ex)
                        {
                            iniciaConexao();


                        }
                    // Update the previous anomaly state for the current house
                    previousAnomalyState[i] = housesWithAnomalies[i];

                    // Check if the anomaly button is pressed
                    if (geraRouboEnergiaButton)
                    {
                        // Set anomaly flag for a random house
                        int houseIndexWithAnomaly = rnd.Next(numeroTotalCasas);
                        //int houseIndexWithAnomaly = 20;
                        housesWithAnomalies[houseIndexWithAnomaly] = true;

                        Console.WriteLine("Anomaly generated for house " + houseIndexWithAnomaly);

                        // Reset the button flag
                        geraRouboEnergiaButton = false;
                    }

                    // Randomly select houses for low consumption
                    if (rnd.NextDouble() < lowConsumptionProbability)
                    {
                        // Set low consumption flag for the current house
                        housesWithLowConsumption[i] = true;
                    }
                    
                }
                //j++;
                Stopwatch.Stop();
                //MessageBox.Show("time elapsed: " + Stopwatch.Elapsed.TotalMilliseconds.ToString());
            }
           

        }

        private void button1_Click(object sender, EventArgs e)
        {
            isRunning = false;  // Set flag to stop sending packets
                                // Optionally, wait for threads to finish sending (if applicable)
                                // ... (wait logic if needed)

            // Close UDP socket (if created)
            if (usocketConexaoUDPModulo2 != null)
            {
                usocketConexaoUDPModulo2.Close();
            }

            // Close the application (can be modified to hide or minimize instead)
            Application.Exit();
        }
    }//fim classe
}
