using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour, IDamageable
{
    public float health = 100f;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Death();
    }

    void Death()
    {
        if (health <= 0f)
        {
            Destroy(this.gameObject);
        }
    }

    public void TakeDamage(float damage)
    {
        if (damage > 0)
        {
            health -= damage;
        }
    }
}
