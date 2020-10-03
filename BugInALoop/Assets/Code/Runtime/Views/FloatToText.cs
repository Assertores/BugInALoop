using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using AsserTOOLres;

namespace BIAL {
	public class FloatToText : MonoBehaviour {

		[SerializeField] TextMeshProUGUI target = null;
		[SerializeField] OFloatIdentifyer type = OFloatIdentifyer.ink;

		void Start() {
			if(!target) {
				Debug.LogError("[FloatToText: " + gameObject.name + "] target reference not set!");
				Destroy(this);
				return;
			}

			OnChange();
			BehaviourFacade.s_instance.floats[(int)type] += OnChange;
		}

		void OnChange() {
			target.text = BehaviourFacade.s_instance.floats[(int)type].ToString();
		}
	}
}
