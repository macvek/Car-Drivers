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
            Assert.ThrowsException<MapException>(()=>DriverMaps.Validate(emptyArray));

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

            Assert.ThrowsException<ArgumentOutOfRangeException>(() => singleFieldMap.FieldAt(-1,0));
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => singleFieldMap.FieldAt(0, -1));
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => singleFieldMap.FieldAt(1, 0));
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => singleFieldMap.FieldAt(0, 1));

            var field = singleFieldMap.FieldAt(0, 0);
            Assert.AreEqual(' ', field.Face);
            Assert.AreEqual(0, field.X);
            Assert.AreEqual(0, field.Y);

            var singleLineMap= new Map();
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

            Assert.ThrowsException<MapException>(() => map.PlaceCar(c1,c2.X,c2.Y));
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

            int[][] forbiddenMoves= [
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
                c.IntentOffY= pair[1];

                w.Simulate() ;
                Assert.AreEqual(c.X - pair[0], 1);
                Assert.AreEqual(c.Y - pair[1], 1);
            }

            for (int i = 0; i < forbiddenMoves.Length; i++)
            {
                int[] pair = forbiddenMoves[i];
                map.PlaceCar(c, 1, 1);
                c.IntentOffX = pair[0];
                c.IntentOffY = pair[1];

                Assert.ThrowsException<SimulateException>(()=>w.Simulate(), "for case "+i);
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
            if (MakeStats) failWithStats(w); // SEARCHES STATS:2 1 0 1 0
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

            if (MakeStats) failWithStats(w); // SEARCHES STATS:3 3 3 3 0
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

            for (int i = 0;i < 100;i++)
            {
                Assert.AreEqual(i+1, cars[i].X);
            }

            if (MakeStats) failWithStats(w); //  SEARCHES STATS:101 101 5050 101 0
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

            map.PlaceCar(a, 0, 0); a.IntentOffX = 1;    a.IntentOffY = 0;
            map.PlaceCar(b, 1, 0); b.IntentOffX = 0;    b.IntentOffY = 1;
            map.PlaceCar(c, 1, 1); c.IntentOffX = -1;   c.IntentOffY = 0;
            map.PlaceCar(d, 0, 1); d.IntentOffX = 0;    d.IntentOffY = -1;
            
            var w = new World { map = map };
            w.Simulate();

            Assert.AreEqual(a.X, 1); Assert.AreEqual(a.Y, 0);
            Assert.AreEqual(b.X, 1); Assert.AreEqual(b.Y, 1);
            Assert.AreEqual(c.X, 0); Assert.AreEqual(c.Y, 1);
            Assert.AreEqual(d.X, 0); Assert.AreEqual(d.Y, 0);

            if (MakeStats) failWithStats(w); //  SEARCHES STATS:4 2 4 1 1
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

            if (MakeStats) failWithStats(w); // SEARCHES STATS:16 3 8 1 2
        }

        private void failWithStats(World w)
        {
            Assert.Fail("SEARCHES STATS:" + w.fieldSearches + " " + w.fieldLoops + " " + w.inLoopChecks+" "+w.mainPasses+" "+w.ringPasses);
        }
    }
}