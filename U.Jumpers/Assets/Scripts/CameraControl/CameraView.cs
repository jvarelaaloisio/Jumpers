using System;
using Events.Channels;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace CameraControl
{
	public class CameraView : MonoBehaviour
	{
		[Header("Setup")] [SerializeField] private new Camera camera;

		[SerializeField] private CameraModel model;

		[SerializeField, Tooltip("The distance from the target where the audio listener is placed")]
		private float audioListenerDistanceFromTarget;

		[SerializeField] private string targetTag;
		private Transform _target;

		[Header("Channels Listened")] [SerializeField, Tooltip("Can be Null")]
		private Vector2ChannelSo panCamera;

		[SerializeField, Tooltip("Can be Null")]
		private VoidChannelSo beginPan;

		[SerializeField, Tooltip("Can be Null")]
		private VoidChannelSo endPan;

		[Header("Events Raised")] [SerializeField]
		private UnityEvent onPanBegins;

		[SerializeField] private UnityEvent onPanEnds;

		private Transform _cameraPivot;
		private Transform _audioListener;
		private CameraController _cameraController;

		private void Start()
		{
			_target = GameObject.FindWithTag(targetTag).transform;

			_cameraPivot = new GameObject("CameraPivot").transform;
			SceneManager.MoveGameObjectToScene(_cameraPivot.gameObject, gameObject.scene);

			Vector3 fromPivotToCamera = (transform.position - _cameraPivot.position).normalized;
			_audioListener = new GameObject("AudioListener")
			                 .AddComponent<AudioListener>()
			                 .transform;
			_audioListener.SetParent(_cameraPivot);
			_audioListener.position = _cameraPivot.position + fromPivotToCamera * audioListenerDistanceFromTarget;
			_cameraController = new CameraController(camera, model, transform, _target, _cameraPivot,
				gameObject.scene.buildIndex);
			_cameraController.onPanBegins += onPanBegins.Invoke;
			_cameraController.onPanEnds += onPanEnds.Invoke;

			beginPan.SubscribeSafely(_cameraController.BeginPan);
			endPan.SubscribeSafely(_cameraController.EndPan);
			panCamera.SubscribeSafely(_cameraController.ControlCamera);
		}

		private void OnDestroy()
		{
			panCamera.Unsubscribe(_cameraController.ControlCamera);
			beginPan.UnSubscribe(_cameraController.BeginPan);
			endPan.UnSubscribe(_cameraController.EndPan);
		}

		private void OnDrawGizmos()
		{
			if (!_audioListener)
				return;
			Gizmos.DrawIcon(_audioListener.position, "d_SceneViewAudio@2x", false);
		}
	}
}