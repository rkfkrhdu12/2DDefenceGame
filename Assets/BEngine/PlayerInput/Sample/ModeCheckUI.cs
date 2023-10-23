using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


namespace BEngineSample
{
    public class ModeCheckUI : InteractUI
    {
        public override void Interact()
        {
            _textBox.enabled = !_textBox.enabled;
        }

        #region Variable

        [SerializeField, HideInInspector]
        private TMP_Text _textBox;

        #endregion

        #region Unity Function

        protected new void Start()
        {
            base.Start();

            if (_inputSystem == null) return;

            if (_textBox != null) return;

            _textBox = GetComponent<TMP_Text>();
            if (_textBox == null) return;

            _textBox.enabled = CurrentUIMode;
        }

        #endregion
    }
}