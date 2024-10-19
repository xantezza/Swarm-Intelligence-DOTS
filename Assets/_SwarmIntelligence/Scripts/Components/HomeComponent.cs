using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;

namespace _SwarmIntelligence.Components
{
    [BurstCompile]
    public struct HomeComponent : IComponentData
    {
        public float3 Position;
    }
}