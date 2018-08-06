using Android.Content;
using Android.Database;
using Android.Net;
using Android.OS;
using Android.Provider;
using Java.Lang;
using Debug = System.Diagnostics.Debug;

namespace Int.Droid.Extensions
{
    public static partial class Extensions
    {
        public static string GetFilePath(this Uri uri, Context context)
        {
            try
            {
                var isKitKat = Build.VERSION.SdkInt >= BuildVersionCodes.Kitkat;

                // DocumentProvider
                if (isKitKat && DocumentsContract.IsDocumentUri(context, uri))
                {
                    // ExternalStorageProvider
                    if (IsExternalStorageDocument(uri))
                    {
                        var docId = DocumentsContract.GetDocumentId(uri);
                        var split = docId.Split(':');
                        var type = split[0];

                        return Environment.GetExternalStoragePublicDirectory(type) + "/" + split[1];
                    }
                    // DownloadsProvider
                    if (IsDownloadsDocument(uri))
                    {
                        var id = DocumentsContract.GetDocumentId(uri);
                        var contentUri = ContentUris.WithAppendedId(
                            Uri.Parse("content://downloads/public_downloads"), Long.ParseLong(id));

                        return GetDataColumn(context, contentUri, null, null);
                    }
                    // MediaProvider
                    if (IsMediaDocument(uri))
                    {
                        var docId = DocumentsContract.GetDocumentId(uri);
                        var split = docId.Split(':');
                        var type = split[0];

                        Uri contentUri = null;
                        switch (type)
                        {
                            case "image":
                                contentUri = MediaStore.Images.Media.ExternalContentUri;
                                break;
                            case "video":
                                contentUri = MediaStore.Video.Media.ExternalContentUri;
                                break;
                            case "audio":
                                contentUri = MediaStore.Audio.Media.ExternalContentUri;
                                break;
                        }

                        const string selection = "_id=?";
                        var selectionArgs = new[]
                        {
                            split[1]
                        };

                        return GetDataColumn(context, contentUri, selection, selectionArgs);
                    }
                }
                // MediaStore (and general)
                else
                {
                    switch (uri.Scheme)
                    {
                        case "content":
                            return GetDataColumn(context, uri, null, null);
                        case "file":
                            return uri.Path;
                    }
                }

                return null;
            }
            catch (Exception e)
            {
                Debug.WriteLine($"[Uri.GetPath Extension error]\n{e.Message}\n{e.StackTrace}");
                return null;
            }
        }

        /**
         * Get the value of the data column for this Uri. This is useful for
         * MediaStore Uris, and other file-based ContentProviders.
         *
         * @param context The context.
         * @param uri The Uri to query.
         * @param selection (Optional) Filter used in the query.
         * @param selectionArgs (Optional) Selection arguments used in the query.
         * @return The value of the _data column, which is typically a file path.
         */
        private static string GetDataColumn(Context context, Uri uri, string selection,
            params string[] selectionArgs)
        {
            ICursor cursor = null;
            const string column = "_data";
            var projection = new[]
            {
                column
            };

            try
            {
                cursor = context.ContentResolver.Query(uri, projection, selection, selectionArgs,
                    null);
                if (cursor != null && cursor.MoveToFirst())
                {
                    var columnIndex = cursor.GetColumnIndexOrThrow(column);
                    return cursor.GetString(columnIndex);
                }
            }
            finally
            {
                cursor?.Close();
            }
            return null;
        }


        /**
         * @param uri The Uri to check.
         * @return Whether the Uri authority is ExternalStorageProvider.
         */
        private static bool IsExternalStorageDocument(Uri uri)
        {
            return "com.android.externalstorage.documents" == uri.Authority;
        }

        /**
         * @param uri The Uri to check.
         * @return Whether the Uri authority is DownloadsProvider.
         */
        private static bool IsDownloadsDocument(Uri uri)
        {
            return "com.android.providers.downloads.documents" == uri.Authority;
        }

        /**
         * @param uri The Uri to check.
         * @return Whether the Uri authority is MediaProvider.
         */
        private static bool IsMediaDocument(Uri uri)
        {
            return "com.android.providers.media.documents" == uri.Authority;
        }
    }
}