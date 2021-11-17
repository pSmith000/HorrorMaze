using System;
using System.Collections.Generic;
using System.Text;
using MathLibrary;
using Raylib_cs;

namespace HorrorMaze
{
    /// <summary>
    /// An enum for the shape drawn in 3d
    /// </summary>
    public enum Shape
    {
        NONE,
        CUBE,
        SPHERE
    }


    class Actor
    {
        //The class variables
        private string _name;
        private bool _started;
        private Vector3 _forward = new Vector3(0, 0, 1);
        private Collider _collider;
        private Matrix4 _globalTransform = Matrix4.Identity;
        private Matrix4 _localTransform = Matrix4.Identity;
        private Matrix4 _translation = Matrix4.Identity;
        private Matrix4 _rotation = Matrix4.Identity;
        private Matrix4 _scale = Matrix4.Identity;
        private Actor[] _children = new Actor[0];
        private Actor _parent;
        private Shape _shape;
        private Color _color = new Color(100, 100, 100, 255);

        /// <summary>
        /// Property for getting the color of the shape
        /// </summary>
        public Color ShapeColor
        {
            get { return _color; }
        }

        /// <summary>
        /// True if the start function has been called for this actor
        /// </summary>
        public bool Started
        {
            get { return _started; }
        }

        /// <summary>
        /// Property for getting the name of the actor
        /// </summary>
        public string Name
        {
            get { return _name; }
        }

        /// <summary>
        /// Property for getting and setting the local position of the actor
        /// </summary>
        public Vector3 LocalPosition
        {
            //Return the local translation
            get { return new Vector3(_translation.M03, _translation.M13, _translation.M23);  }
            set 
            {
                SetTranslation(value.X, value.Y, value.Z);
            }
        }

        /// <summary>
        /// The position of this actor in the world
        /// </summary>
        public Vector3 WorldPosition
        {
            //Return the global transform's T column
            get { return new Vector3(_globalTransform.M03, _globalTransform.M13, _globalTransform.M23); }
            set
            {
                //If the acto has a parent...
                if (Parent != null)
                {
                    //...convert the world coordinates into the local coordinates and translate the actor
                    float xOffset = (value.X - Parent.WorldPosition.X) / new Vector3(GlobalTransform.M00, GlobalTransform.M10, GlobalTransform.M20).Magnitude;
                    float yOffset = (value.Y - Parent.WorldPosition.Y) / new Vector3(GlobalTransform.M01, GlobalTransform.M11, GlobalTransform.M21).Magnitude;
                    float zOffset = (value.Z - Parent.WorldPosition.Z) / new Vector3(GlobalTransform.M02, GlobalTransform.M12, GlobalTransform.M22).Magnitude;

                    SetTranslation(xOffset, yOffset, zOffset);
                }
                //If this actor doesn't have a parent...
                else
                    //...set the position to the given value
                    LocalPosition = value;
            }
        }

        /// <summary>
        /// The global movement of the actor
        /// </summary>
        public Matrix4 GlobalTransform
        {
            get { return _globalTransform; }
            private set { _globalTransform = value; }
        }

        /// <summary>
        /// The local movement of the actor
        /// </summary>
        public Matrix4 LocalTransform
        {
            get { return _localTransform; }
            private set { _localTransform = value; }
        }

        /// <summary>
        /// The parent of the actor
        /// </summary>
        public Actor Parent
        {
            get { return _parent; }
            set { _parent = value; }
        }

        /// <summary>
        /// An array of the children an actor has
        /// </summary>
        public Actor[] Children
        {
            get { return _children; }
        }

        /// <summary>
        /// The scale of the actor on the x, y, and z axis
        /// </summary>
        public Vector3 Size
        {
            get 
            {
                float xScale = new Vector3(_scale.M00, _scale.M10, _scale.M20).Magnitude;
                float yScale = new Vector3(_scale.M01, _scale.M11, _scale.M21).Magnitude;
                float zScale = new Vector3(_scale.M02, _scale.M12, _scale.M22).Magnitude;

                return new Vector3(xScale, yScale, zScale); 
            }
            set { SetScale(value.X, value.Y, value.Z); }
        }

