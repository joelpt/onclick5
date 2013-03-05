using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {
	
	public float speed = 50.0f;
	public float maxSpeed = 50.0f;
	public float currentVelocity = 0.0f;
	public float dampening = 3.0f;
	public float walkLift = 0.2f;
	public float jumpThrust = 50.0f;
	public float groundDistance = 3.0f;
	public bool jumping = false;
	public Vector3 up;
	public Vector3 targetUp = Vector3.zero;
	public float targetUpLerpSpeed = 0f;
	public float rotateSpeed = 15.0f;
	
	// Use this for initialization
	void Start () {
		Screen.lockCursor = true;
	}
	
	// Update is called once per frame
	void Update () {
		//if (Input.GetKey(KeyCode.E)) {
		//	transform.position += (transform.forward + transform.up * 0.1f) * Time.deltaTime * speed;
		//	rigidbody.AddRelativeForce(transform.forward + transform.up * 0.1f) * Time.deltaTime * speed;
		//}
		//GameObject player = GameObject.FindGameObjectWithTag("Player");
		//Debug.Log (player);
		this.up = transform.up;
		
		if (this.targetUp != Vector3.zero) {
			if (this.targetUp == transform.up) {
				this.targetUp = Vector3.zero;
			}
			Vector3 newUpV=Vector3.Lerp(transform.up, this.targetUp, Time.deltaTime * this.targetUpLerpSpeed * this.rotateSpeed);
			Quaternion newUpQ=Quaternion.FromToRotation(Vector3.up, newUpV);
			transform.rotation = newUpQ;
		}
		
		if (Input.GetKeyDown(KeyCode.X)) {
			Screen.lockCursor = !Screen.lockCursor;
		}

		if (!Screen.lockCursor) {
			if (Input.GetMouseButton(0) || Input.GetMouseButton (1)) {
				Screen.lockCursor = true;
			}
			else {
				return;
			}
		}
		if (Input.GetMouseButtonDown(0)) {
			RaycastHit hit;
			if (Physics.Raycast(Camera.mainCamera.transform.position, Camera.mainCamera.transform.forward, out hit)) {
				Physics.gravity = hit.normal * -9.8f;
				this.targetUp = hit.normal;
				this.targetUpLerpSpeed = 1.0f / Mathf.Sqrt(hit.distance);
				//transform.up = hit.normal;
				//print(Physics.gravity);
//				Vector3 newUpV=Vector3.Lerp(transform.up, hit.normal,Time.deltaTime * 130.0f);
//				Quaternion newUpQ=Quaternion.FromToRotation(Vector3.up, newUpV);
//				transform.rotation = newUpQ;
				//Quaternion rot = Quaternion.FromToRotation(transform.up, hit.normal);
				//transform.rotation = rot;
			    //transform.localEulerAngles = Vector3.Lerp(transform.localEulerAngles, tilt, speed * Time.deltaTime);
				//transform.localEulerAngles = Vector3.Lerp(transform.localEulerAngles, hit.normal, 30.0f * Time.deltaTime);
				//transform.up = hit.normal;
				rigidbody.WakeUp();
			}
		}
		if (Input.GetMouseButtonDown (1)) {
			Debug.Log (transform.rotation.eulerAngles);
			
		}

	}
	
	void FixedUpdate() {
		if (!Screen.lockCursor) return;
		currentVelocity = rigidbody.GetPointVelocity(Vector3.zero).sqrMagnitude;
		var appliedForce = false;
		var grounded = IsGroundedTest();
		
		if (this.jumping && grounded) {
			this.jumping = false;
		}
		if (currentVelocity < maxSpeed) {
			if (grounded && Input.GetKey(KeyCode.Space)) {
				Vector3 v = Physics.gravity.normalized * -jumpThrust;
				rigidbody.AddForce(v);
				appliedForce = true;
				this.jumping = true;
				grounded = false;
			}
			if (grounded) {
				GameObject cam = GameObject.FindWithTag("MainCamera");
				
				if (Input.GetKey(KeyCode.E)) {
					Vector3 v = cam.transform.localEulerAngles + cam.transform.forward * speed;
					rigidbody.AddForce(new Vector3(v.x, walkLift, v.z));
					appliedForce = true;
				}
				if (Input.GetKey(KeyCode.D)) {
					Vector3 v = cam.transform.localEulerAngles + cam.transform.forward * -speed;
					rigidbody.AddForce(new Vector3(v.x, walkLift, v.z));
					appliedForce = true;
				}
				if (Input.GetKey(KeyCode.S)) {
					Vector3 v = Camera.mainCamera.transform.right * -speed;
					rigidbody.AddForce(new Vector3(v.x, walkLift, v.z));
					appliedForce = true;
				}
				if (Input.GetKey(KeyCode.F)) {
					Vector3 v = Camera.mainCamera.transform.right * speed;
					rigidbody.AddForce(new Vector3(v.x, walkLift, v.z));
					appliedForce = true;
				}
			}
		}
		if (appliedForce) {
			return;
		}
		if (currentVelocity > 0.1f && grounded && !this.jumping) {
			Vector3 v = -rigidbody.velocity * dampening;
			rigidbody.AddForce(new Vector3(v.x, v.y, v.z));
		}
	}

	private bool IsGroundedTest () {
		return Physics.Raycast(transform.position, Physics.gravity.normalized, this.groundDistance);
	}
}

