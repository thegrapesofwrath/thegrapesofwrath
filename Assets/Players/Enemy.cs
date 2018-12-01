using UnityEngine;
using System.Collections;

namespace Completed
{
    public class Enemy : MovingObject
    {
        public int playerDamage;
        public AudioClip attackSound1;
        public AudioClip attackSound2;
        public AudioClip chopSound1;
        public AudioClip chopSound2;


        private Animator animator;
        private Transform target;
        private bool skipMove;
        public int hp = 3;


        protected override void Start()
        {
            GameManager.instance.AddEnemyToList(this);
            animator = GetComponent<Animator>();

            //Find the Player GameObject using it's tag and store a reference to its transform component.
            target = GameObject.FindGameObjectWithTag("Player").transform;

            base.Start();
        }


        protected override void AttemptMove<T>(int xDir, int yDir)
        {
            if (skipMove)
            {
                skipMove = false;
                return;

            }
            base.AttemptMove<T>(xDir, yDir);
            skipMove = true;
        }


        //MoveEnemy is called by the GameManger each turn to tell each Enemy to try to move towards the player.
        public void MoveEnemy()
        {
            int xDir = 0;
            int yDir = 0;

            //If the difference in positions is approximately zero (Epsilon) do the following:
            if (Mathf.Abs(target.position.x - transform.position.x) < float.Epsilon)

                //If the y coordinate of the target's (player) position is greater than the y coordinate of this enemy's position set y direction 1 (to move up). If not, set it to -1 (to move down).
                yDir = target.position.y > transform.position.y ? 1 : -1;

            //If the difference in positions is not approximately zero (Epsilon) do the following:
            else
                //Check if target x position is greater than enemy's x position, if so set x direction to 1 (move right), if not set to -1 (move left).
                xDir = target.position.x > transform.position.x ? 1 : -1;

            //Call the AttemptMove function and pass in the generic parameter Player, because Enemy is moving and expecting to potentially encounter a Player
            AttemptMove<Player>(xDir, yDir);
        }

        public void DamageEnemy(int loss)
        {
            SoundManager.instance.RandomizeSfx(chopSound1, chopSound2);

            //Set spriteRenderer to the damaged wall sprite.
            //spriteRenderer.sprite = dmgSprite;

            hp -= loss;

            if (hp <= 0)
            {
                gameObject.SetActive(false);
            }
        }

        protected override void OnCantMove<T>(T component)
        {
            Player hitPlayer = component as Player;
            hitPlayer.LoseFood(playerDamage);
            animator.SetTrigger("enemyAttack");
            SoundManager.instance.RandomizeSfx(attackSound1, attackSound2);
        }
    }
}
