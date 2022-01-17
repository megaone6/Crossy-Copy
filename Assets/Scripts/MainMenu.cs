using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MainMenu : MonoBehaviour
{
    private int coins;
    private SaveCoinObject loadObject;
    [SerializeField] private TextMeshProUGUI coinText;
    private void Start()
    {
        gameObject.SetActive(true);
        if (File.Exists(Application.persistentDataPath + "/save.json"))
        {
            string saveString = File.ReadAllText(Application.persistentDataPath + "/save.json");
            loadObject = JsonUtility.FromJson<SaveCoinObject>(saveString);
        }
        else
        {
            loadObject = new SaveCoinObject
            {
                coinAmount = 0
            };
        }
        coins = loadObject.coinAmount;
        coinText.text = "Coins: " + coins;
    }

    // Start is called before the first frame update
    public void PlayGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    private class SaveCoinObject
    {
        public int coinAmount;
    }
}
