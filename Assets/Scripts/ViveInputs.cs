using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class ViveInputs : MonoBehaviour
{

    public SteamVR_Action_Single  triggerAction;
     float triggerValue= 0.0f;

    // Update is called once per frame
    void Update()
    {
        // Get how much the trigger was pressed
        triggerValue = triggerAction.GetAxis(SteamVR_Input_Sources.Any);

        //if(triggerValue > 0.0f)
        //    Debug.Log("Trigger Value: " + triggerValue);
    }

    public float GetTriggerValue(){
        return this.triggerValue;
    }
}
