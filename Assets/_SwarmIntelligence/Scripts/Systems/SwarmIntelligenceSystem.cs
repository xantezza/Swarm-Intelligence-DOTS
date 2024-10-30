using _SwarmIntelligence.Components;
using _SwarmIntelligence.Jobs;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;

namespace _SwarmIntelligence.Systems
{
    [BurstCompile]
    public partial class SwarmIntelligenceSystem : SystemBase
    {
        private NativeArray<AntComponent> _antComponents;
        private NativeArray<FoodSupplyComponent> _foodSupplyComponents;
        private NativeArray<HomeComponent> _homeComponents;

        [BurstCompile]
        protected override void OnUpdate()
        {
            _antComponents = GetEntityQuery(typeof(AntComponent)).ToComponentDataArray<AntComponent>(Allocator.TempJob);
            _foodSupplyComponents = GetEntityQuery(typeof(FoodSupplyComponent)).ToComponentDataArray<FoodSupplyComponent>(Allocator.TempJob);
            _homeComponents = GetEntityQuery(typeof(HomeComponent)).ToComponentDataArray<HomeComponent>(Allocator.TempJob);

            new SwarmEntityJob
            {
                AntComponents = _antComponents,
                FoodSupplyComponents = _foodSupplyComponents,
                HomeComponents = _homeComponents,
                DeltaTime = SystemAPI.Time.DeltaTime,
                ElapsedTime = SystemAPI.Time.ElapsedTime
            }.ScheduleParallel();
        }
    }
}