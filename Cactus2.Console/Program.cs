using Cactus2.Views;
using Cactus2;

Console.WriteLine("Hello World!");

var p = new Cactus2.Program();
new ConsoleSceneView().Model = p.Scene;
p.StartGame();
