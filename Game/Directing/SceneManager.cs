using System;
using System.Collections.Generic;
using System.IO;
using Unit06.Game.Casting;
using Unit06.Game.Scripting;
using Unit06.Game.Services;


namespace Unit06.Game.Directing
{
    public class SceneManager
    {
        public static AudioService AudioService = new RaylibAudioService();
        public static KeyboardService KeyboardService = new RaylibKeyboardService();
        public static MouseService MouseService = new RaylibMouseService();
        public static PhysicsService PhysicsService = new RaylibPhysicsService();
        public static VideoService VideoService = new RaylibVideoService(Constants.GAME_NAME,
            Constants.SCREEN_WIDTH, Constants.SCREEN_HEIGHT, Constants.BLACK);

        public SceneManager()
        {
        }

        public void PrepareScene(string scene, Cast cast, Script script)
        {
            if (scene == Constants.NEW_GAME)
            {
                PrepareNewGame(cast, script);
            }
            else if (scene == Constants.NEXT_LEVEL)
            {
                PrepareNextLevel(cast, script);
            }
            else if (scene == Constants.TRY_AGAIN)
            {
                PrepareTryAgain(cast, script);
            }
            else if (scene == Constants.IN_PLAY)
            {
                PrepareInPlay(cast, script);
            }
            else if (scene == Constants.GAME_OVER)
            {
                PrepareGameOver(cast, script);
            }
        }

        private void PrepareNewGame(Cast cast, Script script)
        {
            AddStats(cast);
            AddLevel(cast);
            AddScore(cast);
            AddLives(cast);
            AddBullet(cast);
            AddRock(cast);
            AddRocket(cast);
            AddDialog(cast, Constants.ENTER_TO_START);

            script.ClearAllActions();
            AddInitActions(script);
            AddLoadActions(script);

            ChangeSceneAction a = new ChangeSceneAction(KeyboardService, Constants.NEXT_LEVEL);
            script.AddAction(Constants.INPUT, a);

            AddOutputActions(script);
            AddUnloadActions(script);
            AddReleaseActions(script);
        }

        private void ActivateBall(Cast cast)
        {
            Bullet bullet = (Bullet)cast.GetFirstActor(Constants.BULLET_GROUP);
            bullet.Release();
        }
        private void ActivateRock(Cast cast)
        { //sets new velocity to (hopefully) fall
            List<Actor> rocks = cast.GetActors(Constants.ROCK_GROUP);
            foreach (Actor actor in rocks)
            {
                Rock rock = (Rock)actor;
                rock.ChangeRockVelocity();
            }  
        }

        private void PrepareNextLevel(Cast cast, Script script)
        {
            AddBullet(cast);
            AddRock(cast);
            AddRocket(cast);
            AddDialog(cast, Constants.PREP_TO_LAUNCH);

            script.ClearAllActions();

            TimedChangeSceneAction ta = new TimedChangeSceneAction(Constants.IN_PLAY, 2, DateTime.Now);
            script.AddAction(Constants.INPUT, ta);

            AddOutputActions(script);

            PlaySoundAction sa = new PlaySoundAction(AudioService, Constants.WELCOME_SOUND);
            script.AddAction(Constants.OUTPUT, sa);
        }

        private void PrepareTryAgain(Cast cast, Script script)
        {
            AddBullet(cast);
            AddRocket(cast);
            AddDialog(cast, Constants.PREP_TO_LAUNCH);

            script.ClearAllActions();
            
            TimedChangeSceneAction ta = new TimedChangeSceneAction(Constants.IN_PLAY, 2, DateTime.Now);
            script.AddAction(Constants.INPUT, ta);
            
            AddUpdateActions(script);
            AddOutputActions(script);
        }

        private void PrepareInPlay(Cast cast, Script script)
        {
            ActivateBall(cast);
            ActivateRock(cast);
            cast.ClearActors(Constants.DIALOG_GROUP);

            script.ClearAllActions();

            ControlRocketAction action = new ControlRocketAction(KeyboardService);
            script.AddAction(Constants.INPUT, action);

            AddUpdateActions(script);    
            AddOutputActions(script);
        
        }

