using Unity.Entities;
using Unity.Mathematics;

namespace DefaultNamespace
{
    public class CubeComponent : IComponentData
    {
        public float3 moveDirection;
        public float moveSpeed;
    }
}