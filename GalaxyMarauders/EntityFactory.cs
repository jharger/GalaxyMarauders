using GalaxyMarauders.Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.Animations;
using MonoGame.Extended.Animations.SpriteSheets;
using MonoGame.Extended.Entities;
using MonoGame.Extended.Sprites;
using MonoGame.Extended.TextureAtlases;

namespace GalaxyMarauders {
    public class EntityFactory {
        private readonly Game _game;
        private SpriteSheetAnimationFactory _animationFactory;
        private Texture2D _ship;
        private Texture2D _bullet;

        public World World { set; private get; }

        public EntityFactory(Game game) {
            _game = game;
        }

        public void LoadContent() {
            var aliens = _game.Content.Load<Texture2D>("aliens");
            var alienAtlas = TextureAtlas.Create("aliens",
                aliens,
                16,
                10);

            _animationFactory = new SpriteSheetAnimationFactory(alienAtlas);
            _animationFactory.Add("alien0", new SpriteSheetAnimationData(new[] {0, 1}, 0.5f));
            _animationFactory.Add("alien1", new SpriteSheetAnimationData(new[] {2, 3}, 0.5f));
            _animationFactory.Add("alien2", new SpriteSheetAnimationData(new[] {4, 5}, 0.5f));

            _ship = _game.Content.Load<Texture2D>("ship");
            _bullet = _game.Content.Load<Texture2D>("bullet");
        }

        public Entity SpawnShip() {
            var shipSprite = new Sprite(_ship);
            var transform = new Transform2(new Vector2(112, 236));
            var entity = World.CreateEntity();
            entity.Attach(shipSprite);
            entity.Attach(transform);
            entity.Attach(new SpaceShip());
            return entity;
        }

        public Entity SpawnAlien(int row, int column, int style) {
            var alienEntity = World.CreateEntity();
            var alienTransform = new Transform2(new Vector2(column * 16, row * 10));
            var alien = new Alien {Row = row, Column = column};
            alienEntity.Attach(new AnimatedSprite(_animationFactory, $"alien{style}"));
            alienEntity.Attach(alienTransform);
            alienEntity.Attach(alien);
            return alienEntity;
        }

        public Entity SpawnBullet(Vector2 worldPosition) {
            var entity = World.CreateEntity();
            var shipSprite = new Sprite(_bullet);
            var transform = new Transform2(worldPosition);
            entity.Attach(shipSprite);
            entity.Attach(transform);
            entity.Attach(new ShipBullet());
            return entity;
        }
    }
}
