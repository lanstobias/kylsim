using System;
using System.Drawing;
using System.Windows.Forms;

namespace kylsim
{
    public class Kylsim
    {
        private VVS RootComponents = null;
        private VVS RootNodes = null;
        public Log LogFile = new Log();

        /// <summary>
        /// Initializes a new instance of the <see cref="Kylsim"/> class.
        /// </summary>
        public Kylsim()
        {
            const int ROW1 = 100;
            const int ROW2 = 200;
            const int ROW3 = 300;
            const int ROW4 = 400;
            const int ROW5 = 500;
            const int COL1 = 50;
            const int COL2 = 150;
            const int COL3 = 250;
            const int COL4 = 350;
            const int COL5 = 450;
            const int COL6 = 550;
            const int COL7 = 650;
            const int COL8 = 680;

            // Create Nodes
            Node node1 = new Node("N1", COL1, ROW5, 10, 10, 2, false, 0, null);
            Node node2 = new Node("N2", COL4, ROW5, 10, 10, 1, true, 0, node1);
            Node node3 = new Node("N3", COL7, ROW5, 10, 10, 1, true, 0, node2);
            Node node4 = new Node("N4", COL7, ROW3, 10, 10, 1, true, 0, node3);
            Node node5 = new Node("N5", COL5, ROW2, 10, 10, 1, true, 0, node4);
            Node node6 = new Node("N6", COL3, ROW2, 10, 10, 1, true, 0, node5);
            Node node7 = new Node("N7", COL1, ROW2, 10, 10, 2, false, 0, node6);
            Node node8 = new Node("N8", COL8, ROW1, 10, 10, 1, false, 0, node7);
            RootNodes = node8;

            // Create valves
            Valve valve1 = new Valve("V1", COL2, ROW5, 15, 10, 1, 10, node1, node2, null);
            Valve valve2 = new Valve("V2", COL6, ROW2, 15, 10, 1, 10, node5, node4, valve1);
            Valve valve3 = new Valve("V3", COL2, ROW2, 15, 10, 1, 10, node7, node6, valve2);
            Valve valve4 = new Valve("V4", COL5, ROW3, 15, 10, 1, 10, node6, node4, valve3);
            Valve valve5 = new Valve("V5", COL6, ROW1, 15, 10, 1, 10, node5, node8, valve4);

            // Create filters
            const int PUSH_UP = 10;
            Filter filter1 = new Filter("F1", COL4, ROW2 - PUSH_UP, 30, 20, 1, 0.0001, 10, node6, node5, valve5);

            // Create HeatExchangers
            const int PUSH_SIDE = 5;
            HeatExchanger heatExchanger1 = new HeatExchanger("HX1", COL7 + PUSH_SIDE, ROW4, 10, 20, 10, node4, node3, filter1);

            // Create Pump
            Pump pump1 = new Pump("P1", COL5+40, ROW5, 20, 1, 1, node2, node3, heatExchanger1);

            // Change to last component in the chain
            RootComponents = pump1;
        }

        /// <summary>
        /// Draws the specified canvas.
        /// </summary>
        /// <param name="canvas">The canvas.</param>
        public void Draw(Graphics canvas)
        {
            var next = RootNodes;
            while (next != null)
            {
                next.Draw(canvas);
                next = next.Next;
            }
            next = RootComponents;
            while (next != null)
            {
                next.Draw(canvas);
                next = next.Next;
            }
        }

        /// <summary>
        /// Displays the specified canvas.
        /// </summary>
        /// <param name="canvas">The canvas.</param>
        public void Display(Graphics canvas)
        {
            var next = RootNodes;
            while (next != null)
            {
                next.Display(canvas);
                next = next.Next;
            }
            next = RootComponents;
            while (next != null)
            {
                next.Display(canvas);
                next = next.Next;
            }
        }

        /// <summary>
        /// Dynamicses this instance.
        /// </summary>
        public void Dynamics(int dt)
        {
            var next = RootNodes;
            while (next != null)
            {
                next.Dynamics(dt);
                next = next.Next;
            }
            next = RootComponents;
            while (next != null)
            {
                next.Dynamics(dt);
                next = next.Next;
            }
        }

        /// <summary>
        /// Displays the menu.
        /// </summary>
        public void DisplayMenu(MouseEventArgs e, Control ctrl)
        {
            var next = RootComponents;
            while (next != null)
            {
                next.DisplayMenu(e.X, e.Y, ctrl);
                next = next.Next;
            }
        }
    }
}
