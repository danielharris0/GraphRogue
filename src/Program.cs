using Raylib_cs;
using System.Numerics;

public static class Program {

    private static GUI.UpdateNotifier updateNotifier;

    public static GUI.Container windowContainer;
    public static UserInterface userInterface;


    public static int Main() {

        Raylib.InitWindow(16*100, 9*100, "GraphRogue");
        Raylib.SetWindowState(ConfigFlags.FLAG_WINDOW_RESIZABLE);
        Raylib.MaximizeWindow();
        Raylib.SetMouseCursor(MouseCursor.MOUSE_CURSOR_ARROW);

        Load();

        Raylib.SetTargetFPS(60);
        while (!Raylib.WindowShouldClose()) {
            Update();
            Raylib.BeginDrawing();
            Raylib.ClearBackground(Color.LIGHTGRAY);
            Draw();
            Raylib.EndDrawing(); //From my testing it seems EndDrawing() waits to cap the framerate as set by SetTargetFPS(60)
        }

        Raylib.CloseWindow();
        return 0;
    }

    private static void Load() {
        updateNotifier = new GUI.UpdateNotifier();

        windowContainer = new GUI.Container(updateNotifier);
        userInterface = new UserInterface(updateNotifier);

        windowContainer.Resize(new Vector2(Raylib.GetScreenWidth(), Raylib.GetScreenHeight()));

        userInterface.Load();
        Game.Load();
    }
    private static void Update() {
        if (Raylib.IsWindowResized()) windowContainer.Resize(new Vector2(Raylib.GetScreenWidth(), Raylib.GetScreenHeight()));
        updateNotifier.Update();
    }
    private static void Draw() {
        windowContainer.Draw();
    }
}
