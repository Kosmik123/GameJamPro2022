using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PhotosSpawner : MonoBehaviour
{
    [SerializeField]
    private Transform container;

    [ContextMenu("Clear")]
    public void Clear()
    {
        var allPhotoTilemaps = container.GetComponentsInChildren<Tilemap>();
        foreach(var tilemap in allPhotoTilemaps)
        {
            if (tilemap.gameObject != null)
                Destroy(tilemap.gameObject);
        }
    }




}
