using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Simple Physics Toolkit - Buoyancy Item
 * Description: A simple helper to the buoyancy system. 
 * Required Components: Rigidbody & Mesh Filter
 * Author: Dylan Anthony Auty
 * Version: 1.0
 * Last Change: 2020-03-07
 *
 * Special thanks to @eriknordeus for the inspiration of this, find more from him here: https://www.habrador.com
 * 
 * DO NOT APPLY DIRECTLY - This is added by the script dynamically
*/
 
namespace SimplePhysicsToolkit {
	[RequireComponent(typeof(Rigidbody))]
	[RequireComponent(typeof(MeshFilter))]
	public class BuoyantItem : MonoBehaviour{
	    
	    [Header("Note: This should not be added directly to an item.")]
	    private Rigidbody rBody;
	    private Mesh rMesh;
	    private MeshFilter rMeshFilter;
	    private Transform rTrans;

	    private Vector3[] itemVertices;
	    private int[] itemTriangles;

	    private Vector3[] globalVerticePositions;
	    private float[] distancesToFluid;

	    private List<TriangleData> submergedTriangles = new List<TriangleData>();
	    private MeshCollider submergedCollider;

	    public List<int> indexOfOriginalTriangle = new List<int>();
	    public List<FluidSlammingForce> slammingForceData = new List<FluidSlammingForce>();

	    private float itemArea;

	    private Buoyancy fluid;

	    public void Start(){
	    	rBody = gameObject.GetComponent<Rigidbody>();
	    	rMeshFilter = gameObject.GetComponent<MeshFilter>();
	    	
	    	rMesh = rMeshFilter.mesh;
	    	rTrans = gameObject.transform;
	    	itemVertices = rMesh.vertices;
	    	itemTriangles = rMesh.triangles;

	    	globalVerticePositions = new Vector3[itemVertices.Length];
	    	distancesToFluid = new float[itemVertices.Length];

	    	for (int i = 0; i < (itemTriangles.Length / 3); i++){
            	slammingForceData.Add(new FluidSlammingForce());
        	}

	    	CalculateOriginalTrianglesArea();
	    }

	    public void Update(){

	    	//Enable this only to see submerged mesh approximation
	    	//RenderSubmergedMesh();
	    }

	    public void FixedUpdate(){
	    	GenerateSubmergedMesh();
	    	if(submergedTriangles.Count > 0){
	    		ApplyWaterForce();
	    	}
	    }

	    public void SetFluidParent(Buoyancy f){
	    	fluid = f;
	    }

	    public void ApplyWaterForce(){
	    	float resistanceCf = CalculateResistanceCoefficient(fluid.fluidDensity, rBody.velocity.magnitude, CalculateSubmergedLength());

	    	CalculateSlammingVelocities();

            for (int i = 0; i < submergedTriangles.Count; i++){
                TriangleData triangleData = submergedTriangles[i];

                Vector3 force = Vector3.zero;

                force += CalculateBuoyancy(triangleData);
                force += CalculateViscousWaterResistanceForce(triangleData, resistanceCf);

                if(fluid.CalculatePressureForces){
                	force += CaclulatePressureDragForce(triangleData);
                }

                if(fluid.CalculateSlammingForces){
                	int originalTriangleIndex = indexOfOriginalTriangle[i];
                	FluidSlammingForce slammingData = slammingForceData[originalTriangleIndex];
                
                	force += CaluclateSlammingForce(slammingData, triangleData);
                }

                rBody.AddForceAtPosition(force, triangleData.center);

                if(fluid.visualizeForces){
                	Debug.DrawRay(triangleData.center, triangleData.normal * 3f, Color.red);
                	// Debug.DrawRay(triangleData.center, bForce.normalized * -3f, Color.white);
                }
            }
	    }

	    private Vector3 CalculateBuoyancy(TriangleData triangleData){
            Vector3 bForce = fluid.fluidDensity * Physics.gravity.y * fluid.GetDistanceToSurface(triangleData.center) * triangleData.area * triangleData.normal;
            bForce.x = 0f;
            bForce.z = 0f;
            bForce.y = bForce.y > 0f ? bForce.y : 0f;
            return bForce;
	    }

