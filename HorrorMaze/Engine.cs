using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.Threading;
using MathLibrary;
using Raylib_cs;

namespace HorrorMaze
{
    class Engine
    {
        private static bool _applicationShouldClose;
        private static int _currentSceneIndex;
        private Scene[] _scenes = new Scene[0];
        private Stopwatch _stopwatch = new Stopwatch();
        private Camera _camera;
        Player player;

        /// <summary>
        /// Called to begin the application
        /// </summary>
        public void Run()
        {
            //Call start for the entire application
            Start();

            float currentTime = 0;
            float lastTime = 0;
            float deltaTime = 0;

            //Loop until the application is told to close
            while (!_applicationShouldClose && !Raylib.WindowShouldClose())
            {
                //Get how much time has passed since the application started
                currentTime = _stopwatch.ElapsedMilliseconds / 1000.0f;

                //Set delta time to be the difference in time from the last time recorded to the current time
                deltaTime = currentTime - lastTime;

                //Update the application
                Update(deltaTime);
                //Draw all items
                Draw();

                //Set the last time recorded to be the current time
                lastTime = currentTime;
            }

            //Call end for the entire application
            End();

        }


        /// <summary>
        /// Called when the application starts
        /// </summary>
        private void Start()
        {
            _stopwatch.Start();
            //Create a window using raylib
            Raylib.InitWindow(1920, 1080, "Math for Games");
            //Raylib.InitWindow(900, 500, "Math for Games");
            Raylib.SetTargetFPS(60);

            Initialize();

            _scenes[_currentSceneIndex].Start();
        }

        public void Initialize()
        {

            Scene scene = new Scene();

            player = new Player(0, 0, -90, 100, "Player", Shape.SPHERE);
            _camera = new Camera(player);
            player.SetScale(1, 1, 1);
            CircleCollider playerCircleCollider = new CircleCollider(1, player);
            player.SetColor(new Vector4(10, 20, 200, 255));
            player.AddChild(_camera);

            Actor wall = new Actor(0, -2, 0, "Wall");
            wall.SetScale(200, 1, 200);
            wall.SetColor(new Vector4(0, 0, 0, 255));

            Actor wall2 = new Actor(0, -2, 100, "Wall");
            wall2.SetScale(200, 50, 2);

            Actor wall3 = new Actor(0, -2, -100, "Wall");
            wall3.SetScale(200, 50, 2);

            Actor wall4 = new Actor(100, -2, 0, "Wall");
            wall4.SetScale(2, 50, 200);

            Actor wall5 = new Actor(-100, -2, 0, "Wall");
            wall5.SetScale(2, 50, 200);

            Actor wall6 = new Actor(10, -2, -90, "Wall");
            wall6.SetScale(2, 50, 20);

            Actor wall7 = new Actor(-10, -2, -90, "Wall");
            wall7.SetScale(2, 50, 80);

            Actor wall8 = new Actor(34, -2, -80, "Wall");
            wall8.SetScale(50, 50, 2);

            Enemy enemy = new Enemy(50, 1, 50, 10, player, "Enemy", Shape.CUBE);
            enemy.SetTranslation(50, 1, 50);
            enemy.SetColor(new Vector4(255, 0, 0, 255));
            enemy.SetScale(10, 10, 10);
            Actor enemyTorso = new Actor(0, 1, 0, "Enemy");
            enemyTorso.SetColor(new Vector4(255, 0, 0, 255));
            enemy.AddChild(enemyTorso);
            Actor enemyHead = new Actor(0, 2, 0, "Enemy", Shape.SPHERE);
            enemyHead.SetColor(new Vector4(255, 0, 10, 255));
            enemy.AddChild(enemyHead);
            Actor enemyEye1 = new Actor(0, 0, 0, "Enemy", Shape.SPHERE);
            enemyEye1.SetScale(1, 1, 1);
            enemyEye1.SetColor(new Vector4(0, 0, 10, 255));
            enemyHead.AddChild(enemyEye1);
            AABBCollider wallCollider = new AABBCollider(200, 1, 200, wall);
            AABBCollider wall2Collider = new AABBCollider(200, 50, 2, wall2);
            AABBCollider wall3Collider = new AABBCollider(200, 50, 2, wall3);
            AABBCollider wall4Collider = new AABBCollider(2, 50, 200, wall4);
            AABBCollider wall5Collider = new AABBCollider(2, 50, 200, wall5);
            AABBCollider wall6Collider = new AABBCollider(2, 50, 20, wall6);
            AABBCollider wall7Collider = new AABBCollider(2, 50, 40, wall7);
            AABBCollider wall8Collider = new AABBCollider(50, 50, 2, wall8);

            player.Collider = playerCircleCollider;
            wall.Collider = wallCollider;
            wall2.Collider = wall2Collider;
            wall3.Collider = wall3Collider;
            wall4.Collider = wall4Collider;
            wall5.Collider = wall5Collider;
            wall6.Collider = wall6Collider;
            wall7.Collider = wall7Collider;
            wall8.Collider = wall8Collider;

            scene.AddActor(player);
            scene.AddActor(enemy);
            scene.AddActor(enemyTorso);
            scene.AddActor(enemyHead);
            scene.AddActor(enemyEye1);
            scene.AddActor(_camera);
            scene.AddActor(wall);
            scene.AddActor(wall2);
            scene.AddActor(wall3);
            scene.AddActor(wall4);
            scene.AddActor(wall5);
            scene.AddActor(wall6);
            scene.AddActor(wall7);
            scene.AddActor(wall8);

            _currentSceneIndex = AddScene(scene);
        }

