# discord-tf2updates
A Discord bot for posting News and Updates from the popular online FPS from Valve, Team Fortress 2.

<img width="438" alt="Screenshot_5" src="https://user-images.githubusercontent.com/43029469/179492201-8e512deb-e360-42e6-87fa-2d159b450991.png">

This project uses Steam's Web API, this fetches news from /ISteamNews/GetNewsForApp/v0002, and converts it into a discord embed which can be posted in any channel by the bot. The Discord bot can be set to run to post updates automatically, or can be asked for updates by running !latest.

The bot has a few commands, including !setchannel, !latest and !help

## !help

  Lists all current commands for the bot.
  
## !latest

  This will post the most recent updates/news for tf2, this can be run in any channel where the bot has permissions. This command can be run without first invoking         !setchannel.

## !setchannel

  This is required to be run before any updates will be pasted, the updates will be posted in the channel where this command is run, you can change channel by running     this command again. The bot will check for updates every minute, and will post if any new updates are found.
  
  Please note: if you set this option and provide your own Steam Web API key, this will run a GET request every minute, so if you are already using your Steam Web API     key for another purpose, this could push you over the daily request limit.
