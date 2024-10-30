using Unity.Entities;
using Unity.Mathematics;

namespace _SwarmIntelligence.Extensions
{
    public static class RandomExtensions
    {
        public static float3 RandomNormalizedDirection(uint seed)
        {
            return math.normalize(Random.CreateFromIndex(seed * 100).NextFloat3()
                                  - Random.CreateFromIndex(seed + 1).NextFloat3());
        }
        
        public static float3 RandomDirection(uint seed)
        {
            return Random.CreateFromIndex(seed * 100).NextFloat3()
                                  - Random.CreateFromIndex(seed + 1).NextFloat3();
        }
        
    }
}