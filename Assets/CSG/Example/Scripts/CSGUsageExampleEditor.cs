using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

/**
 * This is a simple Editor helper script for rapid testing/prototyping! 
 */
[CustomEditor(typeof(CSGUsageExample))]
public class CSGUsageExampleEditor : Editor
{

    public override void OnInspectorGUI()
    {
        CSGUsageExample example = (CSGUsageExample)target;

        example.lhs = (GameObject)EditorGUILayout.ObjectField(example.lhs, typeof(GameObject), true);

        if (example.lhs == null)
        {
            EditorGUILayout.LabelField("Add a GameObject on left hand side.");

            return;
        }

        example.rhs = (GameObject)EditorGUILayout.ObjectField(example.rhs, typeof(GameObject), true);

        if (example.rhs == null)
        {
            EditorGUILayout.LabelField("Add a GameObject on right hand side.");

            return;
        }

        example.Operation = (CSG.BooleanOp)EditorGUILayout.EnumPopup(example.Operation);



        if (!example.lhs.activeInHierarchy || !example.rhs.activeInHierarchy)
        {
            EditorGUILayout.LabelField("Object is Hidden. Cannot CSG...");

            return;
        }

        if (example.lhs.GetComponent<MeshFilter>() == null || example.rhs.GetComponent<MeshFilter>() == null)
        {
            EditorGUILayout.LabelField("Both GameObjects must have a MeshFilter.");

            return;
        }



        example.material = (Material)EditorGUILayout.ObjectField(example.material, typeof(Material), true);

        if (GUILayout.Button("Perform CSG"))
        {
            example.Perform();
        }
    }
}
