using Raylib_cs;
using System.Numerics;
using static Raylib_cs.Raylib;

public static class Panel {
    public static void Draw() {
        DrawRectangleV(Vector2.Zero, new Vector2(Program.panelWidth, Program.screenSize.Y), Color.Gray);

        DrawTextEx(Program.font, 
            "Hovered node type:" +
            "\n\n\n\n--------------------------------\n\n\n\n" +
            "Applicable Unlocked Actions:" + "\n\n\n\n" +
            "Demand Treasure:\n\n    If a -VASSAL- of YOU,\n\n    -HAS- some TREASURE,\n\n    you may change the endpoint of -HAS- to YOU." + 
            "\n\n\n\n--------------------------------\n\n\n\n" +
            "Unapplicable Unlocked Actions:" +
            "\n\n\n\n--------------------------------\n\n\n\n" +
            "Not Unlocked Actions:"
            , Vector2.Zero, 40, 1, Color.Black);

    }
}