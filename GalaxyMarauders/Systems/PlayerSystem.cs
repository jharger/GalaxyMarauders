using GalaxyMarauders.Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using MonoGame.Extended.Entities;
using MonoGame.Extended.Entities.Systems;

namespace GalaxyMarauders.Systems {
    public class PlayerSystem : EntityUpdateSystem {
        private ComponentMapper<Transform2> _transformMapper;

        public PlayerSystem() : base(Aspect.All(typeof(Transform2), typeof(SpaceShip))) { }

        public override void Initialize(IComponentMapperService mapperService) {
            _transformMapper = mapperService.GetMapper<Transform2>();
        }

        public override void Update(GameTime gameTime) {
            foreach (var entity in ActiveEntities) {
                var transform = _transformMapper.Get(entity);

                var keyboardState = Keyboard.GetState();
                if (keyboardState.IsKeyDown(Keys.Left) && transform.Position.X >= 24) {
                    transform.Position += new Vector2(-1f, 0f);
                }
                else if (keyboardState.IsKeyDown(Keys.Right) && transform.Position.X <= 200) {
                    transform.Position += new Vector2(1f, 0f);
                }
            }
        }
    }
}
