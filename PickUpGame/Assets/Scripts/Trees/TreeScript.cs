using UnityEngine;

public class TreeSpawner : MonoBehaviour
{
    public GameObject[] treePrefabs; // Assign different tree prefabs in inspector
    public int numberOfTrees = 100;
    public Vector2 areaSize = new Vector2(50, 50);

    void Start()
    {
        for (int i = 0; i < numberOfTrees; i++)
        {
            SpawnTree();
        }
    }

    void SpawnTree()
    {
        // Choose a random prefab
        GameObject treePrefab = treePrefabs[Random.Range(0, treePrefabs.Length)];

        // Random position within area
        Vector3 position = new Vector3(
            Random.Range(-areaSize.x / 2, areaSize.x / 2),
            0,
            Random.Range(-areaSize.y / 2, areaSize.y / 2)
        );

        // Instantiate tree
        GameObject tree = Instantiate(treePrefab, position, Quaternion.identity);

        // Random rotation (y-axis)
        float randomYRotation = Random.Range(0f, 360f);
        tree.transform.Rotate(0f, randomYRotation, 0f);

        // Random scale
        float scale = Random.Range(0.8f, 1.3f); // Adjust as needed
        tree.transform.localScale = new Vector3(scale, scale, scale);

        // Optional: Slight color variation (if material supports it)
        Renderer rend = tree.GetComponentInChildren<Renderer>();
        if (rend != null)
        {
            Material mat = rend.material;
            Color baseColor = mat.color;
            float variation = 0.05f;
            mat.color = new Color(
                baseColor.r + Random.Range(-variation, variation),
                baseColor.g + Random.Range(-variation, variation),
                baseColor.b + Random.Range(-variation, variation)
            );
        }
    }
}
