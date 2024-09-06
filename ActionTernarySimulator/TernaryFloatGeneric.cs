using System;

namespace ActionTernarySimulator
{
    public class TernaryFloatGeneric : TernaryNumGeneric
    {

        // These are the instance variables - which determine the behavior and values of each
        // part - significand, exponent, sign, type, and putting them all togethr into
        // 
        private string significand = "";
        private string extendedSignificand = "";
        private string exponent = "";
        private string signSubnormal = "";
        private string type = "";
        private string fullTernaryValue = "";
        private double doubleValue = 0.0;

        // These are the class variables, which determine how to split up x-number of trits
        // into the parts - significand, exponent, signTrit (true/false at this point), and type code
        public static int nSignificandTrits = 0;
        public static int nExponentTrits = 0;
        public static bool signTrit = false;
        public static int nTypeTrits = 0;
        public static int totalNTrits = 0;

        public static int maxExponentValue;
        public static int minExponentValue;
        public static double minNormalPosValue;
        public static double maxNormalPosValue;
        public static double minSubnormalPosValue;

        public string Significand { get => significand; set => ChangeValue(value, exponent, signSubnormal, type); }
        public string Exponent { get => exponent; set => ChangeValue(significand, value, signSubnormal, type); }
        public string SignSubnormal { get => signSubnormal; set => ChangeValue(significand, exponent, value, type); }
        public string Type { get => type; set => ChangeValue(significand, exponent, signSubnormal, value); }
        public string FullTernaryValue { get => fullTernaryValue; set => ChangeValue(value); }
        public double DoubleValue { get => doubleValue; set => ChangeValue(value); }

        //
        public enum TypeCodeEnum
        {
            Infinity = 4,
            QuietNaN = 3,
            SingalingNaN = 2,
            RealNumberRoundedDown = 1,
            RealNumberInfinitePrecision = 0,
            RealNumberRoundedUp = -1,
            UnusedType1 = -2,
            UnusedType2 = -3,
            UnusedType3 = -4
        }

        public TernaryFloatGeneric(string fullTernaryValue)
        {
            this.fullTernaryValue = fullTernaryValue;
            base.fullTernaryString = fullTernaryValue;
            this.type = fullTernaryValue.Substring(0, nTypeTrits);
            if (signTrit) {
                this.signSubnormal = fullTernaryValue.Substring(nTypeTrits, 1);
            }
            this.exponent = fullTernaryValue.Substring(nTypeTrits + signSubnormal.Length, nExponentTrits);
            this.significand = fullTernaryValue.Substring(nTypeTrits + signSubnormal.Length + nExponentTrits, nSignificandTrits);

            this.doubleValue = TernaryFloatGeneric.ConvertFullFeaturedBTSToDouble(fullTernaryValue);

            //Console.WriteLine(DoubleValue);
        }

        public void ChangeValue(string fullTernaryValue)
        {
            this.fullTernaryValue = fullTernaryValue;
            base.fullTernaryString = fullTernaryValue;
            this.type = fullTernaryValue.Substring(0, nTypeTrits);
            if (signTrit)
            {
                this.signSubnormal = fullTernaryValue.Substring(nTypeTrits, 1);
            }
            this.exponent = fullTernaryValue.Substring(nTypeTrits + signSubnormal.Length, nExponentTrits);
            this.significand = fullTernaryValue.Substring(nTypeTrits + signSubnormal.Length + nExponentTrits, nSignificandTrits);

            this.doubleValue = TernaryFloatGeneric.ConvertFullFeaturedBTSToDouble(fullTernaryValue);
        }

        public override void ChangeValue()
        {
            base.ChangeValue();
            ChangeValue(fullTernaryString);
        }

        public static double Normalize(double val, double valmin, double valmax, double min, double max)
        {
            return (((val - valmin) / (valmax - valmin)) * (max - min)) + min;
        }

