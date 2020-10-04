using System;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace BIAL.Runtime.DataStorage.Spawning
{
	public abstract class SpawningDirectorConfig : ScriptableObject
	{
		public SpawnableEntity[] SpawnableEntities;

		public EntityConfig PickRandomEntity(Func<EntityConfig, bool> condition = null)
		{
			if (SpawnableEntities.Length == 0)
			{
				return null;
			}

			float totalWeight = SpawnableEntities.Sum(target => target.SpawnChanceWeight);
			if (totalWeight <= 0)
			{
				return SpawnableEntities[0].Target;
			}

			float randomPoint = Random.value;
			foreach (SpawnableEntity entity in SpawnableEntities)
			{
				randomPoint -= entity.SpawnChanceWeight / totalWeight;
				if (randomPoint <= 0)
				{
					if ((condition == null) || condition.Invoke(entity.Target))
					{
						return entity.Target;
					}
					else
					{
						return null;
					}
				}
			}

			return null;
		}
	}

	[Serializable]
	public class SpawnableEntity
	{
		public EntityConfig Target;
		public float SpawnChanceWeight;
	}
}