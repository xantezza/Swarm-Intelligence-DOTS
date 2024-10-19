using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;

namespace _SwarmIntelligence.Components
{
    [BurstCompile]
    public struct FoodSupplyComponent : IComponentData
    {
        public float3 Position;

    }
}