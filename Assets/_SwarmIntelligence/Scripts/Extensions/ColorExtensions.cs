using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Rendering;
using Unity.Transforms;
using UnityEngine;

namespace _SwarmIntelligence.Extensions
{
    public static class ColorExtensions
    {
        public static float4 ToFloat4(this Color color)
        {
            return new float4(color.r, color.g, color.b, color.a);
        }

        public static void Colorize(this Entity entity, EntityManager entityManager, float4 color)
        {
            entityManager.AddComponentData(entity, new HDRPMaterialPropertyBaseColor {Value = color});
            var buffer = entityManager.GetBuffer<Child>(entity).ToNativeArray(Allocator.Temp);

            foreach (var child in buffer)
            {
                entityManager.AddComponentData(child.Value, new HDRPMaterialPropertyBaseColor {Value = color});
            }
        }
    }
}