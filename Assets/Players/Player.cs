using UnityEngine;
using System.Collections;
using UnityEngine.UI;	//Allows us to use UI.
using UnityEngine.SceneManagement;

namespace Completed
{
    public class Player : MovingObject
    {
        public float restartLevelDelay = 1f;        //Delay time in seconds to restart level.
        public int pointsPerFood = 10;
        public int pointsPerSoda = 20;
        public int wallDamage = 1;
        public int enemyDamage = 2;
        public Text grapesText;
        public AudioClip moveSound1;
        public AudioClip moveSound2;
        public AudioClip eatSound1;
        public AudioClip eatSound2;
        public AudioClip drinkSound1;
        public AudioClip drinkSound2;
        public AudioClip gameOverSound;
        public Bullet bullet;

        private Animator animator;
        private int grapes;


        //Start overrides the Start function of MovingObject
        protected override void Start()
        {
            //Get a component reference to the Player's animator component
            animator = GetComponent<Animator>();

            grapes = GameManager.instance.playerFoodPoints;
            grapesText.text = "Grapes: " + grapes.ToString();
            base.Start();
        }


        //This function is called when the behaviour becomes disabled or inactive.
        private void OnDisable()
        {
            //When Player object is disabled, store the current local food total in the GameManager so it can be re-loaded in next level.
            GameManager.instance.playerFoodPoints = grapes;
        }


        private void Update()
        {
            if (!GameManager.instance.playersTurn) return;

            int horizontal = 0;
            int vertical = 0;
            bool fireLeft = false;
            bool fireRight = false;
            bool fireUp = false;
            bool fireDown = false;

            //Get input from the input manager, round it to an integer and store in horizontal to set x axis move direction
            horizontal = (int)(Input.GetAxisRaw("Horizontal"));
            vertical = (int)(Input.GetAxisRaw("Vertical"));

            fireLeft = (bool)(Input.GetKeyDown("a"));
            fireRight = (bool)(Input.GetKeyDown("d"));
            fireUp = (bool)(Input.GetKeyDown("w"));
            fireDown = (bool)(Input.GetKeyDown("s"));

            if (horizontal != 0)
            {
                vertical = 0;
            }

            //Check if moving
            if (horizontal != 0 || vertical != 0)
            {
                AttemptMove<Wall>(horizontal, vertical);
                AttemptMove<Enemy>(horizontal, vertical);
                grapes--;
            }

            if (fireLeft)
            {
                grapes--;
                grapesText.text = "Grapes: " + grapes;
                CheckIfGameOver();
                bullet.velY = 0;
                bullet.velX = -5;
                Instantiate(bullet, transform.position, Quaternion.identity);
            }
            if (fireRight)
            {
                grapes--;
                grapesText.text = "Grapes: " + grapes;
                CheckIfGameOver();
                bullet.velX = 5;
                bullet.velY = 0;
                Instantiate(bullet, transform.position, Quaternion.identity);
            }
            if (fireUp)
            {
                grapes--;
                grapesText.text = "Grapes: " + grapes;
                CheckIfGameOver();
                bullet.velX = 0;
                bullet.velY = 5;
                Instantiate(bullet, transform.position, Quaternion.identity);
            }
            if (fireDown)
            {
                grapes--;
                grapesText.text = "Grapes: " + grapes;
                CheckIfGameOver();
                bullet.velX = 0;
                bullet.velY = -5;
                Instantiate(bullet, transform.position, Quaternion.identity);
            }
        }

        //AttemptMove overrides the AttemptMove function in the base class MovingObject
        //AttemptMove takes a generic parameter T which for Player will be of the type Wall, it also takes integers for x and y direction to move in.
        protected override void AttemptMove<T>(int xDir, int yDir)
        {
            grapesText.text = "Grapes: " + grapes;

            base.AttemptMove<T>(xDir, yDir);

            //Hit allows us to reference the result of the Linecast done in Move.
            RaycastHit2D hit;

            if (Move(xDir, yDir, out hit))
            {
                SoundManager.instance.RandomizeSfx(moveSound1, moveSound2);
            }

            CheckIfGameOver();

            //Set the playersTurn boolean of GameManager to false now that players turn is over.
            GameManager.instance.playersTurn = false;
        }


        protected override void OnCantMove<T>(T component)
        {

            if (component is Wall)
            {
                Wall hitWall = component as Wall;
                hitWall.DamageWall(wallDamage);
            }
            else if (component is Enemy)
            {
                Enemy hitEnemy = component as Enemy;
                hitEnemy.DamageEnemy(wallDamage);
            }
            //Set the attack trigger of the player's animation controller in order to play the player's attack animation.
            animator.SetTrigger("playerChop");
        }


        //OnTriggerEnter2D is sent when another object enters a trigger collider attached to this object (2D physics only).
        private void OnTriggerEnter2D(Collider2D other)
        {
            //Check if the tag of the trigger collided with is Exit.
            if (other.tag == "Exit")
            {
                //Invoke the Restart function to start the next level with a delay of restartLevelDelay (default 1 second).
                Invoke("Restart", restartLevelDelay);

                //Disable the player object since level is over.
                enabled = false;
            }

            //Check if the tag of the trigger collided with is Food.
            else if (other.tag == "Food")
            {
                grapes += pointsPerFood;
                grapesText.text = "+" + pointsPerFood + " Grapes: " + grapes;

                SoundManager.instance.RandomizeSfx(eatSound1, eatSound2);

                //Disable the food object the player collided with.
                other.gameObject.SetActive(false);
            }

            else if (other.tag == "Soda")
            {
                grapes += pointsPerSoda;
                grapesText.text = "+" + pointsPerSoda + " Grapes: " + grapes;

                SoundManager.instance.RandomizeSfx(drinkSound1, drinkSound2);
                other.gameObject.SetActive(false);
            }
        }


        //Restart reloads the scene when called.
        private void Restart()
        {
            //Load the last scene loaded, in this case Main, the only scene in the game. And we load it in "Single" mode so it replace the existing one
            //and not load all the scene object in the current scene.
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex, LoadSceneMode.Single);
        }


        public void LoseFood(int loss)
        {
            //Set the trigger for the player animator to transition to the playerHit animation.
            animator.SetTrigger("playerHit");

            grapes -= loss;
            grapesText.text = "-" + loss + " Grapes: " + grapes;
            CheckIfGameOver();
        }
        private void CheckIfGameOver()
        {
            if (grapes > 80)
            {
                animator.SetTrigger("playerIdle");
            }
            if (grapes <= 80 && grapes >= 40)
            {
                animator.SetTrigger("halfHealth");
            }
            if (grapes < 40)
            {
                animator.SetTrigger("lowHealth");
            }
            if (grapes <= 0)
            {
                SoundManager.instance.PlaySingle(gameOverSound);
                SoundManager.instance.musicSource.Stop();
                GameManager.instance.GameOver();
            }
        }
    }
}

