using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class ViveGameController : MonoBehaviour
{
    public SteamVR_Action_Boolean myAction;
    public Hand hand;
    public GameObject projectile;

    // Called whenever a vive controller appears
    private void OnEnable(){
        if (hand == null)
            hand = this.GetComponent<Hand>();
        
        if (myAction == null){
            Debug.LogError("<b>[SteamVR Interaction]</b> No plant action assigned");
            return;
        }
        myAction.AddOnChangeListener(OnMyActionChange, hand.handType);
    }

    private void OnDisable() {
        if (myAction != null)
            myAction.RemoveOnChangeListener(OnMyActionChange, hand.handType);
    }

    private void OnMyActionChange(SteamVR_Action_Boolean actionIn, SteamVR_Input_Sources input_Sources, bool newValue){
        if (newValue == true){
            Instantiate(projectile, hand.transform.position, hand.transform.rotation);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
