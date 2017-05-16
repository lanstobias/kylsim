using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kylsim
{
    public class VVS
    {
        public string Name { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public int W { get; set; }
        public int H { get; set; }

        public VVS Next { get; set; }
        public VVS NodeIn { get; set; }
        public VVS NodeOut { get; set; }

        public VVS(string name = "", int x = 0, int y = 0, int w = 0, int h = 0,
                    VVS next = null, VVS nodeIn = null, VVS nodeOut = null)
        {
            Name = name;
            X = x;
            Y = y;
            W = w;
            H = h;
            Next    = next;
            NodeIn  = nodeIn;
            NodeOut = nodeOut;
        }
    }

    public class Node : VVS
    {
        
    }

    public class Valve : VVS
    {
        
    }

    public class Pump : VVS
    {
        
    }

    public class HeatExchanger : VVS
    {
        
    }
}
