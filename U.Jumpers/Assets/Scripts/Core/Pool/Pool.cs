using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Core.Pool
{
	public class Pool<T>
	{
		private readonly List<PoolObject<T>> _objects;
		private readonly Action<T> _onEnableObject;
		private readonly Action<T> _onDisposeObject;
		private readonly Func<T> _constructObject;
		private readonly bool _isDynamic;
		private int _count;

		public Pool(Func<T> constructObject,
					Action<T> onEnableObject,
					Action<T> onDisposeObject,
					int initialStock,
					bool isDynamic = true)
		{
			_constructObject = constructObject;
			_onEnableObject = onEnableObject;
			_onDisposeObject = onDisposeObject;
			_count = initialStock;
			_isDynamic = isDynamic;
			_objects = new List<PoolObject<T>>(initialStock);
		}

		public void Initialize()
		{
			_objects.AddRange(Generate(_constructObject,
										_onEnableObject,
										_onDisposeObject,
										_count));
		}

		public IEnumerator InitializeInBatches(int batchSize,
												float period,
												Action onBatch = null,
												Action onFinish = null)
		{
			var waitForPeriod = new WaitForSeconds(period);
			for (int objectsLeft = _count; objectsLeft > 0; objectsLeft -= batchSize)
			{
				_objects.AddRange(
								Generate(_constructObject,
										_onEnableObject,
										_onDisposeObject,
										Math.Min(batchSize, objectsLeft))
								);
				onBatch?.Invoke();
				yield return waitForPeriod;
			}

			onFinish?.Invoke();
		}

		private static IEnumerable<PoolObject<T>> Generate(Func<T> constructObject,
															Action<T> onEnableObject,
															Action<T> onDisposeObject,
															int quantity)
		{
			while (quantity-- > 0)
				yield return new PoolObject<T>(constructObject(), onEnableObject, onDisposeObject);
		}

		public T GetObject()
		{
			for (int i = 0; i < _count; i++)
			{
				if (_objects[i].IsActive)
					continue;
				_objects[i].IsActive = true;
				return _objects[i].Obj;
			}

			if (!_isDynamic)
				return default;

			PoolObject<T> newObject;
			_objects.Add(newObject = new PoolObject<T>(_constructObject(), _onEnableObject, _onDisposeObject));
			_count++;
			newObject.IsActive = true;
			return newObject.Obj;
		}

		public void DisposeObject(T obj)
		{
			var poolObject = _objects.Find(op => op.Obj.Equals(obj));
			if (poolObject is null)
				return;
			poolObject.IsActive = false;
		}

		private sealed class PoolObject<TR>
		{
			private bool _isActive;
			public TR Obj { get; }

			private readonly Action<TR> _onEnable;
			private readonly Action<TR> _onDispose;

			public PoolObject(TR obj, Action<TR> onEnable, Action<TR> onDispose)
			{
				Obj = obj;
				_onEnable = onEnable;
				_onDispose = onDispose;
				_isActive = false;
			}

			public bool IsActive
			{
				get => _isActive;
				set
				{
					_isActive = value;
					if (_isActive)
						_onEnable(Obj);
					else
						_onDispose(Obj);
				}
			}
		}
	}
}