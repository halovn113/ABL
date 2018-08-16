using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

// bỏ qua vì lỗi multiple edit
#if UNITY_EDITOR
//[CustomEditor(typeof(GeneratorArrayMap))]
public class GeneratorArrayMapEditor : Editor
{
    //public override void OnInspectorGUI()
    //{
    //    var script = target as GeneratorArrayMap;
    //    if ((script.option & GeneratorArrayMap.Option.HasFocusPoint) != 0)
    //    {
    //        script.focusPoint.x = EditorGUILayout.IntSlider("Focus point x: ", script.focusPoint.x, 1, 100);
    //        script.focusPoint.y = EditorGUILayout.IntSlider("Focus point y: ", script.focusPoint.y, 1, 100);

    //    }
    //}
}

#endif
