using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AsserTOOLres;

namespace BIAL.Runtime {
	public class ActivateInScene : MonoBehaviour {
		[SerializeField] Scene scene = Scene.Size;
		private void Awake() {
			BehaviourFacade.s_instance.CurrentScene += OnChange;
			OnChange(BehaviourFacade.s_instance.CurrentScene);
		}

		private void OnDestroy() {
			if(BehaviourFacade.Exists()) {
				BehaviourFacade.s_instance.CurrentScene -= OnChange;
			}
		}

		void OnChange(Observable<Scene> element) {
			gameObject.SetActive(element.value == scene);
		}
	}
}
