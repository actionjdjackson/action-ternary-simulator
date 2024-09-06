using System;
using System.Collections.Generic;
using System.Linq;

namespace ActionTernarySimulator
{
    public class TernaryCPU
    {
        //private TernaryAssembly assembly = new TernaryAssembly();
        public static int PROCESSOR_WIDTH_IN_TRITS = 27;
        public static int MEMORY_BUS_WIDTH_IN_TRITS = 9;
        public static int IO_BUS_WIDTH_IN_TRITS = 9;
        private static int N_GEN_PUR_REGISTERS = 9;
        private static int N_ADDRESS_REGISTERS = 3;
        private static int N_DATA_REGISTERS = 3;
        private static int N_CONTROL_REGISTERS = 3;
        TernaryMemorySystem memsys;
        TernaryIOSystem iosys;
        //private static int N_STACKS = 6;
        //private TernaryMemorySystem ternaryMemorySystem;
        //private TernaryIOSystem ternaryIOSystem;
        private TernaryIntGeneric execpoint;
        private TernaryIntGeneric instreg;
        private TernaryIntGeneric acc;
        private TernaryFloatGeneric facc;
        private TernaryIntGeneric reg1;
        private TernaryIntGeneric reg2;
        private TernaryFloatGeneric freg1;
        private TernaryFloatGeneric freg2;
        private TernaryIntGeneric[] reg = new TernaryIntGeneric[N_GEN_PUR_REGISTERS];
        private TernaryIntGeneric[] ar = new TernaryIntGeneric[N_ADDRESS_REGISTERS];
        private TernaryIntGeneric sp;
        private TernaryNumGeneric datareg;
        private TernaryIntGeneric[] dr = new TernaryIntGeneric[N_DATA_REGISTERS];
        private TernaryIntGeneric sr;
        private TernaryIntGeneric[] cr = new TernaryIntGeneric[N_CONTROL_REGISTERS];
        private TernaryIntGeneric[] stack1 = new TernaryIntGeneric[MaximumPositiveIntegerMemory()];
        private TernaryIntGeneric[] stack2 = new TernaryIntGeneric[MaximumPositiveIntegerMemory()];
        private TernaryIntGeneric[] stack3 = new TernaryIntGeneric[MaximumPositiveIntegerMemory()];
        private TernaryIntGeneric[] stack4 = new TernaryIntGeneric[MaximumPositiveIntegerMemory()];
        private TernaryIntGeneric[] stack5 = new TernaryIntGeneric[MaximumPositiveIntegerMemory()];
        private TernaryIntGeneric[] stack6 = new TernaryIntGeneric[MaximumPositiveIntegerMemory()];

        public TernaryCPU(TernaryMemorySystem memsys, TernaryIOSystem iosys)
        {

            this.memsys = memsys;
            this.iosys = iosys;

            execpoint = new TernaryIntGeneric(MaximumNegativeIntegerMemory());
            instreg = new TernaryIntGeneric(0);
            acc = new TernaryIntGeneric(0);
            sp = new TernaryIntGeneric(0);
            sr = new TernaryIntGeneric(0);
            reg1 = new TernaryIntGeneric(0);
            reg2 = new TernaryIntGeneric(0);
            facc = new TernaryFloatGeneric(0);
            freg1 = new TernaryFloatGeneric(0);
            freg2 = new TernaryFloatGeneric(0);
            datareg = new TernaryIntGeneric(0);

            for (int n = 0; n < N_GEN_PUR_REGISTERS; n++)
            {
                reg[n] = new TernaryIntGeneric(0);
            }
            for (int n = 0; n < N_ADDRESS_REGISTERS; n++)
            {
                ar[n] = new TernaryIntGeneric(0);
            }
            for (int n = 0; n < N_DATA_REGISTERS; n++)
            {
                dr[n] = new TernaryIntGeneric(0);
            }
            for (int n = 0; n < N_CONTROL_REGISTERS; n++)
            {
                cr[n] = new TernaryIntGeneric(0);
            }

            //var running = new TernaryIntGeneric(0);

            //while (running.integerValue == 0)
            //{
            //    running = Cycle();
            //}

        }

        public static Int64 MaximumNegativeInteger()
        {
            return -(Int64)(Math.Pow(3.0, (double)TernaryCPU.PROCESSOR_WIDTH_IN_TRITS) - 1) / 2;
        }

        public static Int64 MaximumPositiveInteger()
        {
            return (Int64)(Math.Pow(3.0, (double)TernaryCPU.PROCESSOR_WIDTH_IN_TRITS) - 1) / 2;
        }

        public static Int64 MaximumNegativeIntegerMemory()
        {
            return -(Int64)(Math.Pow(3.0, (double)TernaryCPU.MEMORY_BUS_WIDTH_IN_TRITS) - 1) / 2;
        }

