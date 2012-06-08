##MCForge Console Interface
In terminal or command line, MCForgeConsole.exe <argument>
Arguments: 
* `Load-Plugin` - Loads plugins on startup, the very first thing it does. Great if your plugin changing internal settings before it sets up.
* `debug` - Sets the server in debug mode
* `abort-setup` - skips setting up the server setttings

In console, you can send messages or run commands.
Console only commands:
* `!stop` - breaks the input stream, allowing nothing to be read after this. **Note: this does not stop server, as a result, a hard stop will be required. Which is not recommended**
* `!copyurl` - Copies the URL of the server to the clipboard.
* `!packets` - Starts logging all incoming packets
	- `stop` - Stops logging all incoming packets
	- `hide <Packet id (byte)>` - hides logging of all that type of packet
	- `unhide <Packet id (byte)>` - Unhides logging of all of that type of packet. (bad grammar, I know)
	- `cancel <Packet id (byte)>` - Blocks all incomming packets of that type.
	- `allow <Packet id (byte)>` - Allows all incoming packets of that type.