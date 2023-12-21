using Raylib_cs;
using System.Numerics;

namespace UIEngine {
	public struct UIFrame {
		public Vector2 pos { get; private set; }
		public Vector2 size { get; private set; }

		public UIFrame(Vector2 p, Vector2 s) { pos = p; size = s; }
		public Rectangle ToRectangle() {
			return new Rectangle(pos.X, pos.Y, size.X, size.Y);
		}

		public void SetPos(Vector2 pos) { this.pos = pos; }
		public void SetSize(Vector2 size) { this.size = size; }

		public void Draw(Color backCol, Color lineCol, float thickness) {
			Raylib.DrawRectangleRec(ToRectangle(), backCol);
			Raylib.DrawRectangleLinesEx(ToRectangle(), thickness, lineCol);
		}
	}

	public abstract class UIComponent {

		//The component's bounding box
			//We expect to use the same frame object reference for the component's lifetime (thus frame is a struct)
		public UIFrame frame;
		public abstract void Draw();

		protected virtual void OnFrameChanged() { }

		public virtual void ChangeFrame(Vector2? pos = null, Vector2? size = null) {
			if (pos != null) frame.SetPos((Vector2)pos);
			if (size != null) frame.SetSize((Vector2)size);
			OnFrameChanged();
		}

		public virtual void Update() { }
	}
}
