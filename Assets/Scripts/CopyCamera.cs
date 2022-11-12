using UnityEngine;
using NaughtyAttributes;
using UnityEngine.Tilemaps;

public struct Photo
{
    public TileBase[] tiles;
    public bool hasCollider; // not working 
    public bool hasRididbody; // not working
}

public class CopyCamera : MonoBehaviour
{
    [Header("To Link")]
    [SerializeField]
    private CopyCameraSettings settings;
    public CopyCameraSettings Settings => settings;

    [SerializeField]
    private SpriteRenderer spriteRenderer;
    public SpriteRenderer SpriteRenderer => spriteRenderer;


    [Header("States")]
    [ReadOnly]
    public Vector2Int intPosition;

    private void Start()
    {
        spriteRenderer.size = settings.ViewTileSize;
    }
}
