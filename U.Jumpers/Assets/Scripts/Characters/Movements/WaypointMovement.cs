using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Characters.Movements
{
	[CreateAssetMenu(menuName = "Models/Movements/Waypoints", fileName = "WaypointMovement")]
	public class WaypointMovement : Movement
	{
		private enum Direction
		{
			Up,
			Down,
			Right,
			Left
		}

		private int index;

		[SerializeField] private Direction[] waypoints;

		private void OnEnable()
		{
			index = 0;
		}

		public override Vector3 GetNextPosition(Transform myTransform, Vector3[] possibleDestinations)
		{
			(Vector3, Direction)[] destinationDirection = new (Vector3, Direction)[possibleDestinations.Length];
			Vector3 myPosition = myTransform.position;
			for (var i = 0; i < possibleDestinations.Length; i++)
			{
				Vector3 directionRaw = possibleDestinations[i] - myPosition;
				float x = directionRaw.x;
				float z = directionRaw.z;
				Direction direction;
				if (Mathf.Abs(z) > Mathf.Abs(x))
					direction = (z > 0) ? Direction.Up : Direction.Down;
				else
					direction = (x > 0) ? Direction.Right : Direction.Left;
				destinationDirection[i] = (possibleDestinations[i], direction);
			}

			for (int i = 0; i < waypoints.Length; i++)
			{
				int lastIndex = index;
				index++;
				if (index >= waypoints.Length)
					index = 0;
				if (destinationDirection.Any(tuple => tuple.Item2 == waypoints[lastIndex]))
					return destinationDirection.First(tuple => tuple.Item2 == waypoints[lastIndex]).Item1;
			}

			return myTransform.position;
		}
	}
}