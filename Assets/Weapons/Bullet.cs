using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Completed
{
    //public class Bullet : MovingObject
    //{
    //    static private int baseDamage = 1;
    //    public int wallDamageModifier = 0;
    //    public int enemyDamageModifier = 0;
    //    public int wallDamage;                  //How much damage a player does to a wall when chopping it.
    //    public int enemyDamage;
    //    public bool isEnemyShot = false;

    //protected override void Start()
    //{

    //    GameManager.instance.AddBulletToList(this);

    //    wallDamage = baseDamage + wallDamageModifier;
    //    enemyDamage = baseDamage + enemyDamageModifier;
    //    base.Start();
    //}

    //protected override void OnCantMove<T>(T component)
    //{
    //    //throw new System.NotImplementedException();
    //    if (component is Wall)
    //    {
    //        Wall hitWall = component as Wall;
    //        hitWall.DamageWall(wallDamage);
    //    }
    //    else if (component is Enemy)
    //    {
    //        Enemy hitEnemy = component as Enemy;
    //        hitEnemy.DamageEnemy(enemyDamage);
    //    }
    //}

    //    private void Update()
    //    {

    //    }
    //}

    public class Bullet : MovingObject
    {
        public float velX;
        public float velY = 0f;
        Rigidbody2D rb2d;

        static private int baseDamage = 1;
        public int wallDamageModifier = 0;
        public int enemyDamageModifier = 0;
        public int wallDamage;                  //How much damage a player does to a wall when chopping it.
        public int enemyDamage;
        public bool isEnemyShot = false;

        //private void Start()
        protected override void Start()
        {
            rb2d = GetComponent<Rigidbody2D>();
            wallDamage = baseDamage + wallDamageModifier;
            enemyDamage = baseDamage + enemyDamageModifier;

            base.Start();
        }

        private void Update()
        {
            rb2d.velocity = new Vector2(velX, velY);
        }

        protected override void OnCantMove<T>(T component)
        {
            //throw new System.NotImplementedException();
            if (component is Wall)
            {
                Wall hitWall = component as Wall;
                hitWall.DamageWall(wallDamage);
            }
            else if (component is Enemy)
            {
                Enemy hitEnemy = component as Enemy;
                hitEnemy.DamageEnemy(enemyDamage);
            }
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

        //protected override void AttemptMove<T>(int xDir, int yDir)
        //{

        //    base.AttemptMove<T>(xDir, yDir);

        //    //Hit allows us to reference the result of the Linecast done in Move.
        //    //RaycastHit2D hit;

        //    //if (Move(xDir, yDir, out hit))
        //    //{
        //    //    SoundManager.instance.RandomizeSfx(moveSound1, moveSound2);
        //    //}

        //    //CheckIfGameOver();

        //    //Set the playersTurn boolean of GameManager to false now that players turn is over.
        //    //GameManager.instance.playersTurn = false;
        //}

    }
}