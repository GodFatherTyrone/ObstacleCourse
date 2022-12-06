using Unit06.Game.Casting;
using Unit06.Game.Services;


namespace Unit06.Game.Scripting
{
    public class CollideBordersAction : Action
    {
        private AudioService _audioService;
        private PhysicsService _physicsService;
        
        public CollideBordersAction(PhysicsService physicsService, AudioService audioService)
        {
            this._physicsService = physicsService;
            this._audioService = audioService;
        }

        public void Execute(Cast cast, Script script, ActionCallback callback)
        {
            Bullet ball = (Bullet)cast.GetFirstActor(Constants.BULLET_GROUP);
            Body body = ball.GetBody();
            Point position = body.GetPosition();
            int x = position.GetX();
            int y = position.GetY();
            
            Sound bounceSound = new Sound(Constants.BOUNCE_SOUND);
            Sound overSound = new Sound(Constants.OVER_SOUND);

            if (x < Constants.FIELD_LEFT)
            {
                ball.BounceX();
                _audioService.PlaySound(bounceSound);
            }
            else if (x >= Constants.FIELD_RIGHT - Constants.BULLET_WIDTH)
            {
                ball.BounceX();
                _audioService.PlaySound(bounceSound);
            }

            if (y < Constants.FIELD_TOP)
            {
                ball.BounceY();
                _audioService.PlaySound(bounceSound);
            }
            else if (y >= Constants.FIELD_BOTTOM - Constants.BULLET_WIDTH)
            {
                ball.BounceY();
                _audioService.PlaySound(bounceSound);
                
                // if (stats.GetLives() > 0)
                // {
                //     callback.OnNext(Constants.TRY_AGAIN);
            }  
        }
    }
}