using TMPro;
using UnityEngine;

namespace BIAL.Runtime
{
	public class FloatToText : MonoBehaviour
	{
		[SerializeField] private TextMeshProUGUI target = null;
		[SerializeField] private OFloatIdentifier type = OFloatIdentifier.Ink;

		private void Start()
		{
			if (!target)
			{
				Debug.LogError("[FloatToText: " + gameObject.name + "] target reference not set!");
				Destroy(this);

				return;
			}

			OnChange();
			BehaviourFacade.s_instance.Floats[(int) type] += OnChange;
		}

		private void OnChange()
		{
			target.text = BehaviourFacade.s_instance.Floats[(int) type].value.ToString();
		}
	}
}