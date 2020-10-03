using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using AsserTOOLres;

namespace BIAL {
	public class FloatToBar : MonoBehaviour {

		[SerializeField] Image bar = null;
		[SerializeField] OFloatIdentifyer type = OFloatIdentifyer.ink;
		[SerializeField] float maxValue = 0;
		[SerializeField] bool shouldScale = true;

		void Start() {
			if(!bar) {
				Debug.LogError("[FloatToBar: " + gameObject.name + "] bar reference not set!");
				Destroy(this);
				return;
			}

			OnChange();
			BehaviourFacade.s_instance.floats[(int)type] += OnChange;
		}

		void OnChange() {
			if(shouldScale && BehaviourFacade.s_instance.floats[(int)type].value > maxValue) {
				maxValue = BehaviourFacade.s_instance.floats[(int)type].value;
			}
			bar.fillAmount = Mathf.Clamp(BehaviourFacade.s_instance.floats[(int)type].value / maxValue, 0, 1);
		}
	}
}
