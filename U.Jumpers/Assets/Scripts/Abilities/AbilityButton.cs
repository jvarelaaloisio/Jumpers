using Characters.Abilities.Events;
using UnityEngine;
using UnityEngine.UI;
using VarelaAloisio.UpdateManagement.Runtime;

namespace Abilities
{
	public class AbilityButton : MonoBehaviour
	{
		[SerializeField] private AbilityUnityEvent onTapButton;
		[SerializeField] private AbilitySo ability;
		[SerializeField] private Image icon;
		[SerializeField] private Button button;
		[SerializeField] private Image coolDownIcon;
		public int SceneIndex { get; set; }

		public void SetAbility(AbilitySo newAbility)
		{
			ability = newAbility;
			icon.sprite = ability.Icon;
		}

		public void OnTapButton()
		{
			onTapButton.Invoke(ability);
			button.interactable = false;
			coolDownIcon.gameObject.SetActive(true);
			new ActionOverTime(ability.CoolDown,
					lerp => coolDownIcon.fillAmount = 1 - lerp,
					() =>
					{
						button.interactable = true;
						coolDownIcon.gameObject.SetActive(false);
					},
					SceneIndex,
					true)
				.StartAction();
		}
	}
}