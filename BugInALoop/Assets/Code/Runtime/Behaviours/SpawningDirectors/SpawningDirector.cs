using System.Collections.Generic;
using BIAL.Runtime.DataStorage;
using BIAL.Singletons;
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
					EntityStateManager.AddActiveEntity(config.Factory.CreateNewEntity(config));
				}
			}
		}

		protected virtual void PostDirectorAwake() { }
		protected virtual void PostDirectorUpdate() { }
		protected virtual void DirectorWillDestroy() { }

		protected abstract bool ShouldSpawn();
		protected abstract IEnumerable<EntityConfig> GetEntitiesToSpawn();
		protected abstract bool ShouldDirectorDestroy();
	}
}