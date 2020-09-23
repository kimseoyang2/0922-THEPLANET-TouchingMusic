using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SimplePhysicsToolkit {
	public class TriangleData {
    	
    	public Vector3 point1;
    	public Vector3 point2;
    	public Vector3 point3;
    	public Vector3 center;
    	public Vector3 normal;
    	public float area;
    	public Vector3 velocity;
    	public Vector3 velocityDir;
    	public float cosTheta;

    	public TriangleData(Vector3 p1, Vector3 p2, Vector3 p3, Rigidbody r){
            float a = Vector3.Distance(p1, p2);
            float c = Vector3.Distance(p3, p1);

            this.point1 = p1;
            this.point2 = p2;
            this.point3 = p3;
            this.center = (p1 + p2 + p3) / 3f;
            this.normal = Vector3.Cross(p2 - p1, p3 - p1).normalized;
            this.area = (a * c * Mathf.Sin(Vector3.Angle(p2 - p1, p3 - p1) * Mathf.Deg2Rad)) / 2f;
            this.velocity = TriangleData.GetVelocity(r, this.center);
            this.velocityDir = this.velocity.normalized;
            this.cosTheta = Vector3.Dot(this.velocityDir, this.normal);
        }

        public static Vector3 GetVelocity(Rigidbody r, Vector3 center){
        	Vector3 velB = r.velocity;
        	Vector3 angularVelB = r.angularVelocity;
        	Vector3 pBA = center - r.worldCenterOfMass;
        	Vector3 velA = velB + Vector3.Cross(angularVelB, pBA);

        	return velA;
        }
    }
}
