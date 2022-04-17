using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Core;
using Events.UnityEvents;
using UnityEngine;
using UnityEngine.Events;

namespace Managers
{
	public class SceneBatchLoader : MonoBehaviour
	{
		[SerializeField]
		private FloatUnityEvent onStateUpdate;

		[SerializeField]
		private UnityEvent onFinish;

		[SerializeField]
		private float stateUpdatePeriod;


		private IEnumerable<IInitializer> _initializers;
		private IInitializer _currentInitializer;
		private bool _initializing;

		private IEnumerator Initialize()
		{
			_initializing = true;
			StartCoroutine(FollowInitializationProcess());
			_initializers = gameObject
							.scene
							.GetRootGameObjects()
							.SelectMany(go => go.GetComponentsInChildren<IInitializer>());
			foreach (var initializer in _initializers)
			{
				_currentInitializer = initializer;
				yield return initializer.Initialize();
			}

			_initializing = false;
			onFinish.Invoke();
		}

		private IEnumerator FollowInitializationProcess()
		{
			var wait = new WaitForSeconds(stateUpdatePeriod);
			float totalPercentage = _initializers.Count();
			float currentPercentage = 0;
			float currentDecimal = 0;
			while (_initializing)
			{
				float currentState = _currentInitializer.InitializationState;
				if (currentDecimal.DifferenceWith(currentState) > 0.001f)
				{
					currentDecimal = currentState;
					currentPercentage = Mathf.Floor(currentPercentage) + currentDecimal;
					onStateUpdate.Invoke(currentPercentage / totalPercentage);
				}

				yield return wait;
			}
		}
	}
}