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

        public VVS Next { get; set; }

        // Graphics
        protected Font Font        = new Font("Courier", 8);
        protected Brush Brush      = new SolidBrush(Color.Black);
        protected Pen LinePen      = new Pen(Color.Blue);
        protected Pen ComponentPen = new Pen(Color.Red);

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

        /// <summary>
        /// Draws the specified canvas.
        /// </summary>
        /// <param name="canvas">The canvas.</param>
        public virtual void Draw(Graphics canvas) {}
        public virtual void Display(Graphics canvas) { }
        public virtual void Dynamics() { }
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
                    double pressure = 0, bool adjustable = false,
                    double sumFlow = 0, VVS next = null)
        {
            Name = name;
            X = x;
            Y = y;
            W = w;
            H = h;
            Pressure   = pressure;
            Adjustable = adjustable;
            SumFlow    = sumFlow;
            Next = next;
        }

        /// <summary>
        /// Draws the specified canvas.
        /// </summary>
        /// <param name="canvas">The canvas.</param>
        public override void Draw(Graphics canvas) 
        {
            // Draw ellipse
            canvas.DrawEllipse(ComponentPen, X, Y-((W+H)/4), W, H);

            // Draw text
            canvas.DrawString(Name, Font, Brush, (float)X + 10, (float)Y + -20);
            canvas.DrawString("Tryck: ", Font, Brush, (float)X + 10, (float)Y + 15);
        }

        /// <summary>
        /// Adds the sumflode.
        /// </summary>
        /// <param name="inFlow">The in flow.</param>
        /// <param name="outFlow">The out flow.</param>
        /// <returns></returns>
        public void AddSumFlow(double Flow)
        {
            SumFlow += Flow;
        }

        /// <summary>
        /// Dynamicses this instance.
        /// </summary>
        public override void Dynamics()
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
        public override void Display(Graphics canvas)
        {
            const string twoDecimals = "F1";
            canvas.DrawString(Pressure.ToString(twoDecimals), Font, Brush, (float)X + 45, (float)Y + 15);
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
        public Node NodeIn { get; set; }
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
                     double position = 0, double admittance = 10, Node nodeIn = null, Node nodeOut = null, VVS next = null)
        {
            Name = name;
            X = x;
            Y = y;
            W = w;
            H = h;
            NodeIn     = nodeIn;
            NodeOut    = nodeOut;
            Position   = position;
            Admittance = admittance;
            Next = next;
        }

        /// <summary>
        /// Draws the specified canvas.
        /// </summary>
        /// <param name="canvas">The canvas.</param>
        /// <param name="brush">The brush.</param>
        /// <param name="font">The font.</param>
        /// <param name="pen">The pen.</param>
        public override void Draw(Graphics canvas)
        {
            const int Ygrad = 10;
            const int Xgrad = 15;
       
            // Draw valve graphics
            canvas.DrawLine(ComponentPen, X, Y, X+Xgrad, Y+ Ygrad);
            canvas.DrawLine(ComponentPen, X, Y, X+Xgrad, Y - Ygrad);
            canvas.DrawLine(ComponentPen, X+Xgrad, Y+Ygrad, X + Xgrad, Y - Ygrad);
            canvas.DrawLine(ComponentPen, X,Y, X-Xgrad, Y - Ygrad);
            canvas.DrawLine(ComponentPen, X, Y, X-Xgrad, Y + Ygrad);
            canvas.DrawLine(ComponentPen, X-Xgrad,  Y-Ygrad, X - Xgrad, Y + Ygrad);

            // Draw lines
            canvas.DrawLine(LinePen, X, Y, NodeIn.X + (NodeIn.W + NodeIn.H)/4, NodeIn.Y);
            canvas.DrawLine(LinePen, X, Y, NodeOut.X + (NodeOut.W + NodeOut.H)/4, NodeOut.Y);

            // Draw text
            canvas.DrawString(Name, Font, Brush, (float)X + 10, (float)Y + -25);
            canvas.DrawString("vpos : ", Font, Brush, (float)X + 10, (float)Y + 15);
            canvas.DrawString("Flow : ", Font, Brush, (float)X + 10, (float)Y + 30);
        }

        /// <summary>
        /// Dynamicses this instance.
        /// </summary>
        public override void Dynamics()
        {
            // Calculate flow difference
            double PressureDifference;
            if (NodeIn.Pressure >= NodeOut.Pressure)
            {
                PressureDifference = (NodeIn.Pressure - NodeOut.Pressure);
                Flow = Admittance * Position * (System.Math.Sqrt(PressureDifference));
            }
            else
            {
                PressureDifference = (NodeOut.Pressure - NodeIn.Pressure);
                Flow = (-Admittance) * Position * (System.Math.Sqrt(PressureDifference));
            }
            NodeIn.AddSumFlow(-Flow);
            NodeOut.AddSumFlow(Flow);
        }

        /// <summary>
        /// Displays the specified canvas.
        /// </summary>
        /// <param name="canvas">The canvas.</param>
        /// <param name="brush">The brush.</param>
        /// <param name="font">The font.</param>
        public override void Display(Graphics canvas)
        {
            const string twoDecimals = "F1";
            canvas.DrawString(Position.ToString(twoDecimals), Font, Brush, (float)X + 45, (float)Y + 15);
            canvas.DrawString(Flow.ToString(twoDecimals), Font, Brush, (float)X + 45, (float)Y + 30);
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

    /// <summary>
    /// Filter
    /// </summary>
    public class Filter
    public class Filter : VVS
    {
        private double Opening { get; set; }
        private double Flow { get; set; }
        private double G { get; set; }
        public Node NodeIn { get; set; }
        public Node NodeOut { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Filter"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <param name="w">The w.</param>
        /// <param name="h">The h.</param>
        /// <param name="position">The position.</param>
        /// <param name="admittance">The admittance.</param>
        /// <param name="nodeIn">The node in.</param>
        /// <param name="nodeOut">The node out.</param>
        /// <param name="next">The next.</param>
        public Filter(string name = "", float x = 0, float y = 0, float w = 0, float h = 0,
                      double opening = 0, double g = 0,
                      Node nodeIn = null, Node nodeOut = null, VVS next = null)
        {
            Name = name;
            X = x;
            Y = y;
            W = w;
            H = h;
            Opening = opening;
            G = g;
            NodeIn  = nodeIn;
            NodeOut = nodeOut;
            Next    = next;
        }

    }
}
