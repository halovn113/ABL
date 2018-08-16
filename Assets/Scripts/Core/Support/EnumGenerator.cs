using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

#if UNITY_EDITOR
using UnityEditor;
#endif

// sẽ hoàn thiện sau và sẽ có dành cho ScriptableObject 
//public class EnumGenerator : MonoBehaviour
[CreateAssetMenu(fileName = "Enum Generator", menuName = "Enum Generator/Create")]
public class EnumGenerator : ScriptableObject
{
    public List<ENUM> ListEnums = new List<ENUM>();

    [System.Serializable]
    public struct ENUM
    {
        public string name;
        public List<string> values;
    }

    public void AddEnums()
    {
        ListEnums.Add(new ENUM());
    }

    public void RemoveLastEnum()
    {
        ListEnums.RemoveAt(ListEnums.Count - 1);
    }

#if UNITY_EDITOR
    public class EnumGeneratorWindow : EditorWindow
    {
        string enumFilePath = "Assets/Other/Enum/";
        string enumFileName = "";

        public List<ENUM> ListEnums;
        List<string> data;
        public const string WARNING_FILE_EXIST = "Warning, there is a file already exists. Do you want to overwrite that file?";

        void OnGUI()
        {
            ScriptableObject target = this;
            SerializedObject so = new SerializedObject(target);
            SerializedProperty stringsProperty = so.FindProperty("ListEnums");
            enumFileName = target.GetType().Name + "Enums";

            EditorGUILayout.PropertyField(stringsProperty, true); // True means show children
            so.ApplyModifiedProperties(); // Remember to apply modified properties

            EditorGUILayout.LabelField(" ------- Create List String Field --------", EditorStyles.boldLabel);
            enumFilePath = EditorGUILayout.TextField("List Enum File Path", enumFilePath);
            enumFileName = EditorGUILayout.TextField("List Enum File Name ", enumFileName);

            if (GUILayout.Button("Create Enums"))
            {
                if (ListEnums.Count == 0)
                {
                    Debug.LogWarning("Warning, this list is empty");
                    return;
                }
                data = new List<string>();
                var list = ListEnums;
                foreach (EnumGenerator.ENUM e in list)
                {
                    data.Add("public enum " + e.name + "\n{");
                    for (int i = 0; i < e.values.Count; i++)
                    {
                        data.Add(string.Format("\t{0} = {1},", e.values[i], i));
                    }
                    data.Add("}\n");
                }

                if (CheckIfFileIsExist(enumFilePath, enumFileName))
                {
                    if (EditorUtility.DisplayDialog("WARNING", WARNING_FILE_EXIST, "YES", "NO"))
                    {
                        WriteToEnum(enumFilePath, enumFileName, data);
                    }
                }
                else
                {
                    WriteToEnum(enumFilePath, enumFileName, data);
                }
            }

            if (GUILayout.Button("Delete Enum"))
            {
                DeleteFile(enumFilePath, enumFileName);
            }
        }

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
#endif

#if UNITY_EDITOR
    [MenuItem("Tools/EnumCreator")]
    public static void ShowEnumCreatorWindow()
    {
        EditorWindow.GetWindow<EnumGeneratorWindow>();
    }
#endif
}
