//Programmer: Ahmed Boulad
//Date: 12/25/2016
//Purpose: Controls the player movement

using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class PlayerMovement : MonoBehaviour 
{	
	private CharacterController controller;

    private Vector3 moveVector;                         //Holds the new transform after movement

	private float baseSpeed = 14.0f;
	private float moveSpeed;                    //Speed at which the player moves
	private float thrust = 12.0f;                        //The jumping force
	private float gravity = 38.0f;
	private float verticalVelocity = 0.0f;

    private float lane = 0;                             //The current lane the player is in
	private Vector2 touchOrigin = -Vector2.one;
	public bool isDead = false;

	public GameObject replayButton;
	public GameObject returnToMain;
	public GameObject explosion;

	public AudioSource gameMusic;
	public AudioSource laneShiftSound;
	public AudioSource jumpSound;
    
	void Start () 
    {
		controller = GetComponent<CharacterController>();
		moveSpeed = baseSpeed;
	}
	
	void Update () 
    {
		if (isDead)
		{
			gameMusic.Stop ();
			replayButton.SetActive (true);
			returnToMain.SetActive (true);
			return;
		}

		moveVector = transform.position;

		#if UNITY_EDITOR || UNITY_STANDALONE || UNITY_WEBPLAYER

			if (Input.GetKeyDown("a"))
			{
				lane -= 2;
				laneShiftSound.Play();
			}

			if (Input.GetKeyDown("d"))
			{
				lane += 2;
				laneShiftSound.Play();
			}

			if (Input.GetKeyDown ("w") && controller.isGrounded)
			{
				verticalVelocity = thrust;
				jumpSound.Play();
			}

			if (Input.GetKeyDown ("s"))
			{
				verticalVelocity = -thrust;
				laneShiftSound.Play();
			}

		#else
			
			if(Input.touchCount > 0)
			{
				Touch myTouch = Input.touches[0];

				if(myTouch.phase == TouchPhase.Began)
				{
					touchOrigin = myTouch.position;
				}
				else if (myTouch.phase == TouchPhase.Ended && touchOrigin.x >= 0)
				{
					Vector2 touchEnd = myTouch.position;
					float x = touchEnd.x - touchOrigin.x;
					float y = touchEnd.y - touchOrigin.y;
					touchOrigin.x = -1;

					if(Mathf.Abs(x) > Mathf.Abs(y))
					{
						lane = x > 0 ? (lane+2) : (lane-2);
						laneShiftSound.Play();
					}
					else if(y > 0 && controller.isGrounded)
					{
						verticalVelocity = thrust;
						jumpSound.Play();
					}
					else if(y < 0)
					{
						verticalVelocity = -thrust;
						laneShiftSound.Play();
					}
				}
			}

		#endif

		moveVector.x = lane;

		transform.position = Vector3.MoveTowards(transform.position, moveVector, 0.2f);

		verticalVelocity -= gravity * Time.deltaTime;

		moveVector = transform.position;
		moveVector.y = verticalVelocity;
		moveVector.z = moveSpeed;

		controller.Move(moveVector * Time.deltaTime);

		if (transform.position.y <= -5.0f)
			Death ();
	}

	public void SetSpeed(float speedMod)
	{
		moveSpeed = baseSpeed + speedMod;
	}

	void OnControllerColliderHit(ControllerColliderHit col)
	{
		if (col.gameObject.tag == "BoxObstacle")
			Death ();

		if (col.gameObject.tag == "Orb") 
		{
			Destroy (col.gameObject);
			GameObject deathExplosion = Instantiate (explosion) as GameObject;
			deathExplosion.transform.position = col.gameObject.transform.position;
		}
	}

	void Death()
	{
		GameObject deathExplosion = Instantiate (explosion) as GameObject;
		deathExplosion.transform.position = transform.position;

		isDead = true;

		this.GetComponent<MeshRenderer> ().enabled = false;
	}
}
