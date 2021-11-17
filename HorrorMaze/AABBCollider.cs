using System;
using System.Collections.Generic;
using System.Text;
using MathLibrary;
using Raylib_cs;

namespace HorrorMaze
{
    class AABBCollider : Collider
    {
        //The class variables
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

        /// <summary>
        /// The size of this collider on the z axis
        /// </summary>
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

        /// <summary>
        /// The farthest z position forwards
        /// </summary>
        public float Front
        {
            get
            {
                return Owner.LocalPosition.Z + Depth / 2;
            }
        }

        /// <summary>
        /// The farthest z position backwards
        /// </summary>
        public float Back
        {
            get
            {
                return Owner.LocalPosition.Z - Depth / 2;
            }
        }

        /// <summary>
        /// The collider constructor
        /// </summary>
        /// <param name="width">the width of the collider</param>
        /// <param name="height">the height of the collider</param>
        /// <param name="depth">the depth of the collider</param>
        /// <param name="owner">the owner of the collider</param>
        public AABBCollider(float width, float height, float depth, Actor owner) : base (owner, ColliderType.AABB)
        {
            _width = width;
            _height = height;
            _depth = depth;
        }

        /// <summary>
        /// Checks if this collider has collided with another AABB colider
        /// </summary>
        /// <param name="other">the other collider</param>
        /// <returns></returns>
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

        /// <summary>
        /// Checks if this collider has collided with a circle collider
        /// </summary>
        /// <param name="other">the other collider</param>
        /// <returns></returns>
        public override bool CheckCollisionCircle(CircleCollider other)
        {
            return other.CheckCollisionAABB(this);
        }

        /// <summary>
        /// Draws the hitbox of the collider
        /// </summary>
        public override void Draw()
        {
            Raylib.DrawCubeWires(new System.Numerics.Vector3(Owner.WorldPosition.X, Owner.WorldPosition.Y, Owner.WorldPosition.Z), Width, Height, Depth, Color.WHITE);
        }
    }
}
