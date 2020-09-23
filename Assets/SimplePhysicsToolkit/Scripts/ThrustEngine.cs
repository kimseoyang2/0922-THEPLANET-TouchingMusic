using UnityEngine;
using System.Collections;

/* Simple Physics Toolkit - Thusrt Engine
 * Description: Allows for thrust force to be added to rigidbodies, 
 *				these rigidbodies do not need to be attached to the thruster but must be part of a parent object at least
 * 				Hover mode will disable some options and calculate the required force to make an object hover at a defined distance
 * Author: Dylan Anthony Auty
 * Version: 1.2
 * Last Change: 2020-03-07
*/

namespace SimplePhysicsToolkit {
	public class ThrustEngine : MonoBehaviour {

		public bool enable = true; //System is on and thrusters will fire
		public float maxPower = 500.0f;
		public float currentPowerPercentage = 0.25f;

		public bool hoverMode = false;
		public float hoverDistance = 2.0f;
		public float hoverSafeRange = 0.5f;

		public Rigidbody boundObject;

		Rigidbody thruster;
		//bool targetingParent = false;
		bool forceDisableThruster = false;

		void Start () {
			if (boundObject != null) {
				//Targeting a specific gameObejct
				thruster = boundObject;
			} else if (GetComponent<Rigidbody> () != null) {
				thruster = GetComponent<Rigidbody> ();
			} else {
				forceDisableThruster = true;
			}

			if (forceDisableThruster != true) {
				if (hoverMode) {
					float theMass = thruster.mass;
					thruster.drag = theMass / 10;
					thruster.angularDrag = theMass / 10;
				}
			}
		}


		void FixedUpdate () {
			if (enable && forceDisableThruster != true) {
				if (hoverMode) {
					RaycastHit hit;
					if (Physics.Raycast (transform.position, -Vector3.up, out hit, hoverDistance + hoverSafeRange)) {

						if (hit.distance < hoverDistance) {

							int thrusterCount = 0;
							if(boundObject != null){
								ThrustEngine[] thrustersBound = boundObject.GetComponentsInChildren<ThrustEngine>();
								foreach (ThrustEngine tmpThruster in thrustersBound){
									if(tmpThruster.hoverMode){
										thrusterCount ++;
									}
								}
							}

							applyThrustHover(thrusterCount, hit.distance);
						}
					}

				} else {
					applyThrust ();
				}
			}
		}

		void applyThrust(){
			thruster.AddForceAtPosition (transform.up * (maxPower * currentPowerPercentage), transform.position, ForceMode.Force);
		}

		void applyThrustHover(int thrusters, float distance){
			float massPerThruster = thruster.mass;
			if(thrusters > 0){
				massPerThruster = thruster.mass / thrusters;
			}

			float powerInconsistency = Mathf.InverseLerp(1f, 0.8f, distance / hoverDistance);
			thruster.AddForceAtPosition(((thruster.transform.up * massPerThruster) * -Physics.gravity.y) * powerInconsistency, transform.position);
		}

		void OnDrawGizmos(){
			if (hoverMode) {
				Gizmos.color = Color.red;
				Gizmos.DrawLine (transform.position, transform.position - transform.up * hoverDistance);

				Gizmos.color = Color.cyan;
				Gizmos.DrawLine (transform.position, transform.position + transform.up * hoverSafeRange);
			} else {
				Gizmos.color = Color.yellow;
				Gizmos.DrawLine (transform.position, transform.position - transform.up * 2.0f);
			}
		}
	}
}
