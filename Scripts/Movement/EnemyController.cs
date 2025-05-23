using UnityEngine;

public class EnemyController : MonoBehaviour
{

    public Transform target;
    public int speed;
    public Hittable hp;
    public HealthBar healthui;
    public bool dead;

    public float last_attack;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        target = GameManager.Instance.player.transform;
        hp.OnDeath += Die;
        healthui.SetHealth(hp);
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 direction = target.position - transform.position;
        if (direction.magnitude < 2f)
        {
            DoAttack();
        }
        else
        {
            GetComponent<Unit>().movement = direction.normalized * speed;
        }
    }
    
    void DoAttack()
    {
        if (last_attack + 2 < Time.time)
        {
            last_attack = Time.time;
            target.gameObject.GetComponent<PlayerController>().hp.Damage(new Damage(5, Damage.Type.PHYSICAL));
        }
    }


    public void Die()
    {
        if (!dead)
        {
            dead = true;
            // Notify StatManager before removing the enemy
            if (StatManager.Instance != null)
            {
                StatManager.Instance.OnEnemyDefeated();
            }
            
            // Remove from GameManager first
            GameManager.Instance.RemoveEnemy(gameObject);
            
            // Clean up components
            if (hp != null)
            {
                hp.OnDeath -= Die; // Unsubscribe from the event
            }
            
            // Destroy the game object
            Destroy(gameObject);
            Debug.Log("Enemy destroyed and cleaned up");
        }
    }
}
