using Packages.UpdateManagement;
using UnityEngine;

namespace Characters
{
	public static class RotationHelper
	{
		public static void RotateTowards(Transform transform, Quaternion desiredDirection, float duration)
		{
			if(transform.rotation.Equals(desiredDirection))
				return;
			Quaternion origin = transform.rotation;
			new ActionOverTime(duration,
				lerp => transform.rotation = Quaternion.Lerp(origin, desiredDirection, lerp),
				true)
				.StartAction();
		}
	}
}