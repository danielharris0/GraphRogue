using System.Numerics;
using Raylib_cs;

namespace UIEngine {

    public class WindowRegion : UIComponent {

        List<WindowComponent> windows = new List<WindowComponent>();

        public WindowRegion(Vector2 size) {
            ChangeFrame(size: size);
        }

        public void Add(WindowComponent window) {
            window.region = this;
            windows.Add(window);
        }
        public override void Draw() {
            frame.Draw(Color.MAROON, Color.BLACK, 5);
            foreach (WindowComponent window in windows) window.Draw();
        }

        public override void Update() {

            int i = windows.Count;
            bool hitWindow = false;
            while (i-- > 0 && !hitWindow) {
                if (Raylib.CheckCollisionPointRec(Raylib.GetMousePosition(), windows[i].frame.ToRectangle())) {
                    windows[i].mouseOnWindow = true;
                    hitWindow = true;
                    break;
                } else {
                    windows[i].mouseOnWindow = false;
                }
            }
            while (i-- > 0) windows[i].mouseOnWindow = false;

            foreach (WindowComponent window in windows) window.Update();

            //Reorder windows
            bool done = false;
            while (!done) {
                done = true;
                foreach (WindowComponent window in windows) {
                    if (window.reorderCommand == ReorderCommand.Top) {
                        windows.Remove(window);
                        windows.Insert(windows.Count, window);
                    }
                    if (window.reorderCommand == ReorderCommand.Bottom) {
                        windows.Remove(window);
                        windows.Insert(0, window);
                    }
                    if (window.reorderCommand != ReorderCommand.None) {
                        done = false;
                        window.reorderCommand = ReorderCommand.None;
                        break;
                    }
                }

            }
        }

        protected override void OnFrameChanged() {
            foreach (WindowComponent window in windows) window.OnRegionFrameChanged();
        }

    }

    public enum ReorderCommand { None, Top, Bottom }
}