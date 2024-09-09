#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(HitBox))]
public class HitBoxGUI : Editor
{
    private GUIState state;

    private HitBox component;

    private void OnEnable()
    {
        component = target as HitBox;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (GUILayout.Button("Move HitBox"))
        {
            if (state == GUIState.Move) state = GUIState.None;
            else state = GUIState.Move;
        }

        if (GUILayout.Button("Size HitBox"))
        {
            if (state == GUIState.Size) state = GUIState.None;
            else state = GUIState.Size;
        }

        if (GUILayout.Button("Next HitBox")) component.Select++;

        if (GUILayout.Button("Prev HitBox")) component.Select--;
    }

    private void OnSceneGUI()
    {
        switch (state)
        {
            case GUIState.None:
                break;

            case GUIState.Move:
                Tools.current = Tool.None;

                component[state] = Handles.PositionHandle(component.transform.position + component[GUIState.Move], component.transform.rotation) - component.transform.position;
                break;

            case GUIState.Size:
                Tools.current = Tool.None;

                component[state] = Handles.ScaleHandle(component[state], component.transform.position + component[GUIState.Move], component.transform.rotation);
                break;
        }
    }

    [DrawGizmo(GizmoType.InSelectionHierarchy)]
    private static void DrawHitBox(HitBox _component, GizmoType _gizmoType)
    {
        Gizmos.matrix = _component.transform.localToWorldMatrix;
        Gizmos.color = Color.green;

        Gizmos.DrawWireCube(_component[GUIState.Move], _component[GUIState.Size] * 2);
    }
}

public enum GUIState
{
    None,
    Move,
    Size
}
#endif