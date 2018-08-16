using NUnit.Framework.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.AI;

[CustomEditor(typeof(EnumGenerator))]
public class EnumGeneratorEditor : Editor
{
    [HideInInspector]
    public string enumFilePath = "Assets/Other/Enum/";
    [HideInInspector]
    public string enumFileName = "";

    string lsFilePath = "Assets/Other/ListString/";
    string lsFileName = "";

    public string enumName;
    public const string WARNING_FILE_EXIST = "Warning, there is a file already exists. Do you want to overwrite that file?";

    public UnityEngine.Object stringObject;

    private List<string>[] listString;
    private EnumGenerator someClass;
    private List<IList> temp;

    private ReorderableList stringListOrder;
    private List<string> data;


    private const string ENUM = "public enum ";

    private void OnEnable()
    {
        someClass = (EnumGenerator)target;
        enumFileName = target.GetType().Name + "Enums";
        lsFileName = target.GetType().Name + "ListString";
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        EditorGUILayout.LabelField(" ------- Create List String Field --------", EditorStyles.boldLabel);
        enumFilePath = EditorGUILayout.TextField("List Enum File Path", enumFilePath);
        enumFileName = EditorGUILayout.TextField("List Enum File Name ", enumFileName);

        if (GUILayout.Button("Add Enum"))
        {
            someClass.AddEnums();
        }

        if (GUILayout.Button("Remove Lastest Enum"))
        {
            someClass.RemoveLastEnum();
        }


        if (GUILayout.Button("Create Enums"))
        {
            if (someClass.ListEnums.Count == 0)
            {
                Debug.LogWarning("Warning, this list is empty");
                return;
            }
            data = new List<string>();
            var list = someClass.ListEnums;
            foreach (EnumGenerator.ENUM e in list)
            {
                data.Add("public enum " + e.name + "\n{");
                for (int i = 0; i < e.values.Count; i++)
                {
                    data.Add(string.Format("\t{0} = {1},", e.values[i], i));
                }
                data.Add("}\n");
            }

            if (EnumWriterEditor.CheckIfFileIsExist(enumFilePath, enumFileName))
            {
                if (EditorUtility.DisplayDialog("WARNING", WARNING_FILE_EXIST, "NO", "YES"))
                {
                    EnumWriterEditor.WriteToEnum(enumFilePath, enumFileName, data);
                }
            }
            else
            {
                EnumWriterEditor.WriteToEnum(enumFilePath, enumFileName, data);
            } 
        }

        if (GUILayout.Button("Delete Enum"))
        {
            EnumWriterEditor.DeleteFile(enumFilePath, enumFileName);
        }
    }
}
