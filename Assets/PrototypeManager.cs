using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrototypeManager : MonoBehaviour
{

    

    //Notes
    //auto battler but make the player feel like they're helping


    //UI
    
    public GameObject[] chaoList;

    public GameObject[] rotList;
    
    
    [SerializeField]
    private int playerHealth;

    [SerializeField]
    private bool combatStarted;

    
    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        rotList = GameObject.FindGameObjectsWithTag("Rot");
        chaoList = GameObject.FindGameObjectsWithTag("Spark");
    }
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartCombat(){
        Debug.Log("Combat started");
        combatStarted = true;
        CollectRotAndSparks();
        

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
                break;
                case SparkScript.AttackType.closest:
                break;
                case SparkScript.AttackType.nothing:

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
        Debug.Log(highestRot.GetComponent<RotScript>().rotLevel);
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
        Debug.Log(lowestRot.GetComponent<RotScript>().rotLevel);
        return lowestRot;
    }
    public GameObject ReturnFarthestRot(GameObject spark){
        if (rotList.Length==0){
            return null;
        }
        return null;
    }
    public GameObject ReturnClosestRot(GameObject spark){
        if (rotList.Length==0){
            return null;
        }
        return null;
    }

    public void CollectRotAndSparks(){
        rotList = GameObject.FindGameObjectsWithTag("Rot");
        chaoList = GameObject.FindGameObjectsWithTag("Spark");
    }

}
