using GalaxyMarauders.Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using MonoGame.Extended.Entities;
using MonoGame.Extended.Entities.Systems;

namespace GalaxyMarauders.Systems {
    public class PlayerSystem : EntityUpdateSystem {
        private const float BulletTimeout = 0.33f;

        private readonly EntityFactory _entityFactory;
        private ComponentMapper<Transform2> _transformMapper;
        private float _bulletCountdown;

        public PlayerSystem(EntityFactory entityFactory) : base(Aspect.All(typeof(Transform2), typeof(SpaceShip))) {
            _entityFactory = entityFactory;
        }

        public override void Initialize(IComponentMapperService mapperService) {
            _transformMapper = mapperService.GetMapper<Transform2>();
        }

        public override void Update(GameTime gameTime) {
            _bulletCountdown -= gameTime.GetElapsedSeconds();

            foreach (var entity in ActiveEntities) {
                var transform = _transformMapper.Get(entity);

                var keyboardState = Keyboard.GetState();
                if (keyboardState.IsKeyDown(Keys.Left) && transform.Position.X >= 24) {
                    transform.Position += new Vector2(-1f, 0f);
                }
                else if (keyboardState.IsKeyDown(Keys.Right) && transform.Position.X <= 200) {
                    transform.Position += new Vector2(1f, 0f);
                }

                if (keyboardState.IsKeyDown(Keys.Z) && _bulletCountdown <= 0) {
                    _bulletCountdown = BulletTimeout;
                    _entityFactory.SpawnBullet(transform.WorldPosition);
                }
            }
        }
    }
}
