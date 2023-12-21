
using Raylib_cs;
using System.Numerics;

namespace UIEngine {

    public class WindowComponent : UIComponent {

        const int sideGrabRange = 5;
        const int cornerGrabRange = 15;

        public bool mouseOnWindow;

        class WindowMoveDragInfo : CurrentInteraction.DragInfo {
            public Vector2 heldPosition; //offset from frame pos
            public WindowMoveDragInfo(WindowComponent window) {
                component = window;
                heldPosition = Raylib.GetMousePosition() - window.frame.pos;
            }
        }

        class WindowResizeDragInfo : CurrentInteraction.DragInfo {
            public ControlMode controlMode;
            public WindowResizeDragInfo(WindowComponent window, ControlMode controlMode) {
                component = window;
                this.controlMode = controlMode;
            }
        }

        enum ControlMode { None, Move, Left, Right, Top, Bottom, TopLeft, TopRight, BottomLeft, BottomRight }

        static Dictionary<ControlMode, MouseCursor> ControlModeToCursorImage = new Dictionary<ControlMode, MouseCursor> {
            {ControlMode.None, MouseCursor.MOUSE_CURSOR_ARROW },

            {ControlMode.Move, MouseCursor.MOUSE_CURSOR_RESIZE_ALL },

            {ControlMode.Left, MouseCursor.MOUSE_CURSOR_RESIZE_EW },
            {ControlMode.Right, MouseCursor.MOUSE_CURSOR_RESIZE_EW },
            {ControlMode.Top, MouseCursor.MOUSE_CURSOR_RESIZE_NS },
            {ControlMode.Bottom, MouseCursor.MOUSE_CURSOR_RESIZE_NS },

            {ControlMode.TopLeft, MouseCursor.MOUSE_CURSOR_RESIZE_NWSE},
            {ControlMode.TopRight, MouseCursor.MOUSE_CURSOR_RESIZE_NESW },
            {ControlMode.BottomLeft, MouseCursor.MOUSE_CURSOR_RESIZE_NESW },
            {ControlMode.BottomRight, MouseCursor.MOUSE_CURSOR_RESIZE_NWSE }
        };

        ControlMode hoveredControlMode;



        public WindowRegion region {
            set;
            private get;
        }

        UIComponent inner;
        int margin;
        public UIFrame relativeFrame;

        public void OnRegionFrameChanged() { ChangeRelativeFrame(relativeFrame.pos, relativeFrame.size); }

        public override void ChangeFrame(Vector2? pos = null, Vector2? size = null) {
            Vector2? newRelativePos = null;
            if (pos != null) { newRelativePos = Vector2.Divide((Vector2) pos - region.frame.pos, region.frame.size); }

            Vector2? newRelativeSize = null;
            if (size != null) { newRelativeSize = Vector2.Divide((Vector2) size, region.frame.size); }

            ChangeRelativeFrame(newRelativePos, newRelativeSize);
        }

        public void ChangeRelativeFrame(Vector2? pos = null, Vector2? size = null) {
            Vector2? newPos = null;
            if (pos != null) {
                relativeFrame.SetPos((Vector2) pos);
                newPos = region.frame.pos + Vector2.Multiply((Vector2) pos, region.frame.size);
            }

            Vector2? newSize = null;
            if (size != null) {
                relativeFrame.SetSize((Vector2)size);
                newSize = Vector2.Multiply((Vector2) size, region.frame.size);
            }

            base.ChangeFrame(newPos, newSize);
        }



        public ReorderCommand reorderCommand = ReorderCommand.None;


        public WindowComponent(Vector2 size, int margin, UIComponent inner) {
            this.inner = inner;
            this.margin = margin;
            relativeFrame.SetSize(Vector2.One*0.5f);
        }

        public override void Draw() {
            frame.Draw(Color.PURPLE, Color.BLACK, 5);
            inner.frame.Draw(Color.YELLOW, Color.BLACK, 5);
            inner.Draw();
        }

