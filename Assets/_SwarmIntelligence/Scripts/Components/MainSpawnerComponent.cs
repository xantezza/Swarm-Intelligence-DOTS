using Unity.Entities;
using Unity.Mathematics;

namespace _SwarmIntelligence.Components
{
    public struct MainSpawnerComponent : IComponentData
    {
        public Entity Home;
        public float4 HomeColor;
        public Entity Food;
        public float4 FoodColor;
        public float FoodSpawnRate;
        public float NextFoodSpawnTime;
        public Entity Ant;
        public int AntCount;
        public float AntTalkRange;
        public float AntMoveSpeed;
        public float4 AntSearchColor;
        public float4 AntBackColor;
    }
}