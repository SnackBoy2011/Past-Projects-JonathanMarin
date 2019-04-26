#version 330
in  vec4 vVertex;
in  vec4 vNormal;

out vec3 eyeDir;
out vec3 vertNormal;
out vec3 lightDir;
 
uniform mat4 projectionMatrix;
uniform mat4 modelViewMatrix;
uniform mat3 normalMatrix;
uniform vec3 lightPos;

uniform float time;

uniform sampler3D noise;

void main() {
	vertNormal = normalMatrix * vNormal.xyz; /// Rotate the normal to the correct orientation 
	eyeDir = vec3(modelViewMatrix * vVertex); /// Create the eye vector 
	lightDir = normalize(lightPos - eyeDir.xyz); /// Create the light direction 

	vec4 noiseVec = texture3D(noise, vVertex.xyz + time);
	float displacement = abs(1.0f * dot(noiseVec.xyz, vNormal.xyz)); //Dot product for how far we will displace our vertex, multiplied by a frequency

	vec3 newPosition = vVertex.xyz + vNormal.xyz * displacement;
	gl_Position =  projectionMatrix * modelViewMatrix * vec4(newPosition, 1.0f); 
}