using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;

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
        // Add your logic here for pre-build actions
        // This function will be called automatically before each build
        // You can perform any necessary tasks such as preparing data, modifying settings, etc.


        LoadSettings();

    }
    
    
    void SaveSettings()
    {
        // PlayerSettings.companyName = m_CompName;
        // PlayerSettings.productName = m_ProdName;
        // PlayerSettings.applicationIdentifier = m_AppIdentifier;
        // PlayerSettings.SetApplicationIdentifier(BuildTargetGroup.Android, m_AppIdentifier);
        //
        // EditorPrefs.SetString("CompName", m_CompName);
        // EditorPrefs.SetString("ProdName", m_ProdName);
        // EditorPrefs.SetString("App ID", m_AppIdentifier);
    }

    void LoadSettings()
    {
        // Define the folder path relative to the Application data path
        // string folderPath = Application.dataPath + "/MyFolder";

        // Define the full file path
        // string filePath = folderPath + "/MyScriptableObject.asset";

        
        string assetPath = "Assets/XRC/StudentInfo.asset";
        
        
        // Load the ScriptableObject from the specified file path
        StudentSubmissionSettings studentSubmissionSettings = UnityEditor.AssetDatabase.LoadAssetAtPath<StudentSubmissionSettings>(assetPath);

        // Check if the ScriptableObject was successfully loaded
        if (studentSubmissionSettings != null)
        {

            // PlayerSettings.companyName = studentSubmissionSettings.m_CompName;
            // PlayerSettings.productName = studentSubmissionSettings.m_ProdName;
            // PlayerSettings.applicationIdentifier = studentSubmissionSettings.m_AppIdentifier;

            // m_CompName = studentSubmissionSettings.
            // Retrieve data from the ScriptableObject and print it to the console
            // Debug.Log("ScriptableObject data:");
            // Debug.Log("Name: " + myScriptableObject.name);
            // Debug.Log("Value: " + myScriptableObject.value);
        }
        else
        {
            // Debug.LogError("ScriptableObjectDataPrinter: Failed to load the ScriptableObject!");
        }
    }
    
    
}