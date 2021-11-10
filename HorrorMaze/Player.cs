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

        public Player(float x, float y, float speed, string name = "Actor", Shape shape = Shape.CUBE) 
            : base( x, y, name, shape)
        {
            _speed = speed;
        }

        public override void Update(float deltaTime)
        {
            //Get the player input direction
            int xDirection = Convert.ToInt32(Raylib.IsKeyDown(KeyboardKey.KEY_A))
                - Convert.ToInt32(Raylib.IsKeyDown(KeyboardKey.KEY_D));
            int zDirection = Convert.ToInt32(Raylib.IsKeyDown(KeyboardKey.KEY_W))
                - Convert.ToInt32(Raylib.IsKeyDown(KeyboardKey.KEY_S));

            int rotZRotation = Convert.ToInt32(Raylib.IsKeyDown(KeyboardKey.KEY_LEFT))
           - Convert.ToInt32(Raylib.IsKeyDown(KeyboardKey.KEY_RIGHT));




            Velocity = (zDirection * Forward.Normalized / 2) + (xDirection * Right.Normalized) * Speed * deltaTime;


            Rotate(0, rotZRotation * 0.05f, 0);
            Translate(Velocity.X, Velocity.Y, Velocity.Z);

            base.Update(deltaTime);
            
        }

        public override void OnCollision(Actor actor)
        {
            Console.WriteLine("Collision occured " + actor.Name);
            
        }

        public override void Draw()
        {
            base.Draw();
        }
    }
}