	    private Vector3 CalculateViscousWaterResistanceForce(TriangleData triangleData, float Cf){
	        Vector3 B = triangleData.normal;
	        Vector3 A = triangleData.velocity;
	        Vector3 velocityTangent = Vector3.Cross(B, (Vector3.Cross(A, B) / B.magnitude)) / B.magnitude;
	        Vector3 tangentialDirection = velocityTangent.normalized * -1f;
	        Vector3 v_f_vec = triangleData.velocity.magnitude * tangentialDirection;
	        Vector3 viscousWaterResistanceForce = 0.5f * fluid.fluidDensity * v_f_vec.magnitude * v_f_vec * triangleData.area * Cf;
	        
	        return viscousWaterResistanceForce;
	    }

	    private Vector3 CaclulatePressureDragForce(TriangleData triangleData){
	        float velocity = triangleData.velocity.magnitude;
	        float velocityReference = velocity;
	        velocity = velocity / velocityReference;
	        Vector3 pressureDragForce = Vector3.zero;

	        if (triangleData.cosTheta > 0f) {
	            float C_PD1 = 10f;
	            float C_PD2 = 10f;
	            float f_P = 0.5f;

	            pressureDragForce = -(C_PD1 * velocity + C_PD2 * (velocity * velocity)) * triangleData.area * Mathf.Pow(triangleData.cosTheta, f_P) * triangleData.normal;
	        } else {
	            float C_SD1 = 10f;
	            float C_SD2 = 10f;
	            float f_S = 0.5f;

	            pressureDragForce = (C_SD1 * velocity + C_SD2 * (velocity * velocity)) * triangleData.area * Mathf.Pow(Mathf.Abs(triangleData.cosTheta), f_S) * triangleData.normal;
	        }

	        return pressureDragForce;
	    }
	    
	    private Vector3 CaluclateSlammingForce(FluidSlammingForce slammingData, TriangleData triangleData){
	        if (triangleData.cosTheta < 0f || slammingData.originalArea <= 0f) {
	            return Vector3.zero;
	        }
	        
	        Vector3 dV = slammingData.submergedArea * slammingData.velocity;
	        Vector3 dV_previous = slammingData.previousSubmergedArea * slammingData.previousVelocity;
	        Vector3 accVec = (dV - dV_previous) / (slammingData.originalArea * Time.fixedDeltaTime);
	        float acc = accVec.magnitude;

	        Vector3 F_stop = rBody.mass * triangleData.velocity * ((2f * triangleData.area) / itemArea);
	        float p = 2f;
	        float acc_max = acc;
	        Vector3 slammingForce = Mathf.Pow(Mathf.Clamp01(acc / acc_max), p) * triangleData.cosTheta * F_stop * fluid.slammingMultiplier;

	        return slammingForce * -1f;  
	    }

	    private void GenerateSubmergedMesh(){
	    	submergedTriangles.Clear();

	    	for (int i = 0; i < slammingForceData.Count; i++){
            	slammingForceData[i].previousSubmergedArea = slammingForceData[i].submergedArea;
        	}

        	indexOfOriginalTriangle.Clear();

	    	for(int i = 0; i < itemVertices.Length; i++){
	    		Vector3 globalVerticePos = rTrans.TransformPoint(itemVertices[i]);
	    		globalVerticePositions[i] = globalVerticePos;
	    		distancesToFluid[i] = fluid.GetDistanceToSurface(globalVerticePos);
	    	}

	    	AddTriangles();
	    }

