#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
 
[ExecuteInEditMode]
public class CreateQuadFromPoints : MonoBehaviour {
 
    public Vector3 lowerLeft;
    public Vector3 upperRight;
 
    private Vector3 lastLowerLeft;
    private Vector3 lastUpperRight;
 
    private MeshRenderer myMeshRenderer;
    private MeshFilter myMeshFilter;
    private Mesh myMesh;
 
    void OnEnable() {
        //Create the needed parts if they're not already attached
        EnsureMeshFilter();
        EnsureMeshRenderer();
        EnsureMesh();
    }
 
    void Update() {
        bool positionChanged = lastLowerLeft != lowerLeft || lastUpperRight != upperRight;
        if (lastLowerLeft != lowerLeft || lastUpperRight != upperRight) {
            lastLowerLeft = lowerLeft;
            lastUpperRight = upperRight;
        }
 
        if (!positionChanged)
            return;
 
        Vector3[] verts = {
            lowerLeft,
            new Vector3(lowerLeft.x, upperRight.y, lowerLeft.z), //upper left
            upperRight,
            new Vector3(upperRight.x, lowerLeft.y, upperRight.z) //lower right
        };
 
        int[] tris = {
            0, 1, 2, //leftmost triangle
            0, 2, 3 //rightmost triangle
        };
 
        myMesh.vertices = verts;
        myMesh.triangles = tris;
        //create uv's and normals and whatever if you need to
    }
 
    private void EnsureMeshRenderer() {
        if (!myMeshRenderer)
            myMeshRenderer = GetComponent<MeshRenderer>();
        if (!myMeshRenderer) {
            myMeshRenderer = gameObject.AddComponent<MeshRenderer>();
            myMeshRenderer.material = new Material(Shader.Find("Standard"));
        }
    }
 
    private void EnsureMeshFilter() {
        if (!myMeshFilter)
            myMeshFilter = GetComponent<MeshFilter>();
        if (!myMeshFilter)
            myMeshFilter = gameObject.AddComponent<MeshFilter>();
    }
 
    private void EnsureMesh() {
        if (!myMesh) {
            if (!myMeshFilter.sharedMesh) {
                myMesh = new Mesh();
                myMeshFilter.sharedMesh = myMesh;
            }
            else {
                myMesh = myMeshFilter.sharedMesh;
            }
        }
    }
}
 
// I threw in a custom editor that draws some handles to allow you to edit the points more easily.
// Protip: you can have this in the same file as the main class! Just wrap it in #If UNITY_EDITOR for builds.
#if UNITY_EDITOR
[CustomEditor(typeof(CreateQuadFromPoints))]
public class CreateQuadFromPointsEditor : Editor {
 
    private CreateQuadFromPoints script;
    public Tool LastTool;
 
    void OnEnable() {
        script = target as CreateQuadFromPoints;
 
        // This makes the transform tool be hidden, so you only see the handles for the upper right and lower right points
        LastTool = Tools.current;
        Tools.current = Tool.None;
    }
 
    void OnDisable() {
        Tools.current = LastTool;
    }
 
    void OnSceneGUI() {
        //Add the position to the handles, and then remove it from the mesh positions
        script.lowerLeft = Handles.DoPositionHandle(script.transform.position + script.lowerLeft, Quaternion.identity) - script.transform.position;
        script.upperRight = Handles.DoPositionHandle(script.transform.position + script.upperRight, Quaternion.identity) - script.transform.position;
        if (GUI.changed) {
            EditorUtility.SetDirty(script);
        }
    }
}
#endif