        /// <summary>
        /// The way that the actor is facing
        /// </summary>
        public Vector3 Forward
        {
            get { return new Vector3(_rotation.M02, _rotation.M12, _rotation.M22); }
            
        }

        /// <summary>
        /// The rotation of the actor on the x axis
        /// </summary>
        public Vector3 Right
        {
            get { return new Vector3(_rotation.M00, _rotation.M10, _rotation.M20); }

        }

        /// <summary>
        /// The collider of the actor
        /// </summary>
        public Collider Collider
        {
            get { return _collider; }
            set { _collider = value; }
        }

        /// <summary>
        /// A base constructor for the actor
        /// </summary>
        public Actor() { }

        /// <summary>
        /// The main constructor for the actor
        /// </summary>
        /// <param name="x">position on the x axis</param>
        /// <param name="y">position on the y axis</param>
        /// <param name="z">position on the z axis</param>
        /// <param name="name">name of the actor</param>
        /// <param name="shape">the shape of the actor</param>
        public Actor(float x, float y, float z, string name = "Actor", Shape shape = Shape.CUBE) :
            this(new Vector3 { X = x, Y = y, Z = z}, name, shape) {}

        /// <summary>
        /// The second constructor for the actor
        /// </summary>
        /// <param name="position">The position of the actor using vector 3</param>
        /// <param name="name">name of the actor</param>
        /// <param name="shape">the shape of the actor</param>
        public Actor(Vector3 position, string name = "Actor", Shape shape = Shape.CUBE)
        {
            LocalPosition = position;
            _name = name;
            _shape = shape;

        }

        /// <summary>
        /// Updating the transforms of the actor in the world
        /// </summary>
        public void UpdateTransforms()
        {
            //The local tranform is set to the rotation scale and translation of the actor
            _localTransform = _translation * _rotation * _scale;

            //If the actor has a child...
            if (Parent != null)
                //...the gloabal transform is set
                GlobalTransform = Parent.GlobalTransform * LocalTransform;
            //If the actor does not have a child...
            else
                //...the global transform is set
                GlobalTransform = LocalTransform; 
        }

        /// <summary>
        /// Adding a child to an actor
        /// </summary>
        /// <param name="child">the child being added</param>
        public void AddChild(Actor child)
        {
            //Create a temp array larger than the original
            Actor[] tempArray = new Actor[_children.Length + 1];

            //Copy all values from the original array into the temp array
            for (int i = 0; i < _children.Length; i++)
            {
                tempArray[i] = _children[i];
            }

            //Add the new child to the end of the new array
            tempArray[_children.Length] = child;

            //Set the old array to be the new array
            _children = tempArray;

            //Set the parent of the child to be this actor
            child.Parent = this;

        }

        /// <summary>
        /// Removing a child from an actor
        /// </summary>
        /// <param name="child">the child being removed</param>
        /// <returns></returns>
        public bool RemoveChild(Actor child)
        {
            //Create a variable to store if the removal was successful
            bool childRemoved = false;

            //Create a new array that is smaller than the original
            Actor[] tempArray = new Actor[_children.Length - 1];

            //Copy all values except the actor we don't want into the new array
            int j = 0;
            for (int i = 0; i < tempArray.Length; i++)
            {
                //If the actor that the loop is on is not the one to remove...
                if (_children[i] != child)
                {
                    //...add the actor back into the new array
                    tempArray[j] = _children[i];
                    j++;
                }
                //Otherwise if this actor is the one to remove...
                else
                    //...set actorRemoved to true
                    childRemoved = true;
            }

            //If the actor removal was successful...
            if (childRemoved)
            {
                //...set the old array to be the new array
                _children = tempArray;

                //Set the parent of the child to be nothing
                child.Parent = null;
            }
           
            return childRemoved;
        }

