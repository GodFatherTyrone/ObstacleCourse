using System.Collections.Generic;
using Unit06.Game.Casting;
using Unit06.Game.Services;


namespace Unit06.Game.Scripting
{
    public class CheckOverAction : Action
    {
        public CheckOverAction()
        {
        }

        public void Execute(Cast cast, Script script, ActionCallback callback)
        {
            List<Actor> rock = cast.GetActors(Constants.ROCK_GROUP);
            if (rock.Count == 0)
            {
                Stats stats = (Stats)cast.GetFirstActor(Constants.STATS_GROUP);
                stats.AddLevel();
                callback.OnNext(Constants.NEXT_LEVEL);
            }
        }
    }
}