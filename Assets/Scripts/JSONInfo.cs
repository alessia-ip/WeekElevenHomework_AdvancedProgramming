using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;

[System.Serializable]
public class Character
{
    public enum characterClass
    {
        mage,
        rogue,
        bard,
        warrior,
        paladin
    }
    
    public string name;
    public characterClass cClass;
    public int health;
    
    
    
    public static Character CreateFromJSON(string path)
    {
        string jsonString = File.ReadAllText(Application.streamingAssetsPath + path);
        return JsonUtility.FromJson<Character>(jsonString); //try and convert from jsonstring to player info struct
    }

    public static void WriteToJson(string path, Character characterInfo)
    {
        string jsonString = JsonUtility.ToJson(characterInfo); //transfer to string, ready to write
        File.WriteAllText(Application.streamingAssetsPath + path, jsonString);
    }

}

[System.Serializable]
public class AllCharacters
{
    public List<Character> allCharacters = new List<Character>();
}

public class JSONInfo : MonoBehaviour
{
    [Header("UI Elements Referenced")]
    public InputField nameInput;
    public Slider healthInput;
    public Dropdown classInput;
    public Toggle overwriteToggle;
    public GameObject deleteButton;
    
    //public List<Character> charactersList = new List<Character>();
    
    public AllCharacters allCharactersList = new AllCharacters();

    private int charInt;
    private bool canOverwrite = false;
    
    void Awake()
    {
        LoadAllCharactersFromJSON("/JSON/PlayerInfo.json");
    }
    
    public void SaveCharacter()
    {

        //if we can't overwrite a character OR don't want to, we want to make a new character in the list
        if (!canOverwrite || !overwriteToggle.isOn) 
        {
            Character newCharacter = new Character();
            newCharacter.name = nameInput.text;
        
            var newCharClass = Character.characterClass.bard;

            switch (classInput.value)
            { 
                case 0:
                    newCharClass = Character.characterClass.mage;
                    break;
                case 1:
                    newCharClass = Character.characterClass.rogue;
                    break;
                case 2:
                    newCharClass = Character.characterClass.bard;
                    break;
                case 3:
                    newCharClass = Character.characterClass.warrior;
                    break;
                case 4:
                    newCharClass = Character.characterClass.paladin;
                    break;
            }
        
            newCharacter.cClass = newCharClass;
            newCharacter.health = (int)healthInput.value;
            
            allCharactersList.allCharacters.Add(newCharacter);
        }
        else //overwrite version: if we can overwrite AND the toggle is checked
        {
            allCharactersList.allCharacters[charInt].name = nameInput.text;
            allCharactersList.allCharacters[charInt].health = (int) healthInput.value;
            
            var newCharClass = Character.characterClass.bard;

            switch (classInput.value)
            { 
                case 0:
                    newCharClass = Character.characterClass.mage;
                    break;
                case 1:
                    newCharClass = Character.characterClass.rogue;
                    break;
                case 2:
                    newCharClass = Character.characterClass.bard;
                    break;
                case 3:
                    newCharClass = Character.characterClass.warrior;
                    break;
                case 4:
                    newCharClass = Character.characterClass.paladin;
                    break;
            }

            allCharactersList.allCharacters[charInt].cClass = newCharClass;
        }

        canOverwrite = false;
        overwriteToggle.isOn = false;
        overwriteToggle.gameObject.SetActive(false);
        WriteCharactersToJSON("/JSON/PlayerInfo.json");
        
    }
    
    
    public void WriteCharactersToJSON(string path){
        string jsonString = JsonUtility.ToJson(allCharactersList); //transfer to string, ready to write
        File.WriteAllText(Application.streamingAssetsPath + path, jsonString);
    }

    public void LoadAllCharactersFromJSON(string path)
    {
        string jsonString = File.ReadAllText(Application.streamingAssetsPath + path);
        allCharactersList = JsonUtility.FromJson<AllCharacters>(jsonString); 
    }

    public void LoadRandomCharacter()
    {
        if (allCharactersList.allCharacters.Count == 0) return;
        
        charInt = Random.Range(0, allCharactersList.allCharacters.Count - 1);

        nameInput.text = allCharactersList.allCharacters[charInt].name;
        healthInput.value = allCharactersList.allCharacters[charInt].health;

        switch (allCharactersList.allCharacters[charInt].cClass)
        {
            case Character.characterClass.mage:
                classInput.value = 0;
                break;
            case Character.characterClass.rogue:
                classInput.value = 1;
                break;
            case Character.characterClass.bard:
                classInput.value = 2;
                break;
            case Character.characterClass.warrior:
                classInput.value = 3;
                break;
            case Character.characterClass.paladin:
                classInput.value = 4;
                break;
        }
        
        deleteButton.SetActive(true);
        deleteButton.GetComponentInChildren<Text>().text = "Delete \n" + nameInput.text;
        
        overwriteToggle.gameObject.SetActive(true);
        overwriteToggle.gameObject.GetComponentInChildren<Text>().text = "Overwrite " + nameInput.text + " ?";
        canOverwrite = true;
    }
    

    public void DestroyAllPreviousCharacters()
    {
        allCharactersList.allCharacters.Clear();
        WriteCharactersToJSON("/JSON/PlayerInfo.json");
    }


    public void DeleteCharacter()
    {
        allCharactersList.allCharacters.RemoveAt(charInt);
        
        nameInput.text = "";
        healthInput.value = 0;
        classInput.value = 0;
        
        canOverwrite = false;
        deleteButton.SetActive(false);
        overwriteToggle.gameObject.SetActive(false);
        WriteCharactersToJSON("/JSON/PlayerInfo.json");
        
       
    }
    
}
