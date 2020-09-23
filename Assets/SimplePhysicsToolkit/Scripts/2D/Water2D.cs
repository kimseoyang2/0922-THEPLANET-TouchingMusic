using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Simple Physics Toolkit - Water 2D
 * Author: Dylan Anthony Auty
 * Version: 1.0
 * Last Change: 2020-06-14
*/

namespace SimplePhysicsToolkit {
	public class Water2D : MonoBehaviour {
	    public float pressure = 3.0f;
		public float waterDrag = 1.0f;

		public bool onlyAffectInteractableItems = false;

		void Start(){
			pressure = pressure / 10.0f;

			//Handles setting up the trigger correctly
			BoxCollider2D col = null;
			if(GetComponent<BoxCollider2D>()){
				col = GetComponent<BoxCollider2D>();
			} else { //Add box Collider
				gameObject.AddComponent<BoxCollider2D>();
				col = GetComponent<BoxCollider2D>();
			}

			if(col != null){
				col.isTrigger = true; //Force trigger
			}
		}

		void OnTriggerStay2D(Collider2D other){ 
			if(other.GetComponent<Rigidbody2D>()){ 
				if (onlyAffectInteractableItems) {
					if (other.GetComponent<InteractableItem> ()) {
						other.GetComponent<Rigidbody2D>().AddForce(pressure * transform.up, ForceMode2D.Impulse);
						other.GetComponent<Rigidbody2D>().drag = waterDrag;
						other.GetComponent<Rigidbody2D>().angularDrag = 2.0f;
					}
				} else {
					other.GetComponent<Rigidbody2D>().AddForce(pressure * transform.up, ForceMode2D.Impulse);
					other.GetComponent<Rigidbody2D>().drag = waterDrag;
					other.GetComponent<Rigidbody2D>().angularDrag = 2.0f;
				}
			}
			
		}

		//Note: Reset values can be altered as preferred - values based on Unity defaults
		void OnTriggerExit2D(Collider2D other){
			if(other.GetComponent<Rigidbody2D>()){
				if (onlyAffectInteractableItems) {
					if (other.GetComponent<InteractableItem> ()) {
						other.GetComponent<Rigidbody2D>().drag = 0.0f; //Reset Drag to zero
						other.GetComponent<Rigidbody2D>().angularDrag = 0.05f; //Reset to default 0.05
					}
				} else {
					other.GetComponent<Rigidbody2D>().drag = 0.0f; //Reset Drag to zero
					other.GetComponent<Rigidbody2D>().angularDrag = 0.05f; //Reset to default 0.05
				}
			}
		}
	}
}