using System;
using System.Collections.Generic;
using System.Text;
using Raylib_cs;
using MathLibrary;

namespace HorrorMaze
{
    class Camera : Actor
    {
        private Camera3D _camera3D;
        private Actor _target;

        public Camera3D Camera3D
        {
            get { return _camera3D; }
            set { _camera3D = value; }
        }

        public Camera(Actor target)
            : base()
        {
            _camera3D = new Camera3D();
            _target = target;
        }

        public override void Start()
        {
            // Camera position
            _camera3D.up = new System.Numerics.Vector3(0, 1, 0); //Camera up vector (rotation towards target)
            _camera3D.fovy = 55; // Camera field of view Y
            _camera3D.projection = CameraProjection.CAMERA_PERSPECTIVE; //Camera mode type

            SetTranslation(0, 4, -10);
        }

        public override void Update(float deltaTime)
        {
            // Camera position
            _camera3D.position = new System.Numerics.Vector3(WorldPosition.X, WorldPosition.Y - 2, WorldPosition.Z);
            // Point the camera is focused on
            _camera3D.target = new System.Numerics.Vector3(_target.WorldPosition.X, _target.WorldPosition.Y, _target.WorldPosition.Z);


            base.Update(deltaTime);
        }
    }
}
