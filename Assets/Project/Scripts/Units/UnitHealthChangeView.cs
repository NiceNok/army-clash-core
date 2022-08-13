using System;
using TMPro;
using UnityEngine;

namespace Project.Scripts.Units
{
    public class UnitHealthChangeView : MonoBehaviour
    {
        public event Action<UnitHealthChangeView> OnFinish;
        [SerializeField] private TextMeshPro text;
        private readonly Vector3 _textRotationToUser = new Vector3(-45, -90, 0);
        
        public void SetText(string text)
        {
            this.text.text = text;
        }

        private void Update()
        {
            text.transform.eulerAngles = _textRotationToUser;
        }

        public void FinishAnimation()
        {
            OnFinish?.Invoke(this);
        }
    }
}
