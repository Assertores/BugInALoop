using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BIAL {
	public abstract class Bug {
		public abstract void Ready();
		public abstract bool CanLeavScreen();
		public abstract bool IsAlone();
		public abstract void Kill();
	}
}
