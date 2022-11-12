using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeath : MonoBehaviour
{
    public event Action OnDied;
    
    [SerializeField]
    private Transform playerTransform;
    [SerializeField]
    private GameObject visuals;
    [SerializeField]
    private float yDeath;
    [SerializeField]
    private TrailRenderer trailRenderer;

    private Vector3 startingPosition;
    private bool isDead;

    private void Start()
    {
        startingPosition = playerTransform.position;
        isDead = false;
    }

    private void Update()
    {
        if (isDead == false && playerTransform.position.y < yDeath)
        {
            isDead = true;
            trailRenderer.time = 0;
            OnDied?.Invoke();
        }
    }

    public void Respawn()
    {
        playerTransform.position = startingPosition;
        isDead = false;
        StartCoroutine(EnableTrailCo());
    }

    private IEnumerator EnableTrailCo()
    {
        yield return null;
        trailRenderer.time = 1;
        trailRenderer.Clear();
    }

}
