using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WorldUIManager : MonoBehaviour
{
    public Image prefabUI;

    private Image uiUse;
    // Start is called before the first frame update
    void Start()
    {
        prefabUI = Resources.Load<Image>("LocationPointer");

        uiUse = Instantiate(prefabUI, FindObjectOfType<Canvas>().transform).GetComponent<Image>();
        
        uiUse.transform.position = Camera.main.WorldToScreenPoint(transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        if (Camera.main.WorldToScreenPoint(transform.position).z >= 0)
        {
            uiUse.transform.position = Camera.main.WorldToScreenPoint(transform.position);
        }
        
    }
}
