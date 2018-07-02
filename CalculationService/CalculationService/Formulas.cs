using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CalculationService
{
    public class Formulas
    {
        public float AverageFromPrevAverage(float prevAverage, float newValue, int nbValue)
        {
            return ((prevAverage * nbValue) + newValue) / (nbValue + 1);
        }
    }
}