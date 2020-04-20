using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProsthesisControl : MonoBehaviour {
	public float lastStatus=0.0f;
	private BoneData[] indicador, polegar, minimo, medio, anelar;

	 public void LoadBones(){
		indicador = new BoneData[2];
		indicador[0] = new BoneData("indicador_01", 0, 58, 0.644f);
		indicador[1] = new BoneData("indicador_02", 0, 80, 0.89f);

		indicador [0].SetTransform ();
		indicador [1].SetTransform ();

		medio = new BoneData[2];
		medio[0] = new BoneData("medio_01", 0, 58, 0.644f);
		medio[1] = new BoneData("medio_02", 0, 80, 0.89f);

		medio [0].SetTransform ();
		medio [1].SetTransform ();

		anelar = new BoneData[2];
		anelar[0] = new BoneData("anelar_01", 0, 58, 0.644f);
		anelar[1] = new BoneData("anelar_02", 0, 80, 0.89f);

		anelar[0].SetTransform ();
		anelar[1].SetTransform ();

		minimo = new BoneData[2];
		minimo[0] = new BoneData("minimo_01", 0, 58, 0.644f);
		minimo[1] = new BoneData("minimo_02", 0, 80, 0.89f);

		minimo[0].SetTransform ();
		minimo[1].SetTransform ();

		polegar = new BoneData[2];
		polegar[0] = new BoneData("polegar", 0 ,90, 1);
		polegar[1] = new BoneData("polegar_02", 0 ,58,  0.644f);
		polegar[0].SetTransform ();
		polegar[1].SetTransform ();
	}

    /// <summary>
	/// Updates the rotation of the bones in the prosthesis.
	/// </summary>
	public void updateRotation ()
	{
		int i;
		for (i = 0; i < 2; i++) {
			indicador[i].OnUpdateX();
			medio[i].OnUpdateX();
			anelar[i].OnUpdateX();
			minimo[i].OnUpdateX();
		}
		polegar[0].OnUpdateY();
		polegar[1].OnUpdateX();
		//Debug.Log("DElta:"+indicador[0].GetDeltaRotation());
	}

	/// <summary>
	/// Determina a porcentagem de quão fechada está a protese (0- Aberta, 100- Fechada)
	/// </summary>
	/// <param name="porc">Porcentagem.</param>
	public void setRotation(float porc){
		lastStatus = porc;
		int i;
		for (i = 0; i < 2; i++) {
			indicador[i].SetDeltaRotation(porc);
			medio[i].SetDeltaRotation(porc);
			anelar[i].SetDeltaRotation(porc);
			minimo[i].SetDeltaRotation(porc);
		}
		polegar[0].SetDeltaRotation(porc);
		polegar[1].SetDeltaRotation(porc);
	}

	/// <summary>
	/// Bone data.
	/// </summary>
	public class BoneData
	{
		private string name;
		private Transform transform;
		private Quaternion initialRotation;
		private float deltaRotation;
		private float minAngle, maxAngle, step;

		public BoneData (string name, float minAngle, float maxAngle, float step) { 
			this.name = name;
			this.minAngle = minAngle;
			this.maxAngle = maxAngle;
			this.step = step;
			this.deltaRotation = 0.0f;
		}

		public void SetTransform(){
			this.transform = GameObject.Find (this.name).GetComponent<Transform> ();
			this.initialRotation = transform.localRotation;
		}

		public void RotateStep(float factor) {
			this.deltaRotation += factor * step;
		}

		public void OnUpdateZ() {
			deltaRotation = Mathf.Clamp(deltaRotation, minAngle, maxAngle);
			transform.localRotation = initialRotation;
			transform.Rotate(new Vector3(0, 0, deltaRotation));
		}

		public void OnUpdateY() {
			deltaRotation = Mathf.Clamp(deltaRotation, minAngle, maxAngle);
			transform.localRotation = initialRotation;
			transform.Rotate(new Vector3(0, deltaRotation, 0));
		}

		public void OnUpdateX() {
			deltaRotation = Mathf.Clamp(deltaRotation, minAngle, maxAngle);
			transform.localRotation = initialRotation;
			transform.Rotate(new Vector3(deltaRotation, 0, 0));
		}

		public bool isClosed() {
			float dporc = (deltaRotation * 100) / maxAngle;
			if (dporc > 80 )
				return true;
			else
				return false;
		}
			
		public bool isOpened() {
			if (deltaRotation == 0)
				return true;
			else
				return false;
		}

		/// <summary>
		/// Sets the delta rotation.
		/// </summary>
		/// <param name="porc">Porc.</param>
		public void SetDeltaRotation(float porc){
			//if (porc == 0)
			if(porc < 15)
				deltaRotation = 0;
			else
				deltaRotation = (maxAngle*porc)/100;
		}

		/// <summary>
		/// Gets the delta rotation.
		/// </summary>
		/// <returns>The delta rotation.</returns>
		public float GetDeltaRotation(){
			return deltaRotation;
		}

		/// <summary>
		/// Gets the max angle.
		/// </summary>
		/// <returns>The max angle.</returns>
		public float GetMaxAngle(){
			return maxAngle;
		}

		/// <summary>
		/// Gets the minimum angle.
		/// </summary>
		/// <returns>The minimum angle.</returns>
		public float GetMinAngle(){
			return minAngle;
		}

		public string GetName(){
			return this.name;
		}
	}



	/// <summary>
	/// Check if the prosthesis is closed.
	/// </summary>
	/// <returns><c>true</c>, if prosthesis is closed, <c>false</c> otherwise.</returns>
	public bool isProsthesisClosed() {
		if (indicador[0].isClosed() && indicador[1].isClosed())
			return true;
		else
			return false;
	}

	/// <summary>
	/// Check if the prosthesis is opened.
	/// </summary>
	/// <returns><c>true</c>, if prosthesis is opened, <c>false</c> otherwise.</returns>
	public bool isProsthesisOpened() {
	//	LoadBones ();
		Debug.Log ("Opened:" + indicador[0]);
		if (indicador[0].isOpened())
			return true;
		else
			return false;
	}

	/// <summary>
	/// Prosthesises the status.
	/// </summary>
	/// <returns>The status.</returns>
	public float ProsthesisStatus(){
		float porc = (polegar [0].GetDeltaRotation() * 100) / polegar [0].GetMaxAngle ();
		return porc;
	}
}
