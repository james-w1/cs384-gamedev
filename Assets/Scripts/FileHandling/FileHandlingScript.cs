using System.Collections;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;

public static class FileHandlingScript
{
    public static void SaveGame()
    {

    }

    public static GameData LoadGame()
    {
        return null;
    }

    public static bool SavePlayerData(string playerName, PlayerSave saveData)
    {
        BinaryFormatter formatter = new BinaryFormatter();

        using (FileStream stream = new FileStream(GetPlayerPath(playerName), FileMode.Create))
        {
            try
            {
                if (saveData == null)
                    saveData = new PlayerSave(playerName);

                formatter.Serialize(stream, saveData);
            }
            catch (System.Exception)
            {
                throw;
            }
            return true;
        }
    }

    public static PlayerSave LoadPlayerData(string playerName)
    {
        try
        {
            string contents = File.ReadAllText(GetPlayerPath(playerName));
            return JsonUtility.FromJson<PlayerSave>(contents);
        }
        catch (System.Exception)
        {
            throw;
        }
    }

    public static List<PlayerSave> LoadAllPlayers()
    {
        BinaryFormatter formatter = new BinaryFormatter();
        List<PlayerSave> output = new List<PlayerSave>();

        try
        {
            string[] files = Directory.GetFiles(GetPlayersDir());

            Debug.Log(files.Length);
            foreach (string fileName in files) {
                using (FileStream stream 
                        = new FileStream(fileName, FileMode.Open))
                {
                    try
                    {
                        PlayerSave s = formatter.Deserialize(stream) as PlayerSave;
                        output.Add(s);
                    }
                    catch (System.Exception)
                    {
                        throw;
                    }
                }
            }
            return output;
        }
        catch (System.Exception)
        {
            throw;
        }
    }

    public static bool PlayerSaveExists(string playerName)
    {
        return File.Exists(GetPlayerPath(playerName));
    }

    public static string GetPlayerPath(string playerName)
    {
        return Application.persistentDataPath + "/players/" + playerName;
    }

    public static string GetPlayersDir()
    {
        try {
            string dir = Application.persistentDataPath + "/players";
            if (!Directory.Exists(dir)) {
                Directory.CreateDirectory(Application.persistentDataPath + "/players");
            }
            return dir;
        }                                                                                                                                                                                                                                
        catch (System.Exception)
        {
            throw;
        }
    }
}
