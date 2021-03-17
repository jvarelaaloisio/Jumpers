using Helpers;
using UnityEngine;
using UnityEngine.UI;
using VarelaAloisio.UpdateManagement.Runtime;

namespace UI
{
	public class Bar : MonoBehaviour
	{
		[SerializeField] private Image bar;
		[SerializeField] private float fillTime;
		[SerializeField] private int sceneIndex;
		private ActionOverTime _fillBar;
		private float _lastFill;
		private float _objFill;

		private void Awake()
		{
			_lastFill = bar.fillAmount;
			_fillBar = new ActionOverTime(
				fillTime,
				lerp => bar.fillAmount = Mathf.Lerp(_lastFill, _objFill, LerpHelper.GetSinLerp(lerp)),
				() => _lastFill = bar.fillAmount,
				sceneIndex,
				true);
		}

		public void SetFillAmount(float fillAmount)
		{
			_objFill = fillAmount;
			_fillBar.StartAction();
		}
	}
}