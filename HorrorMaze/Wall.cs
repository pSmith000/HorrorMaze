using System;
using System.Collections.Generic;
using System.Text;

namespace HorrorMaze
{
    class Wall : Actor
    {
        /// <summary>
        /// A constructor for the wall
        /// </summary>
        /// <param name="x">the position on the x axis</param>
        /// <param name="y">the position on the y axis</param>
        /// <param name="z">the position on the z axis</param>
        /// <param name="scaleX">the scale on the x axis</param>
        /// <param name="scaleY">the scale on the y axis</param>
        /// <param name="scaleZ">the scale on the z axis</param>
        /// <param name="scene">the scene the wall is being added to</param>
        /// <param name="name">the name of the wall</param>
        public Wall(float x, float y, float z, float scaleX, float scaleY, float scaleZ, Scene scene, string name = "Wall") : 
            base(x, y, z, name)
        {
            //Sets the scale, adds a collider, and adds the wall to the scene
            SetScale(scaleX, scaleY, scaleZ);
            AABBCollider wallCollider = new AABBCollider(scaleX, scaleY, scaleZ, this);
            Collider = wallCollider;
            scene.AddActor(this);
        }

    }
}
