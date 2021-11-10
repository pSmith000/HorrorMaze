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
        private Vector3 _currentPosition;
        private Vector3 _velocity;
        int i = 80;

        public float Speed
        {
            get { return _speed; }
            set { _speed = value; }
        }

        public Vector3 Velocity
        {
            get { return _velocity; }
            set { _velocity = value; }
        }

        public Player(float x, float y, float z, float speed, string name = "Actor", Shape shape = Shape.CUBE) 
            : base( x, y, z, name, shape)
        {
            _speed = speed;
            _currentSpeed = speed;
        }

        public override void Update(float deltaTime)
        {
            _currentPosition = Velocity;
            //Get the player input direction
            int xDirection = Convert.ToInt32(Raylib.IsKeyDown(KeyboardKey.KEY_A))
                - Convert.ToInt32(Raylib.IsKeyDown(KeyboardKey.KEY_D));
            int zDirection = Convert.ToInt32(Raylib.IsKeyDown(KeyboardKey.KEY_W))
                - Convert.ToInt32(Raylib.IsKeyDown(KeyboardKey.KEY_S));

            int rotZRotation = Convert.ToInt32(Raylib.IsKeyDown(KeyboardKey.KEY_LEFT))
           - Convert.ToInt32(Raylib.IsKeyDown(KeyboardKey.KEY_RIGHT));




            Velocity = ((zDirection * Forward) + (xDirection * Right)).Normalized * _currentSpeed * deltaTime;

            

            _currentSpeed = Speed;

            Rotate(0, rotZRotation * 0.05f, 0);
            Translate(Velocity.X, Velocity.Y, Velocity.Z);

            base.Update(deltaTime);
            
        }

        public override void OnCollision(Actor actor)
        {
            Console.WriteLine("Collision occured " + actor.Name);

            if (actor.Name == "Wall")
            {
                Velocity = actor.Collider.CollisionNormal * Velocity.Magnitude;
                
                Translate(Velocity.X, Velocity.Y, Velocity.Z);
            }
            
        }

        public override void Draw()
        {
            base.Draw();
        }
    }
}
