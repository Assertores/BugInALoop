using System.Collections.Generic;
using BIAL.Runtime.DataStorage;
using BIAL.Runtime.DataStorage.Spawning;
using UnityEngine;

namespace BIAL.Runtime.Spawning
{
	public class TimeBasedSpawningDirector : SpawningDirector
	{
		private const int MAX_ATTEMPT_RETRIES = 3;
		[SerializeField] private TimeBasedSpawningDirectorConfig config = default;

		private float timeSinceLastSpawn;
		private float currentCredits;

		protected override void PostDirectorAwake()
		{
			currentCredits = config.StartCredits;
		}

		protected override void PostDirectorUpdate()
		{
			timeSinceLastSpawn += Time.deltaTime;
			currentCredits += CalculateCreditGain() * Time.deltaTime;
		}

		private float CalculateCreditGain()
		{
			return config.CreditsOverTime;
		}

		protected override bool ShouldSpawn()
		{
			return timeSinceLastSpawn >= config.SpawnAttemptFrequency;
		}

		protected override IEnumerable<EntityConfig> GetEntitiesToSpawn()
		{
			timeSinceLastSpawn = 0;
			int spawnCount = 0;
			int attemptRetries = 0;
			while ((spawnCount < config.MaxSpawnCountPerAttempt) && (attemptRetries < MAX_ATTEMPT_RETRIES))
			{
				EntityConfig targetEntity = config.PickRandomEntity(ValidEntityTarget);
				if (targetEntity)
				{
					attemptRetries = 0;
					spawnCount++;
					currentCredits -= targetEntity.DirectorCreditCost;

					yield return targetEntity;
				}
				else
				{
					attemptRetries++;
				}
			}
		}

		protected override bool ShouldDirectorDestroy()
		{
			return false;
		}

		private bool ValidEntityTarget(EntityConfig config)
		{
			return currentCredits >= config.DirectorCreditCost;
		}
	}
}