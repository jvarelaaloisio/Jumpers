using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace UI
{
	public class SwipeReader : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
	{
		[SerializeField] private UnityEvent onSwipeUp;
		[SerializeField] private UnityEvent onSwipeDown;
		[SerializeField] private UnityEvent onSwipeLeft;
		[SerializeField] private UnityEvent onSwipeRight;
		private Vector2 _dragOrigin;

		public void OnBeginDrag(PointerEventData eventData)
		{
			_dragOrigin = eventData.position;
		}

		public void OnEndDrag(PointerEventData eventData)
		{
			Vector2 direction = eventData.position - _dragOrigin;
			float absX = Mathf.Abs(direction.x);
			float absY = Mathf.Abs(direction.y);
			if (absX > absY)
			{
				if (direction.x > 0)
					onSwipeRight.Invoke();
				else
					onSwipeLeft.Invoke();
			}
			else
			{
				if (direction.y > 0)
					onSwipeUp.Invoke();
				else
					onSwipeDown.Invoke();
			}
		}

		public void OnDrag(PointerEventData eventData)
		{}
	}
}