using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CopyCamera : MonoBehaviour
{




    [SerializeField]
    private Vector2Int intPosition;

    private Camera mainCamera;

    private void Start()
    {
        mainCamera = Camera.main;
    }

    private void Update()
    {
        var position = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        intPosition = new Vector2Int(
            Mathf.RoundToInt(position.x),
            Mathf.RoundToInt(position.y));

        transform.position = new Vector3(intPosition.x, intPosition.y, transform.position.z);
    }
}
