using UnityEngine;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(Tilemap))]
public class TilemapLayer : MonoBehaviour
{
    public enum Type
    {
        Ground, 
        Hole,
        NotSpawnable,
        NotCopiable,
        Physical
    }

    [SerializeField]
    private Type _type;
    public Type type => _type;

    private Tilemap tilemap;
    public Tilemap Tilemap => tilemap;

    private void Awake()
    {
        tilemap = GetComponent<Tilemap>();
    }


}
