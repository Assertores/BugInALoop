using System;
using System.Collections.Generic;
using BIAL.Entities;
using BIAL.Runtime.DataStorage;
using BIAL.Singletons;
using UnityEngine;
using Random = UnityEngine.Random;

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
				DirectorWillDestroy();
				directorWillDestroy = true;
				Destroy(gameObject);
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

		private static void CreateNewEntity(EntityConfig config)
		{
			Entity newEntity = config.Factory.CreateNewEntity(config);

			Vector2 startPosition = GetRandomSpawnPosition();
			Vector2 walkPosition = GetRandomWalkPosition();
			Vector2 posDifference = walkPosition - startPosition;
			
			newEntity.transform.position = new Vector3(startPosition.x, LevelSettings.Current.EntitySpawnHeight, startPosition.y);
			newEntity.transform.eulerAngles = new Vector3(0,0,Mathf.Atan2(posDifference.y, posDifference.x) * Mathf.Rad2Deg);

			EntityStateManager.AddActiveEntity(newEntity);
		}

		private static Vector2 GetRandomSpawnPosition()
		{
			float xOff = Random.value * LevelSettings.Current.SpawnDistance;
			float yOff = Random.value * LevelSettings.Current.SpawnDistance;

			int xSide = Random.value > 0.5f ? 1 : -1;
			int ySide = Random.value > 0.5f ? 1 : -1;
			
			Rect rect = LevelSettings.Current.NonSpawnableRect;
			return new Vector2(rect.position.x + rect.width / 2 * xSide + xOff * xSide,
								rect.position.y + rect.height / 2 * ySide + yOff * ySide);
		}

		private static Vector2 GetRandomWalkPosition()
		{
			Rect walkRect = LevelSettings.Current.EntityTargetWalkRect;
			float lowerX = walkRect.position.x - walkRect.width / 2;
			float upperX = walkRect.position.x + walkRect.width / 2;
			float lowerY = walkRect.position.y - walkRect.height / 2;
			float upperY = walkRect.position.y + walkRect.height / 2;
			
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