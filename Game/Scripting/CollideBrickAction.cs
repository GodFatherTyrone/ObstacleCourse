using System.Collections.Generic;
using Unit06.Game.Casting;
using Unit06.Game.Services;


namespace Unit06.Game.Scripting
{
    public class CollideRockAction : Action
    {
        private AudioService _audioService;
        private PhysicsService _physicsService;
        
        public CollideRockAction(PhysicsService physicsService, AudioService audioService)
        {
            this._physicsService = physicsService;
            this._audioService = audioService;
        }

        public void Execute(Cast cast, Script script, ActionCallback callback)
        {
            Bullet bullet = (Bullet)cast.GetFirstActor(Constants.BULLET_GROUP);
            Rocket rocket = (Rocket)cast.GetFirstActor(Constants.ROCKET_GROUP);
            List<Actor> rock = cast.GetActors(Constants.ROCK_GROUP);
            Stats stats = (Stats)cast.GetFirstActor(Constants.STATS_GROUP);
            
            foreach (Actor actor in rock)
            {
                Rock Rock = (Rock)actor;
                Body RockBody = Rock.GetBody();
                Body bulletBody = bullet.GetBody();
                Body RocketBody = rocket.GetBody();
                Sound overSound = new Sound(Constants.OVER_SOUND);
                
                Point position = RockBody.GetPosition();
                int x = position.GetX();
                int y = position.GetY();

                if (_physicsService.HasCollided(RockBody, bulletBody))
                {
                    bullet.BounceY();
                    Sound sound = new Sound(Constants.BOUNCE_SOUND);
                    _audioService.PlaySound(sound);
                    int points = Rock.GetPoints();
                    stats.AddPoints(points);
                    cast.RemoveActor(Constants.ROCK_GROUP, Rock);
                    // THIS REMOVES THE BULLET, BUT ALSO CRASHES THE GAME
                    ///cast.RemoveActor(Constants.BULLET_GROUP, bullet);
                }

                if (_physicsService.HasCollided(RocketBody, RockBody))
                {
                    stats.RemoveLife();

                    if (stats.GetLives() > 0)
                    {
                        callback.OnNext(Constants.TRY_AGAIN);
                    }
                    else
                    {
                        callback.OnNext(Constants.GAME_OVER);
                        _audioService.PlaySound(overSound);
                    }
                }
                // Remove Rock when it touches the bottom
                if (y >= Constants.FIELD_BOTTOM - Constants.ROCK_HEIGHT)
                {
                    
                    cast.RemoveActor(Constants.ROCK_GROUP, Rock);
                }




                //Body bottomScreen = new Body(Constants.SCREEN_HEIGHT, 0, 0)
                //if (_physicsService.HasCollided(RockBody, bottomScreen) {
//
                //}
            }
        }
    }
}