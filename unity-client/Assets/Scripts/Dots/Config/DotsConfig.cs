using UnityEngine;

namespace Mdb.Ctd.Dots.Config
{
    [CreateAssetMenu(fileName = "DotsConfig", menuName = "ScriptableObjects/DotsConfig", order = 1)]
    public class DotsConfig : ScriptableObject, IDotsConfig
    {
        [field: SerializeField] public int Width { get; private set; } = 5;

        [field: SerializeField] public int Height { get; private set; } = 5;

        [field: SerializeField] public int MaxNewCellValue { get; private set; } = 64;
    }
}
