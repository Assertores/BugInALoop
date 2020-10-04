using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AsserTOOLres;

namespace BIAL.Runtime {
	public class Pen : MonoBehaviour {

		[SerializeField] Ink ingameInk = null;
		[SerializeField] Ink menuInk = null;
		[SerializeField] Paper ingamePaper = null;

		public static System.Action s_changedBlocker;

		Ink currentInk = null;

		bool isAlreadyDrawing = false;

		private void Awake() {
			InputAdapter.s_instance.startDrawing += StartDrawing;
			InputAdapter.s_instance.currentPos += OnChange;
			BehaviourFacade.s_instance.CurrentScene += ChangeSzene;
			ChangeSzene(BehaviourFacade.s_instance.CurrentScene);
		}

		private void OnDestroy() {
			if(InputAdapter.Exists()) {
				InputAdapter.s_instance.startDrawing -= StartDrawing;
				InputAdapter.s_instance.currentPos -= OnChange;
			}
			if(BehaviourFacade.Exists()) {
				BehaviourFacade.s_instance.CurrentScene -= ChangeSzene;
			}
		}

		private void FixedUpdate() {
			if(isAlreadyDrawing) {
				float inkUsage = currentInk.AddSegment(transform.position);
				if(BehaviourFacade.s_instance.CurrentScene.value == Scene.Game) {
					BehaviourFacade.s_instance.Floats[(int)OFloatIdentifier.Ink].value -= inkUsage;
				}
			}
		}

		void StartDrawing() {
			currentInk.Clear();
			isAlreadyDrawing = true;
		}

		void OnChange(Observable<Vector2> element) {
			if(!ingamePaper) {
				return;
			}
			transform.position = ingamePaper.GetPointOnPaper(Camera.main.ScreenPointToRay(element.value));
		}

		void ChangeSzene(Observable<Scene> szene) {
			currentInk?.Clear();
			if(szene.value == Scene.Game) {
				currentInk = ingameInk;
			} else {
				currentInk = menuInk;
			}
			isAlreadyDrawing = false;
			InputAdapter.s_instance.DoReset();
		}
	}
}
