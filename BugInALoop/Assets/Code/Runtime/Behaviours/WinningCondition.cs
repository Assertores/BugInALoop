using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BIAL.Runtime {
	public class WinningCondition : MonoBehaviour {
		[SerializeField] float startInk = 100;

		void Start() {
			BehaviourFacade.s_instance.Floats[(int)OFloatIdentifier.Ink] += CheckCondition;
			BehaviourFacade.s_instance.CurrentScene += CheckStartGame;
		}

		private void OnDestroy() {
			if(BehaviourFacade.Exists()) {
				BehaviourFacade.s_instance.Floats[(int)OFloatIdentifier.Ink] -= CheckCondition;
				BehaviourFacade.s_instance.CurrentScene -= CheckStartGame;
			}
		}

		void CheckCondition() {
			if(BehaviourFacade.s_instance.CurrentScene.value != Scene.Game) {
				return;
			}

			if(BehaviourFacade.s_instance.Floats[(int)OFloatIdentifier.Ink].value <= 0) {
				BehaviourFacade.s_instance.CurrentScene.value = Scene.GameOver;
			}
		}

		void CheckStartGame() {
			if(BehaviourFacade.s_instance.CurrentScene.value != Scene.Game) {
				return;
			}

			BehaviourFacade.s_instance.Floats[(int)OFloatIdentifier.MaxInk].value = startInk;
			BehaviourFacade.s_instance.Floats[(int)OFloatIdentifier.Ink].value = startInk;
		}
	}
}
