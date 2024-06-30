/*

Applications:

- open world rpg (no irreversable stat changes; only changing gear) - like a maze of puzzles

- abstract graph-based roguelike (think into the breach but graphs not grids)

- narrative / world simulator (think rimworld/dwarf fortress)

*/




using Raylib_cs;
using System.Numerics;
using static Raylib_cs.Raylib;

public enum Controls {up, down,left,right};

public static class Program {

    public static ControlScheme<Controls> controlScheme = new ControlScheme<Controls>();
    public static Graph graph = new Graph();

    public static Dictionary<String, NodeType> nodeTypes = new Dictionary<String, NodeType>();
    public static Dictionary<String, EdgeType> edgeTypes = new Dictionary<String, EdgeType>();

    public const float cellSize = 50;

    public static Font font;

    public static Vector2 screenSize;
    public static float panelWidth = 0;

    public static Vector2 GetMiddle(Rectangle rectangle) => rectangle.Position + rectangle.Size / 2;
    public static bool InRect(Vector2 v, Rectangle r) {
        Vector2 d = v - r.Position;
        return d.X>=0 && d.Y>=0 && d.X<=r.Width && d.Y<=r.Height;
    }

    public static void DrawArrow(Vector2 from, Vector2 to, Color colour, float lineWidth, float arrowSize) {
        Vector2 direction = to - from; direction = direction / direction.Length();
        Vector2 headBase = to - arrowSize * direction;
        Vector2 perp = new Vector2(-direction.Y, direction.X); perp = perp / perp.Length();

        DrawLineEx(from, headBase, lineWidth, colour);
        DrawTriangle(headBase - arrowSize * perp, headBase + arrowSize * perp, to, colour);
    }

    public static void DrawArrowOutlined(Vector2 from, Vector2 to, Color colour, Color outlineColour, float lineWidth, float arrowSize, float outlineWidth) {
        Vector2 direction = to - from; direction = direction / direction.Length();
        DrawArrow(from - direction* outlineWidth, to + direction * outlineWidth, outlineColour, lineWidth + outlineWidth*2, arrowSize + outlineWidth*2);
        DrawArrow(from, to, colour, lineWidth, arrowSize);
    }

    public static int Main() {
        SetConfigFlags(ConfigFlags.ResizableWindow | ConfigFlags.Msaa4xHint);
        InitWindow(800, 450, "raylib [core] example - basic window");
        SetTargetFPS(60);

        controlScheme.Add(Controls.up, KeyboardKey.Up);
        controlScheme.Add(Controls.down, KeyboardKey.Down);
        controlScheme.Add(Controls.left, KeyboardKey.Left);
        controlScheme.Add(Controls.right, KeyboardKey.Right);

        controlScheme.Add(Controls.up, KeyboardKey.W);
        controlScheme.Add(Controls.down, KeyboardKey.S);
        controlScheme.Add(Controls.left, KeyboardKey.A);
        controlScheme.Add(Controls.right, KeyboardKey.D);

        font = LoadFontEx("resources/xcharter/fonts/XCharter-Roman.otf", 128, null, 0);

        MaximizeWindow();

        bool isFirstFrame = true;

        //Init Content
        nodeTypes.Add("Person", new NodeType("Person", Color.Blue));
        nodeTypes.Add("Sword", new NodeType("Sword", Color.Gray));
        edgeTypes.Add("has", new EdgeType("has", new Color(0, 255, 100, 200)));
        edgeTypes.Add("loves", new EdgeType("loves", new Color(255, 100, 100, 200)));

        graph.GenDemo();

        Graph precondition = new Graph();
        { 
            Node p1 = precondition.Add(Program.nodeTypes["Person"], new Vector2(0, 0), 0.5f);
            Node p2 = precondition.Add(Program.nodeTypes["Person"], new Vector2(5, 0), 0.5f);
            Node s1 = precondition.Add(Program.nodeTypes["Sword"], new Vector2(5, 5), 0.5f);
            precondition.Connect(Program.edgeTypes["loves"], p2, p1);
            precondition.Connect(Program.edgeTypes["has"], p2, s1);
        }

        Change result = new Change();

        Process process = new Process(precondition, result);


        //Update Loop

        while (!WindowShouldClose()) {

            if (IsWindowResized() || isFirstFrame) {
                screenSize = new Vector2(GetScreenWidth(), GetScreenHeight());
            }

            graph.UpdateGraphics();

            ClearBackground(Color.Magenta);
            BeginDrawing();
                Background.Draw(new Rectangle(panelWidth, 0, screenSize.X - panelWidth, screenSize.Y));
                BeginMode2D(Camera.camera);
                    graph.Draw();
                    process.DrawCard(new Vector2(500,500));
                EndMode2D();
            EndDrawing();
            Camera.Update(isFirstFrame);
            if (isFirstFrame) isFirstFrame = false;
        }

        CloseWindow();
        return 0;
    }


}

