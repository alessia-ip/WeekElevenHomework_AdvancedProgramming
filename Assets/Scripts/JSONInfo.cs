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

public class JSONInfo : MonoBehaviour
{
    public InputField nameInput;
    public Slider healthInput;
    public Dropdown classInput;
    
    // Start is called before the first frame update
    void Start()
    {
        
        Character newCharacter = new Character();
        newCharacter.name = "Aleks";
        newCharacter.cClass = Character.characterClass.bard;
        newCharacter.health = 10;

        Character.WriteToJson("/JSON/PlayerInfo.json", newCharacter);
        
        //Character newPlayer = Character.CreateFromJSON("/JSON/PlayerInfo.json");
        
    }

    public void SaveCharacter()
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
        
        Character.WriteToJson("/JSON/PlayerInfo.json", newCharacter);
    }
}
