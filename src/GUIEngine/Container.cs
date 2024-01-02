
namespace GUI {

    using ContainerBoxChangeListener = Action<ContentInfo>;

    public class ContentInfo { //A component & how it relates to its container
		public GUIComponent component;
		public Container container;
		public int id;
		public ContainerBoxChangeListener containerBoxChangeListener; //contributes the container's part to the content's box whenever the container's box changes
		public ContentInfo(GUIComponent component, Container container, int id, ContainerBoxChangeListener containerBoxChangeListener) {
			this.component = component; this.container = container; this.id = id; this.containerBoxChangeListener = containerBoxChangeListener;
		}
	}



    public class Container : GUIComponent {

		public static class ContainerBoxChangeListeners {
            public static ContainerBoxChangeListener MATCH_BOX = delegate (ContentInfo contentInfo) {
                contentInfo.component.Resize(contentInfo.container.box.size);
                contentInfo.component.Reposition(contentInfo.container.box.pos);
            };
            public static ContainerBoxChangeListener NONE = delegate (ContentInfo contentInfo) { };
        }


        public List<ContentInfo> contents = new List<ContentInfo>();

		private Box _box;
		override public Box box {
			get { return _box; }
			set {
				_box = value;
				for (int i = 0; i< contents.Count; i++) {
					contents[i].containerBoxChangeListener.Invoke(contents[i]);
				}
			}
		}
		public Container(UpdateNotifier updateNotifier) : base(updateNotifier) { }

		public void Add(GUIComponent component, ContainerBoxChangeListener containerBoxChangeListener) {
			ContentInfo contentInfo = new ContentInfo(component, this, contents.Count, containerBoxChangeListener);
			contents.Add(contentInfo);
			component.contentInfo = contentInfo;
            containerBoxChangeListener.Invoke(contentInfo);
        }

		public override void Draw() {
			foreach (ContentInfo ci in contents) ci.component.Draw();
		}

	}
}