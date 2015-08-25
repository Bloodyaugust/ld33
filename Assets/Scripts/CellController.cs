using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CellController : MonoBehaviour {

	public GameObject[] immuneBodyPrefabs;
	public GameObject VirionPrefab;
	public string owner = "body";
	public float integrity = 100;
	public float infection = 0;
	public float bodyGenerationChance = 0.05f;
	public int virusCount = 0;
	public bool generatesBodies = false;

	AudioSource infectSource;
	Transform integrityBar;
	Transform infectionBar;
	Transform generationIndicator;
	SpriteRenderer cellRenderer;
	PlayerController player;
	CellController targetCell;
	Text virusCountText;
	float maxIntegrity;
	float fullyInfectedColor = 0.4f;
	float spawnInterval = 2.5f;
	float fireInterval = 0.3f;
	float timeToSpawn = 0;
	float timeToFire = 0;
	bool dead = false;

	// Use this for initialization
	void Start () {
		infectSource = GetComponent<AudioSource>();
		integrityBar = transform.Find("Integrity");
		infectionBar = transform.Find("Infection");
		player = GameObject.Find("Player").GetComponent<PlayerController>();
		virusCountText = transform.Find("Canvas/VirusCount").GetComponent<Text>();
		cellRenderer = gameObject.GetComponent<SpriteRenderer>();

		integrity *= transform.localScale.x;
		maxIntegrity = integrity;
		spawnInterval = Mathf.Clamp(Mathf.Cos(transform.localScale.x / 1.5f), 0.1f, 2);
		fireInterval = Mathf.Clamp(Mathf.Cos(transform.localScale.x / 1.5f), 0.1f, 0.3f);

		if (owner == "body") {
			SwitchToBodyControl();

			if (generatesBodies) {
				generationIndicator = transform.Find("BodyGenerationIndicator");
				generationIndicator.GetComponent<SpriteRenderer>().enabled = true;
			}
		} else {
			SwitchToPlayerControl();
		}
	}

	// Update is called once per frame
	void Update () {
		GameObject newVirion;
		VirionController newVirionController;
		float integrityScale, infectionScale, indicatorScale;

		if (!dead) {
			if (owner == "body") {
				if (generatesBodies) {
					indicatorScale = (Mathf.Cos(Time.time * 16) * 0.1f) + 2;

					generationIndicator.transform.localScale = new Vector3(indicatorScale, indicatorScale, 1);
				}
			} else {
				timeToSpawn -= Time.deltaTime;
				timeToFire -= Time.deltaTime;

				if (timeToSpawn <= 0) {
					timeToSpawn = spawnInterval;
					virusCount++;
					virusCountText.text = virusCount + "";
				}

				if (targetCell) {
					if (targetCell.owner != "body") {
						targetCell = null;
					}
				}

				if (targetCell && virusCount > 1 && timeToFire <= 0) {
					newVirion = Instantiate(VirionPrefab, transform.position, Quaternion.identity) as GameObject;
					newVirionController = newVirion.GetComponent<VirionController>();
					newVirionController.TargetCell(targetCell);
					virusCount--;
					virusCountText.text = virusCount + "";
					timeToFire = fireInterval;
				}
			}

			integrityScale = Mathf.Lerp(0, 1, integrity / maxIntegrity);
			infectionScale = Mathf.Lerp(0, 1, infection / integrity);

			integrityBar.localScale = new Vector3(integrityScale, 1, 1);
			infectionBar.localScale = new Vector3(infectionScale, 1, 1);
		} else {
			Destroy(gameObject);
		}
	}

	void OnMouseUp() {
		if (owner == "body") {
			player.TargetCell(this);
		} else {
			player.SelectCell(this);
		}
	}

	void SwitchToPlayerControl() {
		virusCountText.enabled = true;
		owner = "player";
		infection = integrity;
		cellRenderer.color = new Color(1, fullyInfectedColor, fullyInfectedColor);
	}

	void SwitchToBodyControl() {
		virusCountText.enabled = false;
		owner = "body";
	}

	public void Target(CellController cell) {
		targetCell = cell;
	}

	public void Detarget() {
		targetCell = null;
	}

	public void Infect(float amount, Vector2 location) {
		GameObject newImmuneBody;
		Rigidbody2D newImmuneBodyRigidbody;
		float newTint = Mathf.Lerp(1, fullyInfectedColor, infection / integrity);

		infection += amount;
		if (infection / integrity >= 1) {
			SwitchToPlayerControl();
			infectSource.Play();
		}

		if (generatesBodies && Random.Range(0.0f, 1.0f) <= bodyGenerationChance) {
			newImmuneBody = Instantiate(immuneBodyPrefabs[Random.Range(0, immuneBodyPrefabs.Length)], transform.position, Quaternion.identity) as GameObject;
			newImmuneBodyRigidbody = newImmuneBody.GetComponent<Rigidbody2D>();

			Vector2 targetDirection = location - (Vector2)transform.position;
			targetDirection.Normalize();
			targetDirection = targetDirection - new Vector2(Random.Range(-0.3f, 0.3f), Random.Range(-0.3f, 0.3f));
			targetDirection.Normalize();

			newImmuneBodyRigidbody.AddForce(targetDirection * (50000 + (transform.localScale.x * 20000)), ForceMode2D.Impulse);
		}

		cellRenderer.color = new Color(1, newTint, newTint);
	}

	public void Damage(float amount) {
		integrity -= amount;
		if (integrity <= 0) {
			dead = true;
		}
	}

	public CellController GetTargetCell() {
		return targetCell;
	}
}
