using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RotScript : MonoBehaviour
{
    public int rotLevel;

    public TMP_Text rotLevelText;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        rotLevelText.text = "Rot Level: "+rotLevel;
    }

    public void ChangeLevel(int change){
        rotLevel+=change;
    }
    
}
