// (C) Copyright 2011 by Autodesk, Inc. 
//
// Permission to use, copy, modify, and distribute this software
// in object code form for any purpose and without fee is hereby
// granted, provided that the above copyright notice appears in
// all copies and that both that copyright notice and the limited
// warranty and restricted rights notice below appear in all
// supporting documentation.
//
// AUTODESK PROVIDES THIS PROGRAM "AS IS" AND WITH ALL FAULTS. 
// AUTODESK SPECIFICALLY DISCLAIMS ANY IMPLIED WARRANTY OF
// MERCHANTABILITY OR FITNESS FOR A PARTICULAR USE.  AUTODESK,
// INC. DOES NOT WARRANT THAT THE OPERATION OF THE PROGRAM WILL
// BE UNINTERRUPTED OR ERROR FREE.
//
// Use, duplication, or disclosure by the U.S. Government is
// subject to restrictions set forth in FAR 52.227-19 (Commercial
// Computer Software - Restricted Rights) and DFAR 252.227-7013(c)
// (1)(ii)(Rights in Technical Data and Computer Software), as
// applicable.
//

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autodesk.Revit;
using System.Diagnostics;
using System.IO;
using System.Windows.Media;
using System.Windows.Forms;
using System.Windows.Media.Imaging;
using Autodesk.Revit.UI;
using Autodesk.Revit.DB;
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.UI.Events;

namespace ADNPlugin.Revit.FileUpgrader
{
  class Ribbon: IExternalApplication
  {

    // ExternalCommands assembly path
    static string AddInPath = 
      typeof(Ribbon).Assembly.Location;

    #region IExternalApplication Members

    Result IExternalApplication.OnShutdown(
      UIControlledApplication application)
    {
      return Autodesk.Revit.UI.Result.Succeeded;
    }

    Result IExternalApplication.OnStartup(
      UIControlledApplication application)
    {
      try
      {
        // create a Ribbon panel which contains three 
        // stackable buttons and one single push button

        string firstPanelName = "Upgrader";
        RibbonPanel panel = application.CreateRibbonPanel(
          firstPanelName);

        // set the information about the command we will 
        // be assigning to the button 

        PushButtonData pushButtonData = new PushButtonData(
          "FileUpgrader", 
          "File Upgrader", 
          AddInPath, 
          "ADNPlugin.Revit.FileUpgrader.Command");

        //' add a button to the panel 

        PushButton pushButton = panel.AddItem(pushButtonData)
          as PushButton;

        //' add an icon 

        pushButton.LargeImage = LoadPNGImageFromResource(
        "ADNPlugin.Revit.FileUpgrader.upgrade_32by32.png");

        // add a tooltip 
        pushButton.ToolTip = 
          "Upgrade old version Revit documents to current one.";

        // long description

        pushButton.LongDescription =
             "Specify the Source folder that contains a set of Revit files, " + 
             "and Destination folders where you want to save upgraded files.";

        // Context (F1) Help - new in 2013 
        //string appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData); // %AppData% 

        string path;
        path = System.IO.Path.GetDirectoryName(
           System.Reflection.Assembly.GetExecutingAssembly().Location);

        ContextualHelp contextHelp = new ContextualHelp(
            ContextualHelpType.ChmFile,
            path + "/Resources/ADNFileUpgraderHelp.htm"); // hard coding for simplicity. 

        pushButton.SetContextualHelp(contextHelp);

        return Autodesk.Revit.UI.Result.Succeeded;
      }
      catch (Exception ex)
      {
        MessageBox.Show(ex.ToString(), "File Upgrader Ribbon");
        return Autodesk.Revit.UI.Result.Failed;
      }
    }

    /// <summary>
    /// Load the bitmap from resource. The resource image 
    /// 'Build action' must be set to 'Embeded resource'.
    /// PNG extension supports transparency.
    /// </summary>
    /// <param name="imageResourceName">Resource name .PNG</param>
    /// <returns>The loaded image</returns>
    private static System.Windows.Media.ImageSource
      LoadPNGImageFromResource(string imageResourceName)
    {
      System.Reflection.Assembly dotNetAssembly =
        System.Reflection.Assembly.GetExecutingAssembly();
      System.IO.Stream iconStream =
        dotNetAssembly.GetManifestResourceStream(imageResourceName);
      System.Windows.Media.Imaging.PngBitmapDecoder bitmapDecoder =
        new System.Windows.Media.Imaging.PngBitmapDecoder(iconStream,
          System.Windows.Media.Imaging.BitmapCreateOptions.
          PreservePixelFormat, System.Windows.Media.Imaging.
          BitmapCacheOption.Default);
      System.Windows.Media.ImageSource imageSource =
        bitmapDecoder.Frames[0];
      return imageSource;
    }


    #endregion
  }
}
