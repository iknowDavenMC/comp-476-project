﻿using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using StreakerLibrary;

namespace COMP476Proj
{
    public class Map
    {
        /// <summary>
        /// PLACEHOLDER. Should be redesigned properly when pathfinding is done
        /// </summary>
        public class Node
        {
            public int id;
            public Vector2 position;
            public List<Node> connected;
            public Node(float x, float y, int id)
            {
                this.id = id;
                position = new Vector2(x, y);
                connected = new List<Node>();
            }
        }

        public List<NPC> startingNPCs;
        public List<Wall> walls;
        public List<Node> nodes;
        public Vector2 playerStart;
        private Random r;
        public Map()
        {
            startingNPCs = new List<NPC>();
            walls = new List<Wall>();
            nodes = new List<Node>();
            r = new Random();
        }

        public void Load(string filename)
        {
            walls.Clear();
            nodes.Clear();
            startingNPCs.Clear();
            playerStart = new Vector2();

            StreamReader reader = new StreamReader(filename);
            try
            {
                string line = reader.ReadLine();
                if (line.StartsWith("RECTANGLES"))
                    line = reader.ReadLine();
                do
                {
                    int x, y, w, h;
                    int spacei = line.IndexOf(' ');
                    x = int.Parse(line.Substring(0, spacei));
                    line = line.Substring(spacei + 1);
                    spacei = line.IndexOf(' ');
                    y = int.Parse(line.Substring(0, spacei));
                    line = line.Substring(spacei + 1);
                    spacei = line.IndexOf(' ');
                    w = int.Parse(line.Substring(0, spacei));
                    line = line.Substring(spacei + 1);
                    h = int.Parse(line);
                    walls.Add(new Wall(new Vector2(x,y), new BoundingRectangle(x, y, w, h)));
                    line = reader.ReadLine();
                } while (reader.Peek() != -1
                    && !line.StartsWith("NODES")
                    && !line.StartsWith("EDGES")
                    && !line.StartsWith("NPCS")
                    && !line.StartsWith("PLAYER")
                    );

                if (line.StartsWith("NODES"))
                    line = reader.ReadLine();
                do
                {
                    int id, x, y;
                    int spacei = line.IndexOf(' ');
                    id = int.Parse(line.Substring(0, spacei));
                    line = line.Substring(spacei + 1);
                    spacei = line.IndexOf(' ');
                    x = int.Parse(line.Substring(0, spacei));
                    line = line.Substring(spacei + 1);
                    y = int.Parse(line);
                    nodes.Add(new Node(x, y, id));
                    line = reader.ReadLine();
                } while (reader.Peek() != -1
                    && !line.StartsWith("EDGES")
                    && !line.StartsWith("NPCS")
                    && !line.StartsWith("PLAYER")
                    );

                if (line.StartsWith("EDGES"))
                    line = reader.ReadLine();
                do
                {
                    int sid, eid;
                    int spacei = line.IndexOf(' ');
                    sid = int.Parse(line.Substring(0, spacei));
                    line = line.Substring(spacei + 1);
                    eid = int.Parse(line);
                    Node n1 = nodes.Find(n => n.id == sid);
                    Node n2 = nodes.Find(n => n.id == eid);
                    n1.connected.Add(n2);
                    n2.connected.Add(n1);
                    line = reader.ReadLine();
                } while (reader.Peek() != -1
                    && !line.StartsWith("NPCS")
                    && !line.StartsWith("PLAYER")
                    );

                if (line.StartsWith("NPCS"))
                    line = reader.ReadLine();
                do
                {
                    int x, y;
                    string type, mode;
                    int spacei = line.IndexOf(' ');
                    x = int.Parse(line.Substring(0, spacei));
                    line = line.Substring(spacei + 1);
                    spacei = line.IndexOf(' ');
                    y = int.Parse(line.Substring(0, spacei));
                    line = line.Substring(spacei + 1);
                    spacei = line.IndexOf(' ');
                    type = line.Substring(0, spacei);
                    line = line.Substring(spacei + 1);
                    mode = line;
                    NPC npc = null;
                    if (type.StartsWith("Civilian"))
                    {
                        int pnum = r.Next(1, 4);
                        string pedAnim = "student" + pnum + "_static";
                        Animation a = SpriteDatabase.GetAnimation("cop_static");
                        PedestrianState pstate = PedestrianState.WANDER;
                        if (mode.StartsWith("Static"))
                            pstate = PedestrianState.STATIC;
                        npc = new Pedestrian(
                            new PhysicsComponent2D(new Vector2(x, y), 0, new Vector2(20, 20), 150, 750, 75, 1000, 8, 50, 0.25f, true),
                            new MovementAIComponent2D(),
                            new DrawComponent(SpriteDatabase.GetAnimation("student"+pnum+"_static"), Color.White, Vector2.Zero, new Vector2(.4f, .4f), .5f),
                            pstate
                            );
                    }
                    // The cops are not in, so they are ignored for now
                    if (type.StartsWith("DumbCop"))
                    {
                    }
                    if (type.StartsWith("SmartCop"))
                    {
                    }
                    if (npc!=null)
                        startingNPCs.Add(npc);
                    line = reader.ReadLine();
                }
                while (reader.Peek() != -1
                    && !line.StartsWith("PLAYER")
                    );

                if (line.StartsWith("PLAYER"))
                    line = reader.ReadLine();
                do
                {
                    int x, y;
                    int spacei = line.IndexOf(' ');
                    x = int.Parse(line.Substring(0, spacei));
                    line = line.Substring(spacei + 1);
                    spacei = line.IndexOf(' ');
                    y = int.Parse(line);
                    playerStart = new Vector2(x, y);
                    line = reader.ReadLine();
                } while (reader.Peek() != -1);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine(e.StackTrace);
            }
        }
    }
}