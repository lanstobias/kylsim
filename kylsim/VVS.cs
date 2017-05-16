using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kylsim
{
    public class VVS
    {
        public string Name { get; set; }
        public float X { get; set; }
        public float Y { get; set; }
        public float W { get; set; }
        public float H { get; set; }

        public VVS Next { get; set; }
        public VVS NodeIn { get; set; }
        public VVS NodeOut { get; set; }

        public VVS(string name = "", float x = 0, float y = 0, float w = 0, float h = 0,
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
        public float Pressure { get; set; }
        public bool Adjustable { get; set; }


        public Node(string name = "", float x = 0, float y = 0, float w = 0, float h = 0,
                    VVS next = null, VVS nodeIn = null, VVS nodeOut = null, 
                    float pressure = 0, bool adjustable = false)
        {
            Name = name;
            X = x;
            Y = y;
            W = w;
            H = h;
            Next       = next;
            NodeIn     = nodeIn;
            NodeOut    = nodeOut;
            Pressure   = pressure;
            Adjustable = adjustable;
        }

        public void Draw(Graphics canvas)
        {
            Pen pen2 = new Pen(Color.Blue);
            // Draw node
            canvas.DrawEllipse(pen2, X, Y, W, H);
        }

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
