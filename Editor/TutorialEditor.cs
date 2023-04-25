using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Tutorial))]
[CanEditMultipleObjects]
public class TutorialEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        Tutorial t = (Tutorial)target;

        EditorGUILayout.HelpBox("Tutorial Editor:", MessageType.None);

        if (GUILayout.Button("Go To Step Zero"))
        {
            t.GoToZero();
        }

        if (GUILayout.Button("Harvest Layout"))
        {
            t.Harvest();
        }

        if (GUILayout.Button("Apply Text"))
        {
            t.ApplyText();
        }

        if (GUILayout.Button("Bake Current Step"))
        {
            t.BakeCurrentStep();
        }

        if (GUILayout.Button("Next Step"))
        {
            t.NextStep();
        }

        if (GUILayout.Button("Previous Step"))
        {
            t.PreviousStep();
        }

        if (GUILayout.Button("Load Current Step"))
        {
            t.LoadCurrentStep();
        }

        if (GUILayout.Button("Insert New Step At Position"))
        {
            t.InsertBlankAtStep();
        }

        if (GUILayout.Button("Yeet Current Step"))
        {
            t.Yeet();
        }

        if (GUILayout.Button("Reset All Overrides"))
        {
            t.ResetAll();
        }


        if (GUILayout.Button("Export JSON"))
        {
            t.Export();
        }

        if (GUILayout.Button("Import JSON"))
        {
            t.ResetAll();
        }
    }
}