        public void ChangeValue(double doubleValue)
        {
            if (Math.Abs(doubleValue) >= minNormalPosValue && Math.Abs(doubleValue) <= maxNormalPosValue)
            {
                //Console.WriteLine("Double Passed Into TernaryFloatGeneric: " + doubleValue);

                int exponentValueBase3 = (int)Math.Ceiling((Math.Log(Math.Abs(doubleValue)) - Math.Log(1.5)) / Math.Log(3.0));

                this.exponent = TernaryIntGeneric.ConvertIntToBalancedTernaryString(integerValue: exponentValueBase3, nExponentTrits);

                double newSignificand = doubleValue / Math.Pow(3.0, exponentValueBase3);

                (string balTerStr, double remainder) = TernaryFloatGeneric.ConvertDoubleToBTSWithRemainder(newSignificand, TernaryFloatGeneric.nSignificandTrits);
                this.significand = balTerStr;

                //Console.WriteLine(remainder + " remainder after converting from Double to Balanced Ternary");

                SetUpTypeTrits();

                if (signTrit)
                {
                    this.signSubnormal = this.significand.ToCharArray()[0].ToString();
                    this.significand = this.significand.Remove(0, 1);
                }
                if (exponent.Length < nExponentTrits)
                {
                    for (int n = exponent.Length; n < nExponentTrits; n++)
                    {
                        exponent = "0" + exponent;
                    }
                }
                if (significand.Length < nSignificandTrits)
                {
                    //Console.WriteLine("Significand Smaller Than Expected");
                }

                this.fullTernaryValue = this.type + this.signSubnormal + this.exponent + this.significand;
                //Console.WriteLine("Balanced Ternary Value: " + fullTernaryValue);

                base.fullTernaryString = fullTernaryValue;

                this.doubleValue = TernaryFloatGeneric.ConvertFullFeaturedBTSToDouble(this.fullTernaryValue);
                //Console.WriteLine("Verify the double is the same converting back: " + this.doubleValue + "\n\n");
            }

            else if (Math.Abs(doubleValue) < minNormalPosValue && Math.Abs(doubleValue) >= minSubnormalPosValue && signTrit)

            {
                //Console.WriteLine("Double passed Into TernaryFloatGeneric: " + doubleValue);

                int exponentValueBase3 = (int)-((Math.Pow(3.0, nExponentTrits) - 1) / 2);

                //Console.WriteLine("Value is subnormal, changing to extended significand. Power of 3 remains at " + exponentValueBase3);

                double newSignificand = doubleValue / Math.Pow(3.0, exponentValueBase3);

                (string balTerStr, double remainder) = TernaryFloatGeneric.ConvertDoubleToBTSWithRemainder(newSignificand, nSignificandTrits + nExponentTrits, startingTrit: 1);
                this.extendedSignificand = balTerStr;

                //Console.WriteLine(remainder + " remainder after converting from Double to Balanced Ternary");
                SetUpTypeTrits();
                if (signTrit)
                {
                    this.signSubnormal = "0";
                }
                this.fullTernaryValue = this.type + this.signSubnormal + this.extendedSignificand;
                //Console.WriteLine("Balanced Ternary Value: " + fullTernaryValue);

                base.fullTernaryString = fullTernaryValue;

                this.doubleValue = TernaryFloatGeneric.ConvertFullFeaturedBTSToDouble(this.fullTernaryValue);
                //Console.WriteLine("Verify the double is the same converting back: " + this.doubleValue + "\n\n");
            }
            else if (doubleValue == 0)
            {
                SetUpTypeTrits();

                this.signSubnormal = "0";

                this.fullTernaryValue = this.type + this.signSubnormal;
                for (int n = 0; n < nSignificandTrits + nExponentTrits; n++)
                {
                    this.fullTernaryValue += "0";
                }

                base.fullTernaryString = fullTernaryValue;
            }
            else
            {
                Console.WriteLine("Your value was out of range for this implementation.\n");
            }

            void SetUpTypeTrits()
            {
                if (nTypeTrits > 0)
                {
                    this.type = TernaryIntGeneric.ConvertTypeCodeToTernaryString((TernaryIntGeneric.TypeCodeEnum)TypeCodeEnum.RealNumberInfinitePrecision, nTypeTrits);
                }
                else
                {
                    this.type = "";
                }
            }
        }

