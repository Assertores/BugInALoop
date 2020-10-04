using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BIAL.Runtime {

	class segment {
		public float proneTime;
		public GameObject segmentObject;
	}

	public class Ink : MonoBehaviour {

		[SerializeField] float lifeTime = 5;
		[SerializeField] GameObject segmentPrefab = null;

		Queue<segment> line = new Queue<segment>();

		bool isPaused = false;
		float pauseStartTime = 0;

		Vector3 lastPos = new Vector3();

		void FixedUpdate() {
			if(isPaused) {
				return;
			}
			bool hasRemovedSegemnts = false;
			while(line.Count > 0 && line.Peek().proneTime < Time.time) {
				RemoveSegment();
				hasRemovedSegemnts = true;
			}
			if(hasRemovedSegemnts) {
				Pen.s_changedBlocker?.Invoke();
			}
		}

		public float AddSegment(Vector3 endPos) {
			if(lastPos == endPos) {
				return 0;
			}
			segment element = new segment();
			float inkUsage = 0;

			element.proneTime = Time.time + lifeTime;
			element.segmentObject = null;
			if(line.Count > 0) {

				element.segmentObject = Instantiate(segmentPrefab); //Pooling?

				element.segmentObject.transform.position = lastPos;
				element.segmentObject.transform.rotation = Quaternion.LookRotation(endPos - lastPos, Vector3.up);
				//WARNING: prefab dependent stuff. do not use or copy this code
				//element.segmentObject.transform.localScale = new Vector3(1.0f, 1.0f, (endPos - lastPos).magnitude);
				element.segmentObject.transform.GetChild(0).localScale = new Vector3(1.0f, 1.0f, (endPos - lastPos).magnitude);
				element.segmentObject.transform.GetChild(1).localPosition *= (endPos - lastPos).magnitude;

				inkUsage = (endPos - lastPos).magnitude;

				Pen.s_changedBlocker?.Invoke();
			}

			line.Enqueue(element);
			lastPos = endPos;
			return inkUsage;
		}

		public void Clear() {
			while(line.Count > 0) {
				RemoveSegment();
			}
		}

		public void Pause() {
			isPaused = true;
			pauseStartTime = Time.time;
		}

		public void Resume() {
			pauseStartTime = Time.time - pauseStartTime;
			foreach(var it in line) {
				it.proneTime += pauseStartTime;
			}

			isPaused = false;
		}

		void RemoveSegment() {
			var element = line.Dequeue();
			Destroy(element.segmentObject); //Pooling?
		}
	}
}
