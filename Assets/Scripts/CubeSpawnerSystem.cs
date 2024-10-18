using DefaultNamespace;
using Unity.Entities;
using Unity.Burst;
using Unity.Collections;
using Unity.Mathematics;

[BurstCompile]
public partial struct CubeSpawnerSystem : ISystem
{
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        if (!SystemAPI.TryGetSingletonEntity<CubeSpawnerComponent>(out var spawnerEntity))
        {
            return;
        }

        RefRW<CubeSpawnerComponent> spawner = SystemAPI.GetComponentRW<CubeSpawnerComponent>(spawnerEntity);

        EntityCommandBuffer ecb = new EntityCommandBuffer(Allocator.Temp);
        if (spawner.ValueRO.nextSpawnTime < SystemAPI.Time.ElapsedTime)
        {
            Entity newEntity = ecb.Instantiate(spawner.ValueRO.prefab);

            var moveDirection = Random.CreateFromIndex((uint) (SystemAPI.Time.ElapsedTime / SystemAPI.Time.DeltaTime)).NextFloat3();
            ecb.AddComponent(newEntity, new CubeComponent {moveDirection = moveDirection, moveSpeed = 10});
            spawner.ValueRW.nextSpawnTime = (float) SystemAPI.Time.ElapsedTime + spawner.ValueRO.spawnRate;
            ecb.Playback(state.EntityManager);
        }
    }
}