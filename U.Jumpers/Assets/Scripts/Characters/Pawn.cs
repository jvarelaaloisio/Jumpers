using System;
using Characters.CharacterHelpers;
using Helpers.CharacterHelpers;
using LS;
using UnityEngine;

namespace Characters
{
	public class Pawn
	{
		private readonly IPawnModel _pawnModel;
		private readonly Func<Vector3> _getMyPosition;
		private readonly float _rotationDuration;
		private readonly int _sceneIndex;
		public Action<Vector3> OnCharacterMoves;
		public Action OnFinishedMoving;

		public Damageable Damageable { get; }
		public Transform GetTransform { get; }

		public Pawn(
			IPawnModel pawnModel,
			Transform getTransform,
			Func<Vector3> getMyPosition,
			float rotationDuration,
			int sceneIndex)
		{
			_pawnModel = pawnModel;
			GetTransform = getTransform;
			_getMyPosition = getMyPosition;
			_rotationDuration = rotationDuration;
			_sceneIndex = sceneIndex;
			Damageable = new Damageable(pawnModel.LifePoints);
		}

		public void MoveCharacter(Vector3 desiredPosition)
		{
			desiredPosition.y = _getMyPosition().y;
			if (desiredPosition.Equals(_getMyPosition()))
				return;
			RotationHelper.RotateTowards(GetTransform,
				Quaternion.LookRotation(desiredPosition - GetTransform.position, Vector3.up),
				_rotationDuration, _sceneIndex);
			JumpHelper.Jump(GetTransform, desiredPosition, _pawnModel.JumpDuration, _sceneIndex, OnFinishedMoving);
			OnCharacterMoves?.Invoke(desiredPosition);
		}

		public Transform[] GetAvailablePillars()
		{
			Transform[] pillars = JumpHelper.GetClosePillars(GetTransform, _pawnModel.MoveDistance);
			Vector3 myPosition = _getMyPosition();
			var pillarsFiltered = JumpHelper.FilterOwnPillar(in pillars, in myPosition);
			return pillarsFiltered;
		}
	}
}