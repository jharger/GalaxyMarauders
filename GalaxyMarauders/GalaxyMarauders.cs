using GalaxyMarauders.Systems;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace GalaxyMarauders {
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class GalaxyMarauders : Game {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _scalingBatch;
        private RenderTarget2D _scalingTarget;

        public GalaxyMarauders() {
            _graphics = new GraphicsDeviceManager(this) {
                PreferredBackBufferWidth = 1366, PreferredBackBufferHeight = 768
            };

            Content.RootDirectory = "Content";

            var enemySystem = new EnemySystem(this);
            Components.Add(enemySystem);
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize() {
            _scalingBatch = new SpriteBatch(GraphicsDevice);
            _scalingTarget = new RenderTarget2D(GraphicsDevice, 240, 256);

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent() { }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent() {
            Content.Unload();
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime) {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
                Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime) {
            int multiplier;
            if (GraphicsDevice.Viewport.Height <= GraphicsDevice.Viewport.Width) {
                multiplier = GraphicsDevice.Viewport.Height / _scalingTarget.Height;
            }
            else {
                multiplier = GraphicsDevice.Viewport.Width / _scalingTarget.Width;
            }

            var offsetX = (GraphicsDevice.Viewport.Width - _scalingTarget.Width * multiplier) / 2;
            var offsetY = (GraphicsDevice.Viewport.Height - _scalingTarget.Height * multiplier) / 2;

            // This will set the render target, used for upscaling the low resolution playing field to something
            // that fits the game viewport better
            GraphicsDevice.SetRenderTarget(_scalingTarget);
            GraphicsDevice.Clear(Color.Black);

            // This will signal the subsystems to draw
            base.Draw(gameTime);

            // Draw the game playing field to the main viewport
            GraphicsDevice.SetRenderTarget(null);
            GraphicsDevice.Clear(new Color(14, 16, 17));
            _scalingBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp);
            _scalingBatch.Draw(_scalingTarget,
                new Rectangle(offsetX,
                    offsetY,
                    _scalingTarget.Width * multiplier,
                    _scalingTarget.Height * multiplier),
                Color.White);
            _scalingBatch.End();
        }
    }
}
