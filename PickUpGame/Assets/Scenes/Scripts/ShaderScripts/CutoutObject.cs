using UnityEngine;

public class CutoutObject : MonoBehaviour
{
    [SerializeField] private Transform targetObject;
    [SerializeField] private LayerMask wallMask;

    private Camera mainCamera;
    private Vector2 cutoutPos = new Vector2(0.5f, 0.3f);

    private void Awake()
    {
        mainCamera = GetComponent<Camera>();
    }

    private void Update()
    {
        // STEP 1: Clear all wall materials
        foreach (var obj in GameObject.FindGameObjectsWithTag("Wall"))
        {
            var rend = obj.GetComponent<Renderer>();
            if (rend == null) continue;

            foreach (var mat in rend.materials)
                mat.SetFloat("_CutoutSize", 0f);
        }

        // STEP 2: Check if the player is hidden
        Vector3 from = mainCamera.transform.position;
        Vector3 to = targetObject.position;
        Vector3 dir = (to - from).normalized;
        float dist = Vector3.Distance(from, to);

        RaycastHit[] hits = Physics.RaycastAll(from, dir, dist, wallMask);
        if (hits.Length == 0) return;

        // STEP 3: Apply cutout to only obstructing objects
        foreach (var hit in hits)
        {
            var rend = hit.collider.GetComponent<Renderer>();
            if (rend == null) continue;

            foreach (var mat in rend.materials)
            {
                mat.SetVector("_CutoutPos", cutoutPos);
                mat.SetFloat("_CutoutSize", 0.1f);
                mat.SetFloat("_FalloffSize", 0.05f);
            }
        }
    }
}
