#version 330

in vec3 in_position;
in vec2 in_texcoords;
in vec3 in_normal;

out vec2 texcoords;

// Lighting
out vec3 surfaceNormal;
out vec3 toLightVector;
out vec3 toCameraVector;

uniform mat4 transformationMatrix;
uniform mat4 projectionMatrix;
uniform mat4 viewMatrix;
uniform vec3 lightPosition;

void main(void)
{
	vec4 worldPosition = transformationMatrix * vec4(in_position, 1.0);
	gl_Position = projectionMatrix * viewMatrix * worldPosition;
	texcoords = in_texcoords;

	surfaceNormal = (transformationMatrix * vec4(in_normal, 0.0)).xyz;
	toLightVector = lightPosition - worldPosition.xyz;
	toCameraVector = (inverse(viewMatrix) * vec4(0.0, 0.0, 0.0, 1.0)).xyz - worldPosition.xyz;
}