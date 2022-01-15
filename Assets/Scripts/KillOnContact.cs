using UnityEngine;

public class KillOnContact : MonoBehaviour
{
    [SerializeField] private DisplayScore displayScore;
    [SerializeField] private PlayerScript playerScript;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.GetComponent<PlayerScript>() != null)
        {
            Destroy(collision.gameObject);
        }
    }
}
