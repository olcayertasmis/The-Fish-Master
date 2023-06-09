using System;
using UnityEngine;

namespace Fish
{
    public class FishSpawner : MonoBehaviour
    {
        [SerializeField] private Fish fishPrefab;
        [SerializeField] private Fish.FishType[] fishTypes;

        private void Awake()
        {
            for (int i = 0; i < fishTypes.Length; i++)
            {
                int num = 0;
                while (num < fishTypes[i].fishCount)
                {
                    Fish fish = UnityEngine.Object.Instantiate<Fish>(fishPrefab);
                    fish.Type = fishTypes[i];
                    fish.ResetFish();
                    num++;
                }
            }
        }
    }
}