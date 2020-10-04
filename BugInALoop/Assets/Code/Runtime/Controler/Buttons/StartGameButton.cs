using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BIAL.Runtime {
	public class StartGameButton : Button {
		public override void Execute() {
			BehaviourFacade.s_instance.CurrentScene.value = Scene.Game;
		}
	}
}
