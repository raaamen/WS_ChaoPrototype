using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RotScript : MonoBehaviour
{
    public int rotLevel;
    public bool fighting;

    public TMP_Text rotLevelText;
    public Vector3 startingSpot;

    public GameObject target;
    public GameObject fightCloud;
    public float speed;
    public float fightTime;
    // Start is called before the first frame update
    void Start()
    {
        startingSpot = transform.position;
    }

    // Update is called once per frame
    void Update()
    {

        
        rotLevelText.text = "Rot Level: "+rotLevel;

        if (target != null){
            Debug.Log("Rot Current Target "+target.gameObject.name);
            
            //transform.position = currentTarget.transform.position;
            //Debug.Log("Moving from "+transform.position+" to "+target.transform.position);
            var step = speed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, target.transform.position, step);
            if (transform.position == target.transform.position){
                StartCoroutine("FightRot");
            }
            
        }
        else{
            var step = speed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, startingSpot, step);
            
        }
    }

    public IEnumerator FightRot(){
        fighting=true;
        fightCloud.SetActive(true);
        yield return new WaitForSeconds(fightTime);
        
        if (target != null){
            var win = CalculateWin(target.GetComponent<SparkScript>().sparkLevel, rotLevel);
            if (win){
            Debug.Log("Rot wins");
            target.GetComponent<SparkScript>().sparkLevel--;
                if (target.GetComponent<SparkScript>().sparkLevel <= 0){
                    Destroy(target);
                    fightCloud.SetActive(false);
                    fighting=false;
                    yield return null;
                }
            }
            else {
                Debug.Log("Rot loses");
                target.GetComponent<SparkScript>().sparkLevel--;
                rotLevel--;
                if (target.GetComponent<SparkScript>().sparkLevel <= 0){
                        Destroy(target);
                        fightCloud.SetActive(false);
                        fighting=false;
                        yield return null;
                    
                    }
            }
        }
        
        target=null;

        fightCloud.SetActive(false);
        fighting=false;
        yield return null;
    }

    public void ChangeLevel(int change){
        rotLevel+=change;
    }
    public void ChooseTarget(GameObject spark){

    }

    bool CalculateWin(int spark, int rotLevel){

        if (spark > rotLevel){
            //spark wins
            return false;
        }
        else if (spark == rotLevel){
            //nobody wins
            return false;
        }
        
        else return true;

        //TODO
        /*
        float sparkRateToWin = 0;


        if (spark == rotLevel){
            //if they're the same level, win rate is 50%
            sparkRateToWin = 0.5f;
        }

        else if (rotLevel > spark){

        }

        else if (spark > rotLevel){
            
        }

        var random = UnityEngine.Random.Range(0, 1);
        if (random <= sparkRateToWin){
            return true;
        }
        else return false;
        */
    }
    
}
