using Unity.Entities;
using Unity.Burst;
using Unity.Collections;

[BurstCompile]
public partial struct CubeSpawnerSystem : ISystem
{
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        return;
        if (!SystemAPI.TryGetSingletonEntity<CubeSpawnerComponent>(out var spawnerEntity))
        {
            return;
        }

        RefRW<CubeSpawnerComponent> spawner = SystemAPI.GetComponentRW<CubeSpawnerComponent>(spawnerEntity);

        EntityCommandBuffer ecb = new EntityCommandBuffer(Allocator.Temp);
        if (spawner.ValueRO.nextSpawnTime < SystemAPI.Time.ElapsedTime)
        {
            Entity newEntity = ecb.Instantiate(spawner.ValueRO.prefab);
            spawner.ValueRW.nextSpawnTime = (float) SystemAPI.Time.ElapsedTime + spawner.ValueRO.spawnRate;
            ecb.Playback(state.EntityManager);
        }
    }
} 