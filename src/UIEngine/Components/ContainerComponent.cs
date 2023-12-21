using Raylib_cs;
using System.Numerics;

namespace UIEngine {
	public class VerticalContainer : UIComponent {

		List<UIComponent> children = new List<UIComponent>();

		public void Add(UIComponent child) {
			children.Add(child);
		}
		public override void Draw() {
			frame.Draw(Color.GRAY, Color.BLACK, 5);
			foreach (UIComponent child in children) child.Draw();
		}

		public override void Update() {
			foreach (UIComponent child in children) child.Update();
		}

		protected override void OnFrameChanged() {
			//Child layouts: stack downward
			float y = frame.pos.Y;
			for (int i = 0; i < children.Count; i++) {
				UIComponent child = children[i];
				child.ChangeFrame(pos: new Vector2(frame.pos.X, y));
				y += child.frame.size.Y;
			}
		}
	}
}