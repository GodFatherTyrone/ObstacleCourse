using Unit06.Game.Casting;
using Unit06.Game.Services;


namespace Unit06.Game.Scripting
{
    public class DrawBulletAction : Action
    {
        private VideoService _videoService;
        
        public DrawBulletAction(VideoService videoService)
        {
            this._videoService = videoService;
        }

        public void Execute(Cast cast, Script script, ActionCallback callback)
        {
            Bullet bullet = (Bullet)cast.GetFirstActor(Constants.BULLET_GROUP);
            Body body = bullet.GetBody();

            if (bullet.IsDebug())
            {
                Rectangle rectangle = body.GetRectangle();
                Point size = rectangle.GetSize();
                Point pos = rectangle.GetPosition();
                _videoService.DrawRectangle(size, pos, Constants.PURPLE, false);
            }

            Image image = bullet.GetImage();
            Point position = body.GetPosition();
            _videoService.DrawImage(image, position);
        }
    }
}