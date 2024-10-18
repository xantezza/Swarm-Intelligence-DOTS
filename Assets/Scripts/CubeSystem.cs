using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace DefaultNamespace
{
    [BurstCompile]
    public partial struct CubeSystem : ISystem
    {
        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var entityManager = state.EntityManager;
            var entities = entityManager.GetAllEntities();

            foreach (var entity in entities)
            {
                if (entityManager.HasComponent<CubeComponent>(entity))
                {
                    var cube = entityManager.GetComponentData<CubeComponent>(entity);
                    var localTransform = entityManager.GetComponentData<LocalTransform>(entity);
                    float3 moveDirection = cube.moveDirection * SystemAPI.Time.DeltaTime * cube.moveSpeed;
                    localTransform.Position += moveDirection;
                    entityManager.SetComponentData(entity, localTransform);

                    if (cube.moveSpeed > 0)
                    {
                        cube.moveSpeed -= 1 * SystemAPI.Time.DeltaTime;
                        entityManager.SetComponentData(entity, cube);
                    }
                    else
                    {
                        cube.moveSpeed = 0;
                        entityManager.RemoveComponent<CubeComponent>(entity);
                    }
                }
            }
        }
    }
}