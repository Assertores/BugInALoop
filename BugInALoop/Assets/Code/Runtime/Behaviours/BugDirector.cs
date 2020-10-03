using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BIAL {
	public class BugDirector : MonoBehaviour {

		[SerializeField] SpawnStrategy strategy = null;
		[SerializeField] BugFactory factory = null;

		float timeCollapsed = 0;
		List<Bug> bugs = new List<Bug>();

		private void Start() {
			timeCollapsed = 0;
		}

		private void FixedUpdate() {
			timeCollapsed += Time.fixedDeltaTime;

			BugTesting();
			Spawning();
		}

		// may be checkt by the blocker instead
		void BugTesting() {
			for(int i = bugs.Count; i >= 0; i--) {
				if(bugs[i].CanLeavScreen()) {
					continue;
				}
				if(!bugs[i].IsAlone()) {
					continue;
				}


				bugs[i].Kill();
				bugs.RemoveAt(i);
			}
		}

		void Spawning() {
			while(strategy.ShouldSpawnNextBug(timeCollapsed)) {
				Bug element = factory.GetNextBug();
				strategy.SpawnNext(element);
				bugs.Add(element);
				element.Ready();
			}
		}
	}
}
