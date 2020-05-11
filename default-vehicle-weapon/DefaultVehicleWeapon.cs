using System;
using GTA;
using GTA.Native;

/**
 * Description
 * - Default weapon when entering a vehicle (unarmed unless config specifies something else)
 * 
 * TODO
 * - Option to press button and set weapon?
 * - set weapon back to the weapon they had before entering vehicle
 */


namespace DefaultVehicleWeapon
{
    public class DefaultVehicleWeapon : Script
    {

        Configuration config;

        private bool hasSetDefault = false; // trigger for getting in/out of a vehicle and not looping

        public DefaultVehicleWeapon()
        {
            this.Tick += onTick;

            Interval += 100;

            config = new Configuration(); // load from config and set keys
        }


        private void onTick(object sender, EventArgs eventArgs)
        {
            Ped Player = Game.Player.Character;

            if (Player.IsInVehicle())
            {
                if (!hasSetDefault) // if in vehicle but haven't changed weapon
                {
                    // get the selected weapon and create a WeaponHash type of it
                    WeaponHash selectedWeapon = (WeaponHash)Enum.Parse(typeof(WeaponHash), config.defaultWeapon);

                    // if the weapon specified by the user is valid (exists in the WeaponHash)
                    if (Enum.IsDefined(typeof(WeaponHash), config.defaultWeapon))
                    {
                        Game.Player.Character.Weapons.Select(Game.Player.Character.Weapons[selectedWeapon]);
                        hasSetDefault = true;
                    }
                }
            }
            else
            {
                hasSetDefault = false;
                // TODO - set weapon back to the weapon they had before entering vehicle?
            }
        }

    }
}
