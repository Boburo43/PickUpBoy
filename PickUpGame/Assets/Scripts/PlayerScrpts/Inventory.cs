using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [SerializeField] private int maxSlots = 1;
    [SerializeField] private List<Pickupable> items = new List<Pickupable>();
    [SerializeField] private SkinnedMeshRenderer skinnedMeshRenderer;

    public float animationDuration = 0.5f; // Time to open/close lid
    public float itemDelayRatio = 0.5f;     // Start item blendshape halfway through open

    private bool isAnimating = false;

    private void Awake()
    {
        if (skinnedMeshRenderer == null)
            skinnedMeshRenderer = GetComponentInChildren<SkinnedMeshRenderer>();

        skinnedMeshRenderer.SetBlendShapeWeight(0, 0); // open
        skinnedMeshRenderer.SetBlendShapeWeight(1, 0); // itemInside
    }

    public bool AddItem(Pickupable item)
    {
        if (items.Count >= maxSlots || isAnimating)
        {
            Debug.Log("Inventory full or busy");
            return false;
        }

        items.Add(item);
        item.gameObject.SetActive(false);

        StartCoroutine(AnimateItemSequence(true)); // true = adding
        return true;
    }

    public Pickupable RetrieveItem()
    {
        if (items.Count == 0 || isAnimating)
        {
            Debug.Log("Inventory empty or busy");
            return null;
        }

        Pickupable item = items[items.Count - 1];
        items.RemoveAt(items.Count - 1);

        StartCoroutine(AnimateItemSequence(false, item)); // false = retrieving
        return item;
    }

    private IEnumerator AnimateItemSequence(bool adding, Pickupable retrieveItem = null)
    {
        isAnimating = true;

        float elapsed = 0f;

        // --- OPENING ---
        while (elapsed < animationDuration)
        {
            float t = elapsed / animationDuration;
            skinnedMeshRenderer.SetBlendShapeWeight(0, t * 100f); // Open lid

            if (adding && t >= itemDelayRatio)
            {
                float itemT = (t - itemDelayRatio) / (1f - itemDelayRatio);
                skinnedMeshRenderer.SetBlendShapeWeight(1, itemT * 100f); // Item in
            }
            else if (!adding && t >= itemDelayRatio)
            {
                float itemT = 1f - ((t - itemDelayRatio) / (1f - itemDelayRatio));
                skinnedMeshRenderer.SetBlendShapeWeight(1, itemT * 100f); // Item out
            }

            elapsed += Time.deltaTime;
            yield return null;
        }

        skinnedMeshRenderer.SetBlendShapeWeight(0, 100f);
        skinnedMeshRenderer.SetBlendShapeWeight(1, adding ? 100f : 0f);

        if (!adding && retrieveItem != null)
            retrieveItem.gameObject.SetActive(true);

        // --- CLOSING ---
        elapsed = 0f;
        while (elapsed < animationDuration)
        {
            float t = elapsed / animationDuration;
            skinnedMeshRenderer.SetBlendShapeWeight(0, (1f - t) * 100f); // Close lid

            elapsed += Time.deltaTime;
            yield return null;
        }

        skinnedMeshRenderer.SetBlendShapeWeight(0, 0f); // Lid fully closed
        isAnimating = false;
    }
}
