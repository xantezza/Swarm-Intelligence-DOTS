using Unity.Entities;
using Unity.Mathematics;

namespace _SwarmIntelligence.Extensions
{
    public static class RandomExtensions
    {
        public static float3 RandomNormalizedDirection(double seed)
        {
            return math.normalize(Random.CreateFromIndex((uint) (seed * 100f)).NextFloat3()
                                  - Random.CreateFromIndex((uint) (seed + 1f )).NextFloat3());
        }
        
        public static float3 RandomDirection(double seed)
        {
            return Random.CreateFromIndex((uint) (seed * 100f)).NextFloat3()
                                  - Random.CreateFromIndex((uint) (seed + 1f)).NextFloat3();
        }
    }
}