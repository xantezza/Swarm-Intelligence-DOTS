using _SwarmIntelligence.Components;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace _SwarmIntelligence.Jobs
{
    public partial struct MoveAntJob : IJobEntity
    {
        [ReadOnly] public double ElapsedTime;
        [ReadOnly] public float DeltaTime;

        [BurstCompile]
        private void Execute(ref LocalTransform localTransform, ref AntComponent ant)
        {
            MoveAnt(ref localTransform, ref ant, DeltaTime);
        }

        [BurstCompile]
        private void MoveAnt(ref LocalTransform localTransform, ref AntComponent ant, float deltaTime)
        {
            if (math.lengthsq(ant.Position - float3.zero) > 25 * 25)
            {
                ant.MoveDirection = math.normalizesafe(float3.zero - ant.Position);
            }

            float3 random = Random.CreateFromIndex(ant.Seed + (uint) ElapsedTime).NextFloat3(-0.3f, 0.3f);
            float3 moveDirection = math.normalizesafe(ant.MoveDirection + random) * deltaTime * ant.MoveSpeed;
            ant.DistanceToHome++;
            ant.DistanceToFood++;
            quaternion target = quaternion.LookRotationSafe(moveDirection, math.up());
            localTransform.Position += moveDirection;
            localTransform.Rotation = target;
            ant.Position = localTransform.Position;
        }
    }
}