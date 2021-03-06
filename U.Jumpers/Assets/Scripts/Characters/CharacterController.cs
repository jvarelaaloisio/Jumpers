using System;
using Helpers.CharacterHelpers;
using LS;
using UnityEngine;

namespace Characters
{
	public class CharacterController
	{
		private readonly ICharacterModel characterModel;
		private readonly Func<Vector3> getMyPosition;
		private readonly float rotationDuration;
		public Action<Vector3> OnCharacterMoves;
		public Action OnFinishedMoving;

		public Damageable Damageable { get; }
		public Transform GetTransform { get; }

		public CharacterController(ICharacterModel characterModel, Transform getTransform,
			Func<Vector3> getMyPosition, float rotationDuration)
		{
			this.characterModel = characterModel;
			this.GetTransform = getTransform;
			this.getMyPosition = getMyPosition;
			this.rotationDuration = rotationDuration;
			Damageable = new Damageable(characterModel.LifePoints);
		}

		public void MoveCharacter(Vector3 desiredPosition)
		{
			desiredPosition.y = getMyPosition().y;
			if (desiredPosition.Equals(getMyPosition()))
				return;
			RotationHelper.RotateTowards(GetTransform,
				Quaternion.LookRotation(desiredPosition - GetTransform.position, Vector3.up),
				rotationDuration);
			JumpHelper.Jump(GetTransform, desiredPosition, characterModel.JumpDuration, OnFinishedMoving);
			OnCharacterMoves?.Invoke(desiredPosition);
		}

		public Transform[] GetAvailablePillars()
		{
			Transform[] pillars = JumpHelper.GetClosePillars(GetTransform, characterModel.MoveDistance);
			Vector3 myPosition = getMyPosition();
			var pillarsFiltered = JumpHelper.FilterOwnPillar(in pillars, in myPosition);
			return pillarsFiltered;
		}
	}
}