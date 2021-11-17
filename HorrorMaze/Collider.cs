using System;
using System.Collections.Generic;
using System.Text;
using MathLibrary;

namespace HorrorMaze
{
    /// <summary>
    /// Enum for the type of collider
    /// </summary>
    enum ColliderType
    {
        CIRCLE,
        AABB
    }
    
    abstract class Collider
    {
        //Class variables
        private Actor _owner;
        private ColliderType _colliderType;
        private Vector3 _collisionNormal;

        /// <summary>
        /// The owner of the collider
        /// </summary>
        public Actor Owner
        {
            get { return _owner; }
            set { _owner = value; }
        }

        /// <summary>
        /// The type of collider
        /// </summary>
        public ColliderType ColliderType
        {
            get { return _colliderType; }
        }

        /// <summary>
        /// The collision normal between two colliders
        /// </summary>
        public Vector3 CollisionNormal
        {
            get { return _collisionNormal; }
            set { _collisionNormal = value; }
        }

        /// <summary>
        /// The constructor for the collider
        /// </summary>
        /// <param name="owner">the owner of the collider</param>
        /// <param name="colliderType">the type of collider</param>
        public Collider(Actor owner, ColliderType colliderType)
        {
            _owner = owner;
            _colliderType = colliderType;
        }

        /// <summary>
        /// Checks for a collision between colliders
        /// </summary>
        /// <param name="other">the actor being collided into</param>
        /// <returns></returns>
        public bool CheckCollision(Actor other)
        {
            if (other.Collider.ColliderType == ColliderType.CIRCLE)
                return CheckCollisionCircle((CircleCollider)other.Collider);
            else if (other.Collider.ColliderType == ColliderType.AABB)
                return CheckCollisionAABB((AABBCollider)other.Collider);

            return false;
        }

        /// <summary>
        /// Base draw function to be overwritten
        /// </summary>
        public virtual void Draw()
        {

        }

        /// <summary>
        /// Base circle collision to be overwritten
        /// </summary>
        /// <param name="other">the collider being collided into</param>
        /// <returns></returns>
        public virtual bool CheckCollisionCircle(CircleCollider other) { return false; }

        /// <summary>
        /// Base AABB collider to be overwritten
        /// </summary>
        /// <param name="other">the collider being collided into</param>
        /// <returns></returns>
        public virtual bool CheckCollisionAABB(AABBCollider other) { return false; }
    }
}
