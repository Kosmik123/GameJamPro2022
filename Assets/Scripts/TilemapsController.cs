using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapsController : MonoBehaviour
{
    [SerializeField]
    private Grid originalTilemapContainer;
    [SerializeField]
    private Grid spawnedTilemapContainer;

    private readonly Dictionary<TilemapLayer.Type, TilemapLayer> tilemapsByType = new Dictionary<TilemapLayer.Type, TilemapLayer>();
    public IReadOnlyDictionary<TilemapLayer.Type, TilemapLayer> Tilemaps => tilemapsByType;

    private void Start()
    {
        RecreateTilemaps();
    }

    public void RecreateTilemaps()
    {
        Clear();
        CreateCopyTilemaps();
    }

    [ContextMenu("Clear")]
    public void Clear()
    {
        var allPhotoTilemaps = spawnedTilemapContainer.GetComponentsInChildren<TilemapLayer>();
        foreach (var tilemapLayer in allPhotoTilemaps)
        {
            if (tilemapLayer.gameObject != null)
                Destroy(tilemapLayer.gameObject);
        }
        originalTilemapContainer.gameObject.SetActive(false);
    }

    private void CreateCopyTilemaps()
    {
        spawnedTilemapContainer.gameObject.SetActive(true);
        var allLevelTilemaps = originalTilemapContainer.GetComponentsInChildren<TilemapLayer>();
        tilemapsByType.Clear();
        foreach (var tilemap in allLevelTilemaps)
        {
            var spawned = Instantiate(tilemap, spawnedTilemapContainer.transform);
            tilemapsByType.Add(spawned.type, spawned);
            spawned.gameObject.SetActive(true);
        }
    }
}
