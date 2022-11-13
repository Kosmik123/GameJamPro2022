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
            FinishLevel();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    { 
        if (collision.CompareTag(playerTag))
        {
            FinishLevel();
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
            FinishLevel();
    }

    private static void FinishLevel()
    {
        var activeSceneIndex = SceneManager.GetActiveScene().buildIndex;
        if (activeSceneIndex < SceneManager.sceneCountInBuildSettings)
            SceneManager.LoadSceneAsync(activeSceneIndex + 1);
    }

}
