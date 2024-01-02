using GUI;
using System.Numerics;
using static Raylib_cs.Raylib;
using static GUI.Container;

public class UserInterface {

    //Interfaces between a static game state and the GUI Engine

    private UpdateNotifier updateNotifier;
    private Container graphContainer;

    public Container edgeContainer;
    public Container nodeContainer;

    public UserInterface(UpdateNotifier updateNotifier) { this.updateNotifier = updateNotifier; }

    public void Load() {
        graphContainer = new Container(updateNotifier);
        Program.windowContainer.Add(graphContainer, ContainerBoxChangeListeners.MATCH_BOX);

        edgeContainer = new Container(updateNotifier);
        graphContainer.Add(edgeContainer, ContainerBoxChangeListeners.MATCH_BOX);
        nodeContainer = new Container(updateNotifier);
        graphContainer.Add(nodeContainer, ContainerBoxChangeListeners.MATCH_BOX);
    }

    public void RegenerateGraph(Node root) {
        //from root of perception
        //nodes displayed *are* real nodes, but may be 'illusory', or have 'illusory' attributes / connections

        //atm we will generate:
            // -- the whole graph (no filtering/cutoff)
            // -- the true graph (no hidden nodes or hidden edges)


        Random random = new Random();

        Vector2 randomVector() {
            double angle = random.NextDouble() * Math.PI * 2;
            return new Vector2((float) Math.Cos(angle), (float) Math.Sin(angle));
        }

        void explore(VisualNode visualNode) {

            //Explore outgoing edges
            foreach (Edge gameEdge in visualNode.gameNode.outEdges) {

                //Test if the edge has already been added to the visual node
                bool found = false;
                foreach (VisualEdge visualEdge in visualNode.outEdges) {
                    if (visualEdge.gameEdge == gameEdge) found = true;
                }

                if (!found) { //Then create a corresponding visual edge
                    VisualNode visualNodeB = new VisualNode(updateNotifier, gameEdge.nodeTo, visualNode.pos + randomVector() * 300);
                    VisualEdge visualEdge = new VisualEdge(updateNotifier, gameEdge, visualNode, visualNodeB);
                    visualNode.outEdges.Add(visualEdge);
                    visualNodeB.inEdges.Add(visualEdge);
                    explore(visualNodeB);
                }
            }

            //Explore incoming edges
            foreach (Edge gameEdge in visualNode.gameNode.inEdges) {

                //Test if the edge has already been added to the visual node
                bool found = false;
                foreach (VisualEdge visualEdge in visualNode.inEdges) {
                    if (visualEdge.gameEdge == gameEdge) found = true;
                }

                if (!found) { //Then create a corresponding visual edge
                    VisualNode visualNodeA = new VisualNode(updateNotifier, gameEdge.nodeFrom, visualNode.pos + randomVector() * 300);
                    VisualEdge visualEdge = new VisualEdge(updateNotifier, gameEdge, visualNodeA, visualNode);
                    visualNode.inEdges.Add(visualEdge);
                    visualNodeA.outEdges.Add(visualEdge);
                    explore(visualNodeA);
                }
            }

        }
        VisualNode visualRoot = new VisualNode(updateNotifier, root, Vector2.Zero);
        explore(visualRoot);


        void tickAllNodes() {
            float heat = 0;
            foreach (ContentInfo ci in nodeContainer.contents) {
                VisualNode node = (VisualNode)ci.component;
                heat += node.TickForces();
            }
            if (heat / nodeContainer.contents.Count > 0) tickAllNodes();
        }

        tickAllNodes();
       

    }
    public void OnNodeSelected() {

    }
    public void OnEdgeSelected() {

    }
}