using Characters.Abilities.Events;
using Events.Channels;
using UnityEngine;
using VarelaAloisio.UpdateManagement.Runtime;

namespace Abilities
{
	public class AbilityButtonManager : MonoBehaviour
	{
		[Header("Setup")] [SerializeField] private AbilityButton[] buttons;

		[SerializeField] private float buttonsTimeOff;
		[SerializeField] private int sceneIndex;
		

		[Space, Header("Channels Listened")]
		[SerializeField, Tooltip("Can Be Null")]
		private VoidChannelSo showButtons;

		[SerializeField, Tooltip("Can Be Null")]
		private VoidChannelSo hideButtons;

		[SerializeField, Tooltip("Not Null")] private AbilityChannelSo addAbility;

		private int _activeButtons = 0;
		private CountDownTimer _resumeButtons;

		private void Awake()
		{
			SetupButtons();
			hideButtons.SubscribeSafely(HideButtons);
			showButtons.SubscribeSafely(ShowButtons);
			addAbility.Subscribe(AddAbility);
			_resumeButtons = new CountDownTimer(buttonsTimeOff, ShowButtons, sceneIndex);
		}

		private void AddAbility(AbilitySo ability)
		{
			if (_activeButtons >= buttons.Length)
				return;
			_activeButtons++;
			buttons[_activeButtons - 1].SetAbility(ability);
			ability.onUse.AddListener(_ =>
			{
				HideButtons();
				_resumeButtons.StartTimer();
			});
		}

		private void SetupButtons()
		{
			for (int i = 0; i < _activeButtons; i++)
			{
				buttons[i].SceneIndex = sceneIndex;
			}
		}
		private void ShowButtons()
		{
			for (int i = 0; i < _activeButtons; i++)
			{
				buttons[i].gameObject.SetActive(true);
			}
		}

		private void HideButtons()
		{
			foreach (var b in buttons)
			{
				b.gameObject.SetActive(false);
			}
		}

		private void OnDestroy()
		{
			showButtons.UnSubscribe(ShowButtons);
			hideButtons.UnSubscribe(HideButtons);
			addAbility.Unsubscribe(AddAbility);
		}
	}
}