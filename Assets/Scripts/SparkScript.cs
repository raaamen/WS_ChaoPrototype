using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class SparkScript : MonoBehaviour
{
    public bool fighting;

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
            //Debug.Log("Moving from "+transform.position+" to "+currentTarget.transform.position);
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
    

    IEnumerator FightSpark(){
        fighting=true;
        Debug.Log("Fighting");
        GameObject.Find("PrototypeManager").GetComponent<PrototypeManager>().AssistSparks();
        fightCloud.SetActive(true);

        //helpButton.gameObject.SetActive(true);

        yield return new WaitForSeconds(fightTime);

        //Help out chao here, raises percentage of win

        if (currentTarget != null){
            var win = CalculateWin(sparkLevel, currentTarget.GetComponent<RotScript>().rotLevel);
            if (win){
            Debug.Log("Spark wins");
            currentTarget.GetComponent<RotScript>().rotLevel--;
                if (currentTarget.GetComponent<RotScript>().rotLevel <= 0){
                    Destroy(currentTarget);
                    fightCloud.SetActive(false);
                    fighting=false;
                    yield return null;
                }
            }
            else {
                Debug.Log("Spark loses");
                currentTarget.GetComponent<RotScript>().rotLevel--;
                sparkLevel--;
                if (currentTarget.GetComponent<RotScript>().rotLevel <= 0){
                        Destroy(currentTarget);
                        fightCloud.SetActive(false);
                        fighting=false;
                        yield return null;
                    
                    }
            }
        }
        
        currentTarget=null;

        fightCloud.SetActive(false);
        fighting=false;
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

    bool CalculateWin(int spark, int rotLevel){

        if (spark > rotLevel){
            //spark wins
            return true;
        }
        else if (spark == rotLevel){
            //spark loses because it's attacking
            return false;
        }
        
        else return false;

        /*

        float sparkRateToWin = 0;


        if (spark == rotLevel){
            //if they're the same level, win rate is 50%
            sparkRateToWin = 0.5f;
        }

        else if (spark > rotLevel){
            
        }

        var random = UnityEngine.Random.Range(0, 1);
        if (random >= sparkRateToWin){
            return true;
        }
        else return false;
        */
    }


}
