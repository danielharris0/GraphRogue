
namespace GUI {

    public class ContentInfo { //A component & how it relates to its container
		public GUIComponent component;
		public Container container;
		public int id;
		public Container.BoxListener containerBoxChangeListener; //contributes the container's part to the content's box whenever the container's box changes
		public ContentInfo(GUIComponent component, Container container, int id, Container.BoxListener containerBoxChangeListener) {
			this.component = component; this.container = container; this.id = id; this.containerBoxChangeListener = containerBoxChangeListener;
		}
	}




    public class Container : GUIComponent {

        public abstract class BoxListener {

            protected Container container;
            protected GUIComponent component;

            public void Init(Container container, GUIComponent component) {
				this.container = container; this.component = component;	
			}
            public abstract void OnChange();
        }

		public static class BoxListeners {
            public class MatchBox : BoxListener {
                public override void OnChange() {
                    component.Resize(container.box.size);
                    component.Reposition(container.box.pos);
                }
            }

            public class None : BoxListener {
                public override void OnChange() { } //Do Nothing
            }
        }


        public List<ContentInfo> contents = new List<ContentInfo>();

		private Box _box;
		override public Box box {
			get { return _box; }
			set {
				_box = value;
				for (int i = 0; i< contents.Count; i++) {
					contents[i].containerBoxChangeListener.OnChange();
				}
			}
		}
		public Container(UpdateNotifier updateNotifier) : base(updateNotifier) { }

		public void Add(GUIComponent component, Container.BoxListener boxListener) {
			ContentInfo contentInfo = new ContentInfo(component, this, contents.Count, boxListener);
			contents.Add(contentInfo);
			component.contentInfo = contentInfo;
			boxListener.Init(this, component);
            boxListener.OnChange();
        }

		public override void Draw() {
			foreach (ContentInfo ci in contents) ci.component.Draw();
		}

	}
}