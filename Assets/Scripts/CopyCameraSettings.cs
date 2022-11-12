using UnityEngine;

[CreateAssetMenu]
public class CopyCameraSettings : ScriptableObject
{
    [SerializeField]
    private Vector2Int worldTileSize;
    public Vector2Int WorldTileSize => worldTileSize;

    [SerializeField]
    private Vector2Int viewTileSize;
    public Vector2Int ViewTileSize => viewTileSize;
}
