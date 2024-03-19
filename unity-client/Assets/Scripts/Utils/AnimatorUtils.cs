using UnityEngine;

namespace Mdb.Ctd.Utils
{
    public static class AnimatorUtils
    {
        public static int Value => Animator.StringToHash("Value");

        public static int Selected => Animator.StringToHash("Selected");

        public static int Visible => Animator.StringToHash("Visible");

        public static int New => Animator.StringToHash("New");
    }
}
