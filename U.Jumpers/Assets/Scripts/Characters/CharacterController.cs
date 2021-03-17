using System;
using Characters.CharacterHelpers;
using Helpers.CharacterHelpers;
using LS;
using UnityEngine;

namespace Characters
{
	public class CharacterController
	{
		private readonly ICharacterModel _characterModel;
		private readonly Func<Vector3> _getMyPosition;
		private readonly float _rotationDuration;
		private readonly int _sceneIndex;
		public Action<Vector3> OnCharacterMoves;
		public Action OnFinishedMoving;

		public Damageable Damageable { get; }
		public Transform GetTransform { get; }

		public CharacterController(
			ICharacterModel characterModel,
			Transform getTransform,
			Func<Vector3> getMyPosition,
			float rotationDuration,
			int sceneIndex)
		{
			_characterModel = characterModel;
			GetTransform = getTransform;
			_getMyPosition = getMyPosition;
			_rotationDuration = rotationDuration;
			_sceneIndex = sceneIndex;
			Damageable = new Damageable(characterModel.LifePoints);
		}

		public void MoveCharacter(Vector3 desiredPosition)
		{
			desiredPosition.y = _getMyPosition().y;
			if (desiredPosition.Equals(_getMyPosition()))
				return;
			RotationHelper.RotateTowards(GetTransform,
				Quaternion.LookRotation(desiredPosition - GetTransform.position, Vector3.up),
				_rotationDuration, _sceneIndex);
			JumpHelper.Jump(GetTransform, desiredPosition, _characterModel.JumpDuration, _sceneIndex, OnFinishedMoving);
			OnCharacterMoves?.Invoke(desiredPosition);
		}

		public Transform[] GetAvailablePillars()
		{
			Transform[] pillars = JumpHelper.GetClosePillars(GetTransform, _characterModel.MoveDistance);
			Vector3 myPosition = _getMyPosition();
			var pillarsFiltered = JumpHelper.FilterOwnPillar(in pillars, in myPosition);
			return pillarsFiltered;
		}
	}
}