using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AsserTOOLres;

namespace BIAL.Runtime {
	public class Pen : MonoBehaviour {

		[SerializeField] Ink ink = null;
		[SerializeField] Transform bottemLeftCorner = null;
		[SerializeField] Transform topRightCorner = null;

		bool isAlreadyDrawing = false;

		private void Awake() {
			InputAdapter.s_instance.startDrawing += StartDrawing;
			InputAdapter.s_instance.currentPos += OnChange;
		}

		private void FixedUpdate() {
			if(isAlreadyDrawing) {
				ink.AddSegment(transform.position);
			}
		}

		void StartDrawing() {
			isAlreadyDrawing = true;
		}

		void OnChange(Observable<Vector2> element) {

			if(Physics.Raycast(Camera.main.ScreenPointToRay(element.value), out RaycastHit hit)) {
				if(hit.point.x < bottemLeftCorner.position.x ||
					hit.point.x > topRightCorner.position.x ||
					hit.point.y < bottemLeftCorner.position.y ||
					hit.point.y > topRightCorner.position.y) {
					return;
				}

				transform.position = hit.point;
			}
		}
	}
}
