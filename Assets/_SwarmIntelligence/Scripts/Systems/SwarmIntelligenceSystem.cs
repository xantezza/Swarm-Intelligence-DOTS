using _SwarmIntelligence.Components;
using Unity.Burst;
using Unity.Entities;

namespace _SwarmIntelligence.Systems
{
    [BurstCompile]
    public partial struct SwarmIntelligenceSystem : ISystem
    {
        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            return;
            
            var entityManager = state.EntityManager;
            var entities = entityManager.GetAllEntities();

            foreach (var entity in entities)
            {
                if (entityManager.HasComponent<AntComponent>(entity))
                {
/*
                    cube.moveSpeed -= 1 * SystemAPI.Time.DeltaTime;
                    if (cube.moveSpeed > 0)
                    {
                        entityManager.SetComponentData(entity, cube);
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

                        entityManager.RemoveComponent<AntComponent>(entity);
                    }
                    */
                }
            }
        }
    }
}