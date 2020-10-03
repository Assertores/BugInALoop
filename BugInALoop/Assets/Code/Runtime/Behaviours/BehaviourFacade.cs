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

	public class BehaviourFacade : Singleton<BehaviourFacade> {

		public Observable<int>[] ints = new Observable<int>[(int)OIntIdentifyer.size];
		public Observable<float>[] floats = new Observable<float>[(int)OFloatIdentifyer.size];
	}
}
