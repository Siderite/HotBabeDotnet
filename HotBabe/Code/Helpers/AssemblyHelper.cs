#region Using directives

using System;
using System.Collections.Generic;
using System.Reflection;
using HotLogger;
using Monitors;

#endregion

namespace HotBabe.Code.Helpers
{
  ///<summary>
  /// Helps with loading and using assemblies
  ///</summary>
  public static class AssemblyHelper
  {
    #region Public Methods

    ///<summary>
    /// Try loading an assembly 
    ///</summary>
    ///<param name="fileName"></param>
    ///<param name="assembly"></param>
    ///<returns></returns>
    public static bool TryLoadAssembly(string fileName, out Assembly assembly)
    {
      if (string.IsNullOrEmpty(fileName))
      {
        assembly = null;
        return false;
      }
      if (!PathHelper.FileExists(fileName))
      {
        assembly = null;
        return false;
      }
      try
      {
        //assembly = Assembly.LoadFile(PathHelper.GetRootedPath(fileName));
        assembly = Assembly.LoadFrom(PathHelper.GetLocalPath(fileName));
        return true;
      }
      catch (Exception ex)
      {
        Logger.Debug("Exception caught and discarded: " + ex.Message +
                     " at AssemblyHelper.TryLoadAssembly");
        assembly = null;
        return false;
      }
    }

    ///<summary>
    /// Get a list of types that are or inherit from T
    ///</summary>
    ///<param name="assembly"></param>
    ///<typeparam name="T"></typeparam>
    ///<returns></returns>
    public static List<Type> GetAssemblyTypes<T>(Assembly assembly)
    {
      Type[] types = assembly.GetTypes();
      List<Type> typesList = new List<Type>();
      foreach (Type type in types)
      {
        if (type.IsSubclassOf(typeof (T)))
        {
          typesList.Add(type);
        }
      }
      return typesList;
    }

    ///<summary>
    /// Tries to get a type by name from an assembly and returns true if it exists
    ///</summary>
    ///<param name="assembly"></param>
    ///<param name="typeName"></param>
    ///<returns></returns>
    public static bool TypeExists(Assembly assembly, string typeName)
    {
      if (assembly == null)
      {
        return false;
      }
      if (String.IsNullOrEmpty(typeName))
      {
        return false;
      }
      return assembly.GetType(typeName) != null;
    }

    ///<summary>
    /// Return true if the file is a path for a monitor assembly
    ///</summary>
    ///<param name="file"></param>
    ///<returns></returns>
    public static bool IsValidMonitorAssembly(string file)
    {
      Assembly assembly;
      if (TryLoadAssembly(file, out assembly))
      {
        List<Type> monitorTypes = GetAssemblyTypes<BaseMonitor>(assembly);
        if (monitorTypes.Count > 0)
        {
          return true;
        }
      }
      return false;
    }

    #endregion
  }
}