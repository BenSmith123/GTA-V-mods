using System;
using GTA;
using System.Collections.Generic;
using System.Windows.Forms;
using System.IO;
using System.Linq;
using NativeUI;
using System.Drawing;
using GTA.Native;

/**
 * In-game menu UI to to enable/disable mods
 * - Gets all files from the GTAV/scripts folder
 * - Creates a toggle menu item for each mod script
 * - If an item is set to disabled, move the script
 *   into a temporary /disabled folder and reload scripts (manually)
 *   
 *   
 *   
 * TODO:
 *  - Custom menu graphic: https://github.com/Guad/NativeUI/wiki/Custom-Banners
 *    another example https://github.com/root-cause/v-spoutfits/blob/master/SPOutfits/Main.cs
 *   
 *  Menus! - good code ref: https://github.com/Guad/NativeUI/blob/master/MenuExample/MenuExample.cs
 * 
 */


namespace ScriptManager
{
    public class ScriptManager : Script
    {

        private MenuPool _menuPool;
        UIMenu myMenu = new UIMenu("Banner Title", "~b~SUBTITLE");

        public ScriptManager()
        {
            KeyDown += onKeyDown;
            Tick += onTick;

            UI.Notify("Script manager loaded");

            _menuPool = new MenuPool();
            // var mainMenu = new UIMenu("Native UI", "~b~NATIVEUI SHOWCASE");

            UIMenuColoredItem myItem = new UIMenuColoredItem("Simple Button", Color.White, Color.Black);
            myItem.TextColor = Color.Red;
            myItem.HighlightedTextColor = Color.Yellow;

            myMenu.AddItem(myItem);
            
            myMenu.AddItem(new UIMenuCheckboxItem("Simple Checkbox", false));
            myMenu.AddItem(new UIMenuListItem("Simple List", new List<dynamic> { "Item 1", "Item 2", "Item 3" }, 0));
            myMenu.AddItem(new UIMenuItem("Another Button", "Items can have descriptions too!"));

            _menuPool.Add(myMenu);

            //Sprite sprite = new Sprite("shopui_title_graphics_michael", "shopui_title_graphics_michael", Point.Empty, Size.Empty);
            //myMenu.SetBannerType(sprite);

            // var banner = new UIResRectangle(Point.Empty, Size.Empty, Color.FromArgb(255, 255, 255, 255));
            //banner.Color = Color.FromArgb(255, 255, 255, 255);
            // myMenu.SetBannerType(banner);

            //myMenu.SetBannerType(banner);

            _menuPool.RefreshIndex();


            KeyDown += (o, e) =>
            {
                if (e.KeyCode == Keys.F5) // Our menu on/off switch
                    myMenu.Visible = !myMenu.Visible;
            };

            // myMenu.OnItemSelect += ItemSelectHandler;

        }

        private void onTick(object sender, EventArgs eventArgs)
        {
            _menuPool.ProcessMenus();

            if (Game.Player.Character.IsInVehicle())
            {
                //Game.Player.Character.CurrentVehicle
                //Game.Player.Character.Weapons.Select();
                // Game.Player.Character.Weapons.Give();
                //Weapon none = Weapon;

                //Function.Call(Hash.SET_CURRENT_PED_WEAPON, Game.Player.Character, GET_HASH_KEY("WEAPON_UNARMED"));
                
            }

            
        }

        public void ItemSelectHandler(UIMenu sender, UIMenuItem selectedItem, int index)
        {
            UI.Notify("You have selected: ~b~" + selectedItem.Text);
        }

        private void onKeyDown(object sender, KeyEventArgs eventArgs)
        {
            if (eventArgs.KeyCode == Keys.E) { UpdateModFiles(); }
            if (eventArgs.KeyCode == Keys.H) { UI.Notify("hello"); myMenu.GoDown(); } //  myMenu.Visible = !myMenu.Visible;
            if (eventArgs.KeyCode == Keys.Y) { myMenu.Visible = !myMenu.Visible; }
        }


        private void UpdateModFiles()
        {
            string scriptsPath = Path.Combine(Directory.GetCurrentDirectory(), "scripts");
            string scriptsPathDisabled = Path.Combine(scriptsPath, "disabled");

            // if (!File.Exists(scriptsPathDisabled)) { using (FileStream fs = File.Create(scriptsPathDisabled)) { } }

            string[] files = Directory.GetFiles(scriptsPath);

            List<string> scriptList = files.ToList();

            UI.Notify(Path.Combine(scriptsPath, "disabled", scriptList[0]));

            // File.Move(Path.Combine(scriptsPath, scriptList[0]), Path.Combine(scriptsPathDisabled, Path.GetFileName(scriptList[0])));

        }


        private void Alert()
        {

            string scriptsPath = Path.Combine(Directory.GetCurrentDirectory(), "scripts");
            UI.ShowSubtitle(Path.Combine(scriptsPath, "ass.txt"));

            File.Delete(Path.Combine(scriptsPath, "ass.txt"));

            UI.ShowSubtitle("Testing file deletion..");
        }


    }
}
