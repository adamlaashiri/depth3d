#version 330

in vec2 texcoords;

// Lighting
in vec3 surfaceNormal;
in vec3 toCameraVector;

out vec4 color_out;

uniform sampler2D tex;
uniform vec3 lightDirection;
uniform vec3 lightColor;

// Speccularity
uniform float shineDamper;
uniform float reflectivity;

void main(void)
{
	
	// Diffuse light
	vec3 normalizedSurfaceNormal = normalize(surfaceNormal);
	vec3 normalizedLightDirection = normalize(lightDirection);

	float nDot1 = dot(normalizedSurfaceNormal, -normalizedLightDirection);
	float brightness = max(nDot1, 0.15);
	vec3 diffuse = brightness * lightColor;

	// speccularity
	vec3 normalizedHalfwayDir = normalize(-normalizedLightDirection + normalize(toCameraVector));
	vec3 normalizedToCameraVector = normalize (toCameraVector);

	//old phong model
	//vec3 reflectedLightDirection = reflect(normalizedLightDirection, normalizedSurfaceNormal);
	//float specularFactor = dot(reflectedLightDirection, normalizedToCameraVector);

	float specularFactor = dot(normalizedSurfaceNormal, normalizedHalfwayDir);
	specularFactor = max(specularFactor, 0.0);
	float dampedFactor = pow(specularFactor, shineDamper);
	vec3 speccularity = dampedFactor * reflectivity * lightColor;

	color_out = vec4(diffuse, 1.0) * texture(tex, texcoords) + vec4(speccularity, 1.0);
}