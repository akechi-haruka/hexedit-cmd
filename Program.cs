using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hexedit {
    class Program {
        static int Main(string[] args) {

            if (args.Length < 4) {
                Console.WriteLine("Usage: hexedit.exe <file> <outfile> <offset> <tobyte1,tobyte2,...> [frombyte1,frombyte2,...]");
                return 1;
            }

            String file = args[0];
            String outfile = args[1];
            long offset;
            try {
                offset = Convert.ToInt64(args[2], 16);
            } catch {
                Console.WriteLine("Offset is invalid");
                return 2;
            }
            List<byte> tobytes = new List<byte>();
            try {
                foreach (String s in args[3].Split(',')) {
                    tobytes.Add(Convert.ToByte(s, 16));
                }
            } catch {
                Console.WriteLine("Error parsing input bytes");
                return 3;
            }
            List<byte> frombytes = null;
            if (args.Length > 4) {
                frombytes = new List<byte>();
                foreach (String s in args[4].Split(',')) {
                    try {
                        frombytes.Add(Convert.ToByte(s, 16));
                    } catch {
                        Console.WriteLine("Error parsing from bytes: " + s);
                        return 4;
                    }
                }

            }

            if (!File.Exists(file)) {
                Console.WriteLine("Input file not found: " + file);
                return 5;
            }

#if DEBUG
            Console.WriteLine("From ("+frombytes.Count+"): " + string.Join(" ", frombytes));
            Console.WriteLine("To ("+tobytes.Count+"): " + string.Join(" ", tobytes));
            Console.WriteLine("Offset: " + offset);
#endif 

            if (frombytes != null) {
                if (frombytes.Count != tobytes.Count) {
                    Console.WriteLine("frombytes and tobytes have different length");
                    return 6;
                }
            }

            byte[] data = File.ReadAllBytes(file);

            bool allMatch = true;
            for (int i = 0; i < tobytes.Count; i++) {
                if (data[offset + i] != tobytes[i]) {
                    allMatch = false;
                    break;
                }
            }
            if (allMatch) {
                Console.WriteLine("Patch at " + args[0] + ":" + args[2] + " is already applied");
                return 7;
            }

            if (frombytes != null) {
                for (int i = 0; i < frombytes.Count; i++) {
                    if (data[offset + i] != frombytes[i]) {
                        Console.WriteLine("Byte mismatch at location " + $"0x{(offset + i):X}" + ", expected: " + $"0x{(frombytes[i]):X}" + ", got: " + $"0x{(data[offset + i]):X}");
                        return 8;
                    }
                }
            }

            for (int i = 0; i < tobytes.Count; i++) {
                data[offset + i] = tobytes[i];
            }

            File.WriteAllBytes(outfile, data);
            Console.WriteLine("Patch at " + args[0] + ":" + args[2] + " successfully applied");
            return 0;
        }
    }
}
