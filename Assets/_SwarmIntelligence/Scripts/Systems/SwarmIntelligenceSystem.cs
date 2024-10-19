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
        private NativeArray<AntComponent> _ants;
        private NativeArray<HomeComponent> _homeComponents;

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var entityManager = state.EntityManager;
            var entities = state.EntityManager.GetAllEntities();

            if (_foodSupplies.Length == 0)
            {
                _foodSupplies = state.GetEntityQuery(new ComponentType[] {typeof(FoodSupplyComponent)})
                    .ToComponentDataArray<FoodSupplyComponent>(Allocator.Persistent);
                _ants = state.GetEntityQuery(new ComponentType[] {typeof(AntComponent)})
                    .ToComponentDataArray<AntComponent>(Allocator.Persistent);
                _homeComponents = state.GetEntityQuery(new ComponentType[] {typeof(HomeComponent)})
                    .ToComponentDataArray<HomeComponent>(Allocator.Persistent);
            }

            for (var index = 0; index < entities.Length; index++)
            {
                var entity = entities[index];
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

        [BurstCompile]
        private void ProcessFoodAndHome(EntityManager entityManager, Entity entity)
        {
            var ant = entityManager.GetComponentData<AntComponent>(entity);
            if (math.lengthsq(ant.Position - float3.zero) > 25 * 25)
            {
                ant.MoveDirection *= -1;
                entityManager.SetComponentData(entity, ant);
            }

            if (ant.SearchingForFood)
            {
                foreach (var foodSupply in _foodSupplies)
                {
                    if (math.lengthsq(ant.Position - foodSupply.Position) < 1.5 * 1.5)
                    {
                        var color = entityManager.GetComponentData<HDRPMaterialPropertyBaseColor>(entity);
                        ant.DistanceToFood = 0;
                        ant.MoveDirection *= -1;
                        color.Value = ant.BackHomeColor;
                        ant.SearchingForFood = false;
                        entityManager.SetComponentData(entity, color);
                        entityManager.SetComponentData(entity, ant);
                    }
                }
            }
            else
            {
                foreach (var homeComponent in _homeComponents)
                {
                    if (math.lengthsq(ant.Position - homeComponent.Position) < 1.5 * 1.5)
                    {
                        var color = entityManager.GetComponentData<HDRPMaterialPropertyBaseColor>(entity);
                        ant.DistanceToHome = 0;
                        ant.MoveDirection *= -1;
                        color.Value = ant.FoodSearchColor;
                        ant.SearchingForFood = true;
                        entityManager.SetComponentData(entity, color);
                        entityManager.SetComponentData(entity, ant);
                    }
                }
            }
        }
    }
}