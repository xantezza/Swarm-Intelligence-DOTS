using _SwarmIntelligence.Components;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;

namespace _SwarmIntelligence.Jobs
{
    [BurstCompile]
    public struct SwarmIntelligenceJob : IJobParallelFor
    {
        [WriteOnly] private NativeArray<Entity> _ants;
        [ReadOnly] private NativeArray<AntComponent> _otherAnts;

        public SwarmIntelligenceJob(NativeArray<Entity> ants, NativeArray<AntComponent> otherAnts)
        {
            _ants = ants;
            _otherAnts = otherAnts;
        }

        [BurstCompile]
        public void Execute(int index)
        {
            var entity = _ants[index];
            if (!entityManager.HasComponent<AntComponent>(entity)) continue;

            ProcessFoodAndHome(entityManager, entity);

            foreach (var otherAnt in _ants)
            {
                var ant = entityManager.GetComponentData<AntComponent>(entity);

                if (math.lengthsq(ant.Position - otherAnt.Position) < ant.TalkRange * ant.TalkRange)
                {
                    if (ant.SearchingForFood)
                    {
                        if (otherAnt.DistanceToFood + ant.TalkRange < ant.DistanceToFood)
                        {
                            ant.DistanceToFood = (uint) (otherAnt.DistanceToFood + ant.TalkRange);
                            ant.MoveDirection = math.normalize(otherAnt.Position - ant.Position);
                            entityManager.SetComponentData(entity, ant);
                        }
                    }
                    else
                    {
                        if (otherAnt.DistanceToHome + ant.TalkRange < ant.DistanceToHome)
                        {
                            ant.DistanceToHome = (uint) (otherAnt.DistanceToHome + ant.TalkRange);
                            ant.MoveDirection = math.normalize(otherAnt.Position - ant.Position);
                            entityManager.SetComponentData(entity, ant);
                        }
                    }
                }
            }
        }
    }
}