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

- Put the license at the top.
/*
Copyright 2011 MCForge
Dual-licensed under the Educational Community License, Version 2.0 and
the GNU General Public License, Version 3 (the "Licenses"); you may
not use this file except in compliance with the Licenses. You may
obtain a copy of the Licenses at
http://www.opensource.org/licenses/ecl2.php
http://www.gnu.org/licenses/gpl-3.0.html
Unless required by applicable law or agreed to in writing,
software distributed under the Licenses are distributed on an "AS IS"
BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express
or implied. See the Licenses for the specific language governing
permissions and limitations under the Licenses.
*/