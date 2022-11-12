using UnityEngine;
using NaughtyAttributes;
using UnityEngine.Tilemaps;

public class CopyCameraPhotosController : MonoBehaviour
{
    [SerializeField]
    private CopyCamera copyCamera;

    [Space, SerializeField]
    private Tilemap photoTilemap;
    public Tilemap PhotoTilemap => photoTilemap;
    
    [SerializeField]
    private TilemapRenderer photoRenderer;
    public TilemapRenderer PhotoRenderer => photoRenderer;

    [SerializeField, ReadOnly]
    private bool isSpawnMode;
    public bool IsSpawnMode
    {
        get => isSpawnMode;
        set
        {
            isSpawnMode = value;
            copyCamera.SpriteRenderer.color = isSpawnMode ? Color.black : Color.red;
            PhotoRenderer.enabled = isSpawnMode;
        }
    }

    private void Awake()
    {
        currentlySavedPhoto.tiles = new TileBase[copyCamera.Settings.ViewTileSize.x * copyCamera.Settings.ViewTileSize.y];
    }

    private bool isPressed;
    private float holdTime;

    private Photo currentlySavedPhoto;

    private void Update()
    {

        if (Input.GetMouseButtonDown(1))
            IsSpawnMode = !isSpawnMode;

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
                var tilemap = GameObject.FindGameObjectWithTag("Level Tiles").GetComponentInChildren<Tilemap>();
                if (isSpawnMode)
                    SpawnPhoto(tilemap);
                else
                    MakePhoto(tilemap);
                holdTime = 0;
                isPressed = false;
            }
        }
    }

    private void MakePhoto(Tilemap worldTilemap)
    {
        int xEnd = worldTilemap.origin.x + worldTilemap.size.x;
        int yEnd = worldTilemap.origin.y + worldTilemap.size.y;

        Vector2Int size = copyCamera.Settings.ViewTileSize;
        Vector2Int startVisiblePos = copyCamera.intPosition - copyCamera.Settings.ViewTileSize / 2;
        Vector2Int endVisiblePos = startVisiblePos + size;

        int index = 0;
        for (int j = startVisiblePos.y; j < endVisiblePos.y; j++)
        {
            for (int i = startVisiblePos.x; i < endVisiblePos.x; i++)
            {
                var tile = worldTilemap.GetTile(new Vector3Int(i, j, 0));
                currentlySavedPhoto.tiles[index] = tile;
                index++;
            }
        }
        var photoBounds = new BoundsInt(-size.x / 2, -size.y / 2, 0, size.x, size.y, 1);
        photoTilemap.SetTilesBlock(photoBounds, currentlySavedPhoto.tiles);
        IsSpawnMode = true;
    }

    private void SpawnPhoto(Tilemap worldTilemap)
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

        /*
        for (int j = 0; j < boundsSize.y; j++)
        {
            for (int i = 0; i < boundsSize.x; i++)
            {
                var tilePos = new Vector3Int(
                    boundsPosition.x + i,
                    boundsPosition.y + j,
                    0);
                var tile = photoTilemap.GetTile(tilePos);
                if (tile != null)
                {
                    var targetPos = tilePos + (Vector3Int)copyCamera.intPosition;
                    worldTilemap.SetTile(targetPos, tile);
                }
            }
        }
        */

        
        worldTilemap.SetTilesBlock(
            photoTargetBounds,
            photoTilemap.GetTilesBlock(new BoundsInt(boundsPosition, photoBoundsSize)));
        

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
