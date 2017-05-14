#region Using directives

using System;
using System.IO;
using System.Net;
using System.Web;
using ICSharpCode.SharpZipLib.Zip;

#endregion

namespace HotBabe.Code.Helpers
{
  ///<summary>
  /// Class responsible with working with zip files
  ///</summary>
  public static class ZipHelper
  {
    #region Public Methods

    ///<summary>
    /// register the zip:// URI prefix 
    ///</summary>
    public static void Register()
    {
      WebRequest.RegisterPrefix("zip", new ZipRequestCreator());
    }

    #endregion
  }

  ///<summary>
  /// Creator class for zip requests
  ///</summary>
  internal class ZipRequestCreator : IWebRequestCreate
  {
    #region Public Methods

    /// <summary>
    /// Creates a <see cref="T:System.Net.WebRequest"/> instance.
    /// </summary>
    /// <returns>
    /// A <see cref="T:System.Net.WebRequest"/> instance.
    /// </returns>
    /// <param name="uri">The uniform resource identifier (URI) of the Web resource. </param>
    public WebRequest Create(Uri uri)
    {
      return new ZipRequest(uri);
    }

    #endregion
  }

  ///<summary>
  /// Request class for zip files
  ///</summary>
  internal class ZipRequest : WebRequest
  {
    #region Member data

    private readonly string _archive;
    private readonly string _file;

    #endregion

    #region Constructors

    ///<summary>
    /// default constructor
    ///</summary>
    ///<param name="uri"></param>
    public ZipRequest(Uri uri)
    {
      _archive = HttpUtility.UrlDecode(uri.AbsolutePath);
      _file = HttpUtility.UrlDecode(uri.Query.TrimStart('?')).Replace("\\", "/");
    }

    #endregion

    #region Public Methods

    /// <summary>
    /// When overridden in a descendant class, returns a response to an Internet request.
    /// </summary>
    /// <returns>
    /// A <see cref="T:System.Net.WebResponse"/> containing the response to the Internet request.
    /// </returns>
    /// <exception cref="T:System.NotImplementedException">Any attempt is made to access the method, when the method is not overridden in a descendant class. </exception><PermissionSet><IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, ControlEvidence"/></PermissionSet>
    public override WebResponse GetResponse()
    {
      return new ZipResponse(_archive, _file);
    }

    #endregion
  }

  ///<summary>
  /// Response class for zip files
  ///</summary>
  internal class ZipResponse : WebResponse
  {
    #region Member data

    private readonly string _archive;
    private readonly string _file;

    #endregion

    #region Constructors

    ///<summary>
    /// default constructor
    ///</summary>
    ///<param name="archive"></param>
    ///<param name="file"></param>
    public ZipResponse(string archive, string file)
    {
      _archive = archive.Trim('/', '\\');
      _file = file;
    }

    #endregion

    #region Public Methods

    /// <summary>
    /// When overridden in a descendant class, returns the data stream from the Internet resource.
    /// </summary>
    /// <returns>
    /// An instance of the <see cref="T:System.IO.Stream"/> class for reading data from the Internet resource.
    /// </returns>
    /// <exception cref="T:System.NotSupportedException">Any attempt is made to access the method, when the method is not overridden in a descendant class. </exception><PermissionSet><IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, ControlEvidence"/></PermissionSet>
    /// <exception cref="FileNotFoundException"><c>FileNotFoundException</c>.</exception>
    public override Stream GetResponseStream()
    {
      Stream resultStream;
      using (Stream stream = PathHelper.GetReadStream(_archive))
      {
        using (ZipFile zipFile = new ZipFile(stream))
        {
          ZipEntry zipEntry = zipFile.GetEntry(_file);
          if (zipEntry == null)
          {
            throw new FileNotFoundException(string.Format("file {0} not found in archive {1}", _file, _archive));
          }
          using (Stream zipStream = zipFile.GetInputStream(zipEntry))
          {
            resultStream = PathHelper.GetSafeReadStream(zipStream);
          }
        }
      }
      return resultStream;
    }

    #endregion

    #region Properties

    /// <summary>
    /// When overridden in a descendant class, gets or sets the content length of data being received.
    /// </summary>
    /// <returns>
    /// The number of bytes returned from the Internet resource.
    /// </returns>
    /// <exception cref="T:System.NotSupportedException">Any attempt is made to get or set the property, when the property is not overridden in a descendant class. </exception><PermissionSet><IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, ControlEvidence"/></PermissionSet>
    /// <exception cref="FileNotFoundException"><c>FileNotFoundException</c>.</exception>
    public override long ContentLength
    {
      get
      {
        using (Stream stream = PathHelper.GetReadStream(_archive))
        {
          using (ZipFile zipFile = new ZipFile(stream))
          {
            ZipEntry zipEntry = zipFile.GetEntry(_file);
            if (zipEntry == null)
            {
              throw new FileNotFoundException(string.Format("file {0} not found in archive {1}", _file, _archive));
            }
            return zipEntry.Size;
          }
        }
      }
    }

    #endregion
  }
}