using Unit06.Game.Casting;
using System.Collections.Generic;
namespace Unit06.Game.Scripting

////Allow Rocks to fall
{
    public class MoveRockAction : Action
    {
        public MoveRockAction()
        {
        }

        public void Execute(Cast cast, Script script, ActionCallback callback)
        {
            List<Actor> rocks = cast.GetActors(Constants.ROCK_GROUP);
            foreach (Actor actor in rocks)
            {
                Rock rock = (Rock)actor;
                Body body = rock.GetBody();
                Point position = body.GetPosition();
                Point velocity = body.GetVelocity();
                position = position.Add(velocity);
                body.SetPosition(position);
            }
        }
    }
}



