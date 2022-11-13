using UnityEngine;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(Tilemap)), RequireComponent(typeof(TilemapRenderer))]
public class TilemapLayer : MonoBehaviour
{
    public enum Type
    {
        None = 0,
        Ground = 1, 
        Hole = 2,
        NotCopiable = 3,
        NotSpawnable = 4,
        Physical = 5
    }

    [SerializeField]
    private Type _type;
    public Type type => _type;

    private Tilemap tilemap;

    public Tilemap Tilemap
    {
        get
        {
            if (tilemap == null)
                tilemap = GetComponent<Tilemap>();
            return tilemap;
        }
    }

    private void Awake()
    {
        tilemap = GetComponent<Tilemap>();
    }

    private void Start()
    {
    }
}
