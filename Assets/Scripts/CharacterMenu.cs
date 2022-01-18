using UnityEngine;
using TMPro;
using System;
using System.IO;
using System.Collections.Generic;

public class CharacterMenu : MonoBehaviour
{
    [SerializeField] private TMP_Dropdown charactersDropDown;
    [SerializeField] private MainMenu mainMenu;
    [SerializeField] private TextMeshProUGUI coinText;
    [SerializeField] private Popup notEnoughMoney;
    [SerializeField] private Canvas canvas;
    private SelectedCharacter savedCharacter;
    private UnlockedCharacters unlockedCharacters;
    private string json;

    private void Start()
    {
        Invoke("StartupSetup", 0.01f);
    }

    public void SetCharacter(int idIndex)
    {
        String tmpName = charactersDropDown.options[charactersDropDown.value].text;
        int removeFrom = tmpName.IndexOf("(");
        int priceLength;
        if (idIndex == 0)
            priceLength = 1;
        else
            priceLength = 2;
        int tmpCost = int.Parse(tmpName.Substring(removeFrom + 1, priceLength));
        if (tmpCost > mainMenu.coinObject.coinAmount && unlockedCharacters.unlocked[idIndex] == 0)
        {
            charactersDropDown.value = 0;
            notEnoughMoney.gameObject.SetActive(true);
            gameObject.SetActive(false);
            return;
        }
        savedCharacter = new SelectedCharacter
        {
            id = idIndex,
            name = tmpName.Substring(0, removeFrom - 1),
            cost = tmpCost,
        };
        json = JsonUtility.ToJson(savedCharacter);
        File.WriteAllText(Application.persistentDataPath + "/selectedCharacter.json", json);
        if (unlockedCharacters.unlocked[idIndex] == 0)
        {
            unlockedCharacters.unlocked[idIndex] = 1;
            json = JsonUtility.ToJson(unlockedCharacters);
            File.WriteAllText(Application.persistentDataPath + "/unlockedCharacters.json", json);
            int newAmount = mainMenu.coins - tmpCost;
            mainMenu.coins = newAmount;
            mainMenu.coinObject.coinAmount = newAmount;
            UpdateText();
            json = JsonUtility.ToJson(mainMenu.coinObject);
            File.WriteAllText(Application.persistentDataPath + "/save.json", json);
        }
    }

    private void StartupSetup()
    {
        gameObject.SetActive(false);
        if (!File.Exists(Application.persistentDataPath + "/unlockedCharacters.json"))
        {
            unlockedCharacters = new UnlockedCharacters { unlocked = new List<int> { 1, 0, 0, 0 } };
            json = JsonUtility.ToJson(unlockedCharacters);
            File.WriteAllText(Application.persistentDataPath + "/unlockedCharacters.json", json);
        }
        else
        {
            string unlockedCharactersString = File.ReadAllText(Application.persistentDataPath + "/unlockedCharacters.json");
            unlockedCharacters = JsonUtility.FromJson<UnlockedCharacters>(unlockedCharactersString);
        }
        if (File.Exists(Application.persistentDataPath + "/selectedCharacter.json"))
        {
            string selectedCharString = File.ReadAllText(Application.persistentDataPath + "/selectedCharacter.json");
            charactersDropDown.value = JsonUtility.FromJson<SelectedCharacter>(selectedCharString).id;
        }
        else
        {
            charactersDropDown.value = 0;
            SetCharacter(0);
        }
    }

    private void UpdateText()
    {
        coinText.text = "Coins: " + mainMenu.coinObject.coinAmount.ToString();
    }

    private class SelectedCharacter
    {
        public int id;
        public String name;
        public int cost;
    }

    private class UnlockedCharacters
    {
        public List<int> unlocked;
    }
}
