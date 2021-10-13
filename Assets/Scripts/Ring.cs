using UnityEngine;

public class Ring : MonoBehaviour
{
    private void OnCollisionEnter(Collision other)
    {
        if (other.transform.GetComponent<RagDallTrower>())
        {
            Collider enemyCollider = other.transform.GetComponent<Collider>();
            Physics.IgnoreCollision(enemyCollider, transform.GetComponent<Collider>());
        }
    }
}