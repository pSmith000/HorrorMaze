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
        Scene scene = new Scene();

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
            //Raylib.InitWindow(1920, 1080, "Math for Games");
            Raylib.InitWindow(900, 500, "Math for Games");
            Raylib.SetTargetFPS(60);

            InitializeCharacters();
            InitializeWalls();

            _scenes[_currentSceneIndex].Start();
        }

        /// <summary>
        /// Initializes the player and the enemy
        /// </summary>
        public void InitializeCharacters()
        {
            player = new Player(0, 1, -90, 20, "Player", Shape.SPHERE);
            _camera = new Camera(player);
            player.SetScale(1, 1, 1);
            CircleCollider playerCircleCollider = new CircleCollider(1, player);
            player.SetColor(new Vector4(10, 20, 200, 255));
            player.AddChild(_camera);

            Enemy enemy = new Enemy(0, 1, 50, 10, player, "Enemy", Shape.CUBE);
            enemy.SetTranslation(0, 1, 100);
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

            Random rnd = new Random();
            float randomX = rnd.Next(-95, 95);
            float randomZ = rnd.Next(-95, 95);
            Actor winCircle = new Actor(randomX, 3, randomZ, "Win", Shape.SPHERE);
            winCircle.SetColor(new Vector4(0, 255, 0, 255));
            winCircle.SetScale(3, 3, 3);
            CircleCollider winCollider = new CircleCollider(3, winCircle);
            winCircle.Collider = winCollider;
            
            player.Collider = playerCircleCollider;
            
            scene.AddActor(player);
            //scene.AddActor(enemy);
            scene.AddActor(enemyTorso);
            scene.AddActor(enemyHead);
            scene.AddActor(enemyEye1);
            scene.AddActor(_camera);
            scene.AddActor(winCircle);
            
            _currentSceneIndex = AddScene(scene);
        }

        /// <summary>
        /// Initializes all of the walls in the maze
        /// </summary>
        public void InitializeWalls()
        {
            Wall wall = new Wall(0, -1, 0, 200, 1, 200, scene);
            wall.SetColor(new Vector4(0, 0, 0, 255));

            Wall wall2 = new Wall(0, -2, 100, 200, 50, 2, scene);

            Wall wall3 = new Wall(0, -2, -100, 200, 50, 2, scene);

            Wall wall4 = new Wall(100, -2, 0, 2, 50, 200, scene);

            Wall wall5 = new Wall(-100, -2, 0, 2, 50, 200, scene);

            Wall wall6 = new Wall(10, -2, -90, 2, 50, 20, scene);

            Wall wall7 = new Wall(-10, -2, -90, 2, 50, 50, scene);
            wall7.SetScale(2, 50, 80);

            Wall wall8 = new Wall(34, -2, -80, 50, 50, 2, scene);

            Wall wall9 = new Wall(34, -2, -60, 50, 50, 2, scene);

            Wall wall10 = new Wall(80, -2, -70, 2, 50, 30, scene);

            Wall wall11 = new Wall(90, -2, -70, 20, 50, 2, scene);

            Wall wall12 = new Wall(60, -2, -41, 2, 50, 40, scene);

            Wall wall13 = new Wall(-27, -2, -66, 35, 50, 2, scene);

            Wall wall14 = new Wall(-27, -2, -50, 35, 50, 2, scene);

            Wall wall15 = new Wall(-60, -2, -54, 2, 50, 50, scene);

            Wall wall16 = new Wall(-55, -2, -79, 60, 50, 2, scene);

            Wall wall17 = new Wall(-85, -2, -55, 2, 50, 50, scene);

            Wall wall18 = new Wall(-10, -2, -40, 2, 50, 20, scene);

            Wall wall19 = new Wall(5, -2, -30, 32, 50, 2, scene);

            Wall wall20 = new Wall(25, -2, -45, 32, 50, 2, scene);

            Wall wall21 = new Wall(40, -2, -23, 2, 50, 45, scene);

            Wall wall22 = new Wall(30, -2, -30, 2, 50, 2, scene);
            wall22.SetScale(20, 50, 2);

            Wall wall23 = new Wall(80, -2, -20, 2, 50, 40, scene);

            Wall wall24 = new Wall(80, -2, 20, 40, 50, 2, scene);

            Wall wall25 = new Wall(60, -2, 0, 42, 50, 2, scene);

            Wall wall26 = new Wall(10, -2, -10, 60, 50, 2, scene);

            Wall wall27 = new Wall(20, -2, -5.5f, 2, 50, 11, scene);

            Wall wall28 = new Wall(-40, -2, -30, 40, 50, 2, scene);

            Wall wall29 = new Wall(30, -2, 0, 22, 50, 2, scene);

            Wall wall30 = new Wall(-20, -2, -10, 2, 50, 2, scene);
            wall30.SetScale(40, 50, 2);

            Wall wall31 = new Wall(-72.5f, -2, -30, 11, 50, 2, scene);

            Wall wall32 = new Wall(-72.5f, -2, -50, 1, 50, 40, scene);

            Wall wall33 = new Wall(-85, -2, -25, 2, 50, 30, scene);

            Wall wall34 = new Wall(-63, -2, -10, 46, 50, 2, scene);

            Wall wall35 = new Wall(-85, -2, 10, 30, 50, 2, scene);

            Wall wall36 = new Wall(-70, -2, 29, 2, 50, 41, scene);

            Wall wall37 = new Wall(-85, -2, 55, 2, 50, 50, scene);

            Wall wall38 = new Wall(-79, -2, 80, 60, 50, 2, scene);

            Wall wall39 = new Wall(-50, -2, 50, 2, 50, 60, scene);

            Wall wall40 = new Wall(-30, -2, 80, 2, 50, 60, scene);

            Wall wall41 = new Wall(-30, -2, 25, 2, 50, 30, scene);

            Wall wall42 = new Wall(80, -2, 35, 2, 50, 30, scene);

            Wall wall43 = new Wall(60, -2, 70, 40, 50, 2, scene);

            Wall wall44 = new Wall(50, -2, 75, 2, 50, 10, scene);

            Wall wall45 = new Wall(60, -2, 75, 2, 50, 10, scene);

            Wall wall46 = new Wall(70, -2, 75, 2, 50, 10, scene);

            Wall wall47 = new Wall(5, -2, 80, 50, 50, 2, scene);

            Wall wall48 = new Wall(5, -2, 90, 2, 50, 20, scene);

            Wall wall49 = new Wall(60, -2, 30, 2, 50, 22, scene);

            Wall wall50 = new Wall(40, -2, 30, 2, 50, 22, scene);

            Wall wall51 = new Wall(25, -2, 20, 30, 50, 2, scene);

            Wall wall52 = new Wall(10, -2, 16, 2, 50, 10, scene);

            Wall wall53 = new Wall(1, -2, 10, 20, 50, 2, scene);

            Wall wall54 = new Wall(-10, -2, 29, 2, 50, 40, scene);

            Wall wall55 = new Wall(10, -2, 65, 40, 50, 2, scene);

            Wall wall56 = new Wall(25, -2, 40, 30, 50, 2, scene);

            Wall wall57 = new Wall(80, -2, 60, 2, 50, 20, scene);

            Wall wall58 = new Wall(10, -2, 75, 2, 50, 10, scene);

            Wall wall59 = new Wall(0, -2, 70, 2, 50, 10, scene);

            Wall wall60 = new Wall(20, -2, 70, 2, 50, 10, scene);

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
