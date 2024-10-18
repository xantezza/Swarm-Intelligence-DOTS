using Unity.Entities;
using Unity.Mathematics;

namespace _SwarmIntelligence.Components
{
    public struct AntComponent : IComponentData
    {
        public float TalkRange;
        public float MoveSpeed;
        
        public float3 MoveDirection;
    }
}