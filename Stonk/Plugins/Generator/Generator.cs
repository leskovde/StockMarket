using Stonk.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Stonks.Plugins.Generator
{
    public class Generator
    {
        static Random rng = new Random();

        /// <summary>
        ///maximum value change per tick
        ///this maximum is hit rarely since the random change distribution is gaussian
        ///eg. 0.01 means 1%
        /// </summary>
        static readonly decimal ValueChangeRate = 0.295M;
        static readonly double GrowthChangeRate = 0.0501;

        /// random number between (-1,1), gaussian distribution
        static double RandGaussian() => (rng.NextDouble() - rng.NextDouble()) * rng.NextDouble();

        /// <summary>
        ///  performs random in-place modifications of a stock
        /// </summary>
        /// <param name="stock"></param>
        /// <param name="growthModifier">force-adjusts growthRate for all the iterations;  -1: rapid fall, 0: stagnation, 1: rapid growth </param>
        /// <returns></returns>
        public static Stock RandomlyModify(Stock stock, double growthModifier = 0.0)
        {
            var GrowthTrend = stock.growTrend;
            var Value = stock.firstValue;

            GrowthTrend = Math.Tanh(GrowthTrend + (RandGaussian() + growthModifier) * GrowthChangeRate);
            Value *= 1 + (decimal)(RandGaussian() + GrowthTrend) * ValueChangeRate;

            stock.firstValue = Value;
            stock.growTrend = GrowthTrend;
            return stock;
        }

    }
}



