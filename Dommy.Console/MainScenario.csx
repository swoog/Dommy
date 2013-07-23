#r "Dommy.Business.dll"
#r "System.Windows.Forms"

using System;
using System.Windows.Forms;
using Dommy.Business.Syntax;

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

