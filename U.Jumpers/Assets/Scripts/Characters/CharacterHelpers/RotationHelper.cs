using UnityEngine;
using VarelaAloisio.UpdateManagement.Runtime;

namespace Characters.CharacterHelpers
{
	public static class RotationHelper
	{
		public static void RotateTowards(Transform transform, Quaternion desiredDirection, float duration, int sceneIndex)
		{
			if(transform.rotation.Equals(desiredDirection))
				return;
			Quaternion origin = transform.rotation;
			new ActionOverTime(duration,
				lerp => transform.rotation = Quaternion.Lerp(origin, desiredDirection, lerp),
				sceneIndex,
				true)
				.StartAction();
		}
	}
}