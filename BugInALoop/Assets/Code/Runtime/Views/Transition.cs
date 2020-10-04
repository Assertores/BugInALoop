using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AsserTOOLres;

namespace BIAL.Singletons {
	public class Transition : Singleton<Transition> {
		Vector3 startPos;
		Quaternion startRot;
		Vector3 endPos;
		Quaternion endRot;
		float endTime = 0;
		float startTime = 0;
		bool isInTransition = false;

		public void TriggerTransition(Transform end, float time) {
			startPos = Camera.main.transform.position;
			startRot = Camera.main.transform.rotation;
			endPos = end.position;
			endRot = end.rotation;
			startTime = Time.time;
			endTime = startTime + time;
			isInTransition = true;
		}

		void Update() {
			if(!isInTransition) {
				return;
			}
			if(Time.time > endTime) {
				isInTransition = false;
				Camera.main.transform.position = endPos;
				Camera.main.transform.rotation = endRot;
				return;
			}

			float delta = (Time.time - startTime) / (endTime - startTime);
			delta = Mathf.Clamp01(delta);
			Camera.main.transform.position = Vector3.Lerp(startPos, endPos, delta);
			Camera.main.transform.rotation = Quaternion.Lerp(startRot, endRot, delta);
		}
	}
}
