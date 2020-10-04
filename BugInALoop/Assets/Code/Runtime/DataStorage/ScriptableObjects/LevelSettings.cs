using BIAL.Runtime.Spawning;
using UnityEngine;

namespace BIAL.Runtime.DataStorage
{
	public class LevelSettings : ScriptableObject
	{
		public static LevelSettings Current { get; set; }

		public Rect EntityTargetWalkRect;
		public Rect NonSpawnableRect;
		public float SpawnDistance;
		public float EntitySpawnHeight;
		public SpawningDirector[] StartSpawningDirectors;
	}
}