using UnityEngine;


namespace XRC.Core
{

    /// <summary>
    /// Represents a pose marker in the scene that visualizes the pose in 3D space.
    /// </summary>
    public class PoseMarker : MonoBehaviour
    {
        private Material m_BlueMaterial;
        private Material m_DefaultMaterial;
        private Material m_GreenMaterial;
        private Material m_RedMaterial;

        private GameObject m_XAxis;

        private GameObject m_XPositive;
        private GameObject m_YAxis;
        private GameObject m_YPositive;
        private GameObject m_ZAxis;
        private GameObject m_ZPositive;

        /// <summary>
        /// Handles the event when the pose marker is enabled or disabled.
        /// </summary>
        /// <param name="isEnabled">Indicates whether the pose marker is enabled.</param>

        private void HandlePoseMarkerEnabledChanged(bool isEnabled)
        {
            // Handle the pose marker enabled changed event
            if (isEnabled)
            {
                Debug.Log("Pose marker is enabled!");
                ShowPoseMarker();
            }
            else
            {
                Debug.Log("Pose marker is disabled!");
                HidePoseMarker();
            }
        }

        /// <summary>
        /// Initializes the pose marker on Start.
        /// </summary>
        private void Start()
        {
            CreateMaterials();
            CreatePoseMarker();
        }

        /// <summary>
        /// Shows the pose marker by enabling the necessary game objects.
        /// </summary>
        public void ShowPoseMarker()
        {
            m_XAxis.GetComponent<MeshRenderer>().enabled = true;
            m_YAxis.GetComponent<MeshRenderer>().enabled = true;
            m_ZAxis.GetComponent<MeshRenderer>().enabled = true;

            m_XPositive.GetComponent<MeshRenderer>().enabled = true;
            m_YPositive.GetComponent<MeshRenderer>().enabled = true;
            m_ZPositive.GetComponent<MeshRenderer>().enabled = true;
        }

        /// <summary>
        /// Hides the pose marker by disabling the necessary game objects.
        /// </summary>
        public void HidePoseMarker()
        {
            m_XAxis.GetComponent<MeshRenderer>().enabled = false;
            m_YAxis.GetComponent<MeshRenderer>().enabled = false;
            m_ZAxis.GetComponent<MeshRenderer>().enabled = false;

            m_XPositive.GetComponent<MeshRenderer>().enabled = false;
            m_YPositive.GetComponent<MeshRenderer>().enabled = false;
            m_ZPositive.GetComponent<MeshRenderer>().enabled = false;
        }

        /// <summary>
        /// Creates the necessary game objects to represent the pose marker.
        /// </summary>
        private void CreatePoseMarker()
        {
            // Use the bounds of the renderer
            var rend = GetComponent<Renderer>();
            var bounds = rend.bounds;


            var position = transform.position;
            var rotation = transform.rotation;

            var scaleFactorMax = Mathf.Max(bounds.size.x, bounds.size.y, bounds.size.z) * 1.0f;
            var scaleFactorMin = Mathf.Min(bounds.size.x, bounds.size.y, bounds.size.z) * 1.0f;

            var axisWidth = Mathf.Clamp(scaleFactorMin * 0.05f, 0.01f, 0.2f);
            var indicatorWidth = 2 * axisWidth;

            var axisSize = Mathf.Max(bounds.extents.x, Mathf.Max(bounds.extents.y, bounds.extents.z));


            // Create axis cubes

            m_XAxis = CreateCube(position, rotation, new Vector3(axisSize * 2.5f, axisWidth, axisWidth),
                m_RedMaterial, "xAxis");
            m_YAxis = CreateCube(position, rotation, new Vector3(axisWidth, axisSize * 2.5f, axisWidth),
                m_GreenMaterial, "yAxis");
            m_ZAxis = CreateCube(position, rotation, new Vector3(axisWidth, axisWidth, axisSize * 2.5f),
                m_BlueMaterial, "zAxis");

            // Create positive indicator cubes

            m_XPositive = CreateCube(position + Vector3.right * axisSize * 1.25f, rotation,
                new Vector3(indicatorWidth, indicatorWidth, indicatorWidth), m_RedMaterial, "X-Positive");
            m_YPositive = CreateCube(position + Vector3.up * axisSize * 1.25f, rotation,
                new Vector3(indicatorWidth, indicatorWidth, indicatorWidth), m_GreenMaterial, "Y-Positive");
            m_ZPositive = CreateCube(position + Vector3.forward * axisSize * 1.25f, rotation,
                new Vector3(indicatorWidth, indicatorWidth, indicatorWidth), m_BlueMaterial, "Z-Positive");


            // After PoseMarker has been initialized, parent it to the game object.
            m_XAxis.transform.SetParent(transform);
            m_YAxis.transform.SetParent(transform);
            m_ZAxis.transform.SetParent(transform);
            m_XPositive.transform.SetParent(transform);
            m_YPositive.transform.SetParent(transform);
            m_ZPositive.transform.SetParent(transform);
        }

