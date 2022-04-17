using System;
using System.Collections;
using Core.Pool;
using UnityEngine;

namespace Core.Factory
{
	public abstract class UnityFactory<T> where T : MonoBehaviour
	{
		protected readonly Transform Parent;

		/// <summary>
		/// Change this name to give the objects made by this factory a common name.
		/// </summary>
		protected readonly string CommonName;

		private readonly string _namePrefix;
		private int _lastId;
		private Pool<T> _pool;

		/// <summary>
		/// Base Constructor. Always Call
		/// </summary>
		/// <param name="namePrefix">Prefix name to differentiate between
		/// objects made by the same type of Factory</param>
		/// <param name="commonName">Common name for all objects made by this Factory</param>
		/// <param name="parent">Parent object to which all the pool objects will be child</param>
		public UnityFactory(
			string namePrefix,
			string commonName,
			Transform parent = null)
		{
			_namePrefix = namePrefix;
			CommonName = commonName;
			Parent = parent;
			_lastId = 0;
		}

		public void InitializePool(int count, bool isDynamic)
		{
			_pool = new Pool<T>(
								InstantiateObject,
								EnableObject,
								DisposeObject,
								count,
								isDynamic);
			_pool.Initialize();
		}

		public IEnumerator InitializePoolInBatches(int count,
													bool isDynamic,
													int batchSize,
													float period,
													Action onBatch = null,
													Action onFinish = null)
		{
			_pool = new Pool<T>(
								InstantiateObject,
								EnableObject,
								DisposeObject,
								count,
								isDynamic);
			yield return _pool.InitializeInBatches(batchSize,
													period,
													onBatch,
													onFinish);
		}

		public virtual T Get() => _pool.GetObject();
		public virtual void Dispose(T player) => _pool.DisposeObject(player);

		protected abstract T InstantiateObject();

		protected abstract void EnableObject(T player);

		protected abstract void DisposeObject(T player);

		protected int GetNextID() => _lastId++;

		protected string GetName(int id) => _namePrefix + CommonName + id;
	}
}