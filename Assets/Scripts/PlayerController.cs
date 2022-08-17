using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

	[SerializeField] private float thrust, minTiltSmooth, maxTiltSmooth, hoverDistance, hoverSpeed;
	private bool start;
	private float timer, tiltSmooth, y;
	private Rigidbody2D playerRigid;
	private Quaternion downRotation, upRotation;

	void Start () {
		tiltSmooth = maxTiltSmooth;
		playerRigid = GetComponent<Rigidbody2D> ();
		downRotation = Quaternion.Euler (0, 0, -90);
		upRotation = Quaternion.Euler (0, 0, 35);
	}

	void Update () {
		if (!start) {
			// Hover the player before starting the game
			timer += Time.deltaTime;
			y = hoverDistance * Mathf.Sin (hoverSpeed * timer);
			transform.localPosition = new Vector3 (0, y, 0);
		} else {
			// Rotate downward while falling
			transform.rotation = Quaternion.Lerp (transform.rotation, downRotation, tiltSmooth * Time.deltaTime);
		}
		// Limit the rotation that can occur to the player
		transform.rotation = new Quaternion (transform.rotation.x, transform.rotation.y, Mathf.Clamp (transform.rotation.z, downRotation.z, upRotation.z), transform.rotation.w);
	}

	void LateUpdate () {
		if (GameManager.Instance.GameState ()) {
			if (Input.GetMouseButtonDown (0)) {
				if(!start){
					// This code checks the first tap. After first tap the tutorial image is removed and game starts
					start = true;
					GameManager.Instance.GetReady ();
					GetComponent<Animator>().speed = 2;
				}
                SetForce();

                //playerRigid.gravityScale = 1f;
				tiltSmooth = minTiltSmooth;
				transform.rotation = upRotation;
				//playerRigid.velocity = Vector2.zero;
				//// Push the player upwards
				//playerRigid.AddForce (Vector2.up * thrust);
				SoundManager.Instance.PlayTheAudio("Flap");
			}
		}
		//if (playerRigid.velocity.y < -1f) {
		//	// Increase gravity so that downward motion is faster than upward motion
		//	tiltSmooth = maxTiltSmooth;
		//	playerRigid.gravityScale = 2f;
		//}
	}
    float force = 0f;
    public float defaultForce = 0.15f;
    public float gravity = -0.098f;
    void SetForce()
    {
        force += defaultForce;
        force = Mathf.Clamp(force, 0, 1.5f * defaultForce);
    }
    float timeForce = 0.8f;
    private void FixedUpdate()
    {
        if (force > 0)
        {
            force -= defaultForce *  Time.fixedDeltaTime/ timeForce;
            if (force < 0)
            {
                force = 0f;
            }
        }
        if (force + gravity < 0)
        {
            tiltSmooth = maxTiltSmooth;
        }
        transform.position += new Vector3(0, force + gravity, 0);
    }
    void OnTriggerEnter2D (Collider2D col) {
		if (col.transform.CompareTag ("Score")) {
			Destroy (col.gameObject);
			GameManager.Instance.UpdateScore ();
		} else if (col.transform.CompareTag ("Obstacle")) {
			// Destroy the Obstacles after they reach a certain area on the screen
			foreach (Transform child in col.transform.parent.transform) {
				child.gameObject.GetComponent<BoxCollider2D> ().enabled = false;
			}
            force = 0;
            gravity = 0;
            KillPlayer ();
		}
	}

	void OnCollisionEnter2D (Collision2D col) {
		if (col.transform.CompareTag ("Ground")) {
            force = 0;
            gravity = 0;
            playerRigid.simulated = false;
			KillPlayer ();
			transform.rotation = downRotation;
		}
	}

	public void KillPlayer () {

        IOnDie[] onDies = GetComponentsInChildren<IOnDie>();
        foreach (IOnDie onDie in onDies)
        {
            onDie.OnDie();
        }

        GameManager.Instance.EndGame ();
		playerRigid.velocity = Vector2.zero;
		// Stop the flapping animation
		GetComponent<Animator> ().enabled = false;          
    }
    public void SetSkin(RuntimeAnimatorController controller)
    {
        IOnChangeSkin[] changeSkins = GetComponentsInChildren<IOnChangeSkin>();
        foreach(IOnChangeSkin skin in changeSkins)
        {
            skin.OnChangeSkin(controller);
        }
    }
}