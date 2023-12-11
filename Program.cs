using Raylib_cs;
using System.Numerics;

public static class Program {

    static Vector2 ballPosition = new Vector2(-100.0f, -100.0f);
    static Color ballColor = Color.DARKBLUE;

    //Note: resource paths are relative to the OUTPUT directory (I have an itemgroup defined in my .csproj to auto-copy my /resources into the OUTPUT directory)
    static Font font = Raylib.LoadFontEx("resources/fonts/Rethink_Sans/static/RethinkSans-Regular.ttf", 128, null, 250);

    public static int Main() {
        Raylib.InitWindow(16*100, 9*100, "GraphRogue");
        Raylib.SetWindowState(ConfigFlags.FLAG_WINDOW_RESIZABLE);
        Raylib.MaximizeWindow();

        Raylib.SetTargetFPS(60);
        while (!Raylib.WindowShouldClose()) {
            Update();
            Raylib.BeginDrawing();
            Raylib.ClearBackground(Color.GRAY);
            Draw();
            Raylib.EndDrawing(); //From my testing it seems EndDrawing() waits to cap the framerate as set by SetTargetFPS(60)
        }

        Raylib.CloseWindow();
        return 0;
    }

    static void Update() {
        ballPosition = Raylib.GetMousePosition();

        if (Raylib.IsMouseButtonPressed(MouseButton.MOUSE_LEFT_BUTTON)) {
            new Task(LongTask).Start();
            ballColor = Color.MAROON;
        } else if (Raylib.IsMouseButtonPressed(MouseButton.MOUSE_MIDDLE_BUTTON)) {
            ballColor = Color.LIME;
        } else if (Raylib.IsMouseButtonPressed(MouseButton.MOUSE_RIGHT_BUTTON)) {
            ballColor = Color.DARKBLUE;
        }
    }

    static void Draw() {
        Raylib.DrawCircleV(ballPosition, 40, ballColor);
        Raylib.DrawTextEx(font, "tinyfont", new Vector2(20, 20), 30, 0, Color.DARKGRAY);
        Raylib.DrawTextEx(font, "hello", new Vector2(50, 50), 128, 0, Color.WHITE);
    }

    static void LongTask() {
        for (int i=0; i<10; i++) {
            Console.WriteLine(i);
            for (int j = 0; j<Math.Pow(10,7); j++) {}
        }
    }


}
