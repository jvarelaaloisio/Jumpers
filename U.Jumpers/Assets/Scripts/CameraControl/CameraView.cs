using System;
using Events.Channels;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace CameraControl
{
	public class CameraView : MonoBehaviour
	{
		[Header("Setup")]
		[SerializeField] private new Camera camera;
		[SerializeField] private CameraModel model;
		[SerializeField] private string targetTag;
		private Transform _target;
		[Header("Channels Listened")]
		[SerializeField, Tooltip("Can be Null")] private Vector2ChannelSo panCamera;
		[SerializeField, Tooltip("Can be Null")] private VoidChannelSo beginPan;
		[SerializeField, Tooltip("Can be Null")] private VoidChannelSo endPan;
		[Header("Events Raised")]
		[SerializeField] private UnityEvent onPanBegins;
		[SerializeField] private UnityEvent onPanEnds;
		private Transform _cameraPivot;
		private CameraController _cameraController;
		public int myScene;
		private void Start()
		{
			_target = GameObject.FindWithTag(targetTag).transform;
			_cameraPivot = new GameObject("CameraPivot").transform;
			SceneManager.MoveGameObjectToScene(_cameraPivot.gameObject, SceneManager.GetSceneByBuildIndex(myScene));
			_cameraController = new CameraController(camera, model, transform, _target, _cameraPivot);
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
	}
}