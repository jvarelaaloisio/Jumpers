using System;
using System.Collections;
using System.Data;
using Characters.CharacterHelpers;
using Helpers.CharacterHelpers;
using LS;
using UnityEngine;

namespace Characters
{
	public class Pawn
	{
		private readonly Func<Vector3> _getMyPosition;
		private readonly float _rotationDuration;
		private readonly int _sceneIndex;
		public Action<Vector3> OnCharacterMoves = delegate { };
		public Action OnFinishedMoving = delegate { };

		public Hashtable Data = new PropertyCollection();
		public IPawnModel Model { get; }
		public Damageable Damageable { get; }
		public Transform GetTransform { get; }
		public MonoBehaviour GetMonoBehaviour { get; }

		public Pawn(
			IPawnModel pawnModel,
			Transform getTransform,
			Func<Vector3> getMyPosition,
			float rotationDuration,
			int sceneIndex,
			MonoBehaviour getMonoBehaviour)
		{
			Model = pawnModel;
			GetTransform = getTransform;
			_getMyPosition = getMyPosition;
			_rotationDuration = rotationDuration;
			_sceneIndex = sceneIndex;
			GetMonoBehaviour = getMonoBehaviour;
			Damageable = new Damageable(pawnModel.LifePoints, pawnModel.StartingLifePoints);
		}

		public void MoveCharacter(Vector3 desiredPosition)
		{
			desiredPosition.y = _getMyPosition().y;
			if (desiredPosition.Equals(_getMyPosition()))
				return;
			RotationHelper.RotateTowards(GetTransform,
				Quaternion.LookRotation(desiredPosition - GetTransform.position, Vector3.up),
				_rotationDuration, _sceneIndex);
			JumpHelper.Jump(GetTransform, desiredPosition, Model.JumpDuration, _sceneIndex, OnFinishedMoving);
			OnCharacterMoves?.Invoke(desiredPosition);
		}

		public Transform[] GetAvailablePillars(Vector3 myPosition)
		{
			Transform[] pillars = JumpHelper.GetClosePillars(GetTransform, Model.MoveDistance);
			var pillarsFiltered = JumpHelper.FilterOwnPillar(in pillars, in myPosition);
			return pillarsFiltered;
		}
	}
}