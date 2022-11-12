using UnityEngine;
using NaughtyAttributes;
using UnityEngine.Tilemaps;
using System.Collections.Generic;
using System;

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


    [SerializeField]
    private Grid tilemapContainer;
    public Grid TilemapsContainer => tilemapContainer;

    [Header("States")]
    [ReadOnly]
    public Vector2Int intPosition;

    public readonly Dictionary<TilemapLayer.Type, TilemapLayer> tilemapLayers = new Dictionary<TilemapLayer.Type, TilemapLayer>(4);

    private void Awake()
    {
        var tilemapLayerComponents = tilemapContainer.GetComponentsInChildren<TilemapLayer>();
        foreach (var tilemapLayer in tilemapLayerComponents)
        {
            tilemapLayers[tilemapLayer.type] = tilemapLayer;
        }
    }

    private void Start()
    {
        foreach (var tilemapLayer in tilemapLayers)
        {

        }
        RefreshVisualsSizes();
    }

    private int TilemapRenderer(out object var)
    {
        throw new NotImplementedException();
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
