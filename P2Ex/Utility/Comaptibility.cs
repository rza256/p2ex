using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P2Ex.Utility
{
    internal class Comaptibility
    {
        static string[] supportedCameras = { "AG-HVX200P" };

        public static bool IsCameraSupported(string cameraModel)
        {
            return Array.Exists(supportedCameras, model => model.Equals(cameraModel, StringComparison.OrdinalIgnoreCase));
        }
    }
}
