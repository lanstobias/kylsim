using System;
using System.Drawing;
using System.Windows.Forms;

namespace kylsim
{
    /// <summary>
    /// VVS
    /// </summary>
    public class VVS
    {
        public string Name { get; protected set; }
        public string LogFileName { get; set; }
        public float X { get; protected set; }
        public float Y { get; protected set; }
        public float W { get; protected set; }
        public float H { get; protected set; }
        public double gt { get; set; }

        public VVS Next { get; set; }

        // Graphics
        protected Font Font        = new Font("Courier", 8);
        protected Font FontBold    = new Font("Arial", 8, FontStyle.Bold);
        protected Brush Brush      = new SolidBrush(Color.Black);
        protected Brush GrayBrush  = new SolidBrush(Color.Gray);
        protected Brush RedBrush   = new SolidBrush(Color.Red);
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
            LogFileName = "kylsim.log";
        }

        /// <summary>
        /// Draws the specified canvas.
        /// </summary>
        /// <param name="canvas">The canvas.</param>
        public virtual void Draw(Graphics canvas) {}
        public virtual void Display(Graphics canvas) { }
        public virtual void Dynamics(int dt) { }
        public virtual void DisplayMenu(int clickX, int clickY, Control ctrl) { }
        public virtual bool clickInsideComponent(int clickX, int clickY) { return false; }
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
        public double SumFlow_old { get; private set; }
        public double PropConst { get; private set; }
        public double InError { get; private set; }

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
            PropConst = 0.05;
            InError = 0.001;
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
        public override void Dynamics(int dt)
        {
            if (Adjustable)
            {
                if (Math.Abs(SumFlow) > 0.1)
                {
                    if (SumFlow * SumFlow_old < 0 && Math.Abs(SumFlow) > Math.Abs(SumFlow_old))
                        PropConst *= 0.8;
                    else
                        PropConst *= 1.05;
                    if (PropConst * Math.Abs(SumFlow) > 0.8 * Pressure)
                        PropConst = 0.8 * Pressure / Math.Abs(SumFlow);
                    if (PropConst < 0.0001)
                        PropConst = 0.0001;
                }
                InError += PropConst * SumFlow;
                Pressure = SumFlow * PropConst * 0.25 + InError;
                if (Pressure < 0.001)
                    Pressure = 0.001;
            }
            SumFlow_old = SumFlow;
            SumFlow = 0;
        }

        /// <summary>
        /// Displays the specified canvas.
        /// </summary>
        /// <param name="canvas">The canvas.</param>
        public override void Display(Graphics canvas)
        {
            const string twoDecimals = "F1";
            canvas.DrawString(Pressure.ToString(twoDecimals), FontBold, Brush, (float)X + 45, (float)Y + 15);
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
        private bool Open { get; set; }
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
        public Valve(string name = "", float x = 0, float y = 0, float w = 15, float h = 10,
                     double position = 0, double admittance = 10, bool open = true, Node nodeIn = null, Node nodeOut = null, VVS next = null)
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
            Open = open;
            gt = 0.5;
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
            // Draw valve graphics
            canvas.DrawLine(ComponentPen, X, Y, X+W, Y+ H);
            canvas.DrawLine(ComponentPen, X, Y, X+W, Y - H);
            canvas.DrawLine(ComponentPen, X+W, Y+H, X + W, Y - H);
            canvas.DrawLine(ComponentPen, X,Y, X-W, Y - H);
            canvas.DrawLine(ComponentPen, X, Y, X-W, Y + H);
            canvas.DrawLine(ComponentPen, X-W,  Y-H, X - W, Y + H);

            // Draw lines
            if (X == NodeIn.X)
                canvas.DrawLine(LinePen, X, Y, NodeIn.X + (NodeIn.W + NodeIn.H) / 4, NodeIn.Y);
            else
            {
                canvas.DrawLine(LinePen, X, Y, NodeIn.X + (NodeIn.W + NodeIn.H) / 4, Y);
                canvas.DrawLine(LinePen, NodeIn.X + (NodeIn.W + NodeIn.H) / 4, Y, NodeIn.X + (NodeIn.W + NodeIn.H) / 4, NodeIn.Y);
            }
            if (X == NodeOut.X)
                canvas.DrawLine(LinePen, X, Y, NodeOut.X + (NodeOut.W + NodeOut.H) / 4, NodeOut.Y);
            else
            {
                canvas.DrawLine(LinePen, X, Y, NodeOut.X + (NodeOut.W + NodeOut.H) / 4, Y);
                canvas.DrawLine(LinePen, NodeOut.X + (NodeOut.W + NodeOut.H) / 4, Y, NodeOut.X + (NodeOut.W + NodeOut.H) / 4, NodeOut.Y);
            }

            // Draw text
            canvas.DrawString(Name, Font, Brush, (float)X + 10, (float)Y + -25);
            canvas.DrawString("vpos : ", Font, Brush, (float)X + 10, (float)Y + 15);
            canvas.DrawString("Flow : ", Font, Brush, (float)X + 10, (float)Y + 30);
            canvas.DrawString("gt : ", Font, Brush, (float)X + 10, (float)Y + 45);
        }