	    private void AddTriangles(){
	    	List<ItemVertexData> vertexData = new List<ItemVertexData>();
	    	
	    	vertexData.Add(new ItemVertexData());
	    	vertexData.Add(new ItemVertexData());
	    	vertexData.Add(new ItemVertexData());
	    	
	    	int i = 0;
	    	int triangleCounter = 0;
	    	while(i < itemTriangles.Length){
	    		
	    		for(int vi = 0; vi < 3; vi++){
	    			vertexData[vi].distance = distancesToFluid[itemTriangles[i]];
	    			vertexData[vi].index = vi;
	    			vertexData[vi].globalVertexPos = globalVerticePositions[itemTriangles[i]];
	    			i++;
	    		}

	    		if (vertexData[0].distance > 0f && vertexData[1].distance > 0f && vertexData[2].distance > 0f){
                	slammingForceData[triangleCounter].submergedArea = 0f;
                    continue;
                }

	    		if (vertexData[0].distance < 0f && vertexData[1].distance < 0f && vertexData[2].distance < 0f){
                    submergedTriangles.Add(new TriangleData(vertexData[0].globalVertexPos, vertexData[1].globalVertexPos, vertexData[2].globalVertexPos, rBody));
                    
                    slammingForceData[triangleCounter].submergedArea = slammingForceData[triangleCounter].originalArea;
                    indexOfOriginalTriangle.Add(triangleCounter);

                } else {
                    vertexData.Sort((x, y) => x.distance.CompareTo(y.distance));
                    vertexData.Reverse();

                    if (vertexData[0].distance > 0f && vertexData[1].distance < 0f && vertexData[2].distance < 0f){
                    	//One vertice is above the fluid, the rest are below
			            
			            Vector3 p1 = vertexData[0].globalVertexPos;
			            Vector3 p2 = Vector3.zero;
			            Vector3 p3 = Vector3.zero;

			            float p1_distance = vertexData[0].distance;
			            float p2_distance = 0f;
			            float p3_distance = 0f;
			            
			            int p2_index = vertexData[0].index - 1;
			            if (p2_index < 0){
			                p2_index = 2;
			            }

			            if (vertexData[1].index == p2_index){
			                p2 = vertexData[1].globalVertexPos;
			                p3 = vertexData[2].globalVertexPos;
			                p2_distance = vertexData[1].distance;
			                p3_distance = vertexData[2].distance;
			            } else {
			                p2 = vertexData[2].globalVertexPos;
			                p3 = vertexData[1].globalVertexPos;

			                p2_distance = vertexData[2].distance;
			                p3_distance = vertexData[1].distance;
			            }
						
			            //Find Cut point2
			            Vector3 p2p1 = p1 - p2;
			            float t_p2 = -p2_distance / (p1_distance - p2_distance);
			            Vector3 ic_p2 = t_p2 * p2p1;
			            Vector3 i_p2 = ic_p2 + p2;

			            //Find Cut point3
			            Vector3 p3p1 = p1 - p3;
			            float t_p3 = -p3_distance / (p1_distance - p3_distance);
			            Vector3 ic_p3 = t_p3 * p3p1;
			            Vector3 i_p3 = ic_p3 + p3;

			            TriangleData t1 = new TriangleData(p2, i_p2, i_p3, rBody);
			            TriangleData t2 = new TriangleData(p2, i_p3, p3, rBody);
			            submergedTriangles.Add(t1);
			            submergedTriangles.Add(t2);

			            slammingForceData[triangleCounter].submergedArea = t1.area + t2.area;

			            indexOfOriginalTriangle.Add(triangleCounter);
        				indexOfOriginalTriangle.Add(triangleCounter);

                    } else if (vertexData[0].distance > 0f && vertexData[1].distance > 0f && vertexData[2].distance < 0f) {
                    	//Two vertices are above the fluid, the other is below
                       	
			            Vector3 p1 = Vector3.zero;
			            Vector3 p2 = Vector3.zero;
			            Vector3 p3 = vertexData[2].globalVertexPos;
			            
			            float p1_distance = 0f;
			            float p2_distance = 0f;
			            float p3_distance = vertexData[2].distance;

			            int p1_index = vertexData[2].index + 1;
			            if (p1_index > 2){
			                p1_index = 0;
			            }

			            if (vertexData[1].index == p1_index){
			                p1 = vertexData[1].globalVertexPos;
			                p2 = vertexData[0].globalVertexPos;
			                p1_distance = vertexData[1].distance;
			                p2_distance = vertexData[0].distance;
			            } else {
			                p1 = vertexData[0].globalVertexPos;
			                p2 = vertexData[1].globalVertexPos;
			                p1_distance = vertexData[0].distance;
			                p2_distance = vertexData[1].distance;
			            }

			            //Find cut for p2
			            Vector3 p3p2 = p2 - p3;
			            float t_p2 = -p3_distance / (p2_distance - p3_distance);
			            Vector3 ic_p2 = t_p2 * p3p2;
			            Vector3 i_p2 = ic_p2 + p3;

			            //Find cut for p1
			            Vector3 p3p1 = p1 - p3;
			            float t_p1 = -p3_distance / (p1_distance - p3_distance);
			            Vector3 ic_p1 = t_p1 * p3p1;
			            Vector3 i_p1 = ic_p1 + p3;

			            TriangleData t1 = new TriangleData(p3, i_p1, i_p2, rBody);
			            submergedTriangles.Add(t1);

        				slammingForceData[triangleCounter].submergedArea = t1.area;

        				indexOfOriginalTriangle.Add(triangleCounter);

                    }
                }

                triangleCounter += 1;
	    	}
	    }
        
