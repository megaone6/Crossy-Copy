using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DisplayScore : MonoBehaviour
{
    [SerializeField] private TMPro.TextMeshProUGUI txt;

    public void UpdateScore(int newScore)
    {
        txt.text = "Score: " + newScore.ToString();
    }

    public void GameOver(int score)
    {
        txt.text = "Score: " + score.ToString() + "\nGame over! Back to menu in 5 seconds...";
        Invoke("BackToMenu", 5f);
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }
}
