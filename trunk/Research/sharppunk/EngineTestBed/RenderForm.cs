using System.Drawing;
using System.Windows.Forms;
using sharppunk;

namespace EngineTestBed
{
    public partial class RenderForm : Form
    {
        public RenderForm()
        {
            InitializeComponent();
        }

        private Engine _engine = null;

        private Entity _testEntity1;
        private Entity _testEntity2;
        private Entity _testEntity3;
        private Entity _testEntity4;
        private Entity _testEntity5;

        private void RenderForm_Load(object sender, System.EventArgs e)
        {
            _engine = new Engine(800, 600, ".\\resources");

            var testEntityGraphic = new sharppunk.graphics.Image(new Bitmap(Bitmap.FromFile("test.bmp"))); //hmmmm

            _testEntity1 = new Entity(50, 50, testEntityGraphic);
            _testEntity2 = new Entity(150, 50, testEntityGraphic);
            _testEntity3 = new Entity(50, 150, testEntityGraphic);
            _testEntity4 = new Entity(250, 50, testEntityGraphic);
            _testEntity5 = new Entity(50, 250, testEntityGraphic);

            MP.currentWorld.Add(_testEntity1);
            MP.currentWorld.Add(_testEntity2);
            MP.currentWorld.Add(_testEntity3);
            MP.currentWorld.Add(_testEntity4);
            MP.currentWorld.Add(_testEntity5);
        }

        private void refreshTimer_Tick(object sender, System.EventArgs e)
        {
            if (_engine != null)
            {
                _engine.Render();
                OutputImage.Image = MP.Buffer;

                var image = (_testEntity1.Graphic as sharppunk.graphics.Image);

                image.Flipped = !image.Flipped;
                image.Angle += 15;

                if (image.Angle > 345) image.Angle = 0;
            }
        }
    }
}