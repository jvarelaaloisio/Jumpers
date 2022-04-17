using System;
using System.Collections;
using System.Collections.Generic;
using Core;
using Debugging;
using UnityEngine;
using VarelaAloisio.UpdateManagement.Runtime;

namespace Characters.Enemies
{
	public class EnemyManager : MonoBehaviour, IInitializer, IUpdateable
	{
		[Header("Setup")]
		[SerializeField]
		[Tooltip("Quantity of enemies that may be shown at the same time in the screen")]
		private int poolSize;

		[SerializeField]
		[Tooltip("Should the pool size be augmented when the quantity of active enemies is surpassed")]
		private bool isPoolDynamic;

		[SerializeField]
		[Tooltip("Size of the batches that will be spawned asynchronously")]
		private int spawnBatchSize;

		[SerializeField]
		[Tooltip("Period to wait between every batch")]
		private float spawnBatchPeriod;

		private Enemy.Factory _factory;
		private List<Enemy> _enemies;

		public float InitializationState { get; private set; }
		public event Action OnInitialized;

		public void SubscribeToOnInitialized(Action listener) => OnInitialized += listener;

		public void UnSubscribeToOnInitialized(Action listener) => OnInitialized -= listener;

		public IEnumerator Initialize()
		{
			var parent = new GameObject("Enemies").transform;
			_factory = new Enemy.Factory(string.Empty, parent);
			yield return _factory.InitializePoolInBatches(poolSize,
														isPoolDynamic,
														spawnBatchSize,
														spawnBatchPeriod,
														UpdateState,
														OnInitialized);
			UpdateManager.Subscribe(this);
		}

		public void OnUpdate()
		{
			
		}

		private void UpdateState()
			=> InitializationState += 1.0f * poolSize / spawnBatchSize;
	}
}