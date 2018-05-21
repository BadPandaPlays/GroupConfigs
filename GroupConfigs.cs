using System;
using System.Collections.Generic;
using System.IO;
using Terraria;
using TerrariaApi.Server;
using TShockAPI;
using System.Linq;

namespace GroupConfigs
{
    [ApiVersion(2, 1)]
    public class GroupConfigs : TerrariaPlugin
    {
        /// <summary>
        /// Gets the author(s) of this plugin
        /// </summary>
        public override string Author => "OneBadPanda";

        /// <summary>
        /// Gets the description of this plugin.
        /// A short, one lined description that tells people what your plugin does.
        /// </summary>
        public override string Description => "A sample test plugin";

        /// <summary>
        /// Gets the name of this plugin.
        /// </summary>
        public override string Name => "Test Plugin";

        /// <summary>
        /// Gets the version of this plugin.
        /// </summary>
        public override Version Version => new Version(1, 0, 0, 0);
        /// <summary>
        /// Initializes a new instance of the GroupConfigs class.
        /// This is where you set the plugin's order and perfrom other constructor logic
        /// </summary>
        public GroupConfigs(Main game) : base(game)
        {
            Order = +5;

        }

        private static string GroupDirectory = "tshock/Groups/";
        private string file = "tshock/Groups/Groups.txt";

        /// <summary>
        /// Handles plugin initialization. 
        /// Fired when the server is started and the plugin is being loaded.
        /// You may register hooks, perform loading procedures etc here.
        /// </summary>
        public override void Initialize()
        {   // Create a Folder
            // Create a file

            List<string> data = new List<string>();
                data.Add("default");
                data.Add("255,255,255");
                data.Add("tshock.canchat");
                data.Add("tshock.account.login");

            if (!Directory.Exists(GroupDirectory))
            {
                Directory.CreateDirectory(GroupDirectory);
                Console.WriteLine("Directory Created");
                if (!File.Exists(file))
                {
                    CreateFile.DoCreateFile(file, data);
                    Console.WriteLine("File Created");
                }
            }

            // Read contents of a file and create groups based on them.
            foreach (string dir in Directory.GetFiles(GroupDirectory))
            {
                Console.WriteLine(dir);
                string thing = Path.GetFullPath(dir);
                Console.WriteLine(thing);
                GroupObject bleh = new GroupObject(thing);
                //if the group doesn't exist, and the parent does, execute.
                Console.WriteLine("Adding Group: " + bleh.Name + " with parent: " + bleh.parent);
                string parent = Path.GetFullPath(bleh.parent);

                if (!TShock.Groups.GroupExists(bleh.Name))
                {
                    // check if parent group exists
                    if (TShock.Groups.GroupExists(bleh.parent.ToString())) {
                        AddGroupFromFile(bleh);
                    }
                    // if parent doesn't exists, but file for it does
                    else if (File.Exists(parent.ToString()))
                    {
                        GroupObject Parent = new GroupObject(parent);
                        AddGroupFromFile(Parent);
                        AddGroupFromFile(bleh);
                    }
                    // TO DO: Make this recursive
                    else
                    {
                    Console.WriteLine(bleh.Name + "has failed to load because it's parent Group " + parent + ", parent's doesn't exist.  Also, creator of this plugin needs to learn how to handle this scenario.");
                    }
                }
            }


        }
        /// <summary>
        /// Handles plugin disposal logic.
        /// *Supposed* to fire when the server shuts down.
        /// You should deregister hooks and free all resources here.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                // Deregister hooks here
            }
            base.Dispose(disposing);
        }
        //HelperFunctions and classes
        public class CreateFile //This is a Constructor?
        {
            public string file { get; set; }
            public List<string> data { get; set; }
            public static void DoCreateFile(string file, List<string> data) //This is a method
            {
                if (!File.Exists(file))
                {
                    File.AppendAllLines(file, data);
                }
            }
        }
        public class GroupObject

        {
            public GroupObject(string file)
            {
                System.IO.StreamReader dis = new System.IO.StreamReader(file);
                Name = Path.GetFileNameWithoutExtension(file);
                parent = dis.ReadLine();
                color = dis.ReadLine();
                do
                {
                    string line;
                    line = dis.ReadLine();
                    try
                    {
                        Perm.Add(line);
                    }
                    catch (System.NullReferenceException) { continue; }
                }
                while (dis.Peek() >= 0);
            }
            public string Name { get; }

            public override string ToString()
            {
                return Name.ToString();
            }
            public string parent { get; set; }
            public string color { get; set; }
            public List<string> Perm = new List<string>();
        }
        public static void AddGroupFromFile(GroupObject file)
        {
            string GroupPermissions = String.Join(",", file.Perm.ToArray());
            TShock.Groups.AddGroup(file.Name, file.parent, GroupPermissions.ToString(), file.color);   
        }

    }

}