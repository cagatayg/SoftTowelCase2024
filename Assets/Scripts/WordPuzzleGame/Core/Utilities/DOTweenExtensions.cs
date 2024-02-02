using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Extensions.DOTweenExtensions
{
    public static class DOTweenExtensions
    {
   
        public static Tweener DOFade(this Image target, float endValue, float duration)
        {
            return DOTween.ToAlpha(() => target.color, x => target.color = x, endValue, duration)
                .SetTarget(target);
        }


        public static Tweener DOFade(this RawImage target, float endValue, float duration)
        {
            return DOTween.ToAlpha(() => target.color, x => target.color = x, endValue, duration)
                .SetTarget(target);
        }

        public static Tweener DOFade(this CanvasGroup target, float endValue, float duration)
        {
            return DOTween.To(() => target.alpha, x => target.alpha = x, endValue, duration)
                .SetTarget(target);
        }

        public static Tweener DOColor(this Image target, Color endValue, float duration)
        {
            return DOTween.To(() => target.color, x => target.color = x, endValue, duration)
                .SetTarget(target);
        }
    }
}
