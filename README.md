# BotGear

How to make a Bot:
After learn how to create a bot on discord , uysing the discord.net library
 Just use the Hellobot solution and add your changes.. .
 
 How to make plugins jsut create a .net dll with a file like "botcommands" in the hellobot solution and sjut add this attributes in assembly.cs file 
 [assembly: ModuleInfoAssemblyWebSite(" yourweb site here")]
[assembly: ModuleInfoAssemblySourceCode("tyour coderepo in case the bot plugin is opne source ")]
[assembly: BotPlugin(true)]
and compile it.



Packages you need for both:
Packages :
Discord.net (everyting)
System.ComponentModel.Composition ( it exists in the .net framework)
BotGer either in source code or in dll
Entity Framework
and everythign it's referenced in hellobot solution.
