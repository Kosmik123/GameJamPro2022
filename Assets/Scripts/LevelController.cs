using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class LevelController : MonoBehaviour
{
    [SerializeField]
    private TilemapsManager tilemapsController;
    [SerializeField]
    private PlayerDeath playerDeath;
    [SerializeField]
    private CopyCameraPhotosController copyCameraController;

    private void OnEnable()
    {
        playerDeath.OnDied += RestartLevel;
    }

    public void RestartLevel()
    {
        tilemapsController.RecreateTilemaps();
        playerDeath.Respawn();
        copyCameraController.Clear();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            RestartLevel();
        }
        Tile tile;
    }

    private void OnDisable()
    {
        playerDeath.OnDied -= RestartLevel;
    }
}
