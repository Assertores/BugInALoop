using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Paper : MonoBehaviour {
	[Header("Rotatiion is rotation of paper. scalle is size of paper. (0 to 1)")]

	Plane plane = new Plane();

	private void Awake() {
		plane = new Plane(transform.up, transform.position);
	}

	public Vector3 GetPointOnPaper(Ray ray) {
		var point = new Vector3();
		if(plane.Raycast(ray, out float dist)) {
			point = ray.GetPoint(dist);
		}

		point = transform.InverseTransformPoint(point);
		point.x = Mathf.Clamp01(point.x);
		point.z = Mathf.Clamp01(point.z);
		return transform.TransformPoint(point);
	}
}
