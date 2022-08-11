# discord-tf2updates
A Discord bot for posting News and Updates from the popular online FPS by Valve, Team Fortress 2.

<img width="438" alt="Screenshot_5" src="https://user-images.githubusercontent.com/43029469/179492201-8e512deb-e360-42e6-87fa-2d159b450991.png">

This project uses Steam's Web API, this fetches news from /ISteamNews/GetNewsForApp/v0002, and converts it into a discord embed which can be posted in any channel by the bot. The Discord bot can be set to run to post updates automatically, or can be asked for updates by running /latest-updates.

## Commands

### /latestupdates

  Post the latest news and updates.

### /setchannel

  Sets the channel where the latest news and updates will be posted.
  
### /removechannel

  Removes a channel so it will no longer receive news and updates.

## Permissions

  You can either use the link provided here: [Invite](https://discord.com/api/oauth2/authorize?client_id=995286897758830632&permissions=2147502080&scope=bot%20applications.commands)

  ### Current required permissions
  
  In case you wanted to set permissions manually:

  Permissions Integer: 2147502080

  - [x] bot
    - [x] Send Messages
    - [x] Embed Links
    - [x] Use Slash Commands
  - [x] applications.commands

## Configuration

  There's 2 configuration files included in ./Templates, AppConfig and SlashCommands.

  ### AppConfig

  - DiscordToken
    - Discord token for your Bot.
  - SteamToken
    - Token for Steams API, note this bot only uses the free public API.
  - PollingRateInSecs
    - How often the application will check Steam's Web API for updates.
  - SteamWebApiUri
    - The base URI of the Steam Web API, this will be "https://api.steampowered.com" by default.
  - NewsEndpoint
    - Endpoint for Steam App News Interface, this will be "/ISteamNews/GetNewsForApp/v0002" by default.
  - AppId
    - The ID of the Game/Application to request, this will be 440 by default. (440 is the AppId of Team Fortress 2.)
  - Feeds
    - The Feeds to fetch to request, this will be "tf2_blog" by default. There are other feeds, these are different for each AppId and are based on the author of th post.
  - DeveloperMode
    - This will enable more verbose logging. if set to true.
  - DeveloperGuildId
    - Required if you have one or more commands which use guild commands. guild commands for this application are only used for Development Purposes.

  ### SlashCommands