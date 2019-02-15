using GalaxyMarauders.Components;
using Microsoft.Xna.Framework;
using MonoGame.Extended;
using MonoGame.Extended.Entities;
using MonoGame.Extended.Entities.Systems;

namespace GalaxyMarauders.Systems {
    public class ShipBulletMovementSystem : EntityUpdateSystem {
        private const float BulletSpeed = 256f;

        private ComponentMapper<Transform2> _transformMapper;

        public ShipBulletMovementSystem() : base(Aspect.All(typeof(Transform2), typeof(ShipBullet))) { }

        public override void Initialize(IComponentMapperService mapperService) {
            _transformMapper = mapperService.GetMapper<Transform2>();
        }

        public override void Update(GameTime gameTime) {
            foreach (var entity in ActiveEntities) {
                var transform = _transformMapper.Get(entity);
                transform.Position -= new Vector2(0f, BulletSpeed * gameTime.GetElapsedSeconds());
                if (transform.Position.Y <= 0) {
                    DestroyEntity(entity);
                }
            }
        }
    }
}
