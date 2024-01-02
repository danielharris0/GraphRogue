
using System.Collections.Generic;

namespace GUI {
    public class UpdateNotifier {

        private List<GUIComponent> components = new List<GUIComponent>();

        public void Subscribe(GUIComponent c) {
            components.Add(c);
        }
        public void Update() {
            for (int i = components.Count - 1; i >= 0; i--) {
                GUIComponent c = components[i];
                if (!c.live) components.RemoveAt(i);
                else c.Update();
            }
        }
    }
}
