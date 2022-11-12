using NaughtyAttributes;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Finish : MonoBehaviour
{
    [SerializeField, Tag]
    private string playerTag;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag(playerTag))
        {
            var activeSceneIndex = SceneManager.GetActiveScene().buildIndex;
            if (activeSceneIndex < SceneManager.sceneCountInBuildSettings)
                SceneManager.LoadSceneAsync(activeSceneIndex + 1);
        }
    }
}
