using TMPro;
using UnityEngine;

namespace BIAL.Runtime
{
	public class IntToText : MonoBehaviour
	{
		[SerializeField] private TextMeshProUGUI target = null;
		[SerializeField] private OIntIdentifier type = OIntIdentifier.BugCaught;

		private void Start()
		{
			if (!target)
			{
				Debug.LogError("[IntToText: " + gameObject.name + "] target reference not set!");
				Destroy(this);

				return;
			}

			OnChange();
			BehaviourFacade.s_instance.Integers[(int) type] += OnChange;
		}

		private void OnChange()
		{
			target.text = BehaviourFacade.s_instance.Integers[(int) type].value.ToString();
		}
	}
}