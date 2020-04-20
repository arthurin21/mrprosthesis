using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class armControl : MonoBehaviour
{
    private ProsthesisControl _hand;
	
	private float maxAperture, minAperture;
	private bool maxConfigured, minConfigured;

	private ViveInputs inputs;
	private bool isFreeze;

	private float frozenValue;

	// Use this for initialization
	void Start () {
        _hand = GameObject.Find("Protese").GetComponent<ProsthesisControl>();
		_hand.LoadBones ();
		
		inputs= GameObject.Find("Actions").GetComponent<ViveInputs>();

		maxAperture = 0;
		minAperture = 0;
		maxConfigured = false;
		minConfigured = false;
		isFreeze=false;
		frozenValue = 101;
    }

	void Update ()
	{
		float triggerValue= inputs.GetTriggerValue()*100;
        _hand.setRotation(triggerValue);
		//Debug.Log("ArmControl-trigger value:"+inputs.GetTriggerValue()*100);
        if(triggerValue < frozenValue )
				_hand.updateRotation();
	}

	public void SetFreezeStatus(bool status, float value){
		isFreeze = status;
		frozenValue = value;
	}

	//Determina quanto o usuário consegue mover o braço para frente levando o potenciometro ao ponto maximo
	void setMaxAperture(float max){
		this.maxAperture =max;
		maxConfigured = true;
	}

	//Determina a posição de descanso e seu valor no potenciometro
	void setMinAperture(float min){
		this.minAperture = min;
		minConfigured = true;
	}
}
