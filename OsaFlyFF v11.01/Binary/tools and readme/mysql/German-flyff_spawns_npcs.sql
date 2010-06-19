/*
MySQL Data Transfer
Source Host: localhost
Source Database: translation
Target Host: localhost
Target Database: translation
Date: 17.04.2009 00:36:26
*/
/*Translated by MazeXD*/

SET FOREIGN_KEY_CHECKS=0;
-- ----------------------------
-- Table structure for flyff_spawns_npcs
-- ----------------------------
DROP TABLE IF EXISTS `flyff_spawns_npcs`;
CREATE TABLE `flyff_spawns_npcs` (
  `flyff_spawnid` int(11) NOT NULL,
  `flyff_model` int(11) NOT NULL,
  `flyff_size` int(11) NOT NULL DEFAULT '1',
  `flyff_typename` varchar(25) NOT NULL,
  `flyff_speechtime` int(11) NOT NULL DEFAULT '15',
  `flyff_worldid` int(10) unsigned NOT NULL DEFAULT '1',
  `flyff_positionx` float NOT NULL,
  `flyff_positiony` float NOT NULL,
  `flyff_positionz` float NOT NULL,
  `flyff_angle` int(11) NOT NULL,
  `flyff_speechdelay` int(11) NOT NULL DEFAULT '10',
  `flyff_speechtext` varchar(100) NOT NULL,
  PRIMARY KEY (`flyff_spawnid`)
) ENGINE=MyISAM AUTO_INCREMENT=657 DEFAULT CHARSET=latin1;

