using System;

namespace ActionTernarySimulator
{
    class Program
    {
        static void Main(string[] args)
        {
            int nTrits;
            double precision;
            int orderOfMag;
            int nFlags;
            bool extendedSig;
            int nSigTrits;
            int nExponentTrits;
            int nFlagTrits;


            Console.WriteLine("Welome to the Action Ternary Simulator. Enter (X)-Trit Float Solver/Tester or (T)est Ternary Simulator");
            var mainInput = Console.ReadLine();
            if (mainInput == "t" || mainInput == "T")
            {

                TernaryIntGeneric.SetUpTernaryImplementation(TernaryCPU.PROCESSOR_WIDTH_IN_TRITS, 0);
                TernaryChar.SetUpTernaryImplementation(TernaryCPU.PROCESSOR_WIDTH_IN_TRITS);
                TernaryFloatGeneric.SetUpTernaryImplementation(19, 5, true, 2);

                TernaryAssembly assembly = new TernaryAssembly();

                var memsys = new TernaryMemorySystem("/Users/jacobjackson/Documents/Code/Ternary/ActionTernarySimulator/ActionTernarySimulator/test.trom");
                var iosys = new TernaryIOSystem();
                var cpu = new TernaryCPU(memsys, iosys);

                var status = new TernaryIntGeneric(0);
                var startTime = DateTimeOffset.Now.ToUnixTimeMilliseconds();
                var clcnt = 1;
                var targspeed = 6.5; //approx. speed in KHz
                var targtime = 1 / (targspeed);

                while (status.integerValue == 0)
                {
                    status = cpu.Cycle();
                    clcnt++;
                    var xtime = (startTime + (clcnt - 1) * targtime) - DateTimeOffset.Now.ToUnixTimeMilliseconds();
                    if (xtime > 0.0)
                    {
                        System.Threading.Thread.Sleep((int)xtime);
                    }
                }
                Console.WriteLine("TernaryCPU Stopped With Status Code: " + status.integerValue);

                Console.WriteLine($"Approx. Speed: {(float)(clcnt) / (DateTimeOffset.Now.ToUnixTimeMilliseconds() - startTime)}" + " KHz");

                //Console.WriteLine(TernaryCPU.AddTernaryFloats(new TernaryFloatGeneric(-0.75), new TernaryFloatGeneric(-0.25)).DoubleValue);
                //Console.WriteLine(TernaryCPU.AddTernaryFloats(new TernaryFloatGeneric(3.75), new TernaryFloatGeneric(1.25)).DoubleValue);
                //Console.WriteLine(TernaryCPU.AddTernaryFloats(new TernaryFloatGeneric(50), new TernaryFloatGeneric(20)).DoubleValue);
                //Console.WriteLine(TernaryCPU.AddTernaryFloats(new TernaryFloatGeneric(0.126), new TernaryFloatGeneric(0.125)).DoubleValue);
                //Console.WriteLine(TernaryCPU.AddTernaryFloats(new TernaryFloatGeneric(200), new TernaryFloatGeneric(-190)).DoubleValue);
                //Console.WriteLine(TernaryCPU.AddTernaryFloats(new TernaryFloatGeneric(200), new TernaryFloatGeneric(-180)).DoubleValue);
                //Console.WriteLine(TernaryCPU.AddTernaryFloats(new TernaryFloatGeneric(200), new TernaryFloatGeneric(-170)).DoubleValue);
                //Console.WriteLine(TernaryCPU.AddTernaryFloats(new TernaryFloatGeneric(-180), new TernaryFloatGeneric(200)).DoubleValue);
                //Console.WriteLine(TernaryCPU.AddTernaryFloats(new TernaryFloatGeneric(200), new TernaryFloatGeneric(-200.125)).DoubleValue);
                //Console.WriteLine(TernaryCPU.AddTernaryFloats(new TernaryFloatGeneric(3.14159), new TernaryFloatGeneric(2.7)).DoubleValue);
                //Console.WriteLine(TernaryCPU.AddTernaryFloats(new TernaryFloatGeneric(2165.3734), new TernaryFloatGeneric(10943.1334)).DoubleValue);
            }
            else if (mainInput == "X" || mainInput == "x")
            {
                while (true)
                {
                    Console.WriteLine("Do you want to (s)olve for a particular precision/magnitude/flags or (c)reate your own standard? Or (q)uit?");
                    var input = Console.ReadLine();
                    double value;
                    if (input == "c")
                    {
                        Console.WriteLine("Creating your own standard.");
                    Significand:
                        Console.WriteLine("How many trits for precision (the significand)? ");
                        input = Console.ReadLine();
                        if (double.TryParse(input, out value) && value > 0)
                        {
                            nSigTrits = (int)value;
                        }
                        else
                        {
                            Console.WriteLine("Please enter a positive number.");
                            goto Significand;
                        }
                    Exponent:
                        Console.WriteLine("How many trits for magnitude (the exponent)? ");
                        input = Console.ReadLine();
                        if (double.TryParse(input, out value) && value > 0)
                        {
                            nExponentTrits = (int)value;
                        }
                        else
                        {
                            Console.WriteLine("Please enter a positive number.");
                            goto Exponent;
                        }
                        Console.WriteLine("Do you want an extended signficand for subnormal numbers?\nThis will also increase your normal significand by one trit.\n(y or n, defaults to y)? ");
                        input = Console.ReadLine();
                        switch (input)
                        {
                            case "y":
                            case "Y":
                                extendedSig = true;
                                break;
                            case "n":
                            case "N":
                                extendedSig = false;
                                break;
                            default:
                                extendedSig = true;
                                break;
                        }
                    FlagTrits:
                        Console.WriteLine("How many trits for the flags/type codes? ");
                        input = Console.ReadLine();
                        if (double.TryParse(input, out value) && value >= 0)
                        {
                            nFlagTrits = (int)value;
                        }
                        else
                        {
                            Console.WriteLine("Please enter zero or a positive number.");
                            goto FlagTrits;
                        }
                        int totalNTrits = nSigTrits + nExponentTrits + nFlagTrits;
                        if (extendedSig)
                        {
                            totalNTrits += 1;
                        }
                        Console.WriteLine($"Your implementation has a total of {totalNTrits} trits.\n");

                        TernaryFloatGeneric.SetUpTernaryImplementation(nSigTrits, nExponentTrits, extendedSig, nFlagTrits);

                        Console.WriteLine($"Done setting up TernaryFloatGeneric with {totalNTrits} trits.");
                        _ = StartTests(ref value, totalNTrits);

                        //var terFloatB = new TernaryFloatGeneric("00++++++------------------------------");
                    }
                    else if (input == "s")
                    {
                        Console.WriteLine("Starting up the solver...");
                    NTrits:
                        Console.WriteLine("How many trits are you aiming for? Please give your upper bound value: ");
                        input = Console.ReadLine();
                        if (double.TryParse(input, out value) && value >= 6)
                        {
                            nTrits = (int)value;
                        }
                        else
                        {
                            Console.WriteLine("Sorry, that is not a valid response. Please enter a number 6 or larger.");
                            goto NTrits;
                        }
                    Precision:
                        Console.WriteLine("What level of precision are you looking for? Try 1E-10 for example: ");
                        input = Console.ReadLine();
                        if (double.TryParse(input, out value) && value < 1.0 && value > 0)
                        {
                            precision = value;
                        }
                        else
                        {
                            Console.WriteLine("Sorry, that is not a valid response. Please enter a number less than 1 and greater than zero.");
                            goto Precision;
                        }
                    Magnitude:
                        Console.WriteLine("What largest order of magnitude are you looking for? As in, what power of 10? ");
                        input = Console.ReadLine();
                        if (double.TryParse(input, out value) && value > 0)
                        {
                            orderOfMag = (int)Math.Round(value);
                        }
                        else
                        {
                            Console.WriteLine("Sorry, that is not a valid response. Please enter a number between 1 and, well, something reasonable ;) ");
                            goto Magnitude;
                        }
                    Flags:
                        Console.WriteLine("How many type code types / flag states are you looking for? ");
                        input = Console.ReadLine();
                        if (double.TryParse(input, out value) && value >= 0)
                        {
                            nFlags = (int)value;
                        }
                        else
                        {
                            Console.WriteLine("Sorry, that is not a valid response. Please enter a number greater than or equal to zero. ");
                            goto Flags;
                        }
                        Console.WriteLine("Do you want an extended significand for subnormal numbers? y or n. Defaults to yes. ");
                        input = Console.ReadLine();
                        switch (input)
                        {
                            case "y":
                            case "Y":
                            case "yes":
                                extendedSig = true;
                                break;
                            case "n":
                            case "N":
                            case "no":
                                extendedSig = false;
                                break;
                            default:
                                extendedSig = true;
                                break;
                        }
                        TernaryFloatGeneric.SetUpTernaryImplementation(nTrits, precision, orderOfMag, nFlags, extendedSig);
                        input = StartTests(ref value, nTrits);
                        if (input == "s" || input == "S")
                        {
                            continue;
                        }
                    }
                    else if (input == "q" || input == "Q")
                    {
                        break;
                    }
                }
            }
        }

