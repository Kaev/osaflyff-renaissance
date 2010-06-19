luabin:AddValue(1,0);
luabin:AddValue(0,1);
luabin:AddValue(40,9);

pak:StartNewMergedPacketLUA(MyMoverID, 0x0140);
pak:Addbyte(0);
pak:Addbyte(10);
pak:Addlong(luabin.qword);
