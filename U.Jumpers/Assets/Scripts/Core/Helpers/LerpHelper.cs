using UnityEngine;

namespace Helpers
{
	public static class LerpHelper
	{
		/// <summary>
		/// Quick at the start and end. Slow in the middle
		/// </summary>
		/// <param name="x"></param>
		/// <returns></returns>
		public static float GetArcSinLerp(float x)
		{
			return Mathf.Asin(2 * x - 1)/ Mathf.PI + .5f;
		}
		/// <summary>
		/// Ease in. Accelerates till the end
		/// </summary>
		/// <param name="x"></param>
		/// <returns></returns>
		public static float GetCircumferenceLerp(float x)
		{
			return 1 - Mathf.Sqrt(1 - Mathf.Pow(x, 2));
		}
		/// <summary>
		/// Ease in, Ease out
		/// </summary>
		/// <param name="x"></param>
		/// <returns></returns>
		public static float GetSinLerp(float x)
		{
			return (Mathf.Sin((x - .5f) * Mathf.PI) + 1) / 2;
		}
	}
}