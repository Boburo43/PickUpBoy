using UnityEngine;

public class Vendor : MonoBehaviour
{
    [SerializeField] private GameObject Coin;
    [SerializeField] private GameObject Fish;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject == Coin)
        {
            Destroy(Coin);
            
        }
        }
    }
