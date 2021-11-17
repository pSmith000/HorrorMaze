using System;
using System.Collections.Generic;
using System.Text;
using MathLibrary;
using Raylib_cs;

namespace HorrorMaze
{
    class CircleCollider : Collider
    {
        //The class variable
        private float _collisionRadius;

        /// <summary>
        /// The radius of the collider
        /// </summary>
        public float CollisionRadius
        {
            get { return _collisionRadius; }
            set { _collisionRadius = value; }
        }

        /// <summary>
        /// Constructor for this collider
        /// </summary>
        /// <param name="collisionRadius">the radius of the collider</param>
        /// <param name="owner">the owner of the collider</param>
        public CircleCollider(float collisionRadius, Actor owner) : base(owner, ColliderType.CIRCLE)
        {
            _collisionRadius = collisionRadius;
        }

        /// <summary>
        /// Checks for a collision with another circle collider
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public override bool CheckCollisionCircle(CircleCollider other)
        {
            //Checks to seee if the owner has collided with itself
            if (other.Owner == Owner)
                return false;

            //Find the distance between the two actors
            float distance = Vector3.Distance(other.Owner.WorldPosition, Owner.WorldPosition);
            //Find the length of the radii combined
            float combinedRadii = other.CollisionRadius + CollisionRadius;

            return distance <= combinedRadii;
        }

        /// <summary>
        /// Checks to see if this collider has collided with an AABB collider
        /// </summary>
        /// <param name="other">the AABB collider</param>
        /// <returns></returns>
        public override bool CheckCollisionAABB(AABBCollider other)
        {
            //Return false if this collider is checking collision against itself
            if (other.Owner == Owner)
                return false;

            //Get the direction from this collider to the AABB
            Vector3 direction = Owner.WorldPosition - other.Owner.WorldPosition;

            //Clamp the direction vector to hbe within the bounds of the AABB
            direction.X = Math.Clamp(direction.X, -other.Width/2, other.Width/2);
            direction.Y = Math.Clamp(direction.Y, -other.Height/2, other.Height/2);
            direction.Z = Math.Clamp(direction.Z, -other.Depth/2, other.Depth/2);

            //Add the direction vector to the AABB center to get the closest point to the circle
            Vector3 closestPoint = other.Owner.WorldPosition + direction;

            //The collision normal of this collider is set 
            CollisionNormal = (closestPoint - Owner.WorldPosition).Normalized;

            //The collision normal of the other collider is set
            other.CollisionNormal = (Owner.WorldPosition - closestPoint).Normalized;

            //Find the distance from the circle's center to the closest point
            float distanceFromClosestPoint = Vector3.Distance(Owner.WorldPosition, closestPoint);

            //Return whether or not the distance is less then the circle's radius
            return distanceFromClosestPoint <= CollisionRadius;
        }

        /// <summary>
        /// Draws the circle hitbox
        /// </summary>
        public override void Draw()
        {
            base.Draw();
            Raylib.DrawSphereWires(new System.Numerics.Vector3(Owner.WorldPosition.X, Owner.WorldPosition.Y, Owner.WorldPosition.Z), CollisionRadius, 40, 4, Color.WHITE);
        }
    }
}
