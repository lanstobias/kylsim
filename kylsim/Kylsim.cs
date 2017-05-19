using System;
using System.Drawing;

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
            // Create Nodes
            Node node1 = new Node("N1", 100, 200, 10, 10, 5, false, 0, null);
            Node node2 = new Node("N2", 300, 200, 10, 10, 1, true, 0, node1);
            Node node3 = new Node("N3", 500, 200, 10, 10, 1, false, 0, node2);
            RootNodes = node3;

            //Create valves
            Valve valve1 = new Valve("V1", 200, 200, 10, 10, 1, 10, node1, node2, null);
            Valve valve2 = new Valve("V2", 400, 200, 10, 10, 1, 10, node2, node3, valve1);
            RootComponents = valve2;
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
    }
}
