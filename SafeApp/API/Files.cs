using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SafeApp.AppBindings;
using SafeApp.Core;

namespace SafeApp.API
{
    /// <summary>
    /// Files API.
    /// </summary>
    public class Files
    {
        static readonly IAppBindings AppBindings = AppResolver.Current;
        IntPtr _appPtr;

        /// <summary>
        /// Initializes an Files object for the Session instance.
        /// The app pointer is required to perform network operations.
        /// </summary>
        /// <param name="safeAppPtr"></param>
        internal Files(SafeAppPtr safeAppPtr)
            => _appPtr = safeAppPtr;

        /// <summary>
        /// Create a FilesContainer.
        /// </summary>
        /// <param name="location"></param>
        /// <param name="dest"></param>
        /// <param name="recursive"></param>
        /// <param name="dryRun"></param>
        /// <returns></returns>
        public Task<(string, ProcessedFiles, FilesMap)> FilesContainerCreateAsync(
            string location,
            string dest,
            bool recursive,
            bool dryRun)
            => AppBindings.FilesContainerCreateAsync(_appPtr, location, dest, recursive, dryRun);

        /// <summary>
        /// Fetch an existing FilesContainer.
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public Task<(ulong, FilesMap)> FilesContainerGetAsync(
            string url)
            => AppBindings.FilesContainerGetAsync(_appPtr, url);

        /// <summary>
        /// Sync up local folder with the content on a FilesContainer.
        /// </summary>
        /// <param name="location"></param>
        /// <param name="url"></param>
        /// <param name="recursive"></param>
        /// <param name="delete"></param>
        /// <param name="updateNrs"></param>
        /// <param name="dryRun"></param>
        /// <returns></returns>
        public Task<(ulong, ProcessedFiles, FilesMap)> FilesContainerSyncAsync(
            string location,
            string url,
            bool recursive,
            bool delete,
            bool updateNrs,
            bool dryRun)
            => AppBindings.FilesContainerSyncAsync(_appPtr, location, url, recursive, delete, updateNrs, dryRun);

        /// <summary>
        /// Add a file, either a local path or a published file, on an existing FilesContainer.
        /// </summary>
        /// <param name="sourceFile"></param>
        /// <param name="url"></param>
        /// <param name="force"></param>
        /// <param name="updateNrs"></param>
        /// <param name="dryRun"></param>
        /// <returns></returns>
        public Task<(ulong, ProcessedFiles, FilesMap)> FilesContainerAddAsync(
            string sourceFile,
            string url,
            bool force,
            bool updateNrs,
            bool dryRun)
            => AppBindings.FilesContainerAddAsync(_appPtr, sourceFile, url, force, updateNrs, dryRun);

        /// <summary>
        /// Add a file, from raw bytes, on an existing FilesContainer.
        /// </summary>
        /// <param name="data"></param>
        /// <param name="url"></param>
        /// <param name="force"></param>
        /// <param name="updateNrs"></param>
        /// <param name="dryRun"></param>
        /// <returns></returns>
        public Task<(ulong, ProcessedFiles, FilesMap)> FilesContainerAddFromRawAsync(
            List<byte> data,
            string url,
            bool force,
            bool updateNrs,
            bool dryRun)
            => AppBindings.FilesContainerAddFromRawAsync(_appPtr, data, url, force, updateNrs, dryRun);

        /// <summary>
        /// Put Published ImmutableData
        /// </summary>
        /// <param name="data"></param>
        /// <param name="mediaType"></param>
        /// <returns></returns>
        public Task<string> FilesPutPublishedImmutableAsync(
            List<byte> data,
            string mediaType)
            => AppBindings.FilesPutPublishedImmutableAsync(_appPtr, data, mediaType);

        /// <summary>
        /// Get Published ImmutableData
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public Task<List<byte>> FilesGetPublishedImmutableAsync(
            string url)
            => AppBindings.FilesGetPublishedImmutableAsync(_appPtr, url);
    }
}