        private void PrepareGameOver(Cast cast, Script script)
        {
            AddBullet(cast);
            AddRocket(cast);
            AddDialog(cast, Constants.WAS_GOOD_GAME);

            script.ClearAllActions();

            TimedChangeSceneAction ta = new TimedChangeSceneAction(Constants.NEW_GAME, 5, DateTime.Now);
            script.AddAction(Constants.INPUT, ta);

            AddOutputActions(script);
        }

        // -----------------------------------------------------------------------------------------
        // casting methods
        // -----------------------------------------------------------------------------------------

        

        private void AddRock(Cast cast)
        {
            cast.ClearActors(Constants.ROCK_GROUP);

            Stats stats = (Stats)cast.GetFirstActor(Constants.STATS_GROUP);
            int level = stats.GetLevel() % Constants.BASE_LEVELS;
            string filename = string.Format(Constants.LEVEL_FILE, level);
            List<List<string>> rows = LoadLevel(filename);

            for (int r = 0; r < rows.Count; r++)
            {
                for (int c = 0; c < rows[r].Count; c++)
                {
                    int x = Constants.FIELD_LEFT + c * Constants.ROCK_WIDTH;
                    int y = Constants.FIELD_TOP + r * Constants.ROCK_HEIGHT;

                    string color = rows[r][c][0].ToString();
                    int frames = (int)Char.GetNumericValue(rows[r][c][1]);
                    int points = Constants.ROCK_POINTS;

                    Point position = new Point(x, y);
                    Point size = new Point(Constants.ROCK_WIDTH, Constants.ROCK_HEIGHT);
                    Point velocity = new Point(0, 5);
                    List<string> images = Constants.ROCK_IMAGES[color].GetRange(0, frames);

                    Body body = new Body(position, size, velocity);
                    Animation animation = new Animation(images, Constants.ROCK_RATE, 1);
                    
                    Rock rock = new Rock(body, animation, points, false);
                    cast.AddActor(Constants.ROCK_GROUP, rock);
                }
            }
        }

        public void AddBullet(Cast cast)
        {
            cast.ClearActors(Constants.BULLET_GROUP);
        
            int x = Constants.CENTER_X - Constants.BULLET_WIDTH / 2;
            int y = Constants.SCREEN_HEIGHT - Constants.ROCKET_HEIGHT - Constants.BULLET_HEIGHT;
        
            Point position = new Point(x, y);
            Point size = new Point(Constants.BULLET_WIDTH, Constants.BULLET_HEIGHT);
            Point velocity = new Point(0, 0);
        
            Body body = new Body(position, size, velocity);
            Image image = new Image(Constants.BULLET_IMAGE);
            Bullet ball = new Bullet(body, image, false);
        
            cast.AddActor(Constants.BULLET_GROUP, ball);
            cast.AddActor(Constants.BULLET_GROUP, ball);
        }

        private void AddDialog(Cast cast, string message)
        {
            cast.ClearActors(Constants.DIALOG_GROUP);

            Text text = new Text(message, Constants.FONT_FILE, Constants.FONT_SIZE, 
                Constants.ALIGN_CENTER, Constants.WHITE);
            Point position = new Point(Constants.CENTER_X, Constants.CENTER_Y);

            Label label = new Label(text, position);
            cast.AddActor(Constants.DIALOG_GROUP, label);   
        }

        private void AddLevel(Cast cast)
        {
            cast.ClearActors(Constants.LEVEL_GROUP);

            Text text = new Text(Constants.LEVEL_FORMAT, Constants.FONT_FILE, Constants.FONT_SIZE, 
                Constants.ALIGN_LEFT, Constants.WHITE);
            Point position = new Point(Constants.HUD_MARGIN, Constants.HUD_MARGIN);

            Label label = new Label(text, position);
            cast.AddActor(Constants.LEVEL_GROUP, label);
        }

        private void AddLives(Cast cast)
        {
            cast.ClearActors(Constants.LIVES_GROUP);

            Text text = new Text(Constants.LIVES_FORMAT, Constants.FONT_FILE, Constants.FONT_SIZE, 
                Constants.ALIGN_RIGHT, Constants.WHITE);
            Point position = new Point(Constants.SCREEN_WIDTH - Constants.HUD_MARGIN, 
                Constants.HUD_MARGIN);

            Label label = new Label(text, position);
            cast.AddActor(Constants.LIVES_GROUP, label);   
        }

