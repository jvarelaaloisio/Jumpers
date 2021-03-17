using System.Collections.Generic;
using System.Linq;
using Debugging;
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

		private Dictionary<Transform, int> _transformIndex;

		[SerializeField] private Direction[] waypoints;

		private void OnEnable()
		{
			_transformIndex = new Dictionary<Transform, int>();
		}

		public override Vector3 GetNextPosition(Transform transform, Vector3[] possibleDestinations)
		{
			if (!_transformIndex.ContainsKey(transform))
				_transformIndex.Add(transform, 0);
			var destinationDirection = new (Vector3, Direction)[possibleDestinations.Length];
			var myPosition = transform.position;
			for (var i = 0; i < possibleDestinations.Length; i++)
			{
				var directionRaw = possibleDestinations[i] - myPosition;
				var x = directionRaw.x;
				var z = directionRaw.z;
				Direction direction;
				if (Mathf.Abs(z) > Mathf.Abs(x))
					direction = (z > 0) ? Direction.Up : Direction.Down;
				else
					direction = (x > 0) ? Direction.Right : Direction.Left;
				destinationDirection[i] = (possibleDestinations[i], direction);
				Printer.Log(LogLevel.Info, $"{name} => {transform.name}: possible dir added: {direction}");
			}

			for (int i = 0; i < waypoints.Length; i++)
			{
				int lastIndex = _transformIndex[transform];
				_transformIndex[transform]++;
				if (_transformIndex[transform] >= waypoints.Length)
					_transformIndex[transform] = 0;
				if (destinationDirection.All(tuple => tuple.Item2 != waypoints[lastIndex]))
					continue;
				Printer.Log(LogLevel.Info, $"{name} => {transform.name}: next dir: {waypoints[lastIndex]}");
				return destinationDirection.First(tuple => tuple.Item2 == waypoints[lastIndex]).Item1;
			}

			Printer.Log(LogLevel.Warning, $"{name} => {transform.name}: no dir satisfies the waypoints");
			return transform.position;
		}
	}
}