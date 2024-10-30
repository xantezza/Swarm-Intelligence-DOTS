using _SwarmIntelligence.Components;
using _SwarmIntelligence.Extensions;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Rendering;
using Unity.Transforms;

namespace _SwarmIntelligence.Systems
{
    [BurstCompile]
    public partial struct MainSpawnerSystem : ISystem
    {
        private bool _inited;

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            if (!_inited)
            {
                var entityManager = state.EntityManager;
                var entities = entityManager.GetAllEntities();

                foreach (var entity in entities)
                {
                    if (entityManager.HasComponent<MainSpawnerComponent>(entity))
                    {
                        var mainSpawner = entityManager.GetComponentData<MainSpawnerComponent>(entity);

                        SpawnHome(ref state, mainSpawner);
                        SpawnAnts(ref state, mainSpawner);
                        SpawnFood(ref state, ref mainSpawner);
                        _inited = true;
                    }
                }
            }
        }


        [BurstCompile]
        private void SpawnHome(ref SystemState state, MainSpawnerComponent spawner)
        {
            EntityCommandBuffer ecb = new EntityCommandBuffer(Allocator.Temp);
            Entity newEntity = ecb.Instantiate(spawner.Home);
            ecb.AddComponent(newEntity, new HDRPMaterialPropertyBaseColor {Value = spawner.HomeColor});
            ecb.AddComponent(newEntity, new HomeComponent() {Position = float3.zero});
            ecb.Playback(state.EntityManager);
        }

        [BurstCompile]
        private void SpawnAnts(ref SystemState state, MainSpawnerComponent spawner)
        {
            EntityCommandBuffer ecb = new EntityCommandBuffer(Allocator.Temp);

            for (int i = 0; i < spawner.AntCount; i++)
            {
                Entity newEntity = ecb.Instantiate(spawner.Ant);
                ecb.AddComponent(newEntity, new HDRPMaterialPropertyBaseColor {Value = spawner.AntSearchColor});
                var moveDirection = RandomExtensions.RandomNormalizedDirection(SystemAPI.Time.ElapsedTime + i);
                ecb.AddComponent(newEntity, new AntComponent
                {
                    TalkRange = spawner.AntTalkRange,
                    MoveSpeed = spawner.AntMoveSpeed,
                    MoveDirection = moveDirection,
                    SearchingForFood = true,
                    FoodSearchColor = spawner.AntSearchColor,
                    BackHomeColor = spawner.AntBackColor
                });
            }

            ecb.Playback(state.EntityManager);
        }

        [BurstCompile]
        private void SpawnFood(ref SystemState state, ref MainSpawnerComponent spawner)
        {
            if (spawner.NextFoodSpawnTime > SystemAPI.Time.ElapsedTime) return;

            EntityCommandBuffer ecb = new EntityCommandBuffer(Allocator.Temp);

            for (int i = 0; i < 1; i++)
            {
                Entity newEntity = ecb.Instantiate(spawner.Food);

                ecb.AddComponent(newEntity, new HDRPMaterialPropertyBaseColor {Value = spawner.FoodColor});
                var position = RandomExtensions.RandomNormalizedDirection(SystemAPI.Time.ElapsedTime + i) * 15;
                ecb.SetComponent(newEntity, new LocalTransform {Position = position, Rotation = quaternion.identity, Scale = 2});
                ecb.AddComponent(newEntity, new FoodSupplyComponent() {Position = position});
            }

            spawner.NextFoodSpawnTime = (float) SystemAPI.Time.ElapsedTime + spawner.FoodSpawnRate;
            ecb.Playback(state.EntityManager);
        }
    }
}