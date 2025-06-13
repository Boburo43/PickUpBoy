using UnityEngine;

public class CutoutObject : MonoBehaviour
{
    private float currentCutoutSize = 0f;
    private float targetCutoutSize = 0f;
    public float cutoutSize = 0.3f;
    public float cutoutFalloff = 0.0f;
    public float lerpSpeed = 5f;

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
        // STEP 1: Check if player is obstructed
        Vector3 from = mainCamera.transform.position;
        Vector3 to = targetObject.position;
        Vector3 dir = (to - from).normalized;
        float dist = Vector3.Distance(from, to);

        RaycastHit[] hits = Physics.RaycastAll(from, dir, dist, wallMask);

        // STEP 2: Set target cutout size based on hit
        targetCutoutSize = hits.Length > 0 ? cutoutSize : 0f;

        // STEP 3: Smoothly transition current size
        currentCutoutSize = Mathf.Lerp(currentCutoutSize, targetCutoutSize, Time.deltaTime * lerpSpeed);

        // STEP 4: Reset all wall objects
        foreach (var obj in GameObject.FindGameObjectsWithTag("Wall"))
        {
            var rend = obj.GetComponent<Renderer>();
            if (rend == null) continue;

            rend.materials = rend.materials; // force instance
            foreach (var mat in rend.materials)
            {
                mat.SetVector("_CutoutPos", cutoutPos);
                mat.SetFloat("_CutoutSize", currentCutoutSize);
                mat.SetFloat("_FalloffSize", cutoutFalloff);
            }
        }
    }
}
