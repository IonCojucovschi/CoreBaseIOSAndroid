using System;
using System.IO;
using System.Linq;
using System.Threading;
using Android.Graphics;
using Android.Views;
using Android.Widget;
using Com.Bumptech.Glide;
using FFImageLoading;
using FFImageLoading.Svg.Platform;
using FFImageLoading.Transformations;
using FFImageLoading.Views;
using FFImageLoading.Work;
using Int.Core.Application.Exception;
using Int.Core.Application.Widget.Contract;
using Int.Core.Extensions;
using Int.Core.Wrappers;
using Int.Core.Wrappers.Widget.Exceptions;
using System.Xml.Linq;

namespace Int.Droid.Wrappers.Widget.FactoryConcreteProducts
{
    public class CrossImageWrapper : CrossViewWrapper, IImage
    {
        private const string ImageNotFoundMessage = "Image named \"{0}\" was not found in resources OR not match system android 'NAMING'";
        private const string UrlIsEmptyOrNull = "Url is empty or null";
        private const string ResourceTypeDrawable = "drawable";
        private const string ResourceTypeDrawableNoDpi = "drawable-nodpi";
        private const string ImageAsyncErrorMessage = "Svg image can be set only to ImageViewAsync";
        private const string SvgExtension = "svg";
        private const string GifExtension = "gif";

        private Stream _strem;

        private string _imageColor;

        public CrossImageWrapper(View imageView) : base(imageView)
        {
            if (!(imageView is ImageView))
                throw new CrossWidgetWrapperConstructorException(
                    string.Format(NotCompatibleError, imageView?.GetType()));
        }

        protected virtual ImageView ImageView
        {
            get
            {
                var imageView = WrappedObject as ImageView;

                if (imageView == null)
                    ExceptionLogger.RaiseNonFatalException(
                        new ExceptionWithCustomStack(
                            WrappedObjectIsNull, Environment.StackTrace));

                return imageView;
            }
        }

        public void SetImageColorFilter(string color)
        {
            ThreadPool.QueueUserWorkItem(_ =>
            {
                AppTools.RunOnUiThread(() =>
                {
                    _imageColor = color;

                    if (string.IsNullOrWhiteSpace(_imageColor))
                    {
                        ImageView?.Drawable?.Mutate();
                        ImageView?.ClearColorFilter();

                        return;
                    }

                    var colorStruct = Extensions.Extensions.ToColor(_imageColor);

                    var mutatedDrawable = ImageView?.Drawable?.GetConstantState()?.NewDrawable()?.Mutate();
                    if (mutatedDrawable == null) return;
                    mutatedDrawable.SetColorFilter(colorStruct, PorterDuff.Mode.SrcIn);
                    ImageView.SetImageDrawable(mutatedDrawable);
                });
            });
        }

        public void SetImageFromUrl(string url, float quality = 1.0f)
        {
            if (string.IsNullOrWhiteSpace(url))
            {
                ExceptionLogger.RaiseNonFatalException(
                    new ExceptionWithCustomStack(
                        UrlIsEmptyOrNull, Environment.StackTrace));
                return;
            }

            Url = url;

            if (url.Split('.').LastOrDefault() == SvgExtension)
            {
                SetSvgImageFromUrl(url);
                return;
            }

            SetAsyncUrlImage(url);
        }

        public void SetImageFromResource(string imageName)
        {
            if (ImageView == null) return;

            if (imageName == null)
            {
                ImageView.SetImageDrawable(null);
                return;
            }

            switch (imageName.Split('.').LastOrDefault())
            {
                case SvgExtension:
                    SetSvgImageFromResource(imageName);
                    return;
                case GifExtension:
                    SetAsyncResourceImage(imageName);
                    return;
            }

            var resources = ImageView.Context.Resources;
            var packageName = ImageView.Context.PackageName;

            DrawableId = resources.GetIdentifier(imageName, ResourceTypeDrawable, packageName);

            if (DrawableId == 0)
                DrawableId = resources.GetIdentifier(imageName, ResourceTypeDrawableNoDpi, packageName);

            if (DrawableId == 0 && !string.IsNullOrWhiteSpace(imageName))
                ExceptionLogger.RaiseNonFatalException(
                    new ExceptionWithCustomStack(
                        string.Format(ImageNotFoundMessage, imageName), Environment.StackTrace));

            AppTools.CurrentActivity.RunOnUiThread(() =>
            {
                ImageView.SetImageResource(DrawableId);
            });
        }

        public void SetImageFromStream(Stream imageStream)
        {
            _strem = imageStream;
            SetAsyncStreamImage(imageStream);
        }

        public void SetSelected(string selectedColor)
        {
            ColorSelectedChild = selectedColor;
        }

        public override void OnTouchView(State state)
        {
            base.OnTouchView(state);

            switch (state)
            {
                case State.Began:
                case State.MoveIn:
                    SelectorBackroundImage(true);
                    break;
                default:
                    SelectorBackroundImage(false);
                    break;
            }
        }