        public static Int64 MaximumPositiveIntegerMemory()
        {
            return (Int64)(Math.Pow(3.0, (double)TernaryCPU.MEMORY_BUS_WIDTH_IN_TRITS) - 1) / 2;
        }

        public static Int64 MaximumNegativeIntegerIO()
        {
            return -(Int64)(Math.Pow(3.0, (double)TernaryCPU.IO_BUS_WIDTH_IN_TRITS) - 1) / 2;
        }

        public static Int64 MaximumPositiveIntegerIO()
        {
            return (Int64)(Math.Pow(3.0, (double)TernaryCPU.IO_BUS_WIDTH_IN_TRITS) - 1) / 2;
        }

        private void Goto(TernaryIntGeneric address)
        {
            execpoint = new TernaryIntGeneric(address.integerValue);
        }

        public TernaryIntGeneric Cycle()
        {

            if (execpoint.integerValue > MaximumPositiveIntegerMemory())
            {
                Console.WriteLine("Execution Pointer Overrun");
                return new TernaryIntGeneric(-3);
            }

            Console.WriteLine("Execution Pointer: " + execpoint.FullTernaryString + " = " + execpoint.integerValue);

            instreg.ChangeValue(this.memsys.GetInst(execpoint).integerValue);
            if (this.memsys.GetData(execpoint) is TernaryIntGeneric)
            {
                datareg = new TernaryIntGeneric(this.memsys.GetData(execpoint).FullTernaryString);
            }
            else if (this.memsys.GetData(execpoint) is TernaryFloatGeneric)
            {
                datareg = new TernaryFloatGeneric(this.memsys.GetData(execpoint).FullTernaryString);
            }

            Console.WriteLine("Instruction Register: " + instreg.FullTernaryString + " = " + instreg.integerValue);
            if (datareg is TernaryFloatGeneric)
            {
                Console.WriteLine("Data Register: " + datareg.FullTernaryString + " = " + ((TernaryFloatGeneric)datareg).DoubleValue);
            }
            else if (datareg is TernaryIntGeneric)
            {
                Console.WriteLine("Data Register: " + datareg.FullTernaryString + " = " + ((TernaryIntGeneric)datareg).integerValue);
            }

            switch (instreg.integerValue)
            {
                case 0:
                    break;
                case -9841:
                    reg1.ChangeValue((datareg as TernaryIntGeneric).integerValue); //set register 1 to data register value
                    break;
                case -9840:
                    reg2.ChangeValue((datareg as TernaryIntGeneric).integerValue); //set register 2 to data register value
                    break;
                case -9839:
                    reg1.ChangeValue(reg2.integerValue);    //copy register 2 to register 1
                    break;
                case -9838:
                    reg2.ChangeValue(reg1.integerValue);    //copy register 1 to register 2
                    break;
                case -9837:
                    acc = new TernaryIntGeneric(reg1.integerValue); //swap register 1 and 2 using accumulator as an intermediary
                    reg1.ChangeValue(reg2.integerValue);
                    reg2.ChangeValue(acc.integerValue);
                    break;
                case -9836:
                    reg1.Invert();  //invert register 1
                    break;
                case -9835:
                    reg2.Invert();  //invert register 2
                    break;
                case -9834:
                    reg1.ChangeValue(TernaryCPU.AbsoluteValue(reg1).integerValue);  //absolute value of reg 1
                    break;
                case -9833:
                    reg2.ChangeValue(TernaryCPU.AbsoluteValue(reg2).integerValue); //absolute value of reg 2
                    break;
                case -9832:
                    reg1.ChangeValue(TernaryCPU.NegativeAbsoluteValue(reg1).integerValue);  //negative abs value of reg 1
                    break;
                case -9831:
                    reg2.ChangeValue(TernaryCPU.NegativeAbsoluteValue(reg2).integerValue);  //negative abs value of reg 2
                    break;

                case -9830: //tAND
                case -9829: //tOR
                case -9828: //tXNOR
                case -9827: //tCONS
                case -9826: //tSUM
                case -9825: //tEQUALITY
                case -9824: //tCROSS
                case -9823: //tNEGATE
                    break;

                case -9822:    //shift reg1 trits left by reg2 times
                    reg1.ChangeValue(TernaryCPU.TritShiftLeft(reg1, (int)reg2.integerValue).integerValue);
                    break;
                case -9821:   //shift reg1 trits right by reg2 times
                    reg1.ChangeValue(TernaryCPU.TritShiftRight(reg1, (int)reg2.integerValue).integerValue);
                    break;

                case -9820: //NOT
                case -9819: //EQUAL TO
                case -9818: //AND
                case -9817: //OR
                    break;
                case -9816: //COMPARE
                    acc.ChangeValue(new TernaryIntGeneric(TernaryCPU.ComparisonOperator(reg1, reg2).ToString()).integerValue);
                    break;
                case -9815: //NOT EQUAL TO
                    break;

                case -9000:
                    return new TernaryIntGeneric(-1);

                case -9800: //add reg1 and reg2 and place in reg 1
                    reg1.ChangeValue(TernaryCPU.AddTernaryInts(reg1, reg2).integerValue);
                    PointerRoll1();
                    break;
                case -9799: //add reg1 and reg 2 and place in reg 2
                    reg2.ChangeValue(TernaryCPU.AddTernaryInts(reg1, reg2).integerValue);
                    PointerRoll2();
                    break;
                case -9798:  //add reg 1 and data reg and place in reg 1
                    reg1.ChangeValue(TernaryCPU.AddTernaryInts((datareg as TernaryIntGeneric), reg1).integerValue);
                    PointerRoll1();
                    break;
                case -9797:  //add reg 2 and data reg and place in reg 2
                    reg2.ChangeValue(TernaryCPU.AddTernaryInts((datareg as TernaryIntGeneric), reg2).integerValue);
                    PointerRoll2();
                    break;
                case -9796: //sub instructions
                    reg1.ChangeValue(TernaryCPU.SubtractTernaryInts(reg1, reg2).integerValue);
                    PointerRoll1();
                    break;
                case -9795:
                    reg2.ChangeValue(TernaryCPU.SubtractTernaryInts(reg1, reg2).integerValue);
                    PointerRoll2();
                    break;
                case -9794:
                    reg1.ChangeValue(TernaryCPU.SubtractTernaryInts(reg1, (datareg as TernaryIntGeneric)).integerValue);
                    PointerRoll1();
                    break;
                case -9793:
                    reg2.ChangeValue(TernaryCPU.SubtractTernaryInts(reg2, (datareg as TernaryIntGeneric)).integerValue);
                    PointerRoll2();
                    break;
                case -9792: //mul instructions
                    reg1.ChangeValue(TernaryCPU.MultiplyTernaryInts(reg1, reg2).integerValue);
                    PointerRoll1();
                    break;
                case -9791:
                    reg2.ChangeValue(TernaryCPU.MultiplyTernaryInts(reg1, reg2).integerValue);
                    PointerRoll2();
                    break;
                case -9790:
                    reg1.ChangeValue(TernaryCPU.MultiplyTernaryInts(reg1, (datareg as TernaryIntGeneric)).integerValue);
                    PointerRoll1();
                    break;
                case -9789:
                    reg2.ChangeValue(TernaryCPU.MultiplyTernaryInts(reg2, (datareg as TernaryIntGeneric)).integerValue);
                    PointerRoll2();
                    break;
                case -9788: //div instructions
                    acc = TernaryCPU.DivideTernaryInts(reg1, reg2);
                    if (acc == null)
                    {
                        return new TernaryIntGeneric(-2);
                    }
                    else
                    {
                        reg1.ChangeValue(acc.integerValue);
                        PointerRoll1();
                    }
                    break;
                case -9787:
                    acc = TernaryCPU.DivideTernaryInts(reg1, reg2);
                    if (acc == null)
                    {
                        return new TernaryIntGeneric(-2);
                    }
                    else
                    {
                        reg2.ChangeValue(acc.integerValue);
                        PointerRoll2();
                    }
                    break;
                case -9786:
                    acc = TernaryCPU.DivideTernaryInts(reg1, (datareg as TernaryIntGeneric));
                    if (acc == null)
                    {
                        return new TernaryIntGeneric(-2);
                    }
                    else
                    {
                        reg1.ChangeValue(acc.integerValue);
                        PointerRoll1();
                    }
                    break;
                case -9785:
                    acc = TernaryCPU.DivideTernaryInts(reg2, (datareg as TernaryIntGeneric));
                    if (acc == null)
                    {
                        return new TernaryIntGeneric(-2);
                    }
                    else
                    {
                        reg2.ChangeValue(acc.integerValue);
                        PointerRoll2();
                    }
                    break;
                case -9784: //modulus
                    acc = TernaryCPU.ModulusTernaryInts(reg1, reg2);
                    if (acc == null)
                    {
                        return new TernaryIntGeneric(-2);
                    }
                    else
                    {
                        reg1.ChangeValue(acc.integerValue);
                        PointerRoll1();
                    }
                    break;
                case -9783:
                    acc = TernaryCPU.ModulusTernaryInts(reg1, reg2);
                    if (acc == null)
                    {
                        return new TernaryIntGeneric(-2);
                    }
                    else
                    {
                        reg2.ChangeValue(acc.integerValue);
                        PointerRoll2();
                    }
                    break;
                case -9782:
                    //Console.WriteLine("Power Ternary Ints: " + reg1.integerValue + " ^ " + reg2.integerValue + " = " + TernaryCPU.PowerTernaryInts(reg1, reg2).integerValue);
                    reg1.ChangeValue(TernaryCPU.PowerTernaryInts(reg1, reg2).integerValue);
                    PointerRoll1();
                    break;
                case -9781:
                    reg2.ChangeValue(TernaryCPU.PowerTernaryInts(reg1, reg2).integerValue);
                    PointerRoll2();
                    break;
                case -9780:
                    reg1.ChangeValue(TernaryCPU.PowerTernaryInts(reg1, datareg as TernaryIntGeneric).integerValue);
                    PointerRoll1();
                    break;
                case -9779:
                    reg2.ChangeValue(TernaryCPU.PowerTernaryInts(reg2, datareg as TernaryIntGeneric).integerValue);
                    PointerRoll2();
                    break;



                case -9700:     //floating point operations
                    freg1.ChangeValue((datareg as TernaryFloatGeneric).FullTernaryValue); //set register 1 to data register value
                    break;
                case -9699:
                    freg2.ChangeValue((datareg as TernaryFloatGeneric).FullTernaryValue); //set register 2 to data register value
                    break;
                case -9698:
                    freg1.ChangeValue(freg2.FullTernaryValue);    //copy register 2 to register 1
                    break;
                case -9697:
                    freg2.ChangeValue(freg1.FullTernaryValue);    //copy register 1 to register 2
                    break;
                case -9696:
                    facc = new TernaryFloatGeneric(freg1.FullTernaryValue); //swap register 1 and 2 using accumulator as an intermediary
                    freg1.ChangeValue(freg2.FullTernaryValue);
                    freg2.ChangeValue(facc.FullTernaryValue);
                    break;
                case -9695:
                    freg1.Invert();  //invert register 1 value not type codes
                    break;
                case -9694:
                    freg2.Invert();  //invert register 2 value not type codes
                    break;
                case -9693:
                    //freg1.ChangeValue(TernaryCPU.FloatAbsoluteValue(freg1).FullTernaryValue);  //absolute value of reg 1
                    break;
                case -9692:
                    //freg2.ChangeValue(TernaryCPU.FloatAbsoluteValue(freg2).FullTernaryValue); //absolute value of reg 2
                    break;
                case -9691:
                    //freg1.ChangeValue(TernaryCPU.FloatNegativeAbsoluteValue(freg1).FullTernaryValue);  //negative abs value of reg 1
                    break;
                case -9690:
                    //freg2.ChangeValue(TernaryCPU.FloatNegativeAbsoluteValue(freg2).FullTernaryValue);  //negative abs value of reg 2
                    break;
                case -9689:
                    //acc.ChangeValue(new TernaryIntGeneric(TernaryCPU.ComparisonOperator(freg1, freg2).ToString()).integerValue);
                    break;

                case -9688: //add freg1 and freg2 and place in freg 1
                    freg1.ChangeValue(TernaryCPU.AddTernaryFloats(freg1, freg2).FullTernaryValue);
                    PointerRoll1();
                    break;
                case -9687: //add freg1 and freg 2 and place in freg 2
                    freg2.ChangeValue(TernaryCPU.AddTernaryFloats(freg1, freg2).FullTernaryValue);
                    PointerRoll2();
                    break;
                case -9686:
                    freg1.ChangeValue(TernaryCPU.AddTernaryFloats(datareg as TernaryFloatGeneric, freg1).FullTernaryValue);
                    PointerRoll1();
                    break;
                case -9685:
                    freg2.ChangeValue(TernaryCPU.AddTernaryFloats(datareg as TernaryFloatGeneric, freg2).FullTernaryValue);
                    PointerRoll2();
                    break;


                case -9684: //subtract freg2 from freg1 and place in freg1
                    freg1.ChangeValue(TernaryCPU.SubtractTernaryFloats(freg1, freg2).FullTernaryValue);
                    PointerRoll1();
                    break;
                case -9683: //subtract freg2 from freg1 and place in freg2
                    freg2.ChangeValue(TernaryCPU.SubtractTernaryFloats(freg1, freg2).FullTernaryValue);
                    PointerRoll2();
                    break;
                case -9682:
                    freg1.ChangeValue(TernaryCPU.SubtractTernaryFloats(datareg as TernaryFloatGeneric, freg1).FullTernaryValue);
                    PointerRoll1();
                    break;
                case -9681:
                    freg2.ChangeValue(TernaryCPU.SubtractTernaryFloats(datareg as TernaryFloatGeneric, freg2).FullTernaryValue);
                    PointerRoll2();
                    break;



                case -9600:     //goto operations
                    Goto(datareg as TernaryIntGeneric);
                    break;
                case -9599:
                    if (ComparisonOperator(reg1, reg2) == '0')  //equal
                    {
                        Goto(datareg as TernaryIntGeneric);
                    }
                    break;
                case -9598:
                    if (ComparisonOperator(reg1, reg2) == '-')  //less
                    {
                        Goto(datareg as TernaryIntGeneric);
                    }
                    break;
                case -9597:
                    if (ComparisonOperator(reg1, reg2) == '+')  //more
                    {
                        Goto(datareg as TernaryIntGeneric);
                    }
                    break;
                case -9596:
                    Goto(reg1);
                    break;
                case -9595:
                    Goto(reg2);
                    break;

                case -9500:     //memory read
                    reg1.ChangeValue(this.memsys.GetData(datareg as TernaryIntGeneric).FullTernaryString);
                    break;
                case -9499:
                    reg2.ChangeValue(this.memsys.GetData(datareg as TernaryIntGeneric).FullTernaryString);
                    break;
                case -9498:
                    reg1.ChangeValue(this.memsys.GetInst(datareg as TernaryIntGeneric).integerValue);
                    break;
                case -9497:
                    reg2.ChangeValue(this.memsys.GetInst(datareg as TernaryIntGeneric).integerValue);
                    break;
                case -9496: //memory write
                    this.memsys.SetData(datareg as TernaryIntGeneric, reg1);
                    break;
                case -9495:
                    this.memsys.SetData(datareg as TernaryIntGeneric, reg2);
                    break;
                case -9494:
                    this.memsys.SetInst(datareg as TernaryIntGeneric, reg1);
                    break;
                case -9493:
                    this.memsys.SetInst(datareg as TernaryIntGeneric, reg2);
                    break;
                case -9492:     //io write
                    this.iosys.IOWrite(datareg as TernaryIntGeneric, reg1);
                    break;
                case -9491:
                    this.iosys.IOWrite(datareg as TernaryIntGeneric, reg2);
                    break;
                case -9490:     //io read
                    reg1.ChangeValue(this.iosys.IORead(datareg as TernaryIntGeneric).FullTernaryString);
                    break;
                case -9489:
                    reg2.ChangeValue(this.iosys.IORead(datareg as TernaryIntGeneric).FullTernaryString);
                    break;

                case -9400: //float memory read
                    freg1.ChangeValue(this.memsys.GetData(datareg as TernaryIntGeneric).FullTernaryString);
                    break;
                case -9399:
                    freg2.ChangeValue(this.memsys.GetData(datareg as TernaryIntGeneric).FullTernaryString);
                    break;
                case -9398: //float memory write
                    this.memsys.SetData(datareg as TernaryIntGeneric, freg1);
                    break;
                case -9397:
                    this.memsys.SetData(datareg as TernaryIntGeneric, freg2);
                    break;
                case -9396:     //float io write
                    this.iosys.IOWrite(datareg as TernaryIntGeneric, freg1);
                    break;
                case -9395:
                    this.iosys.IOWrite(datareg as TernaryIntGeneric, freg2);
                    break;
                case -9394:     //float io read
                    freg1.ChangeValue(this.iosys.IORead(datareg as TernaryIntGeneric).FullTernaryString);
                    break;
                case -9393:
                    freg2.ChangeValue(this.iosys.IORead(datareg as TernaryIntGeneric).FullTernaryString);
                    break;

                case -9300: //character read/write operations
                    this.iosys.IOTtyWrite(new TernaryChar(reg1.FullTernaryString));
                    break;
                case -9299:
                    this.iosys.IOTtyWrite(new TernaryChar(reg2.FullTernaryString));
                    break;
                case -9298:
                    reg1.ChangeValue(this.iosys.IOTtyRead().FullTernaryString);
                    break;
                case -9297:
                    reg2.ChangeValue(this.iosys.IOTtyRead().FullTernaryString);
                    break;


                    //            case "fopwri1" -9460: //fast output ports (FOPs)
                    //case "fopset1" -9459:
                    //            case "fopwri2" -9458:
                    //            case "fopset2" -9457:
                    //            case "fopwri3" -9456:
                    //            case "fopset3" -9455:

            }

            execpoint.ChangeValue(execpoint.integerValue + 1);

            return new TernaryIntGeneric(0);
        }

