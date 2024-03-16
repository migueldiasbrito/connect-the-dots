using UnityEngine;

namespace Mdb.Ctd.Utils
{
    public static class AnimatorUtils
    {
        public static int Value => Animator.StringToHash("Value");

        public static int Selected => Animator.StringToHash("Selected");
    }
}
