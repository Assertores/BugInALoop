using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using AsserTOOLres;

namespace BIAL {
	public class IntToText : MonoBehaviour {

		[SerializeField] TextMeshProUGUI target = null;
		[SerializeField] OIntIdentifyer type = OIntIdentifyer.bugCatched;

		void Start() {
			if(!target) {
				Debug.LogError("[IntToText: " + gameObject.name + "] target reference not set!");
				Destroy(this);
				return;
			}

			OnChange();
			BehaviourFacade.s_instance.ints[(int)type] += OnChange;
		}

		void OnChange() {
			target.text = BehaviourFacade.s_instance.ints[(int)type].ToString();
		}
	}
}
