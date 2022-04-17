using System;
using System.Collections;

namespace Core
{
	public interface IInitializer
	{
		float InitializationState { get; }
		event Action OnInitialized;
		void SubscribeToOnInitialized(Action listener);
		void UnSubscribeToOnInitialized(Action listener);
		IEnumerator Initialize();
	}
}