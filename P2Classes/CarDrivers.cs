namespace P2Classes
{
    public class MapException : Exception {
        public MapException(string msg) : base(msg) { }
    }

    public class SimulateException : Exception
    {
        public SimulateException(string msg) : base(msg) { }
    }

    public class MapFieldsFactory
    {
        public static Dictionary<char, Func<char, MapField>> generators = new Dictionary<char, Func<char,MapField>>
        {
            {' ', MapFieldGenerators.EmptyField },
            {'#', MapFieldGenerators.WallField }
        };
    }

    public class DriverMaps
    {
        public static string[] RoundMap = [
            //start 1x1
            "##########",
            "#        #",
            "# ###### #",
            "# ###### #",
            "# ###### #",
            "# ###### #",
            "# ###### #",
            "# ###### #",
            "#        #",
            "##########",
        ];

        public static string[] MazeMap = [
           //start 4x4
            "##        ",
            "## ###### ",
            "## #      ",
            "#  # ### #",
            "# ##   # #",
            "#  ### # #",
            "## ### # #",
            "#  ### # #",
            "# #### # #",
            "#      ###",
        ];

        public static string[] BorderMap= [
           //start 1x1
            "##########",
            "#        #",
            "#        #",
            "#        #",
            "#        #",
            "#        #",
            "#        #",
            "#        #",
            "#        #",
            "##########",
        ];

        public static string[] ClockwiseMap = [
           //start 1x1
            "##############",
            "#            #",
            "##           #",
            "#          # #",
            "#  #         #",
            "#        #   #",
            "#   #        #",
            "#       #    #",
            "# #          #",
            "#         #  #",
            "#            #",
            "##############",
        ];

        public static void Validate(string[] input)
        {
            if (input.Length == 0)
            {
                throw new MapException("No fields defined, got empty input array");
            }

            int firstLineLength = input[0].Length;
            if (firstLineLength == 0)
            {
                throw new MapException("Line cannot be empty. Affected line: 0");
            }

            for (int y = 0; y<input.Length;++y)
            {
                if (input[y].Length != firstLineLength)
                {
                    throw new MapException("Line has different length than previous one. Affected line: "+y);
                }

                for (int x = 0; x < firstLineLength; x++)
                {
                    char testValue = input[y][x];
                    if (!MapFieldsFactory.generators.ContainsKey(testValue))
                    {
                        throw new MapException("Unsupported character at " + x + "," + y + " value: " + testValue);
                    }
                }
            }
        }
    }

    public class MapFieldGenerators
    {
        public static MapField EmptyField(char face)
        {
            return new MapField(face)
            {
                Blockade = false
            };
        }

        public static MapField WallField(char face)
        {
            return new MapField(face)
            {
                Blockade = true
            };
        }
    }


    public class MapField
    {
        public bool Blockade {  get; set; }
        public Car? Car {get;set;}

        public char Face { get; set; }
        
        public int X { get; set; }
        public int Y { get; set; }

        public List<Car> Candidates = [];
        public MapField(char face) { Face = face; }
    }

    public class Car
    {
        public int X { get; set; } = -1;
        public int Y { get; set; } = -1;

        public int IntentOffX { get; set; } = 0;
        public int IntentOffY { get; set; } = 0;
        public bool PendingIntent { get; set; }

        public char Face { get; set; } = 'S';

        public Car() { }

        public override string ToString()
        {
            return "CAR(x:" + X + ", y:" + Y + ")";
        }

    }

    public class CarControllerClockwise
    {
        public Car Car { get; set; }

        public int Stage { get; set; } = 0;

        public int[][] stageMoves = [ [1,0], [0,1], [-1,0], [0,-1] ];

        public bool ApplyIntension()
        {
            // Previous move was not successful, so change Stage
            if (Car.IntentOffY != 0 || Car.IntentOffX != 0)
            {
                Stage = (Stage + 1) % stageMoves.Length;
            }
            
            Car.IntentOffX = stageMoves[Stage][0];
            Car.IntentOffY = stageMoves[Stage][1];

            return true;
        }
    }

    public class CarControllerToPoint
    {
        public Car Car { get; set; }

        public int DestinationX {  get; set; }
        public int DestinationY { get; set; }


        public bool ApplyIntension()
        {
            int setX = 0;
            int setY = 0;

            // move horizontal first
            if (DestinationX != Car.X)
            {
                setX = (DestinationX - Car.X) / Math.Abs(DestinationX - Car.X);
            }
            else if (DestinationY != Car.Y)
            {
                setY = (DestinationY - Car.Y) / Math.Abs(DestinationY - Car.Y);
            }

            Car.IntentOffX = setX;
            Car.IntentOffY = setY;

            return setX != 0 || setY != 0;
            
        }
    }


    public class World
    {
        public Map map;
        public int fieldLoops;
        public int fieldSearches;
        public int inLoopChecks = 0;

        public void Simulate()
        {
            fieldSearches = 0;
            fieldLoops = 0;
            inLoopChecks = 0;

            List<MapField> nextPass = [];

            for (int y = 0; y<map.Height; y++)
            {
                for (int x = 0;x<map.Width; x++)
                {
                    var field = map.FieldAt(x, y);
                    ++fieldSearches;
                    var car = field.Car;
                    if (car != null)
                    {
                        if (IsOffsetProducingMovement(car.IntentOffX, car.IntentOffY))
                        {
                            int newX = car.X + car.IntentOffX;
                            int newY = car.Y + car.IntentOffY;
                            if (map.InBound(newX, newY) && map.IsAllowedToMove(newX, newY))
                            {
                                var newField = map.FieldAt(newX, newY);
                                newField.Candidates.Add(car);
                                car.PendingIntent = true;
                                nextPass.Add(newField);
                            }
                        }
                    }
                }
            }

            for (; ; )
            {
                List<MapField> thisPass = nextPass;
                nextPass = [];
                ++fieldLoops;
                foreach (var field in thisPass)
                {
                    ++inLoopChecks;
                    if (field.Car == null)
                    {
                        var firstOne = field.Candidates[0];
                        map.PlaceCar(firstOne, field.X, field.Y);

                        firstOne.IntentOffX = 0;
                        firstOne.IntentOffY = 0;

                        foreach (var candidate in field.Candidates)
                        {
                            candidate.PendingIntent = false;
                        }

                        field.Candidates.Clear();
                    }
                    else
                    {
                        nextPass.Add(field);
                    }
                }

                // eventually 0 or higher if no field.Car==null hit
                if (thisPass.Count == nextPass.Count) {
                    break;
                }
            }

            foreach(var field in nextPass)
            {
                foreach (var c in field.Candidates)
                {
                    c.PendingIntent = false;
                }
                field.Candidates.Clear();
            }
        }

        static bool IsOffsetProducingMovement(int x, int y)
        {
            if (( x == -1 || x == 1 ) && y ==0 || (y == -1 || y == 1) && x == 0) { return true; }
            if (x == 0 && y == 0) { return false; }

            throw new SimulateException("Invalid pair of intended movement x:" + x + " y:" + y);

        }
    }

    public class Map
    {
        public int Width { get; set; }
        public int Height { get; set; }

        List<MapField> Fields = [];

        public void Load(string[] input)
        {
            var width= input[0].Length;
            var height= input.Length;

            Fields = new List<MapField>(height * width);

            for (int y = 0; y < height; y++)
            {
                var line = input[y];
                for (int x = 0; x < width; x++)
                {
                    char face = line[x];
                    var eachField = MapFieldsFactory.generators[face](face);
                    eachField.X = x;
                    eachField.Y = y;
                    Fields.Add(eachField);
                }
            }

            Width = width;
            Height = height;
        }

        public MapField FieldAt(int x, int y)
        {
            if (!InBound(x,y)) { 
                throw new ArgumentOutOfRangeException("x or y", "range <0 .. "+Width+") , got "+x+" range <0 .. " + Height+ ") , got " + y);
            }

            return Fields[y * Width + x];
        }

        public bool InBound(int x, int y)
        {
            return !(x < 0 || y < 0 || x >= Width || y >= Height);
        }

        public bool IsAllowedToMove(int x, int y)
        {
            var f = FieldAt(x, y);
            return !f.Blockade;
        }

        public void PlaceCar(Car c, int x, int y)
        {
            if (!InBound(x,y))
            {
                throw new MapException("Cannot place car outside of map: Car=" + c + " x:" + x + " y:" + y);
            }

            var dest = FieldAt(x, y);
            PlaceCar(c, dest);
        }

        public void PlaceCar(Car c, MapField dest)
        {
            if (dest.Car != null)
            {
                if (dest.Car == c)
                {
                    return;
                }
                else
                {
                    throw new MapException("Cannot place car on already taken field: Car=" + c + " x:" + dest.X + " y:" + dest.Y);
                }
            }

            if (dest.Blockade)
            {
                throw new MapException("Cannot place car onto blocked field: Car=" + c + " x:" + dest.X + " y:" + dest.Y);
            }

            if (c.X != -1 && c.Y != -1)
            {
                FieldAt(c.X, c.Y).Car = null;
            }

            dest.Car = c;
            c.X = dest.X;
            c.Y = dest.Y;
        }
    }

}
