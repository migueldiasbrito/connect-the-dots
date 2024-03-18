using Mdb.Ctd.Utils;
using UnityEngine;

namespace Mdb.Ctd.Dots.Presentation
{
    public class ConnectionUiDisplay : MonoBehaviour
    {
        [SerializeField] private Animator _animator;

        public void SetVisible(bool visible)
        {
            _animator.SetBool(AnimatorUtils.Visible, visible);
        }

        public void SetValue(int value)
        {
            _animator.SetInteger(AnimatorUtils.Value, value);
        }
    }
}
