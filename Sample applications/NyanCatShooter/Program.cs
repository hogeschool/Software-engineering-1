using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace MainApplication
{
  class Game : Microsoft.Xna.Framework.Game
  {
    SpriteBatch spriteBatch;
    GraphicsDeviceManager graphics;
    AsteroidGame.GameState gameState;

    public Game()
    {
      graphics = new GraphicsDeviceManager(this);
      Content.RootDirectory = "Content";
    }

    protected override void LoadContent()
    {
      spriteBatch = new SpriteBatch(GraphicsDevice);
      gameState = AsteroidGame.initialState();
      base.LoadContent();
    }

    protected override void Update(GameTime gameTime)
    {
      gameState = AsteroidGame.updateState(
        Keyboard.GetState(), Mouse.GetState(), 
        (float)gameTime.ElapsedGameTime.TotalSeconds, 
        gameState);
      base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
      GraphicsDevice.Clear(Color.CornflowerBlue);

      spriteBatch.Begin();
      foreach (var drawable in AsteroidGame.drawState(gameState))
      {
        spriteBatch.Draw(Content.Load<Texture2D>(drawable.Image),
          drawable.Position, Color.White);
      }
      spriteBatch.End();

      base.Draw(gameTime);
    }
  }

  class Program
  {
    static void Main(string[] args)
    {
      using (var game = new Game())
      {
        game.Run();
      }
    }
  }
}
