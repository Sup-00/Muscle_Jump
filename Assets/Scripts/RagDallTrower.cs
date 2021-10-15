using UnityEngine;

public class RagDallTrower : MonoBehaviour
{
    private void OnCollisionEnter(Collision other)
    {
        if (other.transform.GetComponent<CharectorMoving>())
        {
            Collider enemyCollider = other.transform.GetComponent<Collider>();
            Physics.IgnoreCollision(enemyCollider, transform.GetComponent<Collider>());
        }
    }
}