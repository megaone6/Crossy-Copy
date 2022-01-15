using UnityEngine;

public class PlayerScript : MonoBehaviour
{
	private const float MAX_SWIPE_TIME = 0.5f;
	private const float MIN_SWIPE_DISTANCE = 0.1f;

	private Animator animator;
	private Rigidbody rb;
	private Vector3 posTmp;
	private Vector2 startPos;
	private float startTime;
	private int currentPos;
	private int direction;

	public int score;

	[SerializeField] public LevelGenerator levelGen;
	[SerializeField] public CameraMovement camMove;
	[SerializeField] public DisplayScore displayScore;

	// Start is called before the first frame update
	void Start()
    {
        animator = GetComponent<Animator>();
		rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
		if (!GetComponent<Renderer>().isVisible && camMove.timestamp < Time.realtimeSinceStartup)
        {
			displayScore.GameOver(score);
			camMove.SetDeath();
			Destroy(gameObject);
		}
		if (Input.GetKeyDown(KeyCode.D))
        {
			MovePlayer("hor", -1);
			direction = 0;
			camMove.FollowPlayerRight();
		}
		if (Input.GetKeyDown(KeyCode.A))
		{
			MovePlayer("hor", 1);
			direction = 1;
			camMove.FollowPlayerLeft();
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
				camMove.FollowPlayerForward();
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
						camMove.FollowPlayerRight();
						direction = 0;
					}
					else
                    {
						MovePlayer("hor", 1);
						camMove.FollowPlayerLeft();
						direction = 1;
					}
				}
				else
				{
					if (swipe.y > 0)
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
							camMove.FollowPlayerForward();
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
				camMove.FollowPlayerLeft();
			else if (direction == 1)
				camMove.FollowPlayerRight();
			rb.MovePosition(posTmp);
		}
		if (collision.collider.GetComponent<KillOnContact>() != null)
        {
			camMove.SetDeath();
			displayScore.GameOver(score);
		}
			
	}

	private void MovePlayer(string alignment, int translate)
    {
		animator.SetTrigger("move");
		if (alignment == "hor")
        {
			Vector3 tmp = new Vector3(0, 0, translate);
			tmp = tmp.normalized;
			posTmp = transform.position;
			rb.MovePosition(transform.position + tmp);
		}
		else if (alignment == "vert")
        {
			Vector3 tmp = new Vector3(translate, 0, 0);
			tmp = tmp.normalized;
			posTmp = transform.position;
			rb.MovePosition(transform.position + tmp);
		}
    }
}
