using UnityEngine;

public class EnemyPichAttackController : MonoBehaviour
{

    private bool playerInRange = false;
    public bool PlayerInRange { get { return playerInRange; } } 


    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.GetComponent<Player>() != null)
        {
            playerInRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.GetComponent<Player>() != null)
        {
            playerInRange = false;
        }
    }

}
