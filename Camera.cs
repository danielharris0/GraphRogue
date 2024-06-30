using Raylib_cs;
using System.Numerics;
using static Raylib_cs.Raylib;

public static class Camera {
    public static Camera2D camera = new Camera2D(Vector2.Zero, Vector2.Zero, 0, 1);

    public static Vector2 ScreenToWorld(Vector2 v) => GetScreenToWorld2D(v, camera);
    public static float ScreenToWorld(float f) => f / camera.Zoom;
    public static Vector2 WorldToScreen(Vector2 v) => GetWorldToScreen2D(v, camera);
    public static float WorldToScreen(float f) => f * camera.Zoom;


    public static void Update(bool isFirstFrame) {

        if (Program.controlScheme.IsDown(Controls.left)) { camera.Offset.X += 5; }
        if (Program.controlScheme.IsDown(Controls.right)) { camera.Offset.X -= 5; }
        if (Program.controlScheme.IsDown(Controls.up)) { camera.Offset.Y += 5; }
        if (Program.controlScheme.IsDown(Controls.down)) { camera.Offset.Y -= 5; }

        float wheel = GetMouseWheelMove();
        if (wheel != 0 || isFirstFrame) {
            Vector2 mouseWorldPos = GetScreenToWorld2D(GetMousePosition(), camera);
            camera.Offset = GetMousePosition();
            camera.Target = mouseWorldPos;
            camera.Zoom *= (float)Math.Pow(1.1f, wheel);
            camera.Zoom = Math.Clamp(camera.Zoom, 0.2f, 20f);
        }
    }
}