        /// <summary>
        /// Starts the actor and sets the started variable to true
        /// </summary>
        public virtual void Start()
        {
            _started = true;
        }

        /// <summary>
        /// Updates the transforms
        /// </summary>
        /// <param name="deltaTime">the amount of time that has passed</param>
        public virtual void Update(float deltaTime)
        {
            UpdateTransforms();
        }

        /// <summary>
        /// Draws the shapes and colliders to the screen
        /// </summary>
        public virtual void Draw()
        {
            //The start and end position of the shapes
            System.Numerics.Vector3 startPosition = new System.Numerics.Vector3(WorldPosition.X, WorldPosition.Y, WorldPosition.Z);
            System.Numerics.Vector3 endPosition = new System.Numerics.Vector3(WorldPosition.X + Forward.X * 50, WorldPosition.Y + Forward.Y * 50, WorldPosition.Z + Forward.Z * 50);


            switch (_shape)
            {
                //If the shape is a cube...
                case Shape.CUBE:
                    //...draw a cube
                    Raylib.DrawCube(startPosition, Size.X, Size.Y, Size.Z, ShapeColor);
                    break;
                    //If the shape is a shpere...
                case Shape.SPHERE:
                    //...draw a sphere
                    Raylib.DrawSphere(startPosition, Size.X, ShapeColor);
                    break;
            }
            
            //If the actor has a collider...
            if (this.Collider != null)
                //...draw the collider
                Collider.Draw();
        }

        /// <summary>
        /// Ends the actor
        /// </summary>
        public void End() { }

        /// <summary>
        /// Called when the actor collides with another
        /// </summary>
        /// <param name="other">the actor that is colliding with this one</param>
        public virtual void OnCollision(Actor other) { }

        /// <summary>
        /// Checks for collision between actors
        /// </summary>
        /// <param name="other">the actor colliding with this one</param>
        /// <returns>true if the actor has collided</returns>
        public virtual bool CheckForCollision(Actor other)
        {
            //Return false if either actor doesn't have a collider attached 
            if (Collider == null || other.Collider == null)
                return false;

            //Checks collision from the collider
            return Collider.CheckCollision(other);
        }

        /// <summary>
        /// Sets the translation of the actor
        /// </summary>
        /// <param name="translationX">translation on the x axis</param>
        /// <param name="translationY">translation on the y axis</param>
        /// <param name="translationZ">translation on the z axis</param>
        public void SetTranslation(float translationX, float translationY, float translationZ)
        {
            _translation = Matrix4.CreateTranslation(translationX, translationY, translationZ);
        }

        /// <summary>
        /// Translates the actor to the position
        /// </summary>
        /// <param name="translationX">translation on the x axis</param>
        /// <param name="translationY">translation on the y axis</param>
        /// <param name="translationZ">translation on the z axis</param>
        public void Translate(float translationX, float translationY, float translationZ)
        {
            _translation *= Matrix4.CreateTranslation(translationX, translationY, translationZ);
        }

        /// <summary>
        /// Sets the roatation of the actor
        /// </summary>
        /// <param name="roatationX">roatation on the x axis</param>
        /// <param name="roatationY">roatation on the y axis</param>
        /// <param name="roatationZ">roatation on the z axis</param>
        public void SetRotation(float radiansX, float radiansY, float radiansZ)
        {
            Matrix4 rotationX = Matrix4.CreateRotationX(radiansX);
            Matrix4 rotationY = Matrix4.CreateRotationY(radiansY);
            Matrix4 rotationZ = Matrix4.CreateRotationZ(radiansZ);
            _rotation = rotationX * rotationY * rotationZ;
        }

