using _SwarmIntelligence.Components;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Rendering;
using Unity.Transforms;

namespace _SwarmIntelligence.Jobs
{
    [BurstCompile]
    public partial struct SwarmEntityJob : IJobEntity
    {
        [ReadOnly] public NativeArray<AntComponent> AntComponents;
        [ReadOnly] public NativeArray<FoodSupplyComponent> FoodSupplyComponents;
        [ReadOnly] public NativeArray<HomeComponent> HomeComponents;
        [ReadOnly] public float DeltaTime;
        [ReadOnly] public double ElapsedTime;

        [BurstCompile]
        private void Execute(ref LocalTransform localTransform, ref AntComponent ant, ref HDRPMaterialPropertyBaseColor color)
        {
            Move(ref localTransform, ref ant, DeltaTime);
            ProcessFood(ref ant, ref color);
            ProcessTalk(ref ant);
        }   

        [BurstCompile]
        private void Move(ref LocalTransform localTransform, ref AntComponent ant, float deltaTime)
        {
            float3 random = Random.CreateFromIndex(ant.Seed + (uint)ElapsedTime).NextFloat3(-0.2f, 0.2f);
            float3 moveDirection = math.normalizesafe(ant.MoveDirection + random) * deltaTime * ant.MoveSpeed;
            ant.DistanceToHome++;
            ant.DistanceToFood++;
            localTransform.Position += moveDirection;
            var target = quaternion.LookRotationSafe(moveDirection, math.up());
            localTransform.Rotation = target;
            ant.Position = localTransform.Position;
        }

        [BurstCompile]
        private void ProcessTalk(ref AntComponent ant)
        {
            foreach (var otherAnt in AntComponents)
            {
                if (ant.SearchingForFood)
                {
                    if (math.lengthsq(ant.Position - otherAnt.Position) < ant.TalkRange * ant.TalkRange)
                    {
                        if (otherAnt.DistanceToFood + ant.TalkRange < ant.DistanceToFood)
                        {
                            ant.DistanceToFood = otherAnt.DistanceToFood + ant.TalkRange;

                            ant.MoveDirection = math.normalizesafe(otherAnt.Position - ant.Position);
                            return;
                        }
                    }
                }
                else
                {
                    if (math.lengthsq(ant.Position - otherAnt.Position) < ant.TalkRange * ant.TalkRange)
                    {
                        if (otherAnt.DistanceToHome + ant.TalkRange < ant.DistanceToHome)
                        {
                            ant.DistanceToHome = otherAnt.DistanceToHome + ant.TalkRange;

                            ant.MoveDirection = math.normalizesafe(otherAnt.Position - ant.Position);
                            return;
                        }
                    }
                }
            }
        }

        [BurstCompile]
        private void ProcessFood(ref AntComponent ant, ref HDRPMaterialPropertyBaseColor color)
        {
            if (math.lengthsq(ant.Position - float3.zero) > 35 * 35)
            {
                ant.MoveDirection *= -1;
                return;
            }

            if (ant.SearchingForFood)
            {
                foreach (var foodSupply in FoodSupplyComponents)
                {
                    if (math.lengthsq(ant.Position - foodSupply.Position) < 1)
                    {
                        ant.DistanceToFood = 0;
                        ant.MoveDirection *= -1;
                        color.Value = ant.BackHomeColor;
                        ant.SearchingForFood = false;
                        return;
                    }
                }
            }

            if (!ant.SearchingForFood)
            {
                foreach (var homeComponent in HomeComponents)
                {
                    if (math.lengthsq(ant.Position - homeComponent.Position) < 1)
                    {
                        ant.DistanceToHome = 0;
                        ant.MoveDirection *= -1;
                        color.Value = ant.FoodSearchColor;
                        ant.SearchingForFood = true;
                        return;
                    }
                }
            }
        }
    }
}