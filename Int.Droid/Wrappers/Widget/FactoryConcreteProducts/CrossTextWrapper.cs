using System;
using Android.Runtime;
using Android.Text;
using Android.Text.Method;
using Android.Views;
using Android.Widget;
using Int.Core.Application.Exception;
using Int.Core.Application.Widget.Contract;
using Int.Core.Extensions;
using Int.Core.Wrappers;
using Int.Core.Wrappers.Widget.Exceptions;
using Int.Droid.Extensions;
using Java.Lang;

namespace Int.Droid.Wrappers.Widget.FactoryConcreteProducts
{
    public class CrossTextWrapper : CrossViewWrapper, IText
    {
        private const string EmptyTextColorError = "Text color is null or empty";

        protected string ColorOriginalText;

        public CrossTextWrapper(View textView) : base(textView)
        {
            if (!(textView is TextView))
                throw new CrossWidgetWrapperConstructorException(string.Format(NotCompatibleError,
                    textView?.GetType()));
            AssignTextChangeEvents();
        }

        public Action<IText> TextChanged { get; set; }
        public Action Focus { get; set; }

        public string Text
        {
            get => (WrappedObject as TextView)?.Text;
            set
            {
                AppTools.CurrentActivity.RunOnUiThread(() =>
                {
                    if (WrappedObject is TextView view)
                        view.Text = value;
                });
            }
        }

        public string Hint
        {
            get => (WrappedObject as EditText)?.Hint;
            set
            {
                if (WrappedObject is EditText text)
                    text.Hint = value;
            }
        }


        public void SetFont(string fontType, float sizeFont = 16f)
        {
            (WrappedObject as TextView)?.SetFont(fontType, sizeFont);
        }

        public void SetHintColor(string textColor)
        {
            if (string.IsNullOrWhiteSpace(textColor))
            {
                ExceptionLogger.RaiseNonFatalException(
                    new ExceptionWithCustomStack(
                        EmptyTextColorError,
                        Environment.StackTrace));

                return;
            }

            if (!(WrappedObject is EditText editTextView)) return;

            // Android applies hint color only if text is null.
            var memorizedTextValue = editTextView.Text;
            editTextView.Text = null;
            editTextView.SetHintTextColor(textColor.ToColor());
            editTextView.Text = memorizedTextValue;
        }

        public void SetTextColor(string textColor)
        {
            if (string.IsNullOrWhiteSpace(textColor))
            {
                ExceptionLogger.RaiseNonFatalException(
                    new ExceptionWithCustomStack(
                        EmptyTextColorError,
                        Environment.StackTrace));

                return;
            }

            ColorOriginalText = textColor;

            AppTools.CurrentActivity?.RunOnUiThread(() =>
            {
                (WrappedObject as TextView)?.SetTextColor(ColorOriginalText.ToColor());
            });
        }
        public void SetLinkAndStyle(string substring, string link, string style)
        {
            var content = (WrappedObject as TextView)?.Text;
            var formatedSubstring = @"<a href='" + link + "' style='" + style + "'>" + substring + "</a>";
            var sb = new StringBuilder(content);
            if (content != null)
                sb.Replace(content.IndexOf(substring, StringComparison.CurrentCulture),
                    content.IndexOf(substring, StringComparison.CurrentCulture) + substring.Length, formatedSubstring);
            SetFormattedText((WrappedObject as TextView), sb.ToString());

            if ((WrappedObject as TextView) == null) return;
            ((TextView)WrappedObject).LinksClickable = true;
            ((TextView)WrappedObject).MovementMethod = LinkMovementMethod.Instance;

        }
        public void SetLinkAndStyle(string[] substring, string[] link, string style)
        {
            var content = (WrappedObject as TextView)?.Text;
            var plusLength = 0;
            var sb = new StringBuilder(content);
            for (var i = 0; i < substring.Length; i++)
            {
                var formatedSubstring = @"<a href='" + link[i] + "' style='" + style + "'>" + substring[i] + "</a>";
                if (content != null)
                    sb.Replace(content.IndexOf(substring[i], StringComparison.CurrentCulture) + plusLength,
                        content.IndexOf(substring[i], StringComparison.CurrentCulture) + plusLength +
                        substring[i].Length, formatedSubstring);
                plusLength += formatedSubstring.Length - substring[i].Length;
            }
            SetFormattedText((WrappedObject as TextView), sb.ToString());

            if ((WrappedObject as TextView) == null) return;

            ((TextView)WrappedObject).LinksClickable = true;
            ((TextView)WrappedObject).MovementMethod = LinkMovementMethod.Instance;
        }
        public void SetFormattedText(TextView textView, string text)
        {
            var result = default(ISpanned);

#pragma warning disable CS0618 // Type or member is obsolete
            result = (int)Android.OS.Build.VERSION.SdkInt >= 24
                ? Html.FromHtml(text, FromHtmlOptions.ModeLegacy)
                : Html.FromHtml(text);
#pragma warning restore CS0618 // Type or member is obsolete

            textView.SetText(result, TextView.BufferType.Normal);
        }

