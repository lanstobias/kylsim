using System;
using System.Drawing;

namespace kylsim
{
    public class VVS
    {
        protected string Name { get; set; }
        protected float X { get; set; }
        protected float Y { get; set; }
        protected float W { get; set; }
        protected float H { get; set; }

        protected VVS Next    { get; set; }
        protected VVS NodeIn  { get; set; }
        protected VVS NodeOut { get; set; }

        protected VVS(string name = "", float x = 0, float y = 0, float w = 0, float h = 0,
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
        private float Pressure { get; set; }
        private bool Adjustable { get; set; }


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
            Pen pen2 = new Pen(Color.Red);
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
