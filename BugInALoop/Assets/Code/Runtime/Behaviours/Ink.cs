using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BIAL.Runtime {

	struct segment {
		public float proneTime;
		public GameObject segmentObject;
	}

	public class Ink : MonoBehaviour {

		[SerializeField] float lifeTime = 5;
		[SerializeField] GameObject segmentPrefab = null;

		Queue<segment> line = new Queue<segment>();
		

		Vector3 lastPos = new Vector3();

		void FixedUpdate() {
			while(line.Peek().proneTime < Time.time) {
				var element = line.Dequeue();

				Destroy(element.segmentObject); //Pooling?
			}
		}

		public void AddSegment(Vector3 endPos) {
			if(lastPos == endPos) {
				return;
			}
			segment element;

			element.proneTime = Time.time + lifeTime;
			element.segmentObject = null;
			if(line.Count > 0) {
				element.segmentObject = Instantiate(segmentPrefab); //Pooling?
				element.segmentObject.transform.position = lastPos;
				element.segmentObject.transform.rotation = Quaternion.LookRotation(endPos - lastPos, Vector3.up);
				element.segmentObject.transform.localScale = new Vector3(1.0f, 1.0f, (endPos - lastPos).magnitude);
			}

			line.Enqueue(element);
		}
	}
}
