using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniDolarMarket
{
    class Bank
    {

        public int Dolars { get; set; }
        public int Pesos { get; set ; }

        public void GetDolarsFromRet()
        {
            Random r = new Random();
            Dolars = Dolars + r.Next(0,10000);

        }

        public void GetPesosFromPrint(int Demand)
        {
            //Who is deciding how to emit
            Pesos = Pesos + Demand;
        }

        internal Boolean Sell(int exchangeRate, Person Buyer)
        {
            Boolean ExchangeOk = false;
            int CanBuy = Buyer.Pesos / exchangeRate;
            int CanBuyEff = (CanBuy > Dolars) ? Dolars : CanBuy;

            if (CanBuyEff < 200) // Limit
            { 
                //Seller
                Dolars = Dolars - CanBuyEff;
                Pesos = Pesos + CanBuyEff * exchangeRate;
                //Buyer
                Buyer.Pesos = Buyer.Pesos - (CanBuyEff * exchangeRate);
                Buyer.Dolars = Buyer.Dolars + CanBuyEff;
                ExchangeOk = true;
                
            }
            return (ExchangeOk);
        }

    }
}
