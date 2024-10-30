using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;

namespace _SwarmIntelligence.Components
{
    [BurstCompile]
    public struct AntComponent : IComponentData
    {
        public float TalkRange;
        public float MoveSpeed;
        
        public float3 MoveDirection;
        public float3 Position;
        public bool SearchingForFood;
        
        public float DistanceToFood;
        public float DistanceToHome;
        public uint Seed;

        public float4 FoodSearchColor;
        public float4 BackHomeColor;
    }
}