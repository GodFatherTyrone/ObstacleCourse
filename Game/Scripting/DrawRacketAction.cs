using Unit06.Game.Casting;
using Unit06.Game.Services;


namespace Unit06.Game.Scripting
{
    public class DrawRocketAction : Action
    {
        private VideoService _videoService;
        
        public DrawRocketAction(VideoService videoService)
        {
            this._videoService = videoService;
        }

        public void Execute(Cast cast, Script script, ActionCallback callback)
        {
            Rocket rocket = (Rocket)cast.GetFirstActor(Constants.ROCKET_GROUP);
            Body body = rocket.GetBody();

            if (rocket.IsDebug())
            {
                Rectangle rectangle = body.GetRectangle();
                Point size = rectangle.GetSize();
                Point pos = rectangle.GetPosition();
                _videoService.DrawRectangle(size, pos, Constants.PURPLE, false);
            }

            Animation animation = rocket.GetAnimation();
            Image image = animation.NextImage();
            Point position = body.GetPosition();
            _videoService.DrawImage(image, position);
        }
    }
}