using Mdb.Ctd.Dots.Data;
using Mdb.Ctd.Utils;
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

            _animator.SetInteger(AnimatorUtils.Value, Dot.Value);
            _valueLabel.text = Dot.Value.ToString();

            Rect parent = ((RectTransform)transform.parent).rect;
            ((RectTransform)transform).sizeDelta = new Vector2(parent.width, parent.height);
        }

        internal void SetSelected(bool selected)
        {
            _animator.SetBool(AnimatorUtils.Selected, selected);
        }
    }
}
