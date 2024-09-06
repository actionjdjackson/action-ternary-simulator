using System;

namespace ActionTernarySimulator
{
    public class TernaryIntGeneric : TernaryNumGeneric
    {

        public enum TypeCodeEnum
        {
            Complex = 4,
            Imaginary = 3,
            Real = 2,
            Max = 1,
            Normal = 0,
            Min = -1,
            iVector = -2,
            jVector = -3,
            kVector = -4
        }

        public Int64 integerValue = 0;
        public TypeCodeEnum typeCode = TypeCodeEnum.Normal;
        public string integerPartTernaryValue = "";
        public string typeCodeTernary = "";
        public string fullTernaryValue = "";
        public static int TotalNTrits { get => totalNTrits; set => totalNTrits = value; }
        public static int NTypeTrits { get => nTypeTrits; set => nTypeTrits = value; }
        public static int NIntegerTrits { get => nIntegerTrits; set => nIntegerTrits = value; }
        private static int nIntegerTrits;
        private static int totalNTrits;
        private static int nTypeTrits;

        public TernaryIntGeneric(Int64 integerValue)
        {
            this.integerValue = integerValue;
            this.integerPartTernaryValue = ConvertIntToBalancedTernaryString(this.integerValue, nIntegerTrits);
            this.typeCodeTernary = ConvertTypeCodeToTernaryString(typeCode, nTypeTrits);
            this.fullTernaryValue = typeCodeTernary + integerPartTernaryValue;
            base.fullTernaryString = fullTernaryValue;
        }

        public TernaryIntGeneric(string ternaryString)
        {
            this.integerPartTernaryValue = ternaryString;
            if (integerPartTernaryValue.Length < nIntegerTrits)
            {
                var len = integerPartTernaryValue.Length;
                for (int i = len; i < nIntegerTrits; i++)
                {
                    integerPartTernaryValue = "0" + integerPartTernaryValue;
                }
            }
            this.integerValue = ConvertBalancedTernaryStringToInt(this.integerPartTernaryValue);
            this.typeCodeTernary = ConvertTypeCodeToTernaryString(typeCode, nTypeTrits);
            this.fullTernaryValue = typeCodeTernary + integerPartTernaryValue;
            base.fullTernaryString = fullTernaryValue;
        }

        public static void SetUpTernaryImplementation(int totalNTrits, int nTypeTrits)
        {
            TernaryIntGeneric.TotalNTrits = totalNTrits;
            TernaryIntGeneric.NTypeTrits = nTypeTrits;
            TernaryIntGeneric.NIntegerTrits = totalNTrits - nTypeTrits;
        }

        public override void ChangeValue()
        {
            base.ChangeValue();
            ChangeValue(fullTernaryString);
        }

        public void ChangeValue(Int64 integerValue)
        {
            this.integerValue = integerValue;
            this.integerPartTernaryValue = ConvertIntToBalancedTernaryString(this.integerValue, nIntegerTrits);
            this.typeCodeTernary = ConvertTypeCodeToTernaryString(typeCode, nTypeTrits);
            this.fullTernaryValue = typeCodeTernary + integerPartTernaryValue;
            base.fullTernaryString = fullTernaryValue;
        }

        public void ChangeValue(string ternaryString)
        {
            this.integerPartTernaryValue = ternaryString;
            if (integerPartTernaryValue.Length < nIntegerTrits)
            {
                var len = integerPartTernaryValue.Length;
                for (int i = len; i < nIntegerTrits; i++)
                {
                    integerPartTernaryValue = "0" + integerPartTernaryValue;
                }
            }
            this.integerValue = ConvertBalancedTernaryStringToInt(this.integerPartTernaryValue);
            this.typeCodeTernary = ConvertTypeCodeToTernaryString(typeCode, nTypeTrits);
            this.fullTernaryValue = typeCodeTernary + integerPartTernaryValue;
            base.fullTernaryString = fullTernaryValue;
        }

        public override void Invert()
        {
            this.integerPartTernaryValue = InvertBalancedTernaryString(this.integerPartTernaryValue);
            this.integerValue = -this.integerValue;
            this.fullTernaryValue = typeCodeTernary + integerPartTernaryValue;
            base.fullTernaryString = fullTernaryValue;
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

        public static Int64 ConvertBalancedTernaryStringToInt(string ternaryString)
        {
            var workChars = ternaryString.ToCharArray();
            Array.Reverse(workChars);
            //var reversedTernString = workChars.ToString();
            Int64 sum = 0;
            double exponent = 0;
            foreach (char trit in workChars)
            {
                if (trit == '+')
                {
                    sum += (Int64)Math.Round(Math.Pow(3.0, exponent));
                }
                else if (trit == '-')
                {
                    sum -= (Int64)Math.Round(Math.Pow(3.0, exponent));
                }
                else if (trit == '0')
                {
                    //do nothing, this place is zero
                }
                else
                {
                    Console.WriteLine("ERROR: Invalid trit character in ternaryString passed to ConvertBalancedTernaryStringToInt");
                }
                exponent++;
            }
            return sum;
        }

        public static string ConvertIntToBalancedTernaryString(Int64 integerValue, int nTrits)
        {
            var workValue = integerValue;
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
            if (integerValue < 0)
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
                for (int i = len; i < nTrits; i++)
                {
                    workString = "0" + workString;
                }
            }

            return workString;
        }



        public static string ConvertTypeCodeToTernaryString(TypeCodeEnum? typeCode, int nTrits)
        {
            TypeCodeEnum workingType = TypeCodeEnum.Normal;
            if (typeCode != null)
            {
                workingType = (TypeCodeEnum)typeCode!;
            }

            var workValue = (Int64)workingType;
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

            if ((Int64)workingType < 0)
            {
                workString = InvertBalancedTernaryString(workString);
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
    }

}
