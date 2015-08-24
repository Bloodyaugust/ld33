using UnityEngine;
using System.Collections;

public class CytotoxinController : MonoBehaviour {

	// Use this for initialization
	void Start () {
		float randX, randY;

		randX = Random.Range(-1f, 1f);
		randY = Random.Range(-1f, 1f);

		Vector3 directionVector = new Vector3(randX, randY, 0);

		directionVector.Normalize();

		GetComponent<Rigidbody2D>().AddForce(directionVector * Random.Range(150, 850), ForceMode2D.Impulse);
	}

	// Update is called once per frame
	void Update () {

	}

	void OnCollisionEnter2D(Collision2D col) {
		if (col.gameObject.tag == "virion") {
			Destroy(col.gameObject);
			Destroy(gameObject);
		}
	}
}
