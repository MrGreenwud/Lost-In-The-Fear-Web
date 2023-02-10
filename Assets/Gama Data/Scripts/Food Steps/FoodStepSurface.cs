using UnityEngine;
using NaughtyAttributes;

public class FoodStepSurface : MonoBehaviour
{
    public enum SurfaceType
    {
        Tarrain,
        Mash,
        Null
    }

    public enum MaterialType
    {
        Grass,
        Gravle,
        Wood
    }

    [SerializeField] private SurfaceType m_SurfaceType;

    [ShowIf("m_SurfaceType", SurfaceType.Mash)]
    [SerializeField] private MaterialType m_MaterialType;

    public SurfaceType GetSurfaceType() => m_SurfaceType;
    public MaterialType GetMaterialType() => m_MaterialType;
}
