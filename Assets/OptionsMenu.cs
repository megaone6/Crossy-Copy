using UnityEngine;
using TMPro;
using System.IO;

public class OptionsMenu : MonoBehaviour
{
    [SerializeField] private TMP_Dropdown graphicsDropDown;
    private SaveGraphicsObject savedQuality;
    private void Start()
    {
        gameObject.SetActive(false);
        if (File.Exists(Application.persistentDataPath + "/settings.json"))
        {
            string settingsString = File.ReadAllText(Application.persistentDataPath + "/settings.json");
            graphicsDropDown.value = JsonUtility.FromJson<SaveGraphicsObject>(settingsString).qualitySetting;
        }
        else
            graphicsDropDown.value = 2;
    }

    public void SetQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
        savedQuality = new SaveGraphicsObject
        {
            qualitySetting = qualityIndex
        };
        string json = JsonUtility.ToJson(savedQuality);
        File.WriteAllText(Application.persistentDataPath + "/settings.json", json);
    }

    private class SaveGraphicsObject
    {
        public int qualitySetting;
    }
}
