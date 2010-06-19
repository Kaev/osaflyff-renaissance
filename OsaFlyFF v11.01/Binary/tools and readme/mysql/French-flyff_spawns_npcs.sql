/*
SQLyog Community Edition- MySQL GUI v6.52
MySQL - 5.1.30-community : Database - flyffemu
*********************************************************************
*/

/*!40101 SET NAMES utf8 */;

/*!40101 SET SQL_MODE=''*/;

/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;


/*Table structure for table `flyff_spawns_npcs` */

DROP TABLE IF EXISTS `flyff_spawns_npcs`;

CREATE TABLE `flyff_spawns_npcs` (
  `flyff_spawnid` int(11) NOT NULL,
  `flyff_model` int(11) NOT NULL,
  `flyff_size` int(11) NOT NULL DEFAULT '100',
  `flyff_typename` varchar(25) NOT NULL,
  `flyff_speechtext` varchar(120) NOT NULL,
  `flyff_speechtime` int(11) NOT NULL DEFAULT '15',
  `flyff_worldid` int(10) unsigned NOT NULL DEFAULT '1',
  `flyff_positionx` float NOT NULL,
  `flyff_positiony` float NOT NULL,
  `flyff_positionz` float NOT NULL,
  `flyff_angle` int(11) NOT NULL,
  `flyff_speechdelay` int(11) NOT NULL DEFAULT '10',
  PRIMARY KEY (`flyff_spawnid`)
) ENGINE=MyISAM AUTO_INCREMENT=657 DEFAULT CHARSET=latin1;

/*Data for the table `flyff_spawns_npcs` */

