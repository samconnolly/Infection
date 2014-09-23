using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace GameJam
{
    public class GameModule : ModuleBase
    {
        private Virus _virus;
        private Virus _virus2;

        private EnemyGroup testEnemies;
        //private AntiViralNodule _antiViralNodule;

        //private Proliferate _proliferate;
        //private DoubleUp _doubleUp;
        //private Reproduce _reproduce;
        
        private List<SpriteBase> _cells;
        private List<SpriteBase> _items;
        private List<SpriteBase> _deadList;
        private List<SpriteBase> _addList;

        private SpriteFont font;
        private SpriteFont font2;
        private SpriteFont font3;
        
        private bool _isPlayingMusic = false;
        private bool _isMuted = false;
        private Song beneath;
        private Background _background;

        private bool _isPaused = false;
        private Texture2D _pauseTexture;

        private SpawnII spawn2;
        private int spawnTimer = 0;
        private int level = 1;
        private int wave = 1;
        private bool spawning = false;
        //private bool health = false;
        List<SpriteBase> add = new List<SpriteBase> { };
        List<SpriteBase> addItem = new List<SpriteBase> { };
        List<SpriteBase> usedItems = new List<SpriteBase> { };

        private int itemTimer = 0;
        private int itemTime = 20000;
        private int itemMax = 30000;
        private int itemMin = 10000;
        private Random rand = new Random();

        private bool kill = false;

        // pause menu
        private int selected = 0;
        private int tree = 0;

        public GameModule(Game game)
            : base(game)
        {
            _cells = new List<SpriteBase>();
            _items = new List<SpriteBase>();
            _deadList = new List<SpriteBase>();
            DeathHelper.KillCell = _deadList;
        }

        #region ModuleBase Overrides

        public override bool IsMouseVisible
        {
            get { return false; }
        }

        internal override void Initialize()
        {
            ScoreHelper.Score = 0;
            _cells = new List<SpriteBase>();
            _items = new List<SpriteBase>();
            _addList = new List<SpriteBase>();
            _deadList = new List<SpriteBase>();
        }

        internal override void LoadContent(SpriteBatch batch)
        {
            //Load the Background set refrence in SpriteHelper.
            _background = new Background();
            _background.LoadContent(batch, this.Game);
            //SpriteHelper.CurrentBackground = _background;

            //Pause Texture
            _pauseTexture = this.Game.Content.Load<Texture2D>("Pause Menu copy");

            //Load virus Testure.
            Texture2D virusTexture = this.Game.Content.Load<Texture2D>("player");
            Texture2D eyeTexture = this.Game.Content.Load<Texture2D>("eyes");
            Texture2D virusTexture2 = this.Game.Content.Load<Texture2D>("virusp2");
            Texture2D viruslingTexture = this.Game.Content.Load<Texture2D>("nanites");
            Texture2D laserTexture = this.Game.Content.Load<Texture2D>("laser");

            
            //// mega awesome powerups
            Texture2D powerupTexture = this.Game.Content.Load<Texture2D>("powerups");
            Texture2D specialTexture = this.Game.Content.Load<Texture2D>("specials");

            _virus = new Virus(virusTexture, viruslingTexture,eyeTexture,laserTexture,specialTexture, new Vector2(380,320));

            _virus2 = new Virus(virusTexture2, viruslingTexture, eyeTexture, laserTexture, specialTexture, new Vector2(880, 120), 2);
            
            Texture2D gruntTexture = this.Game.Content.Load<Texture2D>("grunt");
            Texture2D chargerTexture = this.Game.Content.Load<Texture2D>("charger");
            Texture2D sleeperTexture = this.Game.Content.Load<Texture2D>("sleeper");
            Texture2D artilleryTexture = this.Game.Content.Load<Texture2D>("artillery");
            Texture2D turretTexture = this.Game.Content.Load<Texture2D>("turret");

            Texture2D bombTexture = this.Game.Content.Load<Texture2D>("bomb");
            Texture2D crossTexture = this.Game.Content.Load<Texture2D>("cross");
            Texture2D missileTexture = this.Game.Content.Load<Texture2D>("missile");

            Texture2D spawnTexture = this.Game.Content.Load<Texture2D>("whitebloodspawn");

            //// AntiViral Nodule
            //Texture2D antiViralNoduleTexture = this.Game.Content.Load<Texture2D>("boss");
            //_antiViralNodule = new AntiViralNodule(antiViralNoduleTexture, new Vector2(750, 650));
            //_cells.Add(_antiViralNodule);


            //_proliferate = new Proliferate(proliferateTexture, new Vector2(30,30));
            //_doubleUp = new DoubleUp(doubleTexture, new Vector2(450,450));
            //_reproduce = new Reproduce(reproduceTexture, new Vector2(30,400));
            //_cells.Add(_proliferate);
            //_cells.Add(_doubleUp);
            //_cells.Add(_reproduce);
            
            testEnemies = new EnemyGroup(turretTexture, spawnTexture, new Vector2(300, 300), 3, 1, 3, new Vector2(6, 5),bombTexture,crossTexture);

            //_cells.Add(testEnemies);


            spawn2 = new SpawnII(gruntTexture,chargerTexture,
                                        sleeperTexture,turretTexture,artilleryTexture,
                                            missileTexture, crossTexture, bombTexture, spawnTexture,
                                                powerupTexture, specialTexture);
                      
            // initial powerup(s)
            //_items.Add(new PowerUpBase(powerupTexture, specialTexture, new Vector2(400, 400), 2));
            //_items.Add(new PowerUpBase(powerupTexture, specialTexture, new Vector2(500, 300), 8));
            //_items.Add(new PowerUpBase(powerupTexture, specialTexture, new Vector2(100, 500), 3));
            //_items.Add(new PowerUpBase(powerupTexture, specialTexture, new Vector2(200, 500), 4));
            //_items.Add(new PowerUpBase(powerupTexture, specialTexture, new Vector2(300, 500), 7));
            //_items.Add(new PowerUpBase(powerupTexture, specialTexture, new Vector2(400, 500), 10));
            //_items.Add(new PowerUpBase(powerupTexture, specialTexture, new Vector2(500, 500), 15));
            //_items.Add(new PowerUpBase(powerupTexture, specialTexture, new Vector2(600, 500), 16));
            //_items.Add(new PowerUpBase(powerupTexture, specialTexture, new Vector2(700, 500), 17));
            //_items.Add(new PowerUpBase(powerupTexture, specialTexture, new Vector2(800, 500), 18));
            //_items.Add(new PowerUpBase(powerupTexture, specialTexture, new Vector2(900, 500), 19));
            //_items.Add(new PowerUpBase(powerupTexture, specialTexture, new Vector2(100, 600), 20));
            //_items.Add(new PowerUpBase(powerupTexture, specialTexture, new Vector2(200, 600), 21));

            //Load atmospheric music.
            beneath = this.Game.Content.Load<Song>("Beneath");

            //Load Sound effects.
            SoundEffectPlayer.LoadContent(this.Game);

            // load fonts
            font = this.Game.Content.Load<SpriteFont>("font");
            font2 = this.Game.Content.Load<SpriteFont>("font2");
            font3 = this.Game.Content.Load<SpriteFont>("font3");

            FontHelper.Fonts.Add(font);
            FontHelper.Fonts.Add(font2);
            FontHelper.Fonts.Add(font3);

            if (ScoreHelper.Hardcore == false)
            {
                ScoreHelper.Lives = 3;
            }
        }

        internal override void UnloadContent()
        {

        }

        internal override void Update(GameTime gameTime, SpriteBatch batch)
        {
            // spawning

            CellsHelper.Cells = _cells;
            CellsHelper.AddCells = _addList;
            
            // antidote
            if (CellsHelper.Antidote == true)
            {
                _cells = new List<SpriteBase> { };
                CellsHelper.Antidote = false;
            }
            
            if (_cells.Count <= 0 && kill == false)
            {
                spawning = true;
            }

            if (spawning == true)
            {
                spawnTimer += gameTime.ElapsedGameTime.Milliseconds;

                add.Clear();

                // spawn red blood cells
                //if (spawnTimer >= 1000 && health == false)
                //{
                //    health = true;
                //    //add = spawn.SpawnRed(1);
                //    add = spawn2.SpawnRed(1);
                
                //}

                // spawn enemies
                if (spawnTimer >= 2000)
                {
                    spawnTimer = 0;
                    //health = false;
                    spawning = false;
                    wave += 1;

                    if (wave > 5)
                    {
                        wave = 1;
                        level += 1;
                    }

                    //add = spawn.SpawnEnemies(level);
                    add = spawn2.SpawnEnemies(level);
                }
                
                // add to cell list
                if (add.Count() > 0)
                {
                    foreach (SpriteBase sprite in add)
                    {
                        _cells.Add(sprite);
                    }
                }               
            }

            // spawn power-ups 

            itemTimer += gameTime.ElapsedGameTime.Milliseconds;

            if (itemTimer >= itemTime)
            {
                addItem = spawn2.SpawnPowerUps();
                itemTime = rand.Next(itemMin, itemMax);
                itemTimer = 0;
            }
            // cheat!!!
            if (InputHelper.WasButtonPressed(Keys.P))
            {
                addItem = spawn2.SpawnPowerUps();
            }

            //Play music if not playing already.
            if (!_isPlayingMusic)
            {
                MediaPlayer.Play(beneath);
                MediaPlayer.IsRepeating = true;
                _isPlayingMusic = true;
            }


            //Mute music
            if (InputHelper.WasButtonPressed(Keys.M))
            {
                if (_isMuted)
                {
                    MediaPlayer.Resume();
                    _isMuted = false;
                }
                else
                {
                    MediaPlayer.Pause();
                    _isMuted = true;
                }
            }

             // Destroy Everything!
            if (InputHelper.WasButtonPressed(Keys.K))
            {
                _cells = new List<SpriteBase> { };

                if (kill == false)
                {
                    kill = true;
                }

                else
                {
                    kill = false;
                }
            }
               

            //Update accordingly
            if (_isPaused)
            {
                // key assignments
                if (InputHelper.ForceKeys == true)
                {
                    InputHelper.Keys = 1;
                }
                else if (InputHelper.CurrentGamePadStatePlayer1.IsConnected == false && InputHelper.CurrentGamePadStatePlayer2.IsConnected == false)
                {
                    InputHelper.Keys = 1;
                }
                else if (InputHelper.CurrentGamePadStatePlayer1.IsConnected == false && InputHelper.CurrentGamePadStatePlayer2.IsConnected == true)
                {
                    InputHelper.Keys = 1;
                }
                else if (InputHelper.CurrentGamePadStatePlayer1.IsConnected == true && InputHelper.CurrentGamePadStatePlayer2.IsConnected == false)
                {
                    InputHelper.Keys = 2;
                }
                else
                {
                    InputHelper.Keys = 0;
                }

                // menu

                if (InputHelper.WasButtonPressed(Keys.Escape) || InputHelper.WasPadButtonPressedP1(Buttons.B))
                {
                    GameStateManager.CurrentGameState = GameState.MainMenu;
                    GameStateManager.HasChanged = true;
                }
                if (InputHelper.WasButtonPressed(Keys.R) || InputHelper.WasPadButtonPressedP1(Buttons.A))
                {
                    _isPaused = false;
                }
                if (InputHelper.WasButtonPressed(Keys.Down) | InputHelper.WasPadThumbstickDownP1())
                {
                    selected += 1;
                }
                else if (InputHelper.WasButtonPressed(Keys.Up) | InputHelper.WasPadThumbstickUpP1())
                {
                    selected -= 1;
                }
                if (selected > 5)
                {
                    selected = 0;
                }
                else if (selected < 0)
                {
                    selected = 5;
                }

                if (InputHelper.WasButtonPressed(Keys.Enter) | InputHelper.WasButtonPressed(Keys.Space) | InputHelper.WasPadButtonPressedP1(Buttons.A))
                {
                    if (tree == 0)
                    {
                        if (selected == 0)
                        {
                            _isPaused = false;
                        }
                        if (selected == 1)
                        {
                            ViewPortHelper.ToggleFullscreen();
                        }
                        if (selected == 2)
                        {
                            if (_isMuted)
                            {
                                MediaPlayer.Resume();
                                _isMuted = false;
                            }
                            else
                            {
                                MediaPlayer.Pause();
                                _isMuted = true;
                            }
                        }
                        if (selected == 3)
                        {
                            tree = 1;
                            selected = 0;
                        }
                        if (selected == 4)
                        {
                            GameStateManager.CurrentGameState = GameState.InGame;
                            GameStateManager.HasChanged = true;
                            _isPaused = false;
                        }
                        if (selected == 5)
                        {
                            GameStateManager.CurrentGameState = GameState.MainMenu;
                            GameStateManager.HasChanged = true;
                            _isPaused = false;
                        }
                    }

                    else if (tree == 1)
                    {
                        if (selected == 0)
                        {
                            // controls!
                        }
                        else if (selected == 1)
                        {
                            if (InputHelper.ForceKeys == false)
                            {
                                InputHelper.ForceKeys = true;
                            }
                            else
                            {
                                InputHelper.ForceKeys = false;
                            }
                        }
                        else
                        {
                            tree = 0;
                            selected = 0;
                        }
                    }
                }
            }
            else
            {
                //Update The virus sprite.
                _virus.Update(gameTime, batch);

                if (InputHelper.Players == 2)
                {
                    _virus2.Update(gameTime, batch);
                }

                //Update all cells.
                foreach (SpriteBase sprite in _cells)
                {
                    sprite.Update(gameTime, batch);
                }

                //Update all items.
                foreach (SpriteBase sprite in _items)
                {
                    sprite.Update(gameTime, batch);
                }

                // add new cells

                _addList = CellsHelper.AddCells;

                foreach (SpriteBase sprite in _addList)
                {
                    _cells.Add(sprite);
                }

                _addList = new List<SpriteBase> { };

                // get rid of dead cells

                _deadList = DeathHelper.KillCell;

                foreach (SpriteBase sprite in _deadList)
                {
                    _cells.Remove(sprite);
                }

                _deadList = new List<SpriteBase> { };

                // add new powerups
                
                foreach (SpriteBase sprite in addItem)
                {
                    _items.Add(sprite);
                }

                addItem = new List<SpriteBase> { };

                // get rid of used powerups

                usedItems = DeathHelper.UsedItems;

                foreach (SpriteBase sprite in usedItems)
                {
                    _items.Remove(sprite);
                }

                usedItems = new List<SpriteBase> { };
            }

            //Check to see if we are paused.
            if (InputHelper.WasButtonPressed(Keys.Escape) || InputHelper.WasPadButtonPressedP1(Buttons.Start))
            {
                _isPaused = true;
            }


        }

        internal override void Draw(GameTime gameTime, SpriteBatch batch)
        {
            if (_isPaused)
            {
                List<Color>  colours = new List<Color> { Color.White, Color.White, Color.White, Color.White, Color.White, Color.White };
                colours[selected] = Color.Black;

                if (tree == 0)
                {
                    batch.DrawString(font, "Resume", new Vector2(200, 300), colours[0], 0.0f, Vector2.Zero, 1.0f, SpriteEffects.None, 0.0f);
                    batch.DrawString(font, "Toggle Fullscreen", new Vector2(200, 350), colours[1], 0.0f, Vector2.Zero, 1.0f, SpriteEffects.None, 0.0f);
                    batch.DrawString(font, "Mute", new Vector2(200, 400), colours[2], 0.0f, Vector2.Zero, 1.0f, SpriteEffects.None, 0.0f);
                    batch.DrawString(font, "Controls", new Vector2(200, 450), colours[3], 0.0f, Vector2.Zero, 1.0f, SpriteEffects.None, 0.0f);
                    batch.DrawString(font, "Restart", new Vector2(200, 500), colours[4], 0.0f, Vector2.Zero, 1.0f, SpriteEffects.None, 0.0f);
                    batch.DrawString(font, "Exit", new Vector2(200, 550), colours[5], 0.0f, Vector2.Zero, 1.0f, SpriteEffects.None, 0.0f);
                }
                else if (tree == 1)
                {

                    batch.DrawString(font, "Show Controls", new Vector2(200, 300), colours[0], 0.0f, Vector2.Zero, 1.0f, SpriteEffects.None, 0.0f);
                    batch.DrawString(font, "Toggle keys/pad", new Vector2(200, 350), colours[1], 0.0f, Vector2.Zero, 1.0f, SpriteEffects.None, 0.0f);
                    batch.DrawString(font, "Back", new Vector2(200, 400), colours[2], 0.0f, Vector2.Zero, 1.0f, SpriteEffects.None, 0.0f);

                    batch.DrawString(font, "Pad 1 Connected: " + InputHelper.CurrentGamePadStatePlayer1.IsConnected.ToString(), new Vector2(200, 450), Color.White, 0.0f, Vector2.Zero, 1.0f, SpriteEffects.None, 0.0f);
                    batch.DrawString(font, "Pad 2 Connected: " + InputHelper.CurrentGamePadStatePlayer2.IsConnected.ToString(), new Vector2(200, 500), Color.White, 0.0f, Vector2.Zero, 1.0f, SpriteEffects.None, 0.0f);

                    string p1 = "pad";
                    string p2 = "pad";
                    if (InputHelper.Keys == 1)
                    {
                        p1 = "keys";
                    }
                    else if (InputHelper.Keys == 2)
                    {
                        p2 = "keys";
                    }

                    batch.DrawString(font, "P1: " + p1.ToString(), new Vector2(200, 550), Color.White, 0.0f, Vector2.Zero, 1.0f, SpriteEffects.None, 0.0f);
                    batch.DrawString(font, "P2: " + p2.ToString(), new Vector2(200, 600), Color.White, 0.0f, Vector2.Zero, 1.0f, SpriteEffects.None, 0.0f);
                }
                batch.Draw(_pauseTexture, new Vector2(0, 0),null, Color.White,0,Vector2.Zero,1.0f,SpriteEffects.None,0.5f);
            }

            else
            {
                //Draw the Virus.
                _virus.Draw(gameTime, batch, 0.2f);

                if (InputHelper.Players == 2)
                {
                    _virus2.Draw(gameTime, batch, 0.2f);
                }

                float cellLayer = 0;

                //Draw all cells.
                foreach (SpriteBase sprite in _cells)
                {
                    sprite.Draw(gameTime, batch, 0.5f + cellLayer);
                    cellLayer += 0.0001f;
                }

                //Draw all items.
                foreach (SpriteBase sprite in _items)
                {
                    sprite.Draw(gameTime, batch, 0.5f + cellLayer);
                    cellLayer += 0.0001f;
                }

                //Draw the background.
                _background.Draw(gameTime, batch);

                // Draw score
                batch.DrawString(font, "Level: " + level.ToString() + " Wave:" + wave.ToString(), new Vector2(400, 20), Color.White);
                batch.DrawString(font, "Score: " + ScoreHelper.Score.ToString(), new Vector2(520, 60), Color.White);
                if (ScoreHelper.Hardcore == false)
                {
                    batch.DrawString(font, "Lives: " + ScoreHelper.Lives.ToString(), new Vector2(520, 100), Color.White);
                }

                // circles

                batch.DrawString(font3, "1: Radius 1: " + VirusHelper.Radius1.ToString(), new Vector2(20, 20), Color.White);
                batch.DrawString(font3, "2: Radius 2: " + VirusHelper.Radius2.ToString(), new Vector2(20, 40), Color.White);
                batch.DrawString(font3, "3: Radius 3: " + VirusHelper.Radius3.ToString(), new Vector2(20, 60), Color.White);

                batch.DrawString(font3, "4: fractional Slowing: " + VirusHelper.InnerSlow.ToString(), new Vector2(20, 80), Color.White);
                //batch.DrawString(font3, "5: Outer Slow: " + VirusHelper.OuterSlow.ToString(), new Vector2(20, 100), Color.White);

                batch.DrawString(font3, "6: reverse slowing: " + VirusHelper.InnerAccn.ToString(), new Vector2(20, 120), Color.White);
                batch.DrawString(font3, "7: inward accn: " + VirusHelper.OuterAccn.ToString(), new Vector2(20, 140), Color.White);
                batch.DrawString(font3, "8: repel Accn: " + VirusHelper.OuterOuterAccn.ToString(), new Vector2(20, 160), Color.White);

                batch.DrawString(font3, "9: strong inward accn: " + VirusHelper.OuterOuterOuterAccn.ToString(), new Vector2(20, 180), Color.White);

                //batch.DrawString(font3, "0: V Outer Slow: " + VirusHelper.OuterOuterSlow.ToString(), new Vector2(20, 200), Color.White);

                batch.DrawString(font3, "Enter: Active Powerup: " + VirusHelper.Virus.activePowerup.ToString(), new Vector2(20, 220), Color.White);
            }
        }

        #endregion
    }
}
