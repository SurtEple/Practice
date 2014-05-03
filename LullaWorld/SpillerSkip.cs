using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Net;

namespace LullaWorld
{
    class SpillerSkip : GameComponent
    {
        
      
        private Viewport _viewPort;
        private Texture2D _spillerTexture;
        private SpriteBatch _spriteBatch;
        private GraphicsDeviceManager _graphics;
        private Color[] _collisionMapData; //Lagre farger i Kart-texturen
        private Texture2D _collisionMap; //Texture til kart
        private Vector2 turretCoords= new Vector2(100,100);

        private const float Circle = MathHelper.Pi * 2;
        private const float Gravitasjon = 0.050f;
       
        private const float SpillerScale = 0.2f;
        private const float Maxspeed = 0.05f;


        private Vector2 _spillerPosisjon;
        private Vector2 _gammelPosisjon;
        private Vector2 _spillerRotasjonsPunkt;
        private Vector2 _spillerHastighet;

        private float _spillerAksellerasjon;
        private float _spillerRotasjon;
        private float _gammelRotasjon;
       

        private int _spillerHelse=100;
        private int _spillerSkjold=50;

 


        public SpillerSkip(Game game, GraphicsDeviceManager graphics) : base(game)
        {
            LoadContent(game, graphics); //Last inn content
        }
         

        protected void LoadContent(Game game, GraphicsDeviceManager graphics)
        {
            _viewPort = graphics.GraphicsDevice.Viewport;
            _graphics = graphics;

            //Romskipet
           _spillerTexture = game.Content.Load<Texture2D>("ship.png");
           _spillerPosisjon = new Vector2(_viewPort.Width / 2, _viewPort.Height / 2);
           _gammelPosisjon = _spillerPosisjon;
           _spillerRotasjonsPunkt = new Vector2(_spillerTexture.Width / 2, _spillerTexture.Height / 2);
          


            //Kart
           _collisionMap = game.Content.Load<Texture2D>("mapwithturrets");
           _collisionMapData = new Color[_collisionMap.Width * _collisionMap.Height]; //Nytt array 
           _collisionMap.GetData(_collisionMapData);  //Hente farger??

            
         

 
        }

        public override void Update(GameTime gameTime)
        {
        
           _gammelRotasjon = _spillerRotasjon; 
           _gammelPosisjon = _spillerPosisjon;
        
           
            _spillerHastighet += new Vector2((float) Math.Cos(_spillerRotasjon), (float) Math.Sin(_spillerRotasjon))*
                                _spillerAksellerasjon;

            _spillerHastighet += Gravitasjon*new Vector2(0, 1f); //Gravitasjon virker nedover
            _spillerPosisjon += _spillerHastighet;
            _spillerPosisjon = Vector2.Lerp(_gammelPosisjon, _spillerPosisjon, 0.8f);
            _spillerRotasjon = MathHelper.Lerp(_gammelRotasjon, _spillerRotasjon, 0.3f);



            //Kollidere med "bakken"
            if(Collision(_spillerPosisjon))
            // if (_spillerPosisjon.Y >= 500)
            {
                _spillerPosisjon = _gammelPosisjon;
                _spillerHastighet= new Vector2(0f,0f);
               
            }

            //Holde skipet innenfor skjermen
            if (_spillerPosisjon.X <= 5)
            {
                _spillerPosisjon = _gammelPosisjon;
                _spillerHastighet = new Vector2(0f, 0f);
            }
            if (_spillerPosisjon.X >= _viewPort.Width -5)
            {
                _spillerPosisjon = _gammelPosisjon;
                _spillerHastighet = new Vector2(0f, 0f);
            }

            if (_spillerPosisjon.Y <= 5)
            {
                _spillerPosisjon = _gammelPosisjon;
                _spillerHastighet = new Vector2(0f, 0f);
            }
            if (_spillerRotasjon == MathHelper.TwoPi) _spillerRotasjon = 0;
            if (_spillerRotasjon == MathHelper.TwoPi * -1) _spillerRotasjon = 0;


            if (Keyboard.GetState().IsKeyDown(Keys.D))
            {
                _spillerRotasjon += 0.05f;
                _spillerRotasjon = _spillerRotasjon % Circle;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.A))
            {
                _spillerRotasjon -= 0.05f;
                _spillerRotasjon = _spillerRotasjon % Circle;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.W))
            {
                if (_spillerAksellerasjon < 0.1f)
                _spillerAksellerasjon += 0.01f;
            }
            else
            {
                if (_spillerAksellerasjon > 0)
                    _spillerAksellerasjon -= 0.02f;

            }
           

        }
        
        


        public void Draw(SpriteBatch spriteBatch)
        {
            _spriteBatch = spriteBatch;
            spriteBatch.Begin(); //Begin drawing stuff
            spriteBatch.Draw(_collisionMap, new Rectangle(0, 0, _collisionMap.Width, _collisionMap.Height), Color.White);

       
            spriteBatch.Draw(_spillerTexture, turretCoords, null, Color.Red, 0f, _spillerRotasjonsPunkt, new Vector2(SpillerScale, SpillerScale), SpriteEffects.None, 0f);

            spriteBatch.Draw(_spillerTexture, _spillerPosisjon, null, Color.Magenta, _spillerRotasjon, _spillerRotasjonsPunkt, new Vector2(SpillerScale, SpillerScale), SpriteEffects.None, 0f);
            

            spriteBatch.End(); //End drawing stuff
        }

        //Fra stack overflow
        //http://stackoverflow.com/questions/14894796/per-pixel-collision-could-do-with-some-general-tips
        public Boolean Collision(Vector2 position)
        {
            int index = (int)position.Y * _collisionMap.Width + (int)position.X;

            if (index < 0 || index >= _collisionMapData.Length) //Out of bounds  
                return true;

            if (_collisionMapData[index] == Color.Black)
                return true;

            return false;
        }


       
    }
}
