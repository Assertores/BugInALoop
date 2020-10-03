namespace BIAL.Runtime.DataStorage.Spawning
{
	public class TimeBasedSpawningDirectorConfig : SpawningDirectorConfig
	{
		public float StartCredits;
		public float CreditsOverTime;
		public float SpawnAttemptFrequency;

		public int MaxSpawnCountPerAttempt;
	}
}