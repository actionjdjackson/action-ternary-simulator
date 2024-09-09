## Action Ternary Simulator ##
Working on a Ternary Computer Simulation using strings of ```0, -, +``` characters as data and instructions.
Currently can process addition, subtraction, multiplication, division, x to the power of y, absolute value, negative absolute value, and ternary comparisons on ternary integers of the class ```TernaryIntGeneric```.
I don't simply let the computer do the math in binary and then translate to ternary, instead, I focus on changing the values of the character strings directly to do the operations I described. I also currently have floating point addition and subtraction working using the class ```TernaryFloatGeneric```. Also working on a simple assembly language. The simulator runs on a .trom file with instruction codes and data register values in pairs.

### X-Trit Floating Point Standard ###
Also combines a generic Ternary Floating Point number system with a class called ```TernaryFloatGeneric```, with which you can specify the maximum number of trits, the precision, and the magnitude 
(as well as any flags you want to include) and it will solve for those values, allowing you to cut it down or build it up as you choose, based on the total number of trits you provided. The floating point 
standard is based on "Ternary27 A Balanced Ternary Floating Point Format Version 3.1 by Mechanical Advantage" - you can specify whether you want a sign trit (allowing for a zero trit sign that means a 
subnormal number is being represented and more precision is granted based on the size of the exponent part of the floating point number, which is assumed to be at its least possible value (-121 in 
the case of a 27-trit ternary float with 5 trits for the exponent) and the extra trits from the exponent are used for precision. This is recommended and is the default behavior. The flags are specified by how many different flag values you want to be represented in the float, which is 6 in the case of the Ternary27 implementation - with 3 extras that have not been defined - because two trits are used for the type code/flags. 
The numerical accuracy is limited to the `double`'s level of precision (a 64-bit FLoat).

#### License ####
The MIT License (MIT)
Copyright © 2024 Jacob Jackson

Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the “Software”), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED “AS IS”, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
