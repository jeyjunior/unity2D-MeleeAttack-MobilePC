using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hit : MonoBehaviour
{
    
    void Start()
    {
        Invoke("Destroy", 0.5f);
    }

    void Destroy() {
        Destroy(this.gameObject);
    }
}
