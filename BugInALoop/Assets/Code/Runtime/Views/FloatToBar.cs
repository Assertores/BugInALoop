using UnityEngine;
using UnityEngine.UI;

namespace BIAL.Runtime
{
	public class FloatToBar : MonoBehaviour
	{
		[SerializeField] private Image bar = null;
		[SerializeField] private OFloatIdentifier type = OFloatIdentifier.Ink;
		[SerializeField] private OFloatIdentifier maxValue = OFloatIdentifier.MaxInk;

		private void Start()
		{
			if (!bar)
			{
				Debug.LogError("[FloatToBar: " + gameObject.name + "] bar reference not set!");
				Destroy(this);

				return;
			}

			OnChange();
			BehaviourFacade.s_instance.Floats[(int) type] += OnChange;
		}

		private void OnDestroy() {
			if(BehaviourFacade.Exists()) {
				BehaviourFacade.s_instance.Floats[(int)type] -= OnChange;
			}
		}

		private void OnChange()
		{
			bar.fillAmount = Mathf.Clamp(BehaviourFacade.s_instance.Floats[(int) type].value / BehaviourFacade.s_instance.Floats[(int)maxValue].value, 0, 1);
		}
	}
}