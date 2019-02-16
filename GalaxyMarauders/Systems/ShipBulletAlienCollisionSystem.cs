using System.Collections.Generic;
using System.Linq;
using GalaxyMarauders.Components;
using Microsoft.Xna.Framework;
using MonoGame.Extended;
using MonoGame.Extended.Animations;
using MonoGame.Extended.Entities;
using MonoGame.Extended.Entities.Systems;
using MonoGame.Extended.Sprites;

namespace GalaxyMarauders.Systems {
    public class ShipBulletAlienCollisionSystem : EntityUpdateSystem {
        private ComponentMapper<Transform2> _transformMapper;
        private ComponentMapper<ShipBullet> _bulletMapper;
        private ComponentMapper<Alien> _alienMapper;
        private ComponentMapper<Sprite> _spriteMapper;
        private ComponentMapper<AnimatedSprite> _animatedSpriteMapper;
        private List<int> _bullets;
        private List<int> _aliens;
        private HashSet<int> _bulletsToDestroy;

        public ShipBulletAlienCollisionSystem() : base(Aspect.All(typeof(Transform2))
            .One(typeof(ShipBullet), typeof(Alien))
            .One(typeof(Sprite), typeof(AnimatedSprite))) { }

        public override void Initialize(IComponentMapperService mapperService) {
            _bullets = new List<int>();
            _aliens = new List<int>();
            _bulletsToDestroy = new HashSet<int>();
            _transformMapper = mapperService.GetMapper<Transform2>();
            _bulletMapper = mapperService.GetMapper<ShipBullet>();
            _alienMapper = mapperService.GetMapper<Alien>();
            _animatedSpriteMapper = mapperService.GetMapper<AnimatedSprite>();
            _spriteMapper = mapperService.GetMapper<Sprite>();
        }

        public override void Update(GameTime gameTime) {
            _bullets.AddRange(ActiveEntities.Where(e => _bulletMapper.Has(e)));
            _aliens.AddRange(ActiveEntities.Where(e => _alienMapper.Has(e)));

            foreach (var alien in _aliens) {
                var alienSprite = _animatedSpriteMapper.Get(alien);
                var alienTransform = _transformMapper.Get(alien);
                var alienRect = alienSprite.GetBoundingRectangle(alienTransform);
                var destroyAlien = false;
                foreach (var bullet in _bullets) {
                    if (_bulletsToDestroy.Contains(bullet)) {
                        continue;
                    }
                    var bulletSprite = _spriteMapper.Get(bullet);
                    var bulletTransform = _transformMapper.Get(bullet);
                    var bulletRect = bulletSprite.GetBoundingRectangle(bulletTransform);

                    if (bulletRect.Intersects(alienRect)) {
                        destroyAlien = true;
                        _bulletsToDestroy.Add(bullet);
                        break;
                    }
                }

                if (destroyAlien) {
                    DestroyEntity(alien);
                }
            }

            foreach (var destroy in _bulletsToDestroy) {
                DestroyEntity(destroy);
            }

            _bulletsToDestroy.Clear();
            _bullets.Clear();
            _aliens.Clear();
        }
    }
}
