using System.IO;
using System;
using UnityEngine;

public class DataSaver  {
    public LevelBuilderData Load(string mapName) {
        string fullPath = Application.dataPath + "/" + "SaveMapData" + "/" + mapName;
        LevelBuilderData loadedData = null;
        if(File.Exists(fullPath)) {
            try {
                string dataToLoad = "";

                using(FileStream stream = new FileStream(fullPath , FileMode.Open)) {
                    using(StreamReader reader = new StreamReader(stream)) {
                        dataToLoad = reader.ReadToEnd();
                    }
                }

                loadedData = JsonUtility.FromJson<LevelBuilderData>(dataToLoad);
            }
            catch(Exception e) {
                Debug.LogError("Error occured when trying to load data to file: " + fullPath + "\n" + e);
            }
        }

        return loadedData;
    }

    public void Save(string mapName, LevelBuilderData data) {
        string fullPath = Application.dataPath + "/" + "SaveMapData" + "/" + mapName;
        try {
            Directory.CreateDirectory(Path.GetDirectoryName(fullPath));

            string dataToStore = JsonUtility.ToJson(data , true);

            using(FileStream stream = new FileStream(fullPath , FileMode.Create)) {
                using(StreamWriter writer = new StreamWriter(stream)) {
                    writer.Write(dataToStore);
                }
            }
        }
        catch(Exception e) {
            Debug.LogError("Error occured when tryinf to save data to file: " + fullPath + "\n" + e);
        }
    }
}
