using UnityEngine;
using NaughtyAttributes;
using UnityEngine.Tilemaps;
using UnityEditor;
using System;


public class CopyCameraPhotosController : MonoBehaviour
{
    [Header("To Link")]
    [SerializeField]
    private TilemapsManager tilemapsManager;

    [Header("Elements")]
    [SerializeField]
    private CopyCamera copyCamera;

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

    private TileBase[] cachedTiles;

    private void RefreshVisuals()
    {
        copyCamera.PhotoModeGraphic.gameObject.SetActive(!isSpawnMode);
        copyCamera.SpawnModeGraphic.gameObject.SetActive(IsSpawnMode);
        copyCamera.TilemapsContainer.gameObject.SetActive(isSpawnMode);
    }

    private bool isPressed;
    private float holdTime;

    private bool isPhotoTaken = false;
    private Photo currentlySavedPhoto;

    private void Awake()
    {
        currentlySavedPhoto = new Photo(copyCamera.Settings.ViewTileSize.x, copyCamera.Settings.ViewTileSize.y);
        cachedTiles = new TileBase[copyCamera.Settings.ViewTileSize.x * copyCamera.Settings.ViewTileSize.y];
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
        Vector2Int size = copyCamera.Settings.ViewTileSize;
        Vector2Int startVisiblePos = copyCamera.intPosition - copyCamera.Settings.ViewTileSize / 2;

        foreach (var tilemap in tilemapsManager.TilemapLayers)
        {
            if (tilemap.Key != TilemapLayer.Type.NotCopiable)
                MakePhoto(tilemap.Value, startVisiblePos, size);
        }
    }

    private void MakePhoto(TilemapLayer tilemapLayer, Vector2Int startVisiblePos, Vector2Int size)
    {
        var type = tilemapLayer.type;
        var tilemap = tilemapLayer.Tilemap;
        Vector2Int endVisiblePos = startVisiblePos + size;

        int index = 0;
        for (int j = startVisiblePos.y; j < endVisiblePos.y; j++)
        {
            for (int i = startVisiblePos.x; i < endVisiblePos.x; i++)
            {
                var tile = tilemap.GetTile(new Vector3Int(i, j, 0));
                cachedTiles[index] = tile;
                index++;
            }
        }

        currentlySavedPhoto.SetTiles(type, cachedTiles);

        var photoBounds = new BoundsInt(-size.x / 2, -size.y / 2, 0, size.x, size.y, 1);
        copyCamera.tilemapLayers[type].Tilemap.SetTilesBlock(photoBounds, currentlySavedPhoto.tilesByType[type]);
        isPhotoTaken = true;
        IsSpawnMode = true;
    }

    private void SpawnPhoto()
    {
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
        foreach (var tilesArray in currentlySavedPhoto.tilesByType)
        {
            var type = tilesArray.Key;

            SpawnPhoto(tilesArray.Value, tilemapsManager.TilemapLayers[type].Tilemap, boundsPosition, photoBoundsSize, targetBoundsPosition, photoTargetBounds);
        }
    }

    private void SpawnPhoto(TileBase[] tiles, Tilemap targetTilemap, Vector3Int boundsPosition, Vector3Int photoBoundsSize, Vector3Int targetBoundsPosition, BoundsInt photoTargetBounds)
    {

        //targetTilemap.SetTilesBlock(photoTargetBounds, tiles);  // to kopiuje też null tile, więc nie
        int index = 0;
        for (int j = 0; j < photoBoundsSize.y; j++)
        {
            for (int i = 0; i < photoBoundsSize.x; i++)
            {
                var tilePos = new Vector3Int(
                    boundsPosition.x + i,
                    boundsPosition.y + j,
                    0);

                var tile = tiles[index];
                if (tile != null)
                {
                    var targetPos = tilePos + (Vector3Int)copyCamera.intPosition;
                    targetTilemap.SetTile(targetPos, tile);
                }
                index++;
            }
        }

    }
}
