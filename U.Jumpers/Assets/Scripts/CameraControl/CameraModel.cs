using UnityEngine;

namespace CameraControl
{
	[CreateAssetMenu(menuName = "Models/Camera Model", fileName = "CameraModel")]
	public class CameraModel : ScriptableObject
	{
		[SerializeField] private bool isInvertedX;
		[SerializeField] private bool isInvertedY;
		[SerializeField] private int maxPanAngle;
		[SerializeField] private int minFOV;
		[SerializeField] private int maxFOV;

		public bool IsInvertedX => isInvertedX;
		
		public bool IsInvertedY => isInvertedY;

		public int MaxPanAngle => maxPanAngle;

		public int MinFOV => minFOV;

		public int MaxFOV => maxFOV;
	}
}