        void ResizeWindowByDragInfo(WindowResizeDragInfo info) {
            Vector2 p = Raylib.GetMousePosition();

            void TryResize(Vector2? pos = null, Vector2? size = null) {
                bool ValidSize(Vector2 size) => size.X >= margin * 4 && size.Y >= margin * 4;
                if (size == null || ValidSize((Vector2)size)) ChangeFrame(pos: pos, size: size);
            }

            switch (info.controlMode) {
                case ControlMode.Left: TryResize(pos: new Vector2(p.X, frame.pos.Y), size: new Vector2(frame.size.X + frame.pos.X - p.X, frame.size.Y)); break;
                case ControlMode.Right: TryResize(size: new Vector2(p.X - frame.pos.X, frame.size.Y)); break;
                case ControlMode.Top: TryResize(pos: new Vector2(frame.pos.X, p.Y), size: new Vector2(frame.size.X, frame.size.Y + frame.pos.Y - p.Y)); break;
                case ControlMode.Bottom: TryResize(size: new Vector2(frame.size.X, p.Y - frame.pos.Y)); break;
                case ControlMode.TopLeft: TryResize(pos: p, size: new Vector2(frame.size.X + frame.pos.X - p.X, frame.size.Y + frame.pos.Y - p.Y)); break;
                case ControlMode.TopRight: TryResize(pos: new Vector2(frame.pos.X, p.Y), size: new Vector2(p.X - frame.pos.X, frame.size.Y + frame.pos.Y - p.Y)); break;
                case ControlMode.BottomLeft: TryResize(pos: new Vector2(p.X, frame.pos.Y), size: new Vector2(frame.size.X + frame.pos.X - p.X, p.Y - frame.pos.Y)); break;
                case ControlMode.BottomRight: TryResize(size: new Vector2(p.X - frame.pos.X, p.Y - frame.pos.Y)); break;
            }
        }

        public override void Update() {
            hoveredControlMode = GetHoveredControlMode();
            CheckMousePresses();

            if (CurrentInteraction.IsDragging(this)) {
                switch (CurrentInteraction.dragInfo) {
                    case WindowMoveDragInfo info:
                        Vector2 newPos = Raylib.GetMousePosition() - info.heldPosition;
                        if (WithinRegion(newPos)) {
                            ChangeFrame(pos: newPos);
                        }
                        break;
                    case WindowResizeDragInfo info:
                        ResizeWindowByDragInfo(info);
                        break;
                }
            }

            if (mouseOnWindow) Raylib.SetMouseCursor(ControlModeToCursorImage[hoveredControlMode]);

            inner.Update();
        }

        private void CheckMousePresses() {

            if (Raylib.IsMouseButtonPressed(MouseButton.MOUSE_LEFT_BUTTON) && mouseOnWindow) {
                if (hoveredControlMode == ControlMode.Move) {
                    CurrentInteraction.dragInfo = new WindowMoveDragInfo(this);
                    reorderCommand = ReorderCommand.Top;
                } else if (hoveredControlMode != ControlMode.None) {
                    //hoveredControlMode = resizing
                    CurrentInteraction.dragInfo = new WindowResizeDragInfo(this, hoveredControlMode);
                }
            }
            if (Raylib.IsMouseButtonReleased(MouseButton.MOUSE_LEFT_BUTTON)) CurrentInteraction.dragInfo = null;
        }

        private ControlMode GetHoveredControlMode() {
            Vector2 p = Raylib.GetMousePosition();

            if ((frame.pos - p).Length() < cornerGrabRange) return ControlMode.TopLeft;
            if ((frame.pos + frame.size - p).Length() < cornerGrabRange) return ControlMode.BottomRight;
            if ((frame.pos + new Vector2(frame.size.X, 0) - p).Length() < cornerGrabRange) return ControlMode.TopRight;
            if ((frame.pos + new Vector2(0, frame.size.Y) - p).Length() < cornerGrabRange) return ControlMode.BottomLeft;

            if (p.X < frame.pos.X + sideGrabRange) return ControlMode.Left;
            if (p.X > frame.pos.X + frame.size.X - sideGrabRange) return ControlMode.Right;
            if (p.Y < frame.pos.Y + sideGrabRange) return ControlMode.Top;
            if (p.Y > frame.pos.Y + +frame.size.Y - sideGrabRange) return ControlMode.Bottom;

            bool PointOnFrameMargin(Vector2 p) => Raylib.CheckCollisionPointRec(p, frame.ToRectangle()) && !Raylib.CheckCollisionPointRec(p, inner.frame.ToRectangle());

            if (PointOnFrameMargin(p)) return ControlMode.Move;

            return ControlMode.None;
        }

        bool WithinRegion(Vector2 p) => p.X >= region.frame.pos.X && p.Y >= region.frame.pos.Y && p.X + frame.size.X <= region.frame.pos.X + region.frame.size.X && p.Y + frame.size.Y <= region.frame.pos.Y + region.frame.size.Y;

        protected override void OnFrameChanged() {
            Vector2 marginVector = new Vector2(margin, margin);
            inner.ChangeFrame(
                pos: frame.pos + marginVector,
                size: frame.size - marginVector * 2
            );
        }
    }
}