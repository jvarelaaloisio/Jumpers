using System;
using TMPro;
using UnityEngine;

namespace Plugins.DebugSystem.Console.Utils
{
    [ExecuteAlways]
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class TMP_FitFontSizeToScreen : MonoBehaviour
    {
        [SerializeField] private float minSize = 14;
        [SerializeField] private float maxSize = 36;
        [SerializeField] private float fontToScreenWidthRatio = 53;
        private TMP_Text _tmpText;

        private void Reset() => FetchComponents();

        private void Awake() => FetchComponents();

        /// <summary>
        /// GetComponents
        /// </summary>
        private void FetchComponents()
        {
            _tmpText ??= GetComponent<TMP_Text>();
        }

        private void Update()
        {
            _tmpText = GetComponent<TMP_Text>();
            _tmpText.enableAutoSizing = false;
            _tmpText.fontSize = Mathf.Clamp(Screen.width / fontToScreenWidthRatio, minSize, maxSize);
        }
    }
}
