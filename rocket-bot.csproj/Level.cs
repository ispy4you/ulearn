using System.Drawing;

namespace rocket_bot
{
    public class Level
    {
        public Level(Rocket rocket, Vector[] checkpoints, Physics physics)
        {
            Rocket = rocket;
            InitialRocket = rocket;
            Checkpoints = checkpoints;
            this.physics = physics;
        }

        public readonly Size SpaceSize = new Size(800, 600);
        public readonly Rocket InitialRocket;
        private readonly Physics physics;
        public readonly Vector[] Checkpoints;
        public readonly int MaxTicksCount = 1000;
        public Rocket Rocket;

        public Vector GetNextRocketCheckpoint() => Checkpoints[Rocket.TakenCheckpointsCount % Checkpoints.Length];
        public Level Clone() => new Level(Rocket, Checkpoints, physics);
        public bool IsCompleted => Rocket.Time >= MaxTicksCount;

        public void Move(Turn turn)
        {
            if (IsCompleted) return;
            var nextCheckpoint = Rocket.TakenCheckpointsCount % Checkpoints.Length;
            if (nextCheckpoint != Checkpoints.Length && (Rocket.Location - Checkpoints[nextCheckpoint]).Length < 20)
            {
                Rocket = Rocket.IncreaseCheckpoints();
            }
            Rocket = physics.MoveRocket(Rocket, 1.0, turn, 0.5);
        }
    }
}