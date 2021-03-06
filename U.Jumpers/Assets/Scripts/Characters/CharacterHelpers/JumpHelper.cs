using System;
using System.Linq;
using Characters;
using Packages.UpdateManagement;
using UnityEngine;

namespace Helpers.CharacterHelpers
{
	public static class JumpHelper
	{
		private const float MINIMUM_TRAVEL_DISTANCE = 1;
		private const string PILLARS_LAYER = "Pillars";

		public static void Jump(Transform transform, Vector3 destination, float jumpDuration)
		{
			Vector3 origin = transform.position;
			new ActionOverTime(jumpDuration,
					(lerp) => transform.position = Vector3.Lerp(origin, destination, LerpHelper.GetSinLerp(lerp)),
					true)
				.StartAction();
		}

		public static void Jump(Transform transform, Vector3 destination, float jumpDuration, Action onFinish)
		{
			Jump(transform, destination, jumpDuration);
			new CountDownTimer(jumpDuration, onFinish).StartTimer();
		}

		public static Transform[] FilterOwnPillar(in Transform[] pillars, in Vector3 ownPosition)
		{
			Vector2 ownHorPosition = new Vector2(ownPosition.x, ownPosition.z);

			Transform[] pillarsFiltered = pillars.Where(t =>
			{
				Vector3 pillarPosition = t.position;
				Vector2 pillarHorPosition = new Vector2(pillarPosition.x, pillarPosition.z);
				return Vector2.Distance(ownHorPosition, pillarHorPosition) >= MINIMUM_TRAVEL_DISTANCE;
			}).ToArray();
			return pillarsFiltered;
		}

		public static Transform[] GetClosePillars(Transform transform, float radius)
		{
			Collider[] overlaps = Physics.OverlapSphere(transform.position, radius, LayerMask.GetMask(PILLARS_LAYER));
			Transform[] result = overlaps.Select(c => c.transform).ToArray();
			return result;
		}
	}
}