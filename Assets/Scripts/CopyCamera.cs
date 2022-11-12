using UnityEngine;
using NaughtyAttributes;
using UnityEngine.Tilemaps;
using System;

public class CopyCamera : MonoBehaviour
{
    [Header("To Link")]
    [SerializeField]
    private CopyCameraSettings settings;
    [SerializeField]
    private SpriteRenderer spriteRenderer;

    [Header("States")]
    [SerializeField, ReadOnly]
    private Vector2Int intPosition;
    
    private Camera mainCamera;
    
    private bool isPressed;
    private float holdTime;

    private void Start()
    {
        Cursor.visible = false;
        mainCamera = Camera.main;
        spriteRenderer.size = settings.ViewTileSize;
    }

    private void Update()
    {
        var position = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        intPosition = new Vector2Int(
            Mathf.RoundToInt(position.x),
            Mathf.RoundToInt(position.y));
        intPosition = ValidatePosition(intPosition);
        transform.position = new Vector3(intPosition.x, intPosition.y, transform.position.z);

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
            if (holdTime >= settings.ActionHoldTime)
            {
                DoAction();
            }
        }
    }

    private void DoAction()
    {
        Debug.Log("TAKE PICTURE!");
        isPressed = false;
    }

    private Vector2Int ValidatePosition(Vector2Int position)
    {
        int halfHorizontalMargin = (settings.WorldTileSize.x - settings.ViewTileSize.x) / 2;
        int halfVerticalMargin = (settings.WorldTileSize.y - settings.ViewTileSize.y) / 2;

        return new Vector2Int(
            Mathf.Clamp(position.x, - halfHorizontalMargin, halfHorizontalMargin),
            Mathf.Clamp(position.y, -halfVerticalMargin, halfVerticalMargin));
    }

    



}
