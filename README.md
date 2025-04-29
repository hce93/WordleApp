# WordleApp
Simple wordle style app. Contains c.1,300 words to guess and a dictioanry of c.15,000 valid words a user can input to guess the word.

Uses KeyListener to accept keyboard input from the user. I have modified the plugin to allow the enter key to be recognised, which it wasn't on the current download as of 14/10/2024. What is changed is as follows:
    - Removed android as target framework (as this app isn't built for android)
    - Commented out the string.IsNullOrWhiteSpace check in UIPressesData.cs to allow the enter key to be recognised for mac catalyst. Original line is as follows

    if (press.Key is UIKey key && !string.IsNullOrWhiteSpace(key.CharactersIgnoringModifiers))

Link to plugin:
    - https://github.com/davidortinau/Plugin.Maui.KeyListener
    - run-> dotnet add package Plugin.Maui.KeyListener --version 1.0.0-preview1


Currently tested for mac and iphone 16