        public static void PointerRoll1()
        {
            
        }

        public static void PointerRoll2()
        {

        }

        public static TernaryFloatGeneric SubtractTernaryFloats(TernaryFloatGeneric float1, TernaryFloatGeneric float2)
        {
            var temp = new TernaryFloatGeneric(float2.FullTernaryValue);
            temp.Invert();
            return AddTernaryFloats(float1, temp);
        }

        public static TernaryFloatGeneric AddTernaryFloats(TernaryFloatGeneric float1, TernaryFloatGeneric float2)
        {
            if (float1.DoubleValue == 0)
            {
                return float2;
            }
            else if (float2.DoubleValue == 0)
            {
                return float1;
            }

            var float1exp = new TernaryIntGeneric(float1.Exponent);
            var float2exp = new TernaryIntGeneric(float2.Exponent);

            if ((float1.SignSubnormal == "+" && float2.SignSubnormal == "+") || (float1.SignSubnormal == "-" && float2.SignSubnormal == "-"))
            {
                if (ComparisonOperator(float1exp, float2exp) == '+')
                {
                    var diff = SubtractTernaryInts(float1exp, float2exp);
                    var temp = TritShiftRight(float2.SignSubnormal + float2.Significand, (int)diff.integerValue);
                    (var addedSigs, var carry) = AddTernaryFloatSignificands(temp, float1.SignSubnormal + float1.Significand);
                    var carryInt = new TernaryIntGeneric(carry);
                    if (addedSigs.Length > TernaryFloatGeneric.nSignificandTrits)
                    {
                        addedSigs = addedSigs.Substring(1);
                    }
                    var newExp = AddTernaryInts(float1exp, carryInt);
                    var newExpStr = TernaryIntGeneric.ConvertIntToBalancedTernaryString(newExp.integerValue, TernaryFloatGeneric.nExponentTrits);
                    if (newExp.integerValue > TernaryFloatGeneric.maxExponentValue)
                    {
                        return new TernaryFloatGeneric(addedSigs, newExpStr, float1.SignSubnormal, TernaryFloatGeneric.TypeCodeEnum.Infinity);
                    }
                    return new TernaryFloatGeneric(addedSigs, newExpStr, float1.SignSubnormal, TernaryFloatGeneric.TypeCodeEnum.RealNumberInfinitePrecision);

                }
                else if (ComparisonOperator(float1exp, float2exp) == '-')
                {
                    var diff = SubtractTernaryInts(float2exp, float1exp);
                    var temp = TritShiftRight(float1.SignSubnormal + float1.Significand, (int)diff.integerValue);
                    (var addedSigs, var carry) = AddTernaryFloatSignificands(temp, float2.SignSubnormal + float2.Significand);
                    var carryInt = new TernaryIntGeneric(carry);
                    if (addedSigs.Length > TernaryFloatGeneric.nSignificandTrits)
                    {
                        addedSigs = addedSigs.Substring(1);
                    }
                    var newExp = AddTernaryInts(float2exp, carryInt);
                    var newExpStr = TernaryIntGeneric.ConvertIntToBalancedTernaryString(newExp.integerValue, TernaryFloatGeneric.nExponentTrits);
                    if (newExp.integerValue > TernaryFloatGeneric.maxExponentValue)
                    {
                        return new TernaryFloatGeneric(addedSigs, newExpStr, float1.SignSubnormal, TernaryFloatGeneric.TypeCodeEnum.Infinity);
                    }
                    return new TernaryFloatGeneric(addedSigs, newExpStr, float2.SignSubnormal, TernaryFloatGeneric.TypeCodeEnum.RealNumberInfinitePrecision);
                }
                else if (ComparisonOperator(float1exp, float2exp) == '0')
                {
                    (var addedSigs, var carry) = AddTernaryFloatSignificands(float1.SignSubnormal + float1.Significand, float1.SignSubnormal + float2.Significand);
                    var carryInt = new TernaryIntGeneric(carry);
                    var newExp = AddTernaryInts(float1exp, carryInt);
                    var newExpStr = TernaryIntGeneric.ConvertIntToBalancedTernaryString(newExp.integerValue, TernaryFloatGeneric.nExponentTrits);
                    if (newExp.integerValue > TernaryFloatGeneric.maxExponentValue)
                    {
                        return new TernaryFloatGeneric(addedSigs, newExpStr, float1.SignSubnormal, TernaryFloatGeneric.TypeCodeEnum.Infinity);
                    }
                    return new TernaryFloatGeneric(addedSigs, newExpStr, float1.SignSubnormal, TernaryFloatGeneric.TypeCodeEnum.RealNumberInfinitePrecision);
                }
                else
                {
                    return null;
                }
            }
            else if (float1.SignSubnormal == "-" ^ float2.SignSubnormal == "-")
            {
                if (ComparisonOperator(float1exp, float2exp) == '+')
                {
                    var diff = SubtractTernaryInts(float1exp, float2exp);
                    var temp = TritShiftRight(float2.SignSubnormal + float2.Significand, (int)diff.integerValue);
                    (var addedSigs, var carry) = AddTernaryFloatSignificands(temp, float1.SignSubnormal + float1.Significand);
                    var carryInt = new TernaryIntGeneric(carry);
                    if (addedSigs.Length > TernaryFloatGeneric.nSignificandTrits)
                    {
                        addedSigs = addedSigs.Substring(1);
                    }
                    if (diff.integerValue == 1)
                    {
                        var newSign = addedSigs.ToCharArray()[0].ToString();
                        addedSigs = addedSigs.Substring(1) + "0";
                        var newExp = float2exp;
                        var newExpStr = TernaryIntGeneric.ConvertIntToBalancedTernaryString(newExp.integerValue, TernaryFloatGeneric.nExponentTrits);
                        return new TernaryFloatGeneric(addedSigs, newExpStr, newSign, TernaryFloatGeneric.TypeCodeEnum.RealNumberInfinitePrecision);
                    }
                    else
                    {
                        var newExp = AddTernaryInts(float1exp, carryInt);
                        var newExpStr = TernaryIntGeneric.ConvertIntToBalancedTernaryString(newExp.integerValue, TernaryFloatGeneric.nExponentTrits);
                        return new TernaryFloatGeneric(addedSigs, newExpStr, float1.SignSubnormal, TernaryFloatGeneric.TypeCodeEnum.RealNumberInfinitePrecision);
                    }
                }
                else if (ComparisonOperator(float1exp, float2exp) == '-')
                {
                    var diff = SubtractTernaryInts(float1exp, float2exp);
                    var temp = TritShiftRight(float1.SignSubnormal + float1.Significand, (int)diff.integerValue);
                    (var addedSigs, var carry) = AddTernaryFloatSignificands(temp, float2.SignSubnormal + float2.Significand);
                    var carryInt = new TernaryIntGeneric(carry);
                    if (addedSigs.Length > TernaryFloatGeneric.nSignificandTrits)
                    {
                        addedSigs = addedSigs.Substring(1);
                    }
                    if (diff.integerValue == 1)
                    {
                        var newSign = addedSigs.ToCharArray()[0].ToString();
                        addedSigs = addedSigs.Substring(1) + "0";
                        var newExp = float1exp;
                        var newExpStr = TernaryIntGeneric.ConvertIntToBalancedTernaryString(newExp.integerValue, TernaryFloatGeneric.nExponentTrits);
                        return new TernaryFloatGeneric(addedSigs, newExpStr, newSign, TernaryFloatGeneric.TypeCodeEnum.RealNumberInfinitePrecision);
                    }
                    else
                    {
                        var newExp = AddTernaryInts(float2exp, carryInt);
                        var newExpStr = TernaryIntGeneric.ConvertIntToBalancedTernaryString(newExp.integerValue, TernaryFloatGeneric.nExponentTrits);
                        return new TernaryFloatGeneric(addedSigs, newExpStr, float2.SignSubnormal, TernaryFloatGeneric.TypeCodeEnum.RealNumberInfinitePrecision);
                    }
                }
                else if (ComparisonOperator(float1exp, float2exp) == '0' && float1.DoubleValue != -float2.DoubleValue)
                {
                    (var addedSigs, _) = AddTernaryFloatSignificands(float1.SignSubnormal + float1.Significand, float2.SignSubnormal + float2.Significand);
                    var addedArray = addedSigs.ToCharArray();
                    var newSign = "";
                    var carryInt = new TernaryIntGeneric(0);
                    for (int i = 0; i < addedArray.Length; i++)
                    {
                        if (addedArray[i] == '+' || addedArray[i] == '-')
                        {
                            newSign = addedArray[i].ToString();
                            addedSigs = addedSigs.Substring(i + 1);
                            for (int n = addedSigs.Length; n < TernaryFloatGeneric.nSignificandTrits; n++)
                            {
                                addedSigs = addedSigs + "0";
                            }
                            carryInt = new TernaryIntGeneric(-i);
                            break;
                        }
                    }
                    var newExp = AddTernaryInts(float1exp, carryInt);
                    var newExpStr = TernaryIntGeneric.ConvertIntToBalancedTernaryString(newExp.integerValue, TernaryFloatGeneric.nExponentTrits);
                    return new TernaryFloatGeneric(addedSigs, newExpStr, newSign, TernaryFloatGeneric.TypeCodeEnum.RealNumberInfinitePrecision);
                }
                else
                {
                    return new TernaryFloatGeneric(0);
                }
            }
            else if (float1.SignSubnormal == "0" || float2.SignSubnormal == "0")
            {
                return new TernaryFloatGeneric(0);//TODO: implement subnormal number addition here
            }
            else
            {
                return null;
            }
        }

