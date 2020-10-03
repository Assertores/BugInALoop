using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AsserTOOLres;

namespace BIAL {
	public enum OIntIdentifyer : int {
		bugCount,
	}

	public enum OFloatIdentifyer : int {
		ink,
	}

	public class BehaviourFacade : Singleton<BehaviourFacade> {

		public Observable<float>[] floats;
		public Observable<int>[] ints;
	}
}
