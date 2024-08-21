using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ContentBlockScripable))]
public class PlayerInfoEditor : Editor {
    private ContentBlockScripable _itemBase;
    private void Awake()
    {
        _itemBase = (ContentBlockScripable)target;
    }
    public override void OnInspectorGUI()
    {

        if (GUILayout.Button("New Item"))
        {
            _itemBase.CreateItem();
        }
        if (GUILayout.Button("Remove"))
        {
            _itemBase.RemoveItem();
        }
        if (GUILayout.Button("Save"))
        {
            if (_itemBase != null)
            {
                EditorUtility.SetDirty(_itemBase);
                AssetDatabase.SaveAssets();
            }
        }
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("<"))
        {
            _itemBase.PrevItem();
        }
        if (GUILayout.Button(">"))
        {
            _itemBase.NextItem();
        }
        GUILayout.EndHorizontal();

        base.OnInspectorGUI();
    }
}
