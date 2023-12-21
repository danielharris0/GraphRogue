using Raylib_cs;
using System.Numerics;

namespace UIEngine {

	public abstract class NodeGraphItem {
		public NodeGraphComponent graph;
		public Vector2 posInGraph;
		public virtual void ChangePos(Vector2 p) { }
		public abstract void Update();
		public abstract void Draw();
	}

    public class Node : NodeGraphItem {

		const int radius = 50;
		const int lineThickness = 5;

		public Vector2 pos;

		public Node(Vector2 posInGraph) { this.posInGraph = posInGraph; }

		public override void ChangePos(Vector2 p) { pos = p; }

        public override void Draw() {
			Raylib.DrawCircleV(pos, radius + lineThickness, Color.BLACK);
			Raylib.DrawCircleV(pos, radius, Color.RED);
        }

        public override void Update() {

        }
    }

    public class Edge : NodeGraphItem {

		const int thickness = 5;

		Node nodeA;
		Node nodeB;

		public Edge(Node nodeA, Node nodeB) {
            this.nodeA = nodeA;
            this.nodeB = nodeB;
        }

        public override void Draw() {
			Raylib.DrawLineEx(nodeA.pos, nodeB.pos, thickness, Color.GREEN);
        }

        public override void Update() {
        }
    }

    public class NodeGraphComponent : UIComponent {
		List<NodeGraphItem> contents = new List<NodeGraphItem>();
		Vector2 cameraPos;

		public NodeGraphComponent(Vector2 size) {
			ChangeFrame(size: size);
		}

		public void Add(NodeGraphItem c) {
			c.graph = this;
			contents.Add(c);
		}
		public override void Draw() {
			frame.Draw(Color.MAROON, Color.BLACK, 5);
			foreach (NodeGraphItem c in contents) c.Draw();
		}

		public override void Update() {
			foreach (NodeGraphItem c in contents) c.Update();
		}
		protected override void OnFrameChanged() {
			foreach (NodeGraphItem c in contents) c.ChangePos(frame.pos + c.posInGraph + cameraPos);
		}

	}
}
