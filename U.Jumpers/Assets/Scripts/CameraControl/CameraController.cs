using System;
using Helpers;
using UnityEngine;
using VarelaAloisio.UpdateManagement.Runtime;

namespace CameraControl
{
	public class CameraController : ILateUpdateable
	{
		private readonly Camera _camera;
		private readonly CameraModel _cameraModel;
		private readonly Transform _target;
		private readonly Transform _cameraPivot;
		private readonly int _sceneIndex;
		private readonly Quaternion _leftRotationBound;
		private readonly Quaternion _rightRotationBound;
		private float _panState;
		private float _zoomState;
		public Action onPanBegins;
		public Action onPanEnds;

		public CameraController(
			Camera camera,
			CameraModel cameraModel,
			Transform transform,
			Transform target,
			Transform cameraPivot,
			int sceneIndex)
		{
			_camera = camera;
			_cameraModel = cameraModel;
			_target = target;
			_cameraPivot = cameraPivot;
			_sceneIndex = sceneIndex;
			UpdateManager.Subscribe(this, sceneIndex);
			cameraPivot.position = target.position;
			transform.SetParent(cameraPivot);
			Quaternion startRotation = cameraPivot.rotation;
			_leftRotationBound = startRotation * Quaternion.Euler(Vector3.up * -cameraModel.MaxPanAngle);
			_rightRotationBound = startRotation * Quaternion.Euler(Vector3.up * cameraModel.MaxPanAngle);
			_panState = 0.5f;
			_zoomState = 0;
		}

		public void OnLateUpdate()
		{
			_cameraPivot.position = _target.position;
		}

		public void BeginPan()
		{
			onPanBegins?.Invoke();
		}

		public void ControlCamera(Vector2 input)
		{
			_panState = LerpHelper.GetArcSinLerp((input.x * (_cameraModel.IsInvertedX ? -1 : 1) + 1) / 2);
			_zoomState = LerpHelper.GetArcSinLerp((input.y * (_cameraModel.IsInvertedY ? -1 : 1) + 1) / 2);
			_cameraPivot.rotation = Quaternion.Slerp(
				_leftRotationBound,
				_rightRotationBound,
				LerpHelper.GetArcSinLerp(_panState));

			_camera.fieldOfView = Mathf.Lerp(_cameraModel.MinFOV, _cameraModel.MaxFOV, _zoomState);
		}

		public void EndPan()
		{
			onPanEnds?.Invoke();
		}
	}
}