using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MainMenu : MonoBehaviour
{
    public int coins;
    public SaveCoinObject coinObject;
    [SerializeField] private TextMeshProUGUI coinText;
    private void Start()
    {
        gameObject.SetActive(true);
        if (File.Exists(Application.persistentDataPath + "/save.json"))
        {
            string saveString = File.ReadAllText(Application.persistentDataPath + "/save.json");
            coinObject = JsonUtility.FromJson<SaveCoinObject>(saveString);
        }
        else
        {
            coinObject = new SaveCoinObject
            {
                coinAmount = 0
            };
        }
        coins = coinObject.coinAmount;
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
    public class SaveCoinObject
    {
        public int coinAmount;
    }
}
