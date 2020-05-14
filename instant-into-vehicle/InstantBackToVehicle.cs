using System.Windows.Forms;
using GTA;

/**
 * Description
 * - Puts the player back into the vehicle that was just bailed out of!
 */


namespace InstantBackToVehicle
{
    public class InstantBackToVehicle : Script
    {

        Configuration config;

        public InstantBackToVehicle()
        {
            KeyDown += onKeyDown;

            config = new Configuration(); // load from config and set keys
        }


        private void onKeyDown(object sender, KeyEventArgs eventArgs)
        {
            if (eventArgs.KeyCode == config.IntoVehicleKey) { PutPlayerInPreviousVehicle(); }
        }


        private void PutPlayerInPreviousVehicle()
        {
            Ped player = Game.Player.Character;

            // if not in vehicle and a last vehicle exists
            if (!player.IsInVehicle() && Entity.Exists(Game.Player.LastVehicle))
            {
                player.SetIntoVehicle(Game.Player.LastVehicle, VehicleSeat.Driver);
                Game.Player.Character.CurrentVehicle.IsDriveable = true;
            }
        }

    }
}
