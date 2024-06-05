using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Littlehero.Scripts.view
{
    public partial class GameView: Control
    {
        public void OnStartButtonPressed()
        {
            GetTree().ChangeSceneToFile("res://Level02.tscn");
        }
        public void OnExitButton()
        {
            GetTree().Quit();
        }
    }
}
