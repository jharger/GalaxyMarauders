using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.Animations;
using MonoGame.Extended.Entities;
using MonoGame.Extended.Entities.Systems;
using MonoGame.Extended.Sprites;

namespace GalaxyMarauders.Systems {
    public class SpriteRenderSystem : EntityDrawSystem {
        private GraphicsDevice _graphicsDevice;
        private readonly SpriteBatch _spriteBatch;
        private ComponentMapper<Sprite> _spriteMapper;
        private ComponentMapper<AnimatedSprite> _animatedSpriteMapper;
        private ComponentMapper<Transform2> _transformMapper;

        public SpriteRenderSystem(GraphicsDevice graphicsDevice) : base(Aspect.All(typeof(Transform2))
            .One(typeof(Sprite), typeof(AnimatedSprite))) {
            _graphicsDevice = graphicsDevice;
            _spriteBatch = new SpriteBatch(_graphicsDevice);
        }

        public override void Initialize(IComponentMapperService mapperService) {
            _spriteMapper = mapperService.GetMapper<Sprite>();
            _animatedSpriteMapper = mapperService.GetMapper<AnimatedSprite>();
            _transformMapper = mapperService.GetMapper<Transform2>();
        }

        public override void Draw(GameTime gameTime) {
            _spriteBatch.Begin();

            foreach (var entity in ActiveEntities) {
                var sprite = _animatedSpriteMapper.Has(entity)
                    ? _animatedSpriteMapper.Get(entity)
                    : _spriteMapper.Get(entity);

                var transform = _transformMapper.Get(entity);
                if (sprite is AnimatedSprite animatedSprite) {
                    animatedSprite.Update(gameTime.GetElapsedSeconds());
                }

                if (sprite.IsVisible) {
                    var texture = sprite.TextureRegion.Texture;
                    var sourceRectangle = sprite.TextureRegion.Bounds;
                    var position = transform.WorldPosition;
                    var rotation = transform.WorldRotation;
                    var scale = transform.WorldScale;

                    _spriteBatch.Draw(texture,
                        position,
                        sourceRectangle,
                        sprite.Color,
                        -rotation,
                        sprite.Origin,
                        scale,
                        sprite.Effect,
                        0);
                }
            }

            _spriteBatch.End();
        }
    }
}
