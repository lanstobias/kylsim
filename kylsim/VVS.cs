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

        /// <summary>
        /// Initializes a new instance of the <see cref="VVS"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <param name="w">The w.</param>
        /// <param name="h">The h.</param>
        /// <param name="next">The next.</param>
        /// <param name="nodeIn">The node in.</param>
        /// <param name="nodeOut">The node out.</param>
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
        private double Pressure { get; set; }
        private bool Adjustable { get; set; }
        public double SumFlow { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Node"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <param name="w">The w.</param>
        /// <param name="h">The h.</param>
        /// <param name="next">The next.</param>
        /// <param name="nodeIn">The node in.</param>
        /// <param name="nodeOut">The node out.</param>
        /// <param name="pressure">The pressure.</param>
        /// <param name="adjustable">if set to <c>true</c> [adjustable].</param>
        /// <param name="sumFlow">The sum flow.</param>
        public Node(string name = "", float x = 0, float y = 0, float w = 0, float h = 0,
                    VVS next = null, VVS nodeIn = null, VVS nodeOut = null, 
                    double pressure = 0, bool adjustable = false, double sumFlow = 0)
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
            SumFlow    = sumFlow;
        }

        /// <summary>
        /// Draws the specified canvas.
        /// </summary>
        /// <param name="canvas">The canvas.</param>
        public void Draw(Graphics canvas, Brush brush, Font font, Pen pen)
        {
            // Draw ellipse
     
            canvas.DrawEllipse(pen, X, Y, W, H);

            // Draw text
    
            canvas.DrawString(Name, font, brush, (float)X + 10, (float)Y + -20);
            canvas.DrawString("Tryck: ", font, brush, (float)X + 10, (float)Y + 15);
        }

        /// <summary>
        /// Adds the sumflode.
        /// </summary>
        /// <param name="inFlow">The in flow.</param>
        /// <param name="outFlow">The out flow.</param>
        /// <returns></returns>
        public void AddSumflode(double inFlow, double outFlow)
        {
            SumFlow = (inFlow + outFlow);
        }

        /// <summary>
        /// Dynamicses this instance.
        /// </summary>
        public void Dynamics()
        {
            if (Adjustable)
            {
                if (SumFlow > 0)
                    Pressure += 0.1;
                else if (SumFlow < 0)
                    Pressure -= 0.1;
                SumFlow = 0;

            }
        }

        /// <summary>
        /// Displays the specified canvas.
        /// </summary>
        /// <param name="canvas">The canvas.</param>
        public void Display(Graphics canvas, Brush brush, Font font)
        {
            const string twoDecimals = "F1";

            
            canvas.DrawString(Pressure.ToString(twoDecimals), font, brush, (float)X + 45, (float)Y + 15);
        }
    }

    public class Valve : VVS
    {
        private double Position { get; set; }
        private double Admittance { get; set; }


        /// <summary>
        /// Initializes a new instance of the <see cref="Valve"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <param name="w">The w.</param>
        /// <param name="h">The h.</param>
        /// <param name="next">The next.</param>
        /// <param name="nodeIn">The node in.</param>
        /// <param name="nodeOut">The node out.</param>
        /// <param name="position">The position.</param>
        /// <param name="admittance">The admittance.</param>
        public Valve(string name = "", float x = 0, float y = 0, float w = 0, float h = 0,
                    VVS next = null, VVS nodeIn = null, VVS nodeOut = null,
                    double position = 0, double admittance = 10)
        {
            Name = name;
            X = x;
            Y = y;
            W = w;
            H = h;
            Next = next;
            NodeIn = nodeIn;
            NodeOut = nodeOut;
            Position = position;
            Admittance = admittance;
        }
        public void Draw(Graphics canvas, Brush brush, Font font, Pen pen)
        {
            // Draw ventil
            const int Ygrad = 10;
            const int Xgrad = 15;
       
            canvas.DrawLine(pen, X, Y, X+ Xgrad, Y+ Ygrad);
            canvas.DrawLine(pen, X, Y, X + Xgrad, Y - Ygrad);
            canvas.DrawLine(pen, X+ Xgrad, Y+ Ygrad, X + Xgrad, Y - Ygrad);
            canvas.DrawLine(pen, X, Y, X - Xgrad, Y - Ygrad);
            canvas.DrawLine(pen, X, Y, X - Xgrad, Y + Ygrad);
            canvas.DrawLine(pen, X - Xgrad, Y - Ygrad, X - Xgrad, Y + Ygrad);

            // Draw text

            canvas.DrawString(Name, font, brush, (float)X + 10, (float)Y + -20);
            canvas.DrawString("Jag är en fin ventil :3 ", font, brush, (float)X + 10, (float)Y + 15);
        }

    }

    public class Pump : VVS
    {
        
    }

    public class HeatExchanger : VVS
    {
        
    }
    
}
