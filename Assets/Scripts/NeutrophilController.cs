using UnityEngine;
using System.Collections;

public class NeutrophilController : MonoBehaviour {

	public GameObject cytotoxinPrefab;
	public float cytotoxinClip = 30;

	SpriteRenderer neutrophilRenderer;
	string state = "idle";
	float fireInterval = 0.1f;
	float timeToFire = 0;
	float timeToDie = 1;
	float cytotoxinsFired = 0;


	// Use this for initialization
	void Start () {
		neutrophilRenderer = GetComponent<SpriteRenderer>();
	}

	// Update is called once per frame
	void Update () {
		float currentColor;

		if (state == "firing") {
			timeToFire -= Time.deltaTime;

			if (timeToFire <= 0) {
				timeToFire = fireInterval;
				cytotoxinsFired++;
				Instantiate(cytotoxinPrefab, transform.position, Quaternion.identity);

				currentColor = 1 - (cytotoxinsFired / cytotoxinClip);

				neutrophilRenderer.color = new Color(currentColor, currentColor, currentColor, 1);

				if (cytotoxinsFired == cytotoxinClip) {
					state = "dying";
				}
			}
		} else if (state == "dying") {
			timeToDie -= Time.deltaTime;

			neutrophilRenderer.color = new Color(0, 0, 0, timeToDie);

			if (timeToDie <= 0) {
				state = "dead";
			}
		} else if (state == "dead") {
			Destroy(gameObject);
		}
	}

	void OnTriggerEnter2D(Collider2D col) {
		if (state == "idle" && col.gameObject.tag == "virion") {
			state = "firing";
		}
	}
}
