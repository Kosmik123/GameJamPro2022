using UnityEngine;
using NaughtyAttributes;
using UnityEngine.Tilemaps;

public class CopyCamera : MonoBehaviour
{
    [Header("To Link")]
    [SerializeField]
    private CopyCameraSettings settings;
    public CopyCameraSettings Settings => settings;

    [Header("Visuals")]
    [SerializeField]
    private Transform visuals;
    public Transform Visuals => visuals;

    [SerializeField]
    private SpriteRenderer photoModeGraphic;
    public SpriteRenderer PhotoModeGraphic  => photoModeGraphic;
    [SerializeField]
    private SpriteRenderer spawnModeGraphic;
    public SpriteRenderer SpawnModeGraphic  => spawnModeGraphic;

    [Header("States")]
    [ReadOnly]
    public Vector2Int intPosition;

    private void Start()
    {
        RefreshVisualsSizes();
    }

    private void RefreshVisualsSizes()
    {

    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, (Vector2)settings.ViewTileSize);
    }



}
