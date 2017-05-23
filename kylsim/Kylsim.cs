using System;
using System.Drawing;
using System.Windows.Forms;

namespace kylsim
{
    public class Kylsim
    {
        private VVS RootComponents = null;
        private VVS RootNodes = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="Kylsim"/> class.
        /// </summary>
        public Kylsim()
        {
            const int ROW1 = 100;
            const int ROW2 = 200;
            const int ROW3 = 300;
            const int ROW4 = 400;
            const int COL1 = 100;
            const int COL2 = 200;
            const int COL3 = 300;
            const int COL4 = 400;
            const int COL5 = 500;
            const int COL6 = 600;

            // Create Nodes
            Node node1 = new Node("N1", COL1, ROW2, 10, 10, 5, false, 0, null);
            Node node2 = new Node("N2", COL3, ROW2, 10, 10, 1, true, 0, node1);
            Node node3 = new Node("N3", COL5, ROW2, 10, 10, 1, true, 0, node2);
            Node node4 = new Node("N4", COL1, ROW3, 10, 10, 1, false, 0, node3);
            Node node5 = new Node("N5", COL2, ROW3, 10, 10, 1, false, 0, node4);
            Node node6 = new Node("N6", COL3, ROW3, 10, 10, 1, false, 0, node5);
            Node node7 = new Node("N7", COL4, ROW4, 10, 10, 1, false, 0, node6);
            Node node8 = new Node("N8", COL5, ROW4, 10, 10, 1, false, 0, node7);
            RootNodes = node8;

            //Create valves
            Valve valve1 = new Valve("V1", COL2, ROW2, 15, 10, 1, 10, node1, node2, null);
            Valve valve2 = new Valve("V2", COL4, ROW2, 15, 10, 1, 10, node2, node3, valve1);

            //Create filters
            const int PUSH_UP = 10;
            Filter filter1 = new Filter("F1", COL4, ROW2 - PUSH_UP, 30, 20, 1, 0.0001, 10, node2, node3, valve1);

            //Create HeatExchangers
            const int PUSH_SIDE = 5;
            HeatExchanger heatExchanger1 = new HeatExchanger("HX1", COL5 + PUSH_SIDE, ROW3, 10, 20, 10, node3, node8, filter1);

            // Change to last component in the chain
            RootComponents = heatExchanger1;
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
        public void Dynamics()
        {
            var next = RootNodes;
            while (next != null)
            {
               
                next.Dynamics();
                next = next.Next;
            }
            next = RootComponents;
            while (next != null)
            {
                next.Dynamics();
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
