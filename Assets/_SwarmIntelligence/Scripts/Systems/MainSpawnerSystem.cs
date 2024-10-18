using _SwarmIntelligence.Components;
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
            if (!SystemAPI.TryGetSingletonEntity<MainSpawnerComponent>(out var spawnerEntity))
            {
                return;
            }

            RefRW<MainSpawnerComponent> mainSpawner = SystemAPI.GetComponentRW<MainSpawnerComponent>(spawnerEntity);

            var mainSpawnerRW = mainSpawner.ValueRW;
            if (!_inited)
            {
                _inited = true;
                SpawnHome(ref state, mainSpawnerRW);
                SpawnAnts(ref state, mainSpawnerRW);
            }

            SpawnFood(ref state, mainSpawnerRW);
        }

        [BurstCompile]
        private void SpawnHome(ref SystemState state, MainSpawnerComponent spawner)
        {
            EntityCommandBuffer ecb = new EntityCommandBuffer(Allocator.Temp);
            Entity newEntity = ecb.Instantiate(spawner.Home);
            ecb.AddComponent(newEntity, new HDRPMaterialPropertyBaseColor {Value = spawner.HomeColor});
            ecb.AddComponent(newEntity, new HomeComponent());
            ecb.Playback(state.EntityManager);
        }

        [BurstCompile]
        private void SpawnAnts(ref SystemState state, MainSpawnerComponent spawner)
        {
            for (int i = 0; spawner.AntCount < i; i++)
            {
                EntityCommandBuffer ecb = new EntityCommandBuffer(Allocator.Temp);
                Entity newEntity = ecb.Instantiate(spawner.Ant);
                ecb.AddComponent(newEntity, new HDRPMaterialPropertyBaseColor {Value = spawner.AntSearchColor});
                var moveDirection = math.normalize(Random.CreateFromIndex((uint) (SystemAPI.Time.ElapsedTime / SystemAPI.Time.DeltaTime)).NextFloat3()
                                                   - Random.CreateFromIndex((uint) (SystemAPI.Time.ElapsedTime + 1 / SystemAPI.Time.DeltaTime)).NextFloat3());
                ecb.AddComponent(newEntity, new AntComponent {TalkRange = spawner.AntTalkRange, MoveSpeed = spawner.AntMoveSpeed, MoveDirection = moveDirection});
                ecb.Playback(state.EntityManager);
            }
        }

        [BurstCompile]
        private void SpawnFood(ref SystemState state, MainSpawnerComponent spawner)
        {
            if (spawner.NextFoodSpawnTime > SystemAPI.Time.ElapsedTime) return;

            EntityCommandBuffer ecb = new EntityCommandBuffer(Allocator.Temp);

            Entity newEntity = ecb.Instantiate(spawner.Food);

            ecb.AddComponent(newEntity, new HDRPMaterialPropertyBaseColor {Value = spawner.FoodColor});
            ecb.AddComponent(newEntity, new FoodSupplyComponent());
            var position = math.normalize(Random.CreateFromIndex((uint) (SystemAPI.Time.ElapsedTime / SystemAPI.Time.DeltaTime)).NextFloat3()
                                          - Random.CreateFromIndex((uint) (SystemAPI.Time.ElapsedTime + 1 / SystemAPI.Time.DeltaTime)).NextFloat3()) * 3;
            ecb.SetComponent(newEntity, new LocalTransform {Position = position, Rotation = quaternion.identity, Scale = 1});
            spawner.NextFoodSpawnTime = (float) SystemAPI.Time.ElapsedTime + spawner.FoodSpawnRate;
            ecb.Playback(state.EntityManager);
        }
    }
}