using Raylib_cs;
using System.Numerics;

namespace UIEngine {

    public class TextComponent : UIComponent {

        const int margin = 7;

        private string text;
        private Font font;
        private int size;

        public TextComponent(string text, Font font, int size) {
            this.text = text; this.font = font; this.size = size;
            ChangeFrame(size: Raylib.MeasureTextEx(font, text, size, 0) + Vector2.One*margin*2);
        }

        public override void Draw() {
            frame.Draw(Color.WHITE, Color.BLACK, 5);
            Raylib.DrawTextEx(font, text, frame.pos + Vector2.One*margin, size, 0, Color.BLACK);
        }


    }
}