        private class ItemVertexData{
            public float distance;
            public int index;
            public Vector3 globalVertexPos;
        }

        private void CalculateOriginalTrianglesArea(){
	        int i = 0;
	        int triangleCounter = 0;
	        while (i < itemTriangles.Length){
	            Vector3 p1 = itemVertices[itemTriangles[i]];
	            Vector3 p2 = itemVertices[itemTriangles[i + 1]];
	            Vector3 p3 = itemVertices[itemTriangles[i + 2]];
	            i += 3;

	            TriangleData triangleData = new TriangleData(p1,p2,p3, rBody);
	       
	            //Store the area in a list
	            slammingForceData[triangleCounter].originalArea = triangleData.area;

	            itemArea += triangleData.area;

	            triangleCounter += 1;
	        }
    	}

    	private float CalculateResistanceCoefficient(float rho, float velocity, float length){
	        float nu = 0.000001f;
	        float Rn = (velocity * length) / nu;
	        return 0.075f / Mathf.Pow((Mathf.Log10(Rn) - 2f), 2f);
	    }

	    private float CalculateSubmergedLength(){
	    	Mesh submergedMesh = new Mesh();
	    	List<Vector3> vertices = new List<Vector3>();
            List<int> triangles = new List<int>();

            for (int i = 0; i < submergedTriangles.Count; i++){
                Vector3 p1 = rTrans.InverseTransformPoint(submergedTriangles[i].point1);
                Vector3 p2 = rTrans.InverseTransformPoint(submergedTriangles[i].point2);
                Vector3 p3 = rTrans.InverseTransformPoint(submergedTriangles[i].point3);

                vertices.Add(p1);
                triangles.Add(vertices.Count - 1);

                vertices.Add(p2);
                triangles.Add(vertices.Count - 1);

                vertices.Add(p3);
                triangles.Add(vertices.Count - 1);
            }

            submergedMesh.vertices = vertices.ToArray();
            submergedMesh.triangles = triangles.ToArray();
            submergedMesh.RecalculateBounds();

            return submergedMesh.bounds.size.z;
	    }

	    private void CalculateSlammingVelocities() {
	        for (int i = 0; i < slammingForceData.Count; i++) {
	            slammingForceData[i].previousVelocity = slammingForceData[i].velocity;
	            Vector3 center = transform.TransformPoint(slammingForceData[i].triangleCenter);
	            slammingForceData[i].velocity = TriangleData.GetVelocity(rBody, center);
	        }
   	 	}

        /**
         * For development only 
        */
        private void RenderSubmergedMesh(){
            List<Vector3> vertices = new List<Vector3>();
            List<int> triangles = new List<int>();

            for (int i = 0; i < submergedTriangles.Count; i++){
                Vector3 p1 = rTrans.InverseTransformPoint(submergedTriangles[i].point1);
                Vector3 p2 = rTrans.InverseTransformPoint(submergedTriangles[i].point2);
                Vector3 p3 = rTrans.InverseTransformPoint(submergedTriangles[i].point3);

                vertices.Add(p1);
                triangles.Add(vertices.Count - 1);

                vertices.Add(p2);
                triangles.Add(vertices.Count - 1);

                vertices.Add(p3);
                triangles.Add(vertices.Count - 1);
            }

            rMesh.Clear();
            rMesh.name = "Generated Mesh";
            rMesh.vertices = vertices.ToArray();
            rMesh.triangles = triangles.ToArray();
            rMesh.RecalculateBounds();
        }

	}
}
