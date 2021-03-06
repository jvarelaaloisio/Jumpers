using System;
using Events.Channels;
using Events.UnityEvents;
using UI;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Managers
{
	public class PillarButtonManager : MonoBehaviour
	{
		[SerializeField] private Vector3 pillarButtonsOffset;
		[SerializeField] private int pillarButtonQty;
		[SerializeField] private GameObject pillarButtonPrefab;
		[SerializeField] private GameObject canvas;

		[Header("Events Raised")] [SerializeField]
		private Vector3UnityEvent onPillarSelectedEvent;

		[Header("Channels Listened")]
		[SerializeField] private TransformArrayChannelSo setUpPillars;

		[SerializeField] private VoidChannelSo showPillarButtons;
		[SerializeField] private VoidChannelSo hidePillarButtons;
		[SerializeField] private GameObjectChannelSo selectPillarButton;

		private GameObject pillarButtonsParent;
		private GameObject[] pillarButtonGos;
		private PillarButton[] pillarButtons;
		private int activePillars;

		public int myScene;

		private void Awake()
		{
			pillarButtonsParent = new GameObject("Pillar Buttons");
			SceneManager.MoveGameObjectToScene(pillarButtonsParent, SceneManager.GetSceneByBuildIndex(myScene));
			pillarButtonsParent.transform.SetParent(canvas.transform);
			pillarButtons = new PillarButton[pillarButtonQty];
			pillarButtonGos = new GameObject[pillarButtonQty];
			for (int i = 0; i < pillarButtonQty; i++)
			{
				pillarButtonGos[i] = Instantiate(pillarButtonPrefab, pillarButtonsParent.transform);
				pillarButtons[i] = pillarButtonGos[i].GetComponent<PillarButton>();
			}

			selectPillarButton.Subscribe(go => PillarButtonPress(go.GetComponent<PillarButton>()));
			setUpPillars.Subscribe(SetUpPillarButtons);
			showPillarButtons.Subscribe(ShowPillarButtons);
			hidePillarButtons.Subscribe(HidePillarButtons);
			HidePillarButtons();
		}

		private void SetUpPillarButtons(Transform[] pillars)
		{
			Camera mainCamera = Camera.main;
			activePillars = Mathf.Clamp(pillars.Length, 0, pillarButtonQty);
			for (var i = 0; i < activePillars; i++)
			{
				pillarButtons[i].PillarTransform = pillars[i];
				pillarButtonGos[i].GetComponent<RectTransform>().position =
					mainCamera.WorldToScreenPoint(pillars[i].position) + pillarButtonsOffset;
			}
		}

		private void HidePillarButtons()
		{
			foreach (var go in pillarButtonGos) go.SetActive(false);
		}

		private void ShowPillarButtons()
		{
			for (var i = 0; i < pillarButtonQty; i++)
			{
				bool setActive = i < activePillars;
				pillarButtonGos[i].SetActive(setActive);
			}
		}

		private void PillarButtonPress(PillarButton button)
		{
			onPillarSelectedEvent.Invoke(button.PillarTransform.position);
		}

		private void OnDestroy()
		{
			setUpPillars.Unsubscribe(SetUpPillarButtons);
			showPillarButtons.UnSubscribe(ShowPillarButtons);
			hidePillarButtons.UnSubscribe(HidePillarButtons);
			selectPillarButton.Unsubscribe(go => PillarButtonPress(go.GetComponent<PillarButton>()));
		}
	}
}