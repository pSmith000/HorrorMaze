using System;
using System.Collections.Generic;
using System.Text;
using MathLibrary;
using Raylib_cs;

namespace HorrorMaze
{
    public enum Shape
    {
        NONE,
        CUBE,
        SPHERE
    }

    class Actor
    {
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
        private Color _color = new Color(255, 255, 255, 255);

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

        public string Name
        {
            get { return _name; }
        }

        public Vector3 LocalPosition
        {
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

        public Matrix4 GlobalTransform
        {
            get { return _globalTransform; }
            private set { _globalTransform = value; }
        }

        public Matrix4 LocalTransform
        {
            get { return _localTransform; }
            private set { _localTransform = value; }
        }

        public Actor Parent
        {
            get { return _parent; }
            set { _parent = value; }
        }

        public Actor[] Children
        {
            get { return _children; }
        }

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

        public Vector3 Forward
        {
            get { return new Vector3(_rotation.M02, _rotation.M12, _rotation.M22); }
            
        }

        public Vector3 Right
        {
            get { return new Vector3(_rotation.M00, _rotation.M10, _rotation.M20); }

        }

        public Collider Collider
        {
            get { return _collider; }
            set { _collider = value; }
        }

        public Actor() { }

        public Actor(float x, float y, string name = "Actor", Shape shape = Shape.CUBE) :
            this(new Vector3 { X = x, Y = y}, name, shape) {}


        public Actor(Vector3 position, string name = "Actor", Shape shape = Shape.CUBE)
        {
            LocalPosition = position;
            _name = name;
            _shape = shape;

        }

        public void UpdateTransforms()
        {
            _localTransform = _translation * _rotation * _scale;

            if (Parent != null)
                GlobalTransform = Parent.GlobalTransform * LocalTransform;
            else
                GlobalTransform = LocalTransform; 
        }

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

        public virtual void Start()
        {
            _started = true;
        }

        public virtual void Update(float deltaTime)
        {
            UpdateTransforms();

                
        }

        public virtual void Draw()
        {
            System.Numerics.Vector3 startPosition = new System.Numerics.Vector3(WorldPosition.X, WorldPosition.Y, WorldPosition.Z);
            System.Numerics.Vector3 endPosition = new System.Numerics.Vector3(WorldPosition.X + Forward.X * 50, WorldPosition.Y + Forward.Y * 50, WorldPosition.Z + Forward.Z * 50);


            switch (_shape)
            {
                case Shape.CUBE:
                    Raylib.DrawCube(startPosition, Size.X, Size.Y, Size.Z, ShapeColor);
                    break;
                case Shape.SPHERE:
                    Raylib.DrawSphere(startPosition, Size.X, ShapeColor);
                    break;
            }

            Raylib.DrawLine3D(startPosition, endPosition, Color.RED);

            if (this.Collider != null)
                Collider.Draw();
        }

        public void End()
        {

        }

        public virtual void OnCollision(Actor other)
        {

        }

        public virtual bool CheckForCollision(Actor other)
        {
            //Return false if either actor doesn't have a collider attached 
            if (Collider == null || other.Collider == null)
                return false;

            return Collider.CheckCollision(other);
        }

        public void SetTranslation(float translationX, float translationY, float translationZ)
        {
            _translation = Matrix4.CreateTranslation(translationX, translationY, translationZ);
        }

        public void Translate(float translationX, float translationY, float translationZ)
        {
            _translation *= Matrix4.CreateTranslation(translationX, translationY, translationZ);
        }

        public void SetRotation(float radiansX, float radiansY, float radiansZ)
        {
            Matrix4 rotationX = Matrix4.CreateRotationX(radiansX);
            Matrix4 rotationY = Matrix4.CreateRotationY(radiansY);
            Matrix4 rotationZ = Matrix4.CreateRotationZ(radiansZ);
            _rotation = rotationX * rotationY * rotationZ;
        }

        public void Rotate(float radiansX, float radiansY, float radiansZ)
        {
            Matrix4 rotationX = Matrix4.CreateRotationX(radiansX);
            Matrix4 rotationY = Matrix4.CreateRotationY(radiansY);
            Matrix4 rotationZ = Matrix4.CreateRotationZ(radiansZ);
            _rotation *= rotationX * rotationY * rotationZ;
        }

        public void SetScale(float x, float y, float z)
        {
            _scale = Matrix4.CreateScale(x, y, z);
        }

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
