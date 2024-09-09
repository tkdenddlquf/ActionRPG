#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(HitBox))]
public class HitBoxGUI : Editor
{
    private GUIMode mode;
    private HitBox component;

    private void OnEnable()
    {
        component = target as HitBox;
    }

    public override void OnInspectorGUI()
    {
        component.Select = EditorGUILayout.IntSlider("Select", component.Select, 0, component.pos.Count);
        component[GUIMode.Pos] = EditorGUILayout.Vector3Field("Pos", component[GUIMode.Pos]);
        component[GUIMode.Scale] = EditorGUILayout.Vector3Field("Scale", component[GUIMode.Scale]);

        GUILayout.Space(5);

        GUILayout.BeginHorizontal();

        if (GUILayout.Button("Pos"))
        {
            if (mode == GUIMode.Pos) mode = GUIMode.None;
            else mode = GUIMode.Pos;
        }

        if (GUILayout.Button("Scale"))
        {
            if (mode == GUIMode.Scale) mode = GUIMode.None;
            else mode = GUIMode.Scale;
        }

        GUILayout.EndHorizontal();

        GUILayout.Space(5);

        GUILayout.BeginHorizontal();

        if (GUILayout.Button("Add"))
        {
            component.scale.Add(new(1, 1, 1));
            component.pos.Add(new(0, 0, 0));

            component.Select = component.pos.Count - 1;

            EditorUtility.SetDirty(target);

            Undo.RecordObject(component, "Add");
        }

        if (GUILayout.Button("Remove"))
        {
            if (component.Select == component.pos.Count - 1) component.Select--;

            component.scale.RemoveAt(component.Select + 1);
            component.pos.RemoveAt(component.Select + 1);

            EditorUtility.SetDirty(target);

            Undo.RecordObject(component, "Remove");
        }

        GUILayout.EndHorizontal();

        if (GUI.changed)
        {
            EditorUtility.SetDirty(target);

            Undo.RecordObject(target, "Changed");
        }
    }

    private void OnSceneGUI()
    {
        switch (mode)
        {
            case GUIMode.None:
                break;

            case GUIMode.Pos:
                Tools.current = Tool.None;

                component[mode] = Handles.PositionHandle(component.transform.position + component[GUIMode.Pos], component.transform.rotation) - component.transform.position;
                break;

            case GUIMode.Scale:
                Tools.current = Tool.None;

                component[mode] = Handles.ScaleHandle(component[mode], component.transform.position + component[GUIMode.Pos], component.transform.rotation);
                break;
        }
    }

    [DrawGizmo(GizmoType.InSelectionHierarchy)]
    private static void DrawHitBox(HitBox _component, GizmoType _gizmoType)
    {
        Gizmos.matrix = _component.transform.localToWorldMatrix;
        Gizmos.color = Color.green;

        Gizmos.DrawWireCube(_component[GUIMode.Pos], _component[GUIMode.Scale] * 2);
    }
}

public enum GUIMode
{
    None,
    Pos,
    Scale
}
#endif