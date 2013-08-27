#r "Dommy.Business.dll"
#r "System.Windows.Forms"
#r "System.Drawing"

using System;
using System.Drawing;
using System.Windows.Forms;
using Dommy.Business.Syntax;
using Dommy.Business.Scenarios;
using Dommy.Business.Syntax;
using Dommy.Business.Tools;

Scenario.CreateFrom<OnOffLightScenarioDescription>()
	.With(c => c.RoomNames, new[] {
						new RoomName("la salle", "de la salle", Gender.Female),
						new RoomName("le salon", "du salon",Gender.Male),
						new RoomName("la salle à manger", "de la salle à manger", Gender.Female),
						new RoomName("la pièce à vivre", "de la pièce à vivre", Gender.Female),
					})
	.With(c => c.EedomusId, "11224");


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


Scenario.Create()
		.TileTrigger("A live", Color.FromName("Blue"))
		.TileTrigger("A live", Color.FromName("Blue"))
		.TileTrigger("A live", Color.FromName("Blue"))
		.TileTrigger("A live", Color.FromName("Blue"))
		.TileTrigger("A live", Color.FromName("Blue"))
		.TileTrigger("A live", Color.FromName("Blue"))
		.TileTrigger("A live", Color.FromName("Blue"))
		.TileTrigger("A live", Color.FromName("Blue"))
		.TileTrigger("A live", Color.FromName("Blue"))
		.Say("A live")
		.Start();
