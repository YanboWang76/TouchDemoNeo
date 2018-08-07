using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstantiateWaterController : MonoBehaviour {

    public int StatusFlag;
    public GameObject WaterDrop;
    public GameObject WaterDropLarge;
    private int FrameCount=0;

    public Transform InstantiateReference1, InstantiateReference2;

	// Use this for initialization
	void Start () {
        SetStatusClose();
	}
	
	// Update is called once per frame
	void Update () {


        FrameCount++;
        


        //if(Input.GetKeyDown(KeyCode.Space))
        //      {
        //          SpaceFlag = !SpaceFlag;
        //      }
        //      if(Input.GetKeyDown(KeyCode.I))
        //      {
        //          IFlag = !IFlag;
        //      }
        if (StatusFlag == 1)
        {
            if (FrameCount % 60 == 0)
            {
                Instantiate(WaterDropLarge, transform.position, transform.rotation);
                FrameCount = 0;
            }
            //Instantiate(WaterDrop, transform.position, transform.rotation);
        }
        if (StatusFlag == 2)
        {
            Instantiate(WaterDrop, transform.position, transform.rotation);
            Instantiate(WaterDrop, InstantiateReference1.position, InstantiateReference1.rotation);
            Instantiate(WaterDrop, InstantiateReference2.position, InstantiateReference2.rotation);
        }
    }

    public void SetStatusClose()
    {
        StatusFlag = 0;
    }

    public void SetStatueMiddle()
    {
        StatusFlag = 1;
    }

    public void SetStatusLarge()
    {
        StatusFlag = 2;
    }

    IEnumerator WaitAndInstantiate(float waittime)
    {
        yield return new WaitForSeconds(waittime);
        Instantiate(WaterDrop, transform.position, transform.rotation);
    }

}