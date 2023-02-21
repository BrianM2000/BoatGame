using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class OceanGenerator : MonoBehaviour
{
    Mesh mesh;
    public int xSize;
    public int zSize;
    public float density;
    Vector3[] vertices;
    int[] triangles;
    Material oceanMat;
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<MeshFilter>().mesh = mesh = new Mesh();
        mesh.name = "Test Mesh";
        mesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;
        Generate();
        AssignToMesh();
        /*
        AssetDatabase.CreateAsset(mesh, "Assets\\Mesh.asset");
        AssetDatabase.SaveAssets();
        */
        oceanMat = GetComponent<Renderer>().sharedMaterial;
    }

    void Update() {

        oceanMat.SetFloat("_UnityTime", Time.time);
        
        /*
        for(int i = 0; i < vertices.Length; ++i){
           Vector3 vert = vertices[i];
           vert.y = Mathf.Sin(Time.timeSinceLevelLoad + vertices[i].x);
           vertices[i] = vert;
        }
        */
        

    }

    // Update is called once per frame
	private void Generate () {

		vertices = new Vector3[(xSize + 1) * (zSize + 1)];
		for (int i = 0, z = 0; z <= zSize; z++) {
			for (int x = 0; x <= xSize; x++, i++) {
				vertices[i] = new Vector3(x * density, 0, z * density);
			}
		}

        triangles = new int[xSize * zSize * 6];
		for (int ti = 0, vi = 0, y = 0; y < zSize; y++, vi++) {
			for (int x = 0; x < xSize; x++, ti += 6, vi++) {
				triangles[ti] = vi;
				triangles[ti + 3] = triangles[ti + 2] = vi + 1;
				triangles[ti + 4] = triangles[ti + 1] = vi + xSize + 1;
				triangles[ti + 5] = vi + xSize + 2;
			}
		}

	}

    void AssignToMesh(){
        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        Vector2[] uvs = new Vector2[vertices.Length];

        for (int i = 0; i < uvs.Length; i++)
        {
            uvs[i] = new Vector2(vertices[i].x, vertices[i].z);
        }
        mesh.uv = uvs;

    }
}