        /// <summary>
        /// Called everytime the game loops
        /// </summary>
        private void Update(float deltaTime)
        {
            _scenes[_currentSceneIndex].Update(deltaTime);

            while (Console.KeyAvailable)
                Console.ReadKey(true);

        }

        /// <summary>
        /// Called everytime the game loops to update visuals
        /// </summary>
        private void Draw()
        {
            Raylib.BeginDrawing();
            Raylib.BeginMode3D(_camera.Camera3D);
            

            Raylib.ClearBackground(Color.BLACK);
            Raylib.DrawGrid(100, 10);

            //Adds all actor icons to buffer
            _scenes[_currentSceneIndex].Draw();

            Raylib.EndMode3D();
            Raylib.EndDrawing();
        }

        /// <summary>
        /// Called when the application exits
        /// </summary>
        private void End()
        {
            _scenes[_currentSceneIndex].End();
            Raylib.CloseWindow();
        }

        /// <summary>
        /// Adds a scene to the engine's scene array
        /// </summary>
        /// <param name="scene">The scene that will be added to the scene array</param>
        /// <returns>The index where the new scene is located</returns>
        public int AddScene(Scene scene)
        {
            //Create a new temporary array
            Scene[] tempArray = new Scene[_scenes.Length + 1];

            //Copy all values from old array into the new array
            for (int i = 0; i < _scenes.Length; i++)
            {
                tempArray[i] = _scenes[i];
            }

            //Set the last indec to be the new scene
            tempArray[_scenes.Length] = scene;

            //Set the old array to be the new array
            _scenes = tempArray;

            //Return the last index
            return _scenes.Length - 1;
        }

        /// <summary>
        /// Gets the next key in the input stream
        /// </summary>
        /// <returns>The key that was pressed</returns>
        public static ConsoleKey GetNextKey()
        {
            //If there is no key being pressed...
            if (!Console.KeyAvailable)
                //...return
                return 0;

            //Return the current key being pressed
            return Console.ReadKey(true).Key;
        }

        /// <summary>
        /// Ends the application
        /// </summary>
        public static void CloseApplication()
        {
            _applicationShouldClose = true;
        }
    }
}
