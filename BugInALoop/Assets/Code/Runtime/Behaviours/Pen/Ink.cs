using System.Collections.Generic;
using UnityEngine;

namespace BIAL.Runtime
{
	internal class segment
	{
		public float proneTime;
		public GameObject segmentObject;
	}

	public class Ink : MonoBehaviour
	{
		private const float SEGMENT_DISTANCE_THRESHOLD = 0.0001f;
		[SerializeField] private float lifeTime = 5;
		[SerializeField] private GameObject segmentPrefab = null;

		private readonly Queue<segment> line = new Queue<segment>();

		private bool isPaused = false;
		private float pauseStartTime = 0;

		private Vector3 lastPos = new Vector3();

		private void FixedUpdate()
		{
			if (isPaused)
			{
				return;
			}

			bool removedSegments = false;
			while ((line.Count > 0) && (line.Peek().proneTime < Time.time))
			{
				RemoveSegment();
				removedSegments = true;
			}

			if (removedSegments)
			{
				Pen.s_changedBlocker?.Invoke();
			}
		}

		public float AddSegment(Vector3 endPos)
		{
			float distance = (lastPos - endPos).sqrMagnitude;
			if (distance < SEGMENT_DISTANCE_THRESHOLD)
			{
				return 0;
			}

			distance = Mathf.Sqrt(distance);
			segment element = new segment();
			float inkUsage = 0;
			element.proneTime = Time.time + lifeTime;
			element.segmentObject = null;
			if (line.Count > 0)
			{
				element.segmentObject = Instantiate(segmentPrefab); //Pooling?
				element.segmentObject.transform.position = lastPos;
				element.segmentObject.transform.rotation = Quaternion.LookRotation(endPos - lastPos, Vector3.up);

				//WARNING: prefab dependent stuff. do not use or copy this code
				//element.segmentObject.transform.localScale = new Vector3(1.0f, 1.0f, (endPos - lastPos).magnitude);
				element.segmentObject.transform.GetChild(0).localScale = new Vector3(1.0f, 1.0f, distance);
				element.segmentObject.transform.GetChild(1).localPosition *= distance;
				inkUsage = distance;
			}

			line.Enqueue(element);
			lastPos = endPos;
			Pen.s_changedBlocker?.Invoke();

			return inkUsage;
		}

		public void Clear()
		{
			while (line.Count > 0)
			{
				RemoveSegment();
			}
		}

		public void Pause()
		{
			isPaused = true;
			pauseStartTime = Time.time;
		}

		public void Resume()
		{
			pauseStartTime = Time.time - pauseStartTime;
			foreach (segment it in line)
			{
				it.proneTime += pauseStartTime;
			}

			isPaused = false;
		}

		private void RemoveSegment()
		{
			segment element = line.Dequeue();
			Destroy(element.segmentObject); //Pooling?
		}
	}
}