        private static void TestSuite()
        {
            int maxExp = ((int)Math.Pow(3.0, TernaryFloatGeneric.nExponentTrits) - 1) / 2;
            int minExp = -maxExp;

            Console.WriteLine("MIN AND MAX VALUES WITH EXPONENT OF 0\n");
            var terFloatC = new TernaryFloatGeneric(0.5);
            var terFloatD = new TernaryFloatGeneric(1.5);

            Console.WriteLine("MIN AND MAX VALUES WITH EXPONENT OF 13 (max for 3-trit exponent)\n");
            var terFloatR = new TernaryFloatGeneric(7.971614999999998e5);
            var terFloatS = new TernaryFloatGeneric(2.3914845e6);

            Console.WriteLine("MIN AND MAX VALUES WITH EXPONENT OF -13 (min for 3-trit exponent)\n");
            var terFloatT = new TernaryFloatGeneric(3.1361273719315336E-7);
            var terFloatU = new TernaryFloatGeneric(9.408382115794604e-7);

            Console.WriteLine("MIN AND MAX VALUES WITH EXPONENT OF 40 (max for 4-trit exponent)\n");
            var terFloatN = new TernaryFloatGeneric(6.07883272953e18);
            var terFloatO = new TernaryFloatGeneric(1.82364981886e19);

            Console.WriteLine("MIN AND MAX VALUES WITH EXPONENT OF -40 (min for 4-trit exponent)\n");
            var terFloatP = new TernaryFloatGeneric(4.11263166999e-20);
            var terFloatQ = new TernaryFloatGeneric(1.23378950098e-19);

            Console.WriteLine("MIN AND MAX VALUES WITH EXPONENT OF 121 (max for 5-trit exponent)\n");
            var terFloatF = new TernaryFloatGeneric(2.695515450e57);
            var terFloatE = new TernaryFloatGeneric(8.0865463494e57);

            Console.WriteLine("MIN AND MAX VALUES WITH EXPONENT OF -121 (min for 5-trit exponent)\n");
            var terFLoatK = new TernaryFloatGeneric(9.274663961281E-59);
            var terFloatJ = new TernaryFloatGeneric(2.78239918835E-58);

            Console.WriteLine("MIN AND MAX VALUES WITH EXPONENT OF 364 (max for 6-trit exponent)\n");
            var terFloatH = new TernaryFloatGeneric(2.350210283845e173);
            var terFLoatI = new TernaryFloatGeneric(7.0506308516907e173);

            Console.WriteLine("MIN AND MAX VALUES WITH EXPONENT OF -364 (min for 6-trit exponent)\n");
            var terFloatM = new TernaryFloatGeneric(1.0637346015927e-174);
            var terFloatL = new TernaryFloatGeneric(3.191203804776e-174);

            if (TernaryFloatGeneric.signTrit)
            {
                Console.WriteLine("MINIMUM VALUE OF SUBNORMAL NUMBER (with extended significand)\n");
                var terFloatG = new TernaryFloatGeneric((1.0 / Math.Pow(3.0, TernaryFloatGeneric.nSignificandTrits + TernaryFloatGeneric.nExponentTrits)) * Math.Pow(3.0, minExp));
            }
        }

