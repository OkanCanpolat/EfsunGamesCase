using Newtonsoft.Json;
using System;
using System.IO;
using UnityEngine;

public class FileDataHandler : IDataHandler
{
    private string dataDirPath;
    private string dataFileName;
    private bool useEncryption;
    private readonly string encryptionCode = "EfsunGames";
    public FileDataHandler(string dataDirPath, string dataFileName, bool useEncryption)
    {
        this.dataDirPath = dataDirPath;
        this.dataFileName = dataFileName;
        this.useEncryption = useEncryption;
    }
    public GameData Load()
    {
        GameData gameData = null;

        string fullPath = Path.Combine(dataDirPath, dataFileName);

        if (File.Exists(fullPath))
        {
            try
            {
                string dataToLoad = "";

                using (FileStream fileStream = new FileStream(fullPath, FileMode.Open))
                {
                    using (StreamReader reader = new StreamReader(fileStream))
                    {
                        dataToLoad = reader.ReadToEnd();
                    }
                }

                if (useEncryption) dataToLoad = EncryptDecrypt(dataToLoad);

                gameData = JsonConvert.DeserializeObject<GameData>(dataToLoad);
            }
            catch (Exception e)
            {
                Debug.LogError("Errow while loadind data from file: " + fullPath + "\n" + e);
            }
        }

        return gameData;
    }

    public void Save(GameData gameData)
    {
        string fullPath = Path.Combine(dataDirPath, dataFileName);

        try
        {
            Directory.CreateDirectory(Path.GetDirectoryName(fullPath));

            string dataToStore = JsonConvert.SerializeObject(gameData);

            if (useEncryption) dataToStore = EncryptDecrypt(dataToStore);

            using (FileStream fileStream = new FileStream(fullPath, FileMode.Create))
            {
                using (StreamWriter streamWriter = new StreamWriter(fileStream))
                {
                    streamWriter.Write(dataToStore);
                }
            }
        }

        catch (Exception e)
        {
            Debug.LogError("Error while saving data to file: " + fullPath + "\n" + e);
        }
    }

    private string EncryptDecrypt(string data)
    {
        string result = "";

        for (int i = 0; i < data.Length; i++)
        {
            result += (char)(data[i] ^ encryptionCode[i % encryptionCode.Length]);
        }

        return result;
    }
}
