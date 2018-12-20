using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MovingSprites.Managers;
using MovingSprites.Models;

namespace MovingSprites.Sprites
{
    public class Sprite
    {
        #region Fields

        protected AnimationManager _animationManager;

        protected Dictionary<string, Animation> _animations;

        protected Vector2 _position;

        protected Texture2D _texture;

        protected bool hasJumped = false;

        #endregion

        #region Properties

        public Input Input;

        public Vector2 Position
        {
            get { return _position; }
            set
            {
                _position = value;

                if (_animationManager != null)
                    _animationManager.Position = _position;
            }
        }

        public float Speed = 1f;

        public Vector2 Velocity;

       

        #endregion

        #region Methods

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            if (_texture != null)
                spriteBatch.Draw(_texture, Position, Color.White);
            else if (_animationManager != null)
                _animationManager.Draw(spriteBatch);
            else throw new Exception("This ain't right..!");
        }

          public Sprite(Texture2D texture)
        {
            _texture = texture;
        }

        public virtual void Move()
        {
            Speed = 4;
            if (Keyboard.GetState().IsKeyDown(Input.Up))
                Velocity.Y = -Speed;
            else if (Keyboard.GetState().IsKeyDown(Input.Down))
                Velocity.Y = Speed;
            else if (Keyboard.GetState().IsKeyDown(Input.Left))
                Velocity.X = -Speed;
            else if (Keyboard.GetState().IsKeyDown(Input.Right))
                Velocity.X = Speed;
        }

        // float Bleedoff = 1.0f;
        //readonly Vector2 gravity = new Vector2(0, -9.8f);

        public virtual void Jump()
        {
            //float time = (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (Keyboard.GetState().IsKeyDown(Input.Space) && hasJumped == false)
            {
                _position.Y -= 55f;
                Velocity.Y = -10;
                
                hasJumped = true;

                

            }

            if (hasJumped == true)
            {
               
               float i = 1f;
                Velocity.Y = -5f;
                if(_position.Y >= 500)
                {
                    Velocity.Y = 1000;
                }

            }

            if(_position.Y >= 350)
            {
                hasJumped = false;
            }

            if (hasJumped == false)
            {
                Velocity.Y = 0f;
            }
        }

        protected virtual void SetAnimations()
        {
            if (Velocity.X > 0)
                _animationManager.Play(_animations["WalkRight"]);
            else if (Velocity.X < 0)
                _animationManager.Play(_animations["WalkLeft"]);
                
           
            else _animationManager.Stop();
        }

        public Sprite(Dictionary<string, Animation> animations)
        {
            _animations = animations;
            _animationManager = new AnimationManager(_animations.First().Value);
        }

      

        public virtual void Update(GameTime gameTime, List<Sprite> sprites)
        {
            
            Move();
            Jump();

            SetAnimations();

            _animationManager.Update(gameTime);

            Position += Velocity;
            Velocity = Vector2.Zero;
        }

        #endregion
    }
}