        /// <summary>
        /// Creates the necessary materials for rendering the pose marker.
        /// </summary>
        private void CreateMaterials()
        {
            // Create red material
            m_RedMaterial = new Material(Shader.Find("Unlit/Color"));
            m_RedMaterial.color = Color.red;

            // Create blue material
            m_BlueMaterial = new Material(Shader.Find("Unlit/Color"));
            m_BlueMaterial.color = Color.blue;

            // Create green material
            m_GreenMaterial = new Material(Shader.Find("Unlit/Color"));
            m_GreenMaterial.color = Color.green;

            m_DefaultMaterial = new Material(Shader.Find("Unlit/Color"));
            m_DefaultMaterial.color = Color.white;
        }

        /// <summary>
        /// Creates a cube game object with the specified parameters.
        /// </summary>
        /// <param name="position">The position of the cube.</param>
        /// <param name="rotation">The rotation of the cube.</param>
        /// <param name="size">The size of the cube.</param>
        /// <param name="material">The material to apply to the cube.</param>
        /// <param name="name">The name of the cube game object.</param>
        /// <returns>The created cube game object.</returns>
        private GameObject CreateCube(Vector3 position, Quaternion rotation, Vector3 size, Material material,
            string name = "cube")
        {
            var cube = new GameObject();
            cube.name = name;
            cube.AddComponent<MeshFilter>().mesh = CreateCubeMesh(size);
            cube.AddComponent<MeshRenderer>();
            cube.transform.position = position;

            // Store the pivot transform and direction vectors.
            var customPivot = transform;
            var targetDirection = transform.forward;
            var cubeDirection = cube.transform.forward;

            // Apply the rotation to align the cube
            var rot = Quaternion.FromToRotation(cubeDirection, targetDirection);
            cube.transform.RotateAround(customPivot.position, transform.up, rot.eulerAngles.y);
            var rend = cube.GetComponent<Renderer>();
            rend.material = material;
            return cube;
        }

        /// <summary>
        /// Creates a cube mesh with the specified size.
        /// </summary>
        /// <param name="size">The size of the cube.</param>
        /// <returns>The created cube mesh.</returns>
        private Mesh CreateCubeMesh(Vector3 size)
        {
            var mesh = new Mesh();

            // Vertices
            var vertices = new Vector3[8];
            var halfSizeX = size.x * 0.5f;
            var halfSizeY = size.y * 0.5f;
            var halfSizeZ = size.z * 0.5f;
            vertices[0] = new Vector3(-halfSizeX, -halfSizeY, -halfSizeZ);
            vertices[1] = new Vector3(halfSizeX, -halfSizeY, -halfSizeZ);
            vertices[2] = new Vector3(halfSizeX, -halfSizeY, halfSizeZ);
            vertices[3] = new Vector3(-halfSizeX, -halfSizeY, halfSizeZ);
            vertices[4] = new Vector3(-halfSizeX, halfSizeY, -halfSizeZ);
            vertices[5] = new Vector3(halfSizeX, halfSizeY, -halfSizeZ);
            vertices[6] = new Vector3(halfSizeX, halfSizeY, halfSizeZ);
            vertices[7] = new Vector3(-halfSizeX, halfSizeY, halfSizeZ);

            // Triangles
            var triangles = new int[36];
            triangles[0] = 0;
            triangles[1] = 4;
            triangles[2] = 5;
            triangles[3] = 0;
            triangles[4] = 5;
            triangles[5] = 1;
            triangles[6] = 1;
            triangles[7] = 5;
            triangles[8] = 6;
            triangles[9] = 1;
            triangles[10] = 6;
            triangles[11] = 2;
            triangles[12] = 2;
            triangles[13] = 6;
            triangles[14] = 7;
            triangles[15] = 2;
            triangles[16] = 7;
            triangles[17] = 3;
            triangles[18] = 3;
            triangles[19] = 7;
            triangles[20] = 4;
            triangles[21] = 3;
            triangles[22] = 4;
            triangles[23] = 0;
            triangles[24] = 4;
            triangles[25] = 7;
            triangles[26] = 6;
            triangles[27] = 4;
            triangles[28] = 6;
            triangles[29] = 5;
            triangles[30] = 3;
            triangles[31] = 0;
            triangles[32] = 1;
            triangles[33] = 3;
            triangles[34] = 1;
            triangles[35] = 2;

            mesh.vertices = vertices;
            mesh.triangles = triangles;

            return mesh;
        }
    }

}