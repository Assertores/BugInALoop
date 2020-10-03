using UnityEngine;
using UnityEngine.UI;

namespace BIAL.Runtime
{
	public class FloatToBar : MonoBehaviour
	{
		[SerializeField] private Image bar = null;
		[SerializeField] private OFloatIdentifyer type = OFloatIdentifyer.ink;
		[SerializeField] private float maxValue = 0;
		[SerializeField] private bool shouldScale = true;

		private void Start()
		{
			if (!bar)
			{
				Debug.LogError("[FloatToBar: " + gameObject.name + "] bar reference not set!");
				Destroy(this);

				return;
			}

			OnChange();
			BehaviourFacade.s_instance.floats[(int) type] += OnChange;
		}

		private void OnChange()
		{
			if (shouldScale && (BehaviourFacade.s_instance.floats[(int) type].value > maxValue))
			{
				maxValue = BehaviourFacade.s_instance.floats[(int) type].value;
			}

			bar.fillAmount = Mathf.Clamp(BehaviourFacade.s_instance.floats[(int) type].value / maxValue, 0, 1);
		}
	}
}