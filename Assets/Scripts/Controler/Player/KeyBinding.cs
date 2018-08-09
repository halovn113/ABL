using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyBinding : MonoBehaviour
{
    Dictionary<string, KeyCode> keys = new Dictionary<string, KeyCode>();
    public void Init()
    {
        keys.Add("Up", KeyCode.W);
        keys.Add("Down", KeyCode.S);
        keys.Add("Left", KeyCode.A);
        keys.Add("Right", KeyCode.D);
        keys.Add("Dash", KeyCode.Space);
        keys.Add("Attack1", KeyCode.Mouse0);
    }

    public KeyCode GetKey(string key)
    {
        return keys[key];
    }

    public void EditKey(string key, KeyCode keycode)
    {
        if (!keys.ContainsKey(key))
        {
            Debug.LogWarning("Warning, key is not found");
            return;
        }
        if (keys[key] == keycode)
        {
            Debug.LogWarning("Warning, the input key is same");
            return;
        }
        if (keys.ContainsValue(keycode))
        {
            Debug.Log("Warning, the keycode is already bind ");
            return;
        }
        keys[key] = keycode;
    }
}
