#r "Dommy.Business.dll"
#r "Dommy.Extensions.Kinect.dll"
#r "System.Windows.Forms"
#r "System.Drawing"

using System;
using System.Drawing;
using System.Windows.Forms;
using Dommy.Business;
using Dommy.Business.Syntax;
using Dommy.Business.Scenarios;
using Dommy.Business.Syntax;
using Dommy.Business.Tools;
using Dommy.Extensions.Kinect;

//Scenario.CreateFrom<UpdateScenarioDescription>();

Scenario.Create()
		.SpeechTrigger("redemarre")
		.TileTrigger("PC", "Redémare Dommy", TileColor.Victoria)
		.Say("Redémarage", "Je redémare", "a dans 2 secondes")
		.Action(() =>
		{
			Application.Restart();
			Environment.Exit(0);
			return true;
		})
		.Start();

Scenario.Create()
		.TileTrigger("PC", "Redémarre le PC", TileColor.Victoria)
		.SpeechTrigger("redemarre le pc")
		.Say("Redémarage du PC", "Je redémare le PC")
		.Command("ShutDown", "/t 0 /r")
		.Start();

Scenario.CreateFrom<RecodSkeletonScenarioDescription>();

Scenario.Create("Kinect Test")
		.KinectTrigger<HelloSkeletonCheck>()
		.Say("Hello")
		.Start();

Scenario.Create()
		.TileTrigger("A live", "Autre", TileColor.Victoria)
		.TileTrigger("A live", "Autre", TileColor.Victoria)
		.TileTrigger("A live", "Autre", TileColor.Victoria)
		.TileTrigger("A live", "Autre", TileColor.Victoria)
		.TileTrigger("A live", "Autre", TileColor.Victoria)
		.TileTrigger("A live", "Autre", TileColor.Victoria)
		.TileTrigger("A live", "Autre", TileColor.Victoria)
		.TileTrigger("A live", "Autre", TileColor.Victoria)
		.TileTrigger("A live", "Autre", TileColor.Victoria)
		.TileTrigger("A live", "Autre", TileColor.Victoria)
		.TileTrigger("A live", "Autre", TileColor.Victoria)
		.Say("A live")
		.Start();
