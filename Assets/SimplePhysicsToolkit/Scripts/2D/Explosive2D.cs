using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Simple Physics Toolkit - Explosive 2S
 * Required Components: Collider
 * Author: Dylan Anthony Auty
 * Version: 1.0
 * Last Change: 2020-06-17
*/

namespace SimplePhysicsToolkit {
    [RequireComponent(typeof(Collider2D))]
	public class Explosive2D : MonoBehaviour{
    	public float power = 5f;
        public float radius = 10f;

        public bool onlyAffectInteractableItems = false;

        public GameObject explosionPrefab;

        void OnDrawGizmos(){
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere (transform.position, radius);
        }

        void OnTriggerEnter2D(Collider2D other){
            if (other.GetComponent<Rigidbody2D>()) {
                explode();
            }
        }

        void OnCollisionEnter2D(Collision2D collision){
            if (collision.gameObject.GetComponent<Rigidbody2D>()) {
                explode();
            }
        }

        void explode(){
            Vector2 pos = transform.position;

            Collider2D[] objects = Physics2D.OverlapCircleAll(transform.position, radius);
            foreach (Collider2D col in objects) {
                if (col.GetComponent<Rigidbody2D> ()) { //Must be rigidbody
                    if (onlyAffectInteractableItems) {
                        if (col.GetComponent<InteractableItem> ()) {
                            applyExplosiveForce(col);
                        }
                    } else {
                        applyExplosiveForce(col);
                    }
                }
            }
            if(explosionPrefab != null){
                Instantiate (explosionPrefab, transform.position, transform.rotation);
            }

            Destroy (transform.gameObject);
        }

        void applyExplosiveForce(Collider2D col){
            col.GetComponent<Rigidbody2D>().AddForce(-((power * (transform.position - col.transform.position).normalized)), ForceMode2D.Impulse);
        }
    }
}
