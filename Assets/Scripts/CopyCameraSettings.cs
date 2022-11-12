using UnityEngine;
using NaughtyAttributes;

[CreateAssetMenu]
public class CopyCameraSettings : ScriptableObject
{
    [SerializeField]
    private Vector2Int worldTileSize;
    public Vector2Int WorldTileSize => worldTileSize;

    [SerializeField]
    private Vector2Int viewTileSize;
    public Vector2Int ViewTileSize => viewTileSize;

    [SerializeField]
    private float actionHoldTime;
    public float ActionHoldTime => actionHoldTime;

    [SerializeField, SortingLayer]
    private string terrainSortingLayer;
    public int TerrainSortingLayerID => SortingLayer.NameToID(terrainSortingLayer);

    private void OnValidate()
    {
        if (actionHoldTime <= 0)
            actionHoldTime = 0.001f;
    }

}
