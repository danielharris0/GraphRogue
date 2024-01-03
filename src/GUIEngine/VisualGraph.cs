using GUI;
using System.Numerics;
using Raylib_cs;
using static GUI.Container;

class VisualNode : GUIComponent {
    public List<VisualEdge> inEdges = new List<VisualEdge>();
    public List<VisualEdge> outEdges = new List<VisualEdge>();
    public Node gameNode;

    public Vector2 pos; //pos within the graph
    public String name;

    class BoxListenerForNode : Container.BoxListener {
        public override void OnChange() {
            component.Reposition(container.box.pos + ((VisualNode) component).pos);
        }
    }

    public VisualNode(UpdateNotifier notifier, Node gameNode, Vector2 pos) : base(notifier) {
        Resize(Vector2.One * 100);
        this.pos = pos;

        //Expand gamenode's implicit GUI functionality into stored data
        this.gameNode = gameNode;
        name = gameNode.GetName();

        Program.userInterface.nodeContainer.Add(this, new BoxListenerForNode());
    }

    public override void Draw() {
        Raylib.DrawRectangleRounded(new Rectangle(box.pos.X - box.size.X/2, box.pos.Y - box.size.Y / 2, box.size.X, box.size.Y ), 0.1f, 5, Color.RED);
        Raylib.DrawRectangleRoundedLines(new Rectangle(box.pos.X - box.size.X/2, box.pos.Y - box.size.Y / 2, box.size.X, box.size.Y), 0.1f, 5, 5, Color.BLACK);
        Raylib.DrawText(name, (int) (box.pos.X - box.size.X / 2), (int) (box.pos.Y - box.size.Y / 2), 30, Color.GREEN);
    }

    float currentHeat = 0;

    public float TickForces() { //Return current heat

        currentHeat += 1;

        //If not in-bounds, go towards center
        if (pos.X<0 || pos.Y<0 || pos.X>contentInfo.container.box.size.X || pos.Y > contentInfo.container.box.size.Y) {
            Console.WriteLine("OOB");
            Vector2 dir = contentInfo.container.GetCenter() - pos;
            pos += currentHeat * dir / dir.Length();
        }

        //If touching node, go away
        else {
            VisualNode? touchedNode = null;
            foreach (ContentInfo ci in Program.userInterface.nodeContainer.contents) {
                VisualNode node = (VisualNode) ci.component;
                if (node != this && Raylib.CheckCollisionRecs(GetRectangle(), node.GetRectangle())) {
                    touchedNode = node;
                    break;
                }
            }
            if (touchedNode!=null) {
                Console.WriteLine("Touching");
                Vector2 dir = touchedNode.pos - pos;
                pos += currentHeat * dir / dir.Length();
            } else {
                Console.WriteLine("Cooling");
                currentHeat = Math.Max(currentHeat - 2, 0);
            }
        }

        Reposition(pos + contentInfo.container.box.pos);
        return currentHeat;
    }
}

class VisualEdge : GUIComponent {
    public VisualNode nodeFrom;
    public VisualNode nodeTo;
    public Edge gameEdge;

    public VisualEdge(UpdateNotifier notifier, Edge gameEdge, VisualNode nodeFrom, VisualNode nodeTo) : base(notifier) {
        this.gameEdge = gameEdge; this.nodeFrom = nodeFrom; this.nodeTo = nodeTo;
        Program.userInterface.edgeContainer.Add(this, new Container.BoxListeners.None());
    }

    public override void Draw() {
        Raylib.DrawLineEx(nodeFrom.box.pos, nodeTo.box.pos, 5, Color.BLUE);
    }

}