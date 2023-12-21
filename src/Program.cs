using Raylib_cs;

public static class Program {

    public static int Main() {

        Playground.Play();

        Raylib.InitWindow(16*100, 9*100, "GraphRogue");
        Raylib.SetWindowState(ConfigFlags.FLAG_WINDOW_RESIZABLE);
        Raylib.MaximizeWindow();
        Raylib.SetMouseCursor(MouseCursor.MOUSE_CURSOR_ARROW);

        UI.UIManager.Load();

        Raylib.SetTargetFPS(60);
        while (!Raylib.WindowShouldClose()) {
            UIEngine.UIEngineManager.Update();
            Raylib.BeginDrawing();
            Raylib.ClearBackground(Color.LIGHTGRAY);
            UIEngine.UIEngineManager.Draw();
            Raylib.EndDrawing(); //From my testing it seems EndDrawing() waits to cap the framerate as set by SetTargetFPS(60)
        }

        Raylib.CloseWindow();
        return 0;
    }


}
