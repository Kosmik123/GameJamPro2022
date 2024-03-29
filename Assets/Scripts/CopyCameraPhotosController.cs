﻿using UnityEngine;
using NaughtyAttributes;
using UnityEngine.Tilemaps;
using UnityEditor;
using System;
using UnityEngine.Events;

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

    public UnityEvent OnPhotoMade = new UnityEvent();
    public UnityEvent OnPhotoSpawned = new UnityEvent();

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
        if (copyCamera == null)
            copyCamera = FindObjectOfType<CopyCamera>();
        currentlySavedPhoto = new Photo(copyCamera.Settings.ViewTileSize.x, copyCamera.Settings.ViewTileSize.y);
        Clear();
    }

    public void Clear()
    {
        Debug.LogWarning("Clearing photo");
        currentlySavedPhoto.Clear();
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
                holdTime = 0;
                isPressed = false;
                if (isSpawnMode)
                    SpawnPhoto();
                else
                    MakePhoto();
            }
        }
    }

    private void MakePhoto()
    {
        Clear();
        var tilemapLayers = tilemapsManager.TilemapLayers;
        Vector2Int size = copyCamera.Settings.ViewTileSize;
        Vector2Int startVisiblePos = copyCamera.intPosition - copyCamera.Settings.ViewTileSize / 2;
        Vector2Int endVisiblePos = startVisiblePos + size;

        int index = 0;
        for (int j = 0; j < size.y; j++)
        {
            for (int i = 0; i < size.x; i++)
            {
                foreach (var typeToLayerMapping in tilemapLayers)
                {
                    var tilemap = typeToLayerMapping.Value.Tilemap;
                    var pos = new Vector3Int(i + startVisiblePos.x, j + startVisiblePos.y, 0);
                    if (tilemap == null)
                    {
                        Debug.LogError("Tilemapa jest null");
                    }    
                    var tile = tilemap.GetTile(pos);
                    if (tile != null)
                    {
                        currentlySavedPhoto.SetTile(typeToLayerMapping.Key, new Vector2Int(i, j), tile);
                        break;
                    }
                }
                index++;
            }
        }

        Debug.LogWarning("Idzie ok bez nullów");


        var photoBounds = new BoundsInt(-size.x / 2, -size.y / 2, 0, size.x, size.y, 1);
        foreach (var typeToLayerMapping in tilemapLayers)
        {
            Debug.LogWarning("Bierzemy kolejny layer");
            var type = typeToLayerMapping.Key;
            Debug.LogWarning($"Typ layera to {type}");
            if (type != TilemapLayer.Type.NotCopiable)
            {
                if (copyCamera == null)
                    Debug.LogWarning($"Copy camera to null");
                Debug.LogWarning("Copy camera jest");

                if (copyCamera.tilemapLayers == null)
                    Debug.LogWarning($"Layersy copy camery to null");
                Debug.LogWarning("Słownik typów na wartwy tilemap jest");

                if (copyCamera.tilemapLayers.TryGetValue(type, out var layer))
                {
                    if (layer == null)
                        Debug.LogError($"Layer typu: {type} jest null!");
                    Debug.LogWarning($"Warstwa dla typu {type} istnieje");

                    if (currentlySavedPhoto.tilesByType.TryGetValue(type, out var tiles))
                    {
                        Debug.LogWarning($"Tilemapa dla tej warstwy to: {layer.Tilemap}");
                        layer.Tilemap.SetTilesBlock(photoBounds, tiles);
                    }
                }
                else
                {
                    Debug.Log(type + " jest zły");
                }
            }
            else
            {
                Debug.LogWarning("Typ niekopiowalny");
            }
        }

        isPhotoTaken = true;
        IsSpawnMode = true;

        //foreach (var tilemap in tilemapsManager.TilemapLayers)
        //{
        //    if (tilemap.Key != TilemapLayer.Type.NotCopiable)
        //        MakePhoto(tilemap.Value, startVisiblePos, size);
        //}
        OnPhotoMade.Invoke();
        Debug.LogWarning("Taking photo successful");
    }

    //private void MakePhoto(TilemapLayer tilemapLayer, Vector2Int startVisiblePos, Vector2Int size)
    //{
    //    var type = tilemapLayer.type;
    //    var tilemap = tilemapLayer.Tilemap;

    //    int index = 0;
    //    for (int j = startVisiblePos.y; j < endVisiblePos.y; j++)
    //    {
    //        for (int i = startVisiblePos.x; i < endVisiblePos.x; i++)
    //        {
    //            var tile = tilemap.GetTile(new Vector3Int(i, j, 0));
    //            cachedTiles[index] = tile;
    //            index++;
    //        }
    //    }

    //    currentlySavedPhoto.SetTiles(type, cachedTiles);

    //    var photoBounds = new BoundsInt(-size.x / 2, -size.y / 2, 0, size.x, size.y, 1);
    //    copyCamera.tilemapLayers[type].Tilemap.SetTilesBlock(photoBounds, currentlySavedPhoto.tilesByType[type]);

    //}

    private void SpawnPhoto()
    {
        var worldTilemapLayers = tilemapsManager.TilemapLayers;

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

        int index = 0;
        for (int j = 0; j < photoBoundsSize.y; j++)
        {
            for (int i = 0; i < photoBoundsSize.x; i++)
            {
                bool wasNotNull = false;
                TileBase theTile = null;
                TilemapLayer.Type typeOnPhoto = TilemapLayer.Type.None;          
                foreach (var typeToArrayMapping in currentlySavedPhoto.tilesByType)
                {
                    var type = typeToArrayMapping.Key;
                    var tile = typeToArrayMapping.Value[index];
                    if (tile != null)
                    {
                        wasNotNull = true;
                        theTile = tile;
                        typeOnPhoto = type;
                        break;
                    }
                }

                if (wasNotNull && typeOnPhoto != TilemapLayer.Type.NotCopiable)
                {
                    var pos = new Vector3Int(i, j, 0) + targetBoundsPosition;
                    if (worldTilemapLayers.TryGetValue(TilemapLayer.Type.NotSpawnable, out var layerInWorld)
                        && layerInWorld.Tilemap.GetTile(pos) != null)
                    { 

                    }
                    else
                    { 
                        foreach (var layer in worldTilemapLayers)
                        {
                            if (layer.Key == typeOnPhoto)
                                layer.Value.Tilemap.SetTile(pos, theTile);
                            else
                                layer.Value.Tilemap.SetTile(pos, null);
                        } 
                    }
                }
                index++;
            }
        }


        //foreach (var tilesArray in currentlySavedPhoto.tilesByType)
        // {
        //     var type = tilesArray.Key;
        //     SpawnPhoto(tilesArray.Value, worldTilemapLayers[type].Tilemap, boundsPosition, photoBoundsSize, targetBoundsPosition, photoTargetBounds);
        // }

        OnPhotoSpawned.Invoke();
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
