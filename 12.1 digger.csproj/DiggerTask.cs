using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Digger
{
    public class Terrain : ICreature
    {
        public CreatureCommand Act(int x, int y)
        {
            return new CreatureCommand()
            { DeltaX = 0, DeltaY = 0 };
        }

        public bool DeadInConflict(ICreature conflictedObject) => true;

        public int GetDrawingPriority() => 2;

        public string GetImageFileName() => "Terrain.png";
    }

    public class Player : ICreature
    {
        public static int X = 0, Y = 0, Px = 0, Py = 0;

        public CreatureCommand Act(int x, int y)
        {
            X = x; Y = y;
            KeyPressed();
            if (x + Px < 0 || y + Py < 0
                || x + Px >= Game.MapWidth || y + Py >= Game.MapHeight)
            {
                Px = 0; Py = 0;
            }
            if (Game.Map[x + Px, y + Py] != null)
            {
                if (Game.Map[x + Px, y + Py].ToString() == "Digger.Sack")
                {
                    Px = 0; Py = 0;
                }
            }
            return new CreatureCommand() { DeltaX = Px, DeltaY = Py };
        }

        private static void KeyPressed()
        {
            switch (Game.KeyPressed)
            {
                default:
                    Px = 0; Py = 0;
                    break;
                case Keys.Left:
                    Px = -1; Py = 0;
                    break;
                case Keys.Right:
                    Px = 1; Py = 0;
                    break;
                case Keys.Up:
                    Py = -1; Px = 0;
                    break;
                case Keys.Down:
                    Py = 1; Px = 0;
                    break;
            }
        }

        public bool DeadInConflict(ICreature conflict)
        {
            var objectL = conflict.ToString();
            if (objectL == "Digger.Gold")
                Game.Scores += 10;
            if (objectL == "Digger.Sack" ||
                objectL == "Digger.Monster")
            {
                return true;
            }
            return false;
        }

        public int GetDrawingPriority() => 1;

        public string GetImageFileName() => "Digger.png";
    }


    public class Sack : ICreature
    {
        private int count = 0;

        public CreatureCommand Act(int x, int y)
        {
            if (y < Game.MapHeight - 1)
            {
                var lowerObject = Game.Map[x, y + 1];
                if (lowerObject == null ||
                    (count > 0 && (lowerObject.ToString() == "Digger.Player" ||
                    lowerObject.ToString() == "Digger.Monster")))
                {
                    count++;
                    return new CreatureCommand() { DeltaX = 0, DeltaY = 1 };
                }
            }

            if (count > 1)
            {
                return new CreatureCommand() { DeltaX = 0, DeltaY = 0, TransformTo = new Gold() };
            }
            count = 0;
            return new CreatureCommand() { DeltaY = 0, DeltaX = 0 };
        }


        public bool DeadInConflict(ICreature conflictedObject) => false;

        public int GetDrawingPriority() => 5;

        public string GetImageFileName() => "Sack.png";
    }

    public class Gold : ICreature
    {
        public CreatureCommand Act(int x, int y)
        {
            return new CreatureCommand() { DeltaX = 0, DeltaY = 0 };
        }

        public bool DeadInConflict(ICreature conflict)
        {
            var obj = conflict.ToString();
            return obj == "Digger.Player" ||
            obj == "Digger.Monster";
        }

        public int GetDrawingPriority() => 3;

        public string GetImageFileName() => "Gold.png";
    }


    public class Monster : ICreature
    {
        public CreatureCommand Act(int x, int y)
        {
            int dx = 0; int dy = 0;
            if (Location)
                PlayerStay(x, y, ref dx, ref dy);
            else return new CreatureCommand() { DeltaX = 0, DeltaY = 0 };
            if (!(x + dx >= 0 && x + dx < Game.MapWidth && y + dy >= 0 && y + dy < Game.MapHeight))
                return new CreatureCommand() { DeltaX = 0, DeltaY = 0 };
            var map = Game.Map[x + dx, y + dy];
            if (map != null)
                if (map.ToString() == "Digger.Terrain" || map.ToString() == "Digger.Sack"
                || map.ToString() == "Digger.Monster")
                    return new CreatureCommand() { DeltaX = 0, DeltaY = 0 };
            return new CreatureCommand() { DeltaX = dx, DeltaY = dy };
        }

        private static void PlayerStay(int x, int y, ref int dx, ref int dy)
        {
            if (Player.X == x)
            {
                if (Player.Y < y) dy = -1;
                else if (Player.Y > y) dy = 1;
            }

            else if (Player.Y == y)
            {
                if (Player.X < x) dx = -1;
                else if (Player.X > x) dx = 1;
            }
            else
            {
                if (Player.X < x) dx = -1;
                else if (Player.X > x) dx = 1;
            }
        }



        static bool Location
        {
            get
            {
                for (int i = 0; i < Game.MapWidth; i++)
                {
                    for (int j = 0; j < Game.MapHeight; j++)
                    {
                        var map = Game.Map[i, j];
                        if (map != null)
                        {
                            if (map.ToString() == "Digger.Player")
                            {
                                Player.X = i;
                                Player.Y = j;
                                return true;
                            }
                        }
                    }
                }
                return false;
            }
        }

        public bool DeadInConflict(ICreature conflict)
        {
            return conflict.ToString() == "Digger.Sack" ||
            conflict.ToString() == "Digger.Monster";
        }

        public int GetDrawingPriority() => 0;

        public string GetImageFileName() => "Monster.png";
    }
}