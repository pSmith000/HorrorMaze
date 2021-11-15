using System;
using System.Collections.Generic;
using System.Text;

namespace HorrorMaze
{
    class Wall : Actor
    {
        public Wall(float x, float y, float z, float scaleX, float scaleY, float scaleZ, Scene scene, string name = "Wall") : 
            base(x, y, z, name)
        {
            SetScale(scaleX, scaleY, scaleZ);
            AABBCollider wallCollider = new AABBCollider(scaleX, scaleY, scaleZ, this);
            Collider = wallCollider;
            scene.AddActor(this);
        }

    }
}