        /// <summary>
        /// Dynamicses this instance.
        /// </summary>
        public override void Dynamics(int dt)
        {
            //Check if valve is closed
            if (!Open && Math.Round(Position, 5) > 0)
            {
                Position -= (gt * dt) / 2000;
                if (Position < 0) Position = 0;
            }

            //Check if valve is open
            if (Open && Math.Round(Position, 5) < 1)
            { 
                Position += (gt * dt) / 2000;
                if (Position > 1) Position = 1;
            }
          
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
            canvas.DrawString(Position.ToString(twoDecimals), FontBold, Brush, (float)X + 45, (float)Y + 15);
            canvas.DrawString(Flow.ToString(twoDecimals), FontBold, Brush, (float)X + 45, (float)Y + 30);
            canvas.DrawString(gt.ToString(twoDecimals), FontBold, Brush, (float)X + 45, (float)Y + 45);
        }

        /// <summary>
        /// Displays the menu.
        /// </summary>
        /// <param name="menu">The menu.</param>
        public override void DisplayMenu(int clickX, int clickY, Control ctrl)
        {
            // Check if mouse click is inside the component box
            if (clickInsideComponent(clickX, clickY))
            {
                ContextMenu menu = new ContextMenu();
                menu.MenuItems.Add("Öppna", new EventHandler(menu_select_open));
                menu.MenuItems.Add("Stäng", new EventHandler(menu_select_close));
                menu.MenuItems.Add("Ändra gtidskonstant", new EventHandler(menu_select_gtid));
                menu.Show(ctrl, new Point(clickX, clickY));
            }
        }

        /// <summary>
        /// Handles the open event of the menu_select control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void menu_select_open(object sender, EventArgs e)
        {
            open();
        }

        /// <summary>
        /// Handles the close event of the menu_select control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void menu_select_close(object sender, EventArgs e)
        {
            close();
        }

        /// <summary>
        /// Handles the select gt
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menu_select_gtid(object sender, EventArgs e)
        {
            FormSetGt formSetGt = new FormSetGt(this);
            formSetGt.Show();
        }

        /// <summary>
        /// Clicks the inside component.
        /// </summary>
        /// <param name="clickX">The click x.</param>
        /// <param name="clickY">The click y.</param>
        /// <returns></returns>
        public override bool clickInsideComponent(int clickX, int clickY)
        {
            return ((clickX >= X - W && clickX <= X + W) && (clickY >= Y - H && clickY <= Y + H));
        }

        public void open()
        {
            if (!Open)
            {
                Open = true;
                Log.Write(LogFileName, "Open valve: " + Name);
            }
        }
        public void close()
        {
            if (Open)
            {
                Open = false;
                Log.Write(LogFileName, "Close valve: " + Name);
            }
        }

