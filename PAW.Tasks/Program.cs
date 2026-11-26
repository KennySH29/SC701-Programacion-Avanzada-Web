// See https://aka.ms/new-console-template for more information
using PAW.Tasks;

Console.WriteLine("Hello, World!");

//Sync.Execute(100, "BBB").Wait();
Sync.Execute(100, "AAA");
Sync.Execute(100, "BBB").Wait();
Sync.Execute(100, "CCC");
Sync.Execute(100, "XXX");

Console.ReadLine();
Console.ReadKey();




