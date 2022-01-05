using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class People : MonoBehaviour
{
    public bool move = false;
    
    private Animator anim;
    private GameObject character;
    float speed = 1.0f;

    [SerializeField] private Transform target;
    [SerializeField] float RotSpeed = 10f;
 
    //values for internal use
    private Quaternion lookRotation;
    private Vector3 direction;
    private Vector3 newPosition;
    private Vector3 oldPosition;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        character = GetComponent<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {

        oldPosition = transform.position;
        if ((oldPosition - newPosition).magnitude != 0)
        {
            anim.SetBool("isRunning",true);
        }
        else
        {
            anim.SetBool("isRunning",false);
        }
        newPosition = transform.position;
        if (!move) return;
        //find the vector pointing from our position to the target
        direction = (target.position - transform.position).normalized;
 
        //create the rotation we need to be in to look at the target
        if (direction != Vector3.zero)
        {
            lookRotation = Quaternion.LookRotation(direction);
        };
        
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * RotSpeed);
        
        float step =  speed * Time.deltaTime; // calculate distance to move
        transform.position = Vector3.MoveTowards(transform.position, target.position, step);
        
        
    }
}
