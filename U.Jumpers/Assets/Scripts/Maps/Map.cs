using System;
using Core;

namespace Maps
{
	[Serializable]
	public struct Map
	{
		public int[,,] Layout;

		public Map(int[,,] layout)
		{
			this.Layout = layout;
		}

		public Map(int width, int height, int depth)
		{
			Layout = new int[width, height, depth];
		}

		public Map(Int3 size)
		{
			Layout = new int[size.x, size.y, size.z];
		}
	}
}