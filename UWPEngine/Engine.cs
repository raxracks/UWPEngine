using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Gaming.Input;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Shapes;

namespace UWPEngine
{
    public class Engine
    {
        Canvas ctx;
        Grid grid;
        private Gamepad _Gamepad = null;
        bool gamepadConnected = false;
        List<Windows.System.VirtualKey> keys = new List<Windows.System.VirtualKey>();

        public Engine(Canvas canvas, Grid grid)
        {
            this.grid = grid;
            ctx = canvas;

            Gamepad.GamepadAdded += Gamepad_GamepadAdded;
            Gamepad.GamepadRemoved += Gamepad_GamepadRemoved;
            Window.Current.CoreWindow.KeyDown += CoreWindow_KeyDown;
            Window.Current.CoreWindow.KeyUp += CoreWindow_KeyUp;
        }

        private void CoreWindow_KeyDown(CoreWindow sender, KeyEventArgs args)
        {
            keys.Add(args.VirtualKey);
        }

        private void CoreWindow_KeyUp(CoreWindow sender, KeyEventArgs args)
        {
            keys.Remove(args.VirtualKey);
        }

        public bool IsKeyboardKeyDown(Windows.System.VirtualKey key)
        {
            return keys.Contains(key);
        }

        private void Gamepad_GamepadRemoved(object sender, Gamepad e)
        {
            _Gamepad = null;
            gamepadConnected = false;
        }

        private void Gamepad_GamepadAdded(object sender, Gamepad e)
        {
            _Gamepad = e;
            gamepadConnected = true;
        }

        public void Rect(double x, double y, double width, double height, Color color, double rotation = 0)
        {
            RotateTransform rotateTransform = new RotateTransform();
            rotateTransform.Angle = rotation;
            Rectangle rect = new Rectangle();
            rect.Stroke = new SolidColorBrush(color);
            rect.Fill = new SolidColorBrush(color);
            rect.Width = width;
            rect.Height = height;
            Canvas.SetLeft(rect, x);
            Canvas.SetTop(rect, y);
            rect.RenderTransform = rotateTransform;

            ctx.Children.Add(rect);
        }

        public void UnfilledRect(double x, double y, double width, double height, Color color, double rotation = 0, double thickness = 0, double border_radius = 0)
        {
            RotateTransform rotateTransform = new RotateTransform();
            rotateTransform.Angle = rotation;
            Rectangle rect = new Rectangle();
            rect.Stroke = new SolidColorBrush(color);
            rect.Fill = null;
            rect.Width = width;
            rect.Height = height;
            rect.StrokeThickness = thickness;
            rect.RadiusX = border_radius;
            rect.RadiusY = border_radius;
            Canvas.SetLeft(rect, x);
            Canvas.SetTop(rect, y);
            rect.RenderTransform = rotateTransform;

            ctx.Children.Add(rect);
        }

        public void Text(double x, double y, string text, int fontSize, Color color, bool manualWidth = false, double width = 0, Windows.UI.Xaml.TextAlignment textAlignment = Windows.UI.Xaml.TextAlignment.Left)
        {
            TextBlock textBlock = new TextBlock();
            textBlock.Text = text;
            textBlock.FontSize = fontSize;
            textBlock.Foreground = new SolidColorBrush(color);
            if (manualWidth)
            {
                textBlock.Width = width;
            }

            textBlock.HorizontalTextAlignment = textAlignment;

            Canvas.SetLeft(textBlock, x);
            Canvas.SetTop(textBlock, y);

            ctx.Children.Add(textBlock);
        }

        public Size GetStringSizePX(string text, int fontSize)
        {
            var tb = new TextBlock { Text = text, FontSize = fontSize };
            tb.Measure(new Size(Double.PositiveInfinity, Double.PositiveInfinity));
            return tb.DesiredSize;
        }

        private ImageSource GetImageSource(string imagePath)
        {
            BitmapImage glowIcon = new BitmapImage();

            glowIcon.UriSource = new Uri("ms-appx:///Assets/GameAssets/" + imagePath);

            return glowIcon;
        }

        public struct EngineTexture
        {
            public ImageBrush imgBrush;
        }

        public struct BoundingBox
        {
            public double x;
            public double y;
            public double width;
            public double height;
        }

        public BoundingBox CreateBoundingBox(double x, double y, double width, double height)
        {
            BoundingBox bBox = new BoundingBox();
            bBox.x = x;
            bBox.y = y;
            bBox.width = width;
            bBox.height = height;

            return bBox;
        }

        public bool IntersectAABB(BoundingBox b1, BoundingBox b2)
        {
            return b1.x < b2.x + b2.width && b1.x + b1.width > b2.x && b1.y < b2.y + b2.height && b1.y + b1.height > b2.y;
        }

        public void DrawBoundingBox(BoundingBox bBox)
        {
            Rect(bBox.x, bBox.y, bBox.width, bBox.height, Color.FromArgb(100, 191, 242, 133));
        }

