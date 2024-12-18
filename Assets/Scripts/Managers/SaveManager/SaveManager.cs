using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine.Rendering.PostProcessing;
using System;

public class SaveManager : MonoSingleton<SaveManager>
{
    public static event Action<float> OnMouseSensitivityChanged;

    public void Save(float musicVol, float effectsVol, float bestTime, float mouseSensitivity)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/save.datasave";
        FileStream stream = new FileStream(path, FileMode.Create);

        DataContainer data = new DataContainer();

        data.musicVolume = musicVol;
        data.sEffectsVolume = effectsVol;
        data.bestTime = bestTime;
        data.mouseSensitivity = mouseSensitivity;

        formatter.Serialize(stream, data);
        stream.Close();
    }

    public void Save(float volume, string affected)
    {
        DataContainer loadedData = Load();
        
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/save.datasave";
        FileStream stream = new FileStream(path, FileMode.Create);

        DataContainer data = new DataContainer();

        if(loadedData != null)
        {
            data.sEffectsVolume = loadedData.sEffectsVolume;
            data.musicVolume = loadedData.musicVolume;
        }

        if(affected == "MUSIC")
        {
            data.musicVolume = volume;
            
        }
        else if(affected == "EFFECTS")
        {
            data.sEffectsVolume = volume;
        }

        formatter.Serialize(stream, data);
        stream.Close();
    }

    public void SaveBestTime(float time)
    {
        DataContainer loadedData = Load();
        
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/save.datasave";
        FileStream stream = new FileStream(path, FileMode.Create);

        DataContainer data = new DataContainer();

        if(loadedData != null) data = loadedData;

        data.bestTime = time;

        formatter.Serialize(stream, data);
        stream.Close();
    }

    public void SaveMouseSensitivity(float sensitivity)
    {
        DataContainer loadedData = Load();
        
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/save.datasave";
        FileStream stream = new FileStream(path, FileMode.Create);

        DataContainer data = new DataContainer();

        if(loadedData != null) data = loadedData;

        data.mouseSensitivity = sensitivity;
        if(OnMouseSensitivityChanged != null) OnMouseSensitivityChanged(sensitivity);

        formatter.Serialize(stream, data);
        stream.Close();
    }

    public DataContainer Load()
    {
        string path = Application.persistentDataPath + "/save.datasave";
        if(File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            if(stream.Length == 0)
            {
                stream.Close();
                return null;
            }

            DataContainer data = (DataContainer)formatter.Deserialize(stream);
            stream.Close();

            return data;
        }
        else
        {
            return null;
        }
    }
}