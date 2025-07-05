using UnityEngine;

public class MushroomEnemy : MonoBehaviour
{
    [SerializeField] private int damageAmount = 1;
    [SerializeField] private int health = 3;
    private float lastDamageTime;

    public void TakeDamage(int amount)
    {
        health -= amount;
        Debug.Log("Грибок получил урон: " + amount + ", осталось здоровья: " + health);
        if (health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log("Грибок умер");
        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        TryDamagePlayer(collision.gameObject);
    }

    private void TryDamagePlayer(GameObject target)
    {
        Player player = target.GetComponent<Player>();
        if (player != null)
        {
            player.TakeDamage(damageAmount);
            Debug.Log("Грибок ударил и отнял: " + damageAmount);
            lastDamageTime = Time.time;
        }
    }
}
