--------------------------------------------------------------------------------------------------------------------
------------------------------------------   General Quest template   -------------------------------------------
--------------------------------------------------------------------------------------------------------------------
-- Variable declaration : those variable are local variable used un the function below. Change their value
-- to correspond to your quest
questName="Yorrn le mignon petit chiot"; 		--Name of the quest must be a string alue
questID				=  126;                     -- id of the quest must be an int value
startNPCName		= "MaFl_Loyah";             -- npc that start the state of the quest (for each state you must have a sart and end npc they can be different)
endNPCName			= "MaFl_Loyah";				-- Name of the NPC that finish the quest or the state ofd the quest
questExp		    = 0;						-- Exeperience given by the quest
questPenya			= 15000;					-- Penya given by the quest
questMinLvl			= 5;						-- Level minimum required to have the quest
questMaxLvl			= 15;						-- Level maximum required to have the quest
itemID				= 18164;			-- Just a local variable used for function bellow.Correspond to itemID needed for quest
itemAmount			= 1;				-- Just a local variable used for function bellow.Quantity of the items required.
minState			= 0;				-- starting state of the quest, should be 0
maxstate			= 4;				-- Last state of the quest you can change this value in function of your quest.
JobRequired	= {"VAGRANT"};				-- Just a local variable used for function bellow.You can put int value or constant like you want: max 40
AddDialogs 	= {"Excuse-moi, tu pourrais t'approcher un peu ? Hmm... (regardant attentivement) Bon, je pense que tu pourras m'être utile, enfin j'espère…",
                       "La jolie fille à côté de moi, c'est Losha, ma grande soeur. Je suis Loyah sa cadette. Et donc…",
                       "Ma soeur avait un petit chiot, Yorrn. Elle l'avait trouvé en se balladant dans les montagnes de Leren…",
                       "…il était vraiment frigorifié et apeuré. Elle l'a alors ramené à la maison et en a pris soin, mais un jour il a soudainement disparu…",
                       "Depuis ma soeur a arrêté de boire et manger et le cherche partout. Pourrais-tu aider ma sœur, s'il te plaît ?",
                       "Génial ! Losha se promenait avec Yorrn dans les Montagnes centrales de Leren, là où vivent les Géants Lawolfs... C'est dans cette zone qu'il a disparu..."}
FinishDialogs 	= {"Par Rhisis ! Yorrn ! Où l'as-tu retrouvé, là où je te l'avais indiqué ? Je le savais !! Je suis si heureuse de te revoir Yornn"}
CannotFinishDialogs = {"Tu ne l'as pas trouvé là-bas ? Ou… tu ne l'a même pas cherché, c'est ça? (lançant un regard glacial)"}

--For Dialog you're not obliged to put them here if you don't like "for" loop you can put them directly into the function ADDNPCDialog
---------------------------------------------------------------------------------------------------------------------------------------------------

AddQuest(questname,quesID,questExp,questPenya,questMinLvl,questMaxLvl,minState,maxstate)
--declaration of the main parameter of the quest for penya and exp you don't need to add a function at the end it will be done automatically

--AddState(g_dwQuestID,0,-1)
--SetStartNPC(g_dwQuestID,0,g_szNPCName)
--SetStartRequiredLVL(g_dwQuestID,0,g_dwReqMinLVL,g_dwReqMaxLVL)
--for i = 1 , table.getn(g_szReqJobs) , 1 do
	--AddStartRequiredJob(g_dwQuestID,0,g_szReqJobs[i])
--end
--AddStartNPCTextButton(g_dwQuestID,0,g_szQuestName,g_dwQuestID,1)

--AddState(g_dwQuestID,1,-1)
--SetEndNPC(g_dwQuestID,1,g_szNPCName)
--AddEndNPCTextButton(g_dwQuestID,1,g_szQuestName,g_dwQuestID,1)
--for i = 1 , table.getn(g_szAddDialogs) , 1 do
	--AddEndNPCDialog(g_dwQuestID,1,g_szAddDialogs[i])
--end
--AddEndNPCAnswerButton(g_dwQuestID,1,"BTN_ADD_QUEST","__YES__",g_dwQuestID,3);
--AddEndNPCAnswerButton(g_dwQuestID,1,"BTN_DONT_ADD_QUEST","__NO__",g_dwQuestID,2);

--AddState (g_dwQuestID,2,-1)
--SetEndNPC(g_dwQuestID,2,g_szNPCName)
--AddEndNPCDialog (g_dwQuestID,2,"Comment peux-tu refuser les requêtes d'une petite fille ? Tu es vraiment sans coeur…")
--AddEndNPCAnswerButton(g_dwQuestID,2,"BTN_DONT_ADD_QUEST","__OK__",g_dwQuestID,0);

--AddState(g_dwQuestID,3,0)
--SetStartNPC(g_dwQuestID,3,g_szNPCName)
--AddStartNPCTextButton(g_dwQuestID,3,g_szQuestName,g_dwQuestID,3)
--for i = 1 , table.getn(g_szCannotFinishDialogs) , 1 do
	--AddStartNPCDialog(g_dwQuestID,3,g_szCannotFinishDialogs[i])
--end
--SetEndNPC(g_dwQuestID,3,g_szNPCName)
--AddEndRequiredItem(g_dwQuestID,3,g_dwReqItemID,g_dwReqItemAmount)
--AddEndNPCTextButton(g_dwQuestID,3,g_szQuestName,g_dwQuestID,3)
--for i = 1 , table.getn(g_szFinishDialogs) , 1 do
	--AddEndNPCDialog(g_dwQuestID,3,g_szFinishDialogs[i])
--end
--AddEndNPCAnswerButton(g_dwQuestID,3,"BTN_FINISH_QUEST","__OK__",g_dwQuestID,4)

--AddState(g_dwQuestID,4,14)
--SetEndChangePenya(g_dwQuestID,4,g_dwChgPenya)
--SetEndChangeEXP(g_dwQuestID,4,g_dwChgEXP)
--AddEndRemoveItem(g_dwQuestID,4,g_dwReqItemID,g_dwReqItemAmount)
