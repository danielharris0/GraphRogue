#version 330

// Input vertex attribute (from vertex shader)
in vec2 fragTexCoord;

// Input uniform values
uniform float lineThickness;
uniform float boxSize;
uniform vec2 scroll;
uniform vec2 backgroundSize;

// Output fragment color
out vec4 finalColor;

bool test(float n) {
	float r = mod(n, boxSize);
   return r<lineThickness || r>boxSize-lineThickness;
}

void main() {
   vec2 p = fragTexCoord*backgroundSize + scroll;
   finalColor = test(p.x) || test(p.y)  ? vec4(fragTexCoord,fragTexCoord.x + fragTexCoord.y,1) : vec4(0.5,fragTexCoord.y, fragTexCoord.x,1);
}
