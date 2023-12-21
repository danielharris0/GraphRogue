using Raylib_cs;
using System.Numerics;

namespace UIEngine {

    static class CurrentInteraction {

        public abstract class DragInfo {
            public UIComponent component;
        }
        public static DragInfo? dragInfo;
        public static bool IsDragging(UIComponent c) => dragInfo != null && dragInfo.component == c;
    }

    static class UIEngineManager {
        static WindowRegion main = new WindowRegion(Vector2.Zero);

        public static void Add(WindowComponent c) {
            main.Add(c);
        }

        public static void Update() {
            DemoAsyncTask();

            if (Raylib.IsWindowResized()) {
                main.ChangeFrame(
                    pos: new Vector2(10, 10),
                    size: new Vector2(Raylib.GetScreenWidth() - 20, Raylib.GetScreenHeight() - 20)
                );
            }

            Raylib.SetMouseCursor(MouseCursor.MOUSE_CURSOR_ARROW);
            main.Update();
        }

        private static void DemoAsyncTask() {
            static void LongTask() {
                for (int i = 0; i < 10; i++) {
                    Console.WriteLine(i);
                    for (int j = 0; j < Math.Pow(10, 7); j++) { }
                }
            }
            if (Raylib.IsMouseButtonPressed(MouseButton.MOUSE_MIDDLE_BUTTON)) { new Task(LongTask).Start(); }
        }

        public static void Draw() {
            main.Draw();
        }

    }

}