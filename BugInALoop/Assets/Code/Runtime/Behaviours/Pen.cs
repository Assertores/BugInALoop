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
		Vector3 penTip = new Vector3();

		private void Awake() {
			InputAdapter.s_instance.startDrawing += StartDrawing;
			InputAdapter.s_instance.currentPos += OnChange;
		}

		private void FixedUpdate() {
			ink.AddSegment(penTip);
		}

		void StartDrawing() {
			isAlreadyDrawing = true;
		}

		void OnChange(Observable<Vector2> element) {
			if(!isAlreadyDrawing) {
				return;
			}

			if(Physics.Raycast(Camera.main.ScreenPointToRay(element.value), out RaycastHit hit)) {
				if(hit.point.x < bottemLeftCorner.position.x ||
					hit.point.x > topRightCorner.position.x ||
					hit.point.y < bottemLeftCorner.position.y ||
					hit.point.y > topRightCorner.position.y) {
					return;
				}

				penTip = hit.point;
			}
		}
	}
}
