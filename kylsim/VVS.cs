using System;
using System.Drawing;

namespace kylsim
{
    /// <summary>
    /// VVS
    /// </summary>
    public class VVS
    {
        public string Name { get; protected set; }
        public float X { get; protected set; }
        public float Y { get; protected set; }
        public float W { get; protected set; }
        public float H { get; protected set; }

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
        protected VVS(string name = "", float x = 0, float y = 0, float w = 0, float h = 0)
        {
            Name = name;
            X = x;
            Y = y;
            W = w;
            H = h;
        }
    }
    /// <summary>
    /// Node
    /// </summary>
    /// <seealso cref="kylsim.VVS" />
    public class Node : VVS
    {
        public double Pressure { get; private set; }
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
                    double pressure = 0, bool adjustable = false, double sumFlow = 0)
        {
            Name = name;
            X = x;
            Y = y;
            W = w;
            H = h;
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
            canvas.DrawEllipse(pen, X, Y-((W+H)/4), W, H);

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
        public void AddSumFlow(double Flow)
        {
            SumFlow +=Flow;
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
    /// <summary>
    /// Valve
    /// </summary>
    /// <seealso cref="kylsim.VVS" />
    public class Valve : VVS
    {
        private double Position { get; set; }
        private double Admittance { get; set; }
        private double Flow { get; set; }
        /// <summary>
        /// Gets or sets the node in.
        /// </summary>
        /// <value>
        /// The node in.
        /// </value>
        public Node NodeIn { get; set; }
        /// <summary>
        /// Gets or sets the node out.
        /// </summary>
        /// <value>
        /// The node out.
        /// </value>
        public Node NodeOut { get; set; }

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
                     double position = 0, double admittance = 10, Node nodeIn = null, Node nodeOut = null)
        {
            Name = name;
            X = x;
            Y = y;
            W = w;
            H = h;
            NodeIn = nodeIn;
            NodeOut = nodeOut;
            Position = position;
            Admittance = admittance;
        }

        /// <summary>
        /// Draws the specified canvas.
        /// </summary>
        /// <param name="canvas">The canvas.</param>
        /// <param name="brush">The brush.</param>
        /// <param name="font">The font.</param>
        /// <param name="pen">The pen.</param>
        public void Draw(Graphics canvas, Brush brush, Font font, Pen componentPen, Pen linePen)
        {
            const int Ygrad = 10;
            const int Xgrad = 15;
       
            // Draw valve graphics
            canvas.DrawLine(componentPen, X, Y, X+Xgrad, Y+ Ygrad);
            canvas.DrawLine(componentPen, X, Y, X+Xgrad, Y - Ygrad);
            canvas.DrawLine(componentPen, X+Xgrad, Y+Ygrad, X + Xgrad, Y - Ygrad);
            canvas.DrawLine(componentPen, X,Y, X-Xgrad, Y - Ygrad);
            canvas.DrawLine(componentPen, X, Y, X-Xgrad, Y + Ygrad);
            canvas.DrawLine(componentPen, X-Xgrad,  Y-Ygrad, X - Xgrad, Y + Ygrad);

            // Draw lines
            canvas.DrawLine(linePen, X, Y, NodeIn.X + (NodeIn.W + NodeIn.H)/4, NodeIn.Y);
            canvas.DrawLine(linePen, X, Y, NodeOut.X + (NodeOut.W + NodeOut.H)/4, NodeOut.Y);

            // Draw text
            canvas.DrawString(Name, font, brush, (float)X + 10, (float)Y + -20);
            canvas.DrawString("Jag är en fin ventil :3 ", font, brush, (float)X + 10, (float)Y + 15);
        }
        /// <summary>
        /// Dynamicses this instance.
        /// </summary>
        public void Dynamics()
        {
            //Beräkna tryckskillnaden mellan NodeIn och NodeOut
            double PressureDifference;
            if(NodeIn.Pressure>=NodeOut.Pressure)
            {
                PressureDifference = (NodeIn.Pressure - NodeOut.Pressure);
                Flow = Admittance * Position * (System.Math.Sqrt(PressureDifference));
            }
            else
            {
                PressureDifference = (NodeOut.Pressure - NodeIn.Pressure);
                Flow=(-Admittance) * Position * (System.Math.Sqrt(PressureDifference));
            }
            NodeIn.AddSumFlow(-Flow);
            NodeIn.AddSumFlow(Flow);
        }
    }
    /// <summary>
    /// Pump
    /// </summary>
    /// <seealso cref="kylsim.VVS" />
    public class Pump : VVS
    {
        
    }
    /// <summary>
    /// HeatExchanger
    /// </summary>
    /// <seealso cref="kylsim.VVS" />
    public class HeatExchanger : VVS
    {
        
    }
    
}
