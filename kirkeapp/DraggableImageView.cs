using System;
using MonoTouch.UIKit;
using System.Drawing;

namespace dk.kirkeapp {
	public delegate void OnDroppedImageHandler(PointF location);

	public class DraggableImageView : UIImageView {
		public event OnDroppedImageHandler OnDroppedImage;

		//Store locations for remembering the last positions, and counting the future ones.
		PointF Location;
		PointF StartLocation;
//		bool haveBeenTouchedOnce = false;

		public DraggableImageView(RectangleF frame) {

			//Set the position of the frame with RectangleF (Replacement of CGRectangle)
			this.Frame = frame;
			StartLocation = this.Frame.Location;
		}

		//This event occurs when you just touch the object
		public override void TouchesBegan(MonoTouch.Foundation.NSSet touches, MonoTouch.UIKit.UIEvent e) {
			Console.WriteLine("Touched the object");
			Location = this.Frame.Location;

			var touch = (UITouch)e.TouchesForView(this).AnyObject;
			var bounds = Bounds;

			StartLocation = touch.LocationInView(this);
			this.Frame = new RectangleF(Location, bounds.Size);

		}
		//This event occurs when you drag it around
		public override void TouchesMoved(MonoTouch.Foundation.NSSet touches, MonoTouch.UIKit.UIEvent e) {
			Console.WriteLine("Dragged the object");
			var bounds = Bounds;
			var touch = (UITouch)e.TouchesForView(this).AnyObject;

			//Always refer to the StartLocation of the object that you've been dragging.
			Location.X += touch.LocationInView(this).X - StartLocation.X;
			Location.Y += touch.LocationInView(this).Y - StartLocation.Y;

			this.Frame = new RectangleF(Location, bounds.Size);

//			haveBeenTouchedOnce = true;
		}

		public override void TouchesEnded(MonoTouch.Foundation.NSSet touches, MonoTouch.UIKit.UIEvent e) {
			StartLocation = Location;

			if (OnDroppedImage != null) {
				OnDroppedImage(Location);
			}
		}

	}}

