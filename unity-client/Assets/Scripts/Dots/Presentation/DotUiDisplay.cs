using Mdb.Ctd.Dots.Data;
using Mdb.Ctd.Utils;
using System;
using TMPro;
using UnityEngine;

namespace Mdb.Ctd.Dots.Presentation
{
    public class DotUiDisplay : MonoBehaviour
    {
        [SerializeField] private Animator _animator;
        [SerializeField] private TMP_Text _valueLabel;

        public IDot Dot { get; private set; }

        public void Setup(IDot dot)
        {
            Dot = dot;

            SetValue(Dot.Value);
            SetVisible(true);

            Rect parent = ((RectTransform)transform.parent).rect;
            ((RectTransform)transform).sizeDelta = new Vector2(parent.width, parent.height);
        }

        public void SetSelected(bool selected)
        {
            _animator.SetBool(AnimatorUtils.Selected, selected);
        }

        public void SetVisible(bool visible)
        {
            _animator.SetBool(AnimatorUtils.Visible, visible);
        }

        public void SetValue(int value)
        {
            _valueLabel.text = value.ToString();
            _animator.SetInteger(AnimatorUtils.Value, value);
        }
    }
}
