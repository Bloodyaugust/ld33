using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CellController : MonoBehaviour {

	public GameObject VirionPrefab;
	public string owner = "body";
	public float integrity = 100;
	public float infection = 0;
	public float spawnInterval = 2.5f;
	public float fireInterval = 0.3f;
	public int virusCount = 0;

	Transform integrityBar;
	Transform infectionBar;
	SpriteRenderer cellRenderer;
	PlayerController player;
	CellController targetCell;
	Text virusCountText;
	float maxIntegrity;
	float fullyInfectedColor = 0.4f;
	float timeToSpawn = 0;
	float timeToFire = 0;
	bool dead = false;

	// Use this for initialization
	void Start () {
		integrityBar = transform.Find("Integrity");
		infectionBar = transform.Find("Infection");
		player = GameObject.Find("Player").GetComponent<PlayerController>();
		virusCountText = transform.Find("Canvas/VirusCount").GetComponent<Text>();
		cellRenderer = gameObject.GetComponent<SpriteRenderer>();

		integrity *= transform.localScale.x;
		maxIntegrity = integrity;
		spawnInterval = (Mathf.Acos(-transform.localScale.x / 4) * -1) + 3.1f;

		if (owner == "body") {
			SwitchToBodyControl();
		} else {
			SwitchToPlayerControl();
		}
	}

	// Update is called once per frame
	void Update () {
		GameObject newVirion;
		VirionController newVirionController;
		float integrityScale, infectionScale;

		if (!dead) {
			if (owner == "body") {

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

	public void Infect(float amount) {
		float newTint = Mathf.Lerp(1, fullyInfectedColor, infection / integrity);

		infection += amount;
		if (infection / integrity >= 1) {
			SwitchToPlayerControl();
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
