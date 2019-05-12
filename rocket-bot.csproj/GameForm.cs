using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using System.Windows.Forms;

namespace rocket_bot
{
    public class GameForm : Form
    {
        private readonly Channel<Rocket> channel;
        private readonly Image rocket;
        private readonly Image checkpoint;
        private readonly Level level;
        private bool manualRewindInProgress;
        private string helpText;
        private readonly HashSet<Turn> manualControls = new HashSet<Turn>();
        private readonly TrackBar rewindTrackBar;
        private int skipTurns = 1;
        private bool paused;

        public GameForm(Level level, Channel<Rocket> channel)
        {
            WindowState = FormWindowState.Maximized;
            this.channel = channel;
            this.level = level;
            KeyPreview = true;
            rocket = Image.FromFile("images/rocket.png");
            checkpoint = Image.FromFile("images/flag.png");
            rewindTrackBar = new TrackBar
            {
                Dock = DockStyle.Bottom,
                Height = 10,
                TickFrequency = 10,
                LargeChange = 100,
                TabStop = false
            };
            rewindTrackBar.ValueChanged += (sender, e) => RewindTo(rewindTrackBar.Value);
            rewindTrackBar.MouseDown += (sender, e) => manualRewindInProgress = true;
            rewindTrackBar.MouseUp += (sender, e) => manualRewindInProgress = false;
            Controls.Add(rewindTrackBar);
            var playButtonsPanel = CreatePlayButtonsPanel();
            Controls.Add(playButtonsPanel);

            var timer = new Timer { Interval = 10 };
            timer.Tick += TimerTick;
            timer.Start();
        }

        private TableLayoutPanel CreatePlayButtonsPanel()
        {
            var table = new TableLayoutPanel
            {
                Dock = DockStyle.Bottom,
                TabIndex = 1,
                TabStop = false,
                Height = 40,
            };
            table.RowStyles.Clear();
            table.RowStyles.Add(new RowStyle(SizeType.Absolute, 31));
            table.ColumnStyles.Clear();
            table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 30));
            table.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize, 6));
            table.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize, 6));
            table.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize, 6));
            table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 30));
            var buttonSize = new Size(35, 30);
            var fastButton = new Button
            {
                Text = "►►",
                Size = buttonSize,
                TabStop = false

            };
            fastButton.Click += (sender, e) => SetPlaySpeed(5);
            table.Controls.Add(fastButton, 3, 0);
            var slowButton = new Button
            {
                Text = "►",
                Size = buttonSize,
                TabStop = false
            };
            slowButton.Click += (sender, e) => SetPlaySpeed(1);
            table.Controls.Add(slowButton, 2, 0);
            var pauseButton = new Button
            {
                Text = "❚❚",
                Size = buttonSize,
                TabStop = false
            };
            pauseButton.Click += (sender, e) => paused = true;
            table.Controls.Add(pauseButton, 1, 0);
            table.Controls.Add(new Panel(), 4, 0);
            return table;
        }

        private void SetPlaySpeed(int speed)
        {
            skipTurns = speed;
            paused = false;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            DoubleBuffered = true;
            helpText = "Use A, W and D to control rocket";
            Text = helpText;
        }

        private void TimerTick(object sender, EventArgs e)
        {
            if (level == null) return;
            rewindTrackBar.SetRange(0, level.MaxTicksCount);
            if (!manualRewindInProgress && !paused)
            {
                rewindTrackBar.Value = level.Rocket.Time;
                MoveRocket();
            }

            Text =
                $"{helpText}. Iteration # {level.Rocket.Time} Checkpoints taken: {level.Rocket.TakenCheckpointsCount}. Ticks precalculated: {channel.Count}";
            Invalidate();
            Update();
        }

        private void MoveRocket()
        {
            if (manualControls.Any())
            {
                var control = manualControls.First();
                for (var i = 0; i < skipTurns && !level.IsCompleted; ++i)
                {
                    level.Move(control);
                    channel[level.Rocket.Time] = level.Rocket;
                }
            }
            else
                RewindTo(level.Rocket.Time + (manualRewindInProgress ? 0 : skipTurns));
        }

        private void RewindTo(int time)
        {
            var prevRocket = channel[time] ?? channel.LastItem();
            if (prevRocket != null)
                level.Rocket = prevRocket;
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);
            HandleKey(e.KeyCode, true);
        }

        private void HandleKey(Keys e, bool down)
        {
            if (e == Keys.A || e == Keys.Left) SetManualControl(Turn.Left, down);
            if (e == Keys.D || e == Keys.Right) SetManualControl(Turn.Right, down);
            if (e == Keys.W || e == Keys.Up) SetManualControl(Turn.None, down);
            if (e == Keys.R) channel[0] = level.InitialRocket;
        }

        private void SetManualControl(Turn control, bool down)
        {
            if (down) manualControls.Add(control);
            else manualControls.Remove(control);
        }

        protected override void OnKeyUp(KeyEventArgs e)
        {
            base.OnKeyDown(e);
            HandleKey(e.KeyCode, false);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.FillRectangle(Brushes.Bisque, ClientRectangle);
            var image = new Bitmap(ClientRectangle.Width, ClientRectangle.Height, PixelFormat.Format32bppArgb);
            var g = Graphics.FromImage(image);
            DrawTo(g, ClientRectangle);
            e.Graphics.DrawImage(image, (ClientRectangle.Width - image.Width) / 2,
                (ClientRectangle.Height - image.Height) / 2);
        }

        private void DrawTo(Graphics g, Rectangle rect)
        {
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.FillRectangle(Brushes.Beige, rect);
            var trackBarHeight = 100;
            var aiCalculationBarRect = new Rectangle(rect.Left, rect.Height - trackBarHeight, rect.Width, trackBarHeight + 1);
            g.FillRectangle(Brushes.DimGray, aiCalculationBarRect);
            g.FillRectangle(Brushes.MediumSeaGreen,
                new Rectangle(aiCalculationBarRect.Location, new Size(aiCalculationBarRect.Width * channel.Count / level.MaxTicksCount, trackBarHeight)));

            if (level == null) return;

            g.TranslateTransform((rect.Width - level.SpaceSize.Width) / 2f,
                (rect.Height - level.SpaceSize.Height) / 2f);
            var matrix = g.Transform;

            for (var i = 0; i < level.Checkpoints.Length; ++i)
            {
                g.Transform = matrix;
                g.TranslateTransform((float)level.Checkpoints[i].X, (float)level.Checkpoints[i].Y);
                g.DrawImage(checkpoint, new Point(-checkpoint.Width / 2, -checkpoint.Height / 2));
                if (level.Rocket.TakenCheckpointsCount % level.Checkpoints.Length == i)
                    g.FillEllipse(Brushes.Gold, new Rectangle(-10, -20, 20, 20));
            }

            g.Transform = matrix;
            g.TranslateTransform((float)level.Rocket.Location.X, (float)level.Rocket.Location.Y);
            g.RotateTransform(90 + (float)(level.Rocket.Direction * 180 / Math.PI));
            g.DrawImage(rocket, new Point(-rocket.Width / 2, -rocket.Height / 2));
        }
    }
}