using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SVGToManim
{
    class Program
    {
        static void Main(string[] args)
        {
            var doc = SVGToManim.ReadSVG(args[0]);
            File.WriteAllText(args[1], doc.ToManim());
        }
    }
}
