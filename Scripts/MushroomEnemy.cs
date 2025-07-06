using UnityEngine;

public class MushroomEnemy : MonoBehaviour
{
    [SerializeField] private int damageAmount = 1;
    [SerializeField] private int health = 3;
    private float lastDamageTime;

    private Animator animator;
    private bool isDead = false;
    private Collider2D col;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        col = GetComponent<Collider2D>();
    }

    public void TakeDamage(int amount)
    {
        if (isDead) return; // Не наносим урон, если уже мёртв

        health -= amount;
        Debug.Log("Грибок получил урон: " + amount + ", осталось здоровья: " + health);

        if (health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        if (isDead) return;

        isDead = true;
        Debug.Log("Грибок умер");

        animator.SetTrigger("isDead");

        if (col != null)
            col.enabled = false; 
        Destroy(gameObject, 1f);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isDead) return;

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
