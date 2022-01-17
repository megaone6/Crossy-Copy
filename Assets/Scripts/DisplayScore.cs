using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DisplayScore : MonoBehaviour
{
    private TMPro.TextMeshProUGUI txt;
    // Start is called before the first frame update
    void Start()
    {
        txt = gameObject.GetComponent<TMPro.TextMeshProUGUI>();
    }

    public void UpdateScore(int newScore)
    {
        txt.text = "Score: " + newScore.ToString();
    }

    public void GameOver(int score)
    {
        txt.text = "Score: " + score.ToString() + "\nGame over! Back to menu in 5 seconds...";
        Invoke("BackToMenu", 0.1f);
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }
}
