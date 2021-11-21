using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class exhaustController : MonoBehaviour
{
    public int live = 999;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        live--;
        if (live == 0)
            Destroy(gameObject);
    }
}
