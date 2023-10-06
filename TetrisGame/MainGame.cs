using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using TetrisGame.Debugger;
using TetrisGame.Systems;

namespace TetrisGame
{
    public class MainGame : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private SpriteFont _font;
        private MouseStateExtended _mouseState;
        private DebugTextRenderer _debugTextRenderer;
        private FpsUps _fpsUps;
        private Board _board;
        private KeyboardExtended _keyboardExtended;

        public MainGame()
        {
            _graphics = new GraphicsDeviceManager(this);
            GraphicsAdapter.Adapters.Count.ToString();
            _graphics.PreferredBackBufferWidth = 1920;
            _graphics.PreferredBackBufferHeight = 1000;
            //_graphics.PreferredBackBufferWidth =  GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
            //_graphics.PreferredBackBufferHeight =  GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;

            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            base.Initialize();
            Window.Position = new Point(GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width, 200);

        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            _font = Content.Load<SpriteFont>("courier");
            _mouseState = new MouseStateExtended(Mouse.GetState(), Mouse.GetState());
            _fpsUps = new FpsUps();
            _debugTextRenderer = new DebugTextRenderer(_font, Window);
            Globals.TextRenderer = _debugTextRenderer;
            _debugTextRenderer.Register(_fpsUps);
            _debugTextRenderer.Register(_mouseState);
            _keyboardExtended = new KeyboardExtended();
            _board = new Board(Window, _graphics.GraphicsDevice, _keyboardExtended);
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            GameTimer.Timers.ForEach(t => t.Update(gameTime));
            _keyboardExtended.Update(gameTime);
            _mouseState.CurrentState = Mouse.GetState();
            _board.Update(gameTime);
            _fpsUps.Update(gameTime);
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            // TODO: Add your drawing code here
            _spriteBatch.Begin();
            _fpsUps.Draw(_spriteBatch, gameTime);
            _debugTextRenderer.Draw(_spriteBatch, gameTime);
            _board.Draw(_spriteBatch, gameTime);
            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}