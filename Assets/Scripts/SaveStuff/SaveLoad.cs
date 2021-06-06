using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public static class SaveLoadModule
{
    public static void SaveGame()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/wiz1.dsk");
        bf.Serialize(file, GameManager.ROSTER);
        file.Close();
        LoadGame();
    }

    public static void LoadGame()
    {
        if (!File.Exists(Application.persistentDataPath + "/wiz1.dsk"))
        {
            FileStream file = File.Create(Application.persistentDataPath + "/wiz1.dsk");
        }
        else
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/wiz1.dsk", FileMode.Open);
            if (file.Length > 0) GameManager.ROSTER = (List<PlayerCharacter>)bf.Deserialize(file);
            file.Close();            
        }
    }
}