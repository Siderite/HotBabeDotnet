HotBabe.NET v1.5
by Siderite

This application is a rewrite of the HotBabe application for Linux and Windows, but for Windows Forms, in .NET 3.5 C#. Originally, the program would show over your applications a transparent clipart of a sexy girl who would take her clothes off as the processor usage increased.
HotBabe.NET goes a lot further, customizing the images that are loaded, the values at which the images are loaded, the types of values used, the way the transition between images is done, etc.
Latest version, source included, should be available at http://hotbabenet.codeplex.com .
Original application (Linux) site: http://dindinx.net/hotbabe/
Windows port of application: http://hotbabe.planlos.org/

Contact me (Siderite) at: http://siderite.blogspot.com . There is a chat there and everything. I would prefer to discuss HotBabe itself on the codeplex page, though.

============================

Quick start:
1. start the application
2. load a pack (if not already loaded)
3. setup the application via the traybar icon (uncheck Clickthrough, move the image where you want it to appear, set the Opacity and Start with Windows, then check Clickthrough again)

Available packs:
	Original	- the original hot babe clipart stripping based on CPU usage.
	FHMBabes	- a selection of sexy photos changing based on CPU, memory and a random value.
	TakeABreak	- images suggesting you to take a break if you have been working a while on the computer. The images go away after the break time.
	Blossom		- a photo of an orchid opening its three flowers based on CPU, memory and network use.

============================

Long start:
1. start the application
2. open the Advanced -> Settings form from the traybar icon
3. select the monitors that you want to use (monitors give you the values used to show the images)
	- selecting monitors can be done either by pressing the "Add Monitor(s)" button or by dropping a bunch of monitor files from Windows explorer
4. set up the monitor values (key, update time, minimum value, smooth, whatever custom parameter they use, etc)
5. select the images you want to show
	- selecting monitors can be done either by pressing the "Add Image(s)" button, by adding an empty image editor with "Add editor" and then filling it manually or by dropping a bunch of files on the settings form
	- images can be either files on your computer, over the network, over the internet http or ftp as well as files from zip files using the special "zip:///archive.zip?fileName" format
6. set the values at which to show each image
	- values are in the format "Key=value" separated by spaces, where [Key] is the monitor Key and the value is a number from 0 to 100.
	- images will be displayed based on the proximity from any set of values
7. save the settings and watch the images change

All settings are saved in the HotBabe.xml file. All relative paths there are relative to the application, HotBabe.exe.

In order to save your settings in a custom "pack" do the following:
1. save all used images except the default image in the HotBabe folder, preferably in a folder called Images
2. save the default image (used for the icon and at application start) in the HotBabe folder, preferably names HotBabe.png
3. change settings to use the images above
4. create a zip file containing HotBabe.xml, the default image and the Images folder
*5. Using monitors from pack files is not supported, but if you are using your own custom monitor, add it in the pack, preferably in a Monitors folder
*6. A "readme.txt" file should be added if you want to distribute the pack, explaining what it does and what monitors it uses and how the user should extract any custom monitor assemblies from the pack file.

* = optional

============================

Extra features:
- in the Advanced Settings form there is a "Debug Console" button. Use it to see what the application, monitors etc do in the background.
	- adding a "-d" or "-debug" to the command line will show the console option in the main menu.
- if the "Clickthrough" option is not checked, the HotBabe image accepts dropping your own image that will be displayed without change until you set the Blend option to something else than Custom Image.
	- there is no other way of adding that image there except manually editing the XML file, so it's like a bonus for those reading the documentation.


============================

Developers:
1. download the sources
2. Use Visual Studio 2008 to open the .sln file
3. follow the white code :)

Log messages using the HotLogger.Logger class. It's a simple crappy thing now, feel free to improve on it.
All monitor assemblies should reference the Monitors project of assembly and any monitor classes should inherit BaseMonitor.
All other classes like image transitions, settings managers, settings editors and views are not separated into their own projects. I am lazy, I know. However, you can always add them to the projects directly and then contact me to bundle them in the official distribution.
All transition classes should implement IImageTransition, all main views (that includes all functionality to show the images as well as the traybar icon) should implement IMainView and all settings editor views should implement ISettingsEditorView.

As a sideline, I know that the concerns of classes are split everywhere and that the code is a mess, but I plan to work on it after I finish up all my ideas... hmm... so it will probably never happen. But I do feel bad about it!

There is always a TODO.txt file in the solution folder. I am writing there some of the ideas that I think would work. If you want to help out, read it and either implement what is there or give me more ideas to add to it.

===========================

Licencing:
The most permissive licence I could find (without really looking too much) is the MIT licence, therefore HotBabe.NET uses it.
What I really meant is that this application is in the wild and you can do whatever you want with it, as can I. And I can more. So there!

===========================

Thanks:
I want to thank the original developers of the application, who managed to create something so simple and fun that it made me move my lazy ass to recreate it. And then mess it up with useless options.
Also, thanks to everybody who gave feedback for HotBabe.NET. Remember, with more feedback the application will behave more like YOU want it to.

===========================

Future:
Well, there are always ideas that fester in my putrid brain. Here are some examples:
- what should I do with animated images? Right now I only display the first frame and animating them seems like a useless CPU hog
- should I use an WebBrowser control instead of a simple image, so that I can show anything from said animated images to complex interactions?
- what other effects could I add to the already present Opacity?
- can I dock images to the sides of the screen or even fill the screen so that they would frame it?
- how can I make HotBabe more interactive without hogging up the CPU?