        public TernaryFloatGeneric(double doubleValue)
        {

            if (Math.Abs(doubleValue) >= minNormalPosValue && Math.Abs(doubleValue) <= maxNormalPosValue)
            {
                //Console.WriteLine("Double Passed Into TernaryFloatGeneric: " + doubleValue);

                int exponentValueBase3 = (int)Math.Ceiling((Math.Log(Math.Abs(doubleValue)) - Math.Log(1.5)) / Math.Log(3.0));

                this.exponent = TernaryIntGeneric.ConvertIntToBalancedTernaryString(integerValue: exponentValueBase3, nExponentTrits);

                double newSignificand = doubleValue / Math.Pow(3.0, exponentValueBase3);

                (string balTerStr, double remainder) = TernaryFloatGeneric.ConvertDoubleToBTSWithRemainder(newSignificand, TernaryFloatGeneric.nSignificandTrits);
                this.significand = balTerStr;

                //Console.WriteLine(remainder + " remainder after converting from Double to Balanced Ternary");

                SetUpTypeTrits();

                if (signTrit)
                {
                    this.signSubnormal = this.significand.ToCharArray()[0].ToString();
                    this.significand = this.significand.Remove(0, 1);
                }

                if (significand.Length < nSignificandTrits)
                {
                    //Console.WriteLine("Significand Smaller Than Expected");
                }

                this.fullTernaryValue = this.type + this.signSubnormal + this.exponent + this.significand;
                //Console.WriteLine("Balanced Ternary Value: " + fullTernaryValue);

                base.fullTernaryString = fullTernaryValue;

                this.doubleValue = TernaryFloatGeneric.ConvertFullFeaturedBTSToDouble(this.fullTernaryValue);
                //Console.WriteLine("Verify the double is the same converting back: " + this.DoubleValue + "\n\n");
            }

            else if (Math.Abs(doubleValue) < minNormalPosValue && Math.Abs(doubleValue) >= minSubnormalPosValue && signTrit)

            {
                //Console.WriteLine("Double passed Into TernaryFloatGeneric: " + doubleValue);

                int exponentValueBase3 = (int)-((Math.Pow(3.0, nExponentTrits) - 1) / 2);

                //Console.WriteLine("Value is subnormal, changing to extended significand. Power of 3 remains at " + exponentValueBase3);

                double newSignificand = doubleValue / Math.Pow(3.0, exponentValueBase3);

                (string balTerStr, double remainder) = TernaryFloatGeneric.ConvertDoubleToBTSWithRemainder(newSignificand, nSignificandTrits + nExponentTrits, startingTrit: 1);
                this.extendedSignificand = balTerStr;

                //Console.WriteLine(remainder + " remainder after converting from Double to Balanced Ternary");
                SetUpTypeTrits();
                if (signTrit)
                {
                    this.signSubnormal = "0";
                }
                this.fullTernaryValue = this.type + this.signSubnormal + this.extendedSignificand;
                //Console.WriteLine("Balanced Ternary Value: " + fullTernaryValue);

                base.fullTernaryString = fullTernaryValue;

                this.doubleValue = TernaryFloatGeneric.ConvertFullFeaturedBTSToDouble(this.fullTernaryValue);
                //Console.WriteLine("Verify the double is the same converting back: " + this.doubleValue + "\n\n");
            }

            else if (doubleValue == 0)

            {
                SetUpTypeTrits();

                this.signSubnormal = "0";

                this.fullTernaryValue = this.type + this.signSubnormal;
                for (int n = 0; n < nSignificandTrits + nExponentTrits; n++)
                {
                    this.fullTernaryValue += "0";
                }

                base.fullTernaryString = fullTernaryValue;
            }
            else
            {
                Console.WriteLine("Your value was out of range for this implementation.\n");
            }

            void SetUpTypeTrits()
            {
                if (nTypeTrits > 0)
                {
                    this.type = TernaryIntGeneric.ConvertTypeCodeToTernaryString((TernaryIntGeneric.TypeCodeEnum)TypeCodeEnum.RealNumberInfinitePrecision, nTypeTrits);
                }
                else
                {
                    this.type = "";
                }
            }
        }

