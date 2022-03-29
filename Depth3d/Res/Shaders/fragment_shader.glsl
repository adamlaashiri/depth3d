#version 330

in vec2 texcoords;

// lighting
in vec3 surfaceNormal;
in vec3 toLightVector;
in vec3 toCameraVector;

out vec4 color_out;

uniform sampler2D tex;
uniform vec3 lightColor;
uniform float shineDamper;
uniform float reflectivity;

void main(void)
{
	
	// Diffuse light
	vec3 normalizedSurfaceNormal = normalize(surfaceNormal);
	vec3 normalizedToLightVector = normalize(toLightVector);

	float nDot1 = dot(normalizedSurfaceNormal, normalizedToLightVector);
	float brightness = max(nDot1, 0.12);
	vec3 diffuse = brightness * lightColor;

	// speccularity
	vec3 normalizedToCameraVector = normalize (toCameraVector);
	vec3 lightDirection = -normalizedToLightVector;
	vec3 reflectedLightDirection = reflect(lightDirection, normalizedSurfaceNormal);

	float specularFactor = dot(reflectedLightDirection, normalizedToCameraVector);
	specularFactor = max(specularFactor, 0.0);
	float dampedFactor = pow(specularFactor, shineDamper);
	vec3 finalSpecular = dampedFactor * reflectivity * lightColor;

	color_out = vec4(diffuse, 1.0) * texture(tex, texcoords);
}