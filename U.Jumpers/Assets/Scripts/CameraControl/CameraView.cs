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
		private Transform target;
		[Header("Channels Listened")]
		[SerializeField, Tooltip("Can be Null")] private Vector2ChannelSo panCamera;
		[SerializeField, Tooltip("Can be Null")] private VoidChannelSo beginPan;
		[SerializeField, Tooltip("Can be Null")] private VoidChannelSo endPan;
		[Header("Events Raised")]
		[SerializeField] private UnityEvent onPanBegins;
		[SerializeField] private UnityEvent onPanEnds;
		private Transform cameraPivot;
		private CameraController cameraController;
		public int myScene;
		private void Start()
		{
			target = GameObject.FindWithTag(targetTag).transform;
			cameraPivot = new GameObject("CameraPivot").transform;
			SceneManager.MoveGameObjectToScene(cameraPivot.gameObject, SceneManager.GetSceneByBuildIndex(myScene));
			cameraController = new CameraController(camera, model, transform, target, cameraPivot);
			cameraController.onPanBegins += onPanBegins.Invoke;
			cameraController.onPanEnds += onPanEnds.Invoke;
			beginPan.SubscribeSafely(cameraController.BeginPan);
			endPan.SubscribeSafely(cameraController.EndPan);
			panCamera.SubscribeSafely(cameraController.ControlCamera);
		}

		private void OnDestroy()
		{
			panCamera.Unsubscribe(cameraController.ControlCamera);
			beginPan.UnSubscribe(cameraController.BeginPan);
			endPan.UnSubscribe(cameraController.EndPan);
		}
	}
}