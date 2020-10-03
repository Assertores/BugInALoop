using TMPro;
using UnityEngine;

namespace BIAL.Runtime
{
	public class IntToText : MonoBehaviour
	{
		[SerializeField] private TextMeshProUGUI target = null;
		[SerializeField] private OIntIdentifyer type = OIntIdentifyer.bugCatched;

		private void Start()
		{
			if (!target)
			{
				Debug.LogError("[IntToText: " + gameObject.name + "] target reference not set!");
				Destroy(this);

				return;
			}

			OnChange();
			BehaviourFacade.s_instance.ints[(int) type] += OnChange;
		}

		private void OnChange()
		{
			target.text = BehaviourFacade.s_instance.ints[(int) type].ToString();
		}
	}
}