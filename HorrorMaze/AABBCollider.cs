using System;
using System.Collections.Generic;
using System.Text;
using MathLibrary;
using Raylib_cs;

namespace HorrorMaze
{
    class AABBCollider : Collider
    {
        private float _width;
        private float _height;
        private float _depth;

        /// <summary>
        /// The size of this collider on the x axis
        /// </summary>
        public float Width
        {
            get { return _width; }
            set { _width = value; }
        }

        /// <summary>
        /// The size of this collider on the y axis
        /// </summary>
        public float Height
        {
            get { return _height; }
            set { _height = value; }
        }

        public float Depth
        {
            get { return _depth; }
            set { _depth = value; }
        }

        /// <summary>
        /// The farthest left x position of this collider
        /// </summary>
        public float Left
        {
            get
            {
                return Owner.LocalPosition.X - Width / 2;
            }
        }

        /// <summary>
        /// The farthest right x position of this collider
        /// </summary>
        public float Right
        {
            get
            {
                return Owner.LocalPosition.X + Width / 2; 
            }
        }

        /// <summary>
        /// The farthest y position upwards
        /// </summary>
        public float Top
        {
            get
            {
                return Owner.LocalPosition.Y + Height / 2;
            }
        }

        /// <summary>
        /// The farthest y position downwards
        /// </summary>
        public float Bottom
        {
            get
            {
                return Owner.LocalPosition.Y - Height / 2;
            }
        }

        public float Front
        {
            get
            {
                return Owner.LocalPosition.Z + Depth / 2;
            }
        }

        public float Back
        {
            get
            {
                return Owner.LocalPosition.Z - Depth / 2;
            }
        }

        public AABBCollider(float width, float height, float depth, Actor owner) : base (owner, ColliderType.AABB)
        {
            _width = width;
            _height = height;
            _depth = depth;
        }

        public override bool CheckCollisionAABB(AABBCollider other)
        {
            //Return false if this owner is checking for a collision against itself
            if (other.Owner == Owner)
                return false;

            //Returns true if there is an overlap between boxes
            if (other.Left <= Right &&
                other.Bottom <= Top &&
                other.Back <= Front &&
                Left <= other.Right &&
                Bottom <= other.Top &&
                Back <= other.Front)
            {
                return true;
            }

            //Return false if there is no overlap
            return false;
        }

        public override bool CheckCollisionCircle(CircleCollider other)
        {
            return other.CheckCollisionAABB(this);
        }

        public override void Draw()
        {
            Raylib.DrawCubeWires(new System.Numerics.Vector3(Owner.WorldPosition.X, Owner.WorldPosition.Y, Owner.WorldPosition.Z), Width, Height, Depth, Color.WHITE);
        }
    }
}
