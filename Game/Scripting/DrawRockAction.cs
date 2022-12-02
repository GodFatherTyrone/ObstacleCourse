using System.Collections.Generic;
using Unit06.Game.Casting;
using Unit06.Game.Services;


namespace Unit06.Game.Scripting
{
    public class DrawRockAction : Action
    {
        private VideoService _videoService;
        
        public DrawRockAction(VideoService videoService)
        {
            this._videoService = videoService;
        }

        public void Execute(Cast cast, Script script, ActionCallback callback)
        {
            List<Actor> rocks = cast.GetActors(Constants.ROCK_GROUP);
            foreach (Actor actor in rocks)
            {
                Rock rock = (Rock)actor;
                Body body = rock.GetBody();

                if (rock.IsDebug())
                {
                    Rectangle rectangle = body.GetRectangle();
                    Point size = rectangle.GetSize();
                    Point pos = rectangle.GetPosition();
                    _videoService.DrawRectangle(size, pos, Constants.PURPLE, false);
                }

                Animation animation = rock.GetAnimation();
                Image image = animation.NextImage();
                Point position = body.GetPosition();
                _videoService.DrawImage(image, position);
            }
        }
    }
}