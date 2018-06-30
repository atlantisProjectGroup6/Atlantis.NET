using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalculationEngine
{
    class Formulas
    {
        public float AverageFromPrevAverage(float prevAverage, float newValue, int nbValue)
        {
            return ((prevAverage * nbValue) + newValue) / (nbValue + 1);
        }
    }
}
