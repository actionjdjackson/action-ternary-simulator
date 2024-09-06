using System;
using System.Collections.Generic;
using System.Linq;

namespace ActionTernarySimulator
{
    public class TernaryAssembly
    {
        public TerInst[] instructionList;
 
		public TernaryIntGeneric GetOpCodeForInstruction(string instruction)
        {
			foreach (var inst in instructionList)
			{
				if (inst.keywords.Contains(instruction))
				{
					return inst.opcode;
				}
			}
			return new TernaryIntGeneric(0);
		}

        public TernaryAssembly()
        {
			this.instructionList = new TerInst[] {

				new TerInst( new string[] { "null" }, 0),
				new TerInst( new string[] {"setreg1"}, -9841),
				new TerInst( new string[] {"setreg2"}, -9840),
				new TerInst( new string[] {"copy2to1"}, -9839),
				new TerInst( new string[] {"copy1to2"}, -9838),
				new TerInst( new string[] {"regswap"}, -9837),
				new TerInst( new string[] {"invert1"}, -9836),
				new TerInst( new string[] {"invert2"}, -9835),
				new TerInst( new string[] {"abs1"}, -9834),
				new TerInst( new string[] {"abs2"}, -9833),
				new TerInst( new string[] {"nabs1"}, -9832),
				new TerInst( new string[] {"nabs2"}, -9831),

				new TerInst( new string[] {"tAND", "&"}, -9830),
				new TerInst( new string[] {"tOR", "|"}, -9829),
				new TerInst( new string[] {"tXNOR", "~^"}, -9828),
				new TerInst( new string[] {"tCONS", "?"}, -9827),
				new TerInst( new string[] {"tSUM", "$"}, -9826),
				new TerInst( new string[] {"tEQUALITY", "#"}, -9825),
				new TerInst( new string[] {"tCROSS", "x*"}, -9824),
                new TerInst( new string[] {"tNEGATE", "~"}, -9823),

				new TerInst( new string[] {"shiftleft1by2", "<<<"}, -9822),
				new TerInst( new string[] {"shiftright1by2", ">>>"}, -9821),

				new TerInst( new string[] {"NOT", "!"}, -9820),
                new TerInst( new string[] {"EQUALTO", "=="}, -9819),
				new TerInst( new string[] {"AND", "&&"}, -9818),
				new TerInst( new string[] {"OR", "||"}, -9817),
				new TerInst( new string[] {"COMPARE", "<=>"}, -9816),
				new TerInst( new string[] {"NOTEQUALTO", "!="}, -9815),

				new TerInst( new string[] {"stop"}, -9000),

                new TerInst( new string[] {"add", "add1"}, -9800),
                new TerInst( new string[] {"add2"}, -9799),
				new TerInst( new string[] {"adddata1"}, -9798),
				new TerInst( new string[] {"adddata2"}, -9797),
				new TerInst( new string[] {"sub1", "sub"}, -9796),//sub instructions
				new TerInst( new string[] {"sub2"}, -9795),
				new TerInst( new string[] {"subdata1"}, -9794),
				new TerInst( new string[] {"subdata2"}, -9793),
				new TerInst( new string[] {"mul", "mul1"}, -9792),//mul instructions
				new TerInst( new string[] {"mul2"}, -9791),
				new TerInst( new string[] {"muldata1"}, -9790),
				new TerInst( new string[] {"muldata2"}, -9789),
				new TerInst( new string[] {"div", "div1"}, -9788),//div instructions
				new TerInst( new string[] {"div2"}, -9787),
				new TerInst( new string[] {"divdata1"}, -9786),
				new TerInst( new string[] {"divdata2"}, -9785),
				new TerInst( new string[] {"divmod", "divmod1"}, -9784),//divrem
				new TerInst( new string[] {"divmod2"}, -9783),
				new TerInst( new string[] {"pow", "pow1"}, -9782),
				new TerInst( new string[] {"pow2"}, -9781),
				new TerInst( new string[] {"powdata1"}, -9780),
				new TerInst( new string[] {"powdata2"}, -9779),

				new TerInst( new string[] {"setfreg1"}, -9700),	//float instructions
				new TerInst( new string[] {"setfreg2"}, -9699),
				new TerInst( new string[] {"copyf2to1"}, -9698),
				new TerInst( new string[] {"copyf1to2"}, -9697),
				new TerInst( new string[] {"fregswap"}, -9696),
				new TerInst( new string[] {"finvert1"}, -9695),
				new TerInst( new string[] {"finvert2"}, -9694),
				new TerInst( new string[] {"fabs1"}, -9693),
				new TerInst( new string[] {"fabs2"}, -9692),
				new TerInst( new string[] {"fnabs1"}, -9691),
				new TerInst( new string[] {"fnabs2"}, -9690),
				new TerInst( new string[] {"FCOMPARE"}, -9689),
				new TerInst( new string[] {"fadd", "fadd1"}, -9688),
				new TerInst( new string[] {"fadd2"}, -9687),
				new TerInst( new string[] {"fadddata1"}, -9686),
				new TerInst( new string[] {"fadddata2"}, -9685),
				new TerInst( new string[] {"fsub", "fsub1"}, -9684),
				new TerInst( new string[] {"fsub2"}, -9683),
				new TerInst( new string[] {"fsubdata1", "fsub1fromdata"}, -9682),
				new TerInst( new string[] {"fsubdata2", "fsub2fromdata"}, -9681),

				new TerInst( new string[] {"goto", "gotodata"}, -9600),//goto operations
				new TerInst( new string[] {"gotoif", "gotodataif", "gotoifequal"}, -9599),
				new TerInst( new string[] {"gotoifless"}, -9598),
				new TerInst( new string[] {"gotoifmore", "gotoifgreater"}, -9597),
				new TerInst( new string[] {"gotoreg1"}, -9596),
				new TerInst( new string[] {"gotoreg2"}, -9595),

				new TerInst( new string[] {"dataread1", "romread1"}, -9500),//memory read
				new TerInst( new string[] {"dataread2", "romread2"}, -9499),
				new TerInst( new string[] {"instread1"}, -9498),
				new TerInst( new string[] {"instread2"}, -9497),
				new TerInst( new string[] {"datawrite1", "setdata"}, -9496),//memory write
				new TerInst( new string[] {"datawrite2", "setdata2"}, -9495),
				new TerInst( new string[] {"instwrite1", "setinst"}, -9494),
				new TerInst( new string[] {"instwrite2", "setinst2"}, -9493),
                new TerInst( new string[] {"iowrite1", "IOwrite1"}, -9492),//io write
				new TerInst( new string[] {"iowrite2", "IOwrite2"}, -9491),
                new TerInst( new string[] {"ioread1", "IOread1"}, -9490),//io read
				new TerInst( new string[] {"ioread2", "IOread2"}, -9489),

                new TerInst( new string[] {"fopwri1"}, -9460), //fast output ports (FOPs)
				new TerInst( new string[] {"fopset1"}, -9459),
                new TerInst( new string[] {"fopwri2"}, -9458),
                new TerInst( new string[] {"fopset2"}, -9457),
                new TerInst( new string[] {"fopwri3"}, -9456),
                new TerInst( new string[] {"fopset3"}, -9455),

				new TerInst( new string[] {"datafread1", "romfread1"}, -9400),
				new TerInst( new string[] {"datafread2", "romfread2"}, -9399),
				new TerInst( new string[] {"datafwrite1"}, -9398),
				new TerInst( new string[] {"datafwrite2"}, -9397),
				new TerInst( new string[] {"iofwrite1", "IOfwrite1"}, -9396),
				new TerInst( new string[] {"iofwrite2", "IOfwrite2"}, -9395),
				new TerInst( new string[] {"iofread1", "IOfread1"}, -9394),
				new TerInst( new string[] {"iofread2", "IOfread2"}, -9393),

				//stackbank("s1", -9100),
				//stackbank("s2", -9101),
				//stackbank("s3", -9102),
				//stackbank("s4", -9103),
				//stackbank("s5", -9104),
				//stackbank("s6", -9105),
				new TerInst( new string[] {"excatch"}, 100),
				new TerInst( new string[] {"expass"}, 101),
				new TerInst( new string[] {"exreturn"}, 102),
				new TerInst( new string[] {"exclear"}, 103),
				new TerInst( new string[] {"exceptcode"}, 104),

			};
        }
    }

    public class TerInst
    {
        public string[] keywords;
        public TernaryIntGeneric opcode;

        public TerInst(string[] keywords, int opcode)
        {
            this.keywords = keywords;
            this.opcode = new TernaryIntGeneric(opcode);
        }
    }

}
