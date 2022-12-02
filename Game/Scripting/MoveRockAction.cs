using Unit06.Game.Casting;
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
            Rock rock = (Rock)cast.GetFirstActor(Constants.ROCK_GROUP);
            Body body = rock.GetBody();
            Point position = body.GetPosition();
            Point velocity = body.GetVelocity();
            position = position.Add(velocity);
            body.SetPosition(position);
        }
    }
}