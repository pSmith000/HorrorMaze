using System;
using System.Collections.Generic;
using System.Text;
using Raylib_cs;
using MathLibrary;

namespace HorrorMaze
{
    class Enemy : Actor
    {
        private float _speed;
        private Vector3 _velocity;
        private Player _player;

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

        public Enemy(float x, float y, float speed, Player player, string name = "Actor", Shape shape = Shape.CUBE)
            : base( x, y, name, shape)
        {
            _player = player;
            _speed = speed;
        }

        public override void Update(float deltaTime)
        {
            
            Vector3 moveDirection = _player.LocalPosition - LocalPosition;

            Velocity = moveDirection.Normalized * Speed * deltaTime;

            Translate(Velocity.X, Velocity.Y, Velocity.Z);

            LookAt(_player.WorldPosition);

            base.Update(deltaTime);
        }

        public override void Draw()
        {
            base.Draw();
        }
    }
}
