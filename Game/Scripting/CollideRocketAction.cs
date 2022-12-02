using Unit06.Game.Casting;
using Unit06.Game.Services;


namespace Unit06.Game.Scripting
{
    public class CollideRocketAction : Action
    {
        private AudioService _audioService;
        private PhysicsService _physicsService;
        
        public CollideRocketAction(PhysicsService physicsService, AudioService audioService)
        {
            this._physicsService = physicsService;
            this._audioService = audioService;
        }

        public void Execute(Cast cast, Script script, ActionCallback callback)
        {
            Bullet bullet = (Bullet)cast.GetFirstActor(Constants.BULLET_GROUP);
            Rocket rocket = (Rocket)cast.GetFirstActor(Constants.ROCKET_GROUP);
            Body bulletBody = bullet.GetBody();
            Body rocketBody = rocket.GetBody();
            Sound overSound = new Sound(Constants.OVER_SOUND);

            if (_physicsService.HasCollided(rocketBody, bulletBody))
            {
                bullet.BounceY();
                Sound sound = new Sound(Constants.BOUNCE_SOUND);
                _audioService.PlaySound(sound);
            }
            
            
        }
    }
}