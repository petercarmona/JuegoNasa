using UnityEngine;

public class GroundCheck : MonoBehaviour
{
    [Header("Ground Check Settings")]
    [SerializeField] private bool isGround = false; // Tracks if object touched ground
    [SerializeField] private GameObject groundHitParticle; // Particle prefab to spawn

    [Tooltip("Layer name for ground detection (e.g., 'Ground')")]
    [SerializeField] private string groundLayerName = "Ground";
    [SerializeField] private float particleOffset = 0.05f;
    private void OnCollisionEnter(Collision collision)
    {
        // Check if the collided object belongs to the "Ground" layer
        if (collision.gameObject.layer == LayerMask.NameToLayer(groundLayerName))
        {
            if (!isGround)
            {
                isGround = true;
                Debug.Log($"{gameObject.name} touched the ground!");

                // Spawn the particle at the collision contact point
                if (groundHitParticle != null && collision.contacts.Length > 0)
                {
                    Vector3 contactPoint = collision.contacts[0].point;
                    Instantiate(groundHitParticle, gameObject.transform.position + new Vector3(0,particleOffset,0), Quaternion.identity);
                }
            }
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        // Reset when the object stops touching the ground
        if (collision.gameObject.layer == LayerMask.NameToLayer(groundLayerName))
        {
            isGround = false;
        }
    }
}

