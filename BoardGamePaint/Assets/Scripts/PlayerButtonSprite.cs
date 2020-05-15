using System;
using System.Drawing;
using System.Drawing.Imaging;

public class PlayerButtonSprite:ButtonSprite
{
    private PlayerButton playerButton { get => (PlayerButton)gameObject; }

	private Image playerImage;
	public override Image image { 
		get => playerImage;
		protected set => playerImage = value;
	}

	private ImageAttributes attr;

	public PlayerButtonSprite(PlayerButton playerButton, int size):base(playerButton,size)
	{
		playerImage = ImageUtility.getImage("player");
		//2019-09-09: ColorMap code copied from https://stackoverflow.com/a/27101587/2336212
		// Set the image attribute's color mappings
		ColorMap[] colorMap = new ColorMap[1];
		colorMap[0] = new ColorMap();
		colorMap[0].OldColor = Color.White;
		colorMap[0].NewColor = (this.playerButton.player) ? this.playerButton.player.color : Color.Gray;
		attr = new ImageAttributes();
		attr.SetRemapTable(colorMap);
    }

    public override void draw(Graphics graphics)
    {
        // Draw using the color map
        Rectangle rect = new Rectangle(
            (int)TopLeftScreen.x,
            (int)TopLeftScreen.y,
            size.Width,
            size.Height);
        graphics.DrawImage(
            image,
            rect,
            0,
            0,
            image.Size.Width,
            image.Size.Height,
            GraphicsUnit.Pixel,
            attr
            );
        drawFooterNumber(graphics);
    }
}