        /// <summary>
        /// Uses Assets/GameAssets as root folder, you can make folders inside the Assets/GameAssets folder but cannot have files outside.
        /// </summary>
        /// <param name="GenerateTexture"></param>
        /// <returns>EngineTexture</returns>
        public EngineTexture GenerateTexture(string imagePath, Stretch stretchMethod)
        {
            EngineTexture engineTexture = new EngineTexture();
            ImageBrush imgBrush = new ImageBrush();
            imgBrush.Stretch = stretchMethod;
            imgBrush.ImageSource = GetImageSource(imagePath);
            engineTexture.imgBrush = imgBrush;
            return engineTexture;
        }

        public void TexturedRect(double x, double y, double width, double height, EngineTexture texture, double rotation = 0, double border_radius = 0)
        {
            RotateTransform rotateTransform = new RotateTransform();
            rotateTransform.Angle = rotation;
            rotateTransform.CenterX = width / 2;
            rotateTransform.CenterY = height / 2;
            Rectangle rect = new Rectangle();
            rect.Fill = texture.imgBrush;
            rect.Width = width;
            rect.Height = height;
            rect.RadiusX = border_radius;
            rect.RadiusY = border_radius;
            Canvas.SetLeft(rect, x);
            Canvas.SetTop(rect, y);
            rect.RenderTransform = rotateTransform;

            ctx.Children.Add(rect);
        }

        public async Task<string> GetKeyboardInputAsync()
        {
            TextBox input = new TextBox();
            input.Width = 0;
            input.Height = 0;
            Canvas.SetLeft(input, 0);
            Canvas.SetTop(input, 0);

            grid.Children.Add(input);

            input.Focus(Windows.UI.Xaml.FocusState.Programmatic);

            input.KeyDown += Input_KeyDown;
            input.LostFocus += Input_LostFocus;
            input.FocusDisengaged += Input_FocusDisengaged;

            string text = "";

            while (grid.Children.Contains(input))
            {
                text = input.Text;
                await Task.Delay(1);
            }

            return text;
        }

        private void Input_FocusDisengaged(Control sender, FocusDisengagedEventArgs args)
        {
            grid.Children.Remove(sender as TextBox);
        }

        private void Input_LostFocus(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            grid.Children.Remove(sender as TextBox);
        }

        private void Input_KeyDown(object sender, Windows.UI.Xaml.Input.KeyRoutedEventArgs e)
        {
            e.Handled = true;
            if (e.Key == Windows.System.VirtualKey.Enter || e.Key == Windows.System.VirtualKey.GamepadB)
            {
                grid.Children.Remove(sender as TextBox);
            }
        }

        public void Clear(Color color)
        {
            ctx.Children.Clear();
            ctx.Background = new SolidColorBrush(color);
        }

        public struct GamepadData
        {
            public double LT;
            public double RT;
            public double LSX;
            public double LSY;
            public double RSX;
            public double RSY;
            public bool A;
            public bool B;
            public bool X;
            public bool Y;
            public bool LB;
            public bool RB;
            public bool LS;
            public bool RS;
            public bool DPadLeft;
            public bool DPadRight;
            public bool DPadUp;
            public bool DPadDown;
            public bool Connected;
        }

        public GamepadData GetGamepad()
        {
            GamepadData data;

            if (gamepadConnected)
            {
                var reading = _Gamepad.GetCurrentReading();

                data.LT = reading.LeftTrigger;
                data.RT = reading.RightTrigger;
                data.LSX = reading.LeftThumbstickX;
                data.LSY = reading.LeftThumbstickY;
                data.RSX = reading.RightThumbstickX;
                data.RSY = reading.RightThumbstickY;
                data.A = (reading.Buttons & GamepadButtons.A) == GamepadButtons.A;
                data.B = (reading.Buttons & GamepadButtons.B) == GamepadButtons.B;
                data.X = (reading.Buttons & GamepadButtons.X) == GamepadButtons.X;
                data.Y = (reading.Buttons & GamepadButtons.Y) == GamepadButtons.Y;
                data.LB = (reading.Buttons & GamepadButtons.LeftShoulder) == GamepadButtons.LeftShoulder;
                data.RB = (reading.Buttons & GamepadButtons.RightShoulder) == GamepadButtons.RightShoulder;
                data.LS = (reading.Buttons & GamepadButtons.LeftThumbstick) == GamepadButtons.LeftThumbstick;
                data.RS = (reading.Buttons & GamepadButtons.RightThumbstick) == GamepadButtons.RightThumbstick;
                data.DPadLeft = (reading.Buttons & GamepadButtons.DPadLeft) == GamepadButtons.DPadLeft;
                data.DPadRight = (reading.Buttons & GamepadButtons.DPadRight) == GamepadButtons.DPadRight;
                data.DPadUp = (reading.Buttons & GamepadButtons.DPadUp) == GamepadButtons.DPadUp;
                data.DPadDown = (reading.Buttons & GamepadButtons.DPadDown) == GamepadButtons.DPadDown;
                data.Connected = true;
            }
            else
            {
                data.LT = 0;
                data.RT = 0;
                data.LSX = 0;
                data.LSY = 0;
                data.RSX = 0;
                data.RSY = 0;
                data.A = false;
                data.B = false;
                data.X = false;
                data.Y = false;
                data.LB = false;
                data.RB = false;
                data.LS = false;
                data.RS = false;
                data.DPadLeft = false;
                data.DPadRight = false;
                data.DPadUp = false;
                data.DPadDown = false;
                data.Connected = false;
            }

            return data;
        }
    }
}
