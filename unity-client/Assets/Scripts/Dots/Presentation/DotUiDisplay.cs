using Mdb.Ctd.Dots.Data;
using Mdb.Ctd.Utils;
using System;
using TMPro;
using UnityEngine;

namespace Mdb.Ctd.Dots.Presentation
{
    public class DotUiDisplay : MonoBehaviour
    {
        [field: SerializeField] protected Animator Animator { get; private set; }
        [SerializeField] private TMP_Text _valueLabel;

        public void SetVisible(bool visible)
        {
            Animator.SetBool(AnimatorUtils.Visible, visible);
        }

        public void SetValue(int value)
        {
            _valueLabel.text = value.ToString();
            Animator.SetInteger(AnimatorUtils.Value, value);
        }
    }
}
