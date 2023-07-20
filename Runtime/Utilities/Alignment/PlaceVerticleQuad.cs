using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlaceVerticleQuad : MonoBehaviour
{

    // public CreateQuadFromPoints quad; 
    public GameObject m_PoseMarkerPrefab; 
    public GameObject m_RightController;

    public Material quadMaterial;

    private GameObject m_CurrentPlane;
    private Vector3 m_PointA;
    private Vector3 m_PointB;

    private Vector3 m_PointC;
    private Vector3 m_PointD;
    
    
    private bool m_Pressed;
    private bool m_Released;

    
    public Vector3 lowerLeft;
    public Vector3 upperRight;
 
    private Vector3 lastLowerLeft;
    private Vector3 lastUpperRight;
 
    private MeshRenderer myMeshRenderer;
    private MeshFilter myMeshFilter;
    private Mesh myMesh;
    
    

    public bool is_verticle = true; 

    [SerializeField]
    [Tooltip("The Input System Action that will be used to read data from the  hand controller. Must be a Value float Control.")]
    private InputActionProperty m_CreatePlaneAction;


    [SerializeField] 
    [Tooltip("The Input System Action that will be used to read data from the hand controller. Must be a Value float Control.")]
    InputActionProperty m_PositionAction;
        


    private GameObject m_CurrentPoseMarker;

    public bool m_IsFirstPress = true; 
    
    private void Start()
    {
        m_CreatePlaneAction.action.started  += ctx =>
        {
            m_Pressed = true;
            m_Released = false;

        };
        m_CreatePlaneAction.action.canceled += ctx =>
        {
            m_Pressed = false;
            m_Released = true;
        };
        
        
        // Create a new game object and attach a mesh renderer and filter
        // m_QuadObject = new GameObject("Quad");
        // m_Renderer = m_QuadObject.AddComponent<MeshRenderer>();
        // m_Filter = m_QuadObject.AddComponent<MeshFilter>();

    }

    /// <summary>
    /// See <see cref="MonoBehaviour"/>.
    /// </summary>
    protected void OnEnable()
    {
        m_CreatePlaneAction.action.Enable();
        m_PositionAction.action.Enable();
        
        
        //Create the needed parts if they're not already attached
        EnsureMeshFilter();
        EnsureMeshRenderer();
        EnsureMesh();
        
    }


    /// <summary>
    /// See <see cref="MonoBehaviour"/>.
    /// </summary>
    protected void OnDisable()
    {

        m_CreatePlaneAction.action.Disable();
        m_PositionAction.action.Disable();


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


    private void Update()
    {
        if (m_Pressed && m_IsFirstPress == true)
        {
            m_PointA = GetWorldPoint();
            lowerLeft = m_PointA;
            m_CurrentPoseMarker = Instantiate(m_PoseMarkerPrefab, Vector3.zero, Quaternion.identity);
            m_IsFirstPress = false; 
        }

        if (m_Pressed && m_IsFirstPress == false)
        {
            m_PointB = GetWorldPoint();
            upperRight = m_PointB; 




        }

        if (m_Released)
        {
            m_IsFirstPress = true; 
        }

        
        bool positionChanged = lastLowerLeft != lowerLeft || lastUpperRight != upperRight;
        if (lastLowerLeft != lowerLeft || lastUpperRight != upperRight) {
            lastLowerLeft = lowerLeft;
            lastUpperRight = upperRight;
        }
 
        if (!positionChanged)
            return;

        Vector3 upperLeft = new Vector3(lowerLeft.x, upperRight.y, lowerLeft.z);
        Vector3 lowerLRight = new Vector3(upperRight.x, lowerLeft.y, upperRight.z);

        

        Vector3[] verts = {
            lowerLeft,
            upperLeft, 
            upperRight,
            lowerLRight 
        };
        
       //  Vector3 center = 0.5f * (upperRight - lowerLeft);
        // Vector3 crossProduct = Vector3.Cross((lowerLeft - center), (lowerLRight - center) );

        //m_CurrentPoseMarker.transform.position = center;
        //m_CurrentPoseMarker.transform.rotation = Quaternion.LookRotation(crossProduct);
            
            
 
        int[] tris = {
            0, 1, 2, //leftmost triangle
            0, 2, 3, //rightmost triangle
            1, 0, 2, //leftmost triangle
            2, 0, 3 //rightmost triangle
        };
 
        myMesh.vertices = verts;
        myMesh.triangles = tris;
        myMesh.RecalculateNormals();
        //create uv's and normals and whatever if you need to

    }
    
    
    private Vector3 GetWorldPoint()
    {


        if (m_RightController != null)
        {
            return m_RightController.transform.position; 
        }
        
        
        return Vector3.zero;
    }



}
