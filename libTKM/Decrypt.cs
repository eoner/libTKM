using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace libTKM
{
    public class TKMDecrypt
    {
        static int[] refTable1 = new int[] { 237, 220, 239, 100, 120, 248, 241, 54, 244, 169, 178, 230, 68, 203, 43, 127, 175, 114, 154, 60, 218, 20, 140, 238, 84, 95, 93, 142, 62, 3, 69, 255, 156, 152, 211, 148, 112, 245, 246, 113, 161, 99, 123, 59, 94, 21, 209, 19, 205, 122, 2, 91, 72, 184, 240, 82, 131, 213, 201, 90, 31, 181, 227, 221, 222, 162, 104, 200, 217, 133, 149, 190, 81, 85, 53, 6, 197, 103, 44, 102, 79, 96, 186, 219, 27, 229, 139, 76, 145, 89, 83, 247, 34, 193, 58, 61, 48, 174, 35, 250, 46, 182, 143, 232, 71, 136, 18, 50, 78, 128, 39, 108, 109, 75, 42, 126, 233, 51, 115, 74, 47, 101, 49, 32, 16, 172, 88, 151, 111, 45, 116, 55, 188, 118, 234, 22, 77, 228, 67, 36, 198, 15, 226, 242, 28, 153, 121, 33, 12, 163, 129, 107, 135, 98, 70, 150, 63, 144, 124, 158, 11, 171, 86, 159, 66, 231, 141, 64, 56, 160, 7, 8, 155, 206, 5, 23, 1, 37, 9, 40, 110, 29, 132, 195, 216, 105, 10, 225, 125, 24, 176, 65, 130, 253, 235, 192, 87, 189, 41, 14, 249, 30, 166, 243, 164, 80, 194, 183, 167, 173, 26, 180, 202, 73, 191, 97, 57, 210, 146, 236, 207, 147, 177, 215, 223, 170, 25, 214, 38, 252, 137, 254, 52, 208, 196, 0, 4, 13, 138, 212, 117, 165, 179, 106, 119, 224, 134, 168, 199, 204, 17, 157, 251, 187, 185, 92 };
        static int[] refTable2 = new int[] { 235, 176, 50, 29, 236, 174, 75, 170, 171, 178, 186, 160, 148, 237, 199, 141, 124, 250, 106, 47, 21, 45, 135, 175, 189, 226, 210, 84, 144, 181, 201, 60, 123, 147, 92, 98, 139, 177, 228, 110, 179, 198, 114, 14, 78, 129, 100, 120, 96, 122, 107, 117, 232, 74, 7, 131, 168, 216, 94, 43, 19, 95, 28, 156, 167, 191, 164, 138, 12, 30, 154, 104, 52, 213, 119, 113, 87, 136, 108, 80, 205, 72, 55, 90, 24, 73, 162, 196, 126, 89, 59, 51, 255, 26, 44, 25, 81, 215, 153, 41, 3, 121, 79, 77, 66, 185, 243, 151, 111, 112, 180, 128, 36, 39, 17, 118, 130, 240, 133, 244, 4, 146, 49, 42, 158, 188, 115, 15, 109, 150, 192, 56, 182, 69, 246, 152, 105, 230, 238, 86, 22, 166, 27, 102, 157, 88, 218, 221, 35, 70, 155, 127, 33, 145, 18, 172, 32, 251, 159, 163, 169, 40, 65, 149, 204, 241, 202, 208, 247, 9, 225, 161, 125, 209, 97, 16, 190, 222, 10, 242, 211, 61, 101, 207, 53, 254, 82, 253, 132, 197, 71, 214, 195, 93, 206, 183, 234, 76, 140, 248, 67, 58, 212, 13, 249, 48, 173, 220, 233, 46, 217, 34, 239, 57, 227, 223, 184, 68, 20, 83, 1, 63, 64, 224, 245, 187, 142, 62, 137, 85, 11, 165, 103, 116, 134, 194, 219, 0, 23, 2, 54, 6, 143, 203, 8, 37, 38, 91, 5, 200, 99, 252, 229, 193, 231, 31 };

        static int KEY_SIZE = 8;
        static int CLEAR_TEXT_LENGTH_SECTION_LENGTH = 7;
        static int KEY_SECTION_LENGTH = 30;
        // static int RANDOM_STRING_LEN = 62;

        static string INT_TO_CHAR_TABLE = (char)(0) + "ZNÇV bCKıUt01ÜLşEaB23OÖ456u7M8S!9ŞFRDAIPHpTĞiü/J+%hrGYsyc&" + "(zn)çvjd=ekğmog?*-öf_İ{l}[]#$@<>;.:\"\'WwQqXx\\\n\r,|~é^" +
           (char)(1) + (char)(2) + (char)(3) + (char)(4) + (char)(5) + (char)(6) + (char)(7) + (char)(8) +
           (char)(9) + (char)(11) + (char)(12) + (char)(14) + (char)(15) + (char)(16) + (char)(17) + (char)(18) + (char)(19) + (char)(20);

        static string HexChars = "0123456789ABCDEF";

        public static string Decrypt0(string cipherText, string key)
        {
            var _loc7_ = 0;
            var _loc14_ = 0;
            var _loc15_ = string.Empty;
            var _loc16_ = 0;
            var _loc17_ = 0;
            var _loc18_ = 0;
            var _loc19_ = 0;
            var _loc20_ = 0;
            var _loc21_ = 0;
            var _loc22_ = 0;
            var _loc3_ = "";
            var _loc4_ = "";
            var _loc5_ = "";
            var _loc6_ = "";
            var _loc8_ = cipherText[(cipherText.Length - 1)] - (char)'0';
            switch (_loc8_)
            {
                case 0:
                    _loc6_ = cipherText;
                    break;
                case 1:
                    _loc6_ = convertFromHexStr(cipherText, -1);
                    break;
                case 2:
                    _loc14_ = cipherText[(cipherText.Length - 2)] - (char)'0';
                    _loc15_ = deShuffleHexStr(cipherText, key, _loc14_, -2);
                    _loc6_ = convertFromHexStr(_loc15_, 0);
                    break;
            }
            var _loc9_ = new int[(KEY_SIZE)];
            _loc7_ = 0;
            while (_loc7_ < KEY_SIZE)
            {
                _loc16_ = (char)_loc6_[(20 + _loc7_)] - 90;
                _loc17_ = (int)(_loc6_[25 + KEY_SIZE + _loc16_] - 90);
                _loc9_[_loc7_] = _loc17_;
                _loc7_++;
            }
            var _loc10_ = 0;
            _loc7_ = 0;
            while (_loc7_ < KEY_SIZE)
            {
                _loc10_ = _loc10_ + _loc9_[_loc7_];
                _loc7_++;
            }
            _loc10_ = _loc10_ % _loc9_[0];
            _loc10_++;
            var _loc11_ = 0;
            _loc7_ = 0;
            while (_loc7_ < 5)
            {
                _loc18_ = _loc6_[(20 + KEY_SIZE + _loc7_)] - (60 + _loc7_);
                _loc5_ = _loc5_ + (char)(_loc18_);
                _loc7_++;
            }
            _loc11_ = Convert.ToInt32(_loc5_);
            _loc4_ = _loc6_.Substring(55 + KEY_SIZE, _loc11_);
            var _loc12_ = _loc11_;
            var _loc13_ = 0;
            _loc7_ = 0;
            while (_loc7_ < _loc12_)
            {
                _loc19_ = (int)(_loc4_[(_loc7_)]);
                _loc20_ = _loc9_[_loc13_ % KEY_SIZE];
                _loc21_ = _loc19_ << 8 - _loc20_ & 255;
                _loc19_ = (_loc19_ >> _loc20_ | _loc21_) & 255;
                _loc22_ = _loc19_;
                _loc22_ = _loc22_ - (int)(_loc10_);
                _loc3_ = _loc3_ + INT_TO_CHAR_TABLE[(_loc22_)];
                _loc13_++;
                _loc7_++;
            }
            return _loc3_;
        }

        /// <summary>
        /// TKMDecryptV12
        /// </summary>
        /// <param name="cipherText"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string Decrypt1(string cipherText, string key)
        {
            var _loc7_ = 0;
            var _loc14_ = 0;
            var _loc15_ = string.Empty;
            var _loc16_ = 0;
            var _loc17_ = 0;
            var _loc18_ = 0;
            var _loc19_ = 0;
            var _loc20_ = 0;
            var _loc21_ = 0;
            var _loc22_ = 0;
            var _loc3_ = "";
            var _loc4_ = "";
            var _loc5_ = "";
            var _loc6_ = "";
            var _loc8_ = cipherText[(cipherText.Length - 1)] - (char)'0';
            switch (_loc8_)
            {
                case 0:
                    _loc6_ = cipherText;
                    break;
                case 1:
                    _loc6_ = convertFromHexStr(cipherText, -1);
                    break;
                case 2:
                    _loc14_ = cipherText[(cipherText.Length - 2)] - (char)'0';
                    _loc15_ = deShuffleHexStr(cipherText, key, _loc14_, -2);
                    _loc6_ = convertFromHexStr(_loc15_, 0);
                    break;
            }
            var _loc9_ = new int[(KEY_SIZE)];
            _loc7_ = 0;
            while (_loc7_ < KEY_SIZE)
            {
                _loc16_ = (char)_loc6_[(20 + _loc7_)] - 90;
                _loc17_ = (int)(_loc6_[20 + CLEAR_TEXT_LENGTH_SECTION_LENGTH+ KEY_SIZE + _loc16_] - 90);
                _loc9_[_loc7_] = _loc17_;
                _loc7_++;
            }
            var _loc10_ = 0;
            _loc7_ = 0;
            while (_loc7_ < KEY_SIZE)
            {
                _loc10_ = _loc10_ + _loc9_[_loc7_];
                _loc7_++;
            }
            _loc10_ = _loc10_ % _loc9_[0];
            _loc10_++;
            var _loc11_ = 0;
            _loc7_ = 0;
            while (_loc7_ < CLEAR_TEXT_LENGTH_SECTION_LENGTH)
            {
                _loc18_ = _loc6_[(20 + KEY_SIZE + _loc7_)] - (60 + _loc7_);
                _loc5_ = _loc5_ + (char)(_loc18_);
                _loc7_++;
            }
            _loc11_ = Convert.ToInt32(_loc5_);
            _loc4_ = _loc6_.Substring(20+KEY_SECTION_LENGTH +CLEAR_TEXT_LENGTH_SECTION_LENGTH + KEY_SIZE);
            var _loc12_ = _loc11_;
            var _loc13_ = 0;
            _loc7_ = 0;
            while (_loc7_ < _loc12_)
            {
                _loc19_ = (int)(_loc4_[(_loc7_)]);
                _loc20_ = _loc9_[_loc13_ % KEY_SIZE];
                _loc21_ = _loc19_ << 8 - _loc20_ & 255;
                _loc19_ = (_loc19_ >> _loc20_ | _loc21_) & 255;
                _loc22_ = _loc19_;
                _loc22_ = _loc22_ - (int)(_loc10_);
                _loc3_ = _loc3_ + INT_TO_CHAR_TABLE[(_loc22_)];
                _loc13_++;
                _loc7_++;
            }
            return _loc3_;
        }

        public static string Decrypt2(string cipherText)
        {
            byte[] inBytes = ASCIIEncoding.UTF8.GetBytes(cipherText);
            byte[] outBytes = new byte[inBytes.Length];

            int key = 3;
            int c1 = 6;
            int c2 = 3; //loc6
            while (c1 < inBytes.Length)
            {
                int i1 = (inBytes[c1++] - 48);
                int i2 = (inBytes[c1++] - 48);

                if (i1 > 9) i1 -= 7;
                if (i2 > 9) i2 -= 7;

                i1 = (i1 << 4) + i2;
                i1 = i1 ^ refTable1[key + (c2 & 15)];
                i1 = refTable2[i1];
                outBytes[c2 - 3] = (byte)i1;
                c2++;
            }

            string clearText=ASCIIEncoding.UTF8.GetString(outBytes);
            return clearText.Substring(0, clearText.IndexOf('\0'));
        }

        protected static string convertFromHexStr(string param1, int param2)
        {
            int _loc6_ = 0;
            int _loc3_ = param1.Length + param2;
            var _loc4_ = string.Empty;
            var _loc5_ = 0;
            while (_loc5_ < _loc3_)
            {
                _loc6_ = (int)(HexChars.IndexOf(param1[_loc5_]) << 4 | HexChars.IndexOf(param1[_loc5_ + 1]));
                _loc4_ = _loc4_ + (char)(_loc6_);
                _loc5_ = _loc5_ + 2;
            }
            return _loc4_;
        }

        protected static string deShuffleHexStr(string param1, string param2, int param3, int param4)
        {
            var _loc5_ = 0;
            var _loc6_ = 0;
            var _loc12_ = 0;
            var _loc13_ = 0;
            var _loc14_ = 0;
            var _loc7_ = new int[(KEY_SIZE)];
            var _loc8_ = new List<int>();
            _loc5_ = 0;
            while (_loc5_ < KEY_SIZE)
            {
                _loc7_[(_loc5_ + param3) % KEY_SIZE] = (char)param2[(_loc5_)] - (char)'0';
                _loc5_++;
            }
            var _loc9_ = param1.Length + param4;
            var _loc10_ = param1.Substring(0, _loc9_);
            int _loc11_ = _loc9_ / KEY_SIZE;
            var _loc15_ = 0;
            while (_loc15_ < _loc10_.Length)
            {
                _loc8_.Add((char)_loc10_[_loc15_]);
                _loc15_++;
            }
            _loc5_ = 0;
            while (_loc5_ < _loc11_)
            {
                _loc12_ = _loc5_ * KEY_SIZE;
                _loc6_ = 0;
                while (_loc6_ < KEY_SIZE)
                {
                    _loc13_ = _loc12_ + _loc7_[_loc6_];
                    _loc14_ = (char)param1[(_loc12_ + _loc6_)];
                    //  _loc8_.splice(_loc13_,1,_loc14_);
                    _loc8_[_loc13_] = _loc14_;
                    _loc6_++;
                }
                _loc5_++;
            }
            _loc10_ = "";
            _loc5_ = 0;
            while (_loc5_ < _loc8_.Count)
            {
                _loc10_ = _loc10_ + (char)(_loc8_[_loc5_]);
                _loc5_++;
            }
            return _loc10_;
        }
    }
}
