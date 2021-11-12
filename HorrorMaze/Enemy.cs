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
        float x = 10;

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

        public Enemy(float x, float y, float z, float speed, Player player, string name = "Actor", Shape shape = Shape.CUBE)
            : base( x, y, z, name, shape)
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


            if ((moveDirection > -50 && moveDirection < -40) || (moveDirection < 50 && moveDirection > 40))
            {
                foreach (Actor child in Children)
                {
                    SetScale(4, 4, 4);
                    child.SetScale(4, 4, 4);
                }
            }
            if (moveDirection > -40 && moveDirection < -30)
            {
                foreach (Actor child in Children)
                {
                    SetScale(5, 5, 5);
                    child.SetScale(5, 5, 5);
                }
            }
            if (moveDirection > -30 && moveDirection < -20)
            {
                foreach (Actor child in Children)
                {
                    SetScale(7, 7, 7);
                    child.SetScale(7, 7, 7);
                }
            } 
            if (moveDirection > -10 && moveDirection < -9)
            {
                foreach (Actor child in Children)
                {
                    SetScale(11, 11, 11);
                    child.SetScale(11, 11, 11);
                }
            }
            if (moveDirection > -9 && moveDirection < -7)
            {
                foreach (Actor child in Children)
                {
                    SetScale(12, 12, 12);
                    child.SetScale(12, 12, 12);
                }
            }

            AABBCollider enemyCollider = new AABBCollider(Size.X, Size.Y, Size.Z, this);
            CircleCollider sizeChangeRadius1 = new CircleCollider(20, this);
            Collider = enemyCollider;

            base.Update(deltaTime);
        }

        public override void Draw()
        {
            base.Draw();
        }
    }
}
