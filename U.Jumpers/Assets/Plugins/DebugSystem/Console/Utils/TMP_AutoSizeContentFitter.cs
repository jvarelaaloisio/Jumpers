using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Plugins.DebugSystem.Console.Utils
{
    [Obsolete]
    [RequireComponent(typeof(TextMeshProUGUI))]
    [RequireComponent(typeof(RectTransform))]
    [ExecuteAlways]
    public class TMP_AutoSizeContentFitter : ContentSizeFitter
    {
        private TextMeshProUGUI _tmpText;
        private RectTransform _rectTransform;

        protected override void Reset()
        {
            base.Reset();
            FetchComponents();
        }

        protected override void Awake()
        {
            base.Awake();
            FetchComponents();
        }

        private void FetchComponents()
        {
            _tmpText ??= GetComponent<TextMeshProUGUI>();
            _rectTransform ??= GetComponent<RectTransform>();
        }

        public override void SetLayoutHorizontal()
        {
            UpdatePreferredSize(out var preferredSize);
            SetLayoutHorizontal(preferredSize.x);
        }

        public override void SetLayoutVertical()
        {
            UpdatePreferredSize(out var preferredSize);
            SetLayoutVertical(preferredSize.y);
        }

        private void UpdatePreferredSize(out Vector2 preferredSize)
        {
            _tmpText.ForceMeshUpdate();
            var preferredValues = _tmpText.GetPreferredValues();
            var renderedValues = _tmpText.GetRenderedValues();
            preferredSize = _tmpText.enableAutoSizing
                                ? renderedValues
                                : preferredValues;
            var calculated = CalculateTextSize(_tmpText);
            preferredSize = calculated;
            Vector2 CalculateTextSize(TextMeshProUGUI textMeshPro)
            {
                textMeshPro.ForceMeshUpdate();

                float width = 0;
                float height = 0;

                for (var i = 0; i < textMeshPro.textInfo.lineCount; i++)
                {
                    var lineInfo = textMeshPro.textInfo.lineInfo[i];
                    width = Mathf.Max(width, lineInfo.maxAdvance);
                    height += lineInfo.lineHeight;
                }

                return new Vector2(width, height);
            }
        }

        private void SetLayoutHorizontal(float width)
        {
            if (horizontalFit == FitMode.PreferredSize)
            {
                _rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, width);
            }
        }

        private void SetLayoutVertical(float height)
        {
            if (verticalFit == FitMode.PreferredSize)
            {
                _rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, height);
            }
        }
    }
}