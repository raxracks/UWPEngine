using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using System;
using System.Diagnostics;
using System.Threading;
using Windows.Foundation;

namespace UWPEngine
{
    public sealed partial class MainPage : Page
    {
        // Define engine which will be used for engine calls
        Engine engine;



        /*                  Textures                    */
        Engine.EngineTexture aButtonTexture;
        Engine.EngineTexture bButtonTexture;
        Engine.EngineTexture xButtonTexture;
        Engine.EngineTexture yButtonTexture;
        Engine.EngineTexture spriteTexture;
        /*              End of textures                 */

        /*              Bounding Boxes                  */
        Engine.BoundingBox spriteBoundingBox;
        Engine.BoundingBox objectBoundingBox;
        /*           End of bounding boxes              */



        double spriteX = 475;
        double spriteY = 10;
        double objectX = 450;
        double objectY = 120;
        double FPS = 0;



        int framesInSecond = 0;



        Timer timer;



        // Deltatime Calculations
        DateTime time1 = DateTime.Now;
        DateTime time2 = DateTime.Now;

        public MainPage()
        {
            this.InitializeComponent();

            // Create engine instance
            engine = new Engine(canvas, grid);

            // Run start
            startAsync();
        }

        // Start game
        async void startAsync()
        {
            start();

            // Calls update every millisecond
            while (true)
            {
                update();
                await Task.Delay(1);
            }
        }

        void start()
        {
            /*                  Textures                    */
            aButtonTexture = engine.GenerateTexture("GamepadIcons/A.png", Stretch.Fill);
            bButtonTexture = engine.GenerateTexture("GamepadIcons/B.png", Stretch.Fill);
            xButtonTexture = engine.GenerateTexture("GamepadIcons/X.png", Stretch.Fill);
            yButtonTexture = engine.GenerateTexture("GamepadIcons/Y.png", Stretch.Fill);
            spriteTexture = engine.GenerateTexture("cookie.png", Stretch.Fill);
            /*              End of textures                 */

            // Start FPS timer
            timer = new Timer(FPSCounterTimerTick, null, 0, 1000);
        }

        // Moves frames per second to variable and resets current frames in second counter
        void FPSCounterTimerTick(object state)
        {
            FPS = framesInSecond;
            framesInSecond = 0;
        }

        void update()
        {
            // Check that rendering area is ready
            if (ActualWidth > 0)
            {
                // Deltatime Calculations
                time2 = DateTime.Now;
                float deltaTime = (time2.Ticks - time1.Ticks) / 10000000f;

                // Clear rendering area with specified color
                engine.Clear(Colors.CornflowerBlue);

                // Draw text
                engine.Text(10, 10, "Hello World!", 50, Colors.Black);

                // Draw a textured rectangle
                engine.TexturedRect(10, 90, 30, 30, aButtonTexture);

                // Draw an unfilled rectangle
                engine.UnfilledRect(10, 140, 100, 50, Colors.Black, 10, 5, 10);

                // Draw a filled rectangle
                engine.Rect(10, 230, 100, 100, Colors.Orange, -10);

                // Draw sprite and create a bounding box for it
                engine.TexturedRect(spriteX, spriteY, 50, 50, spriteTexture);
                spriteBoundingBox = engine.CreateBoundingBox(spriteX, spriteY, 50, 50);

                // Draw an object to stop the sprite from falling and create a bounding box for it
                engine.Rect(objectX, objectY, 200, 20, Colors.Green);
                objectBoundingBox = engine.CreateBoundingBox(objectX, objectY, 200, 20);

                // Check if colliding 
                if (engine.IntersectAABB(spriteBoundingBox, objectBoundingBox))
                {
                    // Display recognition of collision
                    engine.Text(objectX, objectY + 60, "Colliding", 20, Colors.Black);

                    // Stop sprite from falling through object (newton's third law)
                    spriteY -= 50 * deltaTime;
                }
                else
                {
                    // Display recognition of no collision
                    engine.Text(objectX, objectY + 60, "Not colliding", 20, Colors.Black);
                }

                // Controls
                if (engine.GetGamepad().DPadRight)
                {
                    spriteX += 50.75 * deltaTime;
                }

                if (engine.GetGamepad().DPadLeft)
                {
                    spriteX -= 50.75 * deltaTime;
                }

                if (engine.GetGamepad().DPadUp)
                {
                    spriteY -= 100.75 * deltaTime;
                }

                if (engine.GetGamepad().DPadDown)
                {
                    spriteY += 50.75 * deltaTime;
                }

                // Super simple "gravity" on the sprite
                spriteY += 50 * deltaTime;

                // Get size of string width and height in pixels
                Size stringSize = engine.GetStringSizePX("FPS: " + FPS, 25);

                // Display FPS
                engine.Text(ActualWidth - stringSize.Width - 50, ActualHeight - stringSize.Height - 50, "FPS: " + FPS, 25, Colors.Black, false, 0, Windows.UI.Xaml.TextAlignment.Right);

                // Deltatime calculations
                time1 = time2;
            }
            else
            {
                Debug.WriteLine("Wait! Still Loading...");
            }

            framesInSecond++;
        }
    }
}
