using P2Classes;
using System.Text;
using System.Timers;
using System.Windows.Forms;

namespace P2
{
    public partial class MainForm : Form
    {
        World world;
        Car c;
        Map map;
        System.Windows.Forms.Timer? timer;

        List<Car> allCars = [];
        List<CarControllerClockwise> controllerAllClockwise = [];

        CarControllerToPoint controllerToPoint = new CarControllerToPoint();
        CarControllerClockwise controllerClockwise = new CarControllerClockwise();

        Action? lastSim;

        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            resetWorldWithMap(DriverMaps.RoundMap, 1, 1);
        }

        private void resetWorldWithMap(string[] sourceMap, int startX, int startY)
        {
            world = new World();
            map = new Map();
            c = new Car();
            world.map = map;
            map.Load(sourceMap);
            map.PlaceCar(c, startX, startY);

            controllerToPoint.Car = c;
            controllerClockwise.Car = c;

            UpdatePreview(map);
        }

        void UpdatePreview(P2Classes.Map inputMap)
        {
            string[] lines = inputMap.Dump();

            PreviewBox.Lines = lines;

            for (int y = 0; y < inputMap.Height; y++)
            {
                for (int x = 0; x < inputMap.Width; x++)
                {
                    var field = inputMap.FieldAt(x, y);
                    if (field.Car != null)
                    {
                        ApplyStyle(x, y, inputMap.Width, FontStyle.Regular, Color.Green);
                    }
                    else if (field.Face == '#')
                    {
                        ApplyStyle(x, y, inputMap.Width, FontStyle.Bold, Color.Red);

                    }
                }
            }

            PreviewBox.Select(0, 0);
        }

        void ApplyStyle(int x, int y, int width, FontStyle style, Color color)
        {
            var from = x + (y * (width + 1)); // textbox adds lineending to every line
            PreviewBox.Select(from, 1);
            if (PreviewBox.SelectionFont != null)
            {
                var boldFont = new Font(PreviewBox.SelectionFont, style);
                PreviewBox.SelectionFont = boldFont;
            }

            PreviewBox.SelectionColor = color;
        }

        private void MoveRight_Click(object sender, EventArgs e)
        {
            MoveCarBy(1, 0);
        }

        private void MoveUp_Click(object sender, EventArgs e)
        {
            MoveCarBy(0, -1);
        }

        private void MoveDown_Click(object sender, EventArgs e)
        {
            MoveCarBy(0, 1);
        }

        private void MoveLeft_Click(object sender, EventArgs e)
        {
            MoveCarBy(-1, 0);
        }

        private void MoveCarBy(int x, int y)
        {
            try
            {
                map.PlaceCar(c, c.X + x, c.Y + y);
            }
            catch (MapException e)
            {
                MessageBox.Show(e.Message, "Failed to move car");
            }
            UpdatePreview(map);
        }

        private void SimClockwise_Click(object sender, EventArgs e)
        {
            lastSim = () =>
            {
                controllerClockwise.Car = c;
                controllerClockwise.ApplyIntension();

                simAndUpdate();
            };

            callLastSim();
        }

        private void simAllClockwise_Click(object sender, EventArgs e)
        {
            lastSim = () =>
            {
                foreach (var c in controllerAllClockwise)
                {
                    c.ApplyIntension();
                }

                simAndUpdate();
            };

            callLastSim();
        }

        private void simToPoint(int x, int y)
        {
            lastSim = () =>
            {
                controllerToPoint.DestinationX = x;
                controllerToPoint.DestinationY = y;
                controllerToPoint.ApplyIntension();

                simAndUpdate();
            };

            callLastSim();
        }

        private void simAndUpdate()
        {
            world.Simulate();
            UpdatePreview(map);
        }

        private void SimMoveTo11_Click(object sender, EventArgs e)
        {
            lastSim = () => simToPoint(1, 1);
            callLastSim();
        }

        private void SimMoveTo81_Click(object sender, EventArgs e)
        {
            lastSim = () => simToPoint(8, 1);
            callLastSim();
        }

        private void SimMoveTo88_Click(object sender, EventArgs e)
        {
            lastSim = () => simToPoint(8, 8);
            callLastSim();
        }

        private void SimMoveTo18_Click(object sender, EventArgs e)
        {
            lastSim = () => simToPoint(1, 8);
            callLastSim();
        }

        private void loadRoundMap_Click(object sender, EventArgs e)
        {
            resetWorldWithMap(DriverMaps.RoundMap, 1, 1);
        }

        private void loadMazeMap_Click(object sender, EventArgs e)
        {
            resetWorldWithMap(DriverMaps.MazeMap, 4, 4);
        }

        private void loadBorderMap_Click(object sender, EventArgs e)
        {
            resetWorldWithMap(DriverMaps.BorderMap, 1, 1);
        }

        private void loadClockwiseMap_Click(object sender, EventArgs e)
        {
            resetWorldWithMap(DriverMaps.ClockwiseMap, 1, 1);
        }

        private void repeat_Click(object sender, EventArgs e)
        {
            if (timer == null)
            {
                timer = new System.Windows.Forms.Timer();
                timer.Tick += (Object? src, EventArgs eea) =>
                {
                    callLastSim();
                };

                timer.Interval = 100;
                timer.Start();
            }
        }

        private void callLastSim()
        {
            lastSim?.Invoke();
        }

        private void stopRepeat_Click(object sender, EventArgs e)
        {
            if (timer != null)
            {
                timer.Stop();
                timer = null;
            }
        }

        private void loadFullBorderMap_Click(object sender, EventArgs e)
        {
            String names = "0123456789abcdefghijklmnopqrst";
            int namePtr = 1;
            resetWorldWithMap(DriverMaps.BorderMap, 1, 1);
            allCars.Clear();
            controllerAllClockwise.Clear();
            var sController = new CarControllerClockwise();
            sController.Car = c;
            c.Face = '0';
            controllerAllClockwise.Add(sController);
            for (int x = 1; x < map.Width - 1; ++x)
            {
                for (int y = 1; y < map.Height - 1; ++y)
                {
                    if (x == 1 && y == 1) continue;
                    if (x == 1 || x == map.Width - 2 || y == 1 || y == map.Height - 2)
                    {
                        Car car = new Car();
                        car.Face = names[(namePtr++) % names.Length];
                        map.PlaceCar(car, x, y);
                        allCars.Add(car);

                        var controller = new CarControllerClockwise();
                        controller.Car = car;
                        if (y == 1) {
                            controller.Stage = 0;
                        }

                        if (x == map.Width - 2)
                        {
                            controller.Stage = 1;
                        }

                        if (y == map.Height - 2)
                        {
                            controller.Stage = 2;
                        }
                        
                        else
                        {
                            controller.Stage = 3;
                        }
                        
                        controllerAllClockwise.Add(controller);
                    }
                }
            }

            UpdatePreview(map);

        }

        
    }
}
