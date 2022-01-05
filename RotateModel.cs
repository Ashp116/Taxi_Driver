using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateModel : MonoBehaviour
{
    private float y = 15;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        gameObject.transform.Rotate(0f, y * Time.deltaTime,0f);
        y = y++;
    }
}
