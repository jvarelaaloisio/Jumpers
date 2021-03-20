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

		private GameObject _pillarButtonsParent;
		private GameObject[] _pillarButtonGos;
		private PillarButton[] _pillarButtons;
		private int _activePillars;

		private void Awake()
		{
			_pillarButtonsParent = new GameObject("Pillar Buttons");
			SceneManager.MoveGameObjectToScene(_pillarButtonsParent, gameObject.scene);
			_pillarButtonsParent.transform.SetParent(canvas.transform);
			_pillarButtons = new PillarButton[pillarButtonQty];
			_pillarButtonGos = new GameObject[pillarButtonQty];
			for (int i = 0; i < pillarButtonQty; i++)
			{
				_pillarButtonGos[i] = Instantiate(pillarButtonPrefab, _pillarButtonsParent.transform);
				_pillarButtons[i] = _pillarButtonGos[i].GetComponent<PillarButton>();
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
			_activePillars = Mathf.Clamp(pillars.Length, 0, pillarButtonQty);
			for (var i = 0; i < _activePillars; i++)
			{
				_pillarButtons[i].PillarTransform = pillars[i];
				_pillarButtonGos[i].GetComponent<RectTransform>().position =
					mainCamera.WorldToScreenPoint(pillars[i].position) + pillarButtonsOffset;
			}
		}

		private void HidePillarButtons()
		{
			foreach (var go in _pillarButtonGos) go.SetActive(false);
		}

		private void ShowPillarButtons()
		{
			for (var i = 0; i < pillarButtonQty; i++)
			{
				bool setActive = i < _activePillars;
				_pillarButtonGos[i].SetActive(setActive);
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