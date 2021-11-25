using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization;
using System.Windows.Forms.DataVisualization.Charting;
using System.Diagnostics;

namespace MiniDolarMarket
{
    public class Model
    {
        string _name;
        int PesosTot = 0;
        int DolarsTot = 0;
        int ExchangeRate = 0;
        int InflationRate = 0;
        int Demand = 0;
        Boolean ApplyFuzzyRules = false;

        public int Dolars { get; set; }
        public int Pesos { get; set; }

        Dictionary<int, Person> Persons = new Dictionary<int, Person>();
        Bank CentralBank = new Bank();

        AgentFuzzyRules AgentRules;

        //Statistics
        int NumberOfExchanges = 0;

        public Model(string name)
        {
            _name = name;
        }

        public void setup(int ExchangeRate,int InflationRate,Boolean ApplyFuzzyRules)
        {
            //inicializar
            this.ExchangeRate = ExchangeRate;
            this.InflationRate = InflationRate;
            this.ApplyFuzzyRules = ApplyFuzzyRules;

            Demand = 10000;
            Dolars = 100000;
            Pesos = 500000;

            Random r = new Random();
            for (int i = 1; i < 100; i++)
            {

                int DolarsPers = r.Next(0, 100); Dolars = Dolars - DolarsPers; 
                int PesosPers = r.Next(0, 1000); Pesos = Pesos - PesosPers;
                Persons.Add(i, new Person(i, DolarsPers, PesosPers));
                //Console.WriteLine("Persona {0:N} tiene {1:N} y {2:N} dolares", i,Pesos,Dolars);
            }
            //Console.WriteLine("Total pesos: {0:N} Total Dolares {1:N}",PesosTot, DolarsTot);
            //SavetoCVS("Salida1.cvs");

            CentralBank.Pesos = Pesos;
            CentralBank.Dolars = Dolars;

            if (ApplyFuzzyRules) AgentRules = new AgentFuzzyRules();
        }


        public void run(int ticksCycles)
        {
            int ticks = 1;
            Random r2 = new Random();

            while (ticks<= ticksCycles)
            {
                Debug.WriteLine("Tick " + ticks);
                Debug.WriteLine("-------------------------------------------------");

                CentralBank.GetDolarsFromRet();

                //Print more pesos to increase salaries

                Demand = Demand + (int)((double)Demand * 0.10);  //Demand fuction                

                CentralBank.GetPesosFromPrint(Demand);

                InflationRate = InflationRate + (int)((double) Demand * 0.0001); //InflationRate rate function

                ExchangeRate = ExchangeRate + (InflationRate *2); // Exchange rate function

                foreach (KeyValuePair<int, Person> p in Persons)
                {

                    //Assign pesos to persons from salaries
                    

                    //select random
                    int idper = r2.Next(1, 100);
                    CentralBank.Sell(ExchangeRate,p.Value);

                    Person SellingPers;
                    Person BuyerPers;
                    idper = r2.Next(1, 100);
                    Debug.WriteLine("ID elegido" + idper);
                    SellingPers = Persons[idper];
                    BuyerPers = p.Value;
                    Boolean ExchangeOk = false;

                    if (SellingPers.Id != BuyerPers.Id && SellingPers.Dolars > 0)
                    {
                        Debug.WriteLine("Compra " + BuyerPers.Id + " a " + SellingPers.Id);
                        if (ApplyFuzzyRules)
                            ExchangeOk = SellingPers.SellWithFuzzyInflation(ExchangeRate,InflationRate,BuyerPers,AgentRules);
                        else
                            ExchangeOk = SellingPers.Sell(ExchangeRate, BuyerPers);
                        
                    }
                    if (ExchangeOk) NumberOfExchanges++;
                    //Debug.Assert(SellingPers.Dolars < 0);
                    //Debug.Assert(BuyerPers.Dolars < 0);

                }
                ticks++;
            }
            //SavetoCVS("Salida2.cvs");

        }


        public string reportStatistics()
        {
            string res = "Total Exchanges : " + NumberOfExchanges.ToString() + Environment.NewLine;
            res = res + "Pesos in Central Bank : " + CentralBank.Pesos + Environment.NewLine; 
            res = res + "Dolars in Central Bank : " + CentralBank.Dolars + Environment.NewLine;
            res = res + "Final Exchange rate : " + ExchangeRate.ToString() + Environment.NewLine;
            res = res + "Final Demand of Pesos: " + Demand.ToString() + Environment.NewLine;
            res = res + "Final InflationRate: " + InflationRate.ToString() + Environment.NewLine;

            return (res);

        }

        private void SavetoCVS(string filename)
        {
            using (var w = new StreamWriter(Application.StartupPath+ "\\"+ filename))
            {
                foreach (KeyValuePair<int, Person> p in Persons)
                {
                    var line = string.Format("{0},{1},{2}", p.Value.Id, p.Value.Pesos,p.Value.Dolars);
                    w.WriteLine(line);
                    w.Flush();
                }
            }
        }

        public void Graph(System.Windows.Forms.DataVisualization.Charting.Chart chart1)
        {
            chart1.Series.Clear(); //ensure that the chart is empty
            chart1.Series.Add("Series0");
            chart1.Series[0].ChartType = SeriesChartType.Column;
            chart1.Series[0].IsVisibleInLegend = true;
            chart1.Legends.Clear();
            foreach (KeyValuePair<int, Person> p in Persons)
            {
                chart1.Series[0].Points.AddXY(p.Value.Id,p.Value.Dolars);
            }
        }

        public void Grid(DataGridView dg)
        {
            foreach (KeyValuePair<int, Person> p in Persons)
            {
                dg.Rows.Add(p.Value.Id, p.Value.Pesos, p.Value.Dolars);
            }
        }
    }
}
