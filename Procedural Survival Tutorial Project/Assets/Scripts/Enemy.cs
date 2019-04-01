using UnityEngine;
using System.Collections;

    //Enemy inherits from MovingObject, our base class for objects that can move, Player also inherits from this.
    public class Enemy : MovingObject
    {
        public int playerDamage;                            //The amount of food points to subtract from the player when attacking.
        
        
        private Animator animator;                          //Variable of type Animator to store a reference to the enemy's Animator component.
        private Transform target;                           //Transform to attempt to move toward each turn.
        private bool skipMove;                              //Boolean to determine whether or not enemy should skip a turn or move this turn.
        
        
        //Start overrides the virtual Start function of the base class.
        protected override void Start ()
        {
            GameManager.instance.AddEnemyToList (this);
            animator = GetComponent<Animator> ();
            target = GameObject.FindGameObjectWithTag ("Player").transform;
            
            base.Start ();
        }
        
        
        protected override void AttemptMove <T> (int xDir, int yDir)
        {
            if(skipMove)
            {
                skipMove = false;
                return;
                
            }
            
            base.AttemptMove <T> (xDir, yDir);
            
            skipMove = true;
        }
        
        
        public void MoveEnemy ()
        {
            int xDir = 0;
            int yDir = 0;
            
            if(Mathf.Abs (target.position.x - transform.position.x) < float.Epsilon)
                
                yDir = target.position.y > transform.position.y ? 1 : -1;
            
            //If the difference in positions is not approximately zero (Epsilon) do the following:
            else
                //Check if target x position is greater than enemy's x position, if so set x direction to 1 (move right), if not set to -1 (move left).
                xDir = target.position.x > transform.position.x ? 1 : -1;
            
            //Call the AttemptMove function and pass in the generic parameter Player, because Enemy is moving and expecting to potentially encounter a Player
            AttemptMove <Player> (xDir, yDir);
        }
        
        
        //OnCantMove is called if Enemy attempts to move into a space occupied by a Player, it overrides the OnCantMove function of MovingObject 
        //and takes a generic parameter T which we use to pass in the component we expect to encounter, in this case Player
        protected override void OnCantMove <T> (T component)
        {
            //Declare hitPlayer and set it to equal the encountered component.
            Player hitPlayer = component as Player;
            
            //Call the LoseFood function of hitPlayer passing it playerDamage, the amount of foodpoints to be subtracted.
            hitPlayer.LoseFood (playerDamage);
            
            //Set the attack trigger of animator to trigger Enemy attack animation.
            animator.SetTrigger ("enemyAttack");

        }
    }