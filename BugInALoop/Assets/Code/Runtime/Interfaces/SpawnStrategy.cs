using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BIAL {
	public abstract class SpawnStrategy {
		public abstract bool ShouldSpawnNextBug(float time);
		public abstract void SpawnNext(Bug bug);
	}
}
