using System;
using System.IO;
using System.Threading;
using FFImageLoading;
using Int.Core.Application.Exception;
using Int.Core.Application.Widget.Contract;
using Int.Core.Wrappers;
using Int.Core.Wrappers.Widget.Exceptions;
using Int.iOS.Extensions;
using UIKit;

namespace Int.iOS.Wrappers.Widget.FactoryConcreteProducts
{
    public class CrossImageWrapper : CrossViewWrapper, IImage
    {
        private const string ImageNotFoundMessage = "Image named \"{0}\" was not found";
        private const string UrlIsEmptyOrNull = "Url is empty or null";
        private UIColor _colorTint;

        private UIImage _originalImage;

        public CrossImageWrapper(UIView imageView) : base(imageView)
        {
            if (!(imageView is UIImageView))
                throw new CrossWidgetWrapperConstructorException(
                    string.Format(NotCompatibleError, imageView?.GetType()));
        }

        protected virtual UIImageView ImageView
        {
            get
            {
                var imageView = WrapedObject as UIImageView;

                if (imageView == null)
                    ExceptionLogger.RaiseNonFatalException(
                        new ExceptionWithCustomStack(
                            WrappedObjectIsNull, Environment.StackTrace));

                return imageView;
            }
        }

        protected string ColorSelectedImage { get; private set; }

        public void SetImageColorFilter(string color)
        {
            ThreadPool.QueueUserWorkItem(_ =>
            {
                AppTools.InvokeOnMainThread(() =>
                {
                    if (ImageView == null) return;

                    _colorTint = string.IsNullOrWhiteSpace(color) ? null : color.FromHex();

                    if (_colorTint == null)
                    {
                        ImageView.Image = _originalImage;
                        return;
                    }

                    ApplyImageTint();
                });
            });
        }

        public void SetImageFromUrl(string url, float quality = 1.0f)
        {
            ImageView?.SetImage(url, success: ApplyImageTint);
        }

        public void SetImageFromResource(string imageName)
        {
            ImageView?.SetImage(imageName, success: ApplyImageTint);
        }

        public void SetImageFromStream(Stream imageStream)
        {
            ImageView?.SetImage(imageStream, ApplyImageTint);
        }

        public void SetSelected(string selectedColorImage)
        {
            ColorSelectedImage = selectedColorImage;
        }

        public override void OnTouchView(State state)
        {
            base.OnTouchView(state);

            switch (state)
            {
                case State.Began:
                case State.MoveIn:
                    SelectorBackroundText(true);
                    break;
                case State.MoveOut:
                case State.Cancel:
                    SelectorBackroundText(false);
                    break;
                default:
                    SelectorBackroundText(false);
                    break;
            }
        }

        private void ApplyImageTint()
        {
            AppTools.InvokeOnMainThread(() =>
            {
                if (ImageView?.Image == null) return;

                _originalImage = new UIImage(ImageView?.Image?.CGImage);

                if (_colorTint == null) return;

                ImageView?.ChangeColorTint(_colorTint);
            });
        }

        private UIImage BackupImageBeforeTouch { get; set; }
        private void SelectorBackroundText(bool invetColor)
        {
            ThreadPool.QueueUserWorkItem(_ =>
            {
                AppTools.InvokeOnMainThread(() =>
                {
                    if (ImageView?.Image == null) return;

                    if (invetColor)
                    {
                        BackupImageBeforeTouch = new UIImage(ImageView?.Image?.CGImage);
                        ImageView.ChangeColorTint(ColorSelectedImage?.ToUIColor());
                    }
                    else
                        ImageView.Image = BackupImageBeforeTouch;
                });
            });
        }

        //todo impliment
        public void SetRoundImage()
        {
        }

        //todo implement
        public void SetRoundCornerImage(int corner)
        {
        }
    }
}