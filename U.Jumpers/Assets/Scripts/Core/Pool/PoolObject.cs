using System;

namespace Core.Pool
{
	public sealed class PoolObject<T>
	{
		private bool _isActive;
		public T Obj { get; }

		private readonly Action<T> _onEnable;
		private readonly Action<T> _onDispose;

		public PoolObject(T obj, Action<T> onEnable, Action<T> onDispose)
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