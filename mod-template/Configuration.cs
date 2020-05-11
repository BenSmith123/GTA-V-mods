using GTA;
using System.IO;
using System.Windows.Forms;


namespace ModTemplate
{
    class Configuration
    {
        ScriptSettings config;

        private string configFile = "scripts\\MyModTemplate.ini";

        public int Number { get; set; } = 30;

        // initialise all keys - only this class can modify their values
        public Keys spawnPedKey { get; private set; }
        public Keys fixVehicleKey { get; private set; }

        public Configuration()
        {
            readConfigurationFile();
        }

        // reads the config file in scripts to set keybindings etc.
        private void readConfigurationFile()
        {
            if (File.Exists(configFile))
            {
                config = ScriptSettings.Load(configFile);
                spawnPedKey = config.GetValue<Keys>("KeyBindings", "spawnPed", Keys.H);
                fixVehicleKey = config.GetValue<Keys>("KeyBindings", "fixVehicle", Keys.X);
            }
            else
            {
                UI.ShowSubtitle("Could not load " + configFile + ", file has now been created in /scripts");
                CreateSettingsFile();
            }
        }

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

    }
}


