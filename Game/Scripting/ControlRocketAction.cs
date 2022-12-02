using Unit06.Game.Casting;
using Unit06.Game.Services;


namespace Unit06.Game.Scripting
{
    public class ControlRocketAction : Action
    {
        private KeyboardService _keyboardService;

        public ControlRocketAction(KeyboardService keyboardService)
        {
            this._keyboardService = keyboardService;
        }

        public void Execute(Cast cast, Script script, ActionCallback callback)
        {
            Rocket rocket = (Rocket)cast.GetFirstActor(Constants.ROCKET_GROUP);
            if (_keyboardService.IsKeyDown(Constants.LEFT))
            {
                rocket.SwingLeft();
            }
            else if (_keyboardService.IsKeyDown(Constants.RIGHT))
            {
                rocket.SwingRight();
            }
            else if (_keyboardService.IsKeyDown(Constants.UP))
            {
                rocket.SwingUp();
            }
            else if (_keyboardService.IsKeyDown(Constants.DOWN))
            {
                rocket.SwingDown();
            }
            else
            {
                rocket.StopMoving();
            }
        }
    }
}