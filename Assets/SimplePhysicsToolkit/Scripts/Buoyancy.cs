using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/* Simple Physics Toolkit - Buoyancy
 * Description: This replaces the legacy water script and allows for more complex physics calculations in fluid
 * Required Components: Collider (Trigger)
 * Author: Dylan Anthony Auty
 * Version: 1.0
 * Last Change: 2020-03-07
 *
 * Special thanks to @eriknordeus for the inspiration of this, find more from him here: https://www.habrador.com
 * 
 * Particularly the displacement calculations were extremely helpful in my implementation.
*/

namespace SimplePhysicsToolkit {
	[RequireComponent(typeof(Collider))]
	public class Buoyancy : MonoBehaviour{
	    
	    public float fluidDensity = 1027f;
	    public bool onlyAffectInteractableItems = false;
	    public bool visualizeForces = true;

	    public bool CalculatePressureForces = false; 
	    public bool CalculateSlammingForces = false;

	    public float slammingMultiplier = 0.1f;

	    private Collider col;
	    private float fluidLevel;

	    public void Start () {
			if (GetComponent<Collider> ()) {
				col = GetComponent<Collider>();
				col.isTrigger = true;

				fluidLevel = transform.position.y + col.bounds.size.y / 2;
			}
		}

		public void OnTriggerEnter(Collider other){
			if(other.GetComponent<Rigidbody>()){ 
				bool canApplyForce = false; 
				if (onlyAffectInteractableItems) {
					if (other.GetComponent<InteractableItem>()) {
						canApplyForce = true;
					}
				} else {
					canApplyForce = true;
				}

				if(canApplyForce){
					RegisterBuoyancyItem(other.gameObject);
				}
			}
		}

	    public void OnTriggerExit(Collider other){ 
			if(other.GetComponent<BuoyantItem>()){
				Destroy(other.GetComponent<BuoyantItem>());
			}
		}

		public float GetDistanceToSurface(Vector3 p){
			return p.y - fluidLevel;
		}

		private void RegisterBuoyancyItem(GameObject o){
			if(!o.GetComponent<BuoyantItem>()){
				o.AddComponent<BuoyantItem>();
				o.GetComponent<BuoyantItem>().SetFluidParent(this);
			}
		}
	}
}
