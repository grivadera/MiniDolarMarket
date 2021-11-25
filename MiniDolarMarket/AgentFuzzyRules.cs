using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FLS;
using FLS.MembershipFunctions;
using FLS.Rules;

namespace MiniDolarMarket
{
    class AgentFuzzyRules
    {
        LinguisticVariable Inflation;
        IMembershipFunction CreepingInflation;
        IMembershipFunction WalkingInflation;
        IMembershipFunction GallopingInflation;
        IMembershipFunction HyperInflation;

        LinguisticVariable BuyingProbability;
        IMembershipFunction low;
        IMembershipFunction high;

        IFuzzyEngine fuzzyEngine = new FuzzyEngineFactory().Default();

        FuzzyRule rule2;
        FuzzyRule rule1;

        public AgentFuzzyRules()
        {
            Inflation = new LinguisticVariable("Inflation");
            CreepingInflation = Inflation.MembershipFunctions.AddTrapezoid("Creeping", 0, 0, 3, 5);
            WalkingInflation = Inflation.MembershipFunctions.AddTriangle("WalkingInflation", 3, 10, 15);
            GallopingInflation = Inflation.MembershipFunctions.AddTriangle("GallopingInflation", 12, 18, 50);
            HyperInflation = Inflation.MembershipFunctions.AddTrapezoid("HyperInflation", 50, 80, 100, 100);

            BuyingProbability = new LinguisticVariable("BuyingProbability");
            low = BuyingProbability.MembershipFunctions.AddTriangle("Low", 0, 25, 50);
            high = BuyingProbability.MembershipFunctions.AddTriangle("High", 25, 50, 75);


            rule2 = Rule.If(Inflation.Is(CreepingInflation)).Then(BuyingProbability.Is(low));
            rule1 = Rule.If(Inflation.Is(WalkingInflation).Or(Inflation.Is(GallopingInflation)).Or(Inflation.Is(HyperInflation))).Then(BuyingProbability.Is(high));

            fuzzyEngine.Rules.Add(rule1, rule2);

        }

        public double Defuzzify(int CurrentInflation)
        {            
            return(fuzzyEngine.Defuzzify(new { Inflation = CurrentInflation }));
        }

    }
}
