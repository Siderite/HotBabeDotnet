#region Using directives

using System;
using System.IO;
using System.Net;
using System.Reflection;

#endregion

namespace HotBabe.Code.Helpers
{
  ///<summary>
  /// Class responsible for working with embedded resources
  ///</summary>
  public class ResourceHelper
  {
    #region Public Methods

    ///<summary>
    /// register the res:// URI scheme
    ///</summary>
    public static void Register()
    {
      WebRequest.RegisterPrefix("res", new ResourceRequestCreator());
    }

    #endregion
  }

  ///<summary>
  /// Request creator class for resources
  ///</summary>
  internal class ResourceRequestCreator : IWebRequestCreate
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
      return new ResourceRequest(uri);
    }

    #endregion
  }

  ///<summary>
  /// Request class for embedded resources
  ///</summary>
  internal class ResourceRequest : WebRequest
  {
    #region Member data

    private readonly string _resource;

    #endregion

    #region Constructors

    ///<summary>
    /// default constructor
    ///</summary>
    ///<param name="uri"></param>
    public ResourceRequest(Uri uri)
    {
      _resource = uri.AbsolutePath.Trim('/', '\\');
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
      return new ResourceResponse(_resource);
    }

    #endregion
  }

  ///<summary>
  /// Response class for embedded resources
  ///</summary>
  internal class ResourceResponse : WebResponse
  {
    #region Member data

    private readonly string _resource;
    private Assembly _assembly;

    #endregion

    #region Constructors

    ///<summary>
    ///default constructor
    ///</summary>
    ///<param name="resource"></param>
    public ResourceResponse(string resource)
    {
      _resource = getFullResourceName(resource);
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
    public override Stream GetResponseStream()
    {
      return PathHelper.GetSafeReadStream(assembly.GetManifestResourceStream(_resource));
    }

    #endregion

    #region Properties

    private Assembly assembly
    {
      get
      {
        if (_assembly == null)
        {
          _assembly = typeof (ResourceHelper).Assembly;
        }
        return _assembly;
      }
    }

    /// <summary>
    /// When overridden in a descendant class, gets or sets the content length of data being received.
    /// </summary>
    /// <returns>
    /// The number of bytes returned from the Internet resource.
    /// </returns>
    /// <exception cref="T:System.NotSupportedException">Any attempt is made to get or set the property, when the property is not overridden in a descendant class. </exception><PermissionSet><IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, ControlEvidence"/></PermissionSet>
    public override long ContentLength
    {
      get
      {
        long length = -1;
        if (_resource == null)
        {
          return length;
        }
        using (Stream stream = assembly.GetManifestResourceStream(_resource))
        {
          if (stream != null)
          {
            length = stream.Length;
          }
        }
        return length;
      }
    }

    #endregion

    #region Private Methods

    private string getFullResourceName(string resource)
    {
      string[] names = assembly.GetManifestResourceNames();
      string result = null;
      foreach (string name in names)
      {
        if (result == null && name.EndsWith(resource, StringComparison.CurrentCultureIgnoreCase))
        {
          result = name;
        }
        if (name.Equals(resource, StringComparison.CurrentCultureIgnoreCase))
        {
          return name;
        }
      }
      return result;
    }

    #endregion
  }
}