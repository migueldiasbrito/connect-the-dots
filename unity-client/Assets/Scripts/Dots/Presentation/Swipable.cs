using Mdb.Ctd.Swipe;
using UnityEngine;

namespace Mdb.Ctd.Dots.Presentation
{
    public class Swipable : MonoBehaviour, ISwipable
    {
        [field: SerializeField] public DotUiDisplay Dot { get; private set; }
    }
}
