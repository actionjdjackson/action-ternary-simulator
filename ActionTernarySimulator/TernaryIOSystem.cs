using System;
using System.Collections.Generic;
using System.Linq;

namespace ActionTernarySimulator
{
    public class TernaryIOSystem
    {
        public static int IO_SUBSYSTEM_BUS_WIDTH = TernaryCPU.IO_BUS_WIDTH_IN_TRITS;
        private Dictionary<Int64, TernaryNumGeneric> ioDict = new Dictionary<long, TernaryNumGeneric>();
        private Dictionary<Int64, TernaryIntGeneric> writeNotifyDict = new Dictionary<long, TernaryIntGeneric>();
        private List<TernaryIntGeneric> writeNotifyList = new List<TernaryIntGeneric>();
        private Dictionary<Int64, TernaryIntGeneric> readNotifyDict = new Dictionary<long, TernaryIntGeneric>();
        private List<TernaryIntGeneric> readNotifyList = new List<TernaryIntGeneric>();
        private Dictionary<Int64, TernaryIntGeneric> readOverrideDict = new Dictionary<long, TernaryIntGeneric>();
        private List<TernaryIntGeneric> readOverrideList = new List<TernaryIntGeneric>();

        public TernaryIOSystem(int io_id = 0)
        {
            Console.WriteLine("Ternary IO System Starting Up...");
            Int64 addrset = TernaryCPU.MaximumNegativeIntegerIO();
            while (addrset <= TernaryCPU.MaximumPositiveIntegerIO())
            {
                ioDict[addrset] = new TernaryIntGeneric(0);
                writeNotifyDict[addrset] = null;
                readNotifyDict[addrset] = null;
                readOverrideDict[addrset] = null;
                addrset++;
            }
            Console.WriteLine("Ternary IO System Padded To Maximum Size");
        }

        public TernaryNumGeneric IORead(TernaryIntGeneric address)
        {
            return this.ioDict[address.integerValue];
        }

        public void IOWrite(TernaryIntGeneric address, TernaryNumGeneric data)
        {
            if (data is TernaryIntGeneric)
            {
                this.ioDict[address.integerValue] = new TernaryIntGeneric((data as TernaryIntGeneric).fullTernaryValue);
                Console.WriteLine("IO address " + address.FullTernaryString + " written with value " + data.FullTernaryString + " = " + (data as TernaryIntGeneric).integerValue);
            }
            else if (data is TernaryFloatGeneric)
            {
                this.ioDict[address.integerValue] = new TernaryFloatGeneric((data as TernaryFloatGeneric).FullTernaryValue);
                Console.WriteLine("IO address " + address.FullTernaryString + " written with value " + data.FullTernaryString + " = " + (data as TernaryFloatGeneric).DoubleValue);
            }
        }

        public void IOTtyWrite(TernaryChar ternaryChar)
        {
            Console.Write(ternaryChar.character);
        }

        public TernaryChar IOTtyRead()
        {
            return new TernaryChar((char)Console.Read());
        }

    }
}
