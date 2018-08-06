using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Android.Content;
using Android.Graphics;
using Android.Util;
using Android.Views;
using Android.Widget;
using Int.Core.Extensions;
using Int.Core.Network.Singleton;
using Int.Core.Tools.Font;

namespace Int.Droid.Tools.Font
{
    public class FontManager : Singleton<FontManager>, IFont<Typeface>
    {
        private static readonly ConcurrentDictionary<string, Typeface> Cache =
            new ConcurrentDictionary<string, Typeface>();

        public Typeface this[string index] => Cache[index];
        public string Extensions { get; set; } = ".ttf";

        public void Add(IList<string> nameFonts, string root = "")
        {
            if (nameFonts == null)
                throw new Exception("Not fonts name");

            foreach (var item in nameFonts.Where(item => Cache.TryAdd(item, LoadFont(item, root))))
                Log.Debug(nameof(FontManager), "Success Add Font");
        }

        public Typeface GetFont(string nameFont, float sizeFont = 16, string root = "")
        {
            var typeFace = Cache.GetOrAdd(nameFont, default(Typeface));

            if (typeFace != null)
                return typeFace;

            try
            {
                typeFace = !root.IsNullOrWhiteSpace()
                    ? Typeface.CreateFromAsset(AppTools.AppContext?.Assets, root + "/Fonts/" + nameFont)
                    : Typeface.CreateFromAsset(AppTools.AppContext?.Assets, "Fonts/" + nameFont);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Not found font" + ex.Message);
            }
            Cache.GetOrAdd(nameFont, typeFace);

            return typeFace;
        }

        public Typeface GetTypeface(string assetPath)
        {
            if (string.IsNullOrWhiteSpace(assetPath))
                throw new Exception("IsNullOrWhiteSpace");

            if (Cache.Count == 0)
                throw new Exception("Not Initialization");

            Typeface typeFace;
            Cache.TryGetValue(assetPath, out typeFace);
            return typeFace;
        }

        public Typeface GetTypeface(Context c, string assetPath, string root = "")
        {
            Typeface typeFace;

            typeFace = !root.IsNullOrWhiteSpace()
                ? Typeface.CreateFromAsset(AppTools.AppContext?.Assets, root + "/Fonts/" + assetPath + Extensions)
                : Typeface.CreateFromAsset(AppTools.AppContext?.Assets, "Fonts/" + assetPath + Extensions);


            return Cache.GetOrAdd(assetPath, typeFace);
        }

        public void OvverideFonts(View view, string resourceFont = "",
            TypefaceStyle typefaceStyle = TypefaceStyle.Normal, Typeface face = null)
        {
            var group = view as ViewGroup;

            if (group != null)
                for (var i = 0; i < group.ChildCount; i++)
                {
                    var child = group.GetChildAt(i);
                    OvverideFonts(child, resourceFont, typefaceStyle);
                }
            else
                (view as TextView)?.SetTypeface(
                    string.IsNullOrEmpty(resourceFont)
                        ? face
                        : Typeface.CreateFromAsset(AppTools.AppContext?.Assets, resourceFont), typefaceStyle);
        }


        private Typeface LoadFont(string item, string root = "")
        {
            Typeface typeFace = null;

            try
            {
                typeFace = !root.IsNullOrWhiteSpace()
                    ? Typeface.CreateFromAsset(AppTools.AppContext?.Assets, root + "/Fonts/" + item)
                    : Typeface.CreateFromAsset(AppTools.AppContext?.Assets, "Fonts/" + item);

                if (typeFace == null)
                    throw new Exception("Fail Load Font");
            }
            catch (Exception ex)
            {
                Debug.Write(ex.Message);
            }


            return typeFace;
        }
    }
}