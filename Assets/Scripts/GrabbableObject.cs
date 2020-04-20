using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Grab;

namespace Grab
{
    public enum GrabParts
    {
        palm,
        thumb,
        finger2,
        finger3,
        finger4,
        finger5,
    }
}

public class GrabbableObject : MonoBehaviour
{
    [SerializeField]
    private Transform parent;
    private float speed = 1.5f;
    private List<GrabParts> parts = new List<GrabParts>();
    private AudioSource source;
    private bool touched = false;
    public bool picked = false;

    private void Awake() {
        source = GetComponent<AudioSource>();
    }

    public void setPart(GrabParts part) {
        if (!parts.Contains(part))
            parts.Add(part);

        //Grab parts contains 2 parts and one of them is palm or thumb
        if (parts.Count == 2 &&(parts.Contains(GrabParts.palm) || parts.Contains(GrabParts.thumb)))
            pickUp();
    }

    public void delPart(GrabParts part) {
        if (parts.Contains(part))
            parts.Remove(part);
        //Grab parts contains less than 2 parts or it doesn't contain neither the palm or the thumb
        if (parts.Count < 2 || (!parts.Contains(GrabParts.palm) && !parts.Contains(GrabParts.thumb)))
            throwDrop();
    }

    void pickUp() {
        if(!picked)
        {
            picked = true;
            touched = true;
            transform.SetParent(parent);
            GetComponent<Rigidbody>().useGravity = false;
            //GetComponent<Rigidbody>().freezeRotation = true;
            GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            GetComponentInChildren<BoxCollider>().enabled = false;
        }

    }

    public void throwDrop() {
        if(picked)
        {
            picked = false;
            transform.SetParent(null);
            GetComponent<Rigidbody>().useGravity = true;
            //GetComponent<Rigidbody>().freezeRotation = false;
            GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;

            GetComponentInChildren<BoxCollider>().enabled = true;
        }

    }

    void OnCollisionEnter(Collision collision) {
        
        print("HIT");
        float vol = Vector3.Magnitude(collision.relativeVelocity) / 10.0f;
        print(vol);
        if (vol >= 0.1)
        {
            source.volume = vol;
            source.Play();
        }

    }
}
