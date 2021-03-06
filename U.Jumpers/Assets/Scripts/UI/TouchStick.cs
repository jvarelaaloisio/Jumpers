using Events.UnityEvents;
using Packages.UpdateManagement;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace UI
{
	[RequireComponent(typeof(RectTransform))]
	public class TouchStick : MonoBehaviour, IDragHandler, IEndDragHandler, IBeginDragHandler
	{
		[Header("Setup")]
		// [SerializeField] private bool ignoreX;
		// [SerializeField] private bool ignoreY;
		[SerializeField] private bool ignoreXPositive;
		[SerializeField] private bool ignoreYPositive;
		[SerializeField] private bool ignoreXNegative;
		[SerializeField] private bool ignoreYNegative;
		[SerializeField] private float resetDuration;
		[SerializeField] private float maxDistanceFromCenter;
		[SerializeField] private RectTransform stickHead;
		[Header("Events Raised")]
		[SerializeField] private UnityEvent onStickHold;
		[SerializeField] private UnityEvent onStickRelease;
		[SerializeField] private Vector2UnityEvent onStickInput;

		private Vector2 lastStickLocalPosition;
		private ActionOverTime resetPosition;

		private void Start()
		{
			resetPosition = new ActionOverTime(resetDuration, ResetPosition,onStickRelease.Invoke, true);
		}

		public void OnBeginDrag(PointerEventData eventData)
		{
			resetPosition.StopAction();
			onStickHold.Invoke();
		}

		public void OnDrag(PointerEventData eventData)
		{
			stickHead.position = eventData.position;
			Vector2 localPosition = stickHead.localPosition;
			// localPosition.x *= ignoreX ? 0 : 1;
			// localPosition.y *= ignoreY ? 0 : 1;
			if (ignoreXNegative)
				localPosition.x = Mathf.Clamp(localPosition.x, 0, -maxDistanceFromCenter);
			if (ignoreYNegative)
				localPosition.y = Mathf.Clamp(localPosition.y, 0, -maxDistanceFromCenter);
			if (ignoreXPositive)
				localPosition.x = Mathf.Clamp(localPosition.x, -maxDistanceFromCenter, 0);
			if (ignoreYPositive)
				localPosition.y = Mathf.Clamp(localPosition.y, -maxDistanceFromCenter, 0);
			localPosition = Vector2.ClampMagnitude(localPosition, maxDistanceFromCenter);
			stickHead.localPosition = localPosition;
			Vector2 normalizedInput = localPosition / maxDistanceFromCenter;
			onStickInput.Invoke(normalizedInput);
		}

		public void OnEndDrag(PointerEventData eventData)
		{
			lastStickLocalPosition = stickHead.localPosition;
			resetPosition.StartAction();
		}

		private void ResetPosition(float lerp)
		{
			Vector2 localPosition = Vector2.Lerp(lastStickLocalPosition, Vector2.zero, lerp);
			stickHead.localPosition = localPosition;
			onStickInput.Invoke(localPosition / maxDistanceFromCenter);
		}
	}
}