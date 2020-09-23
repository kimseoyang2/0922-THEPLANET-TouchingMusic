using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Simple Physics Toolkit - Particle Influencer
 * Description: Attracts or Repels Particles using Particle Force Field Influencers
 * Required Components: None
 * Author: Dylan Anthony Auty
 * Version: 1.1
 * Last Change: 2020-03-07
*/
namespace SimplePhysicsToolkit {
    public class ParticleInfluencer : MonoBehaviour {

        public bool enable = true;
        public bool attract = true;
        public float forceMultiplier = 1f;
        public float drag = 1f;
        public float range = 10f;

        private ParticleSystemForceField forceField;
        private ParticleSystem.ExternalForcesModule externalForcesModule;

        void Start(){
            if(enable){
                forceField = gameObject.AddComponent<ParticleSystemForceField>();
                forceField.endRange = range;
                forceField.drag = drag;

                if(attract){
                    forceField.gravity = (1f * forceMultiplier);
                } else {
                    forceField.gravity = -(1f * forceMultiplier);
                }

                FindAndEnableExternalForces();
            }
        }

        void FixedUpdate(){
            if(enable){
                forceField.endRange = range;
                forceField.drag = drag;

                if(attract){
                    forceField.gravity = (1f * forceMultiplier);
                } else {
                    forceField.gravity = -(1f * forceMultiplier);
                }
            }
        }

        void FindAndEnableExternalForces(){
            ParticleSystem[] partSystems = FindObjectsOfType<ParticleSystem>();
            if(partSystems.Length > 0){
                foreach(ParticleSystem part in  partSystems){
                    ApplyForce(part);
                }
            }
        }

        void ApplyForce(ParticleSystem part){
            externalForcesModule = part.externalForces;
            externalForcesModule.enabled = true;
            externalForcesModule.influenceFilter = ParticleSystemGameObjectFilter.List;
            externalForcesModule.AddInfluence(forceField);
        }

        void OnDrawGizmos(){
            if (enable) {
                Gizmos.color = Color.red;
                Gizmos.DrawWireSphere(transform.position, range);
            }
        }
    }
}
