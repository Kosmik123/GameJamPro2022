using UnityEditor;
using UnityEngine;

public class MainAreaSizeSetter : MonoBehaviour
{
    [SerializeField]
    private CopyCameraSettings settings;
    [SerializeField]
    private CameraMainArea cameraMainArea;

    private void Start()
    {
        Refresh();
    }


    [ContextMenu("Refresh")]
    private void RefreshEditor()
    {
        Refresh();
        EditorUtility.SetDirty(this);
    }


    private void Refresh()
    {
        cameraMainArea.Size = settings.WorldTileSize;
    }
}
