using P2Classes;
namespace P2Tests
{
    [TestClass]
    public class CarDriversTests
    {
        static bool MakeStats = false;
        [TestMethod]
        public void CheckBuildInMaps()
        {
            DriverMaps.Validate(DriverMaps.RoundMap);
            DriverMaps.Validate(DriverMaps.BorderMap);
            DriverMaps.Validate(DriverMaps.MazeMap);
            DriverMaps.Validate(DriverMaps.ClockwiseMap);
            DriverMaps.Validate(DriverMaps.NarrowPass);
        }

        [TestMethod]
        public void checkField1x1OnClockwiseMap()
        {
            var m = new Map();
            m.Load(DriverMaps.ClockwiseMap);
            var field = m.FieldAt(1, 1);
            Assert.AreEqual(14, m.Width);
            Assert.AreEqual(12, m.Height);
            Assert.IsFalse(field.Blockade);
        }

        [TestMethod]
        public void CheckMapValidation()
        {
            string[] emptyArray = [];
            Assert.ThrowsException<MapException>(() => DriverMaps.Validate(emptyArray));

            string[] emptyString = [""];
            Assert.ThrowsException<MapException>(() => DriverMaps.Validate(emptyString));

            string[] differentLineLengths = [
                "#####",
                "####",
                ];

            Assert.ThrowsException<MapException>(() => DriverMaps.Validate(differentLineLengths));

            string[] unsupportedFieldType = ["?"];
            Assert.ThrowsException<MapException>(() => DriverMaps.Validate(unsupportedFieldType));
        }
        [TestMethod]
        public void CheckLoadMap()
        {
            var singleFieldMap = new Map();
            singleFieldMap.Load([" "]);

            Assert.AreEqual(1, singleFieldMap.Width);
            Assert.AreEqual(1, singleFieldMap.Height);

            Assert.ThrowsException<ArgumentOutOfRangeException>(() => singleFieldMap.FieldAt(-1, 0));
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => singleFieldMap.FieldAt(0, -1));
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => singleFieldMap.FieldAt(1, 0));
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => singleFieldMap.FieldAt(0, 1));

            var field = singleFieldMap.FieldAt(0, 0);
            Assert.AreEqual(' ', field.Face);
            Assert.AreEqual(0, field.X);
            Assert.AreEqual(0, field.Y);

            var singleLineMap = new Map();
            singleLineMap.Load(["  "]);

            var leftField = singleLineMap.FieldAt(0, 0);
            var rightField = singleLineMap.FieldAt(1, 0);

            Assert.AreEqual(0, leftField.X);
            Assert.AreEqual(1, rightField.X);

            Assert.AreEqual(0, leftField.Y);
            Assert.AreEqual(0, rightField.Y);

            var quadMap = new Map();
            singleLineMap.Load([
                "# ",
                " #",
            ]);

            var bottomRight = singleLineMap.FieldAt(1, 1);
            Assert.AreEqual('#', bottomRight.Face);
            Assert.AreEqual(1, bottomRight.X);
            Assert.AreEqual(1, bottomRight.Y);
        }

        [TestMethod]
        public void CheckPlaceCar()
        {
            var singleFieldMap = new Map();
            singleFieldMap.Load([" "]);

            Car c = new();
            singleFieldMap.PlaceCar(c, 0, 0);


            Assert.AreEqual(c, singleFieldMap.FieldAt(0, 0).Car);
        }

        [TestMethod]
        public void CheckMoveCar()
        {
            var map = new Map();
            map.Load(["  "]);

            Car c = new();
            map.PlaceCar(c, 0, 0);
            map.PlaceCar(c, 1, 0);

            Assert.AreEqual(null, map.FieldAt(0, 0).Car);
            Assert.AreEqual(c, map.FieldAt(1, 0).Car);
        }


        [TestMethod]
        public void CheckCarCollision()
        {
            var map = new Map();
            map.Load(["  "]);

            Car c1 = new();
            Car c2 = new();
            map.PlaceCar(c1, 0, 0);
            map.PlaceCar(c2, 1, 0);

            map.PlaceCar(c2, 1, 0);

            Assert.ThrowsException<MapException>(() => map.PlaceCar(c1, c2.X, c2.Y));
        }

        [TestMethod]
        public void CheckCarMoveToBlockade()
        {
            var map = new Map();
            map.Load([" #"]);

            Car c1 = new();
            map.PlaceCar(c1, 0, 0);
            Assert.ThrowsException<MapException>(() => map.PlaceCar(c1, 1, 0));
            Assert.ThrowsException<MapException>(() => map.PlaceCar(new Car(), 1, 0));
        }

        [TestMethod]
        public void CheckCarMoveOutOfArea()
        {
            var map = new Map();
            map.Load([" "]);

            Car c1 = new();
            Assert.ThrowsException<MapException>(() => map.PlaceCar(c1, -1, 0));
            map.PlaceCar(c1, 0, 0);
            Assert.ThrowsException<MapException>(() => map.PlaceCar(new Car(), 1, 0));
        }

        [TestMethod]
        public void CheckSimpleSimulate()
        {
            var map = new Map();
            map.Load(["  "]);

            Car c = new();
            map.PlaceCar(c, 0, 0);

            c.IntentOffX = 1;
            c.IntentOffY = 0;

            var w = new World { map = map };
            w.Simulate();
            Assert.AreEqual(1, c.X);

            c.IntentOffX = 0;
            c.IntentOffY = 0;
            w.Simulate();
            Assert.AreEqual(1, c.X);

            c.IntentOffX = -1;
            c.IntentOffY = 0;
            w.Simulate();
            Assert.AreEqual(0, c.IntentOffX); // after commiting a movement, intentions are zeroed
            Assert.AreEqual(0, c.IntentOffY);
            Assert.AreEqual(0, c.X);

            // stuck against a blockade
            c.IntentOffX = -1;
            c.IntentOffY = 0;
            w.Simulate();
            Assert.AreEqual(0, c.X);
            Assert.AreEqual(-1, c.IntentOffX); // after failing to move, intent is left untouched
            Assert.AreEqual(0, c.IntentOffY);

            Car other = new();
            map.PlaceCar(other, 1, 0);

            c.IntentOffX = 1;   // try to move to occupied space
            c.IntentOffY = 0;
            w.Simulate();

            Assert.AreEqual(0, c.X);
        }

        [TestMethod]
        public void CheckSimulateIntoBlockade()
        {
            var map = new Map();
            map.Load([" #"]);

            Car c = new();
            map.PlaceCar(c, 0, 0);

            c.IntentOffX = 1;
            c.IntentOffY = 0;

            var w = new World { map = map };
            w.Simulate(); // move into blockade, should result in no operation with unchanged intent

            Assert.AreEqual(1, c.IntentOffX);
            Assert.AreEqual(0, c.IntentOffY);
        }

        [TestMethod]
        public void CheckRingWithMoveIntoBlockade()
        {
            var map = new Map();
            map.Load([
                "  #",
                "  #",
            ]);

            Car c1 = new();
            Car c2 = new();
            Car c3 = new();
            Car c4 = new();

            map.PlaceCar(c1, 0, 0);
            map.PlaceCar(c2, 1, 0);
            map.PlaceCar(c3, 1, 1);
            map.PlaceCar(c4, 0, 1);

            c1.IntentOffX = 1; c1.IntentOffY = 0;
            c2.IntentOffX = 0; c2.IntentOffY = 1;
            c3.IntentOffX = 1; c3.IntentOffY = 0; // this one tries to move towards blocked field
            c4.IntentOffX = 0; c4.IntentOffY = -1;

            var w = new World { map = map };
            w.Simulate();

            // No move should be performed
            Assert.AreEqual(0, c1.X); Assert.AreEqual(0, c1.Y);
            Assert.AreEqual(1, c2.X); Assert.AreEqual(0, c2.Y);
            Assert.AreEqual(1, c3.X); Assert.AreEqual(1, c3.Y);
            Assert.AreEqual(0, c4.X); Assert.AreEqual(1, c4.Y);
        }

        [TestMethod]
        public void OnlyUpDownLeftRightMovesAreAllowed()
        {
            var map = new Map();
            map.Load([
                "   ",
                "   ",
                "   "]
            );

            Car c = new();
            map.PlaceCar(c, 1, 1);
            var w = new World { map = map };
            w.Simulate();

            int[][] allowedMoves = [
                 [-1,0], // left
                 [1, 0], // right
                 [0,-1], // up
                 [0, 1], // down
            ];

            int[][] forbiddenMoves = [
                 [-1, -1], // top,left
                 [1 , -1], // top,right
                 [-1,  1], // bottom,left
                 [ 1,  1], // bottom,right
                 [ 2, 0], // move by more than 1 
                 [ -2, 2], // move by more than 1 
                 [ 0, -2], // move by more than 1 
            ];


            for (int i = 0; i < allowedMoves.Length; i++)
            {
                int[] pair = allowedMoves[i];
                map.PlaceCar(c, 1, 1);
                c.IntentOffX = pair[0];
                c.IntentOffY = pair[1];

                w.Simulate();
                Assert.AreEqual(c.X - pair[0], 1);
                Assert.AreEqual(c.Y - pair[1], 1);
            }

            for (int i = 0; i < forbiddenMoves.Length; i++)
            {
                int[] pair = forbiddenMoves[i];
                map.PlaceCar(c, 1, 1);
                c.IntentOffX = pair[0];
                c.IntentOffY = pair[1];

                Assert.ThrowsException<SimulateException>(() => w.Simulate(), "for case " + i);
            }


            c.IntentOffX = 0;
            c.IntentOffY = 0;
            w.Simulate();
            Assert.AreEqual(1, c.X);
        }

        [TestMethod]
        public void CheckIntensionToMoveToPoint()
        {
            var controller = new CarControllerToPoint();
            controller.DestinationX = 1;
            controller.DestinationY = 0;

            var map = new Map();
            map.Load(["  "]);

            Car c = new Car();
            controller.Car = c;

            map.PlaceCar(c, 0, 0);

            Assert.IsTrue(controller.ApplyIntension());

            var w = new World { map = map };
            w.Simulate();

            Assert.IsFalse(controller.ApplyIntension());

            Assert.AreEqual(0, c.IntentOffY);
            Assert.AreEqual(0, c.IntentOffX);

            Assert.AreEqual(1, c.X);
            Assert.AreEqual(0, c.Y);
        }

        [TestMethod]
        public void CheckIntensionToMoveClockwise()
        {
            var controller = new CarControllerClockwise();
            controller.Stage = 0;

            var map = new Map();
            map.Load([
                "  ",
                "  "
            ]);

            Car c = new Car();
            controller.Car = c;

            map.PlaceCar(c, 0, 0);

            var w = new World { map = map };

            controller.ApplyIntension();
            w.Simulate();
            Assert.AreEqual(1, c.X); Assert.AreEqual(0, c.Y);

            controller.ApplyIntension();
            w.Simulate(); // hits an obstacle

            controller.ApplyIntension(); // increases stage
            w.Simulate();

            Assert.AreEqual(1, c.X); Assert.AreEqual(1, c.Y);

            controller.ApplyIntension();
            w.Simulate(); // hits an obstacle

            controller.ApplyIntension();
            w.Simulate();
            Assert.AreEqual(0, c.X); Assert.AreEqual(1, c.Y);

            controller.ApplyIntension();
            w.Simulate(); // hits an obstacle

            controller.ApplyIntension();
            w.Simulate();
            Assert.AreEqual(0, c.X); Assert.AreEqual(0, c.Y);

            controller.ApplyIntension();
            w.Simulate(); // hits an obstacle

            controller.ApplyIntension();
            w.Simulate();
            Assert.AreEqual(1, c.X); Assert.AreEqual(0, c.Y);

        }

        [TestMethod]
        public void CheckConflictOfSingleCarBlockingOther()
        {
            var map = new Map();
            map.Load(["  "]);

            Car c1 = new Car();
            Car c2 = new Car();

            map.PlaceCar(c1, 0, 0);
            map.PlaceCar(c2, 1, 0);

            var w = new World { map = map };

            c1.IntentOffY = 0;
            c1.IntentOffX = 1;

            w.Simulate();

            Assert.AreEqual(0, c1.X);
            if (MakeStats) failWithStats(w); // SEARCHES STATS:2 1 0 1 0 ; after sorting 2 1 0 1 0
        }

        [TestMethod]
        public void CheckConflictOfBothMovingAside2Cars()
        {
            var map = new Map();
            map.Load(["   "]);

            Car c1 = new Car();
            Car c2 = new Car();

            map.PlaceCar(c1, 0, 0);
            map.PlaceCar(c2, 1, 0);

            var w = new World { map = map };

            c1.IntentOffY = 0;
            c2.IntentOffY = 0;

            c1.IntentOffX = 1;
            c2.IntentOffX = 1;

            w.Simulate();

            Assert.AreEqual(1, c1.X);

            if (MakeStats) failWithStats(w); // SEARCHES STATS:3 3 3 3 0 ;; after sorting 3 6 2 2 0
        }

        [TestMethod]
        public void CheckConflictOfBothMovingAside100Cars()
        {
            var map = new Map();
            map.Load(["                                                                                                     "]);
            Assert.AreEqual(101, map.Width);

            List<Car> cars = [];
            for (int i = 0; i < 100; i++)
            {
                Car c = new Car();
                map.PlaceCar(c, i, 0);
                c.IntentOffX = 1;
                c.IntentOffY = 0;
                cars.Add(c);
            }

            var w = new World { map = map };
            w.Simulate();

            for (int i = 0; i < 100; i++)
            {
                Assert.AreEqual(i + 1, cars[i].X);
            }

            if (MakeStats) failWithStats(w); //  SEARCHES STATS:101 101 5050 101 0 ;; after sorting: 101 202 100 2 0
        }

        [TestMethod]
        public void CheckConflictWithRing()
        {
            var map = new Map();
            map.Load([
                "  ",
                "  "
                ]);

            Car a = new();
            Car b = new();
            Car c = new();
            Car d = new();

            map.PlaceCar(a, 0, 0); a.IntentOffX = 1; a.IntentOffY = 0;
            map.PlaceCar(b, 1, 0); b.IntentOffX = 0; b.IntentOffY = 1;
            map.PlaceCar(c, 1, 1); c.IntentOffX = -1; c.IntentOffY = 0;
            map.PlaceCar(d, 0, 1); d.IntentOffX = 0; d.IntentOffY = -1;

            var w = new World { map = map };
            w.Simulate();

            Assert.AreEqual(a.X, 1); Assert.AreEqual(a.Y, 0);
            Assert.AreEqual(b.X, 1); Assert.AreEqual(b.Y, 1);
            Assert.AreEqual(c.X, 0); Assert.AreEqual(c.Y, 1);
            Assert.AreEqual(d.X, 0); Assert.AreEqual(d.Y, 0);

            if (MakeStats) failWithStats(w); //  SEARCHES STATS:4 2 4 1 1 ; after sorting 4 6 4 1 1
        }

        [TestMethod]
        public void CheckConflictWithRingGraphical()
        {
            var map = new Map();
            map.Load([
                "  ",
                "  "
            ]);

            placeCars(map,
                [
                    "ab",
                    "dc"
                ], 0,0
            );

            applyIntensions(map,
                [
                    ">v",
                    "^<"
                ], 0,0
            );

            var w = new World { map = map };
            w.Simulate();

            assertCars(map,
                [
                    "da",
                    "cb"
                ], 0, 0
            );
        }

        [TestMethod]
        public void CheckConflictWithTwoRings()
        {
            var map = new Map();
            map.Load([
                "    ",
                "    ",
                "    ",
                "    ",
                ]);

            Car a = new();
            Car b = new();
            Car c = new();
            Car d = new();

            map.PlaceCar(a, 0, 0); a.IntentOffX = 1; a.IntentOffY = 0;
            map.PlaceCar(b, 1, 0); b.IntentOffX = 0; b.IntentOffY = 1;
            map.PlaceCar(c, 1, 1); c.IntentOffX = -1; c.IntentOffY = 0;
            map.PlaceCar(d, 0, 1); d.IntentOffX = 0; d.IntentOffY = -1;

            Car e = new();
            Car f = new();
            Car g = new();
            Car h = new();

            map.PlaceCar(e, 2, 2); e.IntentOffX = 1; e.IntentOffY = 0;
            map.PlaceCar(f, 3, 2); f.IntentOffX = 0; f.IntentOffY = 1;
            map.PlaceCar(g, 3, 3); g.IntentOffX = -1; g.IntentOffY = 0;
            map.PlaceCar(h, 2, 3); h.IntentOffX = 0; h.IntentOffY = -1;

            var w = new World { map = map };
            w.Simulate();

            Assert.AreEqual(a.X, 1); Assert.AreEqual(a.Y, 0);
            Assert.AreEqual(b.X, 1); Assert.AreEqual(b.Y, 1);
            Assert.AreEqual(c.X, 0); Assert.AreEqual(c.Y, 1);
            Assert.AreEqual(d.X, 0); Assert.AreEqual(d.Y, 0);

            Assert.AreEqual(e.X, 3); Assert.AreEqual(e.Y, 2);
            Assert.AreEqual(f.X, 3); Assert.AreEqual(f.Y, 3);
            Assert.AreEqual(g.X, 2); Assert.AreEqual(g.Y, 3);
            Assert.AreEqual(h.X, 2); Assert.AreEqual(h.Y, 2);

            if (MakeStats) failWithStats(w); // SEARCHES STATS:16 3 8 1 2 ; after sorting 16 11 8 1 2 ;; here in ring example; sorting adds complexity as it is not needed due to rings
        }

        [TestMethod]
        public void CheckCannotMoveTowardsEachOther()
        {
            var map = new Map();
            map.Load([
                "####",
                "#   ",
                "#   ",
            ]);

            Car a = new(); a.Face = 'a';
            Car b = new(); b.Face = 'b';

            map.PlaceCar(a, 1, 1); a.IntentOffX = 1; a.IntentOffY = 0;
            map.PlaceCar(b, 2, 1); b.IntentOffX = -1; b.IntentOffY = 0;

            World w = new World();
            w.map = map;

            w.Simulate();

            Assert.AreEqual(1, a.X); Assert.AreEqual(1, a.Y);
            Assert.AreEqual(2, b.X); Assert.AreEqual(1, b.Y);
        }


        [TestMethod]
        public void SimulateClockwiseIterations()
        {
            var map = new Map();
            map.Load(DriverMaps.BorderMap);
            List<Car> allCars = [];
            List<CarControllerClockwise> controllers = [];

            String names = "0123456789abcdefghijklmnopqrst";
            int namePtr = 0;

            for (int x = 1; x < map.Width - 1; ++x)
            {
                for (int y = 1; y < map.Height - 1; ++y)
                {
                    if (x == 1 || x == map.Width - 2 || y == 1 || y == map.Height - 2)
                    {
                        Car car = new Car();
                        car.Face = names[(namePtr++) % names.Length];
                        map.PlaceCar(car, x, y);
                        allCars.Add(car);

                        var controller = new CarControllerClockwise();
                        controller.Car = car;
                        if (y == map.Height - 2)
                        {
                            controller.Stage = 2;
                        }
                        else
                        {
                            controller.Stage = 3;
                        }

                        controllers.Add(controller);
                    }
                }
            }

            World w = new World();
            w.map = map;
            bool dump = false;
            if (dump)
            {
                Console.WriteLine("START");
                dumpMap(map);
            }
            for (int i = 1; i < 1000; ++i)
            {
                foreach (var c in controllers)
                {
                    c.ApplyIntension();
                }

                if (dump)
                {
                    Console.WriteLine("Intension #" + i);
                    dumpIntensionMap(map);
                }

                w.Simulate();
                if (dump)
                {
                    Console.WriteLine("After #" + i);
                    dumpMap(map);
                }
            }
        }

        [TestMethod]
        public void CheckFlippingEntryField()
        {
            var map = new Map();
            map.Load([
                " + "
            ]);

            placeCars(map,
                [
                    "a b"
                ], 0, 0
            );

            applyIntensions(map,
                [
                    "> <"
                ], 0, 0
            );

            var w = new World { map = map };
            w.Simulate();

            assertCars(map,
                [
                    "ab "
                ], 0, 0
            );
        }

        private void dumpMap(Map map)
        {
            foreach (string each in map.Dump())
            {
                Console.WriteLine($"{each}");
            }
        }

        private void dumpIntensionMap(Map map)
        {
            foreach (string each in map.DumpIntension())
            {
                Console.WriteLine($"{each}");
            }
        }

        private void failWithStats(World w)
        {
            Assert.Fail("SEARCHES STATS:" + w.fieldSearches + " " + w.fieldLoops + " " + w.inLoopChecks + " " + w.mainPasses + " " + w.ringPasses);
        }

        private void placeCars(Map map, String[] list, int aX, int aY)
        {
            var cars = new List<Car>();

            for (int y = 0; y < list.Length; y++)
            {
                for (int x = 0; x < list[y].Length; x++)
                {
                    char face = list[y][x];
                    if (face == ' ' || face == '#')
                    {
                        continue;
                    }

                    Car car = new Car();
                    cars.Add(car);
                    car.Face = face;
                    map.PlaceCar(car, x + aX, y + aY);
                }

            }
        }

        private void assertCars(Map map, String[] list, int aX, int aY)
        {
            for (int y = 0; y < list.Length; y++)
            {
                for (int x = 0; x < list.Length; x++)
                {
                    char face = list[y][x];
                    if (face == ' ') continue;
                        
                    MapField field = map.FieldAt(x + aX, y + aY);
                    Car c = field.Car;
                    if (c == null)
                    {
                        Assert.Fail($"Expected a car, but there is none at {x + aX}, {y + aY}");
                    }
                    Assert.AreEqual(face, c.Face, $"Assert car failure at {x + aX}, {y + aY}");
                }
            }
        }

        private void applyIntensions(Map map, String[] list, int aX, int aY)
        {
            for (int y = 0; y < list.Length; y++)
            {
                for (int x = 0; x < list[y].Length; x++)
                {
                    char face = list[y][x];
                    MapField field = map.FieldAt(x + aX, y + aY);
                    Car c = field.Car;
                    if (face == '<')
                    {
                        c.IntentOffX = -1; c.IntentOffY = 0;
                    }

                    if (face == '>')
                    {
                        c.IntentOffX = 1; c.IntentOffY = 0;
                    }

                    if (face == '^')
                    {
                        c.IntentOffX = 0; c.IntentOffY = -1;
                    }

                    if (face == 'v')
                    {
                        c.IntentOffX = 0; c.IntentOffY = 1;
                    }

                }
            }
        }
    }
}