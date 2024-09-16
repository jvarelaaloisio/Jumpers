using UnityEngine;
using UnityEngine.UI;

namespace Plugins.DebugSystem.Console.Utils
{
    [ExecuteAlways]
    public class DynamicCanvasScaler : CanvasScaler
    {
        private void Update()
        {
            var aspectRatio = (float) Screen.width / Screen.height;
            var isHorizontal = aspectRatio > 1;
            m_MatchWidthOrHeight = isHorizontal ? 1 : 0;
        }
    }
}
