using TMPro;
using UnityEngine;
using System;
using System.Collections;

namespace STGames
{
    public class WheelLetterView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI text;
        public char Letter { get; set; }
        public Vector3 TargetPosition { get; set; }
        public string Id { get; private set; }

        private void Awake()
        {
            text.color = Color.white;
            text.transform.localScale = Vector3.one;
        }

        public void SetLetter(char letter)
        {
            Letter = letter;
            text.SetText(letter.ToString());
            Id = Guid.NewGuid().ToString();
        }

        public void SetPosition(Vector3 position)
        {
            TargetPosition = position;
            transform.localPosition = TargetPosition;
        }

        public void Destroy()
        {
            Destroy(gameObject);
        }
    }
}