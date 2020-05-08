using GTA;
using System.IO;
using System.Windows.Forms;


namespace VehicleInstantRepair
{
    class Configuration
    {
        private static ScriptSettings config;
        private string configFile = "scripts\\InstantRepair.ini";

        private static string sectionKeys = "KEYS";
        private static string sectionOptions = "OPTIONS";

        // initialise key map defaults
        private static Keys repairKeyDefault = Keys.X;
        private static Keys cleanKeyDefault = Keys.C;

        // keys/options - overridden if there's a config file
        // default all keys
        public Keys RepairKey { get; private set; } = repairKeyDefault;
        public Keys CleanKey { get; private set; } = cleanKeyDefault;

        // default options
        public bool CleanOnRepair = true;
        public bool RepairEngine = true; // can opt out and only fix vehicle cosmetics
        public bool FrontWindowsDown = true; // after repairing vehicle, if the window isn't down the player needs to break the window to shoot again
        public bool PartialCosmeticRepair = false; // only fix the damage to the body SHAPE - leave broken windows/lights and scratches


        public Configuration()
        {
            ReadConfigurationFile();
        }

        // reads the config file in scripts to set keybindings etc.
        private void ReadConfigurationFile()
        {
            if (File.Exists(configFile))
            {
                config = ScriptSettings.Load(configFile);
                RepairKey = config.GetValue<Keys>(sectionKeys, "RepairVehicle", repairKeyDefault);
                CleanKey = config.GetValue<Keys>(sectionKeys, "CleanVehicle", cleanKeyDefault);

                CleanOnRepair = config.GetValue<bool>(sectionOptions, "CleanOnRepair", true);
                RepairEngine = config.GetValue<bool>(sectionOptions, "RepairEngine", true);
                FrontWindowsDown = config.GetValue<bool>(sectionOptions, "FrontWindowsDown", true);
                PartialCosmeticRepair = config.GetValue<bool>(sectionOptions, "PartialCosmeticRepair", false);
            }

            // OPTIONAL - automatically generate a config file template if no file exists?
            // else
            // {
            //     UI.ShowSubtitle("Could not load " + configFile + ", file has now been created in /scripts");
            //     CreateSettingsFile();
            // }
        }

        /*
        // creates a template file for the config & keys
        private void CreateSettingsFile()
        {
            using (StreamWriter sw = File.CreateText(configFile))
            {
                sw.WriteLine("[KeyBindings]");
                sw.WriteLine("spawnPed = J");
                sw.WriteLine("fixVehicle = X; here is a comment"); // TODO
            }
        }
        */
    }
}
