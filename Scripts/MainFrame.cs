using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class MainFrame : MonoBehaviour
{
    public bool ESignal = false;
    
    // Serialize Fields
    [SerializeField] private GameObject character;
    
    [SerializeField] private Material criminalMaterial;
    [SerializeField] private Material CyborgFemaleMaterial;
    [SerializeField] private Material SkaterFemaleMaterial;
    [SerializeField] private Material SkaterMaleMaterial;
    
    [SerializeField] private Image EUiElement;
    [SerializeField] private TextMeshProUGUI EInfoUiElement;
    
    [SerializeField] private People peopleScript;

    [SerializeField] float RotSpeed = 10f;
    
    private string[] characters = {"Criminal","CyborgFemale","SkaterFemale","SkaterMale"};
    
    private string[] dropOff = {"Apartment", "Mall", "Hotel", "GroceryStore"};
    private string[] pickUp = {"Apartment", "Mall", "Hotel", "GroceryStore"};
    
    private Quaternion lookRotation;
    private Vector3 direction;
    
    private string currentDropOff;
    private string currentPickUp;
    private bool hasPicked;
    private bool isMoving;
    
    private GameObject currentCharacter;
    private GameObject characterMesh;
    private GameObject Taxi;
    
  
    float speed = 1.0f;
    
    void Start()
    {
        Taxi = GameObject.Find("Taxi");
    }

    // Update is called once per frame
    void FixedUpdate()
    {
       

        if (currentDropOff == null)
        {
            currentCharacter = Instantiate(character);
            characterMesh = currentCharacter.transform.GetChild(0).gameObject;
            peopleScript = currentCharacter.GetComponent<People>();
            characterMesh.GetComponent<Renderer>().material = calculateCharacterMaterial();
            
            currentPickUp = calculatePickUp();
            currentDropOff = calculateNextDropOff();

            reroll();
            currentCharacter.transform.position = GameObject.Find(currentPickUp).transform.position;
            
            currentCharacter.AddComponent<PersonPointer>();
            
            
        }
        
        float step =  speed * Time.deltaTime;
       
        
        Transform PickTarget = Taxi.transform;
        Transform DropOffTarget = GameObject.Find(currentDropOff).transform;
        if (ESignal && !hasPicked)
        {
            

            if (!hasPicked && currentCharacter != null)
            {

                if ((PickTarget.position - currentCharacter.transform.position).magnitude > 4.5)
                {
                    ESignal = false;
                    return;
                }
                
                
                EUiElement.enabled = false;
                EInfoUiElement.enabled = false;
                currentCharacter.transform.position = Vector3.MoveTowards(currentCharacter.transform.position,
                    PickTarget.transform.position, step);

                direction = (PickTarget.position - currentCharacter.transform.position).normalized;

                //create the rotation we need to be in to look at the target
                if (direction != Vector3.zero)
                {
                    lookRotation = Quaternion.LookRotation(direction);
                }

                ;
                currentCharacter.transform.rotation = Quaternion.Slerp(currentCharacter.transform.rotation,
                    lookRotation, Time.deltaTime * RotSpeed);

                if ((PickTarget.position - currentCharacter.transform.position).magnitude < .2)
                {
                    characterMesh.GetComponent<SkinnedMeshRenderer>().enabled = false;
                   // currentDropOff = null;
                    peopleScript = null;
                    ESignal = false;
                    hasPicked = true;
                    Destroy (GameObject.Find("PersonPointer(Clone)"));
                    Destroy (currentCharacter.GetComponent<PersonPointer>());
                    GameObject.Find(currentDropOff).AddComponent<WorldUIManager>();
                }

                ;
            }
        }
        else if (hasPicked && ESignal)
        {
            if (!isMoving) currentCharacter.transform.position = Taxi.transform.position;
            if ((currentCharacter.transform.position - DropOffTarget.transform.position).magnitude > 5) return;
            
            characterMesh.GetComponent<SkinnedMeshRenderer>().enabled = true;
            isMoving = true;
            currentCharacter.transform.position = Vector3.MoveTowards(currentCharacter.transform.position,
                DropOffTarget.transform.position, step);

            direction = (DropOffTarget.position - currentCharacter.transform.position).normalized;

            //create the rotation we need to be in to look at the target
            if (direction != Vector3.zero)
            {
                lookRotation = Quaternion.LookRotation(direction);
            }
            
            currentCharacter.transform.rotation = Quaternion.Slerp(currentCharacter.transform.rotation, lookRotation, Time.deltaTime * RotSpeed);

            if ((currentCharacter.transform.position - DropOffTarget.position).magnitude < .2)
            {
                isMoving = false;
                ESignal = false;
                Destroy (GameObject.Find("LocationPointer(Clone)"));
                Destroy (GameObject.Find(currentDropOff).GetComponent<WorldUIManager>());
                Destroy(currentCharacter);
                currentDropOff = null;
                currentPickUp = null;
                hasPicked = false;
               
               
            }
        }
        
        if (Input.GetKey(KeyCode.E) && !hasPicked)
        {
            ESignal = true;
            EUiElement.enabled = false;
            EInfoUiElement.enabled = false;
        }
        else if (Input.GetKey(KeyCode.E) && hasPicked)
        {
            ESignal = true;
            EUiElement.enabled = false;
            EInfoUiElement.enabled = false;
        }
        
        if ((PickTarget.position - currentCharacter.transform.position).magnitude < 4.5 && !hasPicked && !ESignal)
        {
            EUiElement.enabled = true;
            EInfoUiElement.enabled = true;
            EInfoUiElement.text = "Pick Up Passenger";
        }
        else if ((PickTarget.position - currentCharacter.transform.position).magnitude > 4.5 && !hasPicked && !ESignal)
        {
            EUiElement.enabled = false;
            EInfoUiElement.enabled = false;
        }

        if ((DropOffTarget.position - Taxi.transform.position).magnitude < 4.5 && hasPicked && !ESignal)
        {
            EUiElement.enabled = true;
            EInfoUiElement.enabled = true;
            EInfoUiElement.text = "Drop Off Passenger";
        }
        else if ((DropOffTarget.position - Taxi.transform.position).magnitude > 4.5 && hasPicked && !ESignal)
        {
            EUiElement.enabled = false;
            EInfoUiElement.enabled = false;
        }
    }

    void reroll()
    {
        if (currentDropOff == currentPickUp)
        {
            currentDropOff = calculateNextDropOff();
            reroll();
        }
    }
    
    string calculateNextDropOff()
    {
        int dropOffSelection = Random.Range(0, dropOff.Length);
        return dropOff[dropOffSelection];
    }
    
    string calculatePickUp()
    {
        int dropOffSelection = Random.Range(0, pickUp.Length);
        return pickUp[dropOffSelection];
    }
    
    Material calculateCharacterMaterial()
    {
        int characterSelection = Random.Range(0, 4);

        if (characters[characterSelection] == "Criminal")
        {
            return criminalMaterial;
        }
        else if (characters[characterSelection] == "CyborgFemale")
        {
            return CyborgFemaleMaterial;
        }
        else if (characters[characterSelection] == "SkaterFemale")
        {
            return SkaterFemaleMaterial;
        }
        else if (characters[characterSelection] == "SkaterMale")
        {
            return SkaterMaleMaterial;
        }
        
        return SkaterMaleMaterial;
    }
}
