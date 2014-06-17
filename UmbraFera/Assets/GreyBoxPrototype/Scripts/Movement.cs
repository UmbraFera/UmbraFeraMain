using UnityEngine;
using System.Collections;

public class Movement : MonoBehaviour {

	#region Fields
	private Animator anim;
	//60 Frames of movement
	private const float speed = 30.0f;
	private const string AxisHorizontal = "Horizontal";
	private const string AxisVertical = "Vertical";
	private float rayDistance;
	private const float bufferSlope = 2.0f;
	private const float bufferPull = -20.0f;
	private const float clampSpeed = 20.0f;
	private Vector3 gravityBoost = Vector3.zero;
	#endregion
	

	#region Private Methods
	// Use this for initialization
	void Start () 
	{
		//anim = gameObject.GetComponent<Animator>();
		rayDistance = ((CapsuleCollider)collider).height * 0.5f + ((CapsuleCollider)collider).radius;
	}
	
	void Awake()
	{
		gravityBoost = new Vector3(0f, bufferPull * rigidbody.mass, 0f);
	}

	void SlopeOffset()
	{
		RaycastHit hit;
		if(Physics.Raycast(transform.position, -Vector3.up, out hit, rayDistance))
		{
			if(Vector3.Angle (hit.normal, Vector3.up) > bufferSlope)
			{
				Debug.DrawRay(transform.position,hit.normal,Color.red);
				//cancle out slope or play other animation
			}
		}
	}
	
	internal void MoveCharacter(ref float getXAxis, ref float getYAxis)
	{
		Vector3 targetVelocity = new Vector3(getXAxis * speed * Time.deltaTime, 0.0f , getYAxis * speed * Time.deltaTime);
		//targetVelocity = transform.TransformDirection(targetVelocity);
		Vector3 velocityChange = targetVelocity - rigidbody.velocity;
		velocityChange.x = Mathf.Clamp(velocityChange.x, -clampSpeed,clampSpeed);
		velocityChange.z = Mathf.Clamp(velocityChange.z,-clampSpeed,clampSpeed);
		rigidbody.AddForce(velocityChange,ForceMode.VelocityChange);
		rigidbody.AddForce(gravityBoost);

		//Scrap
		//transform.position += transform.right * getXAxis * speed * Time.deltaTime;
		//rigidbody.AddForce(getXAxis * speed * Time.deltaTime, 0.0f , getYAxis * speed * Time.deltaTime, ForceMode.VelocityChange);
		//rigidbody.AddForce(getXAxis * speed * Time.deltaTime, 0.0f , getYAxis * speed * Time.deltaTime, ForceMode.VelocityChange);
	}

	void FixedUpdate()
	{
		SlopeOffset();

		//anim.SetFloat("Speed", Mathf.Abs(Input.GetAxis(axisName)));
		float getXAxis = Input.GetAxis(AxisHorizontal);	
		float getYAxis = Input.GetAxis(AxisVertical);
		Vector3 scale = transform.localScale;

		//Flip character sprite
		if(getXAxis < 0.0f)
		{
			scale.x = scale.y = 1.0f;
			transform.localScale = scale;
		}
		else if(getXAxis > 0.0f)
		{
			scale.x = 1.0f;
			transform.localScale = scale;
		}
		MoveCharacter(ref getXAxis, ref getYAxis);
	}
	#endregion
}
