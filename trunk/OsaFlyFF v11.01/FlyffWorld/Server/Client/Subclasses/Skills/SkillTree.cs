using System;
using System.Collections.Generic;
using System.Text;

namespace FlyffWorld
{
    public class SkillTree
    {
        // How skill tree is presented:
        // There will be a public, static function that will return a skilltree for a specific job.
        // The skill trees are made of arrays.
        // The function will construct a new array which contains the skill IDs for the job.
        public static int[] GetTree(int jobID)
        {
            if (!_skilltree_constructed)
            {
                ConstructSkillTree();
                _skilltree_constructed = true;
            }
            return _skilltree[jobID];
        }
        // The skill tree, the real thing:
        // Two-dimensional array, i presents the job ID and j presents the index, [i][j] presents the skill ID!
        private static int[][] _skilltree = new int[32][]; // 32 jobs
        private static bool _skilltree_constructed = false;
        private static void ConstructSkillTree()
        {
            // create skill tree
            _skilltree[0] = new int[]
            {
                1, 2, 3
            };
            _skilltree[1] = new int[] // merc
            {
                1, 2, 3, 4, 5, 6, 9, 10, 108, 109, 111, 112, 7, 8, 11, 12, 13, 14
            };
            _skilltree[2] = new int[] // acro
            {
                1, 2, 3, 191, 192, 193, 194, 195, 196, 197, 198, 199, 200, 201, 202, 203, 204, 205, 206
            };
            _skilltree[3] = new int[] // assist
            {
                1, 2, 3, 44, 45, 46, 48, 49, 50, 51, 52, 53, 113, 114, 115, 116, 117, 20, 104, 105
            };
            _skilltree[4] = new int[] // magician
            {
                1, 2, 3, 64, 65, 69, 70, 118, 119, 120, 121, 30, 31, 32, 33, 34, 35, 36, 37, 107
            };
            _skilltree[5] = new int[0]; // puppeteer
            _skilltree[6] = new int[] // knight
            {
                1, 2, 3, 4, 5, 6, 9, 10, 108, 109, 111, 112, 7, 8, 11, 12, 13, 14, // basic merc skills
                128, 129, 130, 131, 132, 133, 134, 135
            };
            _skilltree[7] = new int[] // blade
            {
                1, 2, 3, 4, 5, 6, 9, 10, 108, 109, 111, 112, 7, 8, 11, 12, 13, 14, // basic merc skills
                136, 137, 138, 139, 140, 141, 142, 143
            };
            _skilltree[8] = new int[] // jester
            {
                1, 2, 3, 191, 192, 193, 194, 195, 196, 197, 198, 199, 200, 201, 202, 203, 204, 205, 206, // basic acro skills
                207, 208, 209, 210, 211, 212, 213, 214
            };
            _skilltree[9] = new int[] // ranger
            {
                1, 2, 3, 191, 192, 193, 194, 195, 196, 197, 198, 199, 200, 201, 202, 203, 204, 205, 206, // basic acro skills
                215, 216, 217, 218, 219, 220, 221, 222
            };
            _skilltree[10] = new int[] // RM
            {
                1, 2, 3, 44, 45, 46, 48, 49, 50, 51, 52, 53, 113, 114, 115, 116, 117, 20, 104, 105, // basic assist skills
                144, 145, 146, 147, 148, 149, 150, 151
            };
            _skilltree[11] = new int[] // BP
            {
                1, 2, 3, 44, 45, 46, 48, 49, 50, 51, 52, 53, 113, 114, 115, 116, 117, 20, 104, 105, // basic assist skills
                152, 153, 154, 155, 156, 157, 158, 159
            };
            _skilltree[12] = new int[] // psykeeper
            {
                1, 2, 3, 64, 65, 69, 70, 118, 119, 120, 121, 30, 31, 32, 33, 34, 35, 36, 37, 107, // basic magician skills
                160, 161, 162, 163, 164, 165, 166, 167
            };
            _skilltree[13] = new int[] // ele
            {
                1, 2, 3, 64, 65, 69, 70, 118, 119, 120, 121, 30, 31, 32, 33, 34, 35, 36, 37, 107, // basic magician skills
                0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 // spot holders
            };
            int imtired = 168; // still ele
            for (int i = 20; i < _skilltree[13].Length; i++) // yep, ele
            {
                _skilltree[13][i] = imtired++; // ***should be 168 - 186***
            }
            _skilltree[14] = new int[0];
            _skilltree[15] = new int[0];
            _skilltree[16] = new int[] // knight master
            {
                1, 2, 3, 4, 5, 6, 9, 10, 108, 109, 111, 112, 7, 8, 11, 12, 13, 14, // basic merc skills
                128, 129, 130, 131, 132, 133, 134, 135, // basic knight skills
                310
            };
            _skilltree[17] = new int[] // blade master
            {
                1, 2, 3, 4, 5, 6, 9, 10, 108, 109, 111, 112, 7, 8, 11, 12, 13, 14, // basic merc skills
                136, 137, 138, 139, 140, 141, 142, 143, // basic blade skills
                309
            };
            _skilltree[18] = new int[] // jester master
            {
                1, 2, 3, 191, 192, 193, 194, 195, 196, 197, 198, 199, 200, 201, 202, 203, 204, 205, 206, // basic acro skills
                207, 208, 209, 210, 211, 212, 213, 214, // basic jester skills
                311
            };
            _skilltree[19] = new int[] // ranger master
            {
                1, 2, 3, 191, 192, 193, 194, 195, 196, 197, 198, 199, 200, 201, 202, 203, 204, 205, 206, // basic acro skills
                215, 216, 217, 218, 219, 220, 221, 222, // basic ranger skills
                312
            };
            _skilltree[20] = new int[] // RM master
            {
                1, 2, 3, 44, 45, 46, 48, 49, 50, 51, 52, 53, 113, 114, 115, 116, 117, 20, 104, 105, // basic assist skills
                144, 145, 146, 147, 148, 149, 150, 151, // basic RM skills
                316
            };
            _skilltree[21] = new int[] // BP master
            {
                1, 2, 3, 44, 45, 46, 48, 49, 50, 51, 52, 53, 113, 114, 115, 116, 117, 20, 104, 105, // basic assist skills
                152, 153, 154, 155, 156, 157, 158, 159, // basic BP skills
                315
            };
            _skilltree[22] = new int[] // psykeeper master
            {
                1, 2, 3, 64, 65, 69, 70, 118, 119, 120, 121, 30, 31, 32, 33, 34, 35, 36, 37, 107, // basic magician skills
                160, 161, 162, 163, 164, 165, 166, 167, // basic psykeeper skills
                314
            };
            _skilltree[23] = new int[] // ele master
            {
                1, 2, 3, 64, 65, 69, 70, 118, 119, 120, 121, 30, 31, 32, 33, 34, 35, 36, 37, 107, // basic magician skills
                0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, // spot holders (basic ele skills)
                313
            };
            imtired = 168; // still ele master
            for (int i = 20; i < _skilltree[23].Length - 1; i++) // yep, ele master (-1 because we do not set the master skill to imtired++)
            {
                _skilltree[23][i] = imtired++; // ***should be 168 - 186***
            }
            _skilltree[24] = new int[] // knight hero
            {
                1, 2, 3, 4, 5, 6, 9, 10, 108, 109, 111, 112, 7, 8, 11, 12, 13, 14, // basic merc skills
                128, 129, 130, 131, 132, 133, 134, 135, // basic knight skills
                310, // master knight skill
                238
            };
            _skilltree[25] = new int[] // blade hero
            {
                1, 2, 3, 4, 5, 6, 9, 10, 108, 109, 111, 112, 7, 8, 11, 12, 13, 14, // basic merc skills
                136, 137, 138, 139, 140, 141, 142, 143, // basic blade skills
                309, // master blade skill
                237
            };
            _skilltree[26] = new int[] // jester hero
            {
                1, 2, 3, 191, 192, 193, 194, 195, 196, 197, 198, 199, 200, 201, 202, 203, 204, 205, 206, // basic acro skills
                207, 208, 209, 210, 211, 212, 213, 214, // basic jester skills
                311, // master jester skill
                239
            };
            _skilltree[27] = new int[] // ranger hero
            {
                1, 2, 3, 191, 192, 193, 194, 195, 196, 197, 198, 199, 200, 201, 202, 203, 204, 205, 206, // basic acro skills
                215, 216, 217, 218, 219, 220, 221, 222, // basic ranger skills
                312, // master ranger skill
                240
            };
            _skilltree[28] = new int[] // RM hero
            {
                1, 2, 3, 44, 45, 46, 48, 49, 50, 51, 52, 53, 113, 114, 115, 116, 117, 20, 104, 105, // basic assist skills
                144, 145, 146, 147, 148, 149, 150, 151, // basic RM skills
                316, // master RM skills
                244
            };
            _skilltree[29] = new int[] // BP hero
            {
                1, 2, 3, 44, 45, 46, 48, 49, 50, 51, 52, 53, 113, 114, 115, 116, 117, 20, 104, 105, // basic assist skills
                152, 153, 154, 155, 156, 157, 158, 159, // basic BP skills
                315, // master BP skill
                243
            };
            _skilltree[30] = new int[] // psykeeper hero
            {
                1, 2, 3, 64, 65, 69, 70, 118, 119, 120, 121, 30, 31, 32, 33, 34, 35, 36, 37, 107, // basic magician skills
                160, 161, 162, 163, 164, 165, 166, 167, // basic psykeeper skills
                314, // master psykeeper skill
                242
            };
            _skilltree[31] = new int[] // ele hero
            {
                1, 2, 3, 64, 65, 69, 70, 118, 119, 120, 121, 30, 31, 32, 33, 34, 35, 36, 37, 107, // basic magician skills
                0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, // spot holders (basic ele skills)
                313, // master ele skill
                241
            };
            imtired = 168; // still ele hero
            for (int i = 20; i < _skilltree[31].Length - 2; i++) // yep, ele hero (-2 because we do not set the master and hero skills to imtired++)
            {
                _skilltree[31][i] = imtired++; // ***should be 168 - 186***
            }
        }
    }
}
