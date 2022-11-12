using UnityEngine;

[RequireComponent(typeof(CopyCamera))]
public class CopyCameraMovement : MonoBehaviour
{
    private CopyCamera copyCamera;
    private Camera mainCamera;
    private Vector2Int margin;

    private void Awake()
    {
        copyCamera = GetComponent<CopyCamera>(); 
        margin = new Vector2Int(
            (copyCamera.Settings.WorldTileSize.x - copyCamera.Settings.ViewTileSize.x) / 2,
            (copyCamera.Settings.WorldTileSize.y - copyCamera.Settings.ViewTileSize.y) / 2);
    }


    private void Start()
    {
        Cursor.visible = false;
        mainCamera = Camera.main;
    }

    private void Update()
    {
        var position = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        copyCamera.intPosition = new Vector2Int(
            Mathf.RoundToInt(position.x),
            Mathf.RoundToInt(position.y));
        copyCamera.intPosition = GetValidatedPosition(copyCamera.intPosition);
        transform.position = new Vector3(copyCamera.intPosition.x, copyCamera.intPosition.y, transform.position.z);
    }

    private Vector2Int GetValidatedPosition(Vector2Int position)
    {
        return new Vector2Int(
            Mathf.Clamp(position.x, -margin.x, margin.x),
            Mathf.Clamp(position.y, -margin.y, margin.y));
    }
}