-- ----------------------------
-- Records 
-- ----------------------------
INSERT INTO `flyff_spawns_npcs` VALUES ('598', '233', '100', 'MaSa_Hee', '15', '1', '8341.13', '100.6', '3750.59', '8', '10', 'Jeder der ein Magier sein will soll bitte zu mir kommen ! Mein Name ist Hee !');
INSERT INTO `flyff_spawns_npcs` VALUES ('597', '215', '100', 'MaSa_Colack', '15', '1', '8369.51', '100', '3675.5', '1', '10', 'Schilder zum Verkauf ! Bitte eine Reihe bilden .');
INSERT INTO `flyff_spawns_npcs` VALUES ('596', '233', '100', 'MaSa_Lancomi', '15', '1', '8480.75', '100', '3660.42', '63', '10', 'Wir bitten Quests der Bewohner von Saint Morning . Diese Quests sind für Leute die Lvl 20 sind .');
INSERT INTO `flyff_spawns_npcs` VALUES ('595', '219', '100', 'MaSa_Troupemember5', '15', '1', '8385.1', '100', '3653.68', '44', '10', 'Das ist ein Schild und keine Schaufel ...');
INSERT INTO `flyff_spawns_npcs` VALUES ('594', '230', '100', 'MaSa_Troupemember4', '15', '1', '8396.88', '100.1', '3654.48', '37', '10', 'Du ! Wie kann man nichts aus deinem Verstand gemacht haben ?');
INSERT INTO `flyff_spawns_npcs` VALUES ('593', '225', '100', 'MaSa_Troupemember3', '15', '1', '8398.09', '100.1', '3657.5', '67', '10', 'Da ist nichst was ich für dich tun kann . Auf Wiedersehen , Juliat');
INSERT INTO `flyff_spawns_npcs` VALUES ('592', '220', '100', 'MaSa_Troupemember2', '15', '1', '8396.13', '100.2', '3655.72', '22', '10', 'Mein Fenster wird immer für dich geöffnet sein .');
INSERT INTO `flyff_spawns_npcs` VALUES ('590', '222', '100', 'MaSa_Brodeay', '15', '1', '8403.95', '100', '3657.74', '21', '10', 'Ich werde dich das beste Stück sehen lassen , durchgeführt von Pepoview');
INSERT INTO `flyff_spawns_npcs` VALUES ('591', '201', '100', 'MaSa_Troupemember1', '15', '1', '8386.91', '100', '3651.86', '2', '10', 'Wo ist meine Tochter ?Hat irgendjemand sie gesehen ?');
INSERT INTO `flyff_spawns_npcs` VALUES ('589', '219', '100', 'MaSa_Bill', '15', '1', '8410.16', '100', '3680.76', '63', '10', 'Hi! Ich bin Bill. Komm und kauf alles was du brauchst in meinem Allgemeinladen .');
INSERT INTO `flyff_spawns_npcs` VALUES ('587', '216', '100', 'MaSa_Tina', '15', '1', '8412.98', '140.249', '3974.41', '0', '10', 'Alle Informationen übers fliegen findest du hier an der Station .');
INSERT INTO `flyff_spawns_npcs` VALUES ('588', '218', '100', 'MaSa_Martin', '15', '1', '8315.52', '100', '3728.63', '0', '10', 'Willst du wirkungsvolle Magie benutzen ? Dann sollst du mich besuchen , Martin!');
INSERT INTO `flyff_spawns_npcs` VALUES ('586', '212', '100', 'MaSa_Herth', '15', '1', '8360.62', '100', '3751.04', '0', '10', 'Kann ich dir helfen ? Wenn du Fragen hast , dann frag nur .');
INSERT INTO `flyff_spawns_npcs` VALUES ('585', '212', '100', 'MaSa_Ann', '15', '1', '8461.95', '100', '3709.78', '0', '10', 'Kann ich dir helfen ? Wenn du Fragen hast , dann frag nur .');
INSERT INTO `flyff_spawns_npcs` VALUES ('583', '211', '100', 'MaSa_Bozman', '15', '1', '8375.1', '100', '3673.2', '0', '10', 'Schilder die beides haben . Stärke und Schönheit können in meinem Schilderladen gefunden werden .');
INSERT INTO `flyff_spawns_npcs` VALUES ('584', '215', '100', 'MaSa_Bulrox', '15', '1', '8372.59', '100', '3671.63', '1', '10', 'Komm und schau was für überlegene Waffenherstellung wie die meines Waffenladens !');
INSERT INTO `flyff_spawns_npcs` VALUES ('582', '212', '100', 'MaSa_Kimberley', '15', '1', '8374.66', '100', '3659.45', '36', '10', 'Kann ich dir helfen ? Wenn du Fragen hast , dann frag nur .');
INSERT INTO `flyff_spawns_npcs` VALUES ('581', '212', '100', 'MaSa_Janne', '15', '1', '8536.5', '100', '3643.72', '0', '10', 'Kann ich dir helfen ? Wenn du Fragen hast , dann frag nur .');
INSERT INTO `flyff_spawns_npcs` VALUES ('580', '217', '100', 'MaSa_Karin', '15', '1', '8359.34', '100', '3640.24', '27', '10', 'Gesunf Körper für gesunden Verstand ! Hier ist etwas delikates zu Essen von mir !');
INSERT INTO `flyff_spawns_npcs` VALUES ('578', '212', '100', 'MaSa_Helena', '15', '1', '8463.48', '100', '3600.76', '36', '10', 'Saint Hall ist die grösste Struktur in Madrigall *heul* Wie oft muss ich das selbe sagen ?');
INSERT INTO `flyff_spawns_npcs` VALUES ('579', '212', '100', 'MaSa_Leann', '15', '1', '8478.1', '100', '3634.13', '0', '10', '');
INSERT INTO `flyff_spawns_npcs` VALUES ('577', '232', '100', 'MaFl_Kanane', '15', '1', '7741.04', '95.5146', '4074.89', '35', '10', 'Wir warten auf das nächste Abenteuer. Kommt , mutige Seelen !');
INSERT INTO `flyff_spawns_npcs` VALUES ('575', '220', '100', 'MaFl_Ryupang', '15', '1', '7214.24', '109.498', '3673.46', '64', '10', 'Soldaten ! Hebt die blaue Flage von Jung Hwa! Wir existieren für diese Leute !');
INSERT INTO `flyff_spawns_npcs` VALUES ('574', '232', '100', 'MaFl_Gornus', '15', '1', '7453.87', '130.719', '3670.85', '17', '10', 'Wir warten auf das nächste Abenteuer. Kommt , mutige Seelen !');
INSERT INTO `flyff_spawns_npcs` VALUES ('573', '220', '100', 'MaFl_DrEstly', '15', '1', '7186.78', '156.2', '4090.63', '0', '10', 'Die Ökologie von  Madrigal ist ein wahres Meisterstück des Gottes .');
INSERT INTO `flyff_spawns_npcs` VALUES ('572', '233', '100', 'MaFl_Kurumin', '15', '1', '6691.42', '120.054', '3619.13', '44', '10', 'Hallo , du da .');
INSERT INTO `flyff_spawns_npcs` VALUES ('570', '201', '100', 'MaFl_Hastan', '15', '1', '7002.03', '124.395', '3894.99', '62', '10', 'Unser Wille , Wut , und Flüche sollen alles in dieser Welt verschwinden lassen .');
INSERT INTO `flyff_spawns_npcs` VALUES ('571', '235', '100', 'MaFl_Ancimys', '15', '1', '7036.38', '118.339', '3728.5', '0', '10', 'Psst!! Willst du mit mir spielen !? ');
INSERT INTO `flyff_spawns_npcs` VALUES ('569', '201', '100', 'MaFl_Goripeg', '15', '1', '6991.91', '124.603', '3891.94', '10', '10', 'Der Atem von Rhisis wird deinen Körper und deine Seele beschützen .');
INSERT INTO `flyff_spawns_npcs` VALUES ('568', '201', '100', 'MaFl_Langdrong', '15', '1', '6998.49', '125.05', '3897.48', '69', '10', 'Das Schwert schüttelt denn Himmel , und die Riesenaxt teilt die Erde. Alles endet im Verderben !');
INSERT INTO `flyff_spawns_npcs` VALUES ('567', '201', '100', 'MaFl_Tucani', '15', '1', '6995.01', '125.335', '3897.01', '0', '10', 'Hast du schonmal Pfeile von Himmel fliegen sehn ?  Es heisst Pfeilregen . LOL!');
INSERT INTO `flyff_spawns_npcs` VALUES ('566', '235', '100', 'MaMa_Ancimys', '15', '1', '6955.46', '149.134', '3848.43', '34', '10', 'Psst!! Willst du mit mir spielen !?');
INSERT INTO `flyff_spawns_npcs` VALUES ('564', '229', '100', 'MaSa_Heltung', '15', '1', '8447.49', '100', '3576.31', '54', '10', '');
INSERT INTO `flyff_spawns_npcs` VALUES ('565', '233', '100', 'MaSa_Rovanett', '15', '1', '8439.95', '100.025', '3547.75', '0', '10', 'Oh! Bitte sei vorsichtig , das ist ein sehr wichtiges , altes Dokument !!');
INSERT INTO `flyff_spawns_npcs` VALUES ('563', '235', '100', 'MaMa_Ancimys', '15', '1', '8638.01', '100', '3569.62', '0', '10', 'Psst!! Willst du mit mir spielen !?');
INSERT INTO `flyff_spawns_npcs` VALUES ('562', '223', '100', 'MaFl_Tomba', '15', '1', '7203.2', '100', '3231.36', '62', '10', 'Okay,lass es uns einen Tag nennen!');
INSERT INTO `flyff_spawns_npcs` VALUES ('561', '225', '100', 'MaFl_Eoners', '15', '1', '7204.65', '100', '3226.73', '48', '10', 'Ich bin so glücklich !');
INSERT INTO `flyff_spawns_npcs` VALUES ('560', '227', '100', 'MaFl_Phoho', '15', '1', '7204.2', '100', '3229.8', '59', '10', 'Überall wo ich mich ausruhen kann . Da bin ich zuhause .');
INSERT INTO `flyff_spawns_npcs` VALUES ('559', '231', '100', 'MaFl_Ghalade', '15', '1', '7204.37', '100', '3227.88', '52', '10', 'Wir , die Soldaten von Kyon, sind eine Familie !');
INSERT INTO `flyff_spawns_npcs` VALUES ('557', '227', '100', 'MaFl_SsoTta', '15', '1', '7179.42', '100', '3216.34', '51', '10', 'Okay! Slice some raw fish, and boil some rice! We are making sushi!');
INSERT INTO `flyff_spawns_npcs` VALUES ('558', '216', '100', 'MaFl_Teshar', '15', '1', '7202.36', '100', '3228.75', '16', '10', 'Soldaten von Kyon, versammelt euch ! Bewegt euch ! Bewegt euch !');
INSERT INTO `flyff_spawns_npcs` VALUES ('556', '210', '100', 'MaFl_Loyah', '15', '1', '7169.42', '100', '3208.96', '33', '10', 'Hier ist das best gemachte Essen von meist brillianten Koch in Mafrigal , Losha!');
INSERT INTO `flyff_spawns_npcs` VALUES ('555', '228', '100', 'MaFl_Rudvihil', '15', '1', '7219.01', '108.631', '3190.55', '37', '10', 'Ha ha ha ! Nichts ist einfacher als Geld zu machen . Stimmts ? Ha ha ha !');
INSERT INTO `flyff_spawns_npcs` VALUES ('554', '210', '100', 'MaFl_Losha', '15', '1', '7172.42', '100', '3212.39', '39', '10', 'Hier ist das beste Essen in Flarine! Bitte komm zum Lalaen Essensladen !');
INSERT INTO `flyff_spawns_npcs` VALUES ('552', '223', '100', 'MaFl_Ray', '15', '1', '6931.95', '100', '3259.48', '12', '10', 'Denkst du dass du stark genug bist ? Wieso gehst du dann nicht in die PVP Arena !');
INSERT INTO `flyff_spawns_npcs` VALUES ('553', '214', '100', 'MaFl_Marche', '15', '1', '7169.17', '100', '3271.16', '68', '10', 'Hey ich bins, Marche! Hast du immer schon von einer fatastischen magischen Welt geträumt ? ');
INSERT INTO `flyff_spawns_npcs` VALUES ('550', '913', '100', 'MaFl_COLINSE', '15', '1', '6943.97', '100.138', '3101.11', '36', '10', 'Hey ich tausche deine Item Teile gegen ein Ganzes um .');
INSERT INTO `flyff_spawns_npcs` VALUES ('549', '910', '100', 'MaFl_Waforu', '15', '1', '6982.49', '100', '3351.13', '63', '10', 'Du kannst sehr wertvolle Dinge hier bekommen !');
INSERT INTO `flyff_spawns_npcs` VALUES ('548', '224', '100', 'MaFl_Annie', '15', '1', '6988.87', '100', '3321.75', '52', '10', 'Bitte besuch mich wenn du Fragen über denn 1vs1 Gildenkrieg hast .');
INSERT INTO `flyff_spawns_npcs` VALUES ('547', '223', '100', 'MaFl_Amos', '15', '1', '6989.11', '100', '3325.93', '53', '10', 'Sind deine Mitglieder die stärksten ? Dann trete dem 1vs1 Gildenkrieg bei und beweise es !');
INSERT INTO `flyff_spawns_npcs` VALUES ('632', '237', '100', 'KePe_Heron', '15', '500', '1268.85', '115', '1351.55', '0', '10', '');
INSERT INTO `flyff_spawns_npcs` VALUES ('633', '222', '100', 'KePe_Shun', '15', '500', '1286.43', '115', '1365.53', '57', '10', '');
INSERT INTO `flyff_spawns_npcs` VALUES ('634', '223', '100', 'KePe_Rocbin', '15', '500', '1286.9', '115', '1358.82', '57', '10', '');
INSERT INTO `flyff_spawns_npcs` VALUES ('635', '221', '100', 'KePe_Yuna', '15', '500', '1251.63', '114.98', '1349.08', '26', '10', '');
INSERT INTO `flyff_spawns_npcs` VALUES ('543', '211', '100', 'MaFl_Bobochan', '15', '1', '6928.26', '100', '3227.87', '24', '10', 'Du kannst deine Waffe nicht verstärken ,wenn du sie nicht upgradest .');
INSERT INTO `flyff_spawns_npcs` VALUES ('636', '225', '100', 'KePe_Ciel', '15', '500', '1250.2', '115', '1372.78', '13', '10', '');
INSERT INTO `flyff_spawns_npcs` VALUES ('542', '878', '100', 'MaFl_HairShop', '15', '1', '6958.22', '100', '3349.97', '0', '10', '');
INSERT INTO `flyff_spawns_npcs` VALUES ('541', '879', '100', 'MaFl_FaceOff', '15', '1', '6961.31', '100', '3349.83', '0', '10', '');
INSERT INTO `flyff_spawns_npcs` VALUES ('540', '880', '100', 'MaFl_PetTamer', '15', '1', '6989.56', '100', '3256.07', '59', '10', 'Ein Pet ist der beste Partner in Madrigal .');
INSERT INTO `flyff_spawns_npcs` VALUES ('538', '12', '100', 'MaFl_FlaMayor', '15', '1', '6960.03', '100', '3227.32', '0', '10', '');
INSERT INTO `flyff_spawns_npcs` VALUES ('539', '873', '100', 'MaFl_Postbox', '15', '1', '6957.51', '100', '3211.72', '36', '10', '');
INSERT INTO `flyff_spawns_npcs` VALUES ('537', '219', '100', 'MaFl_Ata', '15', '1', '6703.56', '136.085', '3326.79', '25', '10', 'Hmm Es ist sehr schwer Noie\'s Planet zu verfolgen in der Amendus Galaxy .');
INSERT INTO `flyff_spawns_npcs` VALUES ('535', '230', '100', 'MaFl_Donaris', '15', '1', '6989.05', '100', '3331.15', '54', '10', 'Willst du das Ergebnis des letzten Gildenkriegs wissen ? Ich lass es dich wissen .');
INSERT INTO `flyff_spawns_npcs` VALUES ('536', '226', '100', 'MaFl_Gergantes', '15', '1', '6960.13', '100', '3339.73', '0', '10', 'This will be good material for my novel, but I do not know how I should start it! Hmmm..');
INSERT INTO `flyff_spawns_npcs` VALUES ('534', '213', '100', 'MaFl_GuildWar', '15', '1', '6989.21', '100', '3335.09', '53', '10', 'Wenn ihr mutig und Weise seid , dann kommt zusammen ! Der Gildenkrieg erwartet euch !');
INSERT INTO `flyff_spawns_npcs` VALUES ('533', '232', '100', 'MaFl_Himadel', '15', '1', '6835.97', '108.616', '3132.56', '17', '10', '');
INSERT INTO `flyff_spawns_npcs` VALUES ('532', '233', '100', 'MaFl_Martinyc', '15', '1', '6998.79', '100', '3203.15', '33', '10', 'Oh Die Geschichte von Madrigal.diese ist sehr seltasm. Da ist definitiv etwas seltsam ... Definitiv ');
INSERT INTO `flyff_spawns_npcs` VALUES ('531', '220', '100', 'MaFl_Official', '15', '1', '6960.41', '100', '3204.27', '35', '10', 'Hallo . Wilkommenbeim  Office von Flarine.');
INSERT INTO `flyff_spawns_npcs` VALUES ('530', '218', '100', 'MaFl_Maki', '15', '1', '6930.36', '100.3', '3324.3', '19', '10', 'Jeder der ein Assist sein will . Bubble lächelt dich an . ');
INSERT INTO `flyff_spawns_npcs` VALUES ('529', '214', '100', 'MaFl_Kidmen', '15', '1', '7156.19', '100', '3250.17', '48', '10', 'Breite denn Segen Rhisis über die ganze Welt aus ! Spritze Tränen Rhisis überall auf denn Grund .');
INSERT INTO `flyff_spawns_npcs` VALUES ('528', '214', '100', 'MaFl_Hyuit', '15', '1', '7152.51', '99.9986', '3242.4', '48', '10', 'Zeichne deine Waffe und schlage alles nieder was dir im Weg steht ! Geh forwärts, Mercenary!');
INSERT INTO `flyff_spawns_npcs` VALUES ('527', '226', '100', 'MaFl_Elic', '15', '1', '7155.72', '99.9994', '3248.06', '50', '10', 'Du musst nicht anderen helfen . Hilf dir selber erst !');
INSERT INTO `flyff_spawns_npcs` VALUES ('525', '218', '100', 'MaFl_Andy', '15', '1', '6929.96', '100.3', '3328.84', '19', '10', 'Du wirst aufgeklärt mit der Weissheit von Lillip.');
INSERT INTO `flyff_spawns_npcs` VALUES ('526', '226', '100', 'MaFl_Mustang', '15', '1', '7153.92', '99.9986', '3244.07', '48', '10', 'Nicht jeder kann ein Mercenary sein . Fall du kein Mercenary bist , dann bist du nutzlos .');
INSERT INTO `flyff_spawns_npcs` VALUES ('524', '215', '100', 'MaFl_Luda', '15', '1', '6927.3', '100', '3236.48', '6', '10', 'Du kannst hier Vagrant Schilder kaufen .');
INSERT INTO `flyff_spawns_npcs` VALUES ('523', '218', '100', 'MaFl_Noier', '15', '1', '6703.29', '135.965', '3327.76', '17', '10', 'Das Wetter ist heute nur so-so.');
INSERT INTO `flyff_spawns_npcs` VALUES ('521', '219', '100', 'MaFl_Jeff', '15', '1', '6954.72', '100', '3274.1', '18', '10', '*heul* Es ist so schwer mich auf meine Arbeit zu konzentrieren , wenn ich Kimberley so sehr vermisse');
INSERT INTO `flyff_spawns_npcs` VALUES ('522', '233', '100', 'MaFl_Mikyel', '15', '1', '7129.19', '99.9578', '3252.48', '16', '10', 'Eine Abenteuerwelt erwartet dich . Komm her wenn du Level 7 bist .');
INSERT INTO `flyff_spawns_npcs` VALUES ('520', '211', '100', 'MaFl_Boboko', '15', '1', '6926.99', '100', '3229.93', '24', '10', 'Die perfekten Schilder Bobofamilie sind nur hier im Schildladen von Boboko !');
INSERT INTO `flyff_spawns_npcs` VALUES ('519', '211', '100', 'MaFl_Boboku', '15', '1', '6926.1', '100', '3232.11', '18', '10', 'Die Waffen versiegelt mit dem Geheimnis der Bobofamilie sind hier im Waffenladen von Boboku !');
INSERT INTO `flyff_spawns_npcs` VALUES ('517', '212', '100', 'MaFl_Ispim', '15', '1', '7160.07', '100', '3221.46', '35', '10', 'Kann ich dir helfen ? Wenn du Fragen hast , dann frag nur .');
INSERT INTO `flyff_spawns_npcs` VALUES ('518', '212', '100', 'MaFl_Isruel', '15', '1', '6937.83', '100', '3243.34', '17', '10', 'Hi kann ich helfen ? Wenn du Fragen hast , dann frag .');
INSERT INTO `flyff_spawns_npcs` VALUES ('516', '212', '100', 'MaFl_Ismeralda', '15', '1', '7042.26', '99.4944', '3244.29', '54', '10', 'Kann ich dir helfen ? Wenn du Fragen hast , dann frag nur .');
INSERT INTO `flyff_spawns_npcs` VALUES ('515', '212', '100', 'MaFl_Is', '15', '1', '6960.07', '100', '3266.1', '0', '10', 'Kann ich dir helfen ? Wenn du Fragen hast , dann frag nur .');
INSERT INTO `flyff_spawns_npcs` VALUES ('514', '216', '100', 'MaFl_Dior', '15', '1', '6957.96', '100', '3238.35', '64', '10', 'Wilkommen bei Dior Station. Du findest hier Fluginformationen .');
INSERT INTO `flyff_spawns_npcs` VALUES ('402', '215', '100', 'MaCa_Saville', '15', '1', '8066.49', '81.3653', '2654.44', '177', '10', 'Pass in diesen Höhlen auf !');
INSERT INTO `flyff_spawns_npcs` VALUES ('599', '225', '100', 'MaSa_Wingyei', '15', '1', '8483.72', '100.6', '3810.27', '36', '10', 'Bist du bereit diese Belastung dein Leben lang zu tragen ? Hast du Vertrauen in Magie ?');
INSERT INTO `flyff_spawns_npcs` VALUES ('600', '214', '100', 'MaSa_Lopaze', '15', '1', '8481.51', '100', '3810.4', '35', '10', 'Ehre denn Willen von Rhisis! Zeige die Wut von Rhisis! Sei verflucht von Rhisis!');
INSERT INTO `flyff_spawns_npcs` VALUES ('400', '221', '100', 'MaCa_Charlotte', '15', '1', '7608.53', '156.48', '4277.84', '353', '10', 'Hallo .');
INSERT INTO `flyff_spawns_npcs` VALUES ('401', '219', '100', 'MaCa_DuFl_James', '15', '200', '1018', '80', '1107', '240', '10', 'Sei vorsichtig .');
INSERT INTO `flyff_spawns_npcs` VALUES ('513', '213', '100', 'MaFl_Lui', '15', '1', '6975.66', '100', '3266.87', '60', '10', 'Ich habe alles was du brauchst ! Mein Name ist Lui und das ist mein Allgemeinladen !');
INSERT INTO `flyff_spawns_npcs` VALUES ('512', '212', '100', 'MaFl_Juria', '15', '1', '6958.43', '100', '3211.79', '36', '10', 'Alle Informationen die du brauchs findest du beim Office. Bitte besuch es wenn du Hilfe brauchst .');
INSERT INTO `flyff_spawns_npcs` VALUES ('510', '219', '100', 'MaSa_MaYun', '15', '1', '8197.68', '84.7928', '2805.67', '5', '10', 'Oh, cool! Wow! Ich will wirklich eins haben !');
INSERT INTO `flyff_spawns_npcs` VALUES ('509', '220', '100', 'MaFl_DrEstern', '15', '1', '7940.8', '159.076', '2405.93', '0', '10', 'Saint Morning, der schönste Platz in Madrigal! Die Ökologie hier ist der Segen des Gottes selber .');
INSERT INTO `flyff_spawns_npcs` VALUES ('507', '219', '100', 'MaDa_Colar', '15', '1', '6133.4', '100', '4168.77', '0', '10', 'Hilfe ! Hilf mir ! Lass mich raus !');
INSERT INTO `flyff_spawns_npcs` VALUES ('508', '913', '100', 'MaFl_COLINSE', '15', '1', '3851.17', '94.7694', '4660.1', '65', '10', '');
INSERT INTO `flyff_spawns_npcs` VALUES ('506', '891', '100', 'MaMa_PKNPC01', '15', '1', '5482.89', '71.3512', '4362.69', '12', '10', '');
INSERT INTO `flyff_spawns_npcs` VALUES ('504', '223', '100', 'MaFl_Ray', '15', '1', '3801.63', '59', '4448.15', '9', '10', 'Denkst d das du stark genug bist ? Wieso gehst du dann nicht in die PVP Arena ?');
INSERT INTO `flyff_spawns_npcs` VALUES ('505', '220', '100', 'MaDa_DrEst', '15', '1', '5541.34', '71.6196', '4352.41', '0', '10', 'Das Ökosystem in Madrigal ist das Geheimnis die Essenz selber . Schau wie schön es ist !');
INSERT INTO `flyff_spawns_npcs` VALUES ('503', '880', '100', 'MaFl_PetTamer', '15', '1', '3824.43', '59', '4462.05', '69', '10', 'Ein Pet ist der beste Partner in Madrigall.');
INSERT INTO `flyff_spawns_npcs` VALUES ('502', '223', '100', 'MaDa_Roocky', '15', '1', '3742.82', '59', '4419.58', '15', '10', 'Die besten Schilder sind hier . Ich bin Rocky der  Rüstungskünstler .');
INSERT INTO `flyff_spawns_npcs` VALUES ('500', '12', '100', 'MaDa_DarMayor', '15', '1', '3831.63', '59.1', '4457.59', '67', '10', 'Ich werde mein bestes Tun für die Zufriedenheit der Einwohner in Darkon ! Glaub mir .');
INSERT INTO `flyff_spawns_npcs` VALUES ('501', '873', '100', 'MaFl_Postbox', '15', '1', '3834.11', '59', '4456.45', '61', '10', '');
INSERT INTO `flyff_spawns_npcs` VALUES ('499', '12', '100', 'MaDa_Eliff', '15', '1', '3790.86', '59.6', '4492.96', '8', '10', 'Oh Ranger, Der Wille von Hainan wird dich führen .');
INSERT INTO `flyff_spawns_npcs` VALUES ('498', '12', '100', 'MaDa_Lorein', '15', '1', '3784.56', '59.1', '4490.19', '8', '10', 'Die Seele des Jesterhelden , Curenen, wird sie beschützen .');
INSERT INTO `flyff_spawns_npcs` VALUES ('497', '12', '100', 'MaDa_Horison', '15', '1', '3795.17', '59.1', '4498.27', '9', '10', 'Lilip! Elementorheldin ! Bitte beschütze diese Welt mit deiner Weissheit .');
INSERT INTO `flyff_spawns_npcs` VALUES ('496', '12', '100', 'MaDa_Ellend', '15', '1', '3798.54', '59.2', '4490.97', '8', '10', 'Eiene, die Ringmasterheldin , an deinen Segen dieser Welt wird man sich immer errinern .');
INSERT INTO `flyff_spawns_npcs` VALUES ('495', '12', '100', 'MaDa_Corel', '15', '1', '3796.3', '59.2', '4487.33', '9', '10', 'Heren, dein Wunsch hat sich endgültig in dieser Welt verwirklicht .');
INSERT INTO `flyff_spawns_npcs` VALUES ('494', '12', '100', 'MaDa_Karanduru', '15', '1', '3793.46', '59.1', '4482.55', '9', '10', 'Billeie! Die Ehre die du gemacht hast wird sich durch die ganze Welt verbreiten .');
INSERT INTO `flyff_spawns_npcs` VALUES ('493', '12', '100', 'MaDa_Cylor', '15', '1', '3781.48', '59.1', '4485.05', '10', '10', 'Offerep, Psykeeperheld , Ich bin sehr glücklich deinem Geist zu ehren .');
INSERT INTO `flyff_spawns_npcs` VALUES ('492', '12', '100', 'MaDa_Ride', '15', '1', '3788.05', '59.1', '4481.21', '13', '10', 'Roentel, der Samen den du in dieser Welt gesät hast wird gründlich geerntet .');
INSERT INTO `flyff_spawns_npcs` VALUES ('491', '213', '100', 'MaDa_Bernard', '15', '1', '3827.02', '59', '4548.46', '0', '10', '');
INSERT INTO `flyff_spawns_npcs` VALUES ('490', '201', '100', 'MaDa_Liekyen', '15', '1', '3733.89', '59', '4525.85', '65', '10', 'Yo, ich bin ein glücklicher Abenteurer . Ich lebe für Abenteuer!');
INSERT INTO `flyff_spawns_npcs` VALUES ('489', '218', '100', 'MaDa_Pyre', '15', '1', '3850.12', '59', '4425.96', '36', '10', 'Entstehe ... aus der Dunkelheit . Du trägst nun denn scheinenden Name - denn Titel des Acrobats !');
INSERT INTO `flyff_spawns_npcs` VALUES ('488', '214', '100', 'MaDa_Tailer', '15', '1', '3931.98', '59.0206', '4389.94', '55', '10', 'Bist du bereit ?');
INSERT INTO `flyff_spawns_npcs` VALUES ('486', '223', '100', 'MaDa_Krinton', '15', '1', '3742.22', '59', '4413.74', '24', '10', 'Ich verkaufe Rüstungen . Bitte schau dich um .');
INSERT INTO `flyff_spawns_npcs` VALUES ('487', '226', '100', 'MaDa_Hent', '15', '1', '3931.3', '59.0073', '4385.88', '46', '10', 'Gentlemen! Sei ehrgeizig ! Falls du es dir wünscht , dann wird es wahr werden !');
INSERT INTO `flyff_spawns_npcs` VALUES ('484', '227', '100', 'MaDa_Bolpor', '15', '1', '3831.7', '59', '4356.61', '43', '10', 'Wilkommen , wilkommen . Mein Name ist Bolpor und ich bin der Nahrungsverkäufer.');
INSERT INTO `flyff_spawns_npcs` VALUES ('485', '233', '100', 'MaDa_Lurif', '15', '1', '3841.46', '59', '4422.95', '39', '10', 'Jeder Abenteurer muss diese Quests machen . Du musst mindestens level 51 sein . ');
INSERT INTO `flyff_spawns_npcs` VALUES ('483', '225', '100', 'MaDa_Haven', '15', '1', '3908.94', '59', '4376.03', '46', '10', 'Willst du herrliche Magie benutzen ? Dann besuche denn Magieladen der mir gehört , Haven !');
INSERT INTO `flyff_spawns_npcs` VALUES ('482', '229', '100', 'MaDa_Achaben', '15', '1', '3879.86', '59', '4458.07', '0', '10', 'Wilkommen im Allgemeinladen von  Achaben! *hust*');
INSERT INTO `flyff_spawns_npcs` VALUES ('481', '228', '100', 'MaDa_Almani', '15', '1', '3846.71', '59', '4443.02', '0', '10', 'Fluginformationen sind hier verfügbar !');
INSERT INTO `flyff_spawns_npcs` VALUES ('479', '231', '100', 'MaDa_Remine', '15', '1', '3748.58', '59', '4412.3', '15', '10', 'Willst du die beste Waffe in Madrigal? Dann komm her , du wirst nicht widerstehen .');
INSERT INTO `flyff_spawns_npcs` VALUES ('480', '224', '100', 'MaDa_Ollien', '15', '1', '3829.78', '59.1', '4457.24', '1', '10', 'Haben sie fragen übers Office? Dann sollst du mich besuchen , Olien.');
INSERT INTO `flyff_spawns_npcs` VALUES ('478', '232', '100', 'MaDa_Eshylop', '15', '1', '3890.03', '58.8415', '4145.79', '36', '10', 'Eine grosse Kreatur schläft hier . Sie wartet auf deine Herausforderung .');
INSERT INTO `flyff_spawns_npcs` VALUES ('477', '12', '100', 'MaDa_Heedan', '15', '1', '3273.11', '132', '4250.09', '8', '10', 'Aenn, deine Tränen brachten Frieden in diese Welt .');
INSERT INTO `flyff_spawns_npcs` VALUES ('476', '891', '100', 'MaMa_PKNPC01', '15', '1', '6589.83', '100.1', '3822.05', '0', '10', '');
INSERT INTO `flyff_spawns_npcs` VALUES ('475', '228', '100', 'MaDa_CloneHachal', '15', '1', '6552.24', '108.244', '3618.51', '40', '10', '');
INSERT INTO `flyff_spawns_npcs` VALUES ('474', '880', '100', 'MaFl_PetTamer', '15', '1', '5569.15', '75.0194', '3898.98', '30', '10', 'Ein Pet ist der beste Partner in Madrigal.');
INSERT INTO `flyff_spawns_npcs` VALUES ('473', '213', '100', 'MaDa_Amadolka', '15', '1', '5600.74', '75.0001', '3767.31', '0', '10', 'Niemand wird diesem Platz drohen solange ich hier bin !');
INSERT INTO `flyff_spawns_npcs` VALUES ('472', '215', '100', 'MaDa_Cell', '15', '1', '5561.09', '75.0287', '3909.13', '0', '10', 'Du kannst verschiedene Metalle und Mineralien in der Mine finden .');
INSERT INTO `flyff_spawns_npcs` VALUES ('471', '235', '100', 'MaMa_Ancimys', '15', '1', '5370.74', '75', '3991.26', '49', '10', 'Psst!! Willst du mit mir spielen !?');
INSERT INTO `flyff_spawns_npcs` VALUES ('470', '222', '100', 'MaDa_Phacham', '15', '1', '5580.7', '75', '3901.84', '46', '10', 'Hey! Hey! Ich habe etwas gutes . Bitte schau kurz !');
INSERT INTO `flyff_spawns_npcs` VALUES ('469', '221', '100', 'MaDa_Stima', '15', '1', '5572.36', '75.0001', '3897.21', '41', '10', 'Alles was du willst ist hier im  Waffenladen von Stima!');
INSERT INTO `flyff_spawns_npcs` VALUES ('468', '201', '100', 'MaDa_Rankashu', '15', '1', '5072.61', '55', '3962.48', '0', '10', 'Niemand kann dich aufhalten dort reinzuspringen , du weisst des es sehr heiss ist .');
INSERT INTO `flyff_spawns_npcs` VALUES ('467', '232', '100', 'MaDa_CloneEshylop', '15', '1', '3889.87', '59', '3923.52', '36', '10', 'Eine grosse Kreatur schläft hier . Sie wartet auf deine Herausforderung .');
INSERT INTO `flyff_spawns_npcs` VALUES ('466', '12', '100', 'MaDa_Rupim', '15', '1', '3170.87', '65.6278', '3644.52', '0', '10', 'Oh Hainan,ich will deinem grossen Willen folgen .');
INSERT INTO `flyff_spawns_npcs` VALUES ('465', '12', '100', 'MaDa_Laloa', '15', '1', '3007.87', '49.1527', '3967.41', '63', '10', 'Oh oh oh, dort sind heute viele Blumen.');
INSERT INTO `flyff_spawns_npcs` VALUES ('464', '226', '100', 'MaDa_Andre', '15', '1', '2991.35', '56.7307', '4063.5', '0', '10', '*Schreit* Ich bin solch ein Feiglong');
INSERT INTO `flyff_spawns_npcs` VALUES ('463', '228', '100', 'MaDa_Hachal', '15', '1', '5535.89', '97', '3462.18', '42', '10', 'Ich brauche mehr Details . Hmm... ist da jemand der mir helfen kann ?');
INSERT INTO `flyff_spawns_npcs` VALUES ('462', '12', '100', 'MaDa_Condram', '15', '1', '3959.78', '31.3685', '3510.14', '65', '10', 'Lillip, deine Weissheit ist Phew, es ist nicht einfach immer das gleiche zu rufen .');
INSERT INTO `flyff_spawns_npcs` VALUES ('461', '12', '100', 'MaDa_Fera', '15', '1', '3524.22', '41.1026', '3547.31', '41', '10', 'Roentel, bitte segne jeden .');
INSERT INTO `flyff_spawns_npcs` VALUES ('460', '222', '100', 'MaDa_Jinitte', '15', '1', '3208.39', '10.1552', '3437.29', '63', '10', 'Wilkommen im Jinitte Restaurant! Wir haben das beste Nomadessen .');
INSERT INTO `flyff_spawns_npcs` VALUES ('459', '222', '100', 'MaDa_Tandy', '15', '1', '3202.74', '10.0689', '3437.85', '5', '10', 'Hier sind hergestellte Waren der Nomads ! Komm zum Allgemeinladen !');
INSERT INTO `flyff_spawns_npcs` VALUES ('458', '222', '100', 'MaDa_Kablloc', '15', '1', '3239.98', '11.6511', '3417.7', '54', '10', 'Wilkommen beim  Kablloc Waffenladen ! Wir haben die besten Waffen der Nomads .');
INSERT INTO `flyff_spawns_npcs` VALUES ('457', '222', '100', 'MaDa_Chenbing', '15', '1', '3238.18', '11.6511', '3421.03', '62', '10', 'Wilkommen zum Schildladen von Chenbing! Wir haben die besten Schilde der Nomads .');
INSERT INTO `flyff_spawns_npcs` VALUES ('456', '223', '100', 'MaDa_Nein', '15', '1', '3947.29', '117.651', '2748.43', '19', '10', 'Mein Leben ins Feuer des roten Lotus zu werfen wäre sensationell !');
INSERT INTO `flyff_spawns_npcs` VALUES ('455', '12', '100', 'MaDa_Jeperdy', '15', '1', '3526.78', '88.5169', '2792.02', '54', '10', 'Heren, breite deine heldenhafte Presenz durch die Welt aus .');
INSERT INTO `flyff_spawns_npcs` VALUES ('454', '12', '100', 'MaDa_Homeit', '15', '1', '2788.06', '191.923', '2885.31', '28', '10', 'Curenen, ich wünsche mir deinen grossen Willen zu folgen .');
INSERT INTO `flyff_spawns_npcs` VALUES ('453', '12', '100', 'MaDa_Lobiet', '15', '1', '3012.95', '93.4913', '2931.39', '18', '10', 'Billeien, unser Held, ich werde deinem Geist folgen .');
INSERT INTO `flyff_spawns_npcs` VALUES ('452', '234', '100', 'MaDa_Kailreard', '15', '1', '3351.37', '146.986', '2063.72', '17', '10', 'Wünscht du dir der Rangerheldin Hainan zu folgen ? Bist du bereit ein Held zu werden ? ');
INSERT INTO `flyff_spawns_npcs` VALUES ('449', '234', '100', 'MaDa_Wendien', '15', '1', '3348.82', '146.927', '2058.48', '8', '10', 'Du ! Willst du der Nachfolger von der Elementorheldin Lillip werden ? Glaubst du an Element ?');
INSERT INTO `flyff_spawns_npcs` VALUES ('450', '234', '100', 'MaDa_Capafe', '15', '1', '3339.11', '146.917', '2068.67', '46', '10', 'Du ! Welcher denn Willen des Helden folgen will ! Bist du bereit um deine Ehre zu zeigen ?');
INSERT INTO `flyff_spawns_npcs` VALUES ('451', '234', '100', 'MaDa_Heingard', '15', '1', '3336.71', '146.9', '2063.72', '54', '10', 'Du ! Willst du der Nachfolger von dem Jesterheld Curenen werden ? Bist du bereit ein Held zu werden?');
INSERT INTO `flyff_spawns_npcs` VALUES ('448', '234', '100', 'MaDa_Romvoette', '15', '1', '3339.08', '146.882', '2058.45', '63', '10', 'Du ! Welcher der Ringmasterheldin Eiene folnen will . Glaubst du an denn Heldenwillen ?');
INSERT INTO `flyff_spawns_npcs` VALUES ('447', '234', '100', 'MaDa_Shyniff', '15', '1', '3348.99', '146.967', '2068.67', '26', '10', 'Du ! Welcher dem Psykeeperheld Offerep folgen will . Ehrst du den Heldenwillen ?');
INSERT INTO `flyff_spawns_npcs` VALUES ('446', '234', '100', 'MaDa_Sencyrit', '15', '1', '3344.05', '146.952', '2056.1', '0', '10', 'Du ! Welcher dem Bladeheld Heren folgen will . Bist du bereit dem Helden zu folgen ?');
INSERT INTO `flyff_spawns_npcs` VALUES ('445', '234', '100', 'MaDa_Boneper', '15', '1', '3344.03', '146.956', '2071.1', '35', '10', 'Du  ! Welcher dem Knightheld Billeien folgen will .  Bist du bereit schwer zu tragen ?');
INSERT INTO `flyff_spawns_npcs` VALUES ('444', '875', '100', 'MaDa_RedRobeMan', '15', '1', '3343.95', '114.628', '1859.32', '36', '10', 'Hallo , ich bin der mysteriöse Robenmann');
INSERT INTO `flyff_spawns_npcs` VALUES ('213', '231', '100', 'MaCa_Aizel', '15', '1', '7375', '160', '3880', '160', '10', 'Pass in der Höhle auf dich auf .');
INSERT INTO `flyff_spawns_npcs` VALUES ('214', '213', '100', 'MaCa_Ciceron', '15', '1', '7385', '160', '3885', '355', '10', 'Passen sie hier auf !');
INSERT INTO `flyff_spawns_npcs` VALUES ('443', '238', '100', 'MaFl_Ultimate', '15', '500', '1025.5', '115.798', '1015.41', '0', '10', 'Hallo du da, pssst!');
INSERT INTO `flyff_spawns_npcs` VALUES ('442', '205', '100', 'MaFl_KAWIBAWIBO', '15', '500', '1016.51', '117.098', '1014.85', '0', '10', '');
INSERT INTO `flyff_spawns_npcs` VALUES ('441', '204', '100', 'MaFl_FINDWORD', '15', '500', '1018.48', '116.766', '1014.73', '0', '10', '');
INSERT INTO `flyff_spawns_npcs` VALUES ('440', '203', '100', 'MaFl_FIVESYSTEM', '15', '500', '1020.65', '116.394', '1015.28', '0', '10', '');
INSERT INTO `flyff_spawns_npcs` VALUES ('439', '202', '100', 'MaFl_REASSEMBLE', '15', '500', '1023.35', '116', '1015.63', '0', '10', '');
INSERT INTO `flyff_spawns_npcs` VALUES ('438', '222', '100', 'MaSC_SCWeapon10', '15', '120', '1077.56', '98.0003', '1264.61', '0', '10', '');
INSERT INTO `flyff_spawns_npcs` VALUES ('437', '222', '100', 'MaSC_SCArmor10', '15', '120', '1077.06', '98', '1288.79', '0', '10', '');
INSERT INTO `flyff_spawns_npcs` VALUES ('436', '212', '100', 'MaSC_SCGirl10', '15', '120', '1097.57', '98', '1276.62', '0', '10', '');
INSERT INTO `flyff_spawns_npcs` VALUES ('435', '222', '100', 'MaSC_SCArmor9', '15', '120', '1240.86', '99', '1101.17', '0', '10', '');
INSERT INTO `flyff_spawns_npcs` VALUES ('434', '222', '100', 'MaSC_SCWeapon9', '15', '120', '1257.41', '99', '1109.77', '0', '10', '');
INSERT INTO `flyff_spawns_npcs` VALUES ('433', '212', '100', 'MaSC_SCGirl9', '15', '120', '1250.46', '99', '1119.04', '0', '10', '');
INSERT INTO `flyff_spawns_npcs` VALUES ('432', '222', '100', 'MaSC_SCArmor8', '15', '120', '1443.57', '91.5837', '1173.66', '0', '10', '');
INSERT INTO `flyff_spawns_npcs` VALUES ('431', '222', '100', 'MaSC_SCWeapon8', '15', '120', '1459.83', '91.5958', '1182.93', '0', '10', '');
INSERT INTO `flyff_spawns_npcs` VALUES ('430', '212', '100', 'MaSC_SCGirl8', '15', '120', '1433.41', '91.8974', '1192.73', '0', '10', '');
INSERT INTO `flyff_spawns_npcs` VALUES ('429', '222', '100', 'MaSC_SCWeapon7', '15', '120', '1447.68', '92.5928', '1382.41', '0', '10', '');
INSERT INTO `flyff_spawns_npcs` VALUES ('428', '222', '100', 'MaSC_SCArmor7', '15', '120', '1434.14', '92.5928', '1400.46', '0', '10', '');
INSERT INTO `flyff_spawns_npcs` VALUES ('427', '212', '100', 'MaSC_SCGirl7', '15', '120', '1423.04', '92.6826', '1385.88', '0', '10', '');
INSERT INTO `flyff_spawns_npcs` VALUES ('426', '222', '100', 'MaSC_SCArmor6', '15', '120', '1228.95', '94.2322', '1474.86', '0', '10', '');
INSERT INTO `flyff_spawns_npcs` VALUES ('425', '222', '100', 'MaSC_SCWeapon6', '15', '120', '1207.85', '94.2258', '1465.09', '0', '10', '');
INSERT INTO `flyff_spawns_npcs` VALUES ('424', '212', '100', 'MaSC_SCGirl6', '15', '120', '1223.8', '94.2442', '1447.76', '0', '10', '');
INSERT INTO `flyff_spawns_npcs` VALUES ('423', '222', '100', 'MaSC_SCArmor5', '15', '120', '1146.23', '100', '1161.47', '0', '10', '');
INSERT INTO `flyff_spawns_npcs` VALUES ('422', '222', '100', 'MaSC_SCWeapon5', '15', '120', '1138.51', '100.103', '1176.26', '0', '10', '');
INSERT INTO `flyff_spawns_npcs` VALUES ('421', '222', '100', 'MaSC_SCArmor4', '15', '120', '1334.44', '90', '1093.89', '0', '10', '');
INSERT INTO `flyff_spawns_npcs` VALUES ('420', '222', '100', 'MaSC_SCWeapon4', '15', '120', '1368.05', '90.0078', '1102.56', '0', '10', '');
INSERT INTO `flyff_spawns_npcs` VALUES ('419', '222', '100', 'MaSC_SCArmor3', '15', '120', '1473.91', '100', '1255.52', '0', '10', '');
INSERT INTO `flyff_spawns_npcs` VALUES ('418', '222', '100', 'MaSC_SCWeapon3', '15', '120', '1474.02', '100.288', '1273.43', '0', '10', '');
INSERT INTO `flyff_spawns_npcs` VALUES ('417', '222', '100', 'MaSC_SCArmor2', '15', '120', '1392.48', '99.31', '1449.02', '0', '10', '');
INSERT INTO `flyff_spawns_npcs` VALUES ('416', '222', '100', 'MaSC_SCWeapon2', '15', '120', '1368.05', '99.2328', '1449.35', '0', '10', '');
INSERT INTO `flyff_spawns_npcs` VALUES ('415', '222', '100', 'MaSC_SCWeapon1', '15', '120', '1136.87', '97.5937', '1410.27', '0', '10', '');
INSERT INTO `flyff_spawns_npcs` VALUES ('414', '222', '100', 'MaSC_SCArmor1', '15', '120', '1148.59', '100', '1419.95', '0', '10', '');
INSERT INTO `flyff_spawns_npcs` VALUES ('413', '212', '100', 'MaSC_SCGirl5', '15', '120', '1155.85', '100', '1181.24', '0', '10', '');
INSERT INTO `flyff_spawns_npcs` VALUES ('412', '212', '100', 'MaSC_SCGirl4', '15', '120', '1341.82', '90.0006', '1127.8', '0', '10', '');
INSERT INTO `flyff_spawns_npcs` VALUES ('411', '212', '100', 'MaSC_SCGirl3', '15', '120', '1448.81', '100.006', '1263.67', '0', '10', '');
INSERT INTO `flyff_spawns_npcs` VALUES ('410', '212', '100', 'MaSC_SCGirl1', '15', '120', '1153.69', '100', '1403.85', '0', '10', '');
INSERT INTO `flyff_spawns_npcs` VALUES ('409', '212', '100', 'MaSC_SCGirl2', '15', '120', '1373.9', '98.1548', '1425.89', '0', '10', '');
INSERT INTO `flyff_spawns_npcs` VALUES ('408', '213', '100', 'MaFl_Dick', '15', '208', '541.738', '138.176', '512.083', '60', '10', '');
INSERT INTO `flyff_spawns_npcs` VALUES ('407', '223', '100', 'MaFl_Harold', '15', '208', '545.548', '138.624', '485.773', '50', '10', '');
INSERT INTO `flyff_spawns_npcs` VALUES ('406', '232', '100', 'MaSa_Daz', '15', '204', '1075.56', '5.28125', '1337.62', '0', '10', '');
INSERT INTO `flyff_spawns_npcs` VALUES ('405', '201', '100', 'DuDk_Kazen', '15', '201', '1618.4', '84.3547', '746.327', '14', '10', '');
INSERT INTO `flyff_spawns_npcs` VALUES ('404', '691', '100', 'DuDk_Nevil', '15', '201', '1073.51', '86.4', '696.314', '0', '10', '');
INSERT INTO `flyff_spawns_npcs` VALUES ('403', '691', '100', 'DuDk_Drian', '15', '201', '829.026', '85.0022', '966.924', '0', '10', '');
INSERT INTO `flyff_spawns_npcs` VALUES ('601', '223', '100', 'MaSa_Helgar', '15', '1', '8449.09', '100', '3638.13', '16', '10', 'Haben Sie etwas seltenes oder kostbares ?');
INSERT INTO `flyff_spawns_npcs` VALUES ('602', '691', '100', 'MaSa_QueerCollector', '15', '1', '8383.2', '145', '4085.15', '0', '10', 'Hallo! Hehe');
INSERT INTO `flyff_spawns_npcs` VALUES ('603', '225', '100', 'MaSa_JeongHwa', '15', '1', '8334.95', '100', '3713.43', '45', '10', 'Bitte finde mein Kind ! Bitte , Bitte!');
INSERT INTO `flyff_spawns_npcs` VALUES ('604', '213', '100', 'MaSa_Porgo', '15', '1', '8532.44', '100', '3607.49', '67', '10', 'Es ist seltsam . Es ist eigenartig . Hmm ...');
INSERT INTO `flyff_spawns_npcs` VALUES ('605', '228', '100', 'MaSa_Gothante', '15', '1', '8353.02', '99.7752', '3983.67', '63', '10', 'Ha ha ha ! Die Geschichte von Madrigal ist sehr mysteriös . Stimmts ? Ha ha ha');
INSERT INTO `flyff_spawns_npcs` VALUES ('606', '221', '100', 'MaSa_Parine', '15', '1', '8414.34', '140.141', '3974.8', '0', '10', 'Wilkommen bei der Saint Morning Station ! Wir bieten detailierte und sichere Flüge .');
INSERT INTO `flyff_spawns_npcs` VALUES ('607', '12', '100', 'MaSa_SainMayor', '15', '1', '8464.29', '100', '3651.49', '0', '10', 'Jeder in Saint Morning , seid ihr glücklich ? Ich mache das beste was ich tun kann .');
INSERT INTO `flyff_spawns_npcs` VALUES ('637', '212', '100', 'MaFl_Juria', '15', '500', '1270.65', '115', '1351.68', '0', '10', 'Alle Informationen die du brauchst findest du im Office . Bitte besuch es , falls du Hilfe brauchst ');
INSERT INTO `flyff_spawns_npcs` VALUES ('609', '873', '100', 'MaFl_Postbox', '15', '1', '8462.63', '100', '3600.31', '35', '10', '');
INSERT INTO `flyff_spawns_npcs` VALUES ('610', '880', '100', 'MaFl_PetTamer', '15', '1', '8465.42', '100', '3622.85', '0', '10', 'Ein Pet ist der wichtigste Partner in Madrigal');
INSERT INTO `flyff_spawns_npcs` VALUES ('611', '913', '100', 'MaFl_COLINSE', '15', '1', '8636.33', '100.195', '3738.45', '52', '10', '');
INSERT INTO `flyff_spawns_npcs` VALUES ('613', '223', '100', 'MaFl_Ray', '15', '1', '8437.54', '100', '3616.37', '17', '10', 'Denkst du , dass du stark genug bist ? Wieso gehst du dann nicht in die PVP Arena !');
INSERT INTO `flyff_spawns_npcs` VALUES ('614', '201', '100', 'MaSa_Bowler', '15', '1', '9560.33', '90.5346', '4020.01', '54', '10', '');
INSERT INTO `flyff_spawns_npcs` VALUES ('615', '218', '100', 'MaFl_SgRadion', '15', '1', '7640.48', '175.12', '4254.25', '36', '10', 'Hmm ... da ist etwas in der Höhle .');
INSERT INTO `flyff_spawns_npcs` VALUES ('616', '218', '100', 'MaFl_Radyon', '15', '1', '7635.97', '169.02', '4229.07', '21', '10', 'Hmm ... Es scheint immer mehr Leute wünschen sich ein Held zu sein . Ich habe nie gedacht , dass ich');
INSERT INTO `flyff_spawns_npcs` VALUES ('617', '218', '100', 'MaFl_Kimel', '15', '1', '7636.03', '168.84', '4227.36', '18', '10', 'Hahaha! Es liegt an uns sie zum richtigem Weg zu führen .');
INSERT INTO `flyff_spawns_npcs` VALUES ('618', '218', '100', 'MaFl_Hormes', '15', '1', '7677.74', '179.378', '4252.2', '63', '10', 'Hmm.. Jahrzehnte sind vergangen seitdem ich dort gerarbeitet habe . Verdammt !');
INSERT INTO `flyff_spawns_npcs` VALUES ('619', '218', '100', 'MaFl_Guabrill', '15', '1', '7678.73', '179.091', '4251.53', '63', '10', 'Haha, beklag dich nicht so viel . Was wir machen ist nicht umsonst . Stimmts ?');
INSERT INTO `flyff_spawns_npcs` VALUES ('620', '218', '100', 'MaFl_Cuzrill', '15', '1', '7655.6', '161.675', '4191.63', '35', '10', 'Bitte genieße dein Abenteuer ! Ha ha ha!');
INSERT INTO `flyff_spawns_npcs` VALUES ('621', '218', '100', 'MaFl_Cuarine', '15', '1', '7657.55', '161.695', '4191.51', '35', '10', 'Gleich..genau der gleiche. Du bist wirklich erstaunlich . Ha ha ha.');
INSERT INTO `flyff_spawns_npcs` VALUES ('622', '222', '100', 'MaFl_Segho', '15', '1', '7644.78', '236.285', '4349.11', '50', '10', 'Wie wäre es wenn wir heute diesen Gipfel erklimmen ? Hahaha ! Ich scherze nur !');
INSERT INTO `flyff_spawns_npcs` VALUES ('623', '218', '100', 'MaFl_Domek', '15', '1', '7693.45', '170.711', '4227.5', '53', '10', 'Ha ha ha ! Es scheint so als würde ich hier sehr gut reinpassen . Stimmts ? Ha ha ha~\\');
INSERT INTO `flyff_spawns_npcs` VALUES ('624', '218', '100', 'MaFl_Clamb', '15', '1', '7693.29', '170.203', '4225.89', '53', '10', 'Wirklich ? Vielleicht denkst du so ,weil du ein Schreiber bist .Aber ich will keine Unruhe über das ');
INSERT INTO `flyff_spawns_npcs` VALUES ('625', '891', '100', 'MaMa_PKNPC01', '15', '1', '8247.27', '95.5846', '2525.55', '26', '10', '');
INSERT INTO `flyff_spawns_npcs` VALUES ('626', '12', '100', 'MaDa_Pereb', '15', '1', '2522.29', '55', '4599.88', '17', '10', 'Bitte stärke diese Welt mit deinem starkem Geist .');
INSERT INTO `flyff_spawns_npcs` VALUES ('627', '232', '100', 'MaMa_PKNPC01', '15', '203', '501.705', '87.2175', '591.755', '23', '10', '');
INSERT INTO `flyff_spawns_npcs` VALUES ('628', '876', '100', 'MaDa_RedRobeGirl', '15', '203', '1034.63', '170.972', '959.306', '70', '10', 'Hi! Ich bin das mysteriöse , rote Räubermädchen!');
INSERT INTO `flyff_spawns_npcs` VALUES ('629', '232', '100', 'MaDa_GateKeeper', '15', '203', '554.916', '88.5312', '1493.49', '7', '10', '');
INSERT INTO `flyff_spawns_npcs` VALUES ('631', '882', '100', 'MaFl_FIVESYSTEM', '15', '207', '493.367', '60.6908', '495.558', '0', '10', '');
INSERT INTO `flyff_spawns_npcs` VALUES ('638', '910', '100', 'MaCa_WdBeginner_Hank', '15', '501', '129.525', '128.921', '330.972', '315', '10', 'Brauchst du ertwas ?');
INSERT INTO `flyff_spawns_npcs` VALUES ('639', '892', '100', 'MaCa_WdBeginner_Russell', '15', '501', '130.023', '126.19', '328.188', '90', '10', 'Pass auf .');
INSERT INTO `flyff_spawns_npcs` VALUES ('640', '882', '100', 'MaCa_WdBeginner_Linn', '15', '501', '132', '128', '340', '180', '10', 'Hey ! Was machst du hier ?');
INSERT INTO `flyff_spawns_npcs` VALUES ('641', '215', '100', 'MaCa_WdBeginner_Mark', '15', '501', '141', '130', '327', '255', '10', 'Hmm gehörst du hierher..');
INSERT INTO `flyff_spawns_npcs` VALUES ('642', '881', '100', 'MaCa_WdBeginner_Flow', '15', '501', '139', '130', '316', '270', '10', 'Diese Höhle ist großartig');
INSERT INTO `flyff_spawns_npcs` VALUES ('643', '210', '100', 'MaCa_WdBeginner_Jenny', '15', '501', '122', '130', '288', '78', '10', 'Gefährlich .');
INSERT INTO `flyff_spawns_npcs` VALUES ('644', '913', '100', 'MaCa_WdBeginner_Roy', '15', '501', '145', '129', '273', '200', '10', 'Gehörst du hierher ?');
INSERT INTO `flyff_spawns_npcs` VALUES ('646', '231', '100', 'MaCa_WdBeginner_Payan', '15', '501', '123.284', '131.122', '349.423', '236', '10', 'Hallo , kann ich dir helfen ?');
INSERT INTO `flyff_spawns_npcs` VALUES ('647', '12', '100', 'MaCa_WdBeginner_Nevell', '15', '501', '148.725', '130.372', '360.746', '422', '10', 'Hallo , brauchst du etwas ?');
INSERT INTO `flyff_spawns_npcs` VALUES ('648', '12', '100', 'MaCa_WdBeginner_Plim', '15', '501', '149.335', '130.347', '360.133', '210', '10', 'Die Macht der Höhler , mein junger Padawan .');
INSERT INTO `flyff_spawns_npcs` VALUES ('649', '12', '100', 'MaCa_WdBeginner_Misty', '15', '501', '147.805', '130.407', '361.026', '0', '10', 'Grossartiger Platz .');
INSERT INTO `flyff_spawns_npcs` VALUES ('656', '21', '100', 'MaCa_WdBeginner_Dwarf03', '15', '501', '80', '131.08', '191', '0', '10', 'Dies ist so viel wert !');
INSERT INTO `flyff_spawns_npcs` VALUES ('655', '21', '100', 'MaCa_WdBeginner_Dwarf02', '15', '501', '81', '130.924', '190', '345', '10', 'Ich mag diesen Platz .');
INSERT INTO `flyff_spawns_npcs` VALUES ('650', '21', '100', 'MaCa_WdBeginner_Legolas', '15', '501', '146.902', '130.423', '360.674', '294', '10', 'Hallo ,du da !');
INSERT INTO `flyff_spawns_npcs` VALUES ('654', '21', '100', 'MaCa_WdBeginner_Dwarf01', '15', '501', '77.5', '133.236', '187', '100', '10', 'Entschuldigung , ich kann dir nicht helfen .');
INSERT INTO `flyff_spawns_npcs` VALUES ('653', '21', '100', 'MaCa_WdBeginner_Ahmed', '15', '501', '144.682', '133.18', '192.089', '45', '10', 'Pass hier auf huhu');
INSERT INTO `flyff_spawns_npcs` VALUES ('658', '200', '100', 'MaFl_Helper_ver12', '15', '1', '6965.85', '100', '3213.2', '175', '10', 'Kann ich dir helfen ?');
