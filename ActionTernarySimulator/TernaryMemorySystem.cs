using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ActionTernarySimulator
{
    public class TernaryMemorySystem
    {
        public static int MEMORY_SUBSYSTEM_BUS_WIDTH = TernaryCPU.MEMORY_BUS_WIDTH_IN_TRITS;
        private Dictionary<Int64, TernaryNumGeneric> instDict = new Dictionary<long, TernaryNumGeneric>();
        private Dictionary<Int64, TernaryNumGeneric> dataDict = new Dictionary<long, TernaryNumGeneric>();

        public TernaryMemorySystem(string tromFile, int mem_id = 0)
        {
            Console.WriteLine("Ternary Memory System Starting Up...");
            var trom = File.ReadLines(tromFile);
            Int64 lineCount = TernaryCPU.MaximumNegativeIntegerMemory();
            foreach (var line in trom)
            {
                var rmline = line.Replace("\n", "");
                var rmlinePair = rmline.Split(',');
                this.instDict[lineCount] = new TernaryIntGeneric(Int64.Parse(rmlinePair[0]));
                if (rmlinePair[1].Contains(".") || rmlinePair[1].Contains("e") || rmlinePair[1].Contains("E"))
                {
                    try
                    {
                        this.dataDict[lineCount] = new TernaryFloatGeneric(double.Parse(rmlinePair[1]));
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Exception while parsing a floating point number in TROM: " + ex.Message); 
                    }
                }
                else
                {
                    try
                    {
                        this.dataDict[lineCount] = new TernaryIntGeneric(Int64.Parse(rmlinePair[1]));
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Exception while parsing an integer in TROM: " + ex.Message);
                    }
                }
                Console.WriteLine("TROM: " + lineCount + " : " + rmlinePair[0] + ", " + rmlinePair[1]);
                lineCount++;
            }
            Console.WriteLine("Padding Memory...");
            while (lineCount <= TernaryCPU.MaximumPositiveIntegerMemory())
            {
                this.instDict[lineCount] = new TernaryIntGeneric(0);
                this.dataDict[lineCount] = new TernaryIntGeneric(0);
                lineCount++;
            }
        }

        public TernaryIntGeneric GetInst(TernaryIntGeneric address)
        {
            return this.instDict[address.integerValue] as TernaryIntGeneric;
        }

        public TernaryNumGeneric GetData(TernaryIntGeneric address)
        {
            return this.dataDict[address.integerValue];
        }

        public void SetInst(TernaryIntGeneric address, TernaryIntGeneric value)
        {
            (this.instDict[address.integerValue] as TernaryIntGeneric).ChangeValue(value.integerValue);
        }

        public void SetData(TernaryIntGeneric address, TernaryNumGeneric value)
        {
            if (value is TernaryIntGeneric)
            {
                this.dataDict[address.integerValue] = new TernaryIntGeneric((value as TernaryIntGeneric).fullTernaryValue);
            }
            if (value is TernaryFloatGeneric)
            {
                this.dataDict[address.integerValue] = new TernaryFloatGeneric((value as TernaryFloatGeneric).FullTernaryValue);
            }
        }

        public void ResetLoadROM(string tromFile)
        {
            var trom = File.ReadLines(tromFile);
            Int64 lineCount = TernaryCPU.MaximumNegativeIntegerMemory();
            foreach (var line in trom)
            {
                var rmline = line.Replace("\n", "");
                var rmlinePair = rmline.Split(',');
                this.instDict[lineCount] = new TernaryIntGeneric(Int64.Parse(rmlinePair[0]));
                this.dataDict[lineCount] = new TernaryIntGeneric(Int64.Parse(rmlinePair[1]));
                lineCount++;
            }
            while (lineCount <= TernaryCPU.MaximumPositiveIntegerMemory())
            {
                this.instDict[lineCount] = new TernaryIntGeneric(0);
                this.dataDict[lineCount] = new TernaryIntGeneric(0);
                lineCount++;
            }
        }

    }
}
