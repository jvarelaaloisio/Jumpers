using UnityEngine;

namespace Plugins.DebugSystem.Console.Extensions
{
    public static class StringExtensions
    {
        public static string Colored(this string original, Color color)
            => $"<color=#{ColorUtility.ToHtmlStringRGB(color)}>{original}</color>";
    }
}