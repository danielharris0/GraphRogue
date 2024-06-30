using Raylib_cs;
using static Raylib_cs.Raylib;

public class ControlScheme<Control> where Control : System.Enum {
	public Dictionary<Control, List<KeyboardKey>> controlKeys = new Dictionary<Control, List<KeyboardKey>>();

	public void Add(Control control, KeyboardKey key) {
		if (!controlKeys.ContainsKey(control)) controlKeys.Add(control, new List<KeyboardKey>());
		controlKeys[control].Add(key);
	}

	public void Remove(Control control, KeyboardKey key) {
		if (controlKeys.ContainsKey(control)) controlKeys[control].Remove(key);
	}

	public bool IsDown(Control control) {
		foreach (KeyboardKey key in controlKeys[control]) {
			if (IsKeyDown(key)) return true;
		}
		return false;
	}
}