using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AsserTOOLres;

namespace BIAL.Runtime {
	public class Pen : MonoBehaviour {

		[SerializeField] Ink ink = null;
		[SerializeField] Transform bottemLeftCorner = null;
		[SerializeField] Transform topRightCorner = null;
		[SerializeField] LayerMask mask = new LayerMask();

		bool isAlreadyDrawing = false;

		private void Awake() {
			InputAdapter.s_instance.startDrawing += StartDrawing;
			InputAdapter.s_instance.currentPos += OnChange;
		}

		private void FixedUpdate() {
			if(isAlreadyDrawing) {
				float inkUsage = ink.AddSegment(transform.position);
				if(BehaviourFacade.s_instance.currentScene.value == Scene.game) {
					BehaviourFacade.s_instance.floats[(int)OFloatIdentifyer.ink].value -= inkUsage;
				}
			}
		}

		void StartDrawing() {
			ink.Clear();
			isAlreadyDrawing = true;
		}

		void OnChange(Observable<Vector2> element) {

			if(Physics.Raycast(Camera.main.ScreenPointToRay(element.value), out RaycastHit hit, 1000.0f, mask)) {
#if Obsolete
				if(hit.point.x < bottemLeftCorner.position.x ||
					hit.point.x > topRightCorner.position.x ||
					hit.point.z < bottemLeftCorner.position.z ||
					hit.point.z > topRightCorner.position.z) {
					return;
				}
				transform.position = hit.point;
#else
				transform.position = new Vector3(
					Mathf.Clamp(hit.point.x, bottemLeftCorner.position.x, topRightCorner.position.x),
					hit.point.y,
					Mathf.Clamp(hit.point.z, bottemLeftCorner.position.z, topRightCorner.position.z));
#endif
			}
		}
	}
}
