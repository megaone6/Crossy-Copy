using UnityEngine;
using TMPro;
using System;
using System.IO;
using Newtonsoft.Json;
using System.Collections.Generic;

public class CharacterMenu : MonoBehaviour
{
    [SerializeField] private TMP_Dropdown charactersDropDown;
    private SelectedCharacter savedCharacter;

    private void Start()
    {
        gameObject.SetActive(false);
        if (File.Exists(Application.persistentDataPath + "/selectedCharacter.json"))
        {
            string selectedCharString = File.ReadAllText(Application.persistentDataPath + "/selectedCharacter.json");
            charactersDropDown.value = JsonUtility.FromJson<SelectedCharacter>(selectedCharString).id;
        }
        else
            charactersDropDown.value = 0;
        if (!File.Exists(Application.persistentDataPath + "/characters.json"))
        {
            
        }
    }

    public void SetCharacter(int idIndex)
    {
        String tmpName = charactersDropDown.options[charactersDropDown.value].text;
        int removeFrom = tmpName.IndexOf("(");
        savedCharacter = new SelectedCharacter
        {
            id = idIndex,
            name = tmpName.Substring(0, removeFrom - 1),
            //cost = int.Parse(tmpName.Substring(removeFrom + 1, 2)),
        };
        string json = JsonUtility.ToJson(savedCharacter);
        File.WriteAllText(Application.persistentDataPath + "/selectedCharacter.json", json);
    }

    private class SelectedCharacter
    {
        public int id;
        public String name;
        public int cost;
    }

    private class Character
    {
        public int id;
        public String name;
        //public int cost;
        //public int unlocked;
    }
}
