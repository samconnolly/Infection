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
        private WhiteBloodCellGroup _whiteBloodCellGroup;
        private RedBloodCellGroup _redBloodCellGroup;
        private RedBloodCellGroup _redBloodCellGroup2;
        private GreenBloodCellGroup _greenCellGroup;
        private BlueBloodCellGroup _blueCellGroup;
        private PurpleBloodCellGroup _purpleCellGroup;
        private OrangeBloodCellGroup _orangeCellGroup;
        

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

        private Spawn spawn;
        private SpawnII spawn2;
        private int spawnTimer = 0;
        private int level = 0;
        private bool spawning = false;
        private bool health = false;
        List<SpriteBase> add = new List<SpriteBase> { };

        private bool kill = false;

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
            Texture2D virusTexture = this.Game.Content.Load<Texture2D>("virus");
            Texture2D virusTexture2 = this.Game.Content.Load<Texture2D>("virusp2");
            Texture2D viruslingTexture = this.Game.Content.Load<Texture2D>("virusling");
            _virus = new Virus(virusTexture, viruslingTexture,new Vector2(180,120));
            _virus2 = new Virus(virusTexture2, viruslingTexture,new Vector2(880,120), 2);
            
            // Initial cells...
            Texture2D whiteBloodCellTexture = this.Game.Content.Load<Texture2D>("whiteblood");
            Texture2D whiteBloodCellTextureHit = this.Game.Content.Load<Texture2D>("whitebloodhit");
            Texture2D whiteBloodCellTextureSpawn = this.Game.Content.Load<Texture2D>("whitebloodspawn");
            _whiteBloodCellGroup = new WhiteBloodCellGroup(whiteBloodCellTexture,whiteBloodCellTextureHit,whiteBloodCellTextureSpawn, new Vector2(500, 500),3);
            _cells.Add(_whiteBloodCellGroup);

            Texture2D redBloodCellTexture = this.Game.Content.Load<Texture2D>("redblood");
            _redBloodCellGroup = new RedBloodCellGroup(redBloodCellTexture, new Vector2(200, 200),3);
            _redBloodCellGroup2 = new RedBloodCellGroup(redBloodCellTexture, new Vector2(700, 200),3);
            _cells.Add(_redBloodCellGroup);

            if (InputHelper.Players == 2)
            {
                _cells.Add(_redBloodCellGroup2);
            }

            Texture2D greenCellTexture = this.Game.Content.Load<Texture2D>("greencell");
            _greenCellGroup = new GreenBloodCellGroup(greenCellTexture, whiteBloodCellTextureHit, whiteBloodCellTextureSpawn, new Vector2(700, 650), 5);
            _cells.Add(_greenCellGroup);

            Texture2D blueCellTexture = this.Game.Content.Load<Texture2D>("bluecell");
            Texture2D missileTexture = this.Game.Content.Load<Texture2D>("yball");
            _blueCellGroup = new BlueBloodCellGroup(blueCellTexture, whiteBloodCellTextureHit, whiteBloodCellTextureSpawn, missileTexture, new Vector2(130, 600), 1);
            _cells.Add(_blueCellGroup);

            Texture2D purpleCellTexture = this.Game.Content.Load<Texture2D>("purplecell");
            _purpleCellGroup = new PurpleBloodCellGroup(purpleCellTexture, whiteBloodCellTextureHit, whiteBloodCellTextureSpawn, new Vector2(300, 300), 1);
            _cells.Add(_purpleCellGroup);

            Texture2D orangeCellTexture = this.Game.Content.Load<Texture2D>("orangecell");
            Texture2D bombTexture = this.Game.Content.Load<Texture2D>("bomb");
            Texture2D crossTexture = this.Game.Content.Load<Texture2D>("cross");
            _orangeCellGroup = new OrangeBloodCellGroup(orangeCellTexture, whiteBloodCellTextureHit, whiteBloodCellTextureSpawn,crossTexture, bombTexture, new Vector2(400, 400), 1);
            _cells.Add(_orangeCellGroup);

            //// AntiViral Nodule
            //Texture2D antiViralNoduleTexture = this.Game.Content.Load<Texture2D>("boss");
            //_antiViralNodule = new AntiViralNodule(antiViralNoduleTexture, new Vector2(750, 650));
            //_cells.Add(_antiViralNodule);

            //// mega awesome powerups
            Texture2D proliferateTexture = this.Game.Content.Load<Texture2D>("proliferate");
            Texture2D doubleTexture = this.Game.Content.Load<Texture2D>("double");
            Texture2D reproduceTexture = this.Game.Content.Load<Texture2D>("reproduce");
            //_proliferate = new Proliferate(proliferateTexture, new Vector2(30,30));
            //_doubleUp = new DoubleUp(doubleTexture, new Vector2(450,450));
            //_reproduce = new Reproduce(reproduceTexture, new Vector2(30,400));
            //_cells.Add(_proliferate);
            //_cells.Add(_doubleUp);
            //_cells.Add(_reproduce);

            spawn = new Spawn(whiteBloodCellTexture, whiteBloodCellTextureHit,whiteBloodCellTextureSpawn, 
                                redBloodCellTexture, proliferateTexture, doubleTexture, reproduceTexture,
                                greenCellTexture, whiteBloodCellTextureHit, whiteBloodCellTextureSpawn, 
                                blueCellTexture, whiteBloodCellTextureHit, whiteBloodCellTextureSpawn, missileTexture, 
                                purpleCellTexture, whiteBloodCellTextureHit, whiteBloodCellTextureSpawn,
                                orangeCellTexture,whiteBloodCellTextureHit,whiteBloodCellTextureSpawn,crossTexture,bombTexture);


            _cells = new List<SpriteBase> { };

            spawn2 = new SpawnII(whiteBloodCellTexture, whiteBloodCellTextureHit, whiteBloodCellTextureSpawn,
                                redBloodCellTexture, proliferateTexture, doubleTexture, reproduceTexture,
                                greenCellTexture, whiteBloodCellTextureHit, whiteBloodCellTextureSpawn,
                                blueCellTexture, whiteBloodCellTextureHit, whiteBloodCellTextureSpawn, missileTexture,
                                purpleCellTexture, whiteBloodCellTextureHit, whiteBloodCellTextureSpawn,
                                orangeCellTexture, whiteBloodCellTextureHit, whiteBloodCellTextureSpawn, crossTexture, bombTexture);
                      

            //Load atmospheric music.
            beneath = this.Game.Content.Load<Song>("Beneath");

            //Load Sound effects.
            SoundEffectPlayer.LoadContent(this.Game);

            // load fonts
            font = this.Game.Content.Load<SpriteFont>("font");
            font2 = this.Game.Content.Load<SpriteFont>("font2");
            font3 = this.Game.Content.Load<SpriteFont>("font3");
        
        }

        internal override void UnloadContent()
        {

        }

        internal override void Update(GameTime gameTime, SpriteBatch batch)
        {
            // spawning

            CellsHelper.Cells = _cells;
            CellsHelper.AddCells = _addList;
            
            if (_cells.Count <= 0 && kill == false)
            {
                spawning = true;
            }

            if (spawning == true)
            {
                spawnTimer += gameTime.ElapsedGameTime.Milliseconds;

                add.Clear();

                if (spawnTimer >= 1000 && health == false)
                {
                    health = true;
                    //add = spawn.SpawnRed(1);
                    add = spawn2.SpawnRed(1);
                
                }

                else if (spawnTimer >= 2000)
                {
                    spawnTimer = 0;
                    health = false;
                    spawning = false;
                    level += 1;
                    //add = spawn.SpawnEnemies(level);
                    add = spawn2.SpawnEnemies(level);
                }

                if (add.Count() > 0)
                {
                    foreach (SpriteBase sprite in add)
                    {
                        _cells.Add(sprite);
                    }
                }
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
                if (InputHelper.WasButtonPressed(Keys.Escape) || InputHelper.WasPadButtonPressedP1(Buttons.B))
                {
                    GameStateManager.CurrentGameState = GameState.MainMenu;
                    GameStateManager.HasChanged = true;
                }
                if (InputHelper.WasButtonPressed(Keys.R) || InputHelper.WasPadButtonPressedP1(Buttons.A))
                {
                    _isPaused = false;
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
            }

            //Check to see if we are paused.
            if (InputHelper.WasButtonPressed(Keys.Escape) || InputHelper.WasPadButtonPressedP1(Buttons.Start))
            {
                _isPaused = true;
            }
        }

        internal override void Draw(GameTime gameTime, SpriteBatch batch)
        {
            if(_isPaused)
            {
                batch.Draw(_pauseTexture, new Vector2(0, 0), Color.White);
            }

            //Draw the Virus.
            _virus.Draw(gameTime, batch,0.2f);

            if (InputHelper.Players == 2)
            {
                _virus2.Draw(gameTime, batch, 0.2f);
            }

            float cellLayer = 0;

            //Draw all cells.
            foreach (SpriteBase sprite in _cells)
            {
                sprite.Draw(gameTime, batch,0.5f + cellLayer);
                cellLayer += 0.0001f;
            }

            //Draw the background.
            _background.Draw(gameTime, batch);

            // Draw score

            batch.DrawString(font ,ScoreHelper.Score.ToString(),new Vector2(520,40),Color.White);

            // circles

            batch.DrawString(font3,"1: Radius 1: " + VirusHelper.Radius1.ToString(), new Vector2(20, 40), Color.White);
            batch.DrawString(font3, "2: Radius 2: " + VirusHelper.Radius2.ToString(), new Vector2(20, 70), Color.White);
            batch.DrawString(font3, "3: Radius 3: " + VirusHelper.Radius3.ToString(), new Vector2(20, 100), Color.White);

            batch.DrawString(font3, "4: Inner Slow: " + VirusHelper.InnerSlow.ToString(), new Vector2(20, 130), Color.White);
            batch.DrawString(font3, "5: Outer Slow: " + VirusHelper.OuterSlow.ToString(), new Vector2(20, 160), Color.White);

            batch.DrawString(font3, "6: Inner Accn: " + VirusHelper.InnerAccn.ToString(), new Vector2(20, 190), Color.White);
            batch.DrawString(font3, "7: Outer Accn: " + VirusHelper.OuterAccn.ToString(), new Vector2(20, 220), Color.White);
            batch.DrawString(font3, "8: V Outer Accn: " + VirusHelper.OuterOuterAccn.ToString(), new Vector2(20, 250), Color.White);

            batch.DrawString(font3, "9: VV Outer Accn: " + VirusHelper.OuterOuterOuterAccn.ToString(), new Vector2(20, 280), Color.White);

            batch.DrawString(font3, "0: V Outer Slow: " + VirusHelper.OuterOuterSlow.ToString(), new Vector2(20, 310), Color.White);
        }

        #endregion
    }
}
