using System;

namespace FlyffWorld
{
	public class FlyffItemkind
	{
		/*
			FlyFF header file to C# file converter
			This file was automatically converted with Adidishen's technology.
			Original file name: defineitemkind.h
		*/
		public const int


			// IK_2�� ���� �ǹ����� ����, IK_3�� ������ ������ ���
			////////////////////////////////////////////////////////////////////////////
			// 1�� ���� 
			//////////////////////////////////////////////////////////////////////////
			IK1_GOLD                           =	0,
			IK1_WEAPON                         =	1,
			IK1_ARMOR                          =	2,
			IK1_GENERAL                        =	3,
			IK1_RIDE                           =	4,
			IK1_SYSTEM                         =	5,
			IK1_CHARGED                        =	6,

			////////////////////////////////////////////////////////////////////////////
			// 2�� ���� 
			//////////////////////////////////////////////////////////////////////////
			IK2_GOLD                           =	0,
			IK2_WEAPON_HAND                    =	1,
			IK2_WEAPON_DIRECT                  =	2,
			IK2_WEAPON_MAGIC                   =	3,
			IK2_ARMOR                          =	7,
			IK2_ARMORETC                       =	8,
			IK2_CLOTH                          =	9,
			IK2_CLOTHETC                       =	10,
			IK2_REFRESHER                      =	11,
			IK2_POTION                         =	12,
			IK2_JEWELRY                        =	13,
			IK2_FOOD                           =	14,
			IK2_MAGIC                          =	15,
			IK2_GEM                            =	16,
			IK2_MATERIAL                       =	17,
			IK2_TOOLS                          =	18,
			IK2_SYSTEM                         =	19,
			IK2_RIDING                         =	20,
			IK2_MOB                            =	21,
			IK2_BLINKWING                      =	22,
			IK2_AIRFUEL                        =	23,
			IK2_CHARM                          =	24,
			IK2_BULLET                         =	25,
			IK2_TEXT                           =	26,
			IK2_GMTEXT                         =	27,
			IK2_GENERAL                        =	28,
            IK2_BUFF                           =    29,
            IK2_WARP				           =    30,
			IK2_SKILL                          =	31,
			IK2_CLOTHWIG                       =	32,
            IK2_BUFF2		                   =    33,

			////////////////////////////////////////////////////////////////////////////
			// 3�� ���� 
			//////////////////////////////////////////////////////////////////////////
			IK3_GOLD                           =	0,
			// ���⿡ ���õ� �� (IK_WEAPON �Ҽ�)
			IK3_HAND                           =	1,
			IK3_SWD                            =	2,
			IK3_AXE                            =	3,
			IK3_CHEERSTICK                     =	4,
			IK3_KNUCKLEHAMMER                  =	5,
			IK3_WAND                           =	6,
			IK3_STAFF                          =	7,
			IK3_THSWD                          =	8,
			IK3_THAXE                          =	9,
			IK3_VIRTUAL                        =	10,
			IK3_YOYO                           =	11,
			IK3_BOW                            =	12,
			IK3_YOBO                           =	13,

			// ���� ���õ� �� (IK_ARMOR �Ҽ�)
			IK3_SHIELD                         =	16,
			IK3_HELMET                         =	17,
			IK3_SUIT                           =	18,
			IK3_GAUNTLET                       =	19,
			IK3_BOOTS                          =	20,

			// �ǻ� ���õ� �� (IK2_CLOTH �Ҽ�)
			IK3_HAT                            =	21,
			IK3_MASK                           =	22,
			IK3_SHOES                          =	23,
			IK3_CLOAK                          =	24,
			IK3_CLOTH                          =	57,
			IK3_GLOVE                          =	58,

			// �Ϲ� �����۵�
			IK3_REFRESHER                      =	25,
			IK3_POTION                         =	26,
			IK3_EARRING                        =	27,
			IK3_NECKLACE                       =	28,
			IK3_RING                           =	29,
			IK3_INSTANT                        =	30,
			IK3_COOKING                        =	31,
			IK3_ICECEARM                       =	32,
			IK3_PILL                           =	59,
			IK3_MAGICTRICK                     =	33,
			IK3_GEM                            =	34,
			IK3_DRINK                          =	35,
			IK3_COLLECTER                      =	36,
			IK3_ELECARD                        =	37,
			IK3_DICE                           =	38,
			IK3_SUPSTONE                       =	39,

			// Ż��
			IK3_BOARD                          =	40,
			IK3_STICK                          =	41,
			IK3_EVENTMAIN                      =	42,
			IK3_QUEST                          =	43,
			IK3_MAP                            =	44,
			IK3_BLINKWING                      =	45,
			IK3_EVENTSUB                       =	46,
			IK3_TOWNBLINKWING                  =	47,

			//����ü ������2
			IK3_ACCEL                          =	48,
			IK3_DELETE                         =	49,

			//���ȭ ������
			IK3_SCROLL                         =	50,
			IK3_ENCHANTWEAPON                  =	51,
			IK3_CFLIGHT                        =	52,
			IK3_BFLIGHT                        =	53,
			IK3_MAGICBOTH                      =	54,
			IK3_KEY                            =	55,
			IK3_BCHARM                         =	55,
			IK3_RCHARM                         =	56,
			IK3_ARROW                          =	60,

			//���� ī�� �� �Ǿ�� �ֻ���
			IK3_PIERDICE                       =	61,
			IK3_SOCKETCARD                     =	62,
			IK3_SOCKETCARD2                    =	63,

			//���� ����
			IK3_TEXT_BOOK                      =	70,
			IK3_TEXT_SCROLL                    =	71,
			IK3_TEXT_LETTER                    =	72,

			//��� ��� ������ȭ
            IK3_TEXT_DISGUISE                  =    81,
			IK3_TEXT_INVISIBLE                 =	82,
			IK3_TEXT_GM                        =	83,

			// BINDS
			// ���� ��ȯ
			IK3_CREATE_MONSTER                 =	85,

			IK3_POTION_BUFF_STR                =	90,
			IK3_POTION_BUFF_DEX                =	91,
			IK3_POTION_BUFF_INT                =	92,
			IK3_POTION_BUFF_STA                =	93,
			IK3_POTION_BUFF_DEFENSE            =	94,

			IK3_ANGEL_BUFF                     =	95,

			// PET
			IK3_PET                            =	100,
			IK3_RANDOM_SCROLL                  =	101,

			IK3_ULTIMATE                       =	102,

			// General
			IK3_GENERAL                        =	118,

			IK3_ENCHANT                        =	119,

			IK3_EGG                            =	120,

			MAX_ITEM_KIND3                     =	124,
			MAX_UNIQUE_SIZE                    =	400,

			// ��Ʈ�� ������Ʈ Kind
			CK1_CHEST                          =	0,
			CK1_DOOR                           =	1,
			CK1_TRIGGER                        =	2,

			CK2_FADE                           =	1,
			CK2_KEEP                           =	2,

			CK3_FULL                           =	1,
			CK3_HALF                           =	2,

			WEAPON_GENERAL                     =	0,
			WEAPON_UNIQUE                      =	1,
			WEAPON_ULTIMATE                    =	2,

			ARMOR_SET                          =	3;
	}
}