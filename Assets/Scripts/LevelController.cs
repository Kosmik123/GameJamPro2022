using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelController : MonoBehaviour
{
    [SerializeField]
    private TilemapsController tilemapsController;
    [SerializeField]
    private PlayerDeath playerDeath;
    
    private void OnEnable()
    {
        playerDeath.OnDied += RestartLevel;
    }

    public void RestartLevel()
    {
        tilemapsController.RecreateTilemaps();
        playerDeath.Respawn();
        Debug.Log("Reset Level");
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            RestartLevel();
        }
    }

    private void OnDisable()
    {
        playerDeath.OnDied -= RestartLevel;
    }

}
