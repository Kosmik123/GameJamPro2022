using UnityEngine;
using NaughtyAttributes;

public class CopyCamera : MonoBehaviour
{
    [SerializeField]
    private CopyCameraSettings settings;

    [SerializeField]
    private SpriteRenderer spriteRenderer;

    [SerializeField, ReadOnly]
    private Vector2Int intPosition;

    private Camera mainCamera;

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