        public void ChangeValue(string significand, string exponent, string sign = null, string typeCode = null)
        {
            this.significand = significand;
            this.exponent = exponent;
            this.signSubnormal = sign;
            if (this.signSubnormal == null) { this.signSubnormal = ""; }
            this.type = typeCode;
            if (type == null) { this.Type = ""; }
            this.fullTernaryValue = this.type + this.signSubnormal + this.exponent + this.significand;

            base.fullTernaryString = fullTernaryValue;

            this.doubleValue = TernaryFloatGeneric.ConvertFullFeaturedBTSToDouble(this.fullTernaryValue);
        }

        public TernaryFloatGeneric(string significand, string exponent, string sign = null, TypeCodeEnum? typeCodeEnum = TypeCodeEnum.RealNumberInfinitePrecision)
        {
            this.significand = significand;
            this.exponent = exponent;
            this.signSubnormal = sign;
            if (this.signSubnormal == null) { this.signSubnormal = ""; }
            this.type = TernaryIntGeneric.ConvertTypeCodeToTernaryString((TernaryIntGeneric.TypeCodeEnum)typeCodeEnum, nTypeTrits);
            this.fullTernaryValue = this.type + this.signSubnormal + this.exponent + this.significand;

            base.fullTernaryString = fullTernaryValue;

            this.doubleValue = TernaryFloatGeneric.ConvertFullFeaturedBTSToDouble(this.fullTernaryValue);
        }

        public override void Invert()
        {
            base.Invert();
            if (signSubnormal == "0")
            {
                significand = TernaryIntGeneric.InvertBalancedTernaryString(significand);
                exponent = TernaryIntGeneric.InvertBalancedTernaryString(exponent);
            }
            else if (signSubnormal == "+" || signSubnormal == "-")
            {
                signSubnormal = TernaryIntGeneric.InvertBalancedTernaryString(SignSubnormal);
                significand = TernaryIntGeneric.InvertBalancedTernaryString(significand);
            }
        }

        public static double CalculateMaxNormalizedSignificand( int nTrits, int startTrit = 0 )
        {
            double sum = 0;
            for (int n = startTrit; n < nTrits; n++)
            {
                sum += Math.Pow(3.0, -n);
            }
            return sum;
        }

        public static double CalculateMinNormalizedSignificand( int nTrits, int startTrit = 1 )
        {
            double sum = 1;
            for (int n = startTrit; n < nTrits; n++)
            {
                sum -= Math.Pow(3.0, -n);
            }
            return sum;
        }

        public static void SetUpTernaryImplementation(int nSignificandTrits, int nExponentTrits, bool signTrit, int nTypeTrits)
        {
            TernaryFloatGeneric.nSignificandTrits = nSignificandTrits;
            TernaryFloatGeneric.nExponentTrits = nExponentTrits;
            TernaryFloatGeneric.signTrit = signTrit;
            TernaryFloatGeneric.nTypeTrits = nTypeTrits;
            TernaryFloatGeneric.totalNTrits = nSignificandTrits + nExponentTrits + nTypeTrits;
            if (signTrit) { TernaryFloatGeneric.totalNTrits++; }

            TernaryFloatGeneric.maxNormalPosValue = 1.5 * Math.Pow(3.0, (Math.Pow(3.0, nExponentTrits) - 1) / 2);
            TernaryFloatGeneric.minNormalPosValue = 0.5 * Math.Pow(3.0, -(Math.Pow(3.0, nExponentTrits) - 1) / 2);
            if (signTrit)
            {
                TernaryFloatGeneric.minSubnormalPosValue = Math.Pow(3.0, -(nSignificandTrits + nExponentTrits)) * Math.Pow(3.0, -(Math.Pow(3.0, nExponentTrits) - 1) / 2);
            }
            TernaryFloatGeneric.maxExponentValue = (int)(Math.Pow(3, TernaryFloatGeneric.nExponentTrits) - 1) / 2;
            TernaryFloatGeneric.minExponentValue = -maxExponentValue;
        }

