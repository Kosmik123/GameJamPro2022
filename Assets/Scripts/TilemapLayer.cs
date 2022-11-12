using UnityEngine;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(Tilemap))]
public class TilemapLayer : MonoBehaviour
{
    public enum Type
    {
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
    public Tilemap Tilemap => tilemap;

    private void Awake()
    {
        tilemap = GetComponent<Tilemap>();
    }


}
