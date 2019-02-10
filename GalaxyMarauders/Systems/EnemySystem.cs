using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GalaxyMarauders.Systems {
    public class EnemySystem : DrawableGameComponent {
        private enum AlienStyle {
            Top = 0,
            Middle = 1,
            Bottom = 2
        }

        private class Alien {
            public Vector2 Position { get; set; }
            public readonly AlienStyle Style;

            public Alien(AlienStyle style) {
                Style = style;
            }
        }

        private const int HorizontalSpeed = 2;
        private const int VerticalSpeed = 2;

        private SpriteBatch _spriteBatch;
        private Texture2D _atlas;
        private LinkedList<Alien> _aliens = new LinkedList<Alien>();
        private Vector2 _fleetPosition = new Vector2(32, 24);
        private float _stepTime = .05f;
        private float _countdown;
        private int _frame;
        private int _direction = HorizontalSpeed;


        public EnemySystem(Game game) : base(game) { }

        public override void Initialize() {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            for (var row = 0; row < 6; row++) {
                var style = (AlienStyle) (row / 2);
                for (var column = 0; column < 11; column++) {
                    var alien = new Alien(style) {Position = new Vector2(column * 16, row * 10)};
                    _aliens.AddLast(alien);
                }
            }

            _countdown = _stepTime;

            base.Initialize();
        }

        protected override void LoadContent() {
            _atlas = Game.Content.Load<Texture2D>("Aliens");

            base.LoadContent();
        }

        public override void Update(GameTime gameTime) {
            _countdown -= (float) gameTime.ElapsedGameTime.TotalSeconds;
            if (_countdown <= 0f) {
                _frame = (_frame + 1) % 2;
                _fleetPosition += new Vector2(_direction, 0f);
                if (_fleetPosition.X > 48 || _fleetPosition.X < 16f) {
                    _direction = -_direction;
                    _fleetPosition += new Vector2(_direction, VerticalSpeed);
                }

                _countdown = _stepTime;
            }

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime) {
            _spriteBatch.Begin();
            foreach (var alien in _aliens) {
                var sourceRectangle = new Rectangle(_frame * 16,
                    (int) alien.Style * 10,
                    16,
                    10);
                _spriteBatch.Draw(_atlas,
                    _fleetPosition + alien.Position,
                    sourceRectangle,
                    Color.White);
            }

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
