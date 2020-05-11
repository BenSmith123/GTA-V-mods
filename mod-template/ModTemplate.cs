﻿using System;
using GTA;
using GTA.Math;
using GTA.Native;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Drawing;
using NativeUI;
using System.IO;

/**
 * GUIDES
 * 
 * How to's: https://github.com/crosire/scripthookvdotnet/wiki/How-Tos
 * 
 * NativeUI: https://github.com/Guad/NativeUI/wiki/Getting-Started
 * 
 * List of native GTA hash functions: http://www.dev-c.com/nativedb/
 * 
 * Open source GTA V mods:
 * https://github.com/logicspawn/GTARPG - RPG mod: Massive project, good reference
 * https://github.com/hesa656/TurnScript - Full vehicle control / HUD etc.
 */


namespace ModTemplate
{
    public class ModTemplate : Script
    {
        Ped Player;
        float Radius = 2000; // TODO - is this needed

        // list of all dead peds
        private List<int> PedList = new List<int>();
        private int PedTally = 0;

        UIText textSpeedCurr = new UIText("", new Point(1100, 20), 0.5f);
        UIText textSpeedMax = new UIText("", new Point(1100, 40), 0.5f);

        Configuration config;

        float speedKph = 0;
        float highestSpeedKph = 0;

        public ModTemplate()
        {
            this.Tick += onTick;
            this.KeyDown += onKeyDown;
            this.KeyUp += onKeyUp;

            onStart();
        }

        private void onStart()
        {
            //Notification.Show("hello"); // alternative used in ScriptHookVDotNet3
            UI.Notify("hello");
            UI.ShowSubtitle("yo");

            config = new Configuration(); // load from config and set keys
        }

        

        private void onTick(object sender, EventArgs eventArgs)
        {
            // this function will execute on every frame!
            // Note - you can change the inverval time

            // Use examples: Checking player wanted level, getting all nearby NPCs

            Player = Game.Player.Character;

            if (Player.IsInVehicle()) 
            { 
                speedKph = Player.CurrentVehicle.Speed * 3600 / 1000;
                if (speedKph > highestSpeedKph) highestSpeedKph = speedKph;
            }
            else 
            { 
                speedKph = 0; // reset speed when not in vehicle
            }

            textSpeedCurr.Caption = "Speed: " + Math.Round(speedKph).ToString() + "km";
            textSpeedMax.Caption = "Max: " + Math.Round(highestSpeedKph).ToString() + "km";

            textSpeedCurr.Enabled = true;
            textSpeedMax.Enabled = true;

            textSpeedCurr.Draw();
            textSpeedMax.Draw();

            UpdateDeadPeds();
        }


        // executed when a key is pressed in game
        private void onKeyDown(object sender, KeyEventArgs eventArgs)
        {
            if (eventArgs.KeyCode == config.spawnPedKey) { spawnBodyGuard(); }
            if (eventArgs.KeyCode == config.fixVehicleKey) { spawnVehicle(); }
            if (eventArgs.KeyCode == Keys.C) { fixVehicle(); }

            //if (eventArgs.KeyCode == config.fixVehicleKey) { GetClosestPed(); }

        }

        private void onKeyUp(object sender, KeyEventArgs eventArgs)
        {
            // executed when a key is released
        }

        private void spawnVehicle()
        {
            Vehicle vehicle = World.CreateVehicle(VehicleHash.Comet2, Game.Player.Character.Position + Game.Player.Character.ForwardVector * 3.0f, Game.Player.Character.Heading + 90);
            
            vehicle.SetMod(VehicleMod.Horns, 1000, false);
            vehicle.CanTiresBurst = false;
            vehicle.RollDownWindows();
            vehicle.NumberPlate = "hello";
            vehicle.IsInvincible = true;
            vehicle.PlaceOnGround();
            UI.Notify(vehicle.GetHashCode().ToString());
            //vehicle.IsInvincible
        }

        private void fixVehicle()
        {
            if (Player.IsInVehicle())
            {
                Player.CurrentVehicle.Repair();
                // Player.CurrentVehicle.EngineHealth = 100; // 1000 = 100%
                Player.CurrentVehicle.IsDriveable = true;
                Player.CurrentVehicle.RollDownWindows();
                Player.CurrentVehicle.Health = 1000;
                Player.CurrentVehicle.IsDriveable = true;
                Player.CurrentVehicle.EngineHealth = 1000;
                
                Player.CurrentVehicle.EngineRunning = true;
                UI.Notify(Player.CurrentVehicle.FriendlyName);
                // UI.Notify(Player.CurrentVehicle.Acceleration.ToString());
                // UI.Notify(Player.CurrentVehicle.DisplayName); // mod spawn name or default name


            }
        }


