using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BIAL {
	public abstract class Bug : MonoBehaviour {
		public abstract void Ready();
		public abstract bool CanLeavScreen();
		public abstract bool IsAlone();
		public abstract void Kill();
	}
}
