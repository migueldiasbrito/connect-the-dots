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

        public void Setup(IDot dot)
        {
            _animator.SetInteger(AnimatorUtils.Value, dot.Value);
            _valueLabel.text = dot.Value.ToString();

            Rect parent = ((RectTransform)transform.parent).rect;
            ((RectTransform)transform).sizeDelta = new Vector2(parent.width, parent.height);
        }
    }
}
