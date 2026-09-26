using Android.App;
using Android.Content;
using Android.Provider;
using Microsoft.Maui.ApplicationModel;
using System;
using System.IO;
using System.Threading.Tasks;

namespace CVPilot.Platforms.Android.Services;

public static class AndroidPdfSaver
{
    private static TaskCompletionSource<string?>? _saveTask;

    private static byte[]? _pdfBytes;

    private const int CreateDocumentRequestCode = 9001;

    public static Task<string?> SaveAsync(
        string fileName,
        byte[] pdfBytes)
    {
        var activity = Platform.CurrentActivity;

        if (activity == null)
        {
            throw new InvalidOperationException(
                "Android activity is not available.");
        }

        if (_saveTask != null)
        {
            throw new InvalidOperationException(
                "A file save operation is already in progress.");
        }

        if (pdfBytes == null || pdfBytes.Length == 0)
        {
            throw new ArgumentException(
                "PDF data is empty.",
                nameof(pdfBytes));
        }

        // Store PDF bytes until the user selects
        // the location in the Android file picker.
        _pdfBytes = pdfBytes;

        _saveTask =
            new TaskCompletionSource<string?>(
                TaskCreationOptions.RunContinuationsAsynchronously);

        // Create Android file-save dialog.
        var intent =
            new Intent(Intent.ActionCreateDocument);

        intent.AddCategory(Intent.CategoryOpenable);

        intent.SetType("application/pdf");

        intent.PutExtra(
            Intent.ExtraTitle,
            fileName);

        activity.StartActivityForResult(
            intent,
            CreateDocumentRequestCode);

        return _saveTask.Task;
    }

    public static async Task HandleActivityResult(
        int requestCode,
        Result resultCode,
        Intent? data)
    {
        // Ignore unrelated activity results.
        if (requestCode != CreateDocumentRequestCode)
            return;

        var task = _saveTask;

        // Clear the task immediately so another export
        // can be started after this operation finishes.
        _saveTask = null;

        if (task == null)
            return;

        // User cancelled the save dialog.
        if (resultCode != Result.Ok || data?.Data == null)
        {
            _pdfBytes = null;

            task.TrySetResult(null);

            return;
        }

        // IMPORTANT:
        // global:: prevents the compiler from confusing
        // Android.Net.Uri with CVPilot.Platforms.Android.
        global::Android.Net.Uri uri = data.Data;

        try
        {
            var activity = Platform.CurrentActivity;

            if (activity == null)
            {
                _pdfBytes = null;

                task.TrySetException(
                    new InvalidOperationException(
                        "Android activity is not available."));

                return;
            }

            if (_pdfBytes == null || _pdfBytes.Length == 0)
            {
                task.TrySetException(
                    new InvalidOperationException(
                        "PDF data is not available."));

                return;
            }

            // Open the location selected by the user.
            using var outputStream =
                activity.ContentResolver?.OpenOutputStream(uri);

            if (outputStream == null)
            {
                _pdfBytes = null;

                task.TrySetException(
                    new IOException(
                        "Could not open the selected file location."));

                return;
            }

            // Write the actual PDF bytes to the selected location.
            await outputStream.WriteAsync(
                _pdfBytes,
                0,
                _pdfBytes.Length);

            await outputStream.FlushAsync();

            // PDF successfully written.
            _pdfBytes = null;

            // Return the URI of the saved PDF.
            task.TrySetResult(
                uri.ToString());
        }
        catch (Exception ex)
        {
            _pdfBytes = null;

            task.TrySetException(ex);
        }
    }
}