using UnityEngine;

public class Bomb : MonoBehaviour
{
    public GameObject particle = null;
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Tile") ||
            collision.CompareTag("Player"))
        {
            var p = Instantiate(particle, transform.position, Quaternion.identity, null);
            Destroy(p, 1f);
            Destroy(gameObject);
        }
    }
}
