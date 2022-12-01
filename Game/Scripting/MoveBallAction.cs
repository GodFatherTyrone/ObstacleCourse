using Unit06.Game.Casting;
namespace Unit06.Game.Scripting
{
    public class MoveBulletAction : Action
    {
        public MoveBulletAction()
        {
        }

        public void Execute(Cast cast, Script script, ActionCallback callback)
        {
            Bullet bullet = (Bullet)cast.GetFirstActor(Constants.BULLET_GROUP);
            Body body = bullet.GetBody();
            Point position = body.GetPosition();
            Point velocity = body.GetVelocity();
            position = position.Add(velocity);
            body.SetPosition(position);
        }
    }
}