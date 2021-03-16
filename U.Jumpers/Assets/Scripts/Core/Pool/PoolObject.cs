using System;

namespace Core.Pool
{
	public class PoolObject<T>
	{
		private T _obj;
		private bool _isActive;

		private Action<T> _onEnable;
		private Action<T> _onDispose;

		public PoolObject(T obj, Action<T> onEnable, Action<T> onDispose)
		{
			_isActive = false;
			_obj = obj;
			_onEnable = onEnable;
			_onDispose = onDispose;
		}

		public bool IsActive
		{
			get => _isActive;
			set
			{
				_isActive = value;
				if (_isActive)
					_onEnable?.Invoke(_obj);
				else
					_onDispose?.Invoke(_obj);
			}
		}
	}
}