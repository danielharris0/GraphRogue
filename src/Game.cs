public class Action { }

public abstract class Node {

    public List<Edge> inEdges = new List<Edge> ();
    public List<Edge> outEdges = new List<Edge> ();

    public abstract String GetName();
    public abstract String GetInfo();
    public virtual List<Action> GetActions() {
        return new List<Action>();
    }
}

public abstract class Edge {
    public Node nodeFrom;
    public Node nodeTo;

    public abstract String GetName();
    public abstract String GetInfo();
    public virtual List<Action> GetActions() {
        return new List<Action>();
    }
}

static class Game {
    public static void TakeAction(Action action) { }
    public static void Load() {
        int seed = 0;
        Node root = Content.Generator.Generate(seed);
        Program.userInterface.RegenerateGraph(root);
    }

    public static void Link(Node nodeFrom, Edge edge, Node nodeTo) {
        edge.nodeFrom = nodeFrom; edge.nodeTo = nodeTo;
        nodeFrom.outEdges.Add(edge);
        nodeTo.inEdges.Add(edge);
    }
}