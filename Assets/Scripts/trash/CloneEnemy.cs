using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloneEnemy : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject rootObj;

    void Start()
    {   
        for(int i=0; i<GameManager.I.stage *4; i++)
        {
            GameObject duplicate = Instantiate(rootObj);
        }
        
    }
}