        private static void RandomTest()
        {
            int maxExp = ((int)Math.Pow(3.0, TernaryFloatGeneric.nExponentTrits) - 1) / 2;
            int minExp = -maxExp;
            for (int i = 0; i < 1000; i++)
            {
                var r = new Random();
                var rd = r.NextDouble();
                rd = TernaryFloatGeneric.Normalize(rd, 0.0, 1.0, 0.5, 1.5);
                var s = r.Next(0, 1);
                if (s == 0)
                {
                    rd = -rd;
                }
                var re = r.Next(minExp, maxExp);
                _ = new TernaryFloatGeneric(rd * Math.Pow(3, re));
            }
        }

        private static string EnterTernaryString(int totalNTrits)
        {
            string input;
            Console.WriteLine($"Enter a {totalNTrits} trit Ternary Value: ");
            input = Console.ReadLine();
            var terFloatA = new TernaryFloatGeneric(input);
            return input;
        }

        private static void EnterADouble(out string input, out double value)
        {
            Console.WriteLine("Enter a floating point number: ");
            input = Console.ReadLine();
            if (double.TryParse(input, out value))
            {
                var terFloatA = new TernaryFloatGeneric(value);
            }
        }

        public static string StartTests(ref double value, int totalNTrits)
        {
            string input;
            Console.WriteLine("What sorts of tests would you like to run?");
            Console.WriteLine("(M)ax/Min Tests, (R)andom Numbers, Enter (T)ernary String, or Enter (D)ouble (or (S)tart Over): ");
            input = Console.ReadLine();
            if (input == "R" || input == "r")
            {
                RandomTest();
            }
            else if (input == "M" || input == "m")
            {
                TestSuite();
                StartTests(ref value, totalNTrits);

            }
            else if (input == "T" || input == "t")
            {
                _ = EnterTernaryString(totalNTrits);
                StartTests(ref value, totalNTrits);
            }
            else if (input == "D" || input == "d")
            {
                EnterADouble(out input, out value);
                StartTests(ref value, totalNTrits);
            }
            else if (input == "S" || input == "s")
            {
                Console.WriteLine("Starting over...");
            }

            return input;
        }
    }

}
