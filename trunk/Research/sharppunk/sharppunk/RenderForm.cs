using System.Drawing;
using System.Windows.Forms;

namespace sharppunk
{
    public partial class RenderForm : Form
    {
        public RenderForm()
        {
            InitializeComponent();
        }

        private Engine _engine = null;

        private Entity _testEntity;

        private void RenderForm_Load(object sender, System.EventArgs e)
        {
            _engine = new Engine(800, 600, ".\\resources");

            var testEntityGraphic = new graphics.Image(new Bitmap(Bitmap.FromFile("test.bmp"))); //hmmmm

            _testEntity = new Entity(50, 50, testEntityGraphic);

            MP.currentWorld.Add(_testEntity);
        }

        private void refreshTimer_Tick(object sender, System.EventArgs e)
        {
            if (_engine != null)
            {
                _engine.Render();
                OutputImage.Image = MP.Buffer;

                var image = (_testEntity.Graphic as graphics.Image);

                image.Flipped = !image.Flipped;
                image.Angle += 15;

                if (image.Angle > 360) image.Angle = 0;
            }
        }
    }
}