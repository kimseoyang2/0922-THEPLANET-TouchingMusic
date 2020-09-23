using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Simple Physics Toolkit - Wind 2D
 * Author: Dylan Anthony Auty
 * Version: 1.0
 * Last Change: 2020-06-14
*/

namespace SimplePhysicsToolkit {
	public class Wind2D : MonoBehaviour {
	    public float lift = 30.0f;
		public bool onlyAffectInteractableItems = false;

		void Start(){
			if (GetComponent<Collider2D> ()) {
				GetComponent<Collider2D>().isTrigger = true; //Force Trigger
			}
			lift = lift / 100.0f;
		}
		
		void OnTriggerStay2D(Collider2D other){
			if(other.GetComponent<Rigidbody2D>()){
				if (onlyAffectInteractableItems) {
					if (other.GetComponent<InteractableItem> ()) {
						other.GetComponent<Rigidbody2D> ().AddForce (lift * transform.up, ForceMode2D.Impulse);
					}
				} else {
					other.GetComponent<Rigidbody2D> ().AddForce (lift * transform.up, ForceMode2D.Impulse);
				}
			}
		}

		void OnDrawGizmos(){
			Gizmos.color = Color.cyan;
			Gizmos.DrawLine (transform.position, transform.position + transform.up * 1.0f);
			Gizmos.color = Color.red;
			Gizmos.DrawWireSphere (transform.position, 0.1f);
		}
	}
}