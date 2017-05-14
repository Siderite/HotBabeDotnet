#region Using directives

using System;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;
using HotLogger;

#endregion

namespace HotBabe.Code.Helpers
{
  ///<summary>
  /// Used to help with path operations
  ///</summary>
  internal static class PathHelper
  {
    #region Public Methods

    ///<summary>
    /// Get absolute path from the application startup folder
    ///</summary>
    ///<param name="path"></param>
    ///<returns></returns>
    internal static string GetRootedPath(string path)
    {
      if (string.IsNullOrEmpty(path))
      {
        return null;
      }

      Uri uri;
      if (Uri.TryCreate(path, UriKind.Absolute, out uri))
      {
        return uri.AbsoluteUri;
      }
      if (Uri.TryCreate(StartUpFolderUri, path, out uri))
      {
        return uri.AbsoluteUri;
      }
      if (!IsRooted(path))
      {
        path = Path.Combine(StartUpFolder, path);
      }
      return path;
    }

    ///<summary>
    /// Get the relative path to the application startup folder (if in it)
    ///</summary>
    ///<param name="path"></param>
    ///<returns></returns>
    internal static string GetRelativePath(string path)
    {
      if (string.IsNullOrEmpty(path))
      {
        return null;
      }
      path = GetRootedPath(path);
      Uri uri;
      if (Uri.TryCreate(path, UriKind.Absolute, out uri) && StartUpFolderUri.IsBaseOf(uri))
      {
        return uri.LocalPath.Substring(StartUpFolderUri.LocalPath.Length);
      }
      return path;
    }

    ///<summary>
    /// return true if file exists (starting from application startup folder)
    ///</summary>
    ///<param name="path"></param>
    ///<returns></returns>
    internal static bool FileExists(string path)
    {
      if (string.IsNullOrEmpty(path))
      {
        return false;
      }
      path = GetRootedPath(path);
      Uri uri;
      if (Uri.TryCreate(path, UriKind.Absolute, out uri) && (uri.IsFile || uri.IsUnc))
      {
        return File.Exists(uri.LocalPath);
      }
      bool exists = false;
      try
      {
        using (Stream stream = GetReadStream(path))
        {
          if (stream != null)
          {
            exists = true;
          }
        }
      }
      catch (Exception ex)
      {
        Logger.Debug("Error getting response stream (FileExists)" + path + " : " + ex.Message);
      }
      return exists;
    }

    /// <summary>
    /// Return the directory name from a path or null if empty or invalid
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    internal static string GetDirectoryName(string path)
    {
      if (string.IsNullOrEmpty(path))
      {
        return null;
      }
      string startDir;
      try
      {
        startDir = Path.GetDirectoryName(path);
      }
      catch
      {
        startDir = null;
      }
      return startDir;
    }

    ///<summary>
    /// Get a read stream for a path, be it file, http, ftp, zip, res, etc
    ///</summary>
    ///<param name="path"></param>
    ///<returns></returns>
    internal static Stream GetReadStream(string path)
    {
      Stream ms;
      string rootedPath = GetRootedPath(path);
      WebRequest req = WebRequest.Create(rootedPath);
      using (WebResponse resp = req.GetResponse())
      {
        long length = resp.ContentLength;
        ms = GetSafeReadStream(resp.GetResponseStream(), length);
      }
      return ms;
    }

    ///<summary>
    /// Read a stream into a <see cref="MemoryStream"/> and return that
    ///</summary>
    ///<param name="stream"></param>
    ///<returns></returns>
    internal static Stream GetSafeReadStream(Stream stream)
    {
      return GetSafeReadStream(stream, stream.Length);
    }

    /// <summary>
    /// Read a stream into a <see cref="MemoryStream"/> and return that
    /// </summary>
    /// <param name="originalStream"></param>
    /// <param name="length"></param>
    /// <returns></returns>
    internal static Stream GetSafeReadStream(Stream originalStream, long length)
    {
      MemoryStream ms = length > 0
                            ? new MemoryStream((int) length)
                            : new MemoryStream();
      using (Stream stream = originalStream)
      {
        byte[] arr = new byte[100000];
        int l;
        do
        {
          l = stream.Read(arr, 0, arr.Length);
          if (l > 0)
          {
            ms.Write(arr, 0, l);
          }
        }
        while (l > 0);
      }
      ms.Seek(0, SeekOrigin.Begin);
      return ms;
    }

    ///<summary>
    /// Get a write stream for a path. Only local or UNC supported (files)
    ///</summary>
    ///<param name="path"></param>
    ///<returns></returns>
    internal static Stream GetWriteStream(string path)
    {
      string localPath = GetLocalPath(path);
      if (string.IsNullOrEmpty(localPath))
      {
        Logger.Debug("Can't get write stream for " + path);
        return null;
      }
      return new FileStream(localPath, FileMode.Create, FileAccess.Write,
                            FileShare.ReadWrite);
    }

    ///<summary>
    /// Get a local representation of a path. Returns null if not a file.
    ///</summary>
    ///<param name="path"></param>
    ///<returns></returns>
    internal static string GetLocalPath(string path)
    {
      Uri uri;
      if (Uri.TryCreate(GetRootedPath(path), UriKind.Absolute, out uri) && uri.IsFile)
      {
        return uri.LocalPath;
      }
      return null;
    }

    ///<summary>
    /// return true if there is a scheme or a drive in the filename
    ///</summary>
    ///<param name="path"></param>
    ///<returns></returns>
    public static bool IsRooted(string path)
    {
      return Regex.IsMatch(path, @"^[\w\d]+\:");
    }

    #endregion

    #region Properties

    ///<summary>
    /// Get the application startup folder
    ///</summary>
    internal static string StartUpFolder
    {
      get
      {
        return Path.GetDirectoryName(ApplicationPath);
      }
    }

    ///<summary>
    /// Get the application startup folder
    ///</summary>
    internal static Uri StartUpFolderUri
    {
      get
      {
        return new Uri(Path.GetDirectoryName(ApplicationPath) + "/");
      }
    }

    ///<summary>
    /// The running application path
    ///</summary>
    internal static string ApplicationPath
    {
      get
      {
        return typeof (Program).Assembly.Location;
      }
    }

    #endregion
  }
}