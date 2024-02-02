using System;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Extensions.DOTweenExtensions;
using TMPro;


namespace STGames
{
    public class DefaultLetterBox : BaseLetterBox
    {
        [SerializeField] private float notFoundAlpha;
        [SerializeField] private float targetScale;
        [SerializeField] private Image backgroundImage;
        [SerializeField] private Image letterContainerImage;
        [SerializeField] private TextMeshProUGUI letterText;
        [SerializeField] private Canvas letterCanvas;
        
        private void Awake()
        {
            transform.localScale = Vector3.zero;
        }

        public override void Configure(char letter, int2 position, float2 targetSize)
        {
            base.Configure(letter, position, targetSize);
            letterText.SetText(letter.ToString());
            letterCanvas.sortingOrder = 80 - position.y + 2;
        }

        public override void Disappear()
        {
            Destroy(gameObject);
        }

        public override void AppearEmpty()
        {
            var sequence = DOTween.Sequence();
            sequence.AppendInterval((Position.x + Position.y) / 40f);
            sequence.Append(backgroundImage.DOFade(notFoundAlpha, 0.4f));
            sequence.Join(transform.DOScale(targetScale, 0.2f));
        }

        public override void Found()
        {
            if (IsFound) return;
            AppearEmpty();
            letterContainerImage.transform.position = transform.position;
            letterContainerImage.transform.localScale = Vector3.one;
            IsFound = true;
        }
    }
}