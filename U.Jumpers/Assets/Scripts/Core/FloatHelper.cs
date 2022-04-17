using System;

namespace Core
{
	public static class FloatHelper
	{
		public static float DifferenceWith(this float baseNumber, float compared)
			=> Math.Abs(baseNumber - compared);
	}
}
