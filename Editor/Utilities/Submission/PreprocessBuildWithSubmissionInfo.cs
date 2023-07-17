using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;

namespace XRC.Core
{
    /// <summary>
    ///     In this script, we implement the IPreprocessBuildWithReport interface, which is a Unity interface used to execute
    ///     code before each build. The interface requires implementing the OnPreprocessBuild method, which will be called
    ///     automatically before each build.
    ///     Inside the OnPreprocessBuild method, we call a separate function ExecuteBeforeBuild, which is where you can define
    ///     your
    ///     custom logic that needs to be executed before each build.This is the place to perform any
    ///     necessary tasks such as preparing data, modifying settings, or performing other actions specific to
    ///     your project's needs.
    ///     The callbackOrder property returns the callback order for this
    ///     internal interface implementation.
    ///     A lower value means the method will be called earlier in the build process.In this example, we set it to 0, but you
    ///     can
    ///     adjust it as needed.
    ///     Remember to place this script in the Editor folder of your Unity project for it to
    ///     be recognized and executed during the build process.
    /// </summary>
    public class PreprocessBuildWithSubmissionInfo : IPreprocessBuildWithReport
    {
        public int callbackOrder => 0;

        public void OnPreprocessBuild(BuildReport report)
        {
            // Call your function here that needs to be executed before each build
            ExecuteBeforeBuild();
        }

        private void ExecuteBeforeBuild()
        {
            // This function will be called automatically before each build
            // You can perform any necessary tasks such as preparing data, modifying settings, etc.
            LoadSubmissionSettingsFromAsset();
        }

        private void LoadSubmissionSettingsFromAsset()
        {
            // Define the folder path relative to the Application data path
            var assetPath = "Assets/XRC/StudentInfo.asset";

            // Load the ScriptableObject from the specified file path
            var studentSubmissionSettings = AssetDatabase.LoadAssetAtPath<StudentSubmissionSettings>(assetPath);

            // Check if the ScriptableObject was successfully loaded
            if (studentSubmissionSettings != null)
            {
                PlayerSettings.companyName = studentSubmissionSettings.m_CompName;
                PlayerSettings.productName = studentSubmissionSettings.m_ProdName;
                PlayerSettings.applicationIdentifier = studentSubmissionSettings.m_AppIdentifier;
            }
            else
            {
                Debug.LogError(
                    "LoadSubmissionSettingsFromAsset: Please make sure you fill out StudentSubmissionSettings file in Assets/XRC/StudentInfo.asset.");

                AssetDatabase.CreateAsset(studentSubmissionSettings, assetPath);
            }
        }
    }
}