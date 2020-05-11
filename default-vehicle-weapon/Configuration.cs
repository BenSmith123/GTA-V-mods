using GTA;
using System.IO;


namespace DefaultVehicleWeapon
{
    class Configuration
    {

        public string defaultWeapon { get; private set; } = "Unarmed";

        ScriptSettings config;
        private string configFile = "scripts\\DefaultVehicleWeapon.ini";

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
                defaultWeapon = config.GetValue<string>("SETTINGS", "DefaultWeapon", "Unarmed");
            }
        }

    }
}


