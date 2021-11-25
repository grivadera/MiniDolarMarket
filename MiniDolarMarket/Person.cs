using FLS;
using FLS.MembershipFunctions;
using FLS.Rules;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniDolarMarket
{
    public class Person
    {
        public Person(int Id, int dolars, int pesos)
        {
            this.Id = Id;
            Dolars = dolars;
            Pesos = pesos;
        }

        public int Id { get; }
        public int Dolars { get; set; }
        public int Pesos { get; set; }

        public static explicit operator Person(KeyValuePair<int, Person> v)
        {
            throw new NotImplementedException();
        }

        internal Boolean Sell(int exchangeRate, Person Buyer)
        {
            Boolean ExchangeOk = true;
            int CanBuy = Buyer.Pesos / exchangeRate;
            int CanBuyEff = (CanBuy > Dolars) ? Dolars : CanBuy;

            Debug.WriteLine("Buyer.Pesos = " + Buyer.Pesos + " CanBuy = " + CanBuyEff);
            //Seller
            Dolars = Dolars - CanBuyEff;
            Pesos = Pesos + CanBuyEff * exchangeRate;
            //Buyer
            Buyer.Pesos = Buyer.Pesos-(CanBuyEff * exchangeRate);
            Buyer.Dolars = Buyer.Dolars+ CanBuyEff;
            Debug.WriteLine("Buyer.Pesos = " + Buyer.Pesos);

            return (ExchangeOk);
        }


        internal Boolean SellWithFuzzyInflation(int exchangeRate,int InflationRate, Person Buyer,AgentFuzzyRules Rules)
        {
            Boolean ExchangeOk = false;
            int ResProb = (int) Rules.Defuzzify(InflationRate);

            Random rp = new Random();
            int ProbBuy = rp.Next(0, 100);

            if (ProbBuy<ResProb)
            {
                int CanBuy = Buyer.Pesos / exchangeRate;

                int CanBuyEff = (CanBuy > Dolars) ? Dolars : CanBuy;

                Debug.WriteLine("Buyer.Pesos = " + Buyer.Pesos + " CanBuy = " + CanBuyEff);
                //Seller
                Dolars = Dolars - CanBuyEff;
                Pesos = Pesos + CanBuyEff * exchangeRate;
                //Buyer
                Buyer.Pesos = Buyer.Pesos - (CanBuyEff * exchangeRate);
                Buyer.Dolars = Buyer.Dolars + CanBuyEff;
                Debug.WriteLine("Buyer.Pesos = " + Buyer.Pesos);
                ExchangeOk = true;
            }
            return (ExchangeOk);
        }

    }
}
