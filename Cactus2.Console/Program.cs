using Cactus2.Views;
using Cactus2;

var p = new Cactus2.Program();
var v = new ConsoleSceneView();
v.Model = p.Scene;
p.StartGame();

var t = new Timer(_ =>
{
    v.Update();
}, null, 1000, 2000);
