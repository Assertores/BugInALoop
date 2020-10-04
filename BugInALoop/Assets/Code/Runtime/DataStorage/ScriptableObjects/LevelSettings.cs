using BIAL.Runtime.Spawning;
using UnityEngine;

namespace BIAL.Runtime.DataStorage
{
	public class LevelSettings : ScriptableObject
	{
		public static LevelSettings Current { get; set; }

		public Vector2 EntityWalkRectLowerPoint;
		public Vector2 EntityWalkRectUpperPoint;
		public Vector2 NonSpawnableRectLowerPoint;
		public Vector2 NonSpawnableRectUpperPoint;

		public float SpawnDistance;
		public float EntitySpawnHeight;
		public SpawningDirector[] StartSpawningDirectors;
	}
}