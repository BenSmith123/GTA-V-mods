using GTA;
using GTA.Native;
using System.Windows.Forms;


// TODO - potential options:
// instant fix tyres or windows only - or option?
// show vehicle name?
// clean player option?
// instant get into any vehicle thats close with engine start


namespace VehicleInstantRepair
{
    public class VehicleInstantRepair : Script
    {

        Configuration Config;

        public VehicleInstantRepair()
        {
            Config = new Configuration(); // load from config and set keys
            KeyDown += onKeyDown;
        }

        // executed when a key is pressed in game
        private void onKeyDown(object sender, KeyEventArgs eventArgs)
        {
            // if (eventArgs.KeyCode == Config.RepairKey) { RepairVehicle(); }
            if (eventArgs.KeyCode == Config.RepairKey) { RepairVehicle(); }
            if (eventArgs.KeyCode == Config.CleanKey) { CleanVehicle(); }
        }


        private void CleanVehicle()
        {
            if (Game.Player.Character.IsInVehicle()) { Game.Player.Character.CurrentVehicle.DirtLevel = 0; }
        }


        private void RepairVehicle()
        {
            Ped player = Game.Player.Character;

            if (player.IsInVehicle())
            {
                Vehicle vehicle = player.CurrentVehicle;
                float engineHealth = vehicle.EngineHealth;

                if (Config.CleanOnRepair) { vehicle.DirtLevel = 0; }

                if (!Config.PartialCosmeticRepair)
                {
                    // full repair - engine health & cosmetics
                    player.CurrentVehicle.Repair();
                } 
                else
                {
                    // repair major deformations - not windows or scratches etc.
                    Function.Call(Hash.SET_VEHICLE_DEFORMATION_FIXED, player.CurrentVehicle, true);
                }

                if (Config.FrontWindowsDown)
                {
                    vehicle.RollDownWindow(VehicleWindow.FrontLeftWindow);
                    vehicle.RollDownWindow(VehicleWindow.FrontRightWindow);
                }

                // apply the previous health before repairing vehicle
                if (!Config.RepairEngine) { vehicle.EngineHealth = engineHealth; }

                // if water damaged, bring back to life!
                if (vehicle.IsDead) { player.CurrentVehicle.EngineRunning = true; }

            }
        }

        /*
        // handy for testing
        private void damageVehicle()
        {
            Ped player = Game.Player.Character;

            if (player.IsInVehicle())
            {
                player.CurrentVehicle.EngineHealth = 1;
                player.CurrentVehicle.DirtLevel = 0;
            }
        }
        */
    }
}
