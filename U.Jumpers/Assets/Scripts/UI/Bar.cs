using System;
using Helpers;
using Packages.UpdateManagement;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
	public class Bar : MonoBehaviour
	{
		[SerializeField] private Image bar;
		[SerializeField] private float fillTime;
		private ActionOverTime fillBar;
		private float lastFill;
		private float objFill;

		private void Awake()
		{
			lastFill = bar.fillAmount;
			fillBar = new ActionOverTime(
				fillTime,
				lerp => bar.fillAmount = Mathf.Lerp(lastFill, objFill, LerpHelper.GetSinLerp(lerp)),
				() => lastFill = bar.fillAmount,
				true);
		}

		public void SetFillAmount(float fillAmount)
		{
			objFill = fillAmount;
			fillBar.StartAction();
		}
	}
}