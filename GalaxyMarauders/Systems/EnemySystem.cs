using System.Collections.Generic;
using GalaxyMarauders.Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.Entities;
using MonoGame.Extended.Entities.Systems;

namespace GalaxyMarauders.Systems {
    public class EnemySystem : EntityUpdateSystem {
        private const int HorizontalSpeed = 2;
        private const int VerticalSpeed = 2;

        private Vector2 _fleetPosition = new Vector2(32, 24);
        private float _stepTime = .05f;
        private float _countdown;
        private int _frame;
        private int _direction = HorizontalSpeed;
        private ComponentMapper<Transform2> _transformMapper;
        private ComponentMapper<Alien> _alienMapper;


        public EnemySystem() : base(Aspect.All(typeof(Transform2), typeof(Alien))) { }

        public override void Initialize(IComponentMapperService mapperService) {
            _transformMapper = mapperService.GetMapper<Transform2>();
            _alienMapper = mapperService.GetMapper<Alien>();
            _countdown = _stepTime;
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

                foreach (var entity in ActiveEntities) {
                    var transform = _transformMapper.Get(entity);
                    var alien = _alienMapper.Get(entity);
                    transform.Position = _fleetPosition + new Vector2(alien.Column * 16, alien.Row * 10);
                }

                _countdown = _stepTime;
            }
        }
    }
}
