#r "Dommy.Business.dll"
#r "System.Windows.Forms"
#r "System.Drawing"

using System;
using System.Drawing;
using System.Windows.Forms;
using Dommy.Business.Syntax;
using Dommy.Business.Action;
using Dommy.Business.Scenarios;
using Dommy.Business.Syntax;
using Dommy.Business.Tools;

Scenario.CreateFrom<UpdateScenarioDescription>();

Scenario.Create()
		.SpeechTrigger("redemarre")
		.Say("Redémarage", "Je redémare", "a dans 2 secondes")
		.Action(() =>
		{
			Application.Restart();
			Environment.Exit(0);
			return true;
		})
		.Start();

Scenario.Create()
		.SpeechTrigger("redemarre le pc")
		.Say("Redémarage du PC", "Je redémare le PC")
		.Command("ShutDown", "/t 0 /r")
		.Start();

//Scenario.Create()
//		.TimeTrigger(DateTime.Now, TimeSpan.FromSeconds(10))
//		.TileTrigger("A live", Color.FromName("Blue"))
//		.Say("A live")
//		.Start();