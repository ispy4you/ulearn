namespace rocket_bot
{
    public class Rocket
    {
        public Rocket(Vector location, Vector velocity, double direction, int time = 0, int takenCheckpointsCount = 0)
        {
            Location = location;
            Velocity = velocity;
            Direction = direction;
            Time = time;
            TakenCheckpointsCount = takenCheckpointsCount;

        }

        public readonly Vector Location;
        public readonly Vector Velocity;
        public readonly double Direction;
        public readonly int Time;
        public readonly int TakenCheckpointsCount;

        public Rocket IncreaseCheckpoints()
        {
            return new Rocket(Location, Velocity, Direction, Time, TakenCheckpointsCount + 1);
        }
    }
}