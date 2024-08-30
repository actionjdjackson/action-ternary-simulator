using System;
namespace ActionTernarySimulator
{
    public class TernaryCPU
    {

        private static int PROCESSOR_WIDTH_IN_TRITS = 27;
        private static int N_GEN_PUR_REGISTERS = 3;
        private static int N_ADDRESS_REGISTERS = 3;
        private static int N_DATA_REGISTERS = 3;
        private static int N_CONTROL_REGISTERS = 3;
        //private static int N_STACKS = 6;
        //private TernaryMemorySystem ternaryMemorySystem;
        //private TernaryIOSystem ternaryIOSystem;
        private string pc;
        private string ir;
        private string acc;
        private string[] gen = new string[N_GEN_PUR_REGISTERS];
        private string[] ar = new string[N_ADDRESS_REGISTERS];
        private string sp;
        private string[] dr = new string[N_DATA_REGISTERS];
        private string sr;
        private string[] cr = new string[N_CONTROL_REGISTERS];
        private string[] stack1 = new string[(int)(Math.Pow(3, PROCESSOR_WIDTH_IN_TRITS / 3) - 1) / 2];
        private string[] stack2 = new string[(int)(Math.Pow(3, PROCESSOR_WIDTH_IN_TRITS / 3) - 1) / 2];
        private string[] stack3 = new string[(int)(Math.Pow(3, PROCESSOR_WIDTH_IN_TRITS / 3) - 1) / 2];
        private string[] stack4 = new string[(int)(Math.Pow(3, PROCESSOR_WIDTH_IN_TRITS / 3) - 1) / 2];
        private string[] stack5 = new string[(int)(Math.Pow(3, PROCESSOR_WIDTH_IN_TRITS / 3) - 1) / 2];
        private string[] stack6 = new string[(int)(Math.Pow(3, PROCESSOR_WIDTH_IN_TRITS / 3) - 1) / 2];

        public TernaryCPU()//TernaryMemorySystem memory, TernaryIOSystem iosys)
        {
            string emptyRegister = "";
            for (int t = 0; t < PROCESSOR_WIDTH_IN_TRITS; t++)
            {
                emptyRegister += "0";
            }

            pc = emptyRegister;
            ir = emptyRegister;
            acc = emptyRegister;
            sp = emptyRegister;
            sr = emptyRegister;

            for (int n = 0; n < N_GEN_PUR_REGISTERS; n++)
            {
                gen[n] = emptyRegister;
            }
            for (int n = 0; n < N_ADDRESS_REGISTERS; n++)
            {
                ar[n] = emptyRegister;
            }
            for (int n = 0; n < N_DATA_REGISTERS; n++)
            {
                dr[n] = emptyRegister;
            }
            for (int n = 0; n < N_CONTROL_REGISTERS; n++)
            {
                cr[n] = emptyRegister;
            }
        }

        public static TernaryIntGeneric PowerTernaryInts(TernaryIntGeneric int1, TernaryIntGeneric int2)
        {
            var zero = new TernaryIntGeneric(0);
            var pow = new TernaryIntGeneric(1);
            if (ComparisonOperator(int2, zero) == '0')
            {
                return pow;
            }
            else if (ComparisonOperator(int2, zero) == '+')
            {
                for (int n = 0; n < int2.integerValue; n++)
                {
                    pow = MultiplyTernaryInts(pow, int1);
                }
                return pow;
            }
            else
            {
                return zero;
            }
        }

        public static TernaryIntGeneric DivideTernaryInts(TernaryIntGeneric dividend, TernaryIntGeneric divisor)
        {
            int fullLength = dividend.fullTernaryValue.Length;
            if (fullLength <= PROCESSOR_WIDTH_IN_TRITS)
            {
                if (ComparisonOperator(divisor, new TernaryIntGeneric(0)) == '0')
                {
                    Console.WriteLine("ATTEMPT TO DIVIDE BY ZERO!");
                    return null;
                }
                else if (ComparisonOperator(AbsoluteValue(dividend), AbsoluteValue(divisor)) == '+')
                {
                    var one = new TernaryIntGeneric(1);
                    var zero = new TernaryIntGeneric(0);
                    var divd = AbsoluteValue(dividend);
                    var divsr = AbsoluteValue(divisor);
                    var quot = new TernaryIntGeneric(0);
                    var rem = new TernaryIntGeneric(divd.integerPartTernaryValue);
                    var finalQuotient = new TernaryIntGeneric(0);

                    for (int i = 0; i < TernaryIntGeneric.NIntegerTrits; i++)
                    {
                        rem = SubtractTernaryInts(rem, divsr);
                        quot = AddTernaryInts(quot, one);
                        if (ComparisonOperator(rem, divsr) == '-')
                        {
                            finalQuotient = quot;
                            //Console.WriteLine($"Quotient: {quot.integerValue} Remainder: {rem.integerValue}");
                            break;
                        }
                    }

                    if (ComparisonOperator(dividend, zero) == '-' ^ ComparisonOperator(divisor, zero) == '-')
                    {
                        finalQuotient.Invert();
                    }

                    return finalQuotient;

                }
                else if (dividend.integerPartTernaryValue == divisor.integerPartTernaryValue)
                {
                    return new TernaryIntGeneric(1);
                }
                else
                {
                    return new TernaryIntGeneric(0);
                }
            }
            else
            {
                Console.WriteLine("TERNARY INTEGER TOO LONG FOR PROCESSOR WIDTH");
                return null;
            }
        }