        private void SetSvgImageFromUrl(string imagePath)
        {
            var loader = ImageService.Instance.LoadUrl(imagePath);
            SetSvgImage(loader);
        }

        private void SetSvgImageFromResource(string imagePath)
        {
            var loader = ImageService.Instance
                                     .LoadCompiledResource(imagePath);
            SetSvgImage(loader);
        }

        private void SetAsyncUrlImage(string imagePath)
        {
            ImageViewAsync imageAsync;
            if ((imageAsync = CastImageViewToAsync()) == null)
                return;

            Glide
                .With(AppTools.CurrentActivity)
                .Load(imagePath)
                .Into(imageAsync);

            //momentan
            //ImageService.Instance
            //.LoadUrl(imagePath)
            //.Finish(obj => SetImageColorFilter(_imageColor))
            //.DownSampleMode(InterpolationMode.Low)
            //.Into(imageAsync);
        }

        private void SetAsyncResourceImage(string imagePath)
        {
            ImageViewAsync imageAsync;
            if ((imageAsync = CastImageViewToAsync()) == null)
                return;

            ImageService.Instance
                        .LoadCompiledResource(imagePath)
                        .Finish(obj => SetImageColorFilter(_imageColor))
                        .DownSampleMode(InterpolationMode.Low)
                        .Into(imageAsync);
        }

        private void SetAsyncStreamImage(Stream imageStream)
        {
            ImageViewAsync imageAsync;
            if ((imageAsync = CastImageViewToAsync()) == null)
                return;

            ImageService.Instance
                        .LoadStream(cancelationToken =>
                                System.Threading.Tasks.Task.Factory.StartNew(
                                () =>
                                {
                                    cancelationToken.ThrowIfCancellationRequested();

                                    var streamClone = new MemoryStream();
                                    if (imageStream?.CanSeek ?? false)
                                        imageStream?.Seek(0, SeekOrigin.Begin);

                                    imageStream?.CopyTo(streamClone);

                                    if (streamClone?.CanSeek ?? false)
                                        streamClone?.Seek(0, SeekOrigin.Begin);

                                    return streamClone as Stream;
                                }, cancelationToken))
                        .Finish(obj => SetImageColorFilter(_imageColor))
                        .DownSampleMode(InterpolationMode.Low)
                        .Into(imageAsync);
        }

        private ImageViewAsync CastImageViewToAsync()
        {
            if (ImageView is ImageViewAsync imageAsync) return imageAsync;
            ExceptionLogger.RaiseNonFatalException(
                new ExceptionWithCustomStack(
                    ImageAsyncErrorMessage, Environment.StackTrace));
            return null;
        }

        private void SetSvgImage(TaskParameter loader)
        {
            ImageViewAsync imageAsync;
            if ((imageAsync = CastImageViewToAsync()) == null)
                return;

            loader
                .WithCustomDataResolver(new SvgDataResolver(200))
                .WithCustomLoadingPlaceholderDataResolver(new SvgDataResolver(200))
                .Finish(obj => SetImageColorFilter(_imageColor))
                .Into(imageAsync);
        }

        private void SelectorBackroundImage(bool invetColor)
        {
            if (invetColor)
            {
                if (ColorSelectedChild.IsNullOrWhiteSpace()) return;

                ImageView.ClearColorFilter();
                ImageView.Drawable?.SetColorFilter(ColorSelectedViewAndroid, PorterDuff.Mode.SrcIn);
            }
            else
                ImageView.Drawable?.ClearColorFilter();
        }

        private void Transform()
        {
            ImageViewAsync imageAsync;
            if ((imageAsync = CastImageViewToAsync()) == null)
                return;

            if (!_strem.IsNull())
                ImageService.Instance
                            .LoadStream(cancelationToken =>
                               System.Threading.Tasks.Task.Factory.StartNew(
                               () =>
                               {
                                   cancelationToken.ThrowIfCancellationRequested();

                                   var streamClone = new MemoryStream();
                                   if (_strem?.CanSeek ?? false)
                                       _strem?.Seek(0, SeekOrigin.Begin);

                                   _strem?.CopyTo(streamClone);

                                   if (streamClone?.CanSeek ?? false)
                                       streamClone?.Seek(0, SeekOrigin.Begin);

                                   return streamClone as Stream;
                               }, cancelationToken))
                            .Transform(new CircleTransformation())
                            .Finish(obj => SetImageColorFilter(_imageColor))
                            .Into(imageAsync);
            else if (!Url.IsNullOrWhiteSpace())
            {
                ImageService.Instance
                            .LoadUrl(Url)
                            .Transform(new CircleTransformation())
                            .Finish(obj => SetImageColorFilter(_imageColor))
                            .Into(imageAsync);
            }
            else if (DrawableId > 0)
                ImageService.Instance
                            .LoadCompiledResource(DrawableId.ToString())
                            .Transform(new CircleTransformation())
                            .Finish(obj => SetImageColorFilter(_imageColor))
                            .Into(imageAsync);
        }

        public void SetRoundImage()
        {
            Transform();
        }

    }
}