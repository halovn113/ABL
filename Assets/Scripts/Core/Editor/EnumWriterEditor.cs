using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.Video;

public class EnumWriterEditor : Editor
{
    const string extension = ".cs";

    public static bool CheckIfFileIsExist(string path, string name)
    {
        if (File.Exists(path + name + extension))
        {
            return true;
        }
        return false;
    }

    public static void DeleteFile(string path, string name)
    {
        if (!CheckIfFileIsExist(path, name))
        {
            Debug.LogWarning("Warning, the file doesn't exist");
            return;
        }
        File.Delete(path + name + extension);
    }

    public static void WriteToEnum<T>(string path, string name, ICollection<T> data)
    {
        if (!Directory.Exists(path))
        {
            DirectoryInfo di = Directory.CreateDirectory(path);
        }
        using (StreamWriter file = File.CreateText(path + name + extension))
        {
            foreach (var d in data)
            {
                Debug.Log("d " + d.ToString());
                file.WriteLine(d.ToString());

            }
        }
        Debug.LogWarning("Write Enum Success");
        AssetDatabase.ImportAsset(path + name + extension);
    }

}
