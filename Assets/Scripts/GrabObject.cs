using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabObject : MonoBehaviour {
	// Referencia protese

	private GameObject referenceObject;
	private ProsthesisControl prosthesisControl;
	private armControl control;
	private Rigidbody rigid;

	// 
	private bool firstFingerIn;
	private bool firstFingerOut;
	private bool otherFingerIn;
	private bool otherFingerOut;
	//
	//public GameObject[] dedos;
	private GameObject otherFinger;
	private GameObject root;

	// Determina qual o estado do movimento grab
	// 0 - Sem colisão com os dedos 
	// 1 - colidiu com 1 dos dedos
	// 2 - colidiu com os 2 dedos (executa a função de agarrar o objeto)
	private int estado;

	private List<GameObject> dedos = new List<GameObject>();

	void Start () {
		firstFingerIn = false;
		firstFingerOut = false;
		otherFingerIn = false;
		otherFingerOut = false;

		estado = 0;

		referenceObject = GameObject.Find ("Protese");
		prosthesisControl = referenceObject.GetComponent<ProsthesisControl> ();
		control = GameObject.Find("Protese").GetComponent<armControl>();
		root = this.transform.root.gameObject;

		/*dedos = new GameObject[2];
		dedos [0] = GameObject.Find("indicador_02");
		dedos [1] = GameObject.Find("polegar_02");*/


	}

	void Update () {
	
	}

	void OnTriggerEnter(Collider col) {
		//Debug.Log("Entrou: "+col.gameObject);
		if (!col.gameObject.tag.Equals("Considered"))
			return;
		
		if(!dedos.Contains(col.gameObject) && dedos.Count < 2)
			dedos.Add(col.gameObject);

		Debug.Log("Dedos:"+dedos.Count);
		if(dedos.Count == 2)
			LinkObject();
		
	}

	void OnTriggerExit(Collider col) {
		//Debug.Log("Saiu: "+col.gameObject);
		if (col.gameObject.tag != "Considered")
			return;
		Debug.Log("Dedos:"+dedos.Count);
		if(dedos.Contains(col.gameObject)){
			if(dedos.Count == 2)
				UnlinkObject();
			dedos.Remove(col.gameObject);
		}
		
	}

	// Torna o objeto filho da protese e remove o RigidBody
	void LinkObject(){
		//if(!prosthesisControl.isProsthesisOpened()){
			//rigid = root.GetComponent<Rigidbody>();
			//Destroy (rigid);
			GetComponent<Rigidbody>().useGravity = false;
			GetComponent<Rigidbody>().constraints= RigidbodyConstraints.FreezeAll;
			GetComponentInChildren<BoxCollider>().enabled = false;
			root.transform.parent = referenceObject.transform;
			control.SetFreezeStatus(true, prosthesisControl.lastStatus);
	}

	// Remove a filiação do objeto e adiciona o RigidBody ao objeto
	public void UnlinkObject(){
		GetComponent<Rigidbody>().useGravity = true;
		GetComponent<Rigidbody>().constraints= RigidbodyConstraints.None;
		GetComponentInChildren<BoxCollider>().enabled = true;
		root.transform.parent = null;
		control.SetFreezeStatus(false, 101);
		//if (root.GetComponent<Rigidbody>() == null)
		//	root.AddComponent<Rigidbody> ();
	}

	/// <summary>
	/// Gets the estado.
	/// </summary>
	/// <returns>Estado da prótese.</returns>
	public int getEstado(){
		return estado;
	}
}
