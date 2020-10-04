using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BIAL.Runtime {
	public class ExitButton : Button {
		public override void Execute() {
			Application.Quit();
#if UNITY_EDITOR
			Debug.Break();
#endif
		}
	}
}