        public static TernaryIntGeneric ModulusTernaryInts(TernaryIntGeneric dividend, TernaryIntGeneric divisor)
        {
            int fullLength = dividend.fullTernaryValue.Length;
            if (fullLength <= PROCESSOR_WIDTH_IN_TRITS)
            {
                if (ComparisonOperator(divisor, new TernaryIntGeneric(0)) == '0')
                {
                    Console.WriteLine("ATTEMPT TO DIVIDE BY ZERO!");
                    return null;
                }
                else if (ComparisonOperator(AbsoluteValue(dividend), AbsoluteValue(divisor)) == '+')
                {
                    var one = new TernaryIntGeneric(1);
                    var zero = new TernaryIntGeneric(0);
                    var divd = AbsoluteValue(dividend);
                    var divsr = AbsoluteValue(divisor);
                    var quot = new TernaryIntGeneric(0);
                    var rem = new TernaryIntGeneric(divd.integerPartTernaryValue);
                    var finalModulus = new TernaryIntGeneric(0);

                    for (int i = 0; i < TernaryIntGeneric.NIntegerTrits; i++)
                    {
                        rem = SubtractTernaryInts(rem, divsr);
                        quot = AddTernaryInts(quot, one);
                        if (ComparisonOperator(rem, divsr) == '-')
                        {
                            finalModulus = rem;
                            //Console.WriteLine($"Quotient: {quot.integerValue} Remainder: {rem.integerValue}");
                            break;
                        }
                    }

                    if ((ComparisonOperator(dividend, zero) == '-' && ComparisonOperator(divisor, zero) == '-') || ComparisonOperator(divisor, zero) == '-')
                    {
                        finalModulus.Invert();
                    }

                    return finalModulus;

                }
                else if (dividend.integerPartTernaryValue == divisor.integerPartTernaryValue)
                {
                    return new TernaryIntGeneric(1);
                }
                else
                {
                    return new TernaryIntGeneric(0);
                }
            }
            else
            {
                Console.WriteLine("TERNARY INTEGER TOO LONG FOR PROCESSOR WIDTH");
                return null;
            }
        }

        public static TernaryIntGeneric AbsoluteValue(TernaryIntGeneric int1)
        {
            var arr1 = int1.integerPartTernaryValue.ToCharArray();
            for (int i = 0; i < arr1.Length; i++)
            {
                if (arr1[i] == '-')
                {
                    return new TernaryIntGeneric(TernaryIntGeneric.InvertBalancedTernaryString(int1.integerPartTernaryValue));
                }
                else if (arr1[i] == '+')
                {
                    return int1;
                }
            }
            return int1;
        }

        public static char ComparisonOperator(TernaryIntGeneric int1, TernaryIntGeneric int2)
        {
            var arr1 = int1.integerPartTernaryValue.ToCharArray();
            var arr2 = int2.integerPartTernaryValue.ToCharArray();

            for (int i = 0; i < arr1.Length; i++)
            {
                if (arr1[i] != arr2[i])
                {
                    if (arr1[i] == '+')
                    {
                        return '+';
                    }
                    else if (arr2[i] == '+')
                    {
                        return '-';
                    }
                    else if (arr1[i] == '-')
                    {
                        return '-';
                    }
                    else if (arr2[i] == '-')
                    {
                        return '+';
                    }
                }
            }
            return '0';
        }

        public static TernaryIntGeneric MultiplyTernaryInts(TernaryIntGeneric int1, TernaryIntGeneric int2)
        {
            int fullLength = int1.fullTernaryValue.Length;
            if (fullLength <= PROCESSOR_WIDTH_IN_TRITS)
            {
                var temp1 = int1.integerPartTernaryValue.ToCharArray();
                Array.Reverse(temp1);
                var temp2 = int2.integerPartTernaryValue;
                var product = new TernaryIntGeneric(0);
                for (int i = 0; i < temp1.Length; i++)
                {
                    var c = temp1[i];
                    var temp3 = new TernaryIntGeneric(0);
                    if (c == '-')
                    {
                        temp3 = new TernaryIntGeneric(TernaryIntGeneric.InvertBalancedTernaryString(temp2));
                    }
                    else if (c == '+')
                    {
                        temp3 = new TernaryIntGeneric(temp2);
                    }
                    temp3 = TritShiftLeft(temp3, i);
                    product = AddTernaryInts(product, temp3);
                }
                return product;
            }
            else
            {
                Console.WriteLine("TERNARY INTEGER TOO LONG FOR PROCESSOR WIDTH");
                return null;
            }
        }

