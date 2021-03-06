using System;
using Characters.Abilities.Events;
using Events.Channels;
using Packages.UpdateManagement;
using UnityEngine;

namespace Abilities
{
	public class AbilityButtonManager : MonoBehaviour
	{
		[Header("Setup")] [SerializeField] private AbilityButton[] buttons;

		[SerializeField] private float buttonsTimeOff;

		[Space, Header("Channels Listened")]
		[SerializeField, Tooltip("Can Be Null")]
		private VoidChannelSo showButtons;

		[SerializeField, Tooltip("Can Be Null")]
		private VoidChannelSo hideButtons;

		[SerializeField, Tooltip("Not Null")] private AbilityChannelSo addAbility;

		private int activeButtons = 0;
		private CountDownTimer resumeButtons;

		private void Awake()
		{
			hideButtons.SubscribeSafely(HideButtons);
			showButtons.SubscribeSafely(ShowButtons);
			addAbility.Subscribe(AddAbility);
			resumeButtons = new CountDownTimer(buttonsTimeOff, ShowButtons);
		}

		private void AddAbility(AbilitySo ability)
		{
			if (activeButtons >= buttons.Length)
				return;
			activeButtons++;
			buttons[activeButtons - 1].SetAbility(ability);
			ability.onUse.AddListener(_ =>
			{
				HideButtons();
				resumeButtons.StartTimer();
			});
		}

		private void ShowButtons()
		{
			for (int i = 0; i < activeButtons; i++)
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