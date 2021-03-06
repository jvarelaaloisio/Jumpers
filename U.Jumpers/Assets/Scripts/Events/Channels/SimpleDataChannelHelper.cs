using System;

namespace Events.Channels
{
	public static class SimpleDataChannelHelper
	{
		public static void SubscribeSafely<T>(
			this ChannelSo<T> channel,
			in Action<T> handler)
		{
			if (channel) channel.Subscribe(handler);
		}

		public static void RaiseEventSafely<T>(
			this ChannelSo<T> channel,
			in T data)
		{
			if (channel) channel.RaiseEvent(data);
		}
	}
}