        public override void OnTouchView(State state)
        {
            base.OnTouchView(state);

            switch (state)
            {
                case State.Began:
                case State.MoveIn:
                case State.MoveOut:
                    SelectorBackroundText(true);
                    break;
                case State.Ended:
                    SelectorBackroundText(false);
                    break;
            }
        }

        public void SetSelectedTextColor(string colorText)
        {
            ColorSelectedChild = colorText;
        }

        private void AssignTextChangeEvents()
        {
            if (WrappedObject == null)
            {
                ExceptionLogger.RaiseNonFatalException(
                    new ExceptionWithCustomStack(
                        WrappedObjectIsNull, Environment.StackTrace));

                return;
            }

            if (!(WrappedObject is EditText editText)) return;

            editText.AddTextChangedListener(new TextChangeListener(this));
            editText.OnFocusChangeListener = new TextFocusListener(this);

        }

        private void SelectorBackroundText(bool invetColor)
        {
            if (invetColor)
            {
                if (ColorSelectedChild.IsNullOrWhiteSpace()) return;
                (WrappedObject as TextView)?.SetTextColor(ColorSelectedChild.ToColor());
            }
            else
            {
                if (ColorOriginalText.IsNullOrWhiteSpace()) return;
                (WrappedObject as TextView)?.SetTextColor(ColorOriginalText.ToColor());
            }
        }

        public void SetShadowLayer(float radius, float dx, float dy, string color)
        {
            AppTools.CurrentActivity.RunOnUiThread(() =>
            {
                (WrappedObject as TextView)?.SetShadowLayer(radius, dx, dy, color.ToColor());
            });
        }

        public void SetCursorColor(string cursorColor)
        {
            var intPtrtextViewClass = JNIEnv.FindClass(typeof(TextView));
            var mCursorDrawableResProperty = JNIEnv.GetFieldID(intPtrtextViewClass, "mCursorDrawableRes", "I");

            if (WrappedObject is TextView view)
                JNIEnv.SetField(view.Handle, mCursorDrawableResProperty, Resource.Drawable.cursor);
        }

        public void SetSecure(InputType transformation = InputType.Text, object nextController = null, Action executeGo = null)
        {
            if (transformation != InputType.Password) return;

            if (!(WrappedObject is TextView view)) return;
            if (view.InputType.HasFlag(InputTypes.ClassText | InputTypes.TextVariationPassword))
                view.InputType = InputTypes.TextVariationVisiblePassword;
            else
                view.InputType = InputTypes.TextVariationPassword | InputTypes.ClassText;
        }

        public void SetNextCursor(IText textNext = null, Action actionGo = null)
        {
        }

        public bool IsSecure => ((TextView)WrappedObject).InputType == InputTypes.TextVariationPassword;

        private class TextChangeListener : Java.Lang.Object, ITextWatcher
        {
            private readonly CrossTextWrapper _sender;

            public TextChangeListener(CrossTextWrapper sender)
            {
                _sender = sender;
            }

            public void AfterTextChanged(IEditable s)
            {
                _sender.TextChanged?.Invoke(_sender);
            }

            public void BeforeTextChanged(ICharSequence s, int start, int count, int after) { }

            public void OnTextChanged(ICharSequence s, int start, int before, int count) { }
        }

        private class TextFocusListener : Java.Lang.Object, View.IOnFocusChangeListener
        {
            private readonly CrossTextWrapper _sender;

            public TextFocusListener(CrossTextWrapper sender)
            {
                _sender = sender;
            }

            public void OnFocusChange(View v, bool hasFocus)
            {
                _sender.Focus?.Invoke();
            }
        }
    }
}