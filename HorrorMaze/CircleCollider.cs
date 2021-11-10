using System;
using System.Collections.Generic;
using System.Text;
using MathLibrary;
using Raylib_cs;

namespace HorrorMaze
{
    class CircleCollider : Collider
    {
        private float _collisionRadius;

        public float CollisionRadius
        {
            get { return _collisionRadius; }
            set { _collisionRadius = value; }
        }

        public CircleCollider(float collisionRadius, Actor owner) : base(owner, ColliderType.CIRCLE)
        {
            _collisionRadius = collisionRadius;
        }

        public override bool CheckCollisionCircle(CircleCollider other)
        {
            if (other.Owner == Owner)
                return false;

            //Find the distance between the two actors
            float distance = Vector3.Distance(other.Owner.WorldPosition, Owner.WorldPosition);
            //Find the length of the radii combined
            float combinedRadii = other.CollisionRadius + CollisionRadius;

            return distance <= combinedRadii;
        }

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

            CollisionNormal = (closestPoint - Owner.WorldPosition).Normalized;

            other.CollisionNormal = (Owner.WorldPosition - closestPoint).Normalized;

            //Find the distance from the circle's center to the closest point
            float distanceFromClosestPoint = Vector3.Distance(Owner.WorldPosition, closestPoint);

            //Return whether or not the distance is less then the circle's radius
            return distanceFromClosestPoint <= CollisionRadius;
        }

        public override void Draw()
        {
            base.Draw();
            Raylib.DrawSphereWires(new System.Numerics.Vector3(Owner.WorldPosition.X, Owner.WorldPosition.Y, Owner.WorldPosition.Z), CollisionRadius, 40, 4, Color.WHITE);
        }
    }
}
