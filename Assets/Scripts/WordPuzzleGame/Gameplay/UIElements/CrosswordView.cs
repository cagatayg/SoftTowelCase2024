using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

namespace STGames
{
    public class CrosswordView : Singleton<CrosswordView>
    {
        public List<BaseLetterBox> LetterBoxes;
        [SerializeField] private GridLayoutGroup gridLayout;
        [SerializeField] private BaseLetterBox baseLetterPrefab;
        [SerializeField] private BaseLetterBox emptyBaseLetterPrefab;
        [SerializeField] private GameObject topAnchor;
        [SerializeField] private GameObject bottomAnchor;
        private float height;
        private float letterSize;
 
        public void Create()
        {
            DestroyGameObjects();
            UpdateLetterSize(GameBootstrap.CurrentPuzzle.Dimensions.x);
            CreateLetterBoxes();
        }

        public void Destroy()
        {
            DestroyGameObjects();
        }

        public void Draw()
        {
            foreach (var letterBox in LetterBoxes)
            {
                if (letterBox.IsEmpty) continue;
                letterBox.AppearEmpty();
            }
        }

        private void UpdateViewHeight()
        {
            height = Vector3.Distance(topAnchor.transform.localPosition, bottomAnchor.transform.localPosition);
            GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical,
                height);
        }

        private void DestroyGameObjects()
        {
            foreach (Transform child in transform)
            {
                Destroy(child.gameObject);
            }

            LetterBoxes = new List<BaseLetterBox>();
            var letters = FindObjectsOfType<BaseLetterBox>();
            if (letters != null && letters.Length > 0)
            {
                foreach (var item in letters)
                {
                    Destroy(item.gameObject);
                }
            }
        }

        private float ComputeLetterSize()
        {
            var dimensions = GameBootstrap.CurrentPuzzle.Dimensions;
            var rectTransform = gridLayout.gameObject.GetComponent<RectTransform>();
            var rect = rectTransform.rect;
            var possibleW = rect.width - 20;
            var possibleH = height - 20;
            var sizeBasedOnWidth = possibleW / dimensions.x;
            var sizeBasedOnHeight = possibleH / dimensions.y;
            var tileSize = Mathf.Min(sizeBasedOnWidth, sizeBasedOnHeight);
            return Mathf.Min(240, tileSize);
        }

        private void UpdateLetterSize(int constraintCount)
        {
            UpdateViewHeight();
            gridLayout.constraintCount = constraintCount;
            letterSize = ComputeLetterSize();
            gridLayout.cellSize = new Vector2(letterSize, letterSize);
        }

        private BaseLetterBox CreateLetterBox()
        {
            return Instantiate(baseLetterPrefab, gridLayout.transform);
        }

        private BaseLetterBox CreateLetterBoxEmpty()
        {
            return Instantiate(emptyBaseLetterPrefab, gridLayout.transform);
        }

        private void CreateLetterBoxes()
        {
            var puzzle = GameBootstrap.CurrentPuzzle;
            var positions = puzzle.GetAllPositionsAsSet();
            var dimension = puzzle.Dimensions;

            for (var i = 0; i < dimension.y; i++)
            {
                for (var j = 0; j < dimension.x; j++)
                {
                    var position = new int2(j, i);
                    var isLetter = positions.Contains(position);
                    var letterBox = isLetter ? CreateLetterBox() : CreateLetterBoxEmpty();
                    if (isLetter)
                    {
                        var letter = puzzle.GetLetterAtPosition(position);
                        letterBox.Configure(letter, position, new float2(letterSize, letterSize));
                    }
                    else
                    {
                        letterBox.Configure(' ', position, new float2(letterSize, letterSize));
                    }
                    LetterBoxes.Add(letterBox);
                }
            }
            GameBootstrap.LetterBoxes = LetterBoxes;
        }
    }
}