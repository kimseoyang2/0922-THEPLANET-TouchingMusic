using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Simple Physics Toolkit - Wind
 * Description: Creates a list of character joints to allow for basic rope physics
 * Required Components: This script requires a connected body. These can be chained together for cool effects
 * Author: Dylan Anthony Auty
 * Version: 1.0
 * Last Change: 2020-03-07
*/

namespace SimplePhysicsToolkit {
	public class Rope : MonoBehaviour {


		public Transform connectedBody;
		public bool isAnchor = false;
		public int resolution = 5;

		public bool renderRope = true;
	    public float ropeWidth = 0.2f;
	    public bool simpleRopeCollisions = true;
	    public Gradient ropeColor;

		private float segmentLength;
		private List<GameObject> ropeSegments = new List<GameObject>();

		private LineRenderer lineRender;

		public void Start(){

			if(!gameObject.GetComponent<LineRenderer>()){
				gameObject.AddComponent<LineRenderer>();
			}

	        lineRender = gameObject.GetComponent<LineRenderer>();
	        lineRender.material = new Material(Shader.Find("Sprites/Default"));
	        lineRender.colorGradient = ropeColor;

			if(connectedBody != null){
				segmentLength = GetDistanceToTarget() / resolution;
				Vector3 dir = GetDirectionToTarget();

				ropeSegments.Add(transform.gameObject);
				for (int i = 1; i < resolution; i++){
					GameObject ropeSegment = new GameObject("RopeSegment_" + i);
					ropeSegment.transform.position = transform.position + (dir * segmentLength * i);
					ropeSegment.transform.SetParent(transform);
					ropeSegments.Add(ropeSegment);
				}
				ropeSegments.Add(connectedBody.gameObject);

				CreateRopeJoints();
			}
		}

		public void Update() {
        	RenderRope();
    	}

    	private void RenderRope() {
	        lineRender.startWidth = ropeWidth;
	        lineRender.endWidth = ropeWidth;

	        Vector3[] positions = new Vector3[ropeSegments.Count];
	        for (int i = 0; i < ropeSegments.Count; i++) {
	            positions[i] = ropeSegments[i].transform.position;
	        }
	        
	        lineRender.positionCount = positions.Length;
	        lineRender.SetPositions(positions);
	    }

		private void CreateRopeJoints(){
			for (int i = 0; i < ropeSegments.Count; i++) {
				if(!ropeSegments[i].GetComponent<Rigidbody>()){
					Rigidbody jr = ropeSegments[i].AddComponent<Rigidbody>();
					if(i == 0 && isAnchor){
						//Only set this if its the first point in the loop, and we want it to be an anchor point
						jr.isKinematic = true;
					}

					if(simpleRopeCollisions){
						if(i > 0 && i < (ropeSegments.Count -1)){
							ropeSegments[i].AddComponent<SphereCollider>();
							ropeSegments[i].GetComponent<SphereCollider>().radius = ropeWidth;
						}
					}
				}
			}

			for (int i = 0; i < ropeSegments.Count; i++) {
				if(i < (ropeSegments.Count - 1)){
            		CharacterJoint j = ropeSegments[i].AddComponent<CharacterJoint>();
            		j.connectedBody = ropeSegments[i+1].GetComponent<Rigidbody>();
            		j.anchor = Vector3.zero;
            		
            		SoftJointLimit jLTL = j.lowTwistLimit;
            		SoftJointLimit jHTL = j.highTwistLimit;
            		SoftJointLimit jSL1 = j.swing1Limit;
            		SoftJointLimit jSL2 = j.swing2Limit;
            		
            		jLTL.limit = -177f;
            		jHTL.limit = 177f;
            		jSL1.limit = 177f;
            		jSL2.limit = 177f;
            		
            		j.lowTwistLimit = jLTL;
            		j.highTwistLimit = jHTL;
            		j.swing1Limit = jSL1;
            		j.swing2Limit = jSL2;
				} 
            }
		}

		private float GetDistanceToTarget(){
			return Vector3.Distance(transform.position, connectedBody.transform.position);
		}

		private Vector3 GetDirectionToTarget(){
			return -(transform.position - connectedBody.transform.position).normalized;
		}

		private void OnDrawGizmos(){
			if(connectedBody != null){
				Gizmos.color = Color.red;
				Gizmos.DrawLine(transform.position, connectedBody.position);
			}
		}
	}
}
