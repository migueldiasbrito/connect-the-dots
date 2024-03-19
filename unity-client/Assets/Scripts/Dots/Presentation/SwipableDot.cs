using Mdb.Ctd.Swipe;
using UnityEngine;

namespace Mdb.Ctd.Dots.Presentation
{
    public class SwipableDot : MonoBehaviour, ISwipable
    {
        [field: SerializeField] public GridDotUiDisplay Dot { get; private set; }
    }
}
