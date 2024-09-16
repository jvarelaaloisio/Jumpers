using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Plugins.DebugSystem.Console.Utils
{
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class AutosizeText : MonoBehaviour
    {
        private const string PreferredSizeTooltip = "Preferred Size: limits the min size to the default one set before enabling this component.";
        private TMP_Text _tmpText;
        private RectTransform _rectTransform;
        /// <summary>
        /// The pivot that was set for the RectTransform before enabling this component.
        /// <p>We capture it after the first frame and use it to override the pivot on the Disable method.</p>
        /// </summary>
        private Vector2 _defaultPivot;
        /// <summary>
        /// The size that was set for the RectTransform before enabling this component.
        /// <p>We capture it after the first frame and use it to override the size on the Disable method.</p>
        /// </summary>
        private Vector2 _defaultSizeDeltaWithForcedPivot;

        [SerializeField] private Vector2 forcedPivot = Vector2.zero;
        [Tooltip(PreferredSizeTooltip)]
        [SerializeField] private ContentSizeFitter.FitMode horizontalFit;
        [Tooltip(PreferredSizeTooltip)]
        [SerializeField] private ContentSizeFitter.FitMode verticalFit;
        private void Reset()
        {
            FetchComponents();
        }

        private void Awake()
        {
            FetchComponents();
        }

        /// <summary>
        /// GetComponents
        /// </summary>
        private void FetchComponents()
        {
            _tmpText ??= GetComponent<TMP_Text>();
            _rectTransform ??= GetComponent<RectTransform>();
        }

        private void OnEnable()
        {
            StartCoroutine(Initialize());
        }

        /// <summary>
        /// Captures all the default values after the first frame.
        /// <p>Waiting one frame is needed to avoid reading data before other components run their Awake/OnEnable/Start
        /// (this way, we ensure we read the correct values).</p>
        /// </summary>
        /// <returns></returns>
        private IEnumerator Initialize()
        {
            yield return null;
            _defaultPivot = _rectTransform.pivot;
            _rectTransform.pivot = forcedPivot;
            _defaultSizeDeltaWithForcedPivot = _rectTransform.sizeDelta;
            
            _tmpText.OnPreRenderText += HandlePreRenderText;
            _tmpText.ForceMeshUpdate();

        }

        private void OnDisable()
        {
            _rectTransform.sizeDelta = _defaultSizeDeltaWithForcedPivot;
            _rectTransform.pivot = _defaultPivot;
            _tmpText.OnPreRenderText -= HandlePreRenderText;
        }

        private void HandlePreRenderText(TMP_TextInfo textInfo)
        {
            float width = 0;
            float height = 0;

            foreach (var lineInfo in textInfo.lineInfo)
            {
                width = Mathf.Max(width, lineInfo.maxAdvance);
                height += lineInfo.lineHeight;
            }
            
            FitSize(verticalFit, RectTransform.Axis.Vertical, height, _defaultSizeDeltaWithForcedPivot.y);
            FitSize(horizontalFit, RectTransform.Axis.Horizontal, width, _defaultSizeDeltaWithForcedPivot.x);
        }

        private void FitSize(ContentSizeFitter.FitMode fitMode,
                             RectTransform.Axis axis,
                             float minSize,
                             float preferredSize)
        {
            switch (fitMode)
            {
                case ContentSizeFitter.FitMode.MinSize:
                    _rectTransform.SetSizeWithCurrentAnchors(axis, minSize);
                    break;
                case ContentSizeFitter.FitMode.PreferredSize:
                    var size = Mathf.Max(preferredSize, minSize);
                    _rectTransform.SetSizeWithCurrentAnchors(axis, size);
                    break;
                case ContentSizeFitter.FitMode.Unconstrained:
                    break;
                default:
                    Debug.LogException(new ArgumentOutOfRangeException(nameof(fitMode), fitMode, null));
                    break;
            }
        }

        [SerializeField] private bool enableGizmos = true;
        private void OnDrawGizmos()
        {
            if (!enableGizmos)
                return;
            if (!_rectTransform)
                return;
            var points = new Vector3[4];
            _rectTransform.GetWorldCorners(points);
            Gizmos.color = Color.red;
            Gizmos.DrawLineStrip(points, true);
        }
    }
}