        public bool getOpenState()
        {
            return Open;
        }

    }

    /// <summary>
    /// Pump
    /// </summary>
    /// <seealso cref="kylsim.VVS" />
    public class Pump : VVS
    {
        private double Speed { get; set; }
        private double K { get; set; }
        private bool Open { get; set; }
        private double Flow { get; set; }
        public Node NodeIn { get; set; }
        public Node NodeOut { get; set; }
        public float R { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Pump"/> class.
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
        public Pump(string name = "", float x = 0, float y = 0, float r=40,
                     double speed = 0, double k = 1, Node nodeIn = null, Node nodeOut = null, VVS next = null)
        {
            Name = name;
            X = x;
            Y = y;
            R = r;
            W = R * 2;
            H = R * 2;
            NodeIn = nodeIn;
            NodeOut = nodeOut;
            Speed = speed;
            K = k;
            Next = next;
            Open = false;
            gt = 0.5;
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
            // Draw Pump graphics
            float PUSH_SIDE1 = (R / 4);
            float PUSH_SIDE2 = (R / 2) + (R / R);
            float PUSH_SIDE3 = PUSH_SIDE1 + (R / R);

            canvas.DrawEllipse(ComponentPen, X, Y - (R), W, H);
            canvas.DrawLine(ComponentPen, X + R * 2 - PUSH_SIDE1, Y, X + R * 2 - PUSH_SIDE2, Y + PUSH_SIDE3);
            canvas.DrawLine(ComponentPen, X + R * 2 - PUSH_SIDE1, Y, X + R * 2 - PUSH_SIDE2, Y - PUSH_SIDE3);

            // Draw lines
            canvas.DrawLine(LinePen, X, Y, NodeIn.X + (NodeIn.W + NodeIn.H) / 4, NodeIn.Y);
            canvas.DrawLine(LinePen, X, Y, NodeOut.X + (NodeOut.W + NodeOut.H) / 4, NodeOut.Y);

            // Draw text
            canvas.DrawString(Name, Font, Brush, (float)X + 20, (float)Y + - 35);
            canvas.DrawString("Speed : ", Font, Brush, (float)X + 10, (float)Y + 20);
            canvas.DrawString("Flow : ", Font, Brush, (float)X + 10, (float)Y + 35);
            canvas.DrawString("gt : ", Font, Brush, (float)X + 10, (float)Y + 50);
        }

        /// <summary>
        /// Dynamicses this instance.
        /// </summary>
        public override void Dynamics(int dt)
        {
            const double a = 5, b = 10;

            // Check if Pump is closed
            if (!Open && Math.Round(Speed, 5) > 0)
            {
                Speed -= (gt * dt) / 2000;
                if (Speed < 0) Speed = 0;
            }

            // Check if Pump is open
            if (Open && Math.Round(Speed, 5) < 1)
            {
                Speed += (gt * dt) / 2000;
                if (Speed > 1) Speed = 1;
            }

            // Calculate flow difference
            double PressureDifference;
            if (NodeIn.Pressure >= NodeOut.Pressure)
            {
                PressureDifference = (NodeIn.Pressure - NodeOut.Pressure);
                Flow = K * Speed * a * (b + (System.Math.Sqrt(PressureDifference)));
            }
            else
            {
                PressureDifference = (NodeOut.Pressure - NodeIn.Pressure);
                Flow = K * Speed * a * (b - (System.Math.Sqrt(PressureDifference)));
            }
            NodeIn.AddSumFlow(-Flow);
            NodeOut.AddSumFlow(Flow);

            limitflow(1.0);
        }

        /// <summary>
        /// Limitflows the specified limit.
        /// </summary>
        /// <param name="limit">The limit.</param>
        private void limitflow(double limit)
        {
            K = NodeIn.Pressure;
            if (K > limit)
                K = limit;
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
            canvas.DrawString(Speed.ToString(twoDecimals), FontBold, Brush, (float)X + 45, (float)Y + 20);
            canvas.DrawString(Flow.ToString(twoDecimals), FontBold, Brush, (float)X + 45, (float)Y + 35);
            canvas.DrawString(gt.ToString(twoDecimals), FontBold, Brush, (float)X + 45, (float)Y + 50);
        }

        /// <summary>
        /// Displays the menu.
        /// </summary>
        /// <param name="menu">The menu.</param>
        public override void DisplayMenu(int clickX, int clickY, Control ctrl)
        {
            // Check if mouse click is inside the component box
            if (clickInsideComponent(clickX, clickY))
            {
                ContextMenu menu = new ContextMenu();
                menu.MenuItems.Add("Starta", new EventHandler(menu_select_open));
                menu.MenuItems.Add("Stoppa", new EventHandler(menu_select_close));
                menu.MenuItems.Add("Ändra gtidskonstant", new EventHandler(menu_select_gtid));
                menu.Show(ctrl, new Point(clickX, clickY));
            }
        }

        /// <summary>
        /// Handles the open event of the menu_select control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void menu_select_open(object sender, EventArgs e)
        {
            if (!Open)
            {
                Open = true;
                Log.Write(LogFileName, "Open pump: " + Name);
            }
        }

        /// <summary>
        /// Handles the close event of the menu_select control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void menu_select_close(object sender, EventArgs e)
        {
            if (Open)
            {
                Open = false;
                Log.Write(LogFileName, "Close pump: " + Name);
            }
        }

        /// <summary>
        /// Handles the select gt
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menu_select_gtid(object sender, EventArgs e)
        {
            FormSetGt formSetGt = new FormSetGt(this);
            formSetGt.Show();
        }

        /// <summary>
        /// Clicks the inside component.
        /// </summary>
        /// <param name="clickX">The click x.</param>
        /// <param name="clickY">The click y.</param>
        /// <returns></returns>
        public override bool clickInsideComponent(int clickX, int clickY)
        {
            return ((clickX >= X && clickX <= X + W) && (clickY >= Y - H && clickY <= Y + H));
        }
    }

    /// <summary>
    /// HeatExchanger
    /// </summary>
    /// <seealso cref="kylsim.VVS" />
    public class HeatExchanger : VVS
    {
        private const double Position = 1;
        private double Admittance { get; set; }
        private double Flow { get; set; }
        public Node NodeIn { get; set; }
        public Node NodeOut { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="HeatExchanger"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <param name="w">The w.</param>
        /// <param name="h">The h.</param>
        /// <param name="admittance">The admittance.</param>
        /// <param name="nodeIn">The node in.</param>
        /// <param name="nodeOut">The node out.</param>
        /// <param name="next">The next.</param>
        public HeatExchanger(string name = "", float x = 0, float y = 0, float w = 10, float h = 20,
                      double admittance = 10, Node nodeIn = null, Node nodeOut = null, VVS next = null)
        {
            Name = name;
            X = x;
            Y = y;
            W = w;
            H = h;
            Admittance = admittance;
            NodeIn = nodeIn;
            NodeOut = nodeOut;
            Next = next;
        }

        /// <summary>
        /// Draws the specified canvas.
        /// </summary>
        /// <param name="canvas">The canvas.</param>
        public override void Draw(Graphics canvas)
        {

            // Draw HeatExchanger graphics
            canvas.DrawLine(ComponentPen, X - W, Y - H, X - W, Y + H); //vänsterlinje
            canvas.DrawLine(ComponentPen, X - W, Y - H, X + W, Y - H); //övrelinje
            canvas.DrawLine(ComponentPen, X - W, Y - H, X + W, Y - (H / 2 - H / 10)); //snedd linje 1
            canvas.DrawLine(ComponentPen, X + W, Y - (H / 2 - H / 10), X - W, Y + (H/10)); //snedd linje 2
            canvas.DrawLine(ComponentPen, X - W, Y + (H/10), X + W, Y + (H/2)); //snedd linje 3
            canvas.DrawLine(ComponentPen, X + W, Y + (H/2), X - W, Y + H ); //snedd linje 4

            canvas.DrawLine(ComponentPen, X + W, Y + H, X + W, Y - H); //högerlinje
            canvas.DrawLine(ComponentPen, X + W, Y + H, X - W, Y + H); //nedrelinje

            // Draw lines
            canvas.DrawLine(LinePen, X, Y, NodeIn.X + (NodeIn.W + NodeIn.H) / 4, NodeIn.Y);
            canvas.DrawLine(LinePen, X, Y, NodeOut.X + (NodeOut.W + NodeOut.H) / 4, NodeOut.Y);

            // Draw text
            canvas.DrawString(Name, Font, Brush, (float)X + 10, (float)Y + -25);
            canvas.DrawString("Flow : ", Font, Brush, (float)X + 10, (float)Y + 15);
        }

        /// <summary>
        /// Dynamicses this instance.
        /// </summary>
        public override void Dynamics(int dt)
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
        public override void Display(Graphics canvas)
        {
            const string twoDecimals = "F1";
            canvas.DrawString(Flow.ToString(twoDecimals), FontBold, Brush, (float)X + 45, (float)Y + 15);
        }
    }

    /// <summary>
    /// Filter
    /// </summary>
    public class Filter : VVS
    {
        private double Opening { get; set; }
        private double Flow { get; set; }
        private double Admittance { get; set; }
        private double G { get; set; }
        private bool Full = false;
        private double OpeningLimitWarning { get; set; }
        private double OpeningLimitCleaning { get; set; }
        private double OpeningLimit { get; set; }
        public Node NodeIn { get; set; }
        public Node NodeOut { get; set; }
        private bool RensaRed = false;
        private Valve Valves { get; set; }

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
        public Filter(string name = "", float x = 0, float y = 0, float w = 30, float h = 20,
                      double opening = 0, double g = 0, double admittance = 10,
                      Node nodeIn = null, Node nodeOut = null, VVS next = null,  Valve valves = null)
        {
            Name = name;
            X = x;
            Y = y;
            W = w;
            H = h;
            Opening = opening;
            G = g;
            NodeIn = nodeIn;
            NodeOut = nodeOut;
            Next = next;
            Admittance = admittance;
            Valves = valves;
            OpeningLimitWarning = 0.5;
            OpeningLimitCleaning = 0.2;
            OpeningLimit = 1;
        }

        public Valve getValve(string name)
        {
            Valve tempValve = Valves;
            while (tempValve.Next != null)
            {
                if (tempValve.Name == name)
                    return tempValve;

                tempValve = (Valve)tempValve.Next;
            }
            return null;
        }

        public void flipValve(Valve valve)
        {
            if (valve.getOpenState())
                valve.close();
            else valve.open();
        }
        
        /// <summary>
        /// Draws the specified canvas.
        /// </summary>
        /// <param name="canvas">The canvas.</param>
        public override void Draw(Graphics canvas)
        {
            // Left wall / Right wall
            canvas.DrawLine(ComponentPen, X,     Y, X,     Y + H);
            canvas.DrawLine(ComponentPen, X + W, Y, X + W, Y + H);

            // Top / Bottom
            canvas.DrawLine(ComponentPen, X, Y,     X + W, Y    );
            canvas.DrawLine(ComponentPen, X, Y + H, X + W, Y + H);

            // Inside
            canvas.DrawLine(ComponentPen, X + 5, Y, X + 5, Y + (H - 5));
            canvas.DrawLine(ComponentPen, X + (W - 5), Y, X + (W - 5), Y + (H - 5));
            canvas.DrawLine(ComponentPen, X + (W/2), Y + H, X + (W/2), Y + 5);

            // Draw lines
            canvas.DrawLine(LinePen, X + (W / 2), Y + (H/2), NodeIn.X + ((NodeIn.W + NodeIn.H) / 4), NodeIn.Y);
            canvas.DrawLine(LinePen, X + (W / 2), Y + (H /2), NodeOut.X + ((NodeOut.W + NodeOut.H) / 4), NodeOut.Y);

            // Draw text
            canvas.DrawString(Name, Font, Brush, (float)X + 10, (float)Y - 15);
            canvas.DrawString("vpos : ", Font, Brush, (float)X + 10, (float)Y + 25);
            canvas.DrawString("Flow : ", Font, Brush, (float)X + 10, (float)Y + 40);

            DrawRensa(canvas);
        }

        /// <summary>
        /// Dynamicses this instance.
        /// </summary>
        public override void Dynamics(int dt)
        {
            calculateFlowDifference();
            nodeFlowAdjust();
            fouling();
            fullWarningCheck();
            clearingCheck();
            fullCheck();

        }
        public void openingLimiter()
        {
                double openingTemp;
                openingTemp= Opening - G * Flow;
                if (openingTemp > OpeningLimit)
                    Opening = OpeningLimit;
                else Opening = openingTemp;
            
        }
        public void nodeFlowAdjust()
        {
            NodeIn.AddSumFlow(-Flow);
            NodeOut.AddSumFlow(Flow);
        }
        public void fullCheck()
        {
            if (Opening >= OpeningLimit && Full)
            {
                Full = false;
                normalizeFlow();
            }
        }
        public void clearingCheck()
        {
            if (Opening <= OpeningLimitCleaning)
                clearFilter();
        }
        public void fullWarningCheck()
        {
            if (Opening <= OpeningLimitWarning && !Full)
                Full = true;
        }
        public void calculateFlowDifference()
        {
            double PressureDifference;
            if (NodeIn.Pressure >= NodeOut.Pressure)
            {
                PressureDifference = (NodeIn.Pressure - NodeOut.Pressure);
                Flow = Admittance * Opening * (System.Math.Sqrt(PressureDifference));
            }
            else
            {
                PressureDifference = (NodeOut.Pressure - NodeIn.Pressure);
                Flow = (-Admittance) * Opening * (System.Math.Sqrt(PressureDifference));
            }
        }
        public void fouling()
        {
            if (Opening >= 0 && Opening <= OpeningLimit)
            {
                openingLimiter();
            }
        }
        public void clearFilter()
        {
            getValve("V4").open();
            getValve("V5").open();
            getValve("V2").close();
        }
        public void normalizeFlow()
        {
            getValve("V4").close();
            getValve("V5").close();
            getValve("V2").open();
        }

        /// <summary>
        /// Displays the specified canvas.
        /// </summary>
        /// <param name="canvas">The canvas.</param>
        public override void Display(Graphics canvas)
        {
            const string oneDecimal = "F1";
            const string twoDecimals = "F2";
            canvas.DrawString(Opening.ToString(twoDecimals), FontBold, Brush, (float)X + 45, (float)Y + 25);
            canvas.DrawString(Flow.ToString(oneDecimal), FontBold, Brush, (float)X + 45, (float)Y + 40);
        }

        public void DrawRensa(Graphics canvas)
        {
            string str = "Rensa";
            if (!Full)
            {
                canvas.DrawString(str, Font, GrayBrush, (float)X + 10, (float)Y - 35);
            }
            else if (Full)
            {
                if (RensaRed)
                {
                    canvas.DrawString(str, Font, RedBrush, (float)X + 10, (float)Y - 35);
                    RensaRed = false;
                }
                else
                {
                    canvas.DrawString(str, Font, GrayBrush, (float)X + 10, (float)Y - 35);
                    RensaRed = true;
                }
            }
        }
    }
}
