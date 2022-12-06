Using Unit06.Game.Casting;

namespace Unit06.Game.Scripting;

 public class ShootBullet : Action
 {

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
        }







 }
