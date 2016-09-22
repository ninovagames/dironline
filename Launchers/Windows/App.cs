using System;
using System.IO;
using System.Reflection;
using System.Windows.Forms.Integration;
using WaveEngine.Common.Graphics;
using WaveEngine.Common.Input;
using WaveEngine.Common.Math;
using WaveEngine.Framework.Graphics;
using WaveEngine.Framework.Services;

namespace DirOnline
{
    public class App : WaveEngine.Adapter.Application
    {
        DirOnline.Game game;
        SpriteBatch spriteBatch;
        Texture2D splashScreen;
        bool splashState = false;
        public bool uiVisiblity = false;
        TimeSpan time;
        Vector2 position;
        Color backgroundSplashColor;
        public ElementHost ucHost;
		
        public App()
        {
            this.Width = 1280;
            this.Height = 720;
			this.FullScreen = false;
            this.WindowTitle = "DirOnline";
            this.HasVideoSupport = true;
            ucMain ui = new ucMain();
            this.ucHost = new ElementHost() { Child = ui };
            ui.Owner = this;
            ucHost.Width = Width; ucHost.Height = Height;
    }

        public override void Initialize()
        {
            this.game = new DirOnline.Game();
            this.game.Initialize(this);

            this.Form.Controls.Add(ucHost);
            this.ucHost.Visible = uiVisiblity;

            #region DEFAULT SPLASHSCREEN
            this.backgroundSplashColor = new Color("#ebebeb");
            this.spriteBatch = new SpriteBatch(WaveServices.GraphicsDevice);
            
            var resourceNames = Assembly.GetExecutingAssembly().GetManifestResourceNames();
            string name = string.Empty;

            foreach (string item in resourceNames)
            {
                if (item.Contains("SplashScreen.wpk"))
                {
                    name = item;
                    break;
                }
            }

            if (string.IsNullOrWhiteSpace(name))
            {
                //throw new InvalidProgramException("License terms not agreed.");
            }

            using (Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(name))
            {
                this.splashScreen = WaveServices.Assets.Global.LoadAsset<Texture2D>(name, stream);
            }

            position = new Vector2();
            position.X = (this.Width / 2.0f) - (this.splashScreen.Width / 2.0f);
            position.Y = (this.Height / 2.0f) - (this.splashScreen.Height / 2.0f);
            #endregion
        }

        public override void Update(TimeSpan elapsedTime)
        {
            if (this.game != null && !this.game.HasExited)
            {
                if (WaveServices.Input.KeyboardState.F11 == WaveEngine.Common.Input.ButtonState.Pressed)
                {
                    this.FullScreen = !this.FullScreen;
                }

                if (WaveServices.Input.KeyboardState.Escape == WaveEngine.Common.Input.ButtonState.Pressed)
                {
                    this.uiVisiblity = !this.uiVisiblity;
                    ucHost.Visible = this.uiVisiblity;
                    System.Threading.Thread.Sleep(250);
                }

                if (this.splashState)
                {
                    #region DEFAULT SPLASHSCREEN
                    this.time += elapsedTime;
                    if (time > TimeSpan.FromSeconds(2))
                    {
                        this.splashState = false;
                    }
                    #endregion
                }
                else
                {
                    if (WaveServices.Input.KeyboardState.F10 == WaveEngine.Common.Input.ButtonState.Pressed)
                    {
                        WaveServices.Platform.Exit();
                    }
                    else
                    {
                        this.game.UpdateFrame(elapsedTime);
                    }
                }
            }
        }

        public override void Draw(TimeSpan elapsedTime)
        {
            if (this.game != null && !this.game.HasExited)
            {
                if (this.splashState)
                {
                    #region DEFAULT SPLASHSCREEN
                    WaveServices.GraphicsDevice.RenderTargets.SetRenderTarget(null);
                    WaveServices.GraphicsDevice.Clear(ref this.backgroundSplashColor, ClearFlags.Target, 1);
                    this.spriteBatch.Draw(this.splashScreen, this.position, Color.White);
                    this.spriteBatch.Render();
                    #endregion
                }
                else
                {
                    this.game.DrawFrame(elapsedTime);
                }
            }
        }

        /// <summary>
        /// Called when [activated].
        /// </summary>
        public override void OnActivated()
        {
            base.OnActivated();
            if (this.game != null)
            {
                game.OnActivated();
            }
        }

        /// <summary>
        /// Called when [deactivate].
        /// </summary>
        public override void OnDeactivate()
        {
            base.OnDeactivate();
            if (this.game != null)
            {
                game.OnDeactivated();
            }
        }

        // Encrypt
        public static string Base64Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }
        // Decrypt
        public static string Base64Decode(string base64EncodedData)
        {
            var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
            return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
        }
        // Convert an object to a byte array
        public static byte[] ObjectToByteArray(Object obj)
        {
            System.Runtime.Serialization
                .Formatters.Binary.BinaryFormatter bf =
                    new System.Runtime.Serialization
                        .Formatters.Binary.BinaryFormatter();

            using (var ms = new MemoryStream())
            {
                bf.Serialize(ms, obj);
                return ms.ToArray();
            }
        }
        // Convert a byte array to an Object
        public static Object ByteArrayToObject(byte[] arrBytes)
        {
            using (var memStream = new MemoryStream())
            {
                var binForm = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                memStream.Write(arrBytes, 0, arrBytes.Length);
                memStream.Seek(0, SeekOrigin.Begin);
                var obj = binForm.Deserialize(memStream);
                return obj;
            }
        }

        public void HideUI()
        {
            ucHost.Visible = uiVisiblity = false;
            System.Threading.Thread.Sleep(200);
        }
    }
}

