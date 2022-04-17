using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core
{
	[Obsolete]
	public static class Spawner
	{
		public static IEnumerator SpawnOverTime<T>(int spawnQuantity,
		                                           int iterationSize,
		                                           float iterationWaitTime,
		                                           Func<T> spawnMethod,
		                                           Action<List<T>> onIteration = null,
		                                           Action<List<T>> onFinish = null)
		{
			var wait = new WaitForSeconds(iterationWaitTime);
			List<T> result = new List<T>(spawnQuantity);
			for (int i = 0; i < spawnQuantity; i++)
			{
				List<T> currentIteration = new List<T>(iterationSize);
				for (int j = 0; j < iterationSize; j++)
					currentIteration.Add(spawnMethod());
				
				result.AddRange(currentIteration);
				
				onIteration?.Invoke(currentIteration);
				yield return wait;
			}
			onFinish?.Invoke(result);
		}
	}
}