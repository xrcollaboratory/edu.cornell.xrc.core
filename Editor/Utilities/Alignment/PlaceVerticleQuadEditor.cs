#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

#if UNITY_EDITOR
[CustomEditor(typeof(PlaceVerticleQuad))]
public class PlaceVerticleQuadEditor : Editor {
 
    private PlaceVerticleQuad script;
    public Tool LastTool;
 
    void OnEnable() {
        script = target as PlaceVerticleQuad;
 
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