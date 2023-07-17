using System.IO;
using UnityEditor;
using UnityEngine;

namespace XRC.Core
{
    /// <summary>
    ///     This code creates a custom asset file of type StudentSubmissionSettings at the specified path in the
    ///     Unity Editor. It also ensures that the required "Resources" directory exists and creates it if it doesn't exist.
    /// </summary>
    public class CreateStudentSubmissionAsset : MonoBehaviour
    {
        [MenuItem("XRC/Create Student Submission Asset")]
        private static void CreateMyAsset()
        {
            // Check if Resources directory exists, create one if it doesn't
            CheckOrCreateResourcesDirectory();

            // Create a new instance of the ScriptableObject
            var asset = ScriptableObject.CreateInstance<StudentSubmissionSettings>();

            // Create the asset file at the selected path
            var assetPath = "Assets/XRC/StudentInfo.asset";
            AssetDatabase.CreateAsset(asset, assetPath);

            // Save any pending asset changes
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            // Focus and select the newly created asset in the Project window
            EditorUtility.FocusProjectWindow();
            Selection.activeObject = asset;
        }

        private static void CheckOrCreateResourcesDirectory()
        {
            // Define the path of the Resources directory
            var resourcesPath = Application.dataPath + "/XRC";

            // Check if the Resources directory already exists
            if (!Directory.Exists(resourcesPath))
            {
                // Create the Resources directory if it doesn't exist
                Directory.CreateDirectory(resourcesPath);

                // Refresh the Asset Database to show the newly created directory
                AssetDatabase.Refresh();
            }
        }
    }
}