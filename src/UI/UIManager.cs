

using Raylib_cs;
using System.Numerics;
using UIEngine;

namespace UI {
    static class UIManager {

        //Note: resource paths are relative to the OUTPUT directory (I have an itemgroup defined in my .csproj to auto-copy my /resources into the OUTPUT directory)
        static Font font = Raylib.LoadFontEx("resources/fonts/Rethink_Sans/static/RethinkSans-Regular.ttf", 25, null, 250);

        static WindowComponent graphWindow;
        public static void Load() {
            VerticalContainer vc = new VerticalContainer();
            vc.Add(new TextComponent("Nodegraph", font, 25));
            NodeGraphComponent gr = new NodeGraphComponent(new Vector2(300, 300));
            Node a = new Node(new Vector2(50, 50));
            Node b = new Node(new Vector2(100, 100));
            gr.Add(a); gr.Add(b); gr.Add(new Edge(a, b));
            vc.Add(gr);
            graphWindow = new WindowComponent(new Vector2(500, 500), 15, vc);
            UIEngineManager.Add(graphWindow);

            UIEngineManager.Add(new WindowComponent(new Vector2(16 * 50, 9 * 50), 50, new VerticalContainer()));

            WindowRegion wr2 = new WindowRegion(new Vector2(500, 500));
            wr2.Add(new WindowComponent(new Vector2(50, 100), 15, new VerticalContainer()));
            wr2.Add(new WindowComponent(new Vector2(100, 50), 15, new VerticalContainer()));
            UIEngineManager.Add(new WindowComponent(new Vector2(16 * 25, 9 * 25), 30, wr2));
        }

        public static void LoadNodes() {

        }


    }
}
