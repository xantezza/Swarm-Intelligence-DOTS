using _SwarmIntelligence.Components;
using _SwarmIntelligence.Extensions;
using Unity.Entities;
using UnityEngine;

namespace _SwarmIntelligence.Data
{
    public class MainSpawnerAuthoring : MonoBehaviour
    {
        public GameObject Home;
        public Color HomeColor;
        public GameObject Food;
        public Color FoodColor;
        public float foodSpawnRate;
        public GameObject Ant;
        public int AntCount;
        public float AntMoveSpeed;
        public float AntTalkRange;
        public Color AntSearchColor;
        public Color AntBackColor;
    }
    
    public class MainSpawnerAuthoringBaker : Baker<MainSpawnerAuthoring>
    {
        public override void Bake(MainSpawnerAuthoring authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.None);
        
            AddComponent(entity, new MainSpawnerComponent
            {
                Home = GetEntity(authoring.Home, TransformUsageFlags.Dynamic),
                HomeColor = authoring.HomeColor.ToFloat4(),
                Food = GetEntity(authoring.Food, TransformUsageFlags.Dynamic),
                FoodColor = authoring.FoodColor.ToFloat4(),
                FoodSpawnRate = authoring.foodSpawnRate,
                NextFoodSpawnTime = 0f,
                Ant = GetEntity(authoring.Ant, TransformUsageFlags.Dynamic),
                AntCount = authoring.AntCount,
                AntMoveSpeed = authoring.AntMoveSpeed,
                AntSearchColor = authoring.AntSearchColor.ToFloat4(),
                AntBackColor = authoring.AntBackColor.ToFloat4(),
            });
        }
    }
}