
namespace Content {

    class Player : Node { 
        public override string GetInfo() {
            return "It's you!";
        }

        public override string GetName() {
            return "Player";
        }
    }

    class Field : Node {
        public override string GetInfo() {
            return "It's a grassy field.";
        }

        public override string GetName() {
            return "Field";
        }
    }

    class In : Edge {

        public override string GetInfo() {
            return "NodeA is *in* NodeB.";
        }

        public override string GetName() {
            return "In";
        }
    }

    static class Generator {
        public static Node Generate(int seed) {
            //returns pov (player perception) node
            Player p  = new Player();
            Field f = new Field();
            In i = new In();
            Game.Link(p, i, f);
            return p;
        }
    }
}
