using _SwarmIntelligence.Components;
using _SwarmIntelligence.Extensions;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace _SwarmIntelligence.Systems
{
    [BurstCompile]
    public partial struct AntMoveSystem : ISystem
    {
        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var entityManager = state.EntityManager;    
            var entities = entityManager.GetAllEntities();

            foreach (var entity in entities)
            {
                if (entityManager.HasComponent<AntComponent>(entity))
                {
                    var ant = entityManager.GetComponentData<AntComponent>(entity);
                    var localTransform = entityManager.GetComponentData<LocalTransform>(entity);
                    
                    ant.DistanceToFood++;
                    ant.DistanceToHome++;
                    ant.MoveDirection += RandomExtensions.RandomDirection(SystemAPI.Time.DeltaTime) * 0.1f;
                    float3 moveDirection = ant.MoveDirection * SystemAPI.Time.DeltaTime * ant.MoveSpeed;
                    localTransform.Position += moveDirection;
                    var target = quaternion.LookRotationSafe(moveDirection, math.up());
                    localTransform.Rotation = target;
                    ant.Position = localTransform.Position;

                    entityManager.SetComponentData(entity, localTransform);
                    entityManager.SetComponentData(entity, ant);
                }
            }
        }
    }
}