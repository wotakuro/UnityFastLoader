using UnityEngine;
using UnityEditor;
using System.Collections;

public class MeshCheckEditor : EditorWindow
{

    private Mesh mesh;
    private Vector2 scroll;

    [MenuItem("Tools/MeshCheckEditor")]
    public static void Create()
    {
        EditorWindow.GetWindow<MeshCheckEditor>();
    }

    void OnGUI()
    {
        mesh = EditorGUILayout.ObjectField(mesh, typeof(Mesh)) as Mesh;
        if (mesh == null)
        {
            return;
        }

        scroll = EditorGUILayout.BeginScrollView(scroll);

        LabelField("vertexCount	    " , ((mesh.vertexCount == null) ? "null" : mesh.vertexCount.ToString()));

        LabelField("colors          " , ((mesh.colors == null) ? "null" : mesh.colors.Length.ToString()));
        LabelField("colors32        " , ((mesh.colors32 == null) ? "null" : mesh.colors32.Length.ToString()));
        LabelField("normals         " , ((mesh.normals == null) ? "null" : mesh.normals.Length.ToString()));
        LabelField("tangents	    " , ((mesh.tangents == null) ? "null" : mesh.tangents.Length.ToString()));
        LabelField("uv	            " , ((mesh.uv == null) ? "null" : mesh.uv.Length.ToString()));
        LabelField("uv2	            " , ((mesh.uv2 == null) ? "null" : mesh.uv2.Length.ToString()));
        LabelField("uv3     	    " , ((mesh.uv3 == null) ? "null" : mesh.uv3.Length.ToString()));
        LabelField("uv4	            " , ((mesh.uv4 == null) ? "null" : mesh.uv4.Length.ToString()));
        LabelField("vertices	    " , ((mesh.vertices == null) ? "null" : mesh.vertices.Length.ToString()));

        LabelField("subMeshCount" , ((mesh.subMeshCount == null) ? "null" : mesh.subMeshCount.ToString()));
        for (int i = 0; i < mesh.subMeshCount; ++i)
        {
            LabelField("subMesh" + i + "_Indices", mesh.GetIndices(i).Length.ToString());
            LabelField("subMesh" + i + "_GetTriangles", mesh.GetTriangles(i).Length.ToString());
            LabelField("subMesh" + i + "_Topology", mesh.GetTopology(i).ToString());
        }


        /** ボーンアニメに必要 */
        LabelField("bindposes       ", ((mesh.bindposes == null) ? "null" : mesh.bindposes.Length.ToString()));
        LabelField("boneWeights     ", ((mesh.boneWeights == null) ? "null" : mesh.boneWeights.Length.ToString()));

        /** ブレンドアニメに必要 */
        LabelField("blendShapeCount ", ((mesh.blendShapeCount == null) ? "null" : mesh.blendShapeCount.ToString()));
        for (int i = 0; i < mesh.blendShapeCount; ++i)
        {
            int frameCount = mesh.GetBlendShapeFrameCount(i);
            LabelField(" blend " + mesh.GetBlendShapeName(i), frameCount.ToString());
            for (int j = 0; j < frameCount; ++j)
            {
                LabelField("  blendWeight " , mesh.GetBlendShapeFrameWeight(i, j).ToString() );
            }
        }
        EditorGUILayout.EndScrollView();
    }

    private void WriteMeshData(Mesh mesh)
    {
    }

    private void LabelField(string s1,string s2)
    {
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField(s1);
        EditorGUILayout.LabelField(s2);
        EditorGUILayout.EndHorizontal();
    }
}
