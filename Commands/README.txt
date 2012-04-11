- Don't put unnecessary using statements.

- Add
p.SendMessage("Shortcut: /[SHORTCUT]");
At the end of the help void for commands. If there are multiple shortcuts, do something like
p.SendMessage("Shortcuts: /[SHORTCUT 1], /[SHORTCUT 2]");
You get the picture.

- Command.AddReference strings need to be all lowercase.

- If the command has no shortcut, Command.AddReference(this, "commandname") can be like that, there's no need to do new string[1] { "commandname" }.

- To add shortcuts just do Command.AddReference(this, new string[3] { "command", "short1", "short2" }).

- < > designates required argument, [ ] designates optional argument