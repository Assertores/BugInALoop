using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AsserTOOLres;

namespace BIAL {
	public class InputAdapter : Singleton<InputAdapter> {

		public Observable<Vector2> currentPos { get; private set; }
		public System.Action startDrawing;
		bool hasStarted = false;

		public void DoReset() {
			hasStarted = false;
		}

		void Update() {
			var newPos = new Vector2();

			if(!(HandleTouch(ref newPos) ||
				HandleGamePad(ref newPos))) {
				HandleMouse(ref newPos);
			}

			newPos.x = Mathf.Clamp(newPos.x, 0, Screen.width);
			newPos.y = Mathf.Clamp(newPos.y, 0, Screen.height);

			currentPos.value = newPos;
		}

		bool HandleTouch(ref Vector2 newPos) {
			if(Input.touchCount <= 0) {
				return false;
			}

			if(!hasStarted) {
				hasStarted = true;
				startDrawing?.Invoke();
			}

			newPos = Input.touches[0].position;

			return true;
		}

		bool HandleGamePad(ref Vector2 newPos) {
			if(Input.GetJoystickNames().Length <= 0) {
				return false;
			}

			if(!hasStarted
				&& (Input.GetButtonDown("joystick button 0")
				|| Input.GetButtonDown("joystick button 1")
				|| Input.GetButtonDown("joystick button 2")
				|| Input.GetButtonDown("joystick button 3"))) {
				hasStarted = true;
				startDrawing?.Invoke();
			}

			newPos.x = currentPos.value.x + Input.GetAxis("Horizontal");
			newPos.y = currentPos.value.y + Input.GetAxis("Vertical");

			return true;
		}

		bool HandleMouse(ref Vector2 newPos) {
			if(!hasStarted && (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))) {
				hasStarted = true;
				startDrawing?.Invoke();
			}

			var pos = Input.mousePosition;
			newPos.x = pos.x;
			newPos.y = pos.y;
			return true;
		}
	}
}
