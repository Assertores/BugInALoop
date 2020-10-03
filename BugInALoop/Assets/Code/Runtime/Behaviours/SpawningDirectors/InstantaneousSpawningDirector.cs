using System.Collections.Generic;
using BIAL.Runtime.DataStorage;
using BIAL.Runtime.DataStorage.Spawning;
using UnityEngine;

namespace BIAL.Runtime.Spawning
{
	public class InstantaneousSpawningDirector : SpawningDirector
	{
		private const int MAX_ATTEMPT_RETRIES = 6;

		[SerializeField] private InstantaneousSpawningDirectorConfig config = default;

		private float currentCredits;

		protected override IEnumerable<EntityConfig> GetEntitiesToSpawn()
		{
			int attemptRetries = 0;
			currentCredits = config.Credits;
			while (attemptRetries < MAX_ATTEMPT_RETRIES)
			{
				EntityConfig targetEntity = config.PickRandomEntity(ValidEntityTarget);
				if (targetEntity)
				{
					attemptRetries = 0;
					currentCredits -= targetEntity.DirectorCreditCost;

					yield return targetEntity;
				}
				else
				{
					attemptRetries++;
				}
			}
		}

		private bool ValidEntityTarget(EntityConfig config)
		{
			return currentCredits >= config.DirectorCreditCost;
		}

		protected override bool ShouldSpawn()
		{
			return true;
		}

		protected override bool ShouldDirectorDestroy()
		{
			return true;
		}
	}
}