using UnityEngine;
using System.IO;

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
    // Start is called before the first frame update
    void Start()
    {
        
        Character newCharacter = new Character();
        newCharacter.name = "Aleks";
        newCharacter.cClass = Character.characterClass.bard;
        newCharacter.health = 10;

        Character.WriteToJson("/JSON/PlayerInfo.json", newCharacter);


        Character newPlayer = Character.CreateFromJSON("/JSON/PlayerInfo.json");
        
    }
}
