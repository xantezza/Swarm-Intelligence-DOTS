using _SwarmIntelligence.Components;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using UnityEngine;

namespace _SwarmIntelligence.Jobs
{
    public partial struct Test : ISystem, IJobParallelFor
    {
        public NativeArray<Entity> Entities;

        public void OnUpdate(ref SystemState state)
        {
        }

        public void Execute(int index)
        {
         //   Debug.LogError(SystemAPI.HasComponent<AntComponent>(Entities[index]));
        }
    }
}