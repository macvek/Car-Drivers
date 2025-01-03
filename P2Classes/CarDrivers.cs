﻿/* 
MIT License

Copyright (c) 2024 Maciej Aleksandrowicz

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
*/

using System.Text;

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
        public static Dictionary<char, Func<char, MapField>> generators = new Dictionary<char, Func<char, MapField>>
        {
            {' ', MapFieldGenerators.EmptyField },
            {'+', MapFieldGenerators.FlippingField },
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

        public static string[] BorderMap = [
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

        public static string[] BorderMapWithRing = [
            //start 1x1
            "##########",
            "#        #",
            "# ###### #",
            "# #    # #",
            "# #    # #",
            "# #    # #",
            "# #    # #",
            "# ###### #",
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

        public static string[] NarrowPass = [
            "#####################",
            "#                   #",
            "#                   #",
            "#         +         #",
            "#####################",

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

            for (int y = 0; y < input.Length; ++y)
            {
                if (input[y].Length != firstLineLength)
                {
                    throw new MapException("Line has different length than previous one. Affected line: " + y);
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

    public interface CandidatePicker
    {
        public void next()
        {

        }
        public Car pick(List<Car> candidates);
    }

    public class PickFirst : CandidatePicker
    {
        public Car pick(List<Car> candidates)
        {
            return candidates[0];
        }
    }

    public class FailOnPick : CandidatePicker
    {
        public Car pick(List<Car> candidates)
        {
            throw new Exception("Fail on pick triggered.");
        }
    }

    public class PickEven : CandidatePicker
    {
        private int idx = 1;
        public Car pick(List<Car> candidates)
        {
            return candidates[idx % candidates.Count];
        }

        public void next()
        {
            ++idx;
        }
    }


    public class MapFieldGenerators
    {
        public static MapField EmptyField(char face)
        {
            return new MapField(face, new PickFirst())
            {
                Blockade = false
            };
        }

        public static MapField FlippingField(char face)
        {
            return new MapField(face, new PickEven())
            {
                Blockade = false
            };
        }

        public static MapField WallField(char face)
        {
            return new MapField(face, new FailOnPick())
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

        private List<Car> Candidates = [];
        private CandidatePicker Picker;
        public MapField(char face, CandidatePicker picker) { Face = face; Picker = picker; }


        public void AddCandidate(Car c)
        {
            Candidates.Add(c);
        }
        
        public Car PickedCandidate()
        {
            return Picker.pick(Candidates);
        }

        public void ClearCandidates()
        {
            foreach (var c in Candidates)
            {
                c.PendingIntent = false;
            }
            Candidates.Clear();
        }

        public void ResolveCandidates(Map owner)
        {
            var pickedOne = PickedCandidate();
            Picker.next();
            owner.PlaceCar(pickedOne, this);

            pickedOne.IntentOffX = 0;
            pickedOne.IntentOffY = 0;

            ClearCandidates();
        }

        internal void DetachCar()
        {
            Car = null;
        }

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
            return $"CAR(face:{Face} x:{X}, y:{Y})";
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


    public class CarControllerClockwiseLookup
    {
        public Map Map;
        public Car Car { get; set; }
        public bool ccw = false;
        public int Stage { get; set; } = 0;

        public int[][] stageMovesCW  = [[ 1, 0], [0, 1], [-1, 0], [0,-1]];
        public int[][] stageMovesCCW = [[-1, 0], [0, 1], [ 1, 0], [0,-1]];

        public bool ApplyIntension()
        {
            int[][] stages = ccw ? stageMovesCCW : stageMovesCW;
            var stageX = stages[Stage][0];
            var stageY = stages[Stage][1];

            var newX = Car.X + stageX;
            var newY = Car.Y + stageY;

            // Reached a blockade, so change stage
            if (! (Map.InBound(newX, newY) && Map.IsAllowedToMove(newX, newY))) 
            {
                Stage = (Stage + 1) % stages.Length;
                stageX = stages[Stage][0];
                stageY = stages[Stage][1];
            }

            Car.IntentOffX = stageX;
            Car.IntentOffY = stageY;

            return true;
        }
    }

    public class Pair<T,U>(T a, T b) {
        public T A { get; set; } = a; 
        public T B { get; set; } = b;
    }

    public class CarControllerFollowCheckpoints 
    {
        public Car Car { get; set; }

        public int Stage = 0;
        public List<Pair<int, int>> Checkpoints { get; set; } = [];

        public bool ApplyIntension()
        {
            Pair<int, int> checkpoint = Checkpoints[Stage];
            if (Car.X == checkpoint.A && Car.Y == checkpoint.B)
            {
                Stage = (Stage + 1) % Checkpoints.Count;
                checkpoint = Checkpoints[Stage];
            }

            var toPoint = new CarControllerToPoint();
            toPoint.DestinationX = checkpoint.A;
            toPoint.DestinationY = checkpoint.B; 
            toPoint.Car = Car;

            return toPoint.ApplyIntension();
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
        public int fieldLoops = 0;
        public int fieldSearches = 0;
        public int inLoopChecks = 0;
        public int mainPasses = 0;
        public int ringPasses = 0;
        public int simCount = 0;


        public void Simulate()
        {
            ++simCount;
            fieldSearches = 0;
            fieldLoops = 0;
            inLoopChecks = 0;
            mainPasses = 0;
            ringPasses = 0;

            List<MapField> nextPass = [];

            // HINT: could be optimized by updating nextPass upon any Car intent created; but irrelevant for a single simulations step
            for (int y = 0; y<map.Height; y++)
            {
                for (int x = 0;x<map.Width; x++)
                {
                    var field = map.FieldAt(x, y);
                    ++fieldSearches;
                    var car = field.Car;
                    if (car == null)
                    {
                        continue;
                    }

                    car.PendingIntent = false;

                    if (IsOffsetProducingMovement(car.IntentOffX, car.IntentOffY))
                    {
                        int newX = car.X + car.IntentOffX;
                        int newY = car.Y + car.IntentOffY;
                        if (map.InBound(newX, newY) && map.IsAllowedToMove(newX, newY))
                        {
                            var newField = map.FieldAt(newX, newY);
                            Car? other = newField.Car;
                            
                            // only consider this field if it is empty OR can be emptied in this round
                            if (
                                other == null ||  // empty field OR
                                    (other.IntentOffY != 0 || other.IntentOffX != 0) && // field is NOT empty, but other car intends to move, BUT
                                    !( car.IntentOffX == -other.IntentOffX && car.IntentOffY == -other.IntentOffY ) // NOT in direction of current car (i.e. they cannot swap places)
                            )
                            {
                                newField.AddCandidate(car);
                                car.PendingIntent = true;
                                nextPass.Add(newField);
                            }
                        }
                    }
                }
            }

            // HINT: if fields are ordered in order of dependency then this can result in a single pass
            // Every field.ResolveCandidates moves car to current field while also removing it from already occupied field, and hence makes a space for another loop
            // if first item makes root for 2nd item, and all are dependent, then after first pass all dependent fields would be emptied
            // such search might only work starting from empty field, otherwise a ring check must be performed

            // index nextPass by idx
            // list all empty fields (can be later moved to an initial scanning step)
            // place it on result list and iteratively follow its candidates (ring is impossible)
            // follow for every empty field;
            // afterwards add all still indexed fields to a list

            List<MapField> orderedPass = [];
            List<MapField> emptyFields = [];
            HashSet<MapField> candidatesOrigin = [];
            
            foreach (var f in nextPass)
            {
                ++fieldLoops;
                if (f.Car == null)
                {
                    emptyFields.Add(f);
                }
                else
                {
                    candidatesOrigin.Add(f);
                }
            }

            // Sorting phase
            foreach (var f in emptyFields)
            {
                orderedPass.Add(f);
                var walk = f;
                for (; ; )
                {
                    ++fieldLoops;
                    var first = walk.PickedCandidate();
                    var firstField = map.FieldAt(first.X, first.Y);
                    if (candidatesOrigin.Remove(firstField)) // Remove -> True if it was found and removed;
                    {
                        // every item on candidatesOrigin had candidates; so we can safely add it for next processing
                        orderedPass.Add(firstField);
                        walk = firstField;
                    }
                    else
                    {
                        break;
                    }
                }
            }

            foreach (var f in candidatesOrigin)
            {
                orderedPass.Add(f);
            }

            nextPass = orderedPass;
            // Sorted passes phase
            for (; ; )
            {
                List<MapField> thisPass = nextPass;
                nextPass = [];
                ++fieldLoops;
                ++mainPasses;
                foreach (var field in thisPass)
                {
                    ++inLoopChecks;
                    if (field.Car == null)
                    {
                        field.ResolveCandidates(map);
                    }
                    else
                    {
                        nextPass.Add(field);
                    }
                }

                // higher if no field.Car == null hit
                if (thisPass.Count == nextPass.Count) {
                    break;
                }
            }

            // RING check phase
            var ringCheck = new List<MapField>(nextPass);

            while (nextPass.Count > 0)
            {
                ++ringPasses;
                ++fieldLoops;
                // All these fields have blocked moves; check if there is a loop between them
                var anyField = nextPass[0];
                bool fullRing = false;
                var ringList = new List<MapField>();
                ringList.Add(anyField);
                var field = anyField;
                for (; ; )
                {
                    
                    var car = field.Car ?? throw new Exception("Impossible state; car must not be null here");
                    if (car.PendingIntent == false)
                    {
                        break;
                    }
                    
                    var nextField = map.FieldAt(car.IntentOffX + field.X, car.IntentOffY + field.Y);
                    if (nextField.PickedCandidate() == car)
                    {
                        if (nextField == anyField)
                        {
                            fullRing = true;
                            break;
                        }
                        else
                        {
                            ringList.Add(nextField);
                            field = nextField;
                        }
                    }
                    else
                    {
                        break;
                    }
                }

                if (fullRing)
                {
                    foreach (var ringField in ringList)
                    {
                        if (ringField.Car != null)
                        {
                            // to be attached back during other field evaluation
                            ringField.DetachCar();
                        }

                        ringField.ResolveCandidates(map);
                    }

                    foreach (var ringField in ringList)
                    {
                        if (ringField.Car == null)
                        {
                            throw new Exception($"Failure in ring resolution. No car attached at {ringField.X}x{ringField.Y}");
                        }
                    }
                }

                foreach (var each in ringList) 
                {
                    nextPass.Remove(each);
                }   
            }

            foreach(var field in ringCheck)
            {
                field.ClearCandidates();
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

        public List<Car> AllCars()
        {
            List<Car> ret = [];
            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x< Width; x++)
                {
                    Car? c = FieldAt(x, y).Car;
                    if (c != null)
                    {
                        ret.Add(c);
                    }
                }
            }

            return ret;
        }

        public String[] Dump()
        {
            string[] lines = new string[Height];
            for (int y = 0; y < Height; y++)
            {
                StringBuilder sb = new();
                for (int x = 0; x < Width; x++)
                {
                    var field = FieldAt(x, y);
                    if (field.Car != null)
                    {
                        sb.Append(field.Car.Face);
                    }
                    else
                    {
                        sb.Append(field.Face);
                    }
                }

                lines[y] = sb.ToString();
            }

            return lines;
        }

        public String[] DumpIntension()
        {
            string[] lines = new string[Height];
            for (int y = 0; y < Height; y++)
            {
                StringBuilder sb = new();
                for (int x = 0; x < Width; x++)
                {
                    var field = FieldAt(x, y);
                    if (field.Car != null)
                    {
                        char move = '0';
                        if (field.Car.IntentOffX == 1)
                        {
                            move = '>';
                        }
                        if (field.Car.IntentOffX == -1)
                        {
                            move = '<';
                        }
                        if (field.Car.IntentOffY == 1)
                        {
                            move = 'v';
                        }
                        if (field.Car.IntentOffY == -1)
                        {
                            move = '^';
                        }
                        if (field.Car.IntentOffY != 0 && field.Car.IntentOffX != 0) { move = '!'; }
                        sb.Append(move);
                    }
                    else
                    {
                        sb.Append(field.Face);
                    }
                }

                lines[y] = sb.ToString();
            }

            return lines;
        }

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
                var previousField = FieldAt(c.X, c.Y);
                // Cleanup only if car is moved; it could be already detached from a field in such case other can could take its place
                if (previousField.Car == c)
                {
                    previousField.Car = null;
                }
            }

            dest.Car = c;
            c.X = dest.X;
            c.Y = dest.Y;
        }
    }

}
