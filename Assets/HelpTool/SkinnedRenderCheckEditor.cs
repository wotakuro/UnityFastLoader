using UnityEngine;
using UnityEditor;
using System.Collections;

public class SkinnedRenderCheckEditor : EditorWindow
{

    private SkinnedMeshRenderer renderer;
    private Vector2 scroll;

    [MenuItem("Tools/SkinnedRenderCheckEditor")]
    public static void Create()
    {
        EditorWindow.GetWindow<SkinnedRenderCheckEditor>();
    }

    void OnGUI()
    {
        renderer = EditorGUILayout.ObjectField(renderer, typeof(SkinnedMeshRenderer)) as SkinnedMeshRenderer;
        if (renderer == null)
        {
            return;
        }


        scroll = EditorGUILayout.BeginScrollView(scroll);

        EditorGUILayout.ObjectField("root",renderer.rootBone, typeof(Transform));
        EditorGUILayout.Space();

        if (renderer.bones != null)
        {
            for (int i = 0; i <  renderer.bones.Length; ++i)
            {
                EditorGUILayout.ObjectField( i.ToString() , renderer.bones[i],typeof(Transform ) );
            }
        }
        EditorGUILayout.EndScrollView();
    }

    private void LabelField(string s1,string s2)
    {
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField(s1);
        EditorGUILayout.LabelField(s2);
        EditorGUILayout.EndHorizontal();
    }
}
