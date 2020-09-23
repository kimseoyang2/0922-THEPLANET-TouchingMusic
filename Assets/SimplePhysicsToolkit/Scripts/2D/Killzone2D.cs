using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SimplePhysicsToolkit {
	[RequireComponent(typeof(Collider2D))]
	public class Killzone2D : MonoBehaviour {
		void Start(){
			if (GetComponent<Collider2D>()) {
				GetComponent<Collider2D>().isTrigger = true; //Force Trigger
			}
		}

		void OnTriggerEnter2D(Collider2D col){
			GameObject currentItem = col.gameObject;
			Destroy (currentItem);
		}
	}
}