        public static void SetUpTernaryImplementation(int totalNTrits, double desiredPrecision = 10e-10, int desiredOrderOfMagnitude = 10, int desiredNumberOfFlags = 9, bool extendedSignificand = true)
        {
            TernaryFloatGeneric.totalNTrits = totalNTrits;
            int workingSignificandSize = SolveForDesiredPrecision(desiredPrecision);
            int workingExponentSize = SolveForDesiredOrderOfMagnitude(desiredOrderOfMagnitude);
            int workingFlagsSize = SolveForDesiredNumberOfFlags(desiredNumberOfFlags);
            int workingSignSize = extendedSignificand ? 1 : 0;
            int workingTotalNTrits = workingSignificandSize + workingExponentSize + workingFlagsSize + workingSignSize;
            
            if (workingTotalNTrits == totalNTrits)
            {
                Console.WriteLine($"It's a perfect match! A significand of {workingSignificandSize} trits, an exponent of {workingExponentSize} trits, {((signTrit == true) ? "an extended significand for subnormal numbers" : "no extended significand")}, and a type code with {workingFlagsSize} trits fits your requirements!");
                Console.WriteLine("The Ternary Implementation you've created is ready for testing...");
                SetUpTernaryImplementation(workingSignificandSize, workingExponentSize, extendedSignificand, workingFlagsSize);
            }
            else if (workingTotalNTrits < totalNTrits)
            {
                var tritsLeft = totalNTrits - workingTotalNTrits;
                while (tritsLeft > 0)
                {
                    Console.WriteLine($"Looks like you still have some wiggle room. A significand of {workingSignificandSize} trits, an exponent of {workingExponentSize} trits, {((extendedSignificand == true) ? "an extended significand for subnormal numbers" : "no extended significand")}, and a type code with {workingFlagsSize} trits. You have {tritsLeft} trits left over. What do you want to increase? The (p)recision or the (m)agnitude or do you want to add more (f)lags? Otherwise, (k)eep it as is.");
                    var input = Console.ReadLine();
                    switch (input)
                    {
                        case "p":
                            {
                                int value;
                                Console.WriteLine("How many trits do you want to add to the precision (the significand)? ");
                                input = Console.ReadLine();
                                int.TryParse(input, out value);
                                if (value <= tritsLeft)
                                {
                                    workingSignificandSize += value;
                                    workingTotalNTrits += value;
                                    Console.WriteLine($"You have multiplied the granularity of your significand by {Math.Pow(3.0, value)}, and it now takes up {workingSignificandSize} trits");
                                }
                                else
                                {
                                    Console.WriteLine($"Ooops, you don't have enough free trits to do that!");
                                    continue;
                                }

                                break;
                            }

                        case "m":
                            {
                                int value;
                                Console.WriteLine("How many trits do you want to add to the magnitude (the exponent)? ");
                                input = Console.ReadLine();
                                int.TryParse(input, out value);
                                if (value <= tritsLeft)
                                {
                                    workingExponentSize += value;
                                    workingTotalNTrits += value;
                                    Console.WriteLine($"You have multiplied you exponent value by {Math.Pow(3.0, value)}, and it now takes up {workingExponentSize} trits");
                                }
                                else
                                {
                                    Console.WriteLine($"Ooops, you don't have enough free trits to do that!");
                                    continue;
                                }

                                break;
                            }

                        case "f":
                            {
                                int value;
                                Console.WriteLine("How many trits do you want to add to the type code (the flags)? ");
                                input = Console.ReadLine();
                                int.TryParse(input, out value);
                                if (value <= tritsLeft)
                                {
                                    workingFlagsSize += value;
                                    workingTotalNTrits += value;
                                    Console.WriteLine($"You now have {Math.Pow(3.0, workingFlagsSize)} possible flag values.");
                                }
                                else
                                {
                                    Console.WriteLine($"Ooops, you don't have enough free trits to do that!");
                                    continue;
                                }

                                break;
                            }
                        case "k":
                            {
                                Console.WriteLine("Keeping Current Setup...");
                                break;
                            }
                        default:
                            Console.WriteLine("Sorry, I didn't understand your response.");
                            continue;
                    }
                    tritsLeft = totalNTrits - workingTotalNTrits;
                    if (input == "k")
                    {
                        break;
                    }
                }

                SetUpTernaryImplementation(workingSignificandSize, workingExponentSize, extendedSignificand, workingFlagsSize);
                Console.WriteLine($"You now have a significand of {workingSignificandSize} trits, an exponent of {workingExponentSize} trits, {((extendedSignificand == true) ? "an extended significand for subnormal numbers" : "no extended significand")}, and a type code with {workingFlagsSize} trits.");
                Console.WriteLine("Ok, great, the Ternary Implementation you've created is ready for testing...");
                double dvalue = 0.0;
                Program.StartTests(ref dvalue, totalNTrits);

            }
            else
            {
                var input = "";
                var tritsDeficit = totalNTrits - workingTotalNTrits;
                Console.WriteLine($"I'm sorry, but you don't have enough tritspace available for those requirements. You have a deficit of {tritsDeficit} trits.");
                while (tritsDeficit < 0)
                {
                    Console.WriteLine($"What would you like to compromise on? (p)recision, (m)agnitude, or (f)lags?");
                    input = Console.ReadLine();
                    switch (input)
                    {
                        case "p":
                            {
                                int value;
                                Console.WriteLine("How many trits do you want to subtract from the precision (the significand)? ");
                                input = Console.ReadLine();
                                int.TryParse(input, out value);
                                if (value <= Math.Abs(tritsDeficit))
                                {
                                    workingSignificandSize -= value;
                                    workingTotalNTrits -= value;
                                    Console.WriteLine($"You have divided the granularity of your significand by {Math.Pow(3.0, value)}, and it now takes up {workingSignificandSize} trits");
                                }
                                else
                                {
                                    Console.WriteLine($"Ooops, you went too far, please enter a number between 1 and {Math.Abs(tritsDeficit)}");
                                    continue;
                                }
                                break;
                            }
                        case "m":
                            {
                                int value;
                                Console.WriteLine("How many trits do you want to subtract from the magnitude (the exponent)? ");
                                input = Console.ReadLine();
                                int.TryParse(input, out value);
                                if (value <= Math.Abs(tritsDeficit))
                                {
                                    workingExponentSize -= value;
                                    workingTotalNTrits -= value;
                                    Console.WriteLine($"You have divided the possible values of magnitude of the exponent by {Math.Pow(3.0, value)}, and it now takes up {workingExponentSize} trits");
                                }
                                else
                                {
                                    Console.WriteLine($"Ooops, you went too far, please enter a number between 1 and {Math.Abs(tritsDeficit)}");
                                    continue;
                                }
                                break;
                            }
                        case "f":
                            {
                                int value;
                                Console.WriteLine("How many trits do you want to subtract from the type code (the flags)? ");
                                input = Console.ReadLine();
                                int.TryParse(input, out value);
                                if (value <= Math.Abs(tritsDeficit))
                                {
                                    workingFlagsSize -= value;
                                    workingTotalNTrits -= value;
                                    Console.WriteLine($"You have divided the possible number of flags by {Math.Pow(3.0, value)}, and it now takes up {workingFlagsSize} trits");
                                }
                                else
                                {
                                    Console.WriteLine($"Ooops, you went too far, please enter a number between 1 and {Math.Abs(tritsDeficit)}");
                                    continue;
                                }
                                break;
                            }
                        default:
                            break;
                    }

                tritsDeficit = totalNTrits - workingTotalNTrits;

                }
                SetUpTernaryImplementation(workingSignificandSize, workingExponentSize, extendedSignificand, workingFlagsSize);
                Console.WriteLine($"You now have a significand of {workingSignificandSize} trits, an exponent of {workingExponentSize} trits, {((extendedSignificand == true) ? "an extended significand for subnormal numbers" : "no extended significand")}, and a type code with {workingFlagsSize} trits.");
                Console.WriteLine("Ok, great, the Ternary Implementation you've created is ready for testing...");
                double dvalue = 0.0;
                Program.StartTests(ref dvalue, totalNTrits);
            }
        }

