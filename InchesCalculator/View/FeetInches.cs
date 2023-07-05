using System;

namespace InchesCalculator
{
    class FeetInches
    {
        public int GetFeet(string feet)
        {            
            return Int32.Parse(feet);
        }

        public int GetInches (string inch)
        {
            return Int32.Parse(inch);
        }
        public double GetFractionalPart(string fractionalPart)
        {
            return double.Parse(fractionalPart);
        }
    }
    
}