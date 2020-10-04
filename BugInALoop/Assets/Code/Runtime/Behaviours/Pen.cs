using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AsserTOOLres;

namespace BIAL.Runtime {
	public class Pen : MonoBehaviour {

		[SerializeField] Ink ingameInk = null;
		[SerializeField] Ink menuInk = null;
		[SerializeField] Paper ingamePaper = null;
		[SerializeField] Paper menuPaper = null;

		Ink currentInk = null;
		Paper currentPaper = null;

		bool isAlreadyDrawing = false;

		private void Awake() {
			InputAdapter.s_instance.startDrawing += StartDrawing;
			InputAdapter.s_instance.currentPos += OnChange;
			BehaviourFacade.s_instance.currentScene += ChangeSzene;
			ChangeSzene(BehaviourFacade.s_instance.currentScene);
		}

		private void OnDestroy() {
			if(InputAdapter.Exists()) {
				InputAdapter.s_instance.startDrawing -= StartDrawing;
				InputAdapter.s_instance.currentPos -= OnChange;
			}
			if(BehaviourFacade.Exists()) {
				BehaviourFacade.s_instance.currentScene -= ChangeSzene;
			}
		}

		private void FixedUpdate() {
			if(isAlreadyDrawing) {
				float inkUsage = ingameInk.AddSegment(transform.position);
				if(BehaviourFacade.s_instance.currentScene.value == Scene.game) {
					BehaviourFacade.s_instance.floats[(int)OFloatIdentifyer.ink].value -= inkUsage;
				}
			}
		}

		void StartDrawing() {
			ingameInk.Clear();
			isAlreadyDrawing = true;
		}

		void OnChange(Observable<Vector2> element) {
			if(!currentPaper) {
				return;
			}
			transform.position = currentPaper.GetPointOnPaper(Camera.main.ScreenPointToRay(element.value));
		}

		void ChangeSzene(Observable<Scene> szene) {
			switch(szene.value) {
			case Scene.menu:
				currentPaper = menuPaper;
				break;
			case Scene.game:
				currentInk = ingameInk;
				currentPaper = ingamePaper;
				return;
			case Scene.gameOver:
				ingameInk.Clear();
				break;
			default:
				break;
			}
			currentInk = menuInk;
			currentInk.Clear();
		}
	}
}
