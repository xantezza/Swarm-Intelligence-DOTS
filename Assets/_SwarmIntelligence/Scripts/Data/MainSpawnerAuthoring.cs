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
        public GameObject Ant;
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
                Home = GetEntity(authoring.Home, TransformUsageFlags.Renderable),
                HomeColor = authoring.HomeColor.ToFloat4(),
                Food = GetEntity(authoring.Food, TransformUsageFlags.Renderable),
                FoodColor = authoring.FoodColor.ToFloat4(),
                Ant = GetEntity(authoring.Ant, TransformUsageFlags.Renderable),
                AntSearchColor = authoring.AntSearchColor.ToFloat4(),
                AntBackColor = authoring.AntBackColor.ToFloat4(),
            });
        }
    }
}