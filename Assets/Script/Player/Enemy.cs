using UnityEngine;

public class Enemy : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            this.GetComponent<Rigidbody2D>().AddForce(Vector2.up * 5, ForceMode2D.Impulse);
            this.GetComponent<Rigidbody2D>().gravityScale = 1;
            this.GetComponent<Rigidbody2D>().angularVelocity = 800;
            this.GetComponent<Collider2D>().enabled = false;
        }
    }
}
