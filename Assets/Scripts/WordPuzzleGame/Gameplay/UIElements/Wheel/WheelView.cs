using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;

namespace STGames
{
    public class WheelView : Singleton<WheelView>
    {
        [SerializeField] private RectTransform wheelCenterTransform;
        [SerializeField] private RectTransform wheelRadiusTransform;
        [SerializeField] private GameObject wheelLetterPrefab;
        private float wheelRadius;
        private float localWheelRadius;
        private List<WheelLetterView> letterViews;

        private readonly Dictionary<int, float> _letterFactors = new Dictionary<int, float>()
        {
            { 3, 0.800f },
            { 4, 0.835f },
            { 5, 0.855f },
            { 6, 0.885f },
            { 7, 0.915f },
            { 8, 0.935f },
            { 9, 0.915f },
            { 10, 0.915f },
        };

        private readonly Dictionary<int, float> _selectableRadiusFactors = new Dictionary<int, float>()
        {
            { 3, 0.7f },
            { 4, 0.7f },
            { 5, 0.7f },
            { 6, 0.7f },
            { 7, 0.7f },
            { 8, 0.65f },
            { 9, 0.65f },
            { 10, 0.65f },
        };

        private readonly Dictionary<int, float> _letterScaleFactors = new Dictionary<int, float>()
        {
            { 3, 1.4f },
            { 4, 1.3f },
            { 5, 1.2f },
            { 6, 1.1f },
            { 7, 1f },
            { 8, 0.95f },
            { 9, 0.95f },
            { 10, 0.95f },
        };

        public void Create()
        {
            gameObject.SetActive(true);
            DestroyGameObjects();
            CreateWheelLetters();
            CalculateWheelRadius();
            UpdateLetterPositions();
            GameBootstrap.WheelLetters = letterViews;
        }


        public void Destroy()
        {
            DestroyGameObjects();
            StopAllCoroutines();
            gameObject.SetActive(false);
        }


        private void DestroyGameObjects()
        {
            if (letterViews != null)
            {
                foreach (var letterView in letterViews)
                {
                    if (letterView != null) letterView.Destroy();
                }
            }

            letterViews = new List<WheelLetterView>();
        }

        private void CalculateWheelRadius()
        {
            var puzzle = GameBootstrap.CurrentPuzzle;
            var f = 1.15f;
            var letterFactor = _letterFactors[puzzle.Letters.Length];
            wheelRadius = 1.024836f * f * letterFactor;
            localWheelRadius = 249.65f * f * letterFactor;
        }

        private WheelLetterView CreateWheelLetter()
        {
            var puzzle = GameBootstrap.CurrentPuzzle;
            var wheelLetterView = Instantiate(wheelLetterPrefab, wheelCenterTransform);
            wheelLetterView.transform.localScale = Vector3.one * _letterScaleFactors[puzzle.Letters.Length];
            return wheelLetterView.GetComponent<WheelLetterView>();
        }

        private Vector3 CalculateLetterPosition(int letterCount, int letterIndex)
        {
            var theta = -2 * math.PI / letterCount;
            var angle = theta * letterIndex + (math.PI / 2);
            var center = new Vector3(0, 0, 0);
            var x = center.x + localWheelRadius * math.cos(angle);
            var y = center.y + localWheelRadius * math.sin(angle);
            return new Vector3(x, y, 0);
        }

        private void CreateWheelLetters()
        {
            var puzzle = GameBootstrap.CurrentPuzzle;
            foreach (var letter in puzzle.Letters.OrderBy((t) => Common.Random.Next()))
            {
                var letterView = CreateWheelLetter();
                letterView.SetLetter(letter);
                letterViews.Add(letterView);
            }
        }

        private void UpdateLetterPositions(bool force = false)
        {
            for (int i = 0; i < letterViews.Count; i++)
            {
                var view = letterViews[i];
                var targetPosition = CalculateLetterPosition(letterViews.Count, i);
                if (view.TargetPosition == targetPosition && !force) continue;
                view.SetPosition(targetPosition);
            }
        }
    }
}