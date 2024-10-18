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
        public Entity Ant;
        public float4 AntSearchColor;
        public float4 AntBackColor;
    }
}