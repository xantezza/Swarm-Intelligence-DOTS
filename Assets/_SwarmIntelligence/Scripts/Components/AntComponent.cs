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
        
        public uint DistanceToFood;
        public uint DistanceToHome;
        
        
        public float4 FoodSearchColor;
        public float4 BackHomeColor;
    }
}