        public static TernaryIntGeneric TritShiftRight(TernaryIntGeneric ternaryInt, int nTrits)
        {
            return new TernaryIntGeneric(TritShiftRight(ternaryInt.integerPartTernaryValue, nTrits));
        }

        public static string TritShiftRight(string ternaryValue, int nTrits)
        {
            var temp = ternaryValue.ToCharArray();
            var temp2 = new char?[temp.Length];
            for (int i = 0; i < temp2.Length; i++)
            {
                temp2[i] = temp[i];
            }
            var temp3 = new char?[temp.Length];
            var temp4 = new char[temp.Length];
            Array.Copy(temp2, 0, temp3, nTrits, temp.Length - nTrits);
            for (int i = 0; i < temp3.Length; i++)
            {
                if (temp3[i] == null)
                {
                    temp4[i] = '0';
                }
                else
                {
                    temp4[i] = temp3[i].Value;
                }
            }
            return new string(temp4);
        }

        public static TernaryIntGeneric TritShiftLeft(TernaryIntGeneric ternaryInt, int nTrits)
        {
            return new TernaryIntGeneric(TritShiftLeft(ternaryInt.integerPartTernaryValue, nTrits));
        }

        public static string TritShiftLeft(string ternaryValue, int nTrits)
        {
            var temp = ternaryValue.ToCharArray();
            var temp2 = new char?[temp.Length];
            for (int i = 0; i < temp.Length; i++)
            {
                temp2[i] = temp[i];
            }
            var temp3 = new char?[temp.Length];
            var temp4 = new char[temp.Length];
            Array.Copy(temp2, nTrits, temp3, 0, temp.Length - nTrits);
            for (int i = 0; i < temp.Length; i++)
            {
                if (temp3[i] == null)
                {
                    temp4[i] = '0';
                }
                else
                {
                    temp4[i] = temp3[i].Value;
                }
            }
            return new string(temp4);
        }
        

        public static TernaryIntGeneric SubtractTernaryInts(TernaryIntGeneric int1, TernaryIntGeneric int2)
        {
            var sub = new TernaryIntGeneric(TernaryIntGeneric.InvertBalancedTernaryString(int2.integerPartTernaryValue));
            return AddTernaryInts(int1, sub);
        }

        public static TernaryIntGeneric AddTernaryInts(TernaryIntGeneric int1, TernaryIntGeneric int2)
        {
            var temp1 = int1.integerPartTernaryValue.ToCharArray();
            var temp2 = int2.integerPartTernaryValue.ToCharArray();
            char[] temp3 = CreateEmptyTernaryValue(int1.integerPartTernaryValue.Length).ToCharArray();
            int fullLength = int1.fullTernaryValue.Length;
            if (fullLength <= PROCESSOR_WIDTH_IN_TRITS)
            {
                int carry = 0;
                for (int i = temp1.Length - 1; i >= 0; i--)
                {
                    int n1 = ConvertBalancedTritToNumber(temp1[i]);
                    int n2 = ConvertBalancedTritToNumber(temp2[i]);
                    int n3 = n1 + n2 + carry;
                    int sum;
                    if (n3 == 2)
                    {
                        carry = 1;
                        sum = -1;
                    }
                    else if (n3 == -2)
                    {
                        carry = -1;
                        sum = 1;
                    }
                    else if (n3 == 3)
                    {
                        carry = 1;
                        sum = 1;
                    }
                    else if (n3 == -3)
                    {
                        carry = -1;
                        sum = -1;
                    }
                    else
                    {
                        carry = 0;
                        sum = n3;
                    }
                    temp3[i] = ConvertNumberToBalancedTrit(sum);
                    if (i == 0 && carry == -1)
                    {
                        Console.WriteLine("Integer Add Underflow!");
                    }
                    else if (i == 0 && carry == 1)
                    {
                        Console.WriteLine("Integer Add Overflow!");
                    }
                }
                return new TernaryIntGeneric(new string(temp3));
            }
            else
            {
                Console.WriteLine("TERNARY INTEGER TOO LONG FOR PROCESSOR WIDTH");
                return null;
            }
        }

        public static string CreateEmptyTernaryValue(int nTrits)
        {
            string workStr = "";
            for (int i = 0; i < nTrits; i++)
            {
                workStr += "0";
            }
            return workStr;
        }

        public static char ConvertNumberToBalancedTrit(int num)
        {
            if (num == 1)
            {
                return '+';
            }
            else if (num == -1)
            {
                return '-';
            }
            else if (num == 0)
            {
                return '0';
            }
            else
            {
                return '0';
            }
        }

        public static int ConvertBalancedTritToNumber(char trit)
        {
            if (trit == '+')
            {
                return 1;
            }
            else if (trit == '-')
            {
                return -1;
            }
            else if (trit == '0')
            {
                return 0;
            }
            else
            {
                return 0;
            }
        }

    }
}