insert  into `flyff_spawns_npcs`(`flyff_spawnid`,`flyff_model`,`flyff_size`,`flyff_typename`,`flyff_speechtext`,`flyff_speechtime`,`flyff_worldid`,`flyff_positionx`,`flyff_positiony`,`flyff_positionz`,`flyff_angle`,`flyff_speechdelay`) values (598,233,100,'MaSa_Hee','',15,1,8341.13,100.6,3750.59,8,15),
(597,215,100,'MaSa_Colack','',15,1,8369.51,100,3675.5,1,15),
(596,233,100,'MaSa_Lancomi','',15,1,8480.75,100,3660.42,63,15),
(595,219,100,'MaSa_Troupemember5','',15,1,8385.1,100,3653.68,44,15),
(594,230,100,'MaSa_Troupemember4','',15,1,8396.88,100.1,3654.48,37,15),
(593,225,100,'MaSa_Troupemember3','',15,1,8398.09,100.1,3657.5,67,15),
(592,220,100,'MaSa_Troupemember2','',15,1,8396.13,100.2,3655.72,22,15),
(590,222,100,'MaSa_Brodeay','',15,1,8403.95,100,3657.74,21,15),
(591,201,100,'MaSa_Troupemember1','',15,1,8386.91,100,3651.86,2,15),
(589,219,100,'MaSa_Bill','',15,1,8410.16,100,3680.76,63,15),
(587,216,100,'MaSa_Tina','',15,1,8412.98,140.249,3974.41,0,15),
(588,218,100,'MaSa_Martin','',15,1,8315.52,100,3728.63,0,15),
(586,212,100,'MaSa_Herth','',15,1,8360.62,100,3751.04,0,15),
(585,212,100,'MaSa_Ann','',15,1,8461.95,100,3709.78,0,15),
(583,211,100,'MaSa_Bozman','',15,1,8375.1,100,3673.2,0,15),
(584,215,100,'MaSa_Bulrox','',15,1,8372.59,100,3671.63,1,15),
(582,212,100,'MaSa_Kimberley','',15,1,8374.66,100,3659.45,36,15),
(581,212,100,'MaSa_Janne','',15,1,8536.5,100,3643.72,0,15),
(580,217,100,'MaSa_Karin','',15,1,8359.34,100,3640.24,27,15),
(578,212,100,'MaSa_Helena','',15,1,8463.48,100,3600.76,36,15),
(579,212,100,'MaSa_Leann','',15,1,8478.1,100,3634.13,0,15),
(577,232,100,'MaFl_Kanane','',15,1,7741.04,95.5146,4074.89,35,15),
(575,220,100,'MaFl_Ryupang','',15,1,7214.24,109.498,3673.46,64,15),
(574,232,100,'MaFl_Gornus','',15,1,7453.87,130.719,3670.85,17,15),
(573,220,100,'MaFl_DrEstly','',15,1,7186.78,156.2,4090.63,0,15),
(572,233,100,'MaFl_Kurumin','',15,1,6691.42,120.054,3619.13,44,15),
(570,201,100,'MaFl_Hastan','',15,1,7002.03,124.395,3894.99,62,15),
(571,235,100,'MaFl_Ancimys','Qui veut jouer avec moi ?',15,1,7036.38,118.339,3728.5,0,15),
(569,201,100,'MaFl_Goripeg','',15,1,6991.91,124.603,3891.94,10,15),
(568,201,100,'MaFl_Langdrong','',15,1,6998.49,125.05,3897.48,69,15),
(567,201,100,'MaFl_Tucani','',15,1,6995.01,125.335,3897.01,0,15),
(566,235,100,'MaMa_Ancimys','',15,1,6955.46,149.134,3848.43,34,15),
(564,229,100,'MaSa_Heltung','',15,1,8447.49,100,3576.31,54,15),
(565,233,100,'MaSa_Rovanett','',15,1,8439.95,100.025,3547.75,0,15),
(563,235,100,'MaMa_Ancimys','',15,1,8638.01,100,3569.62,0,15),
(562,223,100,'MaFl_Tomba','Cette journée a été bien remplie',15,1,7203.2,100,3231.36,62,15),
(561,225,100,'MaFl_Eoners','Je suis heureux...',15,1,7204.65,100,3226.73,48,15),
(560,227,100,'MaFl_Phoho','Je me sent chez moi des que je peux me relaxer un peu...',15,1,7204.2,100,3229.8,59,15),
(559,231,100,'MaFl_Ghalade','Nous soldats de Kyon sommes une grande Famille',15,1,7204.37,100,3227.88,52,15),
(557,227,100,'MaFl_SsoTta','Le poisson est esquis, le riz est délicat, venez découvrir mes délicieux sushis',15,1,7179.42,100,3216.34,51,15),
(558,216,100,'MaFl_Teshar','Soldat de Kyoon rassemblez vous',15,1,7202.36,100,3228.75,16,15),
(556,210,100,'MaFl_Loyah','Venez gouter aux plats esquis de Loya !',15,1,7169.42,100,3208.96,33,15),
(555,228,100,'MaFl_Rudvihil','',15,1,7219.01,108.631,3190.55,37,15),
(554,210,100,'MaFl_Losha','Les meilleurs plats et aliments de Flarine sont ici',15,1,7172.42,100,3212.39,39,15),
(552,223,100,'MaFl_Ray','Vous vous croyez fort ? Alors prouvez le dans l\'arène !',15,1,6931.95,100,3259.48,12,15),
(553,214,100,'MaFl_Marche','Vous cherchez un univers de magie, de féérie ? Entrez dans ma boutique pour réaliser vos rêves',15,1,7169.17,100,3271.16,68,15),
(550,913,100,'MaFl_COLINSE','',15,1,6943.97,100.138,3101.11,36,15),
(549,910,100,'MaFl_Waforu','Tout les objets rares et précieux de Madrigal sont ici !',15,1,6982.49,100,3351.13,63,15),
(548,224,100,'MaFl_Annie','Je suis à votre disposition si vous avez des questions ou si vous voulez connaître le résultat de la',15,1,6988.87,100,3321.75,52,15),
(547,223,100,'MaFl_Amos','Voulez-vous savoir quelle guilde est la plus forte? Entrez dans l\'Arène et décidez de votre destin!',15,1,6989.11,100,3325.93,53,15),
(543,211,100,'MaFl_Bobochan','Les armes peuvent être rendues plus puissantes en les convertissant aux grades Unique et Ultime!',15,1,6927.71,100,3227.13,24,15),
(542,878,100,'MaFl_HairShop','Hey baby ! Come on par ici ! Je ne peux ps te laisser repartir avec cette horrible coupe de cheveux ',15,1,6958.22,100,3349.97,0,15),
(541,879,100,'MaFl_FaceOff','Besoin d\'un lifting ? Où juste un peu de maquillage pour plaire à votre entourage ? Venez me voir !',15,1,6961.31,100,3349.83,0,15),
(540,880,100,'MaFl_PetTamer','Les familliers sont les partenaires parfait pour les aventuriers de Madrigal',15,1,6989.56,100,3256.07,59,15),
(538,12,100,'MaFl_FlaMayor','',15,1,6960.03,100,3227.32,0,15),
(539,873,100,'MaFl_Postbox','',15,1,6957.51,100,3211.72,36,15),
(537,219,100,'MaFl_Ata','',15,1,6703.56,136.085,3326.79,25,15),
(535,230,100,'MaFl_Donaris','',15,1,6989.05,100,3331.15,54,15),
(536,226,100,'MaFl_Gergantes','Ceci pourrait me servir pour mon prochain roman mais coment mettre tout celà en forme...',15,1,6960.13,100,3339.73,0,15),
(534,213,100,'MaFl_GuildWar','',15,1,6989.21,100,3335.09,53,15),
(533,232,100,'MaFl_Himadel','',15,1,6835.97,108.616,3132.56,17,15),
(532,233,100,'MaFl_Martinyc','Ah l\'histoire de madrigal...Tellement étrange...Il s\'est passé énormément de choses...',15,1,6998.79,100,3203.15,33,15),
(531,220,100,'MaFl_Official','Bienvenue à Flarine',15,1,6960.41,100,3204.27,35,15),
(530,218,100,'MaFl_Maki','Que Rhisis protège les aventuriers qui veulent devenir acolyte ...',15,1,6930.36,100.3,3324.3,19,15),
(529,214,100,'MaFl_Kidmen','Propage la sagesse de Rhisis à travers le monde',15,1,7156.19,100,3250.17,48,15),
(528,214,100,'MaFl_Hyuit','Dégaine ton épée et tranche tout ce qui est en travers de ton chemin en avant mercenaire',15,1,7152.51,99.9986,3242.4,48,15),
(527,226,100,'MaFl_Elic','Avant d\'aider les autres commence par t\'aider toi même',15,1,7155.72,99.9994,3248.06,50,15),
(525,218,100,'MaFl_Andy','Bienvenue à ceux qui veulent devenir Mercenaire. Félicitation, une aventure extraordinaire vous atte',15,1,6929.96,100.3,3328.84,19,15),
(526,226,100,'MaFl_Mustang','Etre un mercenaire n\'est pas à la portée de tout le monde.',15,1,7153.92,99.9986,3244.07,48,15),
(524,215,100,'MaFl_Luda','Venez découvrir de fabuleuses armes et armures pour vagabon !',15,1,6927.3,100,3236.48,6,15),
(523,218,100,'MaFl_Noier','',15,1,6703.29,135.965,3327.76,17,15),
(521,219,100,'MaFl_Jeff','Voyons voir ce qu\'il y a à réparer aujourd\'hui...Holàlà, kimberley me manque tans...',15,1,6954.72,100,3274.1,18,15),
(522,233,100,'MaFl_Mikyel','La vie est un défi à relever, un bonheur à mériter une aventure à tenter',15,1,7129.19,99.9578,3252.48,16,15),
(520,211,100,'MaFl_Boboko','Les fabuleuses armures de la famille Bobo sont ici...',15,1,6926.51,100,3229.29,24,15),
(519,211,100,'MaFl_Boboku','Venez acheter les armes fantastiques de la famille Bobo...ou faire un percing sur votre armure...',15,1,6926.1,100,3232.11,18,15),
(517,212,100,'MaFl_Ispim','Si vous vous posez la moindre question sur notre ville, je suis là pour vous aider !',15,1,7160.07,100,3221.46,35,15),
(518,212,100,'MaFl_Isruel','Si vous vous posez la moindre question sur notre ville, je suis là pour vous aider !',15,1,6937.83,100,3243.34,17,15),
(516,212,100,'MaFl_Ismeralda','Si vous vous posez la moindre question sur notre ville, je suis là pour vous aider !',15,1,7042.26,99.4944,3244.29,54,15),
(515,212,100,'MaFl_Is','Si vous vous posez la moindre question sur notre ville, je suis là pour vous aider !',15,1,6960.07,100,3266.1,0,15),
(514,236,100,'MaFl_santa','',15,1,6957.96,100,3238.35,64,15),
(599,225,100,'MaSa_Wingyei','',15,1,8483.72,100.6,3810.27,36,15),
(600,214,100,'MaSa_Lopaze','',15,1,8481.51,100,3810.4,35,15),
(513,213,100,'MaFl_Lui','Boissons, flèches, mantra, bijoux, c\'est ici que ça se passe...',15,1,6975.66,100,3266.87,60,15),
(512,212,100,'MaFl_Juria','Vous cherchez la Banque ? DEs infos sur les guildes ? C\'est ici !',15,1,6958.43,100,3211.79,36,15),
(510,219,100,'MaSa_MaYun','',15,1,8197.68,84.7928,2805.67,5,15),
(509,220,100,'MaFl_DrEstern','',15,1,7940.8,159.076,2405.93,0,15),
(507,219,100,'MaDa_Colar','Hé! Vous là! Aidez moi! Laissez moi partir! Je veux sortir d\'ici!',15,1,6133.4,100,4168.77,0,15),
(508,913,100,'MaFl_COLINSE','',15,1,3851.17,94.7694,4660.1,65,15),
(506,891,100,'MaMa_PKNPC01','',15,1,5482.89,71.3512,4362.69,12,15),
(504,223,100,'MaFl_Ray','Vous vous croyez fort ? Alors prouvez le dans l\'arène !',15,1,3801.63,59,4448.15,9,15),
(505,220,100,'MaDa_DrEst','L\'écosystème en Madrigal a toujours été un mystére, mais notre vieux continent est vraiment magnifique',15,1,5541.34,71.6196,4352.41,0,15),
(503,880,100,'MaFl_PetTamer','Les familliers sont les partenaires parfait pour les aventuriers de Madrigal',15,1,3824.43,59,4462.05,69,15),
(502,223,100,'MaDa_Roocky','Venez acheter vos armures et vos boucliers à la boutique de Roocky!',15,1,3742.82,59,4419.58,15,15),
(500,12,100,'MaDa_DarMayor','La ville de darkon est la plus importante de tout Madrigal pour le commerce, mais c\'est aussi celle ',15,1,3831.63,59.1,4457.59,67,15),
(501,873,100,'MaFl_Postbox','',15,1,3834.11,59,4456.45,61,15),
(499,12,100,'MaDa_Eliff','Oh Hainan, héro ranger, protège tous les ranger et guide les sur ce monde!',15,1,3790.86,59.6,4492.96,8,15),
(498,12,100,'MaDa_Lorein','Oh Curenen, grand héro Jester, puisse on âme protéger tous les jester!',15,1,3784.56,59.1,4490.19,8,15),
(497,12,100,'MaDa_Horison','Lillip, Héro élémentaliste! Protège ce pauvre monde par ton savoir!',15,1,3795.17,59.1,4498.27,9,15),
(496,12,100,'MaDa_Ellend','Eiene, héro des prêtre, ton nom, ton esprit et tes bénédicion ne seront jamais oublié de part le mon',15,1,3798.54,59.2,4490.97,8,15),
(495,12,100,'MaDa_Corel','Oh Heren, Hero assassin, je suivrais vos souhaits et les réaliserais en ce monde!',15,1,3796.3,59.2,4487.33,9,15),
(494,12,100,'MaDa_Karanduru','Oh Armorius, héro chevalier, fait que notre armure soit aussi dure que la tienne et protège nous du ',15,1,3793.46,59.1,4482.55,9,15),
(493,12,100,'MaDa_Cylor','Offerep,grand héro sorcier, Je suis heureux de pouvoir continuer votre travail en ce monde!',15,1,3781.48,59.1,4485.05,10,15),
(492,12,100,'MaDa_Ride','Roentel, héro moine, Je finirais ce que tu as commencé en ce monde!',15,1,3788.05,59.1,4481.21,13,15),
(491,213,100,'MaDa_Bernard','',15,1,3827.02,59,4548.46,0,15),
(490,201,100,'MaDa_Liekyen','Nous les High-Dwarpets sommes vraiment différents des nains normaux.Nous sommes beaucoup plus nombre',15,1,3733.89,59,4525.85,65,15),
(489,218,100,'MaDa_Pyre','Si vous désirez savoir comment devenir acrobate, venez me voir!',15,1,3850.12,59,4425.96,36,15),
(488,214,100,'MaDa_Tailer','Etes vous prêt à devenir Acrobate ?',15,1,3931.98,59.0206,4389.94,55,15),
(486,223,100,'MaDa_Krinton','Venez à l\'armurerie de Krinton est achetez des armure lourde, mais légères à porter, aujourd\'hui!',15,1,3742.22,59,4413.74,24,15),
(487,226,100,'MaDa_Hent','Hé, vou avez l\'air fort. Voulez vous devenir un acrobate et devenir le combattant le plus puissant d',15,1,3931.3,59.0073,4385.88,46,15),
(484,227,100,'MaDa_Bolpor','Même si vous êtes pauvre et que vous avez faim, Bolpor possède aussi des aliments à bas pris pour vo',15,1,3831.7,59,4356.61,43,15),
(485,233,100,'MaDa_Lurif','Vous avez besoin d\'argent? Alors venez voir le bureau des quêtes de Lurif et prenez une quête à fair',15,1,3841.46,59,4422.95,39,15),
(483,225,100,'MaDa_Haven','Ici à la boutique magique de haven nous vendons des armes puissantes pour Magiciens pour un prix mod',15,1,3908.94,59,4376.03,46,15),
(482,229,100,'MaDa_Achaben','Avez vous besoin d\'articles basiques mais puissant? Si oui, venez voir le magasin général d\'Achaben!',15,1,3879.86,59,4458.07,0,15),
(481,228,100,'MaDa_Almani','Avez vous besoin d\'un balais volant? ou peut être un squate volant? Alors venez voir me articles vol',15,1,3846.71,59,4443.02,0,15),
(479,231,100,'MaDa_Remine','Venez voir la boutique d\'arme de Remine et achetez les meilleures armes de tout Madrigal aujourd\'hui',15,1,3748.58,59,4412.3,15,15),
(480,224,100,'MaDa_Ollien','Si vous avez besoin d\'accéder à votre compte en banque ou voir les résultats de guerre de guilde, da',15,1,3829.78,59.1,4457.24,1,15),
(478,232,100,'MaDa_Eshylop','Quelqu\'un pourrait m\'aider s\'il vous plait ? Clockworks deviens de plus en plus puissant..',15,1,3890.03,58.8415,4145.79,36,15),
(477,12,100,'MaDa_Heedan','Eiene, votre volonté a sauvé le monde!',15,1,3273.11,132,4250.09,8,15),
(476,891,100,'MaMa_PKNPC01','',15,1,6589.83,100.1,3822.05,0,15),
(475,228,100,'MaDa_CloneHachal','',15,1,6552.24,108.244,3618.51,40,15),
(474,880,100,'MaFl_PetTamer','Les familliers sont les partenaires parfait pour les aventuriers de Madrigal',15,1,5569.15,75.0194,3898.98,30,15),
(473,213,100,'MaDa_Amadolka','Aucun monstre ne pourra attaquer ce lieu tant que je le defendrais!',15,1,5600.74,75.0001,3767.31,0,15),
(472,215,100,'MaDa_Cell','La Mine de Dekanes regorge de métaux et minerais...',15,1,5561.09,75.0287,3909.13,0,15),
(471,235,100,'MaMa_Ancimys','',15,1,5370.74,75,3991.26,49,15),
(470,222,100,'MaDa_Phacham','Hey, venez jeter un oeil à ce que j\'ai !',15,1,5580.7,75,3901.84,46,15),
(469,221,100,'MaDa_Stima','Les plus belles armes sont ici, dans la boutique de Stima !',15,1,5572.36,75.0001,3897.21,41,15),
(468,201,100,'MaDa_Rankashu','Personne ne peut t\'empêcher de sauter dedans... mais tu es conscient que c\'est extrêmement chaud ?',15,1,5072.61,55,3962.48,0,15),
(467,232,100,'MaDa_CloneEshylop','',15,1,3889.87,59,3923.52,36,15),
(466,12,100,'MaDa_Rupim','Oh Hainan, notre héro,je suivrais tes préceptes pour toujours!',15,1,3170.87,65.6278,3644.52,0,15),
(465,12,100,'MaDa_Laloa','Il y a tellement de fleurs aujourd\'hui ! Et elles sont tellement belles !',15,1,3007.87,49.1527,3967.41,63,15),
(464,226,100,'MaDa_Andre','Je suis un vrai lache...',15,1,2991.35,56.7307,4063.5,0,15),
(463,228,100,'MaDa_Hachal','Hmm... Y\'a-t-il quelqu\'un qui puisse m\'aider... ?!',15,1,5535.89,97,3462.18,42,15),
(462,12,100,'MaDa_Condram','Lillip, Aidez nous à sauver ce monde par votre volonté!',15,1,3959.78,31.3685,3510.14,65,15),
(461,12,100,'MaDa_Fera','Roentel, S\'il te plait bénie nous tous à l\'aide de tes pouvoirs incommensurable!',15,1,3524.22,41.1026,3547.31,41,15),
(460,222,100,'MaDa_Jinitte','Avez vous faim? Ou peut etre soif? Dans ce cas venez faire un tour à la boutique alimentaire de Jini',15,1,3208.39,10.1552,3437.29,63,15),
(459,222,100,'MaDa_Tandy','Dans la boutique générale de Tandy vous pouvez acheter différents choses de grande qualité faites ma',15,1,3202.74,10.0689,3437.85,5,15),
(458,222,100,'MaDa_Kablloc','Vous vous sentez faible? Vous ne vous sentez pas capable de tuer des monstres que vos amis peuvent? ',15,1,3239.98,11.6511,3417.7,54,15),
(457,222,100,'MaDa_Chenbing','L\'armurerie de Chenbing est ici, vous pouvez acheter des armures de grande qualité pour tous les niv',15,1,3238.18,11.6511,3421.03,62,15),
(456,223,100,'MaDa_Nein','',15,1,3947.29,117.651,2748.43,19,15),
(455,12,100,'MaDa_Jeperdy','Heren, répandez à travers ce monde votre présence héroique!',15,1,3526.78,88.5169,2792.02,54,15),
(454,12,100,'MaDa_Homeit','Curenen, tu sera toujours mon hero!',15,1,2788.06,191.923,2885.31,28,15),
(453,12,100,'MaDa_Lobiet','Armorius, notre héro, je suivrais tes précepte pour toujours!',15,1,3012.95,93.4913,2931.39,18,15),
(452,234,100,'MaDa_Kailreard','Vous avez un regard perçant, une habilité hors du commun à l\'arc, alors suivez la voie du Ranger! El',15,1,3351.37,146.986,2063.72,17,15),
(449,234,100,'MaDa_Wendien','',15,1,3348.82,146.927,2058.48,8,15),
(450,234,100,'MaDa_Capafe','',15,1,3339.11,146.917,2068.67,46,15),
(451,234,100,'MaDa_Heingard','',15,1,3336.71,146.9,2063.72,54,15),
(448,234,100,'MaDa_Romvoette','',15,1,3339.08,146.882,2058.45,63,15),
(447,234,100,'MaDa_Shyniff','',15,1,3348.99,146.967,2068.67,26,15),
(446,234,100,'MaDa_Sencyrit','',15,1,3344.05,146.952,2056.1,0,15),
(445,234,100,'MaDa_Boneper','',15,1,3344.03,146.956,2071.1,35,15),
(444,875,100,'MaDa_RedRobeMan','',15,1,3343.95,114.628,1859.32,36,15),
(408,213,100,'MaFl_Dick','',15,208,541.738,138.176,512.083,60,15),
(407,223,100,'MaFl_Harold','',15,208,545.548,138.624,485.773,50,15),
(406,232,100,'MaSa_Daz','',15,204,1075.56,5.28125,1337.62,0,15),
(405,201,100,'DuDk_Kazen','Hmmm... cela risque de prendre un moment...',15,201,1618.4,84.3547,746.327,14,15),
(404,691,100,'DuDk_Nevil','Ah si je pouvais sortir de cette prison...',15,201,1073.51,86.4,696.314,0,15),
(403,691,100,'DuDk_Drian','Bienvenue dans le domaine des Keakoons jeune aventurier. Tu m\'as l\'air d\'être quelqu\'un de confiance... pourrais-tu me r',15,201,829.026,85.0022,966.924,0,15),
(601,223,100,'MaSa_Helgar','',15,1,8449.09,100,3638.13,16,15),
(602,691,100,'MaSa_QueerCollector','',15,1,8383.2,145,4085.15,0,15),
(603,225,100,'MaSa_JeongHwa','',15,1,8334.95,100,3713.43,45,15),
(604,213,100,'MaSa_Porgo','',15,1,8532.44,100,3607.49,67,15),
(605,228,100,'MaSa_Gothante','',15,1,8353.02,99.7752,3983.67,63,15),
(606,221,100,'MaSa_Parine','',15,1,8414.34,140.141,3974.8,0,15),
(607,12,100,'MaSa_SainMayor','',15,1,8464.29,100,3651.49,0,15),
(609,873,100,'MaFl_Postbox','',15,1,8462.63,100,3600.31,35,15),
(610,880,100,'MaFl_PetTamer','Les familliers sont les partenaires parfait pour les aventuriers de Madrigal',15,1,8465.42,100,3622.85,0,15),
(611,913,100,'MaFl_COLINSE','',15,1,8636.33,100.195,3738.45,52,15),
(613,223,100,'MaFl_Ray','Vous vous croyez fort ? Alors prouvez le dans l\'arène !',15,1,8437.54,100,3616.37,17,15),
(614,201,100,'MaSa_Bowler','',15,1,9560.33,90.5346,4020.01,54,15),
(615,218,100,'MaFl_SgRadion','',15,1,7640.48,175.12,4254.25,36,15),
(616,218,100,'MaFl_Radyon','',15,1,7635.97,169.02,4229.07,21,15),
(617,218,100,'MaFl_Kimel','',15,1,7636.03,168.84,4227.36,18,15),
(618,218,100,'MaFl_Hormes','',15,1,7677.74,179.378,4252.2,63,15),
(619,218,100,'MaFl_Guabrill','',15,1,7678.73,179.091,4251.53,63,15),
(620,218,100,'MaFl_Cuzrill','',15,1,7655.6,161.675,4191.63,35,15),
(621,218,100,'MaFl_Cuarine','',15,1,7657.55,161.695,4191.51,35,15),
(622,222,100,'MaFl_Segho','',15,1,7644.78,236.285,4349.11,50,15),
(623,218,100,'MaFl_Domek','',15,1,7693.45,170.711,4227.5,53,15),
(624,218,100,'MaFl_Clamb','',15,1,7693.29,170.203,4225.89,53,15),
(625,891,100,'MaMa_PKNPC01','',15,1,8247.27,95.5846,2525.55,26,15),
(626,12,100,'MaDa_Pereb','Offerep, donne nous la force avec tes pouvoirs magiques!',15,1,2522.29,55,4599.88,17,15),
(700,216,100,'MaFl_Dior','',15,1,6960.33,100,3237.43,64,15),
(701,200,160,'MaFl_Helper_ver12','Débutant approchez vous, je peux vous aider !',15,1,6965.85,100,3213.2,175,15),
(702,877,100,'MaFl_CardMaster','Amenez moi toutes vos cartes et percing',15,1,7129,100,3255,15,15),
(703,981,100,'MaFl_HEAVENMAN','',15,1,6975,100,3296,115,15),
(704,983,100,'MaFl_KINGAIDE','',15,1,6958,100,3321,0,15),
(705,231,100,'MaFl_Peach','Je ne peut pas imaginer un monde sans gemmes',15,1,6929.85,100,3226.46,35,15),
(706,237,100,'MaFl_Prist','',15,1,6988.12,100,3222.25,0,15);

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