        public static (string, int) AddTernaryFloatSignificands(string sig1, string sig2)
        {
            var temp1 = sig1.ToCharArray();
            var temp2 = sig2.ToCharArray();
            var temp3 = CreateEmptyTernaryValue(temp1.Length).ToCharArray();

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
                    sum = 0;
                }
                else if (n3 == -3)
                {
                    carry = -1;
                    sum = 0;
                }
                else
                {
                    carry = 0;
                    sum = n3;
                }
                temp3[i] = ConvertNumberToBalancedTrit(sum);
                if (i == 0 && carry == 1)
                {
                    var tempstr = new string(temp3);
                    //Console.WriteLine("Carry +1: " + tempstr);
                    tempstr = TritShiftRight(tempstr, 1);
                    //Console.WriteLine("Shift Right 1: " + tempstr);
                    var arr = tempstr.ToCharArray();
                    arr[1] = '-';
                    tempstr = new string(arr).Substring(1);
                    //Console.WriteLine("Add - to char 1 and substring: " + tempstr);
                    return (tempstr, 1);
                }
                else if (i == 0 && carry == -1)
                {
                    var tempstr = new string(temp3);
                    //Console.WriteLine("Carry -1: " + tempstr);
                    tempstr = TritShiftRight(tempstr, 1);
                    //Console.WriteLine("Shift Right 1: " + tempstr);
                    var arr = tempstr.ToCharArray();
                    arr[1] = '+';
                    tempstr = new string(arr).Substring(1);
                    //Console.WriteLine("Add + to char 1 and substring: " + tempstr);
                    return (tempstr, 1);
                }
            }
            return (new string(temp3), 0);
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
                    if (pow.integerValue > TernaryCPU.MaximumPositiveInteger() || pow.integerValue < TernaryCPU.MaximumNegativeInteger() )
                    {
                        Console.WriteLine("Power overflow/underflow! Returning maximum positive or negative integer.");
                        return pow;
                    }
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
                    return new TernaryIntGeneric(0);
                }
                else
                {
                    return dividend;
                }
            }
            else
            {
                Console.WriteLine("TERNARY INTEGER TOO LONG FOR PROCESSOR WIDTH");
                return null;
            }
        }

        public static TernaryIntGeneric NegativeAbsoluteValue(TernaryIntGeneric int1)
        {
            var temp = AbsoluteValue(int1);
            temp.Invert();
            return temp;
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
                    if (temp3.integerValue > TernaryCPU.MaximumPositiveInteger())
                    {
                        Console.WriteLine("Trit Shift Left Produced An Overflow");
                    }
                    product = AddTernaryInts(product, temp3);
                    if (product.integerValue > TernaryCPU.MaximumPositiveInteger())
                    {
                        Console.WriteLine("Product overflow! Maximum positive integer returned.");
                        return new TernaryIntGeneric(TernaryCPU.MaximumPositiveInteger());
                    }
                    else if (product.integerValue < TernaryCPU.MaximumNegativeInteger())
                    {
                        Console.WriteLine("Product underflow! Maximum negative integer returned.");
                        return new TernaryIntGeneric(TernaryCPU.MaximumNegativeInteger());
                    }
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
            if (nTrits > 0)
            {
                var temp = ternaryValue.ToCharArray();

                var temp2 = new char?[temp.Length];
                for (int i = 0; i < temp.Length; i++)
                {
                    if (i < nTrits && temp[i] == '+')
                    {
                        Console.WriteLine("TritShiftLeft Overflow! Returning maximum positive value.");
                        return TernaryIntGeneric.ConvertIntToBalancedTernaryString(TernaryCPU.MaximumPositiveInteger(), temp.Length);
                    }
                    else if (i < nTrits && temp[i] == '-')
                    {
                        Console.WriteLine("TritShiftLeft Underflow! Returning maxmimum negative value.");
                        return TernaryIntGeneric.ConvertIntToBalancedTernaryString(TernaryCPU.MaximumNegativeInteger(), temp.Length);
                    }
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
            else
            {
                return ternaryValue;
            }
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
            char[] temp3 = CreateEmptyTernaryValue(temp1.Length).ToCharArray();
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
                        sum = 0;
                    }
                    else if (n3 == -3)
                    {
                        carry = -1;
                        sum = 0;
                    }
                    else
                    {
                        carry = 0;
                        sum = n3;
                    }
                    temp3[i] = ConvertNumberToBalancedTrit(sum);
                    if (i == 0 && carry == -1)
                    {
                        Console.WriteLine("Integer Add Underflow! Returning maximum negative integer.");
                        return new TernaryIntGeneric(TernaryCPU.MaximumNegativeInteger());
                    }
                    else if (i == 0 && carry == 1)
                    {
                        Console.WriteLine("Integer Add Overflow! Returning maximum positive integer");
                        return new TernaryIntGeneric(TernaryCPU.MaximumPositiveInteger());
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
