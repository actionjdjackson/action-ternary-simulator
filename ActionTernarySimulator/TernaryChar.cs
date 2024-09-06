using System;
namespace ActionTernarySimulator
{
    public class TernaryChar : TernaryNumGeneric
    {

        public char character;
        public string balancedTernaryValue;
        public int numericalValue;
        private static int nTrits;

        public TernaryChar(char character)
        {
            this.character = character;
            this.numericalValue = (int)character;
            this.balancedTernaryValue = ConvertCharToBalancedTernaryString(character);
            base.fullTernaryString = balancedTernaryValue;
        }

        public TernaryChar(string balancedTernary)
        {
            this.balancedTernaryValue = balancedTernary;
            this.character = ConvertBalancedTernaryStringToChar(balancedTernary);
            this.numericalValue = (int)character;
            base.fullTernaryString = balancedTernaryValue;
        }

        public override void ChangeValue()
        {
            base.ChangeValue();
            ChangeValue(base.fullTernaryString);
        }

        public override void Invert()
        {
            base.Invert();
            this.balancedTernaryValue = InvertBalancedTernaryString(this.balancedTernaryValue);
            base.fullTernaryString = balancedTernaryValue;
        }

        public void ChangeValue(char character)
        {
            this.character = character;
            this.balancedTernaryValue = ConvertCharToBalancedTernaryString(character);
            this.numericalValue = (int)character;
            base.fullTernaryString = balancedTernaryValue;
        }

        public void ChangeValue(string balancedTernary)
        {
            this.balancedTernaryValue = balancedTernary;
            this.character = ConvertBalancedTernaryStringToChar(balancedTernary);
            this.numericalValue = (int)character;
            base.fullTernaryString = balancedTernaryValue;
        }

        public static void SetUpTernaryImplementation(int nTrits)
        {
            TernaryChar.nTrits = nTrits;
        }

        public static char ConvertBalancedTernaryStringToChar(string ternaryString)
        {
            var workChars = ternaryString.ToCharArray();
            Array.Reverse(workChars);
            //var reversedTernString = workChars.ToString();
            int sum = 0;
            double exponent = 0;
            foreach (char trit in workChars)
            {
                if (trit == '+')
                {
                    sum += (int)Math.Round(Math.Pow(3.0, exponent));
                }
                else if (trit == '-')
                {
                    sum -= (int)Math.Round(Math.Pow(3.0, exponent));
                }
                else if (trit == '0')
                {
                    //do nothing, this place is zero
                }
                else
                {
                    Console.WriteLine("ERROR: Invalid trit character in ternaryString");
                }
                exponent++;
            }
            return (char)sum;
        }

        public static string ConvertCharToBalancedTernaryString(char character)
        {
            var workValue = (int)character;
            var workString = "";
            workValue = Math.Abs(workValue);
            while (workValue != 0)
            {
                if (workValue % 3 == 0)
                {
                    workString = "0" + workString;
                }
                else if (workValue % 3 == 1)
                {
                    workString = "+" + workString;
                }
                else if (workValue % 3 == 2)
                {
                    workString = "-" + workString;
                }
                workValue = (workValue + 1) / 3;
            }
            if ((int) character < 0)
            {
                workString = InvertBalancedTernaryString(workString);
            }
            if (workString == "")
            {
                workString = "0";
            }
            if (workString.Length < nTrits)
            {
                var len = workString.Length;
                for (int i = 0; i < nTrits - len; i++)
                {
                    workString = "0" + workString;
                }
            }

            return workString;
        }

        public static string InvertBalancedTernaryString(string ternaryString)
        {
            var invertedTernaryString = "";
            foreach (char trit in ternaryString)
            {
                if (trit == '0')
                {
                    invertedTernaryString += "0";
                }
                else if (trit == '+')
                {
                    invertedTernaryString += "-";
                }
                else if (trit == '-')
                {
                    invertedTernaryString += "+";
                }
            }
            return invertedTernaryString;
        }

    }
}
