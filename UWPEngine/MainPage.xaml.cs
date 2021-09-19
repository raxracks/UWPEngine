using Windows.UI;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using System;
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



        float spriteX = 475;
        float spriteY = -10;
        float objectX = 450;
        float objectY = 120;
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
        }

        // Moves frames per second to variable and resets current frames in second counter
        void FPSCounterTimerTick(object state)
        {
            FPS = framesInSecond;
            framesInSecond = 0;
        }

        private async void canvas_CreateResources(Microsoft.Graphics.Canvas.UI.Xaml.CanvasAnimatedControl sender, Microsoft.Graphics.Canvas.UI.CanvasCreateResourcesEventArgs args)
        {
            /*                  Textures                    */
            aButtonTexture = await engine.GenerateTexture("GamepadIcons/A.png", Stretch.Fill);
            bButtonTexture = await engine.GenerateTexture("GamepadIcons/B.png", Stretch.Fill);
            xButtonTexture = await engine.GenerateTexture("GamepadIcons/X.png", Stretch.Fill);
            yButtonTexture = await engine.GenerateTexture("GamepadIcons/Y.png", Stretch.Fill);
            spriteTexture = await engine.GenerateTexture("cookie.png", Stretch.Fill);
            /*              End of textures                 */

            // Start FPS timer
            timer = new Timer(FPSCounterTimerTick, null, 0, 1000);
        }

        private void canvas_Draw(Microsoft.Graphics.Canvas.UI.Xaml.ICanvasAnimatedControl sender, Microsoft.Graphics.Canvas.UI.Xaml.CanvasAnimatedDrawEventArgs args)
        {
            // Update drawing session
            engine.SetDrawingSession(args.DrawingSession);

            // Clear rendering area with specified color
            engine.Clear(Colors.CornflowerBlue);

            // Draw text
            engine.Text(10, 10, "Hello World!", 50, Colors.Black);

            // Draw a textured rectangle
            engine.TexturedRect(10, 90, 30, 30, aButtonTexture);

            // Draw an unfilled rectangle
            engine.UnfilledRect(10, 140, 100, 50, Colors.Black, 10, 5, 10);

            // Draw sprite and floor
            engine.TexturedRect(spriteX, spriteY, 50, 50, spriteTexture);
            engine.Rect(objectX, objectY, 200, 20, Colors.Green);

            // Display FPS
            Rect FPSRect = engine.GetStringSizePX("FPS: " + FPS, 25, Microsoft.Graphics.Canvas.Text.CanvasHorizontalAlignment.Right);
            engine.Text((float)sender.Size.Width - (float)FPSRect.Width, (float)sender.Size.Height - (float)FPSRect.Height, "FPS: " + FPS, 25, Colors.Black);

            // Increment frame count for FPS calculations
            framesInSecond++;
        }

        private void canvas_Update(Microsoft.Graphics.Canvas.UI.Xaml.ICanvasAnimatedControl sender, Microsoft.Graphics.Canvas.UI.Xaml.CanvasAnimatedUpdateEventArgs args)
        {
            // Deltatime Calculations
            time2 = DateTime.Now;
            float deltaTime = (time2.Ticks - time1.Ticks) / 10000000f;

            // Check if colliding 
            if (engine.IntersectAABB(spriteBoundingBox, objectBoundingBox))
            {
                // Stop sprite from falling through object (newton's third law)
                spriteY -= 50 * deltaTime;
            }

            // Controls
            if (engine.GetGamepad().DPadRight)
            {
                spriteX += 50.75f * deltaTime;
            }

            if (engine.GetGamepad().DPadLeft)
            {
                spriteX -= 50.75f * deltaTime;
            }

            if (engine.GetGamepad().DPadUp)
            {
                spriteY -= 100.75f * deltaTime;
            }

            if (engine.GetGamepad().DPadDown)
            {
                spriteY += 50.75f * deltaTime;
            }

            // Super simple "gravity" on the sprite
            spriteY += 50 * deltaTime;

            // Draw sprite and create a bounding box for it
            spriteBoundingBox = engine.CreateBoundingBox(spriteX, spriteY, 50, 50);

            // Draw an object to stop the sprite from falling and create a bounding box for it
            objectBoundingBox = engine.CreateBoundingBox(objectX, objectY, 200, 20);

            // Deltatime calculations
            time1 = time2;
        }
    }
}