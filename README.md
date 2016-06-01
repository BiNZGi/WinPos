# Welcome to WinPos

WinPos is handy tool for Microsoft Windows that can save and restore windows size, position and states through Windows API calls. The data is stored as JSON in %LOCALAPPDATA%\WinPos\WinPos.json file.

## Requirements
* [Microsoft .NET Framework 4 or above](https://www.microsoft.com/en-us/download/details.aspx?id=17851)

This is probably already present with installation of Microsoft Windows. If you are unsure you can use the following in a command line to determine the installed versions:


    wmic product get description | findstr /C:".NET Framework"

## How to use
    WinPos [save] [restore]

| Option  | Description                               |
| ------- | ----------------------------------------- |
| save    | Save current window positions             |
| restore | Restore previously saved window positions |

## Examples
Save the current window positions into JSON file.
    
    WinPos.exe save

Restore the window positions from JSON file.

    WinPos.exe restore

## Technical details

### Save
1. Interate through [EnumWindows](http://www.pinvoke.net/default.aspx/user32.EnumWindows) to get all current window handles.
2. Call to [GetWindowPlacement](http://www.pinvoke.net/default.aspx/user32.getwindowplacement) to get window details.
3. If window is in normal state the position are determined by [GetWindowRect](http://www.pinvoke.net/default.aspx/user32.GetWindowRect). This allows to get the details of windows that are positioned by Windows Snap.    
4. Write JSON file with the following details:
  * Handle
  * Title
  * WindowPlacement
    * Length
    * Flags
    * ShowCmd
    * PtMinPosition
    * PtMaxPosition
    * RcNormalPosition

### Restore
1. Read window data from JSON file according chapter save.
2. Iterate through data.
3. Call [SetWindowPlacement](http://www.pinvoke.net/default.aspx/user32.SetWindowPlacement) using handle with the WindowPlacement details.

Remark: If the window handle has changed somehow the window cannot be found and is not repositioned.

## Third party libraries
* [NuGet Package Json.NET](https://www.nuget.org/packages/newtonsoft.json/) for serialize and deserialize JSON

## License
WinPos is open source under the [MIT License](https://raw.githubusercontent.com/BiNZGi/WinPos/master/LICENSE.md) and is free for commercial use.