using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Completed
{
    public class Bullet : MonoBehaviour
    {
        public float velX;
        public float velY = 0f;
        Rigidbody2D rb2d;

        static private int baseDamage = 1;
        public int wallDamageModifier = 0;
        public int enemyDamageModifier = 0;
        public int wallDamage;
        public int enemyDamage;

        private void Start()
        {
            rb2d = GetComponent<Rigidbody2D>();
            wallDamage = baseDamage + wallDamageModifier;
            enemyDamage = baseDamage + enemyDamageModifier;
        }

        private void Update()
        {
            rb2d.velocity = new Vector2(velX, velY);
        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            //all projectile colliding game objects should be tagged "Enemy" or whatever in inspector but that tag must be reflected in the below if conditional
            if (col.gameObject.tag == "Wall")
            {
                Wall hitWall = col.GetComponent<Wall>();
                hitWall.DamageWall(wallDamage);
                Destroy(gameObject);
            }
            if (col.gameObject.tag == "OuterWall")
            {
                Destroy(gameObject);
            }
            if (col.gameObject.tag == "Enemy")
            {
                Enemy hitEnemy = col.GetComponent<Enemy>();
                hitEnemy.DamageEnemy(enemyDamage);
                Destroy(gameObject);
            }
        }
    }
}