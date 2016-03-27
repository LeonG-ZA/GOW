using Server;
using System;
using System.Collections.Generic;
using Server.Mobiles;
using Server.Gumps;
using Server.MainConfiguration;

namespace Server.Items
{
    public static class GenerateUnderworldRooms
    {
        public static void Initialize()
        {
            if (MainConfig.GenerateUnderworldRoomsEnabled)
            {
                if (ExperimentalRoomController.Instance == null)
                    Generate();
            }
        }

        public static void Generate()
        {
            ExperimentalRoomController controller = new ExperimentalRoomController();
            controller.MoveToWorld(new Point3D(980, 1117, -42), Map.TerMur);

            //Room 0 to 1
            ExperimentalRoomDoor door = new ExperimentalRoomDoor(Room.RoomZero, DoorFacing.WestCCW);
            ExperimentalRoomBlocker blocker = new ExperimentalRoomBlocker(Room.RoomZero);
            door.Hue = 1109;
            door.MoveToWorld(new Point3D(984, 1116, -42), Map.TerMur);
            blocker.MoveToWorld(new Point3D(984, 1116, -42), Map.TerMur);

            door = new ExperimentalRoomDoor(Room.RoomZero, DoorFacing.EastCW);
            blocker = new ExperimentalRoomBlocker(Room.RoomZero);
            door.Hue = 1109;
            door.MoveToWorld(new Point3D(985, 1116, -42), Map.TerMur);
            blocker.MoveToWorld(new Point3D(985, 1116, -42), Map.TerMur);

            //Room 1 to 2
            door = new ExperimentalRoomDoor(Room.RoomOne, DoorFacing.WestCCW);
            blocker = new ExperimentalRoomBlocker(Room.RoomOne);
            door.Hue = 1109;
            door.MoveToWorld(new Point3D(984, 1102, -42), Map.TerMur);
            blocker.MoveToWorld(new Point3D(984, 1102, -42), Map.TerMur);

            door = new ExperimentalRoomDoor(Room.RoomOne, DoorFacing.EastCW);
            blocker = new ExperimentalRoomBlocker(Room.RoomOne);
            door.Hue = 1109;
            door.MoveToWorld(new Point3D(985, 1102, -42), Map.TerMur);
            blocker.MoveToWorld(new Point3D(985, 1102, -42), Map.TerMur);

            //Room 2 to 3
            door = new ExperimentalRoomDoor(Room.RoomTwo, DoorFacing.WestCCW);
            blocker = new ExperimentalRoomBlocker(Room.RoomTwo);
            door.Hue = 1109;
            door.MoveToWorld(new Point3D(984, 1090, -42), Map.TerMur);
            blocker.MoveToWorld(new Point3D(984, 1090, -42), Map.TerMur);

            door = new ExperimentalRoomDoor(Room.RoomTwo, DoorFacing.EastCW);
            blocker = new ExperimentalRoomBlocker(Room.RoomTwo);
            door.Hue = 1109;
            door.MoveToWorld(new Point3D(985, 1090, -42), Map.TerMur);
            blocker.MoveToWorld(new Point3D(985, 1090, -42), Map.TerMur);

            //Room 3 to 4
            door = new ExperimentalRoomDoor(Room.RoomTwo, DoorFacing.WestCCW);
            blocker = new ExperimentalRoomBlocker(Room.RoomThree);
            door.Hue = 1109;
            door.MoveToWorld(new Point3D(984, 1072, -42), Map.TerMur);
            blocker.MoveToWorld(new Point3D(984, 1072, -42), Map.TerMur);

            door = new ExperimentalRoomDoor(Room.RoomTwo, DoorFacing.EastCW);
            blocker = new ExperimentalRoomBlocker(Room.RoomThree);
            door.Hue = 1109;
            door.MoveToWorld(new Point3D(985, 1072, -42), Map.TerMur);
            blocker.MoveToWorld(new Point3D(985, 1072, -42), Map.TerMur);

            ExperimentalRoomChest chest = new ExperimentalRoomChest();
            chest.MoveToWorld(new Point3D(984, 1064, -37), Map.TerMur);

            ExperimentalBook instr = new ExperimentalBook();
            instr.Movable = false;
            instr.MoveToWorld(new Point3D(995, 1114, -36), Map.TerMur);

            SecretDungeonDoor dd = new SecretDungeonDoor(DoorFacing.NorthCCW);
            dd.ClosedID = 87;
            dd.OpenedID = 88;
            dd.MoveToWorld(new Point3D(1007, 1119, -42), Map.TerMur);

            LocalizedSign sign = new LocalizedSign(3026, 1113407);  // Experimental Room Access
            sign.Movable = false;
            sign.MoveToWorld(new Point3D(980, 1119, -37), Map.TerMur);

            //Puzze Room
            PuzzleBox box = new PuzzleBox(PuzzleType.WestBox);
            box.MoveToWorld(new Point3D(1090, 1171, 11), Map.TerMur);

            box = new PuzzleBox(PuzzleType.EastBox);
            box.MoveToWorld(new Point3D(1104, 1171, 11), Map.TerMur);

            box = new PuzzleBox(PuzzleType.NorthBox);
            box.MoveToWorld(new Point3D(1097, 1163, 11), Map.TerMur);

            XmlSpawner spawner = new XmlSpawner("MagicKey");
            spawner.MoveToWorld(new Point3D(1109, 1150, -12), Map.TerMur);
            spawner.SpawnRange = 0;
            spawner.MinDelay = TimeSpan.FromSeconds(30);
            spawner.MaxDelay = TimeSpan.FromSeconds(45);
            spawner.DoRespawn = true;

            PuzzleBook book = new PuzzleBook();
            book.Movable = false;
            book.MoveToWorld(new Point3D(1109, 1153, -17), Map.TerMur);

            PuzzleRoomTeleporter tele = new PuzzleRoomTeleporter();
            tele.PointDest = new Point3D(1097, 1173, 1);
            tele.MapDest = Map.TerMur;
            tele.MoveToWorld(new Point3D(1097, 1175, 0), Map.TerMur);

            tele = new PuzzleRoomTeleporter();
            tele.PointDest = new Point3D(1098, 1173, 1);
            tele.MapDest = Map.TerMur;
            tele.MoveToWorld(new Point3D(1098, 1175, 0), Map.TerMur);

            MetalDoor2 door2 = new MetalDoor2(DoorFacing.WestCCW);
            door2.Locked = true;
            door2.KeyValue = 50000;
            door2.MoveToWorld(new Point3D(1097, 1174, 1), Map.TerMur);

            door2 = new MetalDoor2(DoorFacing.EastCW);
            door2.Locked = true;
            door2.KeyValue = 50000;
            door2.MoveToWorld(new Point3D(1098, 1174, 1), Map.TerMur);

            Teleporter telep = new Teleporter();
            telep.PointDest = new Point3D(1097, 1175, 0);
            telep.MapDest = Map.TerMur;
            telep.MoveToWorld(new Point3D(1097, 1173, 1), Map.TerMur);

            telep = new Teleporter();
            telep.PointDest = new Point3D(1098, 1175, 0);
            telep.MapDest = Map.TerMur;
            telep.MoveToWorld(new Point3D(1098, 1173, 1), Map.TerMur);

            telep = new Teleporter();
            telep.PointDest = new Point3D(996, 1117, -42);
            telep.MapDest = Map.TerMur;
            telep.MoveToWorld(new Point3D(980, 1064, -42), Map.TerMur);

            Static sparkle = new Static(14138);
            sparkle.MoveToWorld(new Point3D(980, 1064, -42), Map.TerMur);
        }
    }
}