        public static int SolveForDesiredPrecision(double precision)
        {
            return (int)Math.Round(Math.Log(1 / precision) / Math.Log(3));
        }

        public static int SolveForDesiredOrderOfMagnitude(int orderOfMagnitude)
        {
            return (int) Math.Ceiling(Math.Abs(Math.Pow((2 * Math.Log(10.0) * orderOfMagnitude) / Math.Log(10) + 1, 1.0 / 3.0)));
        }

        public static int SolveForDesiredNumberOfFlags(int nFlags)
        {
            if (nFlags > 0)
            {
                return (int)Math.Ceiling(Math.Log(nFlags) / Math.Log(3.0));
            }
            else
            {
                return 0;
            }
        }

        public static (string, double) ConvertDoubleToBTSWithRemainder(double doubleValue, int nTrits, int startingTrit = 0)
        {
            string balancedTernaryOutput = "";
            var sum = doubleValue;
            for (int index = startingTrit; index <= nTrits; index++)
            {
                if (index == 0 && doubleValue > 0)
                {
                    sum -= 1 * Math.Pow(3.0, -index);
                    balancedTernaryOutput += "+";
                }
                else if (index == 0 && doubleValue < 0)
                {
                    sum -= -1 * Math.Pow(3.0, -index);
                    balancedTernaryOutput += "-";
                }
                else if (doubleValue == 0)
                {
                    balancedTernaryOutput += "0";
                }
                else
                {
                    var magnitudeDiffNeg = Math.Abs(sum - (-1) * Math.Pow(3.0, -index));
                    var magnitudeDiffPos = Math.Abs(sum - (1) * Math.Pow(3.0, -index));
                    if ((magnitudeDiffNeg < magnitudeDiffPos) && magnitudeDiffNeg < Math.Abs(sum))
                    {
                        sum -= -1 * Math.Pow(3.0, -index);
                        balancedTernaryOutput += "-";
                    }
                    else if ((magnitudeDiffPos < magnitudeDiffNeg) && magnitudeDiffPos < Math.Abs(sum))
                    {
                        sum -= 1 * Math.Pow(3.0, -index);
                        balancedTernaryOutput += "+";
                    }
                    else
                    {
                        balancedTernaryOutput += "0";
                    }
                }

            }
            return (balancedTernaryOutput, sum);
        }