        private void UpdateDeadPeds()
        {
            Ped[] peds = World.GetNearbyPeds(Player, Radius);

            // for each entity in radius
            for (int i = 0; i < peds.Length; i++)
            {
                // check if its dead and damaged by us but not added previously
                if (peds[i].Exists() && peds[i].HasBeenDamagedBy(Player) && peds[i].IsDead)
                {
                    // check if we have added this dead ped yet
                    if (!PedList.Contains(peds[i].GetHashCode()))
                    {
                        PedTally = PedTally + 1;
                        PedList.Add(peds[i].GetHashCode());

                        UI.ShowSubtitle("Kills: " + PedTally.ToString());
                    }
                }
            }
        }


        private void GetClosestPed()
        {

            // if (Player.IsNearEntity(carLast, new Vector3(0, 5, 0))) // if player is x meters from ped

                // Player.IsOnFoot && Player.IsAlive && Player. // TODO - any other validations?
                // get closest ped in front of the player
                // Ped closestPed = World.GetClosestPed(Player.GetOffsetInWorldCoords(new Vector3(0, 5, 0)), 200);

            Ped[] peds = World.GetNearbyPeds(Player, 30); // 10 meter radius of peds

            // Player.ForwardVector

            // sort the list of peds by distance to player
            Array.Sort(peds, delegate (Ped a, Ped b)
            {
                return World.GetDistance(Player.GetOffsetInWorldCoords(Player.ForwardVector), a.GetOffsetInWorldCoords(new Vector3(0, 0, 0)))
                .CompareTo(World.GetDistance(Player.GetOffsetInWorldCoords(Player.ForwardVector), b.GetOffsetInWorldCoords(new Vector3(0, 0, 0))));
            });

            // peds[3].Task.ChatTo(Player);


            // for each entity in radius
            for (int i = 0; i < peds.Length; i++)
            {
                // check if its dead and damaged by us but not added previously
                if (peds[i].Exists() && peds[i].IsAlive)
                {
                    float tempDist = World.GetDistance(Player.GetOffsetInWorldCoords(Player.ForwardVector), peds[i].GetOffsetInWorldCoords(new Vector3(0, 0, 0)));

                    UI.Notify("distance: " + tempDist.ToString());
                    // peds[i].Task.ChatTo(Player);

                    // peds[i].Weapons.Give(WeaponHash.Knife, 1, true, true);
                    peds[i].IsEnemy = false;
                    peds[i].Task.ClearAllImmediately();
                    peds[i].Task.ClearSecondary();
                    peds[i].RelationshipGroup = Player.RelationshipGroup;

                    // peds[i].Task.FightAgainstHatedTargets


                    Function.Call(Hash.SET_PED_AS_GROUP_MEMBER, peds[i], Player.CurrentPedGroup); // puts the bodyguard into the players group
                    // Function.Call(Hash.flee,false);
                    Function.Call(Hash.SET_PED_COMBAT_ABILITY, peds[i], 100); // 100 = attack
                    //Function.Call(Hash.set_player, false);
                    break;
                }
            }
        }


        private void spawnBodyGuard()
        {
            // Game.Player.Character.Position = new Vector3(0.5f, 0.5f, 0.5f); // teleport player

            UI.Notify("Spawning body guard");
            // config.Number = 90;
            // UI.Notify(config.Number.ToString());

            var npc = World.CreatePed(PedHash.JewelSec01, Game.Player.Character.GetOffsetInWorldCoords(new Vector3(0, 5, 0)));

            //npc.Weapons.Give(WeaponHash.Knife, 1, true, true);
            npc.Task.FightAgainst(Game.Player.Character);

            PedGroup playerGroup = Game.Player.Character.CurrentPedGroup;
            // Function.Call(Hash.SET_PED_AS_GROUP_MEMBER, npc, playerGroup); // puts the bodyguard into the players group
            Function.Call(Hash.SET_PED_COMBAT_ABILITY, npc, 100); // 100 = attack
        }

    }
}
