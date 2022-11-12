using UnityEngine;
using NaughtyAttributes;
using UnityEngine.Tilemaps;

[SerializeField]
public class Photo
{
    public TileBase[] tiles;
    public bool hasCollider; // not working 
    public bool hasRididbody; // not working
}


public class CopyCameraPhotosController : MonoBehaviour
{
    [Header("To Link")]
    [SerializeField]
    private TilemapsManager tilemapsManager;

    [Header("Elements")]
    [SerializeField]
    private CopyCamera copyCamera;
    [Space, SerializeField]
    private Tilemap photoTilemap;
    public Tilemap PhotoTilemap => photoTilemap;
    [SerializeField]
    private TilemapRenderer photoRenderer;
    public TilemapRenderer PhotoRenderer => photoRenderer;

    [Header("States")]
    [SerializeField, ReadOnly]
    private bool isSpawnMode;
    public bool IsSpawnMode
    {
        get => isSpawnMode;
        set
        {
            isSpawnMode = value;
            RefreshVisuals();
        }
    }

    private void RefreshVisuals()
    {
        copyCamera.PhotoModeGraphic.gameObject.SetActive(!isSpawnMode);
        copyCamera.SpawnModeGraphic.gameObject.SetActive(IsSpawnMode);
        PhotoRenderer.enabled = isSpawnMode;
    }

    private bool isPressed;
    private float holdTime;

    private Photo currentlySavedPhoto = new Photo();
    private bool isPhotoTaken = false;

    private void Awake()
    {
        currentlySavedPhoto.tiles = new TileBase[copyCamera.Settings.ViewTileSize.x * copyCamera.Settings.ViewTileSize.y];
        Clear();
    }

    public void Clear()
    {
        isPhotoTaken = false;
        IsSpawnMode = false;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(1))
            IsSpawnMode = isPhotoTaken && !isSpawnMode;

        if (Input.GetMouseButtonDown(0))
        {
            holdTime = 0;
            isPressed = true;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            isPressed = false;
        }

        if (isPressed)
        {
            holdTime += Time.deltaTime;
            if (holdTime > copyCamera.Settings.ActionHoldTime)
            {
                if (isSpawnMode)
                    SpawnPhoto();
                else
                    MakePhoto();
                holdTime = 0;
                isPressed = false;
            }
        }
    }


    private void MakePhoto()
    {
        foreach (var tilemap in tilemapsManager.Tilemaps)
        {
            MakePhoto(tilemap.Value);
        }
    }

    private void MakePhoto(Tilemap tilemap)
    {
        int xEnd = tilemap.origin.x + tilemap.size.x;
        int yEnd = tilemap.origin.y + tilemap.size.y;

        Vector2Int size = copyCamera.Settings.ViewTileSize;
        Vector2Int startVisiblePos = copyCamera.intPosition - copyCamera.Settings.ViewTileSize / 2;
        Vector2Int endVisiblePos = startVisiblePos + size;

        int index = 0;
        for (int j = startVisiblePos.y; j < endVisiblePos.y; j++)
        {
            for (int i = startVisiblePos.x; i < endVisiblePos.x; i++)
            {
                var tile = tilemap.GetTile(new Vector3Int(i, j, 0));
                currentlySavedPhoto.tiles[index] = tile;
                index++;
            }
        }
        var photoBounds = new BoundsInt(-size.x / 2, -size.y / 2, 0, size.x, size.y, 1);
        photoTilemap.SetTilesBlock(photoBounds, currentlySavedPhoto.tiles);
        isPhotoTaken = true;
        IsSpawnMode = true;
    }

    private void SpawnPhoto()
    {

    }

    private void SpawnPhoto(Tilemap tilemap)
    {
        #region dziwne
        /*
        var boundsOrigin = new Vector3Int(worldTilemap.origin.x, worldTilemap.origin.y, worldTilemap.size.z - 1);
        var boundsSize = new Vector3Int(worldTilemap.size.x, worldTilemap.size.y, 1);
        var bounds = new BoundsInt(boundsOrigin, boundsSize);
        var tiles = worldTilemap.GetTilesBlock(bounds);
        worldTilemap.InsertCells(worldTilemap.origin, worldTilemap.size.y, worldTilemap.size.x, 1);
        worldTilemap.SetTilesBlock(bounds, tiles);

        var photoTargetBounds = new BoundsInt(new Vector3Int(
            photoTilemap.cellBounds.x + copyCamera.intPosition.x, 
            photoTilemap.cellBounds.y + copyCamera.intPosition.y,
            worldTilemap.size.z - 1), new Vector3Int(
                photoTilemap.size.x,
                photoTilemap.size.y,
                1));

        worldTilemap.SetTilesBlock(
            photoTargetBounds,
            photoTilemap.GetTilesBlock(photoTilemap.cellBounds));
                        */
        #endregion

        var boundsPosition = new Vector3Int(
           -copyCamera.Settings.ViewTileSize.x / 2,
           -copyCamera.Settings.ViewTileSize.y / 2,
           0);

        var targetBoundsPosition = boundsPosition + (Vector3Int)copyCamera.intPosition;
        var photoBoundsSize = new Vector3Int(
               copyCamera.Settings.ViewTileSize.x,
               copyCamera.Settings.ViewTileSize.y,
               1);
        var photoTargetBounds = new BoundsInt(targetBoundsPosition, photoBoundsSize);

        for (int j = 0; j < photoBoundsSize.y; j++)
        {
            for (int i = 0; i < photoBoundsSize.x; i++)
            {
                var tilePos = new Vector3Int(
                    boundsPosition.x + i,
                    boundsPosition.y + j,
                    0);
                var tile = photoTilemap.GetTile(tilePos);
                if (tile != null)
                {
                    var targetPos = tilePos + (Vector3Int)copyCamera.intPosition;
                    tilemap.SetTile(targetPos, tile);
                }
            }
        }

        /*
        worldTilemap.SetTilesBlock(
            photoTargetBounds,
            photoTilemap.GetTilesBlock(new BoundsInt(boundsPosition, photoBoundsSize)));
        */

        /*
        var spawnedPhoto = Instantiate(photoTilemap, worldTilemap.layoutGrid.transform);
        spawnedPhoto.transform.position = photoTilemap.transform.position;
        spawnedPhoto.gameObject.AddComponent<TilemapCollider2D>();
        var renderer = spawnedPhoto.GetComponent<TilemapRenderer>();
        renderer.sortingLayerID = copyCamera.Settings.TerrainSortingLayerID;
        renderer.material = copyCamera.SpriteRenderer.material;
        */
    }
}
