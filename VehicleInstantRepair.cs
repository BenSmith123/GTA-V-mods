using System;
using GTA;
using GTA.Math;
using GTA.Native;
using System.Windows.Forms;
using System.Drawing;
using NativeUI;
using System.IO;


namespace VehicleInstantRepair
{
    public class VehicleInstantRepair : Script
    {

        Configuration Config;

        public VehicleInstantRepair()
        {
            Config = new Configuration(); // load from config and set keys
            KeyDown += onKeyDown;

            UI.Notify("script loaded");
        }

        // executed when a key is pressed in game
        private void onKeyDown(object sender, KeyEventArgs eventArgs)
        {
            // if (eventArgs.KeyCode == Config.RepairKey) { RepairVehicle(); }
            if (eventArgs.KeyCode == Config.RepairKey) { RepairVehicle(); }
            if (eventArgs.KeyCode == Config.CleanKey) { damageVehicle(); }
            if (eventArgs.KeyCode == Keys.Z) { spawnVehicle(); }
        }




        private void spawnVehicle()
        {
            Ped player = Game.Player.Character;

            Vehicle vehicle = World.CreateVehicle(VehicleHash.Futo, player.Position + player.ForwardVector * 3.0f, player.Heading + 90);
            
            vehicle.SetMod(VehicleMod.Horns, 1000, false);
            vehicle.CanTiresBurst = false;
            vehicle.RollDownWindows();
            vehicle.NumberPlate = "hello";
            // vehicle.IsInvincible = true;
            vehicle.PlaceOnGround();

            UI.Notify(vehicle.GetHashCode().ToString());
            //vehicle.IsInvincible

            Game.Player.Character.SetIntoVehicle(vehicle, VehicleSeat.Driver);
        }


        private void RepairVehicle()
        {

            Ped player = Game.Player.Character;

            if (player.IsInVehicle())
            {
                Vehicle vehicle = player.CurrentVehicle;

                float engineHealth = vehicle.EngineHealth;

                // Player.CurrentVehicle.EngineHealth = 100; // 1000 = 100%
                // player.CurrentVehicle.IsDriveable = true;
                // player.CurrentVehicle.Health = 1000;
                // player.CurrentVehicle.IsDriveable = true;
                // player.CurrentVehicle.EngineHealth = 10;

                if (Config.CleanOnRepair) { vehicle.DirtLevel = 0; }

                if (!Config.PartialCosmeticRepair)
                {
                    player.CurrentVehicle.Repair(); // full repair - engine health & cosmetics
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


                /*
                player.CurrentVehicle.FixWindow(VehicleWindow.BackLeftWindow);
                player.CurrentVehicle.FixWindow(VehicleWindow.BackRightWindow);
                player.CurrentVehicle.FixWindow(VehicleWindow.FrontLeftWindow);
                player.CurrentVehicle.FixWindow(VehicleWindow.BackRightWindow);
                */

                // Function.Call(Hash.SET_VEHICLE_FIXED)
                // Function.Call(Hash.SET_VEHICLE_DAMAGE, player.CurrentVehicle, 0);
                // player.CurrentVehicle.LeftIndicatorLightOn
                

                UI.Notify(vehicle.FriendlyName + "dirt level: " + vehicle.DirtLevel.ToString());
                // UI.Notify(Player.CurrentVehicle.Acceleration.ToString());
                // UI.Notify(player.CurrentVehicle.DisplayName); // mod spawn name or default name
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
