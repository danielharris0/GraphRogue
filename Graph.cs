using Raylib_cs;
using System.Numerics;
using static Raylib_cs.Raylib;

public class NodeType {
    public string name;
    public Color colour;
    public NodeType(string name, Color colour) { this.name = name; this.colour = colour; }

}

public class EdgeType {
    public string name;
    public Color colour;
    public EdgeType(string name, Color colour) { this.name = name; this.colour = colour; }
}

public class Edge {
    public Node fromNode; public Node toNode;
    public Vector2 fromPos; public Vector2 toPos;

    public bool hidden = false;
    public EdgeType type;

    public void Draw() {
        Program.DrawArrowOutlined(fromPos, toPos, type.colour, Color.Black, 5, 10, 2);

        Vector2 middle = (fromPos + toPos) / 2;
        Vector2 labelSize = new Vector2(50, 20);
        DrawRectangleRounded(new Rectangle(middle - labelSize/2, labelSize), 0.3f, 5, new Color(0,0,0,100));

    }

    public void DrawOverlay() {
        Vector2 middle = (fromPos + toPos) / 2;
        Vector2 size = MeasureTextEx(Program.font, type.name, 24, 0);
        DrawTextEx(Program.font, type.name, middle - size/2, 24, 0, type.colour);
        
    }

    public void UpdateGraphics() {

    }

    public void CalculateEndpoints() {
        Vector2 a = Program.GetMiddle(fromNode.rectangle); Vector2 b = Program.GetMiddle(toNode.rectangle);
        Vector2? x;
        x = Maths.IntersectLineWithRect(a, b, fromNode.rectangle);
        fromPos = x == null ? a : (Vector2) x;
        x = Maths.IntersectLineWithRect(a, b, toNode.rectangle);
        toPos = x == null ? b : (Vector2) x;
    }
}

public class Label {

}

public class Node {
    public bool hidden = false;
    public List<Edge> edges = new List<Edge>();
    public NodeType type;
    public Rectangle rectangle;

    public List<Label> labels;

    public bool hovered = false;
    public bool dragging = false;

    private Vector2 grabOffset = Vector2.Zero;

    private float textFontSize;
    private Vector2 textOffset;

    public Node(NodeType type, Rectangle rectangle) {
        this.type = type;
        this.rectangle = rectangle;

        textFontSize = 100;
        while (Utils.Larger(MeasureTextEx(Program.font, type.name, textFontSize, 1f), rectangle.Size)) { textFontSize -= 1; };
        textOffset = (rectangle.Size - MeasureTextEx(Program.font, type.name, textFontSize, 1f))/2;
    }

    public void Draw() {
        DrawRectangleRounded(rectangle, 0.1f, 5, type.colour);
        DrawRectangleRoundedLines(rectangle, 0.1f, 5, hovered ? 6 : 3, Color.Black);
    }

    public void UpdateGraphics() {
        hovered = Program.InRect(Camera.ScreenToWorld(GetMousePosition()), rectangle);
        if (hovered && IsMouseButtonPressed(0)) {
            dragging = true;
            grabOffset = rectangle.Position - Camera.ScreenToWorld(GetMousePosition());
        }
        if (IsMouseButtonReleased(0)) dragging = false;
        if (dragging) {
            rectangle.Position = Camera.ScreenToWorld(GetMousePosition()) + grabOffset;
            foreach (Edge edge in edges) edge.CalculateEndpoints();
        }

        if (hovered && IsMouseButtonPressed(MouseButton.Right)) {
           // Node n = Graph.Add(type, new Vector2(10, 10));
           // Graph.Connect(edges[0].type, this, n);
            Program.panelWidth = (Program.panelWidth + 100) % Program.screenSize.X;
        }
    }

    public void DrawOverlay() {
        DrawTextEx(Program.font, type.name, rectangle.Position + textOffset, textFontSize, 1, Color.Black);
    }
}

public class Graph {
    public List<Node> nodes = new List<Node>();
    public List<Edge> edges = new List<Edge>();

    public void UpdateGraphics() {
        foreach (Edge edge in edges) {
            if (!edge.hidden) edge.UpdateGraphics();
        }
        foreach (Node node in nodes) {
            if (!node.hidden) node.UpdateGraphics();
        }
    }

    public void Draw() {
        foreach (Node node in nodes) {
            if (!node.hidden) node.Draw();
        }
        foreach (Edge edge in edges) {
            if (!edge.hidden) edge.Draw();
        }
        foreach (Node node in nodes) {
            if (!node.hidden) node.DrawOverlay();
        }
        foreach (Edge edge in edges) {
            if (!edge.hidden) edge.DrawOverlay();
        }
    }

    public Node Add(NodeType type, Vector2 position, float size = 1) {
        Rectangle rectangle = new Rectangle(position * Program.cellSize * size, new Vector2(2, 1) * Program.cellSize * size);
        Node node = new Node(type, rectangle);
        nodes.Add(node);
        return node;
    }



    public void Connect(EdgeType type, Node fromNode, Node toNode) {
        Edge edge = new Edge();
        edge.type = type; edge.fromNode = fromNode; edge.toNode = toNode;
        edge.CalculateEndpoints();
        fromNode.edges.Add(edge);
        toNode.edges.Add(edge);
        edges.Add(edge);
    }

    public void GenDemo() {
        Node p1 = Add(Program.nodeTypes["Person"], new Vector2(10,10));
        Node p2 = Add(Program.nodeTypes["Person"], new Vector2(20, 10));
        Node s1 = Add(Program.nodeTypes["Sword"], new Vector2(15, 20));
        Node s2 = Add(Program.nodeTypes["Sword"], new Vector2(15, 15));

        Connect(Program.edgeTypes["loves"], p1, p2);
        Connect(Program.edgeTypes["has"], p1, s1);
        Connect(Program.edgeTypes["loves"], p2, s1);
        Connect(Program.edgeTypes["has"], p2, s2);
        Connect(Program.edgeTypes["has"], s2, s1);
        Connect(Program.edgeTypes["loves"], s2, p1);
    }
}
