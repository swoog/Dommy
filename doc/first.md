First Scenario
==============

Dommy is an home automation server who used C# script to create scenarios.

What a scenario
---------------

A scenario is composed of 2 parts :
- One or more triggers
- One or more actions

Hello World scenario
--------------------

1) Create a file named *hello.csx* in directory "scripts/" 

2) In this file add the line : 
```csharp
#r "Dommy.Business.dll"
```
3) To begin the scenario add the line
```csharp
Scenario.Create("Hello World") 
```
4) Specify a SpeechTrigger
```csharp
.SpeechTrigger("coucou") 
```
5) Specify an action
```csharp
.Say("coucou", "salut") 
```
6) Start the scenario
```csharp
.Start() 
```

Complete scenario :
```csharp
 Scenario.Create("Hello World") 
  .SpeechTrigger("coucou") 
  .Say("coucou", "salut") 
  .Start() 

```

With this scenario when you say "coucou" Dommy say randomly "coucou" or "salut".

