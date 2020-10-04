using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BIAL.Runtime {
	public class StartTransition : MonoBehaviour {
		[SerializeField] Transform startPos = null;
		[SerializeField] Transform endPos = null;
		[SerializeField] float time = 0;

		void Start() {
			Camera.main.transform.position = startPos.position;
			Camera.main.transform.rotation = startPos.rotation;
			Singletons.Transition.s_instance.TriggerTransition(endPos, time);
			StartCoroutine(IEChangeSzene(Scene.menu, time));
		}

		IEnumerator IEChangeSzene(Scene scene, float delay) {
			yield return new WaitForSeconds(delay);
			BehaviourFacade.s_instance.currentScene.value = scene;
			Destroy(this);
		}
	}
}
