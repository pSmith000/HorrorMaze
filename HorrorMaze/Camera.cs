using System;
using System.Collections.Generic;
using System.Text;
using Raylib_cs;
using MathLibrary;

namespace HorrorMaze
{
    class Camera : Actor
    {
        //The class variables
        private Camera3D _camera3D;
        private Actor _target;

        /// <summary>
        /// Property to set and get a 3D camera
        /// </summary>
        public Camera3D Camera3D
        {
            get { return _camera3D; }
            set { _camera3D = value; }
        }

        /// <summary>
        /// The camera constructor
        /// </summary>
        /// <param name="target">the target the camera will follow</param>
        public Camera(Actor target)
            : base()
        {
            //The camera and the target it will follow
            _camera3D = new Camera3D();
            _target = target;
        }

        /// <summary>
        /// Starts the camera
        /// </summary>
        public override void Start()
        {
            //Camera position
            _camera3D.up = new System.Numerics.Vector3(0, 1, 0); //Camera up vector (rotation towards target)
            _camera3D.fovy = 55; //Camera field of view Y
            _camera3D.projection = CameraProjection.CAMERA_PERSPECTIVE; //Camera mode type

            //Sets the position above and behind the target
            SetTranslation(0, 4, -10);
        }


        /// <summary>
        /// Updates the camera psotion based on the target position
        /// </summary>
        /// <param name="deltaTime">the time that has elapsed</param>
        public override void Update(float deltaTime)
        {
            //Camera position
            _camera3D.position = new System.Numerics.Vector3(WorldPosition.X, WorldPosition.Y - 2, WorldPosition.Z);
            //Point the camera is focused on
            _camera3D.target = new System.Numerics.Vector3(_target.WorldPosition.X, _target.WorldPosition.Y, _target.WorldPosition.Z);

            base.Update(deltaTime);
        }
    }
}
