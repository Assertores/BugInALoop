using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AsserTOOLres;

namespace BIAL {
	public enum OIntIdentifyer : int {
		bugCatched = 0,
		size
	}

	public enum OFloatIdentifyer : int {
		ink = 0,
		size
	}

	public enum Scene : int {
		startUp = 0,
		menu,
		game,
		gameOver,
		size
	}

	public class BehaviourFacade : Singleton<BehaviourFacade> {

		public Observable<int>[] ints = new Observable<int>[(int)OIntIdentifyer.size];
		public Observable<float>[] floats = new Observable<float>[(int)OFloatIdentifyer.size];
		public Observable<Scene> currentScene = new Observable<Scene>();

		protected override void OnMyAwake() {
			for(int i = 0; i < ints.Length; i++) {
				ints[i] = new Observable<int>();
			}
			for(int i = 0; i < floats.Length; i++) {
				floats[i] = new Observable<float>();
			}
		}
	}
}
