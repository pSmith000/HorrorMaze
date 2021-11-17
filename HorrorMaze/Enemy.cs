using System;
using System.Collections.Generic;
using System.Text;
using Raylib_cs;
using MathLibrary;

namespace HorrorMaze
{
    class Enemy : Actor
    {
        //The class variables
        private float _speed;
        private Vector3 _velocity;
        private Player _player;
        float x = 10;

        /// <summary>
        /// The speed of the enemy
        /// </summary>
        public float Speed
        {
            get { return _speed; }
            set { _speed = value; }
        }

        /// <summary>
        /// The velocity of the enemy
        /// </summary>
        public Vector3 Velocity
        {
            get { return _velocity; }
            set { _velocity = value; }
        }

        /// <summary>
        /// The constructor for the enemy
        /// </summary>
        /// <param name="x">position on the x axis</param>
        /// <param name="y">position on the y axis</param>
        /// <param name="z">position on the z axis</param>
        /// <param name="speed">the speed of the enemy</param>
        /// <param name="player">the player it chases after</param>
        /// <param name="name">the name of the enemy</param>
        /// <param name="shape">the shape of the enemy</param>
        public Enemy(float x, float y, float z, float speed, Player player, string name = "Actor", Shape shape = Shape.CUBE)
            : base( x, y, z, name, shape)
        {
            _player = player;
            _speed = speed;
        }

        /// <summary>
        /// Looks at and chases the player. Scales and changes speed based off distance from the player
        /// </summary>
        /// <param name="deltaTime"></param>
        public override void Update(float deltaTime)
        {
            //The direction to move based on the players position
            Vector3 moveDirection = _player.LocalPosition - LocalPosition;

            //The distance between the player and enemy
            float distance = Vector3.Distance(_player.LocalPosition, LocalPosition);

            //The velocity of the enemy
            Velocity = moveDirection.Normalized * Speed * deltaTime;

            //Translates the enemy towards the player
            Translate(Velocity.X, Velocity.Y, Velocity.Z);

            //Always looks at the player
            LookAt(_player.WorldPosition);

            //The enemy changes speed and size based on how close it is to the player
            if (distance > 60)
            {
                foreach (Actor child in Children)
                {
                    SetScale(3, 3, 3);
                    child.SetScale(3, 3, 3);
                    _speed = 30;
                }
            }
            if (distance < 60 && distance > 50)
            {
                foreach (Actor child in Children)
                {
                    SetScale(4, 4, 4);
                    child.SetScale(4, 4, 4);
                    _speed = 55;
                }
            }
            if (distance < 50 && distance > 40)
            {
                foreach (Actor child in Children)
                {
                    SetScale(5, 5, 5);
                    child.SetScale(5, 5, 5);
                    _speed = 50;
                }
            }
            if (distance < 40 && distance > 30)
            {
                foreach (Actor child in Children)
                {
                    SetScale(7, 7, 7);
                    child.SetScale(7, 7, 7);
                }
            } 
            if (distance < 20 && distance > 10)
            {
                foreach (Actor child in Children)
                {
                    SetScale(9, 9, 9);
                    child.SetScale(9, 9, 9);
                    _speed = 5;
                }
            }

            //Sets the collider for the enemy
            AABBCollider enemyCollider = new AABBCollider(Size.X, Size.Y, Size.Z, this);
            Collider = enemyCollider;

            base.Update(deltaTime);
        }

        /// <summary>
        /// Draws from the base actor class
        /// </summary>
        public override void Draw()
        {
            base.Draw();
        }
    }
}
