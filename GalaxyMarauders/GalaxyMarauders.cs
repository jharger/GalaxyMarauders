using System.Xml;
using GalaxyMarauders.Components;
using GalaxyMarauders.Systems;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using MonoGame.Extended.Animations;
using MonoGame.Extended.Animations.SpriteSheets;
using MonoGame.Extended.Entities;
using MonoGame.Extended.Sprites;
using MonoGame.Extended.TextureAtlases;

namespace GalaxyMarauders {
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class GalaxyMarauders : Game {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _scalingBatch;
        private RenderTarget2D _scalingTarget;
        private EntityFactory _entityFactory;

        private World _world;

        public GalaxyMarauders() {
            _graphics = new GraphicsDeviceManager(this) {
                PreferredBackBufferWidth = 1366, PreferredBackBufferHeight = 768
            };

            _entityFactory = new EntityFactory(this);

            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize() {
            var worldBuilder = new WorldBuilder();
            var playerSystem = new PlayerSystem(_entityFactory);
            _world = worldBuilder.AddSystem(new SpriteRenderSystem(GraphicsDevice))
                .AddSystem(playerSystem)
                .AddSystem(new EnemySystem())
                .AddSystem(new ShipBulletMovementSystem())
                .AddSystem(new ShipBulletAlienCollisionSystem())
                .Build();
            _entityFactory.World = _world;

            _scalingBatch = new SpriteBatch(GraphicsDevice);
            _scalingTarget = new RenderTarget2D(GraphicsDevice, 224, 256);

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent() {
            _entityFactory.LoadContent();

            _entityFactory.SpawnShip();

            for (var row = 0; row < 6; row++) {
                var style = row / 2;
                for (var column = 0; column < 11; column++) {
                    _entityFactory.SpawnAlien(row, column, style);
                }
            }
        }

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

            _world.Update(gameTime);
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
            _world.Draw(gameTime);
            base.Draw(gameTime);

            // Draw the game playing field to the main viewport
            GraphicsDevice.SetRenderTarget(null);
            GraphicsDevice.Clear(new Color(34, 36, 37));
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
