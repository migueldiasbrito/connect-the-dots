using Mdb.Ctd.Dots.Data;
using Mdb.Ctd.Swipe;
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

        public void Setup(IDot dot)
        {
            _animator.SetInteger(AnimatorUtils.Value, dot.Value);
            _valueLabel.text = dot.Value.ToString();

            Rect parent = ((RectTransform)transform.parent).rect;
            ((RectTransform)transform).sizeDelta = new Vector2(parent.width, parent.height);
        }

        internal void SetSelected(bool selected)
        {
            _animator.SetBool(AnimatorUtils.Selected, selected);
        }
    }
}
