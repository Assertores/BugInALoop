using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BIAL {
	public class BugDirector : MonoBehaviour {

		[SerializeField] SpawnStrategy strategy = null;
		[SerializeField] BugFactory factory = null;

		float timeCollapsed = 0;

		private void Start() {
			timeCollapsed = 0;
		}

		private void FixedUpdate() {
			timeCollapsed += Time.fixedDeltaTime;

			while(strategy.ShouldSpawnNextBug(timeCollapsed)) {
				strategy.SpawnNext(factory.GetNextBug());
			}
		}
	}
}
