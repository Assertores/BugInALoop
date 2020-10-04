using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AsserTOOLres;

namespace BIAL.Runtime 
{
	public enum OIntIdentifier : int {
		BugCaught = 0,
		Size
	}

	public enum OFloatIdentifier : int {
		Ink = 0,
		Size
	}

	public enum Scene : int {
		StartUp = 0,
		Menu,
		Game,
		gameOver,
		Size
	}

	public class BehaviourFacade : Singleton<BehaviourFacade> {

		public readonly Observable<int>[] Integers = new Observable<int>[(int)OIntIdentifier.Size];
		public readonly Observable<float>[] Floats = new Observable<float>[(int)OFloatIdentifier.Size];
		public Observable<Scene> CurrentScene = new Observable<Scene>();

		protected override void OnMyAwake() {
			for(int i = 0; i < Integers.Length; i++) {
				Integers[i] = new Observable<int>();
			}
			for(int i = 0; i < Floats.Length; i++) {
				Floats[i] = new Observable<float>();
			}
		}
	}
}
