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

        // private EnemyGroup testEnemies;
        
        private List<SpriteBase> _cells;
        private List<SpriteBase> _items;
        private List<SpriteBase> _deadList;
        private List<SpriteBase> _addList;
        private List<SpriteBase> _addItemList;
        private List<Virus> _virusList;
        private List<Virus> _deadPlayerList;

        private SpriteFont font;
        private SpriteFont font2;
        private SpriteFont font3;
        
        private bool _isPlayingMusic = false;
        private bool _isMuted = false;
        private Song beneath;
        private Song infectious;
        private Background _background;

        private bool _isPaused = false;
        private Texture2D _pauseTexture;
        private Texture2D _controller;

        private Texture2D heartTex;
        private Rectangle heartRect;

        private SpawnII spawn2;
        private int spawnTimer = 0;
        private int level = 0;
        private int wave = 0;
        private bool spawning = false;
        List<SpriteBase> add = new List<SpriteBase> { };
        List<SpriteBase> addItem = new List<SpriteBase> { };
        List<SpriteBase> usedItems = new List<SpriteBase> { };

        private Random rand = new Random();

        private bool kill = false;

        // pause menu
        private int selected = 0;
        private int tree = 0;
        private int max = 5;
        private bool mouseover = false;

        public GameModule(Game game)
            : base(game)
        {
            _cells = new List<SpriteBase>();
            _items = new List<SpriteBase>();
            _deadList = new List<SpriteBase>();
            DeathHelper.KillCell = _deadList;
        }

        #region ModuleBase Overrides
        
        internal override void Initialize()
        {
            CellsHelper.Colours = rand.Next(9);

            ScoreHelper.Score = 0;
            _cells = new List<SpriteBase>();
            _items = new List<SpriteBase>();
            _addList = new List<SpriteBase>();
            _addItemList = new List<SpriteBase>();
            _deadList = new List<SpriteBase>();
            _virusList = new List<Virus>();
            _deadPlayerList = new List<Virus>();
                        
            VirusHelper.Radius1 = 30.0f;
            VirusHelper.Radius2 = 65.0f;
            VirusHelper.Radius3 = 130.0f;

            VirusHelper.InnerSlow = 0.999f;
            VirusHelper.OuterSlow = 0.5f;
            VirusHelper.OuterOuterSlow = 1.0f;

            VirusHelper.InnerAccn = 3.5f;
            VirusHelper.OuterAccn = 5.0f;
            VirusHelper.OuterOuterAccn = 1.0f;
            VirusHelper.OuterOuterOuterAccn = 10.0f;

            VirusHelper.Repel = 0.001f;

            VirusHelper.Rotation = 1;
            VirusHelper.RotationSpeed = 0.05f;

            VirusHelper.Radius1P2 = VirusHelper.Radius1;
            VirusHelper.Radius2P2 = VirusHelper.Radius2;
            VirusHelper.Radius3P2 = VirusHelper.Radius3;

            VirusHelper.InnerSlowP2 = VirusHelper.InnerSlow;
            VirusHelper.OuterSlowP2 = VirusHelper.OuterSlow;
            VirusHelper.OuterOuterSlowP2 = VirusHelper.OuterOuterSlow;

            VirusHelper.InnerAccnP2 = VirusHelper.InnerAccn;
            VirusHelper.OuterAccnP2 = VirusHelper.OuterAccn;
            VirusHelper.OuterOuterAccnP2 = VirusHelper.OuterOuterAccn;
            VirusHelper.OuterOuterOuterAccnP2 = VirusHelper.OuterOuterOuterAccn;

            VirusHelper.RepelP2 = VirusHelper.Repel;

            VirusHelper.RotationP2 = 1;
            VirusHelper.RotationSpeedP2 = 0.05f;

            ScoreHelper.DeadPlayers = _deadPlayerList;
            ScoreHelper.LivePlayers = _virusList;

            IsMouseVisible = false;
        }

        internal override void LoadContent(SpriteBatch batch)
        {
            //Load the Background set refrence in SpriteHelper.
            _background = new Background();
            _background.LoadContent(batch, this.Game);

            //Pause Texture & menu
            _pauseTexture = this.Game.Content.Load<Texture2D>("Pause Menu copy");
            _controller = this.Game.Content.Load<Texture2D>("controller");

            //virus Textures
            Texture2D virusTexture = this.Game.Content.Load<Texture2D>("player");
            Texture2D eyeTexture = this.Game.Content.Load<Texture2D>("eyes");
            Texture2D virusTexture2 = this.Game.Content.Load<Texture2D>("player");
            Texture2D viruslingTexture = this.Game.Content.Load<Texture2D>("nanites");
            Texture2D laserTexture = this.Game.Content.Load<Texture2D>("laser");

            
            //// mega awesome powerups
            Texture2D powerupTexture = this.Game.Content.Load<Texture2D>("powerups");
            Texture2D specialTexture = this.Game.Content.Load<Texture2D>("specials");
            Texture2D powerupTextTex = this.Game.Content.Load<Texture2D>("powerupText");
            Texture2D invinceTexture = this.Game.Content.Load<Texture2D>("Invincible");

            heartTex = this.Game.Content.Load<Texture2D>("heart");
            heartRect = new Rectangle(0, 0, heartTex.Width, heartTex.Height);

            _virus = new Virus(virusTexture, viruslingTexture, eyeTexture, laserTexture, specialTexture, invinceTexture, new Vector2(380, 320));
            _virus2 = new Virus(virusTexture2, viruslingTexture, eyeTexture, laserTexture, specialTexture, invinceTexture, new Vector2(880, 120), 2);

            _virusList.Add(_virus);

            if (InputHelper.Players == 2)
            {
                _virusList.Add(_virus2);
            }

            Texture2D gruntTexture = this.Game.Content.Load<Texture2D>("grunt");
            Texture2D chargerTexture = this.Game.Content.Load<Texture2D>("charger");
            Texture2D sleeperTexture = this.Game.Content.Load<Texture2D>("sleeper");
            Texture2D artilleryTexture = this.Game.Content.Load<Texture2D>("artillery");
            Texture2D turretTexture = this.Game.Content.Load<Texture2D>("turret");

            Texture2D bombTexture = this.Game.Content.Load<Texture2D>("bomb");
            Texture2D crossTexture = this.Game.Content.Load<Texture2D>("cross");
            Texture2D missileTexture = this.Game.Content.Load<Texture2D>("missile");
            Texture2D circleTexture = this.Game.Content.Load<Texture2D>("circle_both");

            Texture2D spawnTexture = this.Game.Content.Load<Texture2D>("spawn");
                        
            //testEnemies = new EnemyGroup(turretTexture, spawnTexture, new Vector2(300, 300), 3, 1, 5, new Vector2(6, 5),missileTexture,crossTexture);
            //_cells.Add(testEnemies);

            spawn2 = new SpawnII(gruntTexture,chargerTexture,
                                        sleeperTexture,turretTexture,artilleryTexture,
                                            missileTexture, crossTexture, bombTexture, spawnTexture,
                                                powerupTexture,powerupTextTex, specialTexture, circleTexture);
            CellsHelper.Spawning = spawn2;          

            // initial powerup(s)
            if (InputHelper.Players == 2)
            {
                _items.Add(new PowerUpBase(powerupTexture, specialTexture, powerupTextTex, new Vector2(400, 400), 2));
            }
            _items.Add(new PowerUpBase(powerupTexture, specialTexture, powerupTextTex, new Vector2(400, 600), 2));
            
            //Load atmospheric music.
            beneath = this.Game.Content.Load<Song>("Beneath");
            infectious = this.Game.Content.Load<Song>("infectiousgaze");
            
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
                ScoreHelper.LivesP2 = 3;
            }

            // check if music is playing already or not
            if (ViewPortHelper.CurrentSong == beneath || ViewPortHelper.CurrentSong == infectious)
            {
                _isPlayingMusic = true;
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
            CellsHelper.AddItems = addItem;
            
            // antidote
            if (CellsHelper.Antidote == true)
            {
                _cells = new List<SpriteBase> { };
                CellsHelper.Antidote = false;
            }
            
            if (_cells.Count <= 0 && kill == false && spawning == false)
            {
                spawning = true;
                wave += 1;
                if (wave > 5 | level == 0)
                {
                    wave = 1;
                    level += 1;
                }
            }

            if (spawning == true)
            {
                spawnTimer += gameTime.ElapsedGameTime.Milliseconds;

                add.Clear();
                
                // spawn enemies
                if (spawnTimer >= 2000)
                {
                    spawnTimer = 0;
                    spawning = false;
                    

                    if (wave == 1 | level == 0)
                    {
                        PowerUpBase powerup = spawn2.SpawnItem(new Vector2(540, 360)); // spawn item at end of each level
                        addItem.Add(powerup);
                    }

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

            // ------------ cheats!!! ---------------------------
            // spawn powerups
            //if (InputHelper.WasButtonPressed(Keys.P))
            //{
            //    addItem = spawn2.SpawnPowerUp(1,2);
            //}
            // Destroy Everything!
            //if (InputHelper.WasButtonPressed(Keys.K))
            //{
            //    _cells = new List<SpriteBase> { };

            //    if (kill == false)
            //    {
            //        kill = true;
            //    }

            //    else
            //    {
            //        kill = false;
            //    }
            //}

            //Play music if not playing already.
            if (!_isPlayingMusic)
            {
                int thisSong = rand.Next(2);
                if (thisSong == 0)
                {
                    MediaPlayer.Play(beneath);
                    ViewPortHelper.CurrentSong = beneath;
                }
                else
                {
                    MediaPlayer.Play(infectious);
                    ViewPortHelper.CurrentSong = infectious;
                }

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
                    if (tree == 0)
                    {
                        _isPaused = false;
                        InputHelper.Game.IsMouseVisible = false;
                    }
                    else if (tree != 3)
                    {
                        tree = 0;
                        selected = 0;
                        max = 5;
                    }
                    else
                    {
                        tree = 2;
                        selected = 0;
                        max = 2;
                    }
                }

                if (InputHelper.WasButtonPressed(Keys.Down) | InputHelper.WasPadThumbstickDownP1() | InputHelper.WasPadButtonPressedP1(Buttons.DPadDown))
                {
                    selected += 1;
                }
                else if (InputHelper.WasButtonPressed(Keys.Up) | InputHelper.WasPadThumbstickUpP1() | InputHelper.WasPadButtonPressedP1(Buttons.DPadUp))
                {
                    selected -= 1;
                }
                else if (InputHelper.CurrentMouseState.X / ViewPortHelper.XScale > 200 && InputHelper.CurrentMouseState.X / ViewPortHelper.XScale < 500)
                {
                    for (int i = 0; i < max + 1; i++)
                    {
                        if (InputHelper.CurrentMouseState.Y != InputHelper.PreviousMouseState.Y &&
                                InputHelper.CurrentMouseState.Y / ViewPortHelper.YScale >= 300 + i * 50 && InputHelper.CurrentMouseState.Y / ViewPortHelper.YScale < 300 + (i + 1) * 50)
                        {
                            selected = i;
                            mouseover = true;
                        }
                    }
                }

                if (selected > max)
                {
                    selected = 0;
                }
                else if (selected < 0)
                {
                    selected = max;
                }

                if (InputHelper.WasButtonPressed(Keys.Enter) | InputHelper.WasButtonPressed(Keys.Space) | 
                        InputHelper.WasPadButtonPressedP1(Buttons.A) | (InputHelper.WasMouseClicked() && mouseover == true))
                {
                    if (tree == 0)
                    {
                        if (selected == 0)
                        {
                            _isPaused = false;
                            InputHelper.Game.IsMouseVisible = false;
                        }
                        if (selected == 1)
                        {
                            ViewPortHelper.ToggleFullscreen();
                        }
                        if (selected == 2)
                        {
                            tree = 1;
                            selected = 0;
                            max = 2;
                        }
                        if (selected == 3)
                        {
                            tree = 2;
                            selected = 0;
                            max = 2;
                        }
                        if (selected == 4)
                        {
                            GameStateManager.CurrentGameState = GameState.InGame;
                            GameStateManager.HasChanged = true;
                            _isPaused = false;
                            InputHelper.Game.IsMouseVisible = false;
                        }
                        if (selected == 5)
                        {
                            GameStateManager.CurrentGameState = GameState.MainMenu;
                            GameStateManager.HasChanged = true;
                           }
                    }
                    else if (tree == 1)
                    {
                         if (selected == 0)
                        {
                            float newVolume = MediaPlayer.Volume + 0.1f;
                            if (newVolume > 1.09999f)
                            {
                                newVolume = 0;
                            }
                            else if (newVolume > 1.0f)
                            {
                                newVolume = 1.0f;
                            }
                            MediaPlayer.Volume = newVolume;
                        }
                        else if (selected == 1)
                        {
                            float newVolume = SoundEffectPlayer.Volume + 0.1f;
                            if (newVolume > 1.09999f)
                            {
                                newVolume = 0;
                            }
                            else if (newVolume > 1.0f)
                            {
                                newVolume = 1.0f;
                            }
                            SoundEffectPlayer.AdjustVolume(newVolume);
                        }
                        else
                        {
                            tree = 0;
                            selected = 0;
                            max = 5;
                        }
                    }
                    else if (tree == 2)
                    {
                        if (selected == 0)
                        {
                            tree = 3;
                            max = 0;
                            selected = 0;
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
                            max = 5;
                        }
                    }
                    else if (tree == 3)
                    {
                        tree = 2;
                        selected = 0;
                        max = 2;
                    }
                }
            }
            else
            {

                //Check to see if we are paused.
                if (InputHelper.WasButtonPressed(Keys.Escape) || InputHelper.WasPadButtonPressedP1(Buttons.Start))
                {
                    _isPaused = true;
                    IsMouseVisible = true;
                    InputHelper.Game.IsMouseVisible = IsMouseVisible;
                }

                //Update The virus sprite.
                foreach (Virus virus in _virusList)
                {
                    virus.Update(gameTime, batch);
                }
                
                _deadPlayerList = ScoreHelper.DeadPlayers;

                if (_deadPlayerList.Count > 0)
                {
                    foreach (Virus virus in _deadPlayerList)
                    {
                        _virusList.Remove(virus);
                    }
                }

                _deadPlayerList = new List<Virus> { };
                ScoreHelper.LivePlayers = _virusList;

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

                addItem = CellsHelper.AddItems;

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
        }

        internal override void Draw(GameTime gameTime, SpriteBatch batch)
        {
            if (_isPaused)
            {
                List<Color>  colours = new List<Color> { Color.White, Color.White, Color.White, Color.White, Color.White, Color.White };
                colours[selected] = Color.Black;

                batch.DrawString(font2, "Paused", new Vector2(200, 150), Color.White, 0.0f, Vector2.Zero, 1.0f, SpriteEffects.None, 0.0f);

                if (tree == 0)
                {
                    batch.DrawString(font, "Resume", new Vector2(200, 300), colours[0], 0.0f, Vector2.Zero, 1.0f, SpriteEffects.None, 0.0f);
                    batch.DrawString(font, "Toggle Fullscreen", new Vector2(200, 350), colours[1], 0.0f, Vector2.Zero, 1.0f, SpriteEffects.None, 0.0f);
                    batch.DrawString(font, "Sound Options", new Vector2(200, 400), colours[2], 0.0f, Vector2.Zero, 1.0f, SpriteEffects.None, 0.0f);
                    batch.DrawString(font, "Controls", new Vector2(200, 450), colours[3], 0.0f, Vector2.Zero, 1.0f, SpriteEffects.None, 0.0f);
                    batch.DrawString(font, "Restart", new Vector2(200, 500), colours[4], 0.0f, Vector2.Zero, 1.0f, SpriteEffects.None, 0.0f);
                    batch.DrawString(font, "Exit", new Vector2(200, 550), colours[5], 0.0f, Vector2.Zero, 1.0f, SpriteEffects.None, 0.0f);
                }
                else if (tree == 1)
                {
                    batch.DrawString(font, "Music Volume -" + ((int)(MediaPlayer.Volume * 100)).ToString(), 
                                                new Vector2(200, 300), colours[0], 0.0f, Vector2.Zero, 1.0f, SpriteEffects.None, 0.0f);
                    batch.DrawString(font, "SFX Volume -" + ((int)(SoundEffectPlayer.Volume * 100)).ToString(), 
                                                new Vector2(200, 350), colours[1], 0.0f, Vector2.Zero, 1.0f, SpriteEffects.None, 0.0f);
                    batch.DrawString(font, "Back", new Vector2(200, 400), colours[2], 0.0f, Vector2.Zero, 1.0f, SpriteEffects.None, 0.0f);
                }
                else if (tree == 2)
                {

                    batch.DrawString(font, "Show Controls", new Vector2(200, 300), colours[0], 0.0f, Vector2.Zero, 1.0f, SpriteEffects.None, 0.0f);
                    batch.DrawString(font, "Toggle keys/pad", new Vector2(200, 350), colours[1], 0.0f, Vector2.Zero, 1.0f, SpriteEffects.None, 0.0f);
                    batch.DrawString(font, "Back", new Vector2(200, 400), colours[2], 0.0f, Vector2.Zero, 1.0f, SpriteEffects.None, 0.0f);

                    batch.DrawString(font, "Pad 1 Connected - " + InputHelper.CurrentGamePadStatePlayer1.IsConnected.ToString(), 
                                                new Vector2(200, 450), Color.White, 0.0f, Vector2.Zero, 1.0f, SpriteEffects.None, 0.0f);
                    batch.DrawString(font, "Pad 2 Connected - " + InputHelper.CurrentGamePadStatePlayer2.IsConnected.ToString(), 
                                                new Vector2(200, 500), Color.White, 0.0f, Vector2.Zero, 1.0f, SpriteEffects.None, 0.0f);

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

                    batch.DrawString(font, "P1 - " + p1.ToString(), new Vector2(200, 550), Color.White, 0.0f, Vector2.Zero, 1.0f, SpriteEffects.None, 0.0f);
                    batch.DrawString(font, "P2 - " + p2.ToString(), new Vector2(200, 600), Color.White, 0.0f, Vector2.Zero, 1.0f, SpriteEffects.None, 0.0f);
                }
                else if (tree == 3)
                {                   
                    batch.DrawString(font, "Back", new Vector2(450, 600), colours[0], 0.0f, Vector2.Zero, 1.0f, SpriteEffects.None, 0.0f);
                    batch.Draw(_controller, new Vector2(200, 200), null, Color.White, 0, Vector2.Zero, 1.0f, SpriteEffects.None, 0.49f);
                }

                batch.Draw(_pauseTexture, new Vector2(0, 0),null, Color.White,0,Vector2.Zero,1.0f,SpriteEffects.None,0.5f);
            }

            else
            {
                //Draw the Virus.
                foreach (Virus virus in _virusList)
                {
                    virus.Draw(gameTime, batch, 0.2f);
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
                
                _background.Draw(gameTime, batch,level);

                // Draw score
                batch.DrawString(font, "Level - " + level.ToString() + " Wave -" + wave.ToString(), new Vector2(400, 20), Color.White);
                batch.DrawString(font, "Score - " + ScoreHelper.Score.ToString(), new Vector2(450, 60), Color.White);

                if (ScoreHelper.Hardcore == false)
                {
                    if (InputHelper.Players == 2)
                    {
                        if (VirusHelper.Virus.dead == false)
                        {
                            for (int i = 0; i < ScoreHelper.Lives; i++)
                            {
                                batch.Draw(heartTex, new Vector2(120 + i * 60, 40), heartRect, Color.White, 0.0f, Vector2.Zero, 0.5f, SpriteEffects.None, 0.49f);
                            }
                        }
                        if (VirusHelper.VirusP2.dead == false)
                        {
                            for (int j = 0; j < ScoreHelper.LivesP2; j++)
                            {
                                batch.Draw(heartTex, new Vector2(910 - j * 60, 40), heartRect, Color.White, 0.0f, Vector2.Zero, 0.5f, SpriteEffects.None, 0.49f);
                            }
                        }
                    }
                    else
                    {
                        for (int i = 0; i < ScoreHelper.Lives; i++)
                        {
                            batch.Draw(heartTex, new Vector2(120 + i*60, 40), heartRect, Color.White, 0.0f, Vector2.Zero, 0.5f, SpriteEffects.None, 0.49f);
                        }
                    }
                }

                // level text
                if (spawning == true && wave == 1)
                {
                    batch.DrawString(font2, "Level " + (level).ToString(), new Vector2(400, 400), Color.White);
                }

                // circles - testing
                if (VirusHelper.Virus.circles == true)
                {
                    batch.DrawString(font3, "1 - Radius 1 - " + VirusHelper.Radius1.ToString(), new Vector2(20, 20), Color.White);
                    batch.DrawString(font3, "2 - Radius 2 - " + VirusHelper.Radius2.ToString(), new Vector2(20, 40), Color.White);
                    batch.DrawString(font3, "3 - Radius 3 - " + VirusHelper.Radius3.ToString(), new Vector2(20, 60), Color.White);

                    batch.DrawString(font3, "4 - fractional Slowing - " + VirusHelper.InnerSlow.ToString(), new Vector2(20, 80), Color.White);

                    batch.DrawString(font3, "6 - reverse slowing - " + VirusHelper.InnerAccn.ToString(), new Vector2(20, 120), Color.White);
                    batch.DrawString(font3, "7 - inward accn - " + VirusHelper.OuterAccn.ToString(), new Vector2(20, 140), Color.White);
                    batch.DrawString(font3, "8 - repel Accn - " + VirusHelper.OuterOuterAccn.ToString(), new Vector2(20, 160), Color.White);

                    batch.DrawString(font3, "9 - strong inward accn - " + VirusHelper.OuterOuterOuterAccn.ToString(), new Vector2(20, 180), Color.White);

                    batch.DrawString(font3, "0 - V Outer Slow - " + VirusHelper.OuterOuterSlow.ToString(), new Vector2(20, 200), Color.White);

                    batch.DrawString(font3, "Enter - Active Powerup - " + VirusHelper.Virus.activePowerup.ToString(), new Vector2(20, 220), Color.White);
                }
            }
        }

        #endregion
    }
}
