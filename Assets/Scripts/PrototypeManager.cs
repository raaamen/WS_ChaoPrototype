using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PrototypeManager : MonoBehaviour
{


    

    //Notes
    //auto battler but make the player feel like they're helping

    public TMP_Text battleText;

    //UI
    
    public GameObject[] chaoList;

    public GameObject[] rotList;
    
    
    [SerializeField]
    private int playerHealth;

    [SerializeField]
    private bool combatStarted;

    public bool sparkBattling;
    public bool rotBattling;

    
    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        CollectRotAndSparks();
    }
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        CollectRotAndSparks();
        CheckBattleStatusRot();
        CheckBattleStatusSpark();   
    }

    public void CheckBattleStatusSpark(){
        CollectRotAndSparks();
        int amtDoneFighting = 0;
        foreach (var item in chaoList)
        {
            if (item.GetComponent<SparkScript>().fighting==false) amtDoneFighting++;
        }
        if (amtDoneFighting == chaoList.Length){
            sparkBattling=false;
        }
        Debug.Log("Sparks are done fighting");
    }

    public void CheckBattleStatusRot(){
        CollectRotAndSparks();
        int amtDoneFighting = 0;
        foreach (var item in rotList)
        {
            if (item.GetComponent<RotScript>().fighting==false) amtDoneFighting++;
        }
        if (amtDoneFighting == rotList.Length){
            rotBattling = false;
        }
        Debug.Log("Rots are done fighting");
    }

    public void StartSparkCombat(){
        sparkBattling = true;
        Debug.Log("Combat started");
        
        CollectRotAndSparks();
        
        if (chaoList.Length==0){
            return;
        }

        foreach (var item in chaoList)
        {



            Debug.Log(item.gameObject.name);
            switch(item.GetComponent<SparkScript>().attackType){
                case SparkScript.AttackType.strongest:

                item.GetComponent<SparkScript>().currentTarget = ReturnStrongestRot();

                break;

                case SparkScript.AttackType.weakest:

                item.GetComponent<SparkScript>().currentTarget = ReturnWeakestRot();

                break;

                case SparkScript.AttackType.farthest:

                item.GetComponent<SparkScript>().currentTarget = ReturnFarthestRot(item);

                break;
                case SparkScript.AttackType.closest:
                item.GetComponent<SparkScript>().currentTarget = ReturnClosestRot(item);
                break;
                case SparkScript.AttackType.nothing:
                item.GetComponent<SparkScript>().fighting=false;
                break;
            }
        }
    }

    

    public GameObject ReturnStrongestRot(){
        if (rotList.Length==0){
            return null;
        }
        var highestLevel = 0;
        GameObject highestRot = null;
        foreach (var item in rotList)
        {
            if (item.GetComponent<RotScript>().rotLevel > highestLevel){
                highestLevel = item.GetComponent<RotScript>().rotLevel;
                highestRot = item;
            }   
        }
        Debug.Log("Weakest Rot: "+highestRot.gameObject.name+" at level "+highestRot.GetComponent<RotScript>().rotLevel);
        return highestRot;
    }

    public GameObject ReturnWeakestRot(){
        if (rotList.Length==0){
            return null;
        }

        var lowestLevel = rotList[0].GetComponent<RotScript>().rotLevel;
        GameObject lowestRot = rotList[0];
        foreach (var item in rotList)
        {
            if (item.GetComponent<RotScript>().rotLevel < lowestLevel){
                lowestLevel = item.GetComponent<RotScript>().rotLevel;
                lowestRot = item;
            }   
        }
        Debug.Log("Weakest Rot: "+lowestRot.gameObject.name+" at level "+lowestRot.GetComponent<RotScript>().rotLevel);
        return lowestRot;
    }
    
    public GameObject ReturnFarthestRot(GameObject spark){
        if (rotList.Length==0){
            return null;
        }
        GameObject farthestRot = null;
        float maxdistance = 0;
        float distance = 0;

        foreach (var item in rotList){
            distance = Vector3.Distance (item.transform.position, spark.transform.position);
            if (distance > maxdistance){
                maxdistance = distance;
                farthestRot = item;
            }

        }
        Debug.Log("Farthest Rot: "+farthestRot.gameObject.name+" at distance "+Vector3.Distance(spark.transform.position, farthestRot.transform.position));
        return farthestRot;
    }

    public GameObject ReturnClosestRot(GameObject spark){
        if (rotList.Length==0){
            return null;
        }

        
        GameObject closest = null;
        float mindistance = Vector3.Distance (rotList[0].transform.position, spark.transform.position);
        float distance = 0;

        foreach (var item in rotList){
            distance = Vector3.Distance (item.transform.position, spark.transform.position);
            if (distance < mindistance){
                mindistance = distance;
                closest = item;
            }

        }
        Debug.Log("Closest Rot: "+closest.gameObject.name+" at distance "+Vector3.Distance(spark.transform.position, closest.transform.position));
        return closest;

    }

    public void CollectRotAndSparks(){
        rotList = GameObject.FindGameObjectsWithTag("Rot");
        chaoList = GameObject.FindGameObjectsWithTag("Spark");
    }

    public void AssistSparks(){

    }

    

    public void StartRotCombat(){
        foreach (var item in rotList)
        {
            //chooses random spark
            GameObject randomTarget = chaoList[Random.Range(0,chaoList.Length)];
            item.GetComponent<RotScript>().target = randomTarget;
        }
    }

    public void ButtonStartCoroutine(string coroutine){
        StartCoroutine(coroutine);
    }

    public IEnumerator SparkBattle(){
        CollectRotAndSparks();
        battleText.text = "Spark Battling";
        StartSparkCombat();
        yield return new WaitUntil(() => sparkBattling == false);
        Debug.Log("Spark Battling False");
        CollectRotAndSparks();
        battleText.text = "Rot Battling";
        yield return new WaitForSeconds(2);
        StartRotCombat();
        yield return new WaitUntil(() => rotBattling == false);
        battleText.text = "Battle Done";

        yield return null;
    }

}
