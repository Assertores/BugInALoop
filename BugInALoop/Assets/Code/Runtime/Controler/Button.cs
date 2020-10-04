using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace BIAL.Runtime {
	public abstract class Button : MonoBehaviour {

		static GameObject s_vanishPoint = null;

		[SerializeField] PlayRandomAudioSource audio = null;

		private void OnEnable() {
			Pen.s_changedBlocker += CheckForExecution;
			
		}

		private void OnDisable() {
			Pen.s_changedBlocker -= CheckForExecution;
		}

		void CheckForExecution() {
			if(!s_vanishPoint) {
				s_vanishPoint = GameObject.FindGameObjectWithTag("Finish");
			}

			var path = new NavMeshPath();
			if(NavMesh.CalculatePath(transform.position, s_vanishPoint.transform.position, NavMesh.AllAreas, path)) {
				if((path.corners[path.corners.Length - 1].XYZtoXZ() - s_vanishPoint.transform.position.XYZtoXZ()).sqrMagnitude < 0.01f) {
					return;
				}
			}
			audio?.Play();
			Execute();
		}

		public abstract void Execute();
	}
}
