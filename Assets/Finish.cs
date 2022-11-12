using NaughtyAttributes;
using UnityEngine;
using UnityEngine.SceneManagement;


public class Finish : MonoBehaviour
{
    [SerializeField, Tag]
    private string playerTag;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(playerTag))
        {
            var activeScene = SceneManager.GetActiveScene();
            SceneManager.LoadSceneAsync(activeScene.buildIndex + 1);
        }
    }

}
