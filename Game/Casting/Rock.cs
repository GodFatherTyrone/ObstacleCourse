using System;
using System.Collections.Generic;

namespace Unit06.Game.Casting
{
    // Brick to Rock
    /// <summary>
    /// A thing that participates in the game.
    /// </summary>
    public class Rock : Actor
    {
        private static Random _random = new Random();

        private Body _body;
        private Animation _animation;
        private int _points;

        /// <summary>
        /// Constructs a new instance of Actor.
        /// </summary>
        public Rock(Body body, Animation animation, int points, bool debug) : base(debug)
        {
            this._body = body;
            this._animation = animation;
            this._points = points;
        }

        /// <summary>
        /// Gets the animation.
        /// </summary>
        /// <returns>The animation.</returns>
        public Animation GetAnimation()
        {
            return _animation;
        }

        /// <summary>
        /// Gets the body.
        /// </summary>
        /// <returns>The body.</returns>
        public Body GetBody()
        {
            return _body;
        }

        /// <summary>
        /// Gets the points.
        /// </summary>
        /// <returns>The points.</returns>
        public int GetPoints()
        {
            return _points;
        }
        public void ChangeRockVelocity()
        {
            Point velocity = _body.GetVelocity();
            List<int> velocities = new List<int> {0, Constants.ROCK_VELOCITY};
            int index = _random.Next(velocities.Count);
            double vx = velocities[index];
            double vy = Constants.BULLET_VELOCITY;
            Point newVelocity = new Point((int)vx, (int)vy);
            _body.SetVelocity(newVelocity);
        }
        
    }
}