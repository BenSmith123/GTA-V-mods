using System;
using GTA;
using GTA.Math;
using GTA.Native;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Drawing;

using NativeUI;


/**
 * GUIDES
 * 
 * How to's: https://github.com/crosire/scripthookvdotnet/wiki/How-Tos
 * 
 * List of native GTA hash functions: http://www.dev-c.com/nativedb/
 */


namespace ModTemplate
{
    public class ModTemplate : Script
    {
        ScriptSettings config;
        Keys spawnPedKey;

        Ped Player;
        float Radius = 2000; // TODO - is this needed

        // list of all dead peds
        private List<int> PedList = new List<int>();
        private int PedTally = 0;


        public ModTemplate()
        {
            this.Tick += onTick;
            this.KeyDown += onKeyDown;
            this.KeyUp += onKeyUp;

            onStart();
        }

        private void onStart()
        {
            // var myMenu = new UIMenu("Banner Title", "~b~SUBTITLE");
            
            //Notification.Show("hello"); // Despite all the tutorials using UI.notify() - this works
            UI.Notify("hello");
            UI.ShowSubtitle("yo");
            readConfigurationFile();
        }

        private void onTick(object sender, EventArgs eventArgs)
        {
            // this function will execute on every frame!
            // Note - you can change the inverval time

            // Use examples: Checking player wanted level, getting all nearby NPCs

            Player = Game.Player.Character;

            UpdateDeadPeds();
        }

        private void onKeyDown(object sender, KeyEventArgs eventArgs)
        {
            // executed when a key is pressed in game
            
            if (eventArgs.KeyCode == Keys.H)// spawnPedKey) // (eventArgs.KeyCode == Keys.H)
            {
                // Game.Player.Character.Position = new Vector3(0.5f, 0.5f, 0.5f);

                // var npc = World.CreatePed(PedHash.Bevhills01AFM, Game.Player.Character.GetOffsetInWorldCoords(new Vector3(0, 5, 0)));
                //Notification.Show("hello");
                UI.Notify("hello");

                var npc = World.CreatePed(PedHash.JewelSec01, Game.Player.Character.GetOffsetInWorldCoords(new Vector3(0, 5, 0)));

                //npc.Weapons.Give(WeaponHash.Knife, 1, true, true);
                //npc.Task.FightAgainst(Game.Player.Character);


                PedGroup playerGroup = Game.Player.Character.CurrentPedGroup;
                Function.Call(Hash.SET_PED_AS_GROUP_MEMBER, npc, playerGroup); // puts the bodyguard into the players group

                Function.Call(Hash.SET_PED_COMBAT_ABILITY, npc, 100); // 100 = attack
            }
           
        }

        private void onKeyUp(object sender, KeyEventArgs eventArgs)
        {
            // executed when a key is released
        }

        private void spawnVehicle()
        {
            Vehicle vehicle = World.CreateVehicle(VehicleHash.Zentorno, Game.Player.Character.Position + Game.Player.Character.ForwardVector * 3.0f, Game.Player.Character.Heading + 90);
            vehicle.CanTiresBurst = false;
            vehicle.PlaceOnGround();
        }


        private void readConfigurationFile()
        {
            config = ScriptSettings.Load("scripts\\ModSettings.ini");
            spawnPedKey = config.GetValue<Keys>("KeyBindings", "spawnPed", Keys.H);
        }


        private void UpdateDeadPeds()
        {
            Ped[] peds = World.GetNearbyPeds(Player, Radius);
            //for each entity in radius
            for (int i = 0; i < peds.Length; i++)
            {
                //check if its dead and damaged by us but not added previously
                if (peds[i].Exists() && peds[i].HasBeenDamagedBy(Player) && peds[i].IsDead)
                {
                    //check if we have added this dead ped yet
                    if (!PedList.Contains(peds[i].GetHashCode()))
                    {
                        PedTally = PedTally + 1;
                        PedList.Add(peds[i].GetHashCode());

                        // Notification.Show("Kills: " + PedTally.ToString());
                        UI.ShowSubtitle("Kills: " + PedTally.ToString());
                    }
                }
            }
        }

    }
}