        public static double ConvertFullFeaturedBTSToDouble(string fullTernaryString)
        {
            string type = fullTernaryString.Substring(0, nTypeTrits);
            int typeValue = (int)TernaryIntGeneric.ConvertBalancedTernaryStringToInt(type);
            string signSubnormal = "";
            string exponent;
            string significand;
            int exponentValue;
            double significandValue;
            if (signTrit)
            {
                signSubnormal = fullTernaryString.Substring(nTypeTrits, 1);
            }
            if (signSubnormal == "0")
            {
                exponentValue = (int) -((Math.Pow(3.0, nExponentTrits) - 1) / 2);
                significand = fullTernaryString.Substring(nTypeTrits + signSubnormal.Length, nSignificandTrits + nExponentTrits);
                significandValue = TernaryFloatGeneric.ConvertBTSToDouble(significand, startingExp: -1);
            }
            else if (signSubnormal == "+" || signSubnormal == "-" )
            {
                exponent = fullTernaryString.Substring(nTypeTrits + signSubnormal.Length, nExponentTrits);
                significand = fullTernaryString.Substring(nTypeTrits + signSubnormal.Length + nExponentTrits, nSignificandTrits);
                exponentValue = (int)TernaryIntGeneric.ConvertBalancedTernaryStringToInt(exponent);
                significandValue = TernaryFloatGeneric.ConvertBTSToDouble(signSubnormal + significand);
            }
            else
            {
                exponent = fullTernaryString.Substring(nTypeTrits + signSubnormal.Length, nExponentTrits);
                significand = fullTernaryString.Substring(nTypeTrits + signSubnormal.Length + nExponentTrits, nSignificandTrits);
                exponentValue = (int)TernaryIntGeneric.ConvertBalancedTernaryStringToInt(exponent);
                significandValue = TernaryFloatGeneric.ConvertBTSToDouble(signSubnormal + significand);
            }
            if (nTypeTrits >= 2)
            {
                if ((TypeCodeEnum)typeValue == TypeCodeEnum.Infinity)
                {
                    if (signSubnormal == "+" || signSubnormal == "0" || significandValue > 0)
                    {
                        return Double.PositiveInfinity;
                    }
                    else if (signSubnormal == "-" || significandValue < 0)
                    {
                        return Double.NegativeInfinity;
                    }
                    else
                    {
                        return Double.PositiveInfinity;
                    }
                }
                else if ((TypeCodeEnum)typeValue == TypeCodeEnum.QuietNaN || (TypeCodeEnum)typeValue == TypeCodeEnum.SingalingNaN)
                {
                    if (signSubnormal == "+" || signSubnormal == "0" || significandValue > 0)
                    {
                        return Double.NaN;
                    }
                    else if (signSubnormal == "-" || significandValue < 0)
                    {
                        return -Double.NaN;
                    }
                    else
                    {
                        return Double.NaN;
                    }
                }
            }
            else if (nTypeTrits >= 1)
            {
                if ((TypeCodeEnum)typeValue == TypeCodeEnum.RealNumberInfinitePrecision)
                {
                    return significandValue * Math.Pow(3.0, exponentValue);
                }
                else if ((TypeCodeEnum)typeValue == TypeCodeEnum.RealNumberRoundedUp)
                {
                    return significandValue * Math.Pow(3.0, exponentValue);
                }
                else if ((TypeCodeEnum)typeValue == TypeCodeEnum.RealNumberRoundedDown)
                {
                    return significandValue * Math.Pow(3.0, exponentValue);
                }
                else
                {
                    return significandValue * Math.Pow(3.0, exponentValue);
                }
            }
            else
            {
                return significandValue * Math.Pow(3.0, exponentValue);
            }
            return significandValue * Math.Pow(3.0, exponentValue);
        }

        public static double ConvertBTSToDouble(string ternaryString, int startingExp = 0)
        {
            double exp = startingExp;
            double sum = 0;

            foreach (char trit in ternaryString)
            {
                sbyte tritValue = 0;
                switch (trit)
                {
                    case '+':
                        tritValue = 1;
                        break;
                    case '0':
                        tritValue = 0;
                        break;
                    case '-':
                        tritValue = -1;
                        break;
                    default:
                        Console.WriteLine("ERROR: Invalid character in ternaryString passed to TernaryFloatGeneric.convertBTSToDouble");
                        break;
                }
                sum += tritValue * Math.Pow(3.0, exp);
                exp -= 1;
            }

            return sum;
        }
    }

}
