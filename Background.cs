using Raylib_cs;
using System.Numerics;
using static Raylib_cs.Raylib;

public static class Background {
    private static Texture2D unitTexture = LoadTexture("resources/unitTexture.png");

    private static Shader backgroundShader = LoadShader(null, "resources/backgroundShader.glsl");
    private static int backgroundShaderScrollUniformLocIndex = GetShaderLocation(backgroundShader, "scroll");
    private static int backgroundShaderBackgroundSizeUniformLocIndex = GetShaderLocation(backgroundShader, "backgroundSize");
    private static int backgroundShaderLineThicknessUniformLocIndex = GetShaderLocation(backgroundShader, "lineThickness");
    private static int backgroundShaderBoxSizeUniformLocIndex = GetShaderLocation(backgroundShader, "boxSize");


    public static void Draw(Rectangle rect) {
        Vector2 scroll = Camera.camera.Target * Camera.camera.Zoom - Camera.camera.Offset;

        SetShaderValue(backgroundShader, backgroundShaderBoxSizeUniformLocIndex, Program.cellSize * Camera.camera.Zoom, ShaderUniformDataType.Float);
        SetShaderValue(backgroundShader, backgroundShaderLineThicknessUniformLocIndex, 2 * Camera.camera.Zoom, ShaderUniformDataType.Float);

        SetShaderValue(backgroundShader, backgroundShaderScrollUniformLocIndex, scroll, ShaderUniformDataType.Vec2);
        SetShaderValue(backgroundShader, backgroundShaderBackgroundSizeUniformLocIndex, rect.Size, ShaderUniformDataType.Vec2);

        BeginShaderMode(backgroundShader);
        DrawTexturePro(unitTexture, new Rectangle(0, 0, 1, 1), rect, Vector2.Zero, 0, Color.White);
        EndShaderMode();
    }
}
