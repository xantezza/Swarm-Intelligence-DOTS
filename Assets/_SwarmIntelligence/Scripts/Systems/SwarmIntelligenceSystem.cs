using _SwarmIntelligence.Components;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Rendering;

namespace _SwarmIntelligence.Systems
{
    [BurstCompile]
    public partial struct SwarmIntelligenceSystem : ISystem
    {
        private NativeArray<FoodSupplyComponent> _foodSupplies;
        private NativeArray<HomeComponent> _homeComponents;

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var entityManager = state.EntityManager;
            NativeArray<Entity> entities = state.EntityManager.GetAllEntities();

            if (_foodSupplies.Length == 0)
            {
                _foodSupplies = state.GetEntityQuery(new ComponentType[] {typeof(FoodSupplyComponent)})
                    .ToComponentDataArray<FoodSupplyComponent>(Allocator.Persistent);
                _homeComponents = state.GetEntityQuery(new ComponentType[] {typeof(HomeComponent)})
                    .ToComponentDataArray<HomeComponent>(Allocator.Persistent);
            }

            for (var index = 0; index < entities.Length; index++)
            {
                var entity = entities[index];
                if (!entityManager.HasComponent<AntComponent>(entity)) continue;

                ProcessFoodAndHome(ref state, entity);
                ProcessTalk(ref state, entities, entity);
            }
        }

        [BurstCompile]
        private void ProcessTalk(ref SystemState state, NativeArray<Entity> entities, Entity entity)
        {
            var ant = SystemAPI.GetComponentRW<AntComponent>(entity);
            foreach (var otherEntity in entities)
            {
                if (entity == otherEntity) continue;
                if (!SystemAPI.HasComponent<AntComponent>(otherEntity)) continue;
                var otherAnt = SystemAPI.GetComponentRW<AntComponent>(otherEntity);

                if (ant.ValueRO.SearchingForFood)
                {
                    if (math.lengthsq(ant.ValueRO.Position - otherAnt.ValueRO.Position) < ant.ValueRO.TalkRange * ant.ValueRO.TalkRange)
                    {
                        if (otherAnt.ValueRO.DistanceToFood + ant.ValueRO.TalkRange < ant.ValueRO.DistanceToFood)
                        {
                            ant.ValueRW.DistanceToFood = otherAnt.ValueRO.DistanceToFood + ant.ValueRO.TalkRange;

                            ant.ValueRW.MoveDirection = math.normalizesafe(otherAnt.ValueRO.Position - ant.ValueRO.Position);
                            return;
                        }
                    }
                }

                if (!ant.ValueRO.SearchingForFood)
                {
                    if (math.lengthsq(ant.ValueRO.Position - otherAnt.ValueRO.Position) < ant.ValueRO.TalkRange * ant.ValueRO.TalkRange)
                    {
                        if (otherAnt.ValueRO.DistanceToHome + ant.ValueRO.TalkRange < ant.ValueRO.DistanceToHome)
                        {
                            ant.ValueRW.DistanceToHome = otherAnt.ValueRO.DistanceToHome + ant.ValueRO.TalkRange;

                            ant.ValueRW.MoveDirection = math.normalizesafe(otherAnt.ValueRO.Position - ant.ValueRO.Position);
                            return;
                        }
                    }
                }
            }
        }

        [BurstCompile]
        private void ProcessFoodAndHome(ref SystemState state, Entity entity)
        {
            var ant = SystemAPI.GetComponentRW<AntComponent>(entity);
            var color = SystemAPI.GetComponentRW<HDRPMaterialPropertyBaseColor>(entity);
            if (math.lengthsq(ant.ValueRO.Position - float3.zero) > 25 * 25)
            {
                ant.ValueRW.MoveDirection *= -1;
                return;
            }

            if (ant.ValueRO.SearchingForFood)
            {
                foreach (var foodSupply in _foodSupplies)
                {
                    if (math.lengthsq(ant.ValueRO.Position - foodSupply.Position) < 1)
                    {
                        ant.ValueRW.DistanceToFood = 0;
                        ant.ValueRW.MoveDirection *= -1;
                        color.ValueRW.Value = ant.ValueRO.BackHomeColor;
                        ant.ValueRW.SearchingForFood = false;
                        return;
                    }
                }
            }

            if (!ant.ValueRO.SearchingForFood)
            {
                foreach (var homeComponent in _homeComponents)
                {
                    if (math.lengthsq(ant.ValueRO.Position - homeComponent.Position) < 1)
                    {
                        ant.ValueRW.DistanceToHome = 0;
                        ant.ValueRW.MoveDirection *= -1;
                        color.ValueRW.Value = ant.ValueRO.FoodSearchColor;
                        ant.ValueRW.SearchingForFood = true;
                        return;
                    }
                }
            }
        }
    }
}