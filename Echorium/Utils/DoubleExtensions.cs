using System;

namespace Echorium.Utils
{
    public static class DoubleExtensions
    {
        /// <summary>
        /// Compare if 2 double values are equal
        /// </summary>
        /// <param name="firstValue"></param>
        /// <param name="secondValue"></param>
        /// <returns></returns>
        public static bool AlsoEqual(this double firstValue, double secondValue)
        {
            if (!double.IsFinite(firstValue) || !double.IsFinite(secondValue))
                return false;

            return Math.Abs(firstValue - secondValue) < double.Epsilon;
        }
    }
}
