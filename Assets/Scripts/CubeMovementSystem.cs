using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Rendering;
using Unity.Transforms;

namespace DefaultNamespace
{
    public partial struct CubeMovementSystem : ISystem
    {
        public void OnUpdate(ref SystemState state)
        {
            var entityManager = state.EntityManager;
            var entities = entityManager.GetAllEntities();

            foreach (var entity in entities)
            {
                if (entityManager.HasComponent<CubeComponent>(entity))
                {
                    var cube = entityManager.GetComponentData<CubeComponent>(entity);

                    cube.moveSpeed -= 1 * SystemAPI.Time.DeltaTime;
                    if (cube.moveSpeed > 0)
                    {
                        entityManager.SetComponentData(entity, cube);
                        var localTransform = entityManager.GetComponentData<LocalTransform>(entity);
                        float3 moveDirection = cube.moveDirection * SystemAPI.Time.DeltaTime * cube.moveSpeed;
                        var target = quaternion.LookRotationSafe(moveDirection, math.up());
                        localTransform.Rotation = target;
                        localTransform = localTransform.RotateZ(-(float) SystemAPI.Time.ElapsedTime * 10);
                        localTransform.Position += moveDirection;
                        entityManager.SetComponentData(entity, localTransform);
                    }
                    else 
                    {
                        entityManager.AddComponentData(entity, new HDRPMaterialPropertyBaseColor {Value = new float4(1, 0, 0, 1)});
                        var buffer = entityManager.GetBuffer<Child>(entity).ToNativeArray(Allocator.Temp);

                        foreach (var child in buffer)
                        {
                            entityManager.AddComponentData(child.Value, new HDRPMaterialPropertyBaseColor {Value = new float4(1, 0, 0, 1)});
                        }

                        entityManager.RemoveComponent<CubeComponent>(entity);
                    }
                }
            }
        }
    }
}