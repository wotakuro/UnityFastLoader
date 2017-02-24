using UnityEngine;
using UnityEditor;
using System.Collections;

public class AnimationCheckEditor : EditorWindow {

    private AnimationClip clip;
    private Vector2 scroll;

    [MenuItem("Tools/AnimationCheck")]
    public static void Create()
    {
        EditorWindow.GetWindow<AnimationCheckEditor>();
    }

    void OnGUI()
    {
        clip = EditorGUILayout.ObjectField(clip, typeof(AnimationClip)) as AnimationClip;
        if (clip == null)
        {
            return;
        }
        var curveBindings = AnimationUtility.GetCurveBindings(clip);

        EditorGUILayout.LabelField("curves " + curveBindings.Length);

        scroll = EditorGUILayout.BeginScrollView(scroll);
        foreach (var binding in curveBindings)
        {
            AnimationCurve curve = AnimationUtility.GetEditorCurve(clip, binding);
            EditorGUILayout.LabelField(binding.path + "/" + binding.propertyName + ", Keys: " + curve.keys.Length);
            EditorGUILayout.CurveField(curve);
        }

        EditorGUILayout.EndScrollView();
    }
}
