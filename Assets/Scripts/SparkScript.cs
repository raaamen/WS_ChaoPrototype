using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class SparkScript : MonoBehaviour
{

    public Button helpButton;

    public float fightTime;
    public GameObject fightCloud;

    public int sparkLevel;

    public TMP_Text sparkLevelText;

    public TMP_Dropdown dropdown;

    public Vector3 startingSpot;

    public enum AttackType{
        strongest,
        weakest,
        farthest,
        closest,
        nothing
    }

    public AttackType attackType;

    public GameObject currentTarget;
    public float speed;

    // Start is called before the first frame update
    void Start()
    {
        startingSpot = transform.position;
        InitAttackType();
    }

    // Update is called once per frame
    void Update()
    {
        sparkLevelText.text = "Spark Level: "+sparkLevel;
        if (currentTarget != null){
            Debug.Log("Current Target "+currentTarget.gameObject.name);
            //transform.position = currentTarget.transform.position;
            Debug.Log("Moving from "+transform.position+" to "+currentTarget.transform.position);
            var step = speed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, currentTarget.transform.position, step);
            if (transform.position == currentTarget.transform.position){
                StartCoroutine("FightSpark");
            }
            
        }
        else{
            var step = speed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, startingSpot, step);
            
        }
    }


    /// <summary>
    /// Sent when an incoming collider makes contact with this object's
    /// collider (2D physics only).
    /// </summary>
    /// <param name="other">The Collision2D data associated with this collision.</param>
    void OnCollisionEnter2D(Collision2D other)
    {
        Debug.Log("Colliding with "+other.gameObject.name);
        if (other.gameObject == currentTarget){
            
            //StartCoroutine("FightSpark");
            
        }
    }

    IEnumerator FightSpark(){
        Debug.Log("Fighting");
        GameObject.Find("PrototypeManager").GetComponent<PrototypeManager>().AssistSparks();
        fightCloud.SetActive(true);

        //helpButton.gameObject.SetActive(true);

        yield return new WaitForSeconds(fightTime);

        //Help out chao here, raises percentage of win




        
        //this can be percentage based
        if (sparkLevel >= currentTarget.GetComponent<RotScript>().rotLevel){
            Debug.Log("Spark wins");
            Destroy(currentTarget);
            currentTarget=null;
        }
        else {
            Debug.Log("Spark loses");
            currentTarget.GetComponent<RotScript>().rotLevel--;
            sparkLevel--;
            currentTarget=null;
        }
        fightCloud.SetActive(false);
        yield return null;
    }

    public void ChangeLevel(int change){
        sparkLevel+=change;
    }
    
    public void ChangeAttackType(){
        Debug.Log("Attack Type Changed");
        switch(dropdown.value){
            case 0:
            attackType = AttackType.strongest;
            break;
            case 1:
            attackType = AttackType.weakest;
            break;
            case 2:
            attackType = AttackType.closest;
            break;
            case 3:
            attackType = AttackType.farthest;
            break;
            case 4:
            attackType = AttackType.nothing;
            break;
        }
    }

    public void InitAttackType(){
        switch(attackType){
            case AttackType.strongest:
            dropdown.value = 0;
            break;
            case AttackType.weakest:
            dropdown.value = 1;
            break;
            case AttackType.farthest:
            dropdown.value = 2;
            break;
            case AttackType.closest:
            dropdown.value = 3;
            break;
            case AttackType.nothing:
            dropdown.value = 4;
            break;
        }
    }

    bool CalculateWin(int sparkLevel, int rotLevel){

        return false;
    }


}
