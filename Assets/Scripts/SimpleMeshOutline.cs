using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class SimpleOutlineNormals : MonoBehaviour
{
    [SerializeField] Color outlineColor = Color.white;
    [SerializeField] float outlineSize = 0.002f;

    GameObject outlineObject;

    void Awake()
    {
        CreateOutline();
    }

    void CreateOutline()
    {
        Mesh originalMesh = GetComponent<MeshFilter>().mesh;

        // Criar mesh duplicado
        Mesh outlineMesh = Instantiate(originalMesh);

        Vector3[] verts = outlineMesh.vertices;
        Vector3[] normals = outlineMesh.normals;

        for (int i = 0; i < verts.Length; i++)
        {
            verts[i] += normals[i] * outlineSize;
        }

        outlineMesh.vertices = verts;
        outlineMesh.RecalculateBounds();

        // Criar objeto
        outlineObject = new GameObject("Outline");
        outlineObject.transform.SetParent(transform, false);

        MeshFilter mf = outlineObject.AddComponent<MeshFilter>();
        mf.mesh = outlineMesh;

        MeshRenderer mr = outlineObject.AddComponent<MeshRenderer>();

        Material mat = new Material(Shader.Find("Unlit/Color"));
        mat.color = outlineColor;

        // 🔑 IMPORTANTE
        mat.renderQueue = 2000;
        mr.material = mat;

        // Renderizar atrás
        mr.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
        mr.receiveShadows = false;
    }

    public void SetOutline(bool active)
    {
        if (outlineObject != null)
            outlineObject.SetActive(active);
    }
}