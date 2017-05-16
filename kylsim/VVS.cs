using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kylsim
{
    public class VVS
    {
<<<<<<< HEAD
=======

>>>>>>> origin/develop
        public string Name { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public int W { get; set; }
        public int H { get; set; }

<<<<<<< HEAD
        public VVS Next { get; set; }
        public VVS NodeIn { get; set; }
        public VVS NodeOut { get; set; }

        public VVS(string name = "", int x = 0, int y = 0, int w = 0, int h = 0,
                    VVS next = null, VVS nodeIn = null, VVS nodeOut = null)
=======
        public VVS Next{get; set;}
        public VVS NodeIn { get; set; }
        public VVS NodeOut { get; set; }
        //Konstruktor
        public VVS(string name="",int x=0, int y=0, int w=0, int h=0,
            VVS next=null, VVS nodeIn=null, VVS nodeOut = null)
>>>>>>> origin/develop
        {
            Name = name;
            X = x;
            Y = y;
            W = w;
            H = h;
<<<<<<< HEAD
            Next    = next;
            NodeIn  = nodeIn;
=======
            Next = next;
            NodeIn = nodeIn;
>>>>>>> origin/develop
            NodeOut = nodeOut;
        }
    }

<<<<<<< HEAD
    public class Node : VVS
    {
        
=======
    public class Node: VVS
    {

>>>>>>> origin/develop
    }

    public class Valve : VVS
    {
<<<<<<< HEAD
        
    }

    public class Pump : VVS
    {
        
    }

    public class HeatExchanger : VVS
    {
        
    }
=======

    }
    
    public class Pump : VVS
    {

    }
    public class HeatExchanger : VVS
    {

    }

>>>>>>> origin/develop
}
