using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/* Simple Physics Toolkit - Magnet 2D
 * Author: Dylan Anthony Auty
 * Version: 1.0
 * Last Change: 2020-06-14
*/

namespace SimplePhysicsToolkit {
	public class Magnet2D : MonoBehaviour {
	    public float magnetForce = 15.0f;
		public bool enable = true;
		public bool attract = true;
		public float innerRadius = 2.0f;
		public float outerRadius = 5.0f;

		public bool onlyAffectInteractableItems = false;
		public bool realismMode = false;

		void FixedUpdate () {
			if (enable) {
				Collider2D[] objects = Physics2D.OverlapCircleAll(transform.position, outerRadius);
				foreach (Collider2D col in objects) {
					if (col.GetComponent<Rigidbody2D> ()) { //Must be rigidbody
						if (onlyAffectInteractableItems) {
							if (col.GetComponent<InteractableItem> ()) {
								attractOrRepel (col);
							}
						} else {
							attractOrRepel (col);
						}
					}
				}
			}
		}

		void attractOrRepel(Collider2D col){
			if (Vector2.Distance (transform.position, col.transform.position) > innerRadius) {
				//Apply force in direction of magnet center
				if (attract) {
					if (realismMode) {
						float dynamicDistance = Mathf.Abs( (Vector2.Distance (transform.position, col.transform.position) ) - (outerRadius + (innerRadius * 2)) );
						float multiplier = dynamicDistance / outerRadius;

						col.GetComponent<Rigidbody2D>().AddForce ( (magnetForce * (transform.position - col.transform.position).normalized) * multiplier, ForceMode2D.Force);
					} else {
						col.GetComponent<Rigidbody2D>().AddForce (magnetForce * (transform.position - col.transform.position).normalized, ForceMode2D.Force);
					}
				} else {
					if (realismMode) {
						float dynamicDistance = Mathf.Abs( (Vector2.Distance (transform.position, col.transform.position) ) - (outerRadius + (innerRadius * 2)) );
						float multiplier = dynamicDistance / outerRadius;

						col.GetComponent<Rigidbody2D>().AddForce (-( (magnetForce * (transform.position - col.transform.position).normalized) * multiplier), ForceMode2D.Force);
					} else {
						col.GetComponent<Rigidbody2D>().AddForce (-magnetForce * (transform.position - col.transform.position).normalized, ForceMode2D.Force);
					}
				}
			} else {
				//Inner Radius float gentle - Future additional handling here
			}
		}

		void OnDrawGizmos(){
			if (enable) {
				Gizmos.color = Color.red;
				Gizmos.DrawWireSphere(transform.position, outerRadius);
				Gizmos.color = Color.yellow;
				Gizmos.DrawWireSphere(transform.position, innerRadius);

				Gizmos.DrawIcon (transform.position, "sptk_magnet.png", true);
			}
		}
	}
}