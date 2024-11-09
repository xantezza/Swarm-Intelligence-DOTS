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

        [BurstCompile]
        private void Execute(ref AntComponent ant, ref HDRPMaterialPropertyBaseColor color)
        {
            ProcessFood(ref ant, ref color);
            ProcessTalk(ref ant);
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