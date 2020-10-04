using System.Collections.Generic;
using BIAL.Runtime.DataStorage;
using BIAL.Runtime.Entities;
using BIAL.Runtime.Singletons;
using UnityEngine;

namespace BIAL.Runtime.Spawning
{
	public abstract class SpawningDirector : MonoBehaviour
	{
		private bool directorWillDestroy;

		protected void Awake()
		{
			UpdateDirector();
			if (!directorWillDestroy)
			{
				PostDirectorAwake();
			}
		}

		protected void Update()
		{
			UpdateDirector();
			if (!directorWillDestroy)
			{
				PostDirectorUpdate();
			}
		}

		private void UpdateDirector()
		{
			TrySpawn();
			if (ShouldDirectorDestroy())
			{
				ForceDestroyDirector();
			}
		}

		private void TrySpawn()
		{
			if (ShouldSpawn())
			{
				foreach (EntityConfig config in GetEntitiesToSpawn())
				{
					CreateNewEntity(config);
				}
			}
		}

		public void ForceDestroyDirector()
		{
			DirectorWillDestroy();
			directorWillDestroy = true;
			Destroy(gameObject);
		}

		private static void CreateNewEntity(EntityConfig config)
		{
			Entity newEntity = config.Factory.CreateNewEntity(config);
			Vector2 startPosition = GetRandomSpawnPosition();
			Vector2 walkPosition = GetRandomWalkPosition();
			Vector2 posDifference = walkPosition - startPosition;
			newEntity.transform.position = new Vector3(startPosition.x, LevelSettings.Current.EntitySpawnHeight, startPosition.y);
			newEntity.transform.forward = posDifference.XZtoXYZ().normalized;
			EntityStateManager.AddActiveEntity(newEntity);
		}

		private static Vector2 GetRandomSpawnPosition()
		{
			Vector2 center = (LevelSettings.Current.NonSpawnableRectLowerPoint + LevelSettings.Current.NonSpawnableRectUpperPoint) / 2;
			Vector2 rectSize = LevelSettings.Current.NonSpawnableRectUpperPoint - LevelSettings.Current.NonSpawnableRectLowerPoint;
			bool verticalSide = Random.value > 0.5f;
			int sideMul = Random.value > 0.5f ? 1 : -1;
			float xVal;
			float yVal;
			if (verticalSide)
			{
				xVal = Random.Range((-rectSize.x / 2) - LevelSettings.Current.SpawnDistance,
									(rectSize.x  / 2) + LevelSettings.Current.SpawnDistance);

				yVal = (rectSize.y + (LevelSettings.Current.SpawnDistance * Random.value)) * sideMul;
			}
			else
			{
				xVal = (rectSize.x + (LevelSettings.Current.SpawnDistance * Random.value)) * sideMul;
				yVal = Random.Range((-rectSize.y / 2) - LevelSettings.Current.SpawnDistance,
									(rectSize.y  / 2) + LevelSettings.Current.SpawnDistance);
			}

			return center + new Vector2(xVal, yVal);
		}

		private static Vector2 GetRandomWalkPosition()
		{
			Vector2 center = (LevelSettings.Current.EntityWalkRectLowerPoint + LevelSettings.Current.EntityWalkRectUpperPoint) / 2;
			Vector2 rectSize = LevelSettings.Current.EntityWalkRectUpperPoint - LevelSettings.Current.EntityWalkRectLowerPoint;
			float lowerX = center.x                                           - (rectSize.x / 2);
			float upperX = center.x                                           + (rectSize.x / 2);
			float lowerY = center.y                                           - (rectSize.y / 2);
			float upperY = center.y                                           + (rectSize.y / 2);

			return new Vector2(Random.Range(lowerX, upperX), Random.Range(lowerY, upperY));
		}

		protected virtual void PostDirectorAwake() { }
		protected virtual void PostDirectorUpdate() { }
		protected virtual void DirectorWillDestroy() { }

		protected abstract bool ShouldSpawn();
		protected abstract IEnumerable<EntityConfig> GetEntitiesToSpawn();
		protected abstract bool ShouldDirectorDestroy();
	}
}