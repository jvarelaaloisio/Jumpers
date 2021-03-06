using System;

namespace Core
{
	[Serializable]
	public struct Int3
	{
		private static readonly Int3 ZeroInt = new Int3(0);
		private static readonly Int3 OneInt = new Int3(1);
		public int x;
		public int y;
		public int z;

		public Int3(int values)
		{
			x = values;
			y = values;
			z = values;
		}
		public Int3(int x, int y, int z)
		{
			this.x = x;
			this.y = y;
			this.z = z;
		}

		public static Int3 Zero => ZeroInt;

		public static Int3 One => OneInt;
	}
}