using UnityEngine;
using UnityEngine.SceneManagement; 
using UnityEngine.UI; 

public class House : MonoBehaviour
{
    public GameObject winPanel; 

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) 
        {
            Debug.Log("Персонаж в домике!");
            ShowWinScreen();
        }
    }

    private void ShowWinScreen()
    {
        if (winPanel != null)
        {
            winPanel.SetActive(true);
            Time.timeScale = 0f;
        }
        else
        {
            Debug.LogWarning("WinPanel не назначена в инспекторе!");
        }
    }
}
