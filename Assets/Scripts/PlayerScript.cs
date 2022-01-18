using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
	private const float MAX_SWIPE_TIME = 0.5f;
	private const float MIN_SWIPE_DISTANCE = 0.1f;

	private Animator animator;
	private Rigidbody rb;
	public Vector3 posTmp;
	private Vector2 startPos;
	private float startTime;
	private int currentPos;
	private int direction;
	private bool exitLog;
	private int coins;
	private SaveObject savedObject;
	private SaveObject loadedObject;
	private SelectedCharacter selectedChar;
	private bool asd; //temporary

	public int score;

	[SerializeField] public LevelGenerator levelGen;
	[SerializeField] public CameraMovement camMove;
	[SerializeField] public DisplayScore displayScore;

	// Start is called before the first frame update
	private void Start()
    {
		asd = false;
		selectedChar = loadCharacter();
		if (gameObject.name != selectedChar.name)
        {
			Destroy(gameObject);
        }
        animator = GetComponent<Animator>();
		rb = GetComponent<Rigidbody>();
		exitLog = false;
		loadedObject = loadCoins();
		coins = loadedObject.coinAmount;
	}

    // Update is called once per frame
    private void Update()
    {
		if(!asd)
        {
			rb.constraints = RigidbodyConstraints.FreezeRotation;
			asd = true;
		}
		if (!GetComponent<Renderer>().isVisible && camMove.timestamp < Time.realtimeSinceStartup)
        {
			saveCoins();
			displayScore.GameOver(score);
			camMove.SetDeath();
			Destroy(gameObject);
		}
		if (Input.GetKeyDown(KeyCode.D))
        {
			MovePlayer("hor", -1);
			direction = 0;
			camMove.FollowPlayerRight(Math.Abs(camMove.transform.position.x - transform.position.x));
		}
		if (Input.GetKeyDown(KeyCode.A))
		{
			MovePlayer("hor", 1);
			direction = 1;
			camMove.FollowPlayerLeft(Math.Abs(camMove.transform.position.x - transform.position.x));
		}
		if (Input.GetKeyDown(KeyCode.W))
		{
			MovePlayer("vert", 1);
			if (score == currentPos)
			{
				score += 1;
				displayScore.UpdateScore(score);
				if (currentPos % 2 == 1)
                {
					levelGen.GenerateLevel();
					levelGen.DestroyTerrain();
				}
				direction = 2;
				camMove.FollowPlayerForward(Math.Abs(camMove.transform.position.x - transform.position.x));
			}
			currentPos += 1;
		}
		if (Input.GetKeyDown(KeyCode.S))
		{
			MovePlayer("vert", -1);
			currentPos -= 1;
		}
		if (Input.touches.Length > 0)
		{
			Touch t = Input.GetTouch(0);
			if (t.phase == TouchPhase.Began)
			{
				startPos = new Vector2(t.position.x / Screen.width, t.position.y / Screen.width);
				startTime = Time.time;
			}
			if (t.phase == TouchPhase.Ended)
			{
				if (Time.time - startTime > MAX_SWIPE_TIME)
					return;

				Vector2 endPos = new Vector2(t.position.x / Screen.width, t.position.y / Screen.width);

				Vector2 swipe = new Vector2(endPos.x - startPos.x, endPos.y - startPos.y);

				if (swipe.magnitude < MIN_SWIPE_DISTANCE)
					return;

				if (Mathf.Abs(swipe.x) > Mathf.Abs(swipe.y))
				{
					if (swipe.x > 0)
                    {
						MovePlayer("hor", -1);
						camMove.FollowPlayerRight(Math.Abs(camMove.transform.position.x - transform.position.x));
						direction = 0;
					}
					else
                    {
						MovePlayer("hor", 1);
						camMove.FollowPlayerLeft(Math.Abs(camMove.transform.position.x - transform.position.x));
						direction = 1;
					}
				}
				else
				{
					if (swipe.y > 0)
					{
						gameObject.transform.parent = null;
						MovePlayer("vert", 1);
						if (score == currentPos)
						{
							score += 1;
							displayScore.UpdateScore(score);
							if (currentPos % 2 == 1)
                            {
								levelGen.GenerateLevel();
								levelGen.DestroyTerrain();
							}
							direction = 2;
							camMove.FollowPlayerForward(Math.Abs(camMove.transform.position.x - transform.position.x));
						}
						currentPos += 1;
					}
					else
					{
						MovePlayer("vert", -1);
						currentPos -= 1;
					}
				}
			}
		}
	}
	private void OnCollisionEnter(Collision collision)
	{
		if (collision.gameObject.name.Contains("Tree"))
        {
			if (direction == 0)
				camMove.FollowPlayerLeft(Math.Abs(camMove.transform.position.x - transform.position.x));
			else if (direction == 1)
				camMove.FollowPlayerRight(Math.Abs(camMove.transform.position.x - transform.position.x));
			rb.MovePosition(posTmp);
		}
		if (collision.collider.GetComponent<KillOnContact>() != null)
        {
			saveCoins();
			camMove.SetDeath();
			displayScore.GameOver(score);
		}
		if (collision.gameObject.name.Contains("LogLeft"))
		{
			exitLog = true;
			camMove.followOnLog = "left";
		}
		else if (collision.gameObject.name.Contains("LogRight"))
        {
			exitLog = true;
			camMove.followOnLog = "right";
		}
		if (collision.gameObject.name.Contains("Coin"))
		{
			Destroy(collision.gameObject);
			coins++;
		}
	}

	private void MovePlayer(string alignment, int translate)
    {
		if (alignment == "hor")
        {
			Vector3 tmp = new Vector3(0, 0, translate);
			tmp = tmp.normalized;
			posTmp = transform.position;
			rb.MovePosition(transform.position + tmp);
		}
		else if (alignment == "vert")
        {
			if (!exitLog)
            {
				Vector3 tmp = new Vector3(translate, 0, 0);
				tmp = tmp.normalized;
				posTmp = transform.position;
				rb.MovePosition(transform.position + tmp);
			}
			else
            {
				Destroy(gameObject.GetComponent<HingeJoint>());
				gameObject.GetComponent<Rigidbody>().useGravity = false;
				gameObject.GetComponent<Rigidbody>().velocity = new Vector3(0,0,0);
				if (gameObject.name == "Player")
					transform.Translate(1, 1, 0);
				else
					transform.Translate(0, 1, 1);
				Vector3 tmp = transform.position;
				tmp.x = Mathf.Round(tmp.x);
				tmp.y = Mathf.Round(tmp.y);
				tmp.z = Mathf.Round(tmp.z);
				transform.position = tmp;
				exitLog = false;
				gameObject.GetComponent<Rigidbody>().useGravity = true;
				camMove.followOnLog = "none";
			}
		}
		animator.SetTrigger("move");
	}

	private void saveCoins()
    {
		savedObject = new SaveObject
		{
			coinAmount = coins
		};
		string json = JsonUtility.ToJson(savedObject);
		File.WriteAllText(Application.persistentDataPath + "/save.json", json);
	}

	private SaveObject loadCoins()
	{
		if (File.Exists(Application.persistentDataPath + "/save.json"))
        {
			string saveString = File.ReadAllText(Application.persistentDataPath + "/save.json");
			return JsonUtility.FromJson<SaveObject>(saveString);
        }
		else
        {
			return new SaveObject {
				coinAmount = 0
			};
        }
	}

	private SelectedCharacter loadCharacter()
	{
		string characterString = File.ReadAllText(Application.persistentDataPath + "/selectedCharacter.json");
		return JsonUtility.FromJson<SelectedCharacter>(characterString);
	}

	private class SaveObject
    {
		public int coinAmount;
    }

	private class SelectedCharacter
	{
		public int id;
		public String name;
	}
}
