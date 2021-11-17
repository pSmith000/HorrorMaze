using System;
using System.Collections.Generic;
using System.Text;
using MathLibrary;
using Raylib_cs;

namespace HorrorMaze
{
    class Player : Actor
    {
        private float _speed;
        private float _currentSpeed;
        private Vector3 _velocity;

        /// <summary>
        /// The speed of the player
        /// </summary>
        public float Speed
        {
            get { return _speed; }
            set { _speed = value; }
        }

        /// <summary>
        /// The velocity of the player
        /// </summary>
        public Vector3 Velocity
        {
            get { return _velocity; }
            set { _velocity = value; }
        }

        /// <summary>
        /// The constructor of the player
        /// </summary>
        /// <param name="x">the position on the x axis</param>
        /// <param name="y">the position on the y axis</param>
        /// <param name="z">the position on the z axis</param>
        /// <param name="speed">The speed of the player</param>
        /// <param name="name">The name of the player</param>
        /// <param name="shape">The shape of the player</param>
        public Player(float x, float y, float z, float speed, string name = "Actor", Shape shape = Shape.CUBE) 
            : base( x, y, z, name, shape)
        {
            _speed = speed;
            _currentSpeed = speed;
        }

        /// <summary>
        /// Moves the player and rotates the camera
        /// </summary>
        /// <param name="deltaTime"></param>
        public override void Update(float deltaTime)
        {
            //Get the player input direction
            int xDirection = Convert.ToInt32(Raylib.IsKeyDown(KeyboardKey.KEY_A))
                - Convert.ToInt32(Raylib.IsKeyDown(KeyboardKey.KEY_D));
            int zDirection = Convert.ToInt32(Raylib.IsKeyDown(KeyboardKey.KEY_W))
                - Convert.ToInt32(Raylib.IsKeyDown(KeyboardKey.KEY_S));

            //Get the player input rotation
            int rotZRotation = Convert.ToInt32(Raylib.IsKeyDown(KeyboardKey.KEY_LEFT))
           - Convert.ToInt32(Raylib.IsKeyDown(KeyboardKey.KEY_RIGHT));

            //Velocity based off of the forward and the right
            Velocity = ((zDirection * Forward) + (xDirection * Right)).Normalized * _currentSpeed * deltaTime;
            //The current speed
            _currentSpeed = Speed;

            //Rotates and translates based off of the input
            Rotate(0, rotZRotation * 0.05f, 0);
            Translate(Velocity.X, 0, Velocity.Z);

            base.Update(deltaTime);
            
        }

        /// <summary>
        /// What happens when the player collides with another actor
        /// </summary>
        /// <param name="actor">the actor the player is colliding with</param>
        public override void OnCollision(Actor actor)
        {
            //If the actor is a wall...
            if (actor.Name == "Wall")
            {
                //...Push the player back
                Velocity = actor.Collider.CollisionNormal * Velocity.Magnitude;
                Translate(Velocity.X, Velocity.Y, Velocity.Z);
            }
            //If the actor is an enemy...
            if (actor.Name == "Enemy")
                //...close the application
                Engine.CloseApplication();
            //If the actor is the win circle...
            if (actor.Name == "Win")
                //...close the application
                Engine.CloseApplication();
            
        }

        /// <summary>
        /// Draw from the base actor
        /// </summary>
        public override void Draw()
        {
            base.Draw();
        }
    }
}
