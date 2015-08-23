using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MacrophageController : MonoBehaviour {

	public float maxVirionConsumes = 20;
	public float virionConsumes = 0;
	public float topSpeed = 200;

	GameObject ctlPrefab;
	GameObject neutrophilPrefab;
	Rigidbody2D physicsBody;
	VirionController targetVirion;
	ArrayList virionsInRange = new ArrayList();
	Vector3 roamHeading = new Vector3(1, 0, 0);
	float instanceID;
	float newRoamHeadingInterval = 1;
	float timeToNewRoamHeading = 0;

	// Use this for initialization
	void Start () {
		physicsBody = gameObject.GetComponent<Rigidbody2D>();
		instanceID = GetInstanceID();
	}

	// Update is called once per frame
	void Update () {
		if (virionConsumes >= maxVirionConsumes) {
			Destroy(gameObject);
		}
	}

	void FixedUpdate() {
		float actualSpeed, randX, randY;

		if (!targetVirion && virionsInRange.Count > 0) {
			for (int i = 0; i < virionsInRange.Count; i++) {
				if (virionsInRange[i] == null) {
					virionsInRange.RemoveAt(i);
					i--;
				} else {
					targetVirion = virionsInRange[i] as VirionController;
					break;
				}
			}
		}

		if (targetVirion) {
			actualSpeed = ((Mathf.Cos((Time.time + instanceID)) + 1) / 2) * topSpeed;

			Vector3 directionVector = targetVirion.transform.position - transform.position;
			Vector2 directionVector2D;

			directionVector.Normalize();
			directionVector *= actualSpeed;

			directionVector2D = new Vector2(directionVector.x, directionVector.y);

			physicsBody.AddForce(directionVector2D);
		} else {
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
		VirionController newTarget = col.gameObject.GetComponent<VirionController>();

		if (newTarget) {
			if (!targetVirion) {
				targetVirion = newTarget;
			} else {
				virionsInRange.Add(newTarget);
			}
		}
	}

	void OnCollisionEnter2D(Collision2D col) {
		if (col.gameObject.tag == "virion") {
			ConsumeVirion();
			Destroy(col.gameObject);
		}
	}

	void ConsumeVirion() {
		virionConsumes++;
	}
}
