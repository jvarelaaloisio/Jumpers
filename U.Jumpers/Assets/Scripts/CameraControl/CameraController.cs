using System;
using Helpers;
using Packages.UpdateManagement;
using UnityEngine;

namespace CameraControl
{
	public class CameraController : ILateUpdateable
	{
		private readonly Camera camera;
		private readonly CameraModel cameraModel;
		private readonly Func<Transform> form;
		private readonly Transform target;
		private readonly Transform cameraPivot;
		private readonly Quaternion leftRotationBound;
		private readonly Quaternion rightRotationBound;
		private float panState;
		private float zoomState;
		public Action onPanBegins;
		public Action onPanEnds;

		public CameraController(
			Camera camera,
			CameraModel cameraModel,
			Transform transform,
			Transform target,
			Transform cameraPivot)
		{
			this.camera = camera;
			this.cameraModel = cameraModel;
			this.target = target;
			this.cameraPivot = cameraPivot;
			UpdateManager.Subscribe(this);
			cameraPivot.position = target.position;
			transform.SetParent(cameraPivot);
			Quaternion startRotation = cameraPivot.rotation;
			leftRotationBound = startRotation * Quaternion.Euler(Vector3.up * -cameraModel.MaxPanAngle);
			rightRotationBound = startRotation * Quaternion.Euler(Vector3.up * cameraModel.MaxPanAngle);
			panState = 0.5f;
			zoomState = 0;
		}

		public void OnLateUpdate()
		{
			cameraPivot.position = target.position;
		}

		public void BeginPan()
		{
			onPanBegins?.Invoke();
		}

		public void ControlCamera(Vector2 input)
		{
			panState = LerpHelper.GetArcSinLerp((input.x * (cameraModel.IsInvertedX ? -1 : 1) + 1) / 2);
			zoomState = LerpHelper.GetArcSinLerp((input.y * (cameraModel.IsInvertedY ? -1 : 1) + 1) / 2);
			cameraPivot.rotation = Quaternion.Slerp(
				leftRotationBound,
				rightRotationBound,
				LerpHelper.GetArcSinLerp(panState));

			camera.fieldOfView = Mathf.Lerp(cameraModel.MinFOV, cameraModel.MaxFOV, zoomState);
		}

		public void EndPan()
		{
			onPanEnds?.Invoke();
		}
	}
}