using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace GalaxyMarauders.Systems {
    public class PlayerSystem : DrawableGameComponent {
        private SpriteBatch _spriteBatch;
        private Texture2D _shipSprite;
        private Vector2 _position;

        public PlayerSystem(Game game) : base(game) { }

        public override void Initialize() {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            _position = new Vector2(112, 236);

            base.Initialize();
        }

        protected override void LoadContent() {
            _shipSprite = Game.Content.Load<Texture2D>("Ship");

            base.LoadContent();
        }

        public override void Update(GameTime gameTime) {
            var keyboardState = Keyboard.GetState();
            if (keyboardState.IsKeyDown(Keys.Left) && _position.X >= 16) {
                _position += new Vector2(-1f, 0f);
            }
            else if (keyboardState.IsKeyDown(Keys.Right) && _position.X <= 192) {
                _position += new Vector2(1f, 0f);
            }

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime) {
            _spriteBatch.Begin();
            _spriteBatch.Draw(_shipSprite, _position);
            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
