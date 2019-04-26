#ifndef MODEL_H
#define MODEL_H
#include <vector>
#include "MMath.h"
#include "Mesh.h"
#include "Entity.h"

namespace GAME {

	using namespace MATH;

	class Model : public Entity {

	protected:

		class Shader *fireball;
		class Shader *shader;
		class Shader *reflectionShader;
		unsigned int textureID[1];
		Matrix4 modelMatrix;
		std::vector<Mesh*> meshes;

	public:

		Matrix4 getmodelMatrix();
		void setmodelMatrix(Matrix4);
		Model(const Vec3 _pos, const Vec3 _orientation);
		Model(const Model&) = delete;
		Model(Model&&) = delete;
		Model& operator = (const Model&) = delete;
		Model& operator = (Model&&) = delete;
		Model();
		float frequency;
		float elapsedTime;
		virtual ~Model();
		

		void setPos(const Vec3& pos_) override;
		void setOrientation(const Vec3& orienration_) override;

		virtual bool OnCreate();
		virtual void OnDestroy();
		virtual void Render() const;
		virtual void Update(const float deltaTime);

		virtual bool LoadMesh(const char* filename);

	protected:

		void updateModelMatrix();
	};
} /// end of namespace

#endif