using UnityEngine;
using System.Collections;

public class VirionController : MonoBehaviour {

	public float infectionAmount = 3;
	public float topSpeed = 100;

	CellController target;
	Rigidbody2D physicsBody;
	Vector3 roamHeading = new Vector3(0, 0, 0);
	string state = "targeted";
	float newRoamHeadingInterval = 1;
	float timeToNewRoamHeading = 0;
	float instanceID;
	bool dead = false;

	// Use this for initialization
	void Start () {
		float randX, randY;

		physicsBody = gameObject.GetComponent<Rigidbody2D>();
		instanceID = GetInstanceID();
		Vector3 directionVector = target.transform.position - transform.position;
		Vector2 directionVector2D;

		directionVector.Normalize();

		randX = Random.Range(-0.3f, 0.3f);
		randY = Random.Range(-0.3f, 0.3f);

		directionVector = directionVector - new Vector3(randX, randY, 0);

		directionVector.Normalize();

		directionVector *= 800;

		directionVector2D = new Vector2(directionVector.x, directionVector.y);

		physicsBody.AddForce(directionVector2D, ForceMode2D.Impulse);
	}

	// Update is called once per frame
	void Update () {
		if (dead) {
			Destroy(gameObject);
		}
	}

	void FixedUpdate() {
		float actualSpeed, randX, randY;

		if (target && target.owner != "body") {
			target = null;
			state = "untargeted";
		}

		if (target && state == "targeted") {
			actualSpeed = ((Mathf.Cos((Time.time + instanceID) * 12) + 1) / 2) * topSpeed;

			Vector3 directionVector = target.transform.position - transform.position;
			Vector2 directionVector2D;

			directionVector.Normalize();
			directionVector *= actualSpeed;

			directionVector2D = new Vector2(directionVector.x, directionVector.y);

			physicsBody.AddForce(directionVector2D);
		} else if (state == "untargeted") {
			timeToNewRoamHeading -= Time.deltaTime;

			if (timeToNewRoamHeading <= 0) {
				timeToNewRoamHeading = newRoamHeadingInterval;

				randX = Random.Range(-1f, 1f);
				randY = Random.Range(-1f, 1f);
				roamHeading = new Vector3(randX, randY, 0);
				roamHeading.Normalize();
			}
			actualSpeed = ((Mathf.Cos((Time.time + instanceID) * 12) + 1) / 2) * topSpeed;

			physicsBody.AddForce(roamHeading * actualSpeed);
		}
	}

	void OnTriggerEnter2D(Collider2D col) {
		CellController cell = col.gameObject.GetComponent<CellController>();

		if (cell && cell.owner == "body") {
			cell.Infect(infectionAmount);
			dead = true;
		}
	}

	public void TargetCell(CellController cell) {
		target = cell;
	}
}