        /// <summary>
        /// Rotates the actor the amount of radians
        /// </summary>
        /// <param name="roatationX">roatation on the x axis</param>
        /// <param name="roatationY">roatation on the y axis</param>
        /// <param name="roatationZ">roatation on the z axis</param>
        public void Rotate(float radiansX, float radiansY, float radiansZ)
        {
            Matrix4 rotationX = Matrix4.CreateRotationX(radiansX);
            Matrix4 rotationY = Matrix4.CreateRotationY(radiansY);
            Matrix4 rotationZ = Matrix4.CreateRotationZ(radiansZ);
            _rotation *= rotationX * rotationY * rotationZ;
        }

        /// <summary>
        /// Sets the scale of the actor
        /// </summary>
        /// <param name="scaleX">scale on the x axis</param>
        /// <param name="scaleY">scale on the y axis</param>
        /// <param name="scaleZ">scale on the z axis</param>
        public void SetScale(float x, float y, float z)
        {
            _scale = Matrix4.CreateScale(x, y, z);
        }

        /// <summary>
        /// Scales the actor 
        /// </summary>
        /// <param name="scaleX">scale on the x axis</param>
        /// <param name="scaleY">scale on the y axis</param>
        /// <param name="scaleZ">scale on the z axis</param>
        public void Scale(float x, float y, float z)
        {
            _scale *= Matrix4.CreateScale(x, y, z);
        }

        /// <summary>
        /// Rotates the actor to face the given position
        /// </summary>
        /// <param name="position">The position the actor should be looking at</param>
        public void LookAt(Vector3 position)
        {
            //Get the direction for the actor to look in
            Vector3 direction = (position - WorldPosition).Normalized;

            //If the direction has a length of 0...
            if (direction.Magnitude == 0)
                //...set it to be the default forward
                direction = new Vector3(0, 0, 1);

            //Create a vector that points directly upwards
            Vector3 alignAxis = new Vector3(0, 1, 0);

            //Create two new vectors that will be the new x and y axis
            Vector3 newYAxis = new Vector3(0, 1, 0);
            Vector3 newXAxis = new Vector3(1, 0, 0);

            //If the direction vector is parallel to the alignAxis vector...
            if (Math.Abs(direction.Y) > 0 && direction.X == 0 && direction.Z == 0)
            {
                //...set the alignAxis vector to point to the right
                alignAxis = new Vector3(1, 0, 0);

                //Get the cross product of the direction and the right to find the y axis
                newYAxis = Vector3.CrossProduct(direction, alignAxis);
                //Normalize the new y axis to prevent the matrix from being scaled
                newYAxis.Normalize();

                //Get the cross product of the new y axis and the direction to find the new x axis
                newXAxis = Vector3.CrossProduct(newYAxis, direction);
                //Normalize the new x axis to prevent the matrix from being scaled
                newXAxis.Normalize();
            }
            //If the direction vector is not parallel...
            else
            {
                //Get the cross product of the alignAxis and the direction vector
                newXAxis = Vector3.CrossProduct(alignAxis, direction);
                //Normalize the new x axis to prevent the matrix from being scaled
                newXAxis.Normalize();

                //Get the cross product of the direction and new x axis
                newYAxis = Vector3.CrossProduct(direction, newXAxis);
                //Normalize the new y axis to prevent the matrix from being scaled
                newYAxis.Normalize();
            }

            //Create a new matrix with the new axis
            _rotation = new Matrix4(newXAxis.X, newYAxis.X, direction.X, 0,
                                    newXAxis.Y, newYAxis.Y, direction.Y, 0,
                                    newXAxis.Z, newYAxis.Z, direction.Z, 0,
                                    0, 0, 0, 1);

        }

        /// <summary>
        /// Sets the color to a Raylib preset color
        /// </summary>
        /// <param name="color"></param>
        public void SetColor(Color color)
        {
            _color = color;
        }

        /// <summary>
        /// First value is red, second value, is green, third value is blue, and fourth value is transparency
        /// </summary>
        /// <param name="colorValue"></param>
        public void SetColor(Vector4 colorValue)
        {
            _color = new Color((int)colorValue.X, (int)colorValue.Y, (int)colorValue.Z, (int)colorValue.W);
        }
    }
}
