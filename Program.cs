using Unit06.Game.Casting;
using Unit06.Game.Directing;
using Unit06.Game.Scripting;
using Unit06.Game.Services;

namespace ObstacleCourse
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            IServiceFactory serviceFactory = new RaylibServiceFactory();
            Scene scene = new Scene();

            SceneLoader menuSceneLoader = new MenuSceneLoader(serviceFactory);
            menuSceneLoader.Load(scene);

            Director director = new Director(serviceFactory);
            director.Direct(scene);
        }
    }
}