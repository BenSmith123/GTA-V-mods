using GTA;
using System.IO;
using System.Windows.Forms;

namespace InstantBackToVehicle
{
    class Configuration
    {
        public Keys IntoVehicleKey { get; private set; } = Keys.E;

        ScriptSettings config;
        private string configFile = "scripts\\InstantBackToVehicle.ini";

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
                IntoVehicleKey = config.GetValue<Keys>("KEYS", "BackToVehicle", Keys.E);
            }
        }

    }
}


