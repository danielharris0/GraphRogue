

using System.Drawing;
using System.Numerics;

namespace GUI {

    public struct Box {
        public Vector2 size;
        public Vector2 pos;

        public Box(Vector2 size, Vector2 pos) {this.size=size; this.pos = pos;}

        public override string ToString() {
            return "Box at " + pos.ToString() + ", size " + size.ToString();
        } 
    }
    public abstract class GUIComponent {

        public virtual Box box { get; set;}
        public bool live = true;

        public ContentInfo contentInfo;

        public GUIComponent(UpdateNotifier updateNotifier) {
            updateNotifier.Subscribe(this);
        }

        public void Resize(Vector2 size) { box = new Box(size, box.pos); }
        public void Reposition(Vector2 pos) { box = new Box(box.size, pos); }

        public Vector2 GetCenter() { return box.pos + box.size/2; }
        public Raylib_cs.Rectangle GetRectangle() { return new Raylib_cs.Rectangle(box.pos.X, box.pos.Y, box.size.X, box.size.Y); }

        public abstract void Draw();
        public virtual void Update() { }
    }
}
