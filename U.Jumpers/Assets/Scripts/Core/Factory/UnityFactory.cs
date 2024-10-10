using System;
using System.Collections;
using Core.Pool;
using UnityEngine;

namespace Core.Factory
{
	public abstract class UnityFactory
	{
		protected readonly Transform Parent;

		/// <summary>
		/// Change this name to give the objects made by this factory a common name.
		/// </summary>
		protected readonly string CommonName;

		private readonly string _namePrefix;
		private int _lastId;

		/// <summary>
		/// Base Constructor. Always Call
		/// </summary>
		/// <param name="namePrefix">Prefix name to differentiate between
		/// objects made by the same type of Factory</param>
		/// <param name="commonName">Common name for all objects made by this Factory</param>
		/// <param name="parent">Parent object to which all the pool objects will be child</param>
		protected UnityFactory(
			string namePrefix,
			string commonName,
			Transform parent = null)
		{
			_namePrefix = namePrefix;
			CommonName = commonName;
			Parent = parent;
			_lastId = 0;
		}
		

		protected string GetName(int id) => _namePrefix + CommonName + id;

		protected int GetNextID() => _lastId++;
		//TODO: Remove pool from factory
		public abstract void InitializePool(int count, bool isDynamic);
	}
	public abstract class UnityFactory<T> : UnityFactory where T : MonoBehaviour
	{
		private Pool<T> _pool;

		public override void InitializePool(int count, bool isDynamic)
		{
			_pool = new Pool<T>(
								Create,
								EnableObject,
								DisposeObject,
								count,
								isDynamic);
			_pool.Initialize();
		}
		
		protected UnityFactory(string namePrefix, string commonName, Transform parent = null)
			: base(namePrefix, commonName, parent) { }

		public IEnumerator InitializePoolInBatches(int count,
		                                           bool isDynamic,
		                                           int batchSize,
		                                           float period,
		                                           Action onBatch = null,
		                                           Action onFinish = null)
		{
			_pool = new Pool<T>(
								Create,
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

		protected abstract T Create();

		protected abstract void EnableObject(T player);

		protected abstract void DisposeObject(T player);
	}
}