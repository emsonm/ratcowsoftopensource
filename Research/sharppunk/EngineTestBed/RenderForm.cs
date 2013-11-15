using System.Drawing;
using System.Windows.Forms;
using sharppunk;
using sharppunk.Utils;

namespace EngineTestBed
{
    public partial class RenderForm : Form
    {
        public RenderForm()
        {
            InitializeComponent();

            Application.AddMessageFilter( KeyMessageFilter.Filter );
        }

        private Engine _engine = null;

        private Entity _testEntity1;
        private Entity _testEntity2;
        private Entity _testEntity3;
        private Entity _testEntity4;
        private Entity _testEntity5;
        private Player _player;

        private void RenderForm_Load(object sender, System.EventArgs e)
        {
            _engine = new Engine(800, 600, ".\\resources");

            refreshTimer.Enabled = false;
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

            _player = new Player(150, 250);
            MP.currentWorld.Add(_player);

            //public var playerSprite: Spritemap = new Spritemap(PLAYER2, 48, 32); 
            //playerSprite.add("stand", [0, 1, 2, 3, 4, 5], 20, true); 
            //playerSprite.add("run", [6, 7, 8, 9, 10, 11], 20, true); 
            //setHitbox(48, 32); 


            refreshTimer.Enabled = true;

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

                (_player.Graphic as sharppunk.graphics.Spritemap).Play("run", false);

                if ( image.Angle % 180 == 0 )
                    ( _player.Graphic as sharppunk.graphics.Image ).Flipped = !( _player.Graphic as sharppunk.graphics.Image ).Flipped;

                if ( Input.Check( Keys.A ) )
                {
                    _player.Position.X += 1;
                }

            }
        }

        private void RenderForm_PreviewKeyDown( object sender, PreviewKeyDownEventArgs e )
        {
        }
    }
}