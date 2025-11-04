using UnityEngine;

public class HealingFXScript : MonoBehaviour
{
    public Rigidbody2D rigidBody;

    public float speed;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // If the Healing FX is Active and if it hasn't reach the position goal, make it move
        if(gameObject.activeInHierarchy && transform.position.y < 0)
        {
            rigidBody.linearVelocityY = speed;
        }

        // When it reached the position goal, Deactivating it and resetting its position
        if(gameObject.activeInHierarchy && transform.position.y >= 0)
        {
            transform.position -= new Vector3(0.0f, 1.8f, 0.0f);
            rigidBody.linearVelocityY = 0.0f;
            gameObject.SetActive(false);
        }
    }
}
