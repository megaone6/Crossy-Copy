using UnityEngine;

public class DisplayScore : MonoBehaviour
{
    TMPro.TextMeshProUGUI txt;
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
        txt.text = "Score: " + score.ToString() + "\nGame over!";
    }
}
