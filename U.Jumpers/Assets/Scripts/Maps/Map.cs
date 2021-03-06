using System;

namespace Maps
{
	[Serializable]
	public struct Map
	{
		public int[] Layout { get; set; }

		public Map(int[] layout)
		{
			Layout = layout;
		}

		public Map(int size)
		{
			Layout = new int[size];
		}
	}
}