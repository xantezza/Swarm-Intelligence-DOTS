using _SwarmIntelligence.Components;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace _SwarmIntelligence.Jobs
{
    public partial struct MoveJob : IJobEntity
    {
        [ReadOnly] public double ElapsedTime;
        [ReadOnly] public float DeltaTime;
        
        [BurstCompile]
        private void Execute(ref LocalTransform localTransform, ref FoodSupplyComponent food)
        {
            Move(ref localTransform, ref food, DeltaTime);
        }

        [BurstCompile]
        private void Move(ref LocalTransform localTransform, ref FoodSupplyComponent food, float deltaTime)
        {
            float3 random = Random.CreateFromIndex((uint) ElapsedTime + food.Seed).NextFloat3(-0.5f, 0.5f);
            float3 moveDirection = math.normalizesafe(random) * deltaTime * 5;
            if (math.lengthsq(localTransform.Position - float3.zero) > 20 * 20)
            {
                moveDirection = math.normalizesafe(float3.zero - localTransform.Position) * deltaTime * 5;
            }

            localTransform.Position += moveDirection;
            food.Position = localTransform.Position;
        }
    }
}