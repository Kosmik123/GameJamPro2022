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

    [ContextMenu("Clear")]
    public void Clear()
    {
        var allPhotoTilemaps = spawnedTilemapContainer.GetComponentsInChildren<Tilemap>();
        foreach(var tilemap in allPhotoTilemaps)
        {
            if (tilemap.gameObject != null)
                Destroy(tilemap.gameObject);
        }
        originalTilemapContainer.gameObject.SetActive(false);
    }

    private void Start()
    {
        RecreateTilemaps();
    }

    private void RecreateTilemaps()
    {
        Clear();
        CreateCopyTilemaps();
    }

    private void CreateCopyTilemaps()
    {
        spawnedTilemapContainer.gameObject.SetActive(true);
        var allLevelTilemaps = originalTilemapContainer.GetComponentsInChildren<Tilemap>();
        foreach (var tilemap in allLevelTilemaps)
        {
            var spawned = Instantiate(tilemap, spawnedTilemapContainer.transform);
            spawned.gameObject.SetActive(true);
        }
    }






}
