using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _game.Scripts.Core
{
    [RequireComponent(typeof(Canvas), typeof(CanvasGroup))]
    public class UiController : MonoBehaviour
    {
        private const float FadeAnimationDuration = .3f;
        private Canvas _canvas;
        private CanvasGroup _canvasGroup;
        private bool _isInitialized;

        private void Initialize()
        {
            _isInitialized = true;
            _canvas = GetComponent<Canvas>();
            _canvasGroup = GetComponent<CanvasGroup>();
        }

        public virtual void Show()
        {
            if (!_isInitialized) Initialize();

            _canvas.enabled = true;
            _canvasGroup.interactable = true;
            _canvasGroup.DOFade(1, FadeAnimationDuration).OnComplete(delegate { _canvasGroup.blocksRaycasts = true; });
        }

        public virtual void Hide()
        {
            _canvasGroup.interactable = false;
            _canvasGroup.DOFade(0, FadeAnimationDuration).OnComplete(delegate
            {
                _canvas.enabled = false;
                _canvasGroup.blocksRaycasts = false;
            });
        }

#if UNITY_EDITOR
        [ButtonGroup]
        public void Editor_ShowInstant()
        {
            if (_canvas == null) _canvas = GetComponent<Canvas>();
            if (_canvasGroup == null) _canvasGroup = GetComponent<CanvasGroup>();

            _canvas.enabled = true;
            _canvasGroup.alpha = 1f;
            _canvasGroup.interactable = true;
            _canvasGroup.blocksRaycasts = true;
        }

        [ButtonGroup]
        public void Editor_HideInstant()
        {
            if (_canvas == null) _canvas = GetComponent<Canvas>();
            if (_canvasGroup == null) _canvasGroup = GetComponent<CanvasGroup>();

            _canvas.enabled = false;
            _canvasGroup.alpha = 0f;
            _canvasGroup.interactable = false;
            _canvasGroup.blocksRaycasts = false;
        }
#endif
    }
}