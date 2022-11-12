using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeath : MonoBehaviour
{
    [SerializeField]
    private LevelController levelController;
    [SerializeField]
    private Transform playerTransform;

    [SerializeField]
    private float yDeath;

    private Vector3 startingPosition;

    private void Start()
    {
        startingPosition = playerTransform.position;
    }

    private void Update()
    {
        if (playerTransform.position.y < yDeath)
        {
            playerTransform.position = startingPosition;
            levelController.ResetLevel();
        }
    }
}
