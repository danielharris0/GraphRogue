using Raylib_cs;
using System.Numerics;

public class InputMouse {
    public static int Main() {
        Raylib.InitWindow(16*100, 9*100, "GraphRogue");
        Raylib.SetWindowState(ConfigFlags.FLAG_WINDOW_RESIZABLE);
        Raylib.MaximizeWindow();

        Vector2 ballPosition = new(-100.0f, -100.0f);
        Color ballColor = Color.DARKBLUE;

        Raylib.SetTargetFPS(60);
        while (!Raylib.WindowShouldClose()) {

            ballPosition = Raylib.GetMousePosition();

            if (Raylib.IsMouseButtonPressed(MouseButton.MOUSE_LEFT_BUTTON)) {
                new Task(LongTask).Start();
                ballColor = Color.MAROON;
            } else if (Raylib.IsMouseButtonPressed(MouseButton.MOUSE_MIDDLE_BUTTON)) {
                ballColor = Color.LIME;
            } else if (Raylib.IsMouseButtonPressed(MouseButton.MOUSE_RIGHT_BUTTON)) {
                ballColor = Color.DARKBLUE;
            }

            Raylib.BeginDrawing();


            Raylib.ClearBackground(Color.RAYWHITE);

            Raylib.DrawCircleV(ballPosition, 40, ballColor);

            Raylib.DrawText("move ball with mouse and click mouse button to change color", 10, 10, 20, Color.DARKGRAY);
            Raylib.EndDrawing(); //From my testing it seems EndDrawing() waits to cap the framerate as set by SetTargetFPS(60)
        }

        Raylib.CloseWindow();
        return 0;
    }

    static void LongTask() {
        for (int i=0; i<10; i++) {
            Console.WriteLine(i);
            for (int j = 0; j<Math.Pow(10,7); j++) {}
        }
    }


}