        private void AddRocket(Cast cast)
        {
            cast.ClearActors(Constants.ROCKET_GROUP);
        
            int x = Constants.CENTER_X - Constants.ROCKET_WIDTH / 2;
            int y = Constants.SCREEN_HEIGHT - Constants.ROCKET_HEIGHT;
        
            Point position = new Point(x, y);
            Point size = new Point(Constants.ROCKET_WIDTH, Constants.ROCKET_HEIGHT);
            Point velocity = new Point(Constants.ROCK_VELOCITY, 0);
        
            Body body = new Body(position, size, velocity);
            Animation animation = new Animation(Constants.ROCKET_IMAGES, Constants.ROCKET_RATE, 0);
            Rocket racket = new Rocket(body, animation, false);
        
            cast.AddActor(Constants.ROCKET_GROUP, racket);
        }

        private void AddScore(Cast cast)
        {
            cast.ClearActors(Constants.SCORE_GROUP);

            Text text = new Text(Constants.SCORE_FORMAT, Constants.FONT_FILE, Constants.FONT_SIZE, 
                Constants.ALIGN_CENTER, Constants.WHITE);
            Point position = new Point(Constants.CENTER_X, Constants.HUD_MARGIN);
            
            Label label = new Label(text, position);
            cast.AddActor(Constants.SCORE_GROUP, label);   
        }

        private void AddStats(Cast cast)
        {
            cast.ClearActors(Constants.STATS_GROUP);
            Stats stats = new Stats();
            cast.AddActor(Constants.STATS_GROUP, stats);
        }

        private List<List<string>> LoadLevel(string filename)
        {
            List<List<string>> data = new List<List<string>>();
            using(StreamReader reader = new StreamReader(filename))
            {
                while (!reader.EndOfStream)
                {
                    string row = reader.ReadLine();
                    List<string> columns = new List<string>(row.Split(',', StringSplitOptions.TrimEntries));
                    data.Add(columns);
                }
            }
            return data;
        }

        // -----------------------------------------------------------------------------------------
        // scriptig methods
        // -----------------------------------------------------------------------------------------

        private void AddInitActions(Script script)
        {
            script.AddAction(Constants.INITIALIZE, new InitializeDevicesAction(AudioService, 
                VideoService));
        }

        private void AddLoadActions(Script script)
        {
            script.AddAction(Constants.LOAD, new LoadAssetsAction(AudioService, VideoService));
        }

        private void AddOutputActions(Script script)
        {
            script.AddAction(Constants.OUTPUT, new StartDrawingAction(VideoService));
            script.AddAction(Constants.OUTPUT, new DrawHudAction(VideoService));
            script.AddAction(Constants.OUTPUT, new DrawBulletAction(VideoService));
            script.AddAction(Constants.OUTPUT, new DrawRockAction(VideoService));
            script.AddAction(Constants.OUTPUT, new DrawRocketAction(VideoService));
            script.AddAction(Constants.OUTPUT, new DrawDialogAction(VideoService));
            script.AddAction(Constants.OUTPUT, new EndDrawingAction(VideoService));
        }

        private void AddUnloadActions(Script script)
        {
            script.AddAction(Constants.UNLOAD, new UnloadAssetsAction(AudioService, VideoService));
        }

        private void AddReleaseActions(Script script)
        {
            script.AddAction(Constants.RELEASE, new ReleaseDevicesAction(AudioService, 
                VideoService));
        }

        private void AddUpdateActions(Script script)
        {
            script.AddAction(Constants.UPDATE, new MoveBulletAction());
            script.AddAction(Constants.UPDATE, new MoveRocketAction());
            script.AddAction(Constants.UPDATE, new MoveRockAction());
            script.AddAction(Constants.UPDATE, new CollideBordersAction(PhysicsService, AudioService));
            script.AddAction(Constants.UPDATE, new CollideRockAction(PhysicsService, AudioService));
            script.AddAction(Constants.UPDATE, new CollideRocketAction(PhysicsService, AudioService));
            script.AddAction(Constants.UPDATE, new CheckOverAction());     
        